using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingManager : Singleton<CraftingManager>
{
    #region Variables
    [Header("Main Screen")]
    [SerializeField] GameObject craftingMenu;

    [Header("Overview Screen")]
    [SerializeField] ItemCategory_SO itemCategory_SO;

    [SerializeField] GameObject overviewScreen;
    [SerializeField] GameObject overviewGridLayoutGroup;
    [SerializeField] GameObject categoryButton_Prefab;

    public List<ItemCategory> categoryItemsList = new List<ItemCategory>();
    public List<GameObject> categoryButtonPrefabList = new List<GameObject>();

    [Header("Selection Screen")]
    [SerializeField] GameObject selectionScreen;
    [SerializeField] Image categorySelectedImage;
    [SerializeField] TextMeshProUGUI categorySelectedName;

    [SerializeField] GameObject selectionButton_Prefab;
    [SerializeField] GameObject selectionSubGridLayoutGroup_Parent;
    [SerializeField] GameObject selectionSubGridLayoutGroup_Prefab;

    public List<bool> selectionSubActiveList = new List<bool>();
    public List<GameObject> selectionSubGridLayoutGroupList = new List<GameObject>();
    public List<GameObject> selectionButtonPrefabList = new List<GameObject>();

    [Header("Crafting Screen")]
    public GameObject craftingScreen;

    [SerializeField] Image categoryCraftingImage;
    [SerializeField] TextMeshProUGUI categoryCraftingName;
    [SerializeField] TextMeshProUGUI categoryCraftingDescription;

    [SerializeField] GameObject requirementGridLayoutGroup;
    [SerializeField] GameObject requirement_Prefab;
    public List<GameObject> requirementPrefabList = new List<GameObject>();
    public bool totalRequirementMet;

    [Header("Other")]
    public ItemCategories activeCategory;
    public Item itemSelected;
    public GameObject activeItemToCraft;
    #endregion


    //--------------------


    private void Start()
    {
        //PlayerButtonManager.isPressed_CloseCraftingMenu += CloseCraftingScreen;

        craftingMenu.SetActive(false);
        overviewScreen.SetActive(false);
        selectionScreen.SetActive(false);
        craftingScreen.SetActive(false);

        SetupItemCategoryList();

        //Set the first useable Category in the itemCategory_SO.ItemCategoryList to be active by default
        activeCategory = itemCategory_SO.ItemCategoryList[0].categoryName;
        SetupSelectionScreen();
    }
    private void Update()
    {
        if (MainManager.Instance.menuStates == MenuStates.CraftingMenu)
        {
            CheckForRequiermentsMet();
        }
    }


    //--------------------


    //Overview Screen
    #region
    void SetupItemCategoryList()
    {
        //Reset overviewScreen
        categoryButtonPrefabList.Clear();

        //Destroy all Children of overviewGridLayoutGroup to prepare for reset
        while (overviewGridLayoutGroup.transform.childCount > 0)
        {
            overviewGridLayoutGroup.transform.GetChild(0).GetComponent<CategoryButton>().OnDestroyCategoryObject();
        }

        //Reset Panel Size
        overviewScreen.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 100);

        //Instantiate CategoryButtons
        for (int i = 0; i < itemCategory_SO.ItemCategoryList.Count; i++)
        {
            categoryButtonPrefabList.Add(Instantiate(categoryButton_Prefab) as GameObject);
            categoryButtonPrefabList[categoryButtonPrefabList.Count - 1].transform.SetParent(overviewGridLayoutGroup.transform);
            categoryButtonPrefabList[categoryButtonPrefabList.Count - 1].transform.GetChild(0).GetComponent<Image>().sprite = itemCategory_SO.ItemCategoryList[i].categorySprite;

            categoryButtonPrefabList[categoryButtonPrefabList.Count - 1].GetComponent<CategoryButton>().categoryType = itemCategory_SO.ItemCategoryList[i].categoryName;

            //Adjust Frame
            overviewScreen.GetComponent<RectTransform>().sizeDelta += new Vector2(70 + 5, 0);
        }

        overviewScreen.GetComponent<RectTransform>().sizeDelta += new Vector2(5, 0);

        //Change frame on the selected Category
        for (int i = 0; i < categoryButtonPrefabList.Count; i++)
        {
            if (categoryButtonPrefabList[i].GetComponent<CategoryButton>().categoryType == activeCategory)
            {
                //Set Frame Orange
                categoryButtonPrefabList[i].GetComponent<Image>().sprite = TabletManager.Instance.squareButton_Passive;

                break;
            }
        }
    }
    #endregion


    //Selection Screen
    #region
    public void SetupSelectionScreen()
    {
        //CategoryDisplay
        categorySelectedImage.sprite = FindActiveCategoryType().categorySprite;
        categorySelectedName.text = SpaceTextConverting.Instance.SetText(FindActiveCategoryType().categoryName.ToString());

        //Reset Panel Size
        selectionScreen.GetComponent<RectTransform>().sizeDelta = new Vector2(380, 130);

        //Instantitate InstantiateSubGridLayoutGroup
        UpdateSelectionSubActiveList();
        InstantiateSubGridLayoutGroup();
        InstantiateSelectionButton_Prefab();
    }
    void UpdateSelectionSubActiveList()
    {
        selectionSubActiveList.Clear();

        //Build List
        for (int i = 0; i < itemCategory_SO.ItemCategoryList.Count; i++)
        {
            if (itemCategory_SO.ItemCategoryList[i].categoryName == activeCategory)
            {
                for (int j = 0; j < itemCategory_SO.ItemCategoryList[i].subCategoryName.Count; j++)
                {
                    selectionSubActiveList.Add(false);
                }

                break;
            }
        }

        //Turn available subCategories on
        for (int k = 0; k < MainManager.Instance.item_SO.itemList.Count; k++)
        {
            for (int i = 0; i < itemCategory_SO.ItemCategoryList.Count; i++)
            {
                if (itemCategory_SO.ItemCategoryList[i].categoryName == activeCategory)
                {
                    for (int j = 0; j < itemCategory_SO.ItemCategoryList[i].subCategoryName.Count; j++)
                    {
                        if (MainManager.Instance.item_SO.itemList[k].subCategoryName == itemCategory_SO.ItemCategoryList[i].subCategoryName[j])
                        {
                            selectionSubActiveList[j] = true;
                        }
                    }

                    break;
                }
            }
        }
    }
    public void ResetSelectedList()
    {
        selectionSubGridLayoutGroupList.Clear();
        while (selectionSubGridLayoutGroup_Parent.transform.childCount > 0)
        {
            DestroyImmediate(selectionSubGridLayoutGroup_Parent.transform.GetChild(0).gameObject);
        }
    }
    public void InstantiateSubGridLayoutGroup()
    {
        //Prepare for reset
        ResetSelectedList();

        //instantiate selectionSubGridLayoutGroup
        for (int i = 0; i < selectionSubActiveList.Count; i++)
        {
            if (selectionSubActiveList[i])
            {
                selectionSubGridLayoutGroupList.Add(Instantiate(selectionSubGridLayoutGroup_Prefab) as GameObject);
                selectionSubGridLayoutGroupList[selectionSubGridLayoutGroupList.Count - 1].transform.SetParent(selectionSubGridLayoutGroup_Parent.transform);

                //Set Name
                for (int j = 0; j < itemCategory_SO.ItemCategoryList.Count; j++)
                {
                    if (itemCategory_SO.ItemCategoryList[j].categoryName == activeCategory)
                    {
                        for (int k = 0; k < itemCategory_SO.ItemCategoryList[j].subCategoryName.Count; k++)
                        {
                            selectionSubGridLayoutGroupList[selectionSubGridLayoutGroupList.Count - 1].GetComponent<SelectionSubPanel>().panelName = itemCategory_SO.ItemCategoryList[j].subCategoryName[i];
                            selectionSubGridLayoutGroupList[selectionSubGridLayoutGroupList.Count - 1].GetComponent<SelectionSubPanel>().SetDisplay();

                            break;
                        }
                    }
                }

                //Adjust Frame
                selectionScreen.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 60);
            }
        }
    }
    void InstantiateSelectionButton_Prefab()
    {
        //for (int i = selectionButtonPrefabList.Count - 1; i >= 0; i--)
        //{
        //    selectionButtonPrefabList[i].transform.GetChild(i).GetComponent<SelectionSubButtonPrefab>().OnDestroySubCategoryObject();
        //}
        selectionButtonPrefabList.Clear();

        int ItemCategoryListIndex = 0;

        for (int i = 0; i < itemCategory_SO.ItemCategoryList.Count; i++)
        {
            if (itemCategory_SO.ItemCategoryList[i].categoryName == activeCategory)
            {
                ItemCategoryListIndex = i;

                break;
            }
        }

        //Instantiate SelectionButton_Prefab
        for (int i = 0; i < selectionSubGridLayoutGroupList.Count; i++)
        {
            //Find amount of items
            for (int j = 0; j < MainManager.Instance.item_SO.itemList.Count; j++)
            {
                bool isReady = true;

                if (/*MainManager.Instance.item_SO.itemList[j].isResearched
                    &&*/ MainManager.Instance.item_SO.itemList[j].isCrafteable
                    && MainManager.Instance.item_SO.itemList[j].craftingRequirements.Count > 0
                    && MainManager.Instance.item_SO.itemList[j].categoryName == activeCategory
                    && MainManager.Instance.item_SO.itemList[j].subCategoryName == selectionSubGridLayoutGroupList[i].GetComponent<SelectionSubPanel>().panelName)
                {
                    //Check if all Required items have been researched
                    for (int k = 0; k < MainManager.Instance.item_SO.itemList[j].craftingRequirements.Count; k++)
                    {
                        if (!MainManager.Instance.GetItem(MainManager.Instance.item_SO.itemList[j].craftingRequirements[k].itemName).isResearched)
                        {
                            isReady = false;

                            //j = MainManager.Instance.item_SO.itemList.Count;

                            break;
                        }
                    }

                    if (isReady)
                    {
                        selectionButtonPrefabList.Add(Instantiate(selectionButton_Prefab) as GameObject);
                        selectionButtonPrefabList[selectionButtonPrefabList.Count - 1].transform.SetParent(selectionSubGridLayoutGroupList[i].transform);

                        selectionButtonPrefabList[selectionButtonPrefabList.Count - 1].GetComponent<SelectionSubButtonPrefab>().item = MainManager.Instance.item_SO.itemList[j];
                        selectionButtonPrefabList[selectionButtonPrefabList.Count - 1].GetComponent<SelectionSubButtonPrefab>().SetDisplay();
                    }
                }
            }
        }

        //Adjust panel size if there are more than 6 items of a subCategory
        for (int i = 0; i < selectionSubGridLayoutGroupList.Count; i++)
        {
            int count = 0;

            for (int j = 0; j < selectionButtonPrefabList.Count; j++)
            {
                if (selectionButtonPrefabList[j].GetComponent<SelectionSubButtonPrefab>().item.subCategoryName == selectionSubGridLayoutGroupList[i].GetComponent<SelectionSubPanel>().panelName)
                {
                    count++;
                }
            }

            if (count < 6)
            {

            }
            else if (count < 12)
            {
                selectionScreen.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 60);
            }
            else if (count < 18)
            {
                selectionScreen.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 60);
            }
            else if (count < 24)
            {
                selectionScreen.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 60);
            }
        }
    }
    #endregion


    //Crafting Screen
    #region
    public void SetupCraftingScreen(Item item)
    {
        //Prepare for reset
        requirementPrefabList.Clear();
        while (requirementGridLayoutGroup.transform.childCount > 0)
        {
            DestroyImmediate(requirementGridLayoutGroup.transform.GetChild(0).gameObject);
        }

        //Reset Panel Size
        craftingScreen.GetComponent<RectTransform>().sizeDelta = new Vector2(270, 240);

        categoryCraftingImage.sprite = item.hotbarSprite;
        categoryCraftingName.text = SpaceTextConverting.Instance.SetText(item.itemName.ToString());
        categoryCraftingDescription.text = item.itemDescription;

        InstantiateCraftingRequirementPrefabs(item);
    }
    public void InstantiateCraftingRequirementPrefabs(Item item)
    {
        for (int i = 0; i < item.craftingRequirements.Count; i++)
        {
            requirementPrefabList.Add(Instantiate(requirement_Prefab) as GameObject);
            requirementPrefabList[requirementPrefabList.Count - 1].transform.SetParent(requirementGridLayoutGroup.transform);
            
            requirementPrefabList[requirementPrefabList.Count - 1].GetComponent<CraftingRequirementPrefab>().requirements = item.craftingRequirements[i];

            for (int j = 0; j < MainManager.Instance.item_SO.itemList.Count; j++)
            {
                if (MainManager.Instance.item_SO.itemList[j].itemName == item.craftingRequirements[i].itemName)
                {
                    requirementPrefabList[requirementPrefabList.Count - 1].GetComponent<CraftingRequirementPrefab>().craftingItemSprite = MainManager.Instance.GetItem(MainManager.Instance.item_SO.itemList[j].itemName).hotbarSprite;

                    break;
                }
            }

            requirementPrefabList[requirementPrefabList.Count - 1].GetComponent<CraftingRequirementPrefab>().CheckRequrement();
            requirementPrefabList[requirementPrefabList.Count - 1].GetComponent<CraftingRequirementPrefab>().SetDisplay();

            //Adjust Frame
            craftingScreen.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 65);
        }

        craftingScreen.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 12);
    }
    private void CheckForRequiermentsMet()
    {
        int count = 0;

        for (int i = 0; i < requirementPrefabList.Count; i++)
        {
            if (requirementPrefabList[i].GetComponent<CraftingRequirementPrefab>().requirementIsMet)
            {
                count++;
            }
        }

        if (count >= requirementGridLayoutGroup.transform.childCount)
        {
            totalRequirementMet = true;
        }
        else
        {
            totalRequirementMet = false;
        }
    }
    #endregion


    //--------------------


    ItemCategory FindActiveCategoryType()
    {
        for (int i = 0; i < itemCategory_SO.ItemCategoryList.Count; i++)
        {
            if (activeCategory == itemCategory_SO.ItemCategoryList[i].categoryName)
            {
                return itemCategory_SO.ItemCategoryList[i];
            }
        }
        return null;
    }


    //--------------------


    void ActivateItemInScripteableObject(Items itemName)
    {
        MainManager.Instance.GetItem(itemName).isResearched = true;
    }
    bool IsItemActivatedInScripteableObject(Items itemName)
    {
        if (MainManager.Instance.GetItem(itemName).isResearched)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    //--------------------


    public void OpenCraftingScreen()
    {
        SetupItemCategoryList();
        UpdateSelectionSubActiveList();

        Cursor.lockState = CursorLockMode.None;
        MainManager.Instance.menuStates = MenuStates.CraftingMenu;

        craftingMenu.SetActive(true);
        overviewScreen.SetActive(true);
        selectionScreen.SetActive(true);

        //Reset Frame Rotation
        for (int i = 0; i < selectionSubGridLayoutGroupList.Count; i++)
        {
            selectionSubGridLayoutGroupList[i].GetComponent<RectTransform>().rotation = Quaternion.identity;
        }

        //Set the Frame of the Active Craftable item to orange 
        if(activeItemToCraft != null)
        {
            activeItemToCraft.GetComponent<Image>().sprite = TabletManager.Instance.squareButton_Passive;
        }
    }
    public void CloseCraftingScreen()
    {
        craftingMenu.SetActive(false);
        overviewScreen.SetActive(false);
        selectionScreen.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        MainManager.Instance.menuStates = MenuStates.None;
    }
}
