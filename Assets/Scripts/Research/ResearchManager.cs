using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResearchManager : Singleton<ResearchManager>
{
    [Header("Research Panel")]
    [SerializeField] GameObject researchedItemList_Parent;
    [SerializeField] GameObject researchedItem_Prefab;

    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] Image itemImage;
    [SerializeField] TextMeshProUGUI itemDescription;

    [SerializeField] GameObject researchButton;
    [SerializeField] GameObject researchProgressBar_Parent;
    [SerializeField] GameObject researchProgressBar;

    //Active Item
    [SerializeField] Items activeItem;
    public bool isResearching;
    public float researchTime_Multiplier = 2;
    float researchTime_Max;
    float researchTime_Current;

    //Lists
    List<GameObject> researchedItemsList = new List<GameObject>();
    List<Items> researchedItemsListNames = new List<Items>();
    List<bool> researched_SOItem = new List<bool>();

    //Markers
    List<int> hotbarMarkerInt = new List<int>();
    List<int> durabilityMarkerInt = new List<int>();

    [Header("Colors")]
    [SerializeField] Color researchedColor;
    [SerializeField] Color notResearchedColor;
    [SerializeField] Color visibleColor;
    [SerializeField] Color unvisibleColor;


    //--------------------


    private void Start()
    {
        //for (int i = 0; i < MainManager.Instance.item_SO.itemList.Count; i++)
        //{
        //    MainManager.Instance.item_SO.itemList[i].researchTime = 5;
        //}
    }
    private void Update()
    {
        //Run when something is researched
        if (isResearching)
        {
            researchTime_Current += Time.deltaTime;

            researchProgressBar.GetComponent<Image>().fillAmount = (researchTime_Current / (researchTime_Max * researchTime_Multiplier));

            if (researchTime_Current >= (researchTime_Max * researchTime_Multiplier))
            {
                CompleteResearch();

                isResearching = false;
            }
        }
    }


    //--------------------


    public void LoadData(List<bool> itemBoolList)
    {
        //Load researchedItemsList
        researchedItemsListNames = DataManager.Instance.researchedItemsListNames_Store;
        SetResearchedItemList();

        //Load _SOItems
        researched_SOItem = itemBoolList;

        if (researched_SOItem.Count <= 0) //New Game
        {
            for (int i = 0; i < MainManager.Instance.item_SO.itemList.Count; i++)
            {
                researched_SOItem.Add(false);
            }
        }

        Update_SOItemList();

        SaveData();
    }
    public void SaveData()
    {
        DataManager.Instance.researchedItemsListNames_Store = researchedItemsListNames;
        DataManager.Instance.researched_SOItem_Store = researched_SOItem;
    }


    //--------------------


    public void UpdateResearchMarkers()
    {
        hotbarMarkerInt.Clear();
        durabilityMarkerInt.Clear();

        for (int i = 0; i < InventoryManager.Instance.itemSlotList_Player.Count; i++)
        {
            if (InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>())
            {
                //Hide Hotbar Marker
                if (InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().hotbarSelectorParent.activeInHierarchy)
                {
                    hotbarMarkerInt.Add(i);

                    InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().hotbarSelectorParent.SetActive(false);
                }

                //Hide Durability Marker
                if (InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().durabilityMeterParent.activeInHierarchy)
                {
                    durabilityMarkerInt.Add(i);

                    InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().durabilityMeterImage.gameObject.SetActive(false);
                    InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().durabilityMeterParent.SetActive(false);
                }
            }
        }

        //print("hotbarMarkerInt: " + hotbarMarkerInt.Count + " | durabilityMarkerInt: " + durabilityMarkerInt.Count);
    }
    public void UpdateResearchItemColor()
    {
        for (int i = 0; i < InventoryManager.Instance.itemSlotList_Player.Count; i++)
        {
            if (InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>())
            {
                Item tempItem = MainManager.Instance.GetItem(InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().itemName);

                //Set Image Color
                if (InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().gameObject.GetComponent<Image>())
                {
                    if (InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().itemName != Items.None)
                    {
                        if (tempItem.isResearched)
                        {
                            InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().ChangeImageColor(researchedColor);
                        }
                        else
                        {
                            InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().ChangeImageColor(notResearchedColor);
                        }
                    }
                }
            }
        }
    }
    
    public void ResetResearchMarkers()
    {
        for (int i = 0; i < InventoryManager.Instance.itemSlotList_Player.Count; i++)
        {
            for (int j = 0; j < hotbarMarkerInt.Count; j++)
            {
                if (hotbarMarkerInt[j] == i)
                {
                    if (InventoryManager.Instance.itemSlotList_Player[i].GetComponent<Image>() && InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>())
                    {
                        //Show Hotbar Marker
                        if (!InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().hotbarSelectorParent.activeInHierarchy)
                        {
                            InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().hotbarSelectorParent.SetActive(true);
                        }
                    }

                    break;
                }
            }

            for (int j = 0; j < durabilityMarkerInt.Count; j++)
            {
                if (durabilityMarkerInt[j] == i)
                {
                    if (InventoryManager.Instance.itemSlotList_Player[i].GetComponent<Image>() && InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>())
                    {
                        //Show Durability Marker
                        if (!InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().durabilityMeterParent.activeInHierarchy)
                        {
                            InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().durabilityMeterImage.gameObject.SetActive(true);
                            InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().durabilityMeterParent.SetActive(true);
                        }
                    }

                    break;
                }
            }
        }
    }
    public void ResetResearchItemColor()
    {
        for (int i = 0; i < InventoryManager.Instance.itemSlotList_Player.Count; i++)
        {
            //Set Image Color
            if (InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().gameObject.GetComponent<Image>())
            {
                if (InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().itemName == Items.None)
                {
                    InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().ChangeImageColor(unvisibleColor /*new Color(255, 255, 255, 0)*/);
                }
                else
                {
                    InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().ChangeImageColor(visibleColor /*new Color(255, 255, 255, 255)*/);
                }
            }
        }
    }


    //--------------------


    public void SetResearchedItemList()
    {
        researchedItemList_Parent.GetComponent<RectTransform>().sizeDelta = new Vector2(225, 0);

        for (int i = 0; i < researchedItemsListNames.Count; i++)
        {
            researchedItemsList.Insert(0, Instantiate(researchedItem_Prefab, researchedItemList_Parent.transform));
            researchedItemsList[0].GetComponent<ResearchItemSlot>().SetItemInfo(MainManager.Instance.GetItem(researchedItemsListNames[i]).hotbarSprite, researchedItemsListNames[i]);
            researchedItemsList[0].transform.SetAsFirstSibling();

            researchedItemList_Parent.GetComponent<RectTransform>().sizeDelta += new Vector2 (0, 55);
        }

        SaveData();
    }
    public void AddToResearchedItemList(Items name)
    {
        //Add to NamesList
        researchedItemsListNames.Add(name);

        //Add to GameObjectList
        researchedItemsList.Insert(0, Instantiate(researchedItem_Prefab, researchedItemList_Parent.transform));
        researchedItemsList[0].GetComponent<ResearchItemSlot>().SetItemInfo(MainManager.Instance.GetItem(name).hotbarSprite, name);
        researchedItemsList[0].transform.SetAsFirstSibling();

        researchedItemList_Parent.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 55);

        SaveData();
    }
    public void Update_SOItemList()
    {
        for (int i = 0; i < MainManager.Instance.item_SO.itemList.Count; i++)
        {
            MainManager.Instance.item_SO.itemList[i].isResearched = researched_SOItem[i];
        }
    }


    //--------------------


    public void SetResearchItemInfo(Items _itemName)
    {
        if (_itemName == Items.None)
        {
            ResetResearchInfo();
        }

        else if (!MainManager.Instance.GetItem(_itemName).isResearched)
        {
            activeItem = _itemName;

            itemName.text = _itemName.ToString();
            itemImage.sprite = MainManager.Instance.GetItem(_itemName).hotbarSprite;
            itemDescription.text = MainManager.Instance.GetItem(_itemName).research_ItemDescription;

            researchButton.SetActive(true);

            researchProgressBar.GetComponent<Image>().fillAmount = 0;
            researchProgressBar.SetActive(true);
            researchProgressBar_Parent.SetActive(true);
        }

        else
        {
            ResetResearchInfo();
        }
    }
    void ResetResearchInfo()
    {
        activeItem = Items.None;

        itemName.text = "";
        itemImage.sprite = MainManager.Instance.GetItem(0).hotbarSprite;
        itemDescription.text = "";

        isResearching = false;
        researchTime_Current = 0;
        researchTime_Max = 0;

        researchButton.SetActive(false);
        researchProgressBar.SetActive(false);
        researchProgressBar_Parent.SetActive(false);
    }
    
    public void ResearchButton_isPressed()
    {
        SoundManager.Instance.Play_Research_Ongoing_Clip();

        researchTime_Max = MainManager.Instance.GetItem(activeItem).researchTime;
        researchTime_Current = 0;

        isResearching = true;
    }
    void CompleteResearch()
    {
        print("1. CompleteResearch");
        SoundManager.Instance.Play_Research_Complete_Clip();

        //Change isResearched stats
        for (int i = 0; i < MainManager.Instance.item_SO.itemList.Count; i++)
        {
            if (MainManager.Instance.item_SO.itemList[i].itemName == activeItem)
            {
                MainManager.Instance.item_SO.itemList[i].isResearched = true;
                researched_SOItem[i] = true;

                AddToResearchedItemList(activeItem);
                Update_SOItemList();
                UpdateResearchItemColor();

                break;
            }
        }

        //Update the itemStateList with the new stats
        for (int j = 0; j < MainManager.Instance.item_SO.itemList.Count; j++)
        {
            print("2. CompleteResearch");

            bool ready = true;

            for (int k = 0; k < MainManager.Instance.item_SO.itemList[j].craftingRequirements.Count; k++)
            {
                if (!MainManager.Instance.GetItem(MainManager.Instance.item_SO.itemList[j].craftingRequirements[k].itemName).isResearched)
                {
                    print("2.5 CompleteResearch");

                    ready = false;

                    break;
                }
            }

            if (ready)
            {
                print("3. CompleteResearch");

                for (int k = 0; k < MainManager.Instance.item_SO.itemList[j].craftingRequirements.Count; k++)
                {
                    if (MainManager.Instance.item_SO.itemList[j].craftingRequirements[k].itemName == activeItem)
                    {
                        print("4. CompleteResearch");

                        //CraftingManager.Instance.itemStateList[j].itemState = CraftingItemState.New;

                        CraftingManager.Instance.UpdateItemState(j, true);

                        break;
                    }
                }
            }
        }

        SetResearchItemInfo(Items.None);
        SaveData();
    }
}
