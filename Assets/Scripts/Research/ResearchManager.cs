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


    private void Update()
    {
        //Run when something is researched
        if (isResearching && DataManager.Instance.hasLoaded)
        {
            float temptime = researchTime_Max * (1 - (PerkManager.Instance.perkValues.researchTime_Decrease_Percentage / 100));

            print("ResearchTime: " + temptime);

            researchTime_Current += Time.deltaTime;

            researchProgressBar.GetComponent<Image>().fillAmount = researchTime_Current / temptime;

            if (researchTime_Current >= temptime)
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

            print("Researched");

            //Set Items to be Researched
            //Nothing
            researched_SOItem[0] = true;

            //Tools
            researched_SOItem[14] = true;
            researched_SOItem[15] = true;
            researched_SOItem[16] = true;
            researched_SOItem[17] = true;
            researched_SOItem[18] = true;
            researched_SOItem[19] = true;
            researched_SOItem[20] = true;
            researched_SOItem[21] = true;
            researched_SOItem[22] = true;
            researched_SOItem[23] = true;
            researched_SOItem[24] = true;
            researched_SOItem[25] = true;

            //MenuEquipments
            researched_SOItem[27] = true;
            researched_SOItem[28] = true;
            researched_SOItem[29] = true;
            researched_SOItem[30] = true;
            researched_SOItem[31] = true;
            researched_SOItem[32] = true;
            researched_SOItem[33] = true;
            researched_SOItem[34] = true;
            researched_SOItem[35] = true;

            //Juice
            researched_SOItem[63] = true;
            researched_SOItem[64] = true;
            researched_SOItem[65] = true;
            researched_SOItem[66] = true;
            researched_SOItem[67] = true;
            researched_SOItem[68] = true;

            //Grilled
            researched_SOItem[69] = true;
            researched_SOItem[70] = true;
            researched_SOItem[71] = true;
            researched_SOItem[72] = true;
            researched_SOItem[73] = true;
            researched_SOItem[74] = true;

            //Flashlight
            researched_SOItem[75] = true;

            //Drinkable
            researched_SOItem[76] = true;
            researched_SOItem[77] = true;
            researched_SOItem[78] = true;

            //Ghost Capturer
            researched_SOItem[79] = true;

            //Arídean Key
            researched_SOItem[80] = true;

            //More Seeds
            researched_SOItem[81] = true;
            researched_SOItem[82] = true;
            researched_SOItem[83] = true;
        }

        Update_SOItemList();

        CheckIfNoItemsAreResearched();

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


    #region The Research
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

        CheckIfNoItemsAreResearched();

        SaveData();
    }
    public void Update_SOItemList()
    {
        for (int i = 0; i < MainManager.Instance.item_SO.itemList.Count; i++)
        {
            MainManager.Instance.item_SO.itemList[i].isResearched = researched_SOItem[i];
        }
    }
    void CheckIfNoItemsAreResearched()
    {
        bool check = false;

        //for (int i = 0; i < researchedItemsList.Count; i++)
        //{
        //    if (researchedItemsList.Count <= 0)
        //    {
        //        check = true;
        //    }
        //}

        if (researchedItemsList.Count <= 0)
        {
            check = true;
        }

        if (check)
        {
            CraftingManager.Instance.noItemResearched_Text.SetActive(true);
        }
        else
        {
            CraftingManager.Instance.noItemResearched_Text.SetActive(false);
        }
    }
    #endregion


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

        isResearching = false;
        researchTime_Current = 0;
        researchTime_Max = 0;

        researchButton.SetActive(false);
        researchProgressBar.SetActive(false);
        researchProgressBar_Parent.SetActive(false);
    }
    
    public void ResearchButton_isPressed()
    {
        SoundManager.Instance.Play_ResearchTable_Researching_Clip();

        researchTime_Max = MainManager.Instance.GetItem(activeItem).researchTime;
        researchTime_Current = 0;

        isResearching = true;
    }
    void CompleteResearch()
    {
        SoundManager.Instance.Play_Research_Complete_Clip();
        SoundManager.Instance.Stop_ResearchTable_Researching_Clip();

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
            bool ready = true;

            for (int k = 0; k < MainManager.Instance.item_SO.itemList[j].craftingRequirements.Count; k++)
            {
                if (!MainManager.Instance.GetItem(MainManager.Instance.item_SO.itemList[j].craftingRequirements[k].itemName).isResearched)
                {
                    ready = false;

                    break;
                }
            }

            if (ready)
            {
                for (int k = 0; k < MainManager.Instance.item_SO.itemList[j].craftingRequirements.Count; k++)
                {
                    if (MainManager.Instance.item_SO.itemList[j].craftingRequirements[k].itemName == activeItem)
                    {
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
