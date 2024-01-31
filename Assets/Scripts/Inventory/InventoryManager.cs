using System;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using static UnityEditor.Progress;

public class InventoryManager : Singleton<InventoryManager>
{
    [Header("Inventory")]
    public Vector2 inventorySize;
    public int cellsize = 70;

    [Header("Item")]
    public Items lastItemToGet;
    [SerializeField] TextMeshProUGUI player_ItemName_Display;
    [SerializeField] TextMeshProUGUI player_ItemDescription_Display;
    [SerializeField] TextMeshProUGUI chest_ItemName_Display;
    [SerializeField] TextMeshProUGUI chest_ItemDescription_Display;

    [Header("GameObjects")]
    public GameObject itemSlot_Prefab;

    public GameObject handDropPoint;
    public GameObject worldObject_Parent;

    [Header("Lists")]
    public List<Inventory> inventories = new List<Inventory>();

    public List<GameObject> itemSlotList_Player = new List<GameObject>();
    public List<GameObject> itemSlotList_Chest = new List<GameObject>();

    public GameObject playerInventory_Fake_Parent;
    public GameObject chestInventory_Fake_Parent;
    [SerializeField] List<GameObject> player_FakeSlot_List = new List<GameObject>();
    [SerializeField] List<GameObject> chest_FakeSlot_List = new List<GameObject>();

    bool inventoryIsOpen;

    public int chestInventoryOpen;
    GameObject itemTemp;



    //--------------------


    private void Start()
    {
        //PlayerButtonManager.OpenPlayerInventory_isPressedDown += OpenPlayerInventory;
        //PlayerButtonManager.ClosePlayerInventory_isPressedDown += ClosePlayerInventory;

        TabletManager.Instance.playerInventory_Parent.SetActive(false);
        TabletManager.Instance.chestInventory_Parent.SetActive(false);
        playerInventory_Fake_Parent.SetActive(false);
        chestInventory_Fake_Parent.SetActive(false);
    }


    //--------------------


    public void LoadData()
    {
        #region Inventory
        inventories = DataManager.Instance.Inventories_StoreList;

        //Safty Inventory check if starting a new game - Always have at least 1 inventory
        if (inventories.Count <= 0)
        {
            print("AddInventory");
            AddInventory(new Vector2(5, 7));

            SaveData();
        }
        #endregion

        #region Player Position
        //Set Player position - The "LoadData()" doesen't activate in the relevant playerMovement script
        MainManager.Instance.player.transform.SetPositionAndRotation(DataManager.Instance.playerPos_Store, DataManager.Instance.playerRot_Store);
        #endregion
    }
    public void SaveData()
    {
        DataManager.Instance.Inventories_StoreList = inventories;
    }
    public void SaveData(ref GameData gameData)
    {
        DataManager.Instance.Inventories_StoreList = inventories;

        print("Save_Inventories");
    }


    //--------------------


    public void AddInventory(Vector2 size)
    {
        //Add empty inventory
        Inventory inventory = new Inventory();

        inventories.Add(inventory);

        //Set inventory stats
        inventories[inventories.Count - 1].inventoryIndex = inventories.Count - 1;
        inventories[inventories.Count - 1].inventorySize = size;

        SaveData();
    }
    public void RemoveInventory(int index)
    {
        inventories.RemoveAt(index);

        SaveData();
    }
    public void SetInventorySize(int inventory, Vector2 size)
    {
        inventories[inventory].inventorySize = size;

        RemoveInventoriesUI();
    }


    //--------------------


    public bool AddItemToInventory(int inventory, GameObject obj, bool itemIsMoved)
    {
        InventoryItem item = new InventoryItem();

        //If item is being moved to another inventory
        if (itemIsMoved)
        {
            item.inventoryIndex = inventory;
            item.itemName = obj.GetComponent<ItemSlot>().itemName;
            item.itemSize = MainManager.Instance.GetItem(obj.GetComponent<ItemSlot>().itemName).itemSize;
            item.itemID = obj.GetComponent<ItemSlot>().itemID;

            lastItemToGet = obj.GetComponent<ItemSlot>().itemName;
            itemTemp = obj;
        }

        //If item is being picked up
        else
        {
            item.inventoryIndex = inventory;
            item.itemName = obj.GetComponent<InteractableObject>().itemName;
            item.itemSize = MainManager.Instance.GetItem(obj.GetComponent<InteractableObject>().itemName).itemSize;

            lastItemToGet = obj.GetComponent<InteractableObject>().itemName;

            //Give the item an ID to be unique
            #region
            bool check = false;
            while (!check)
            {
                bool innerCheck = false;
                item.itemID = UnityEngine.Random.Range(0, 10000000);

                for (int i = 0; i < inventories[inventory].itemsInInventory.Count; i++)
                {
                    if (inventories[inventory].itemsInInventory[i].itemID == item.itemID)
                    {
                        innerCheck = true;

                        break;
                    }
                }

                if (!innerCheck)
                {
                    check = true;
                }
            }
            #endregion
        }

        inventories[inventory].itemsInInventory.Add(item);

        RemoveInventoriesUI();
        PrepareInventoryUI(inventory, itemIsMoved);

        SetBuildingRequirement();

        return true;
    }
    public bool AddItemToInventory(int inventory, Items itemName)
    {
        InventoryItem item = new InventoryItem();

        item.inventoryIndex = inventory;
        item.itemName = itemName;
        item.itemSize = MainManager.Instance.GetItem(itemName).itemSize;

        lastItemToGet = itemName;

        //Give the item an ID to be unique
        #region
        bool check = false;
        while (!check)
        {
            bool innerCheck = false;
            item.itemID = UnityEngine.Random.Range(0, 10000000);

            for (int i = 0; i < inventories[inventory].itemsInInventory.Count; i++)
            {
                if (inventories[inventory].itemsInInventory[i].itemID == item.itemID)
                {
                    innerCheck = true;

                    break;
                }
            }

            if (!innerCheck)
            {
                check = true;
            }
        }
        #endregion

        inventories[inventory].itemsInInventory.Add(item);

        RemoveInventoriesUI();
        PrepareInventoryUI(inventory, false);

        SetBuildingRequirement();

        return true;
    }
    public void RemoveItemFromInventory(int inventory, Items itemName, int ID)
    {
        //From inventory to World
        for (int i = 0; i < inventories[inventory].itemsInInventory.Count; i++)
        {
            if (inventories[inventory].itemsInInventory[i].itemName == itemName
                && inventories[inventory].itemsInInventory[i].itemID == ID)
            {
                inventories[inventory].itemsInInventory.RemoveAt(i);

                break;
            }
        }

        RemoveInventoriesUI();
        PrepareInventoryUI(inventory, false);

        //Spawn item into the World
        WorldObjectManager.Instance.worldObjectList.Add(Instantiate(MainManager.Instance.GetItem(itemName).worldObjectPrefab, handDropPoint.transform.position, Quaternion.identity) as GameObject);
        WorldObjectManager.Instance.worldObjectList[WorldObjectManager.Instance.worldObjectList.Count - 1].transform.parent = worldObject_Parent.transform;

        //Set Gravity true on the worldObject
        WorldObjectManager.Instance.worldObjectList[WorldObjectManager.Instance.worldObjectList.Count - 1].GetComponent<Rigidbody>().isKinematic = false;
        WorldObjectManager.Instance.worldObjectList[WorldObjectManager.Instance.worldObjectList.Count - 1].GetComponent<Rigidbody>().useGravity = true;

        //Update item in the World
        WorldObjectManager.Instance.WorldObject_SaveState_AddObjectToWorld(itemName, WorldObjectManager.Instance.worldObjectList[WorldObjectManager.Instance.worldObjectList.Count - 1]);

        //If item is removed from the inventory, update the Hotbar
        if (inventory <= 0)
        {
            CheckHotbarItemInInventory(ID);
        }

        SetBuildingRequirement();

        SaveData();
    }
    public void RemoveItemFromInventory(int inventory, Items itemName, int ID, bool itemIsMoved)
    {
        //From inventory to another Inventory, or crafting

        //if item has itemID = ID, find it and remove it
        #region
        for (int i = 0; i < inventories[inventory].itemsInInventory.Count; i++)
        {
            if (inventories[inventory].itemsInInventory[i].itemName == itemName
                && inventories[inventory].itemsInInventory[i].itemID == ID)
            {
                RemoveItemFromHotbarBeforeBeingRemoved(inventories[inventory].itemsInInventory[i], false);

                inventories[inventory].itemsInInventory.RemoveAt(i);

                RemoveInventoriesUI();
                PrepareInventoryUI(inventory, true);

                SetBuildingRequirement();

                return;
            }
        }
        #endregion


        //--------------------


        //If itemID <= -1 - Remove THIS if its item type isn't on Hotbar
        #region
        if (ID <= -1)
        {
            int counter = 0;

            //check if there are any of this item on the Hotbar. If not, remove it
            for (int i = 0; i < inventories[inventory].itemsInInventory.Count; i++)
            {
                if (inventories[inventory].itemsInInventory[i].itemName == itemName)
                {
                    for (int j = 0; j < HotbarManager.Instance.hotbarList.Count; j++)
                    {
                        if (inventories[inventory].itemsInInventory[i].itemName == HotbarManager.Instance.hotbarList[j].itemName)
                        {
                            counter = 100;

                            j = HotbarManager.Instance.hotbarList.Count;
                            i = inventories[inventory].itemsInInventory.Count;
                        }
                    }
                }
            }

            if (counter <= 0)
            {
                for (int i = 0; i < inventories[inventory].itemsInInventory.Count; i++)
                {
                    if (inventories[inventory].itemsInInventory[i].itemName == itemName)
                    {
                        //RemoveItemFromHotbarBeforeBeingRemoved(inventories[inventory].itemsInInventory[i]);

                        inventories[inventory].itemsInInventory.RemoveAt(i);

                        RemoveInventoriesUI();
                        PrepareInventoryUI(inventory, true);

                        SetBuildingRequirement();

                        return;
                    }
                }
            }
        }
        #endregion

        //If itemID <= -1 - Remove if THIS spesific item isn't on Hotbar
        #region
        if (ID <= -1)
        {
            //check if this item isn't on the Hotbar. If not, remove it
            for (int i = 0; i < inventories[inventory].itemsInInventory.Count; i++)
            {
                if (inventories[inventory].itemsInInventory[i].itemName == itemName)
                {
                    int counter = 0;
                    int hotbarCounter = 0;

                    for (int j = 0; j < HotbarManager.Instance.hotbarList.Count; j++)
                    {
                        //Count number of item represented on the Hotbar
                        if (HotbarManager.Instance.hotbarList[j].itemName == inventories[inventory].itemsInInventory[i].itemName)
                        {
                            hotbarCounter++;
                        }

                        //Count if item isn't on the Hotbar
                        if (inventories[inventory].itemsInInventory[i].itemName == HotbarManager.Instance.hotbarList[j].itemName
                            && inventories[inventory].itemsInInventory[i].itemID != HotbarManager.Instance.hotbarList[j].itemID)
                        {
                            counter++;
                        }
                    }

                    if (counter >= hotbarCounter)
                    {
                        inventories[inventory].itemsInInventory.RemoveAt(i);

                        RemoveInventoriesUI();
                        PrepareInventoryUI(inventory, true);

                        SetBuildingRequirement();

                        return;
                    }
                }
            }
        }
        #endregion

        //If itemID <= -1 - Remove the last item from the Hotbar
        #region
        if (ID <= -1)
        {
            //Remove the last item on the Hotbar of this type
            for (int i = 0; i < inventories[inventory].itemsInInventory.Count; i++)
            {
                if (inventories[inventory].itemsInInventory[i].itemName == itemName)
                {
                    for (int j = 0; j < HotbarManager.Instance.hotbarList.Count; j++)
                    {
                        if (inventories[inventory].itemsInInventory[i].itemName == HotbarManager.Instance.hotbarList[j].itemName)
                        {
                            RemoveItemFromHotbarBeforeBeingRemoved(inventories[inventory].itemsInInventory[i], true);

                            inventories[inventory].itemsInInventory.RemoveAt(i);

                            RemoveInventoriesUI();
                            PrepareInventoryUI(inventory, true);

                            SetBuildingRequirement();

                            return;
                        }
                    }
                }
            }
        }
        #endregion
    }

    public void MoveItemToInventory(int inventory, GameObject obj, int ID)
    {
        //Move item to Player Inventory
        if (inventory <= 0)
        {
            RemoveItemFromInventory(0, obj.GetComponent<ItemSlot>().itemName, ID, true);
            AddItemToInventory(chestInventoryOpen, obj, true);
        }

        //Move item to Chest Inventory
        else
        {
            RemoveItemFromInventory(chestInventoryOpen, obj.GetComponent<ItemSlot>().itemName, ID, true);
            AddItemToInventory(0, obj, true);
        }

        RemoveInventoriesUI();
        PrepareInventoryUI(0, true);
        PrepareInventoryUI(chestInventoryOpen, true);

        CheckHotbarItemInInventory(ID);

        //Update the Hand to see if slot is empty
        HotbarManager.Instance.ChangeItemInHand();
    }

    public void CheckHotbarItemInInventory(int ID)
    {
        print("1. ID: " + ID);
        for (int i = 0; i < HotbarManager.Instance.hotbarList.Count; i++)
        {
            bool isInInventory = false;

            //Check if Hotbar item is in the player inventory
            for (int j = 0; j < inventories[0].itemsInInventory.Count; j++)
            {
                if (HotbarManager.Instance.hotbarList[i].itemName != Items.None
                    && HotbarManager.Instance.hotbarList[i].itemName == inventories[0].itemsInInventory[j].itemName
                    && HotbarManager.Instance.hotbarList[i].itemID == inventories[0].itemsInInventory[j].itemID)
                {
                    isInInventory = true;

                    break;
                }
            }

            //If HotbarItem isn't in the inventory, remove it from the Hotbar
            if (!isInInventory)
            {
                HotbarManager.Instance.selectedItem = Items.None;

                HotbarManager.Instance.hotbarList[i].itemName = Items.None;
                HotbarManager.Instance.hotbarList[i].itemID = -1;
                HotbarManager.Instance.hotbarList[i].hotbar.GetComponent<HotbarSlot>().RemoveHotbarSlotImage();
                HotbarManager.Instance.hotbarList[i].hotbar.GetComponent<HotbarSlot>().ResetHotbarItem();

                //Update the Hand to see if slot is empty
                HotbarManager.Instance.ChangeItemInHand();
            }
        }

        HotbarManager.Instance.SaveData();
    }
    public void RemoveItemFromHotbarBeforeBeingRemoved(InventoryItem item, bool reverse)
    {
        if (reverse)
        {
            for (int i = HotbarManager.Instance.hotbarList.Count - 1; i >= 0; i--)
            {
                //Check if HotbarItem is in the player inventory
                if (HotbarManager.Instance.hotbarList[i].itemName != Items.None
                    && HotbarManager.Instance.hotbarList[i].itemName == item.itemName
                    && HotbarManager.Instance.hotbarList[i].itemID == item.itemID)
                {
                    HotbarManager.Instance.selectedItem = Items.None;

                    HotbarManager.Instance.hotbarList[i].itemName = Items.None;
                    HotbarManager.Instance.hotbarList[i].itemID = -1;
                    HotbarManager.Instance.hotbarList[i].hotbar.GetComponent<HotbarSlot>().RemoveHotbarSlotImage();
                    HotbarManager.Instance.hotbarList[i].hotbar.GetComponent<HotbarSlot>().ResetHotbarItem();

                    //Update the Hand to see if slot is empty
                    HotbarManager.Instance.ChangeItemInHand();

                    HotbarManager.Instance.SaveData();

                    return;
                }
            }
        }
        else
        {
            for (int i = 0; i < HotbarManager.Instance.hotbarList.Count; i++)
            {
                //Check if HotbarItem is in the player inventory
                if (HotbarManager.Instance.hotbarList[i].itemName != Items.None
                    && HotbarManager.Instance.hotbarList[i].itemName == item.itemName
                    && HotbarManager.Instance.hotbarList[i].itemID == item.itemID)
                {
                    HotbarManager.Instance.selectedItem = Items.None;

                    HotbarManager.Instance.hotbarList[i].itemName = Items.None;
                    HotbarManager.Instance.hotbarList[i].itemID = -1;
                    HotbarManager.Instance.hotbarList[i].hotbar.GetComponent<HotbarSlot>().RemoveHotbarSlotImage();
                    HotbarManager.Instance.hotbarList[i].hotbar.GetComponent<HotbarSlot>().ResetHotbarItem();

                    //Update the Hand to see if slot is empty
                    HotbarManager.Instance.ChangeItemInHand();

                    HotbarManager.Instance.SaveData();

                    return;
                }
            }
        }
    }

    void SetBuildingRequirement()
    {
        if (MoveableObjectManager.Instance.moveableObjectType == MoveableObjectType.BuildingBlock)
        {
            BuildingManager.Instance.SetBuildingRequirements(BuildingManager.Instance.GetBuildingBlock(MoveableObjectManager.Instance.buildingType_Selected, MoveableObjectManager.Instance.buildingMaterial_Selected), BuildingManager.Instance.buildingRequirement_Parent);
        }
        else if (MoveableObjectManager.Instance.moveableObjectType == MoveableObjectType.Machine
            || MoveableObjectManager.Instance.moveableObjectType == MoveableObjectType.Furniture)
        {
            MoveableObjectInfo tempObject = MoveableObjectManager.Instance.GetMoveableObject_SO();
            BuildingManager.Instance.SetBuildingRequirements(tempObject, BuildingManager.Instance.buildingRequirement_Parent);
        }
    }

    public void SetPlayerItemInfo(Items itemName, bool inventory) //true = player, false = chest
    {
        //If playerInventory
        if (inventory)
        {
            if (itemName == Items.None)
            {
                player_ItemName_Display.text = "";
                player_ItemDescription_Display.text = "";
            }
            else
            {
                player_ItemName_Display.text = itemName.ToString();
                player_ItemDescription_Display.text = MainManager.Instance.GetItem(itemName).itemDescription;
            }
        }

        //If chestInventory
        else
        {
            if (itemName == Items.None)
            {
                chest_ItemName_Display.text = "";
                chest_ItemDescription_Display.text = "";
            }
            else
            {
                chest_ItemName_Display.text = itemName.ToString();
                chest_ItemDescription_Display.text = MainManager.Instance.GetItem(itemName).itemDescription;
            }
        }
    }

    public void SetItemSelectedHighlight_Active(int inventory, int ID, Items itemName, bool activate)
    {
        int itemSizeCounter = 0;

        //If it's player inventory
        if (inventory <= 0)
        {
            for (int i = 0; i < itemSlotList_Player.Count; i++)
            {
                //Find item of correct ID
                if (itemSlotList_Player[i].GetComponent<ItemSlot>().itemID == ID && itemSlotList_Player[i].GetComponent<ItemSlot>().itemName == itemName
                    && itemSlotList_Player[i].GetComponent<ItemSlot>().itemName != Items.None)
                {
                    //If cursor enters ItemSlot
                    if (activate)
                    {
                        if (itemSizeCounter <= 0)
                        {
                            itemSlotList_Player[i].GetComponent<Image>().sprite = MainManager.Instance.GetItem(itemName).itemSelected_SpriteList[0];
                        }
                        else
                        {
                            itemSlotList_Player[i].GetComponent<Image>().sprite = MainManager.Instance.GetItem(itemName).itemSelected_SpriteList[itemSizeCounter];
                        }
                    }

                    //If cursor exits ItemSlot
                    else
                    {
                        if (itemSizeCounter <= 0)
                        {
                            itemSlotList_Player[i].GetComponent<Image>().sprite = MainManager.Instance.GetItem(itemName).itemSpriteList[0];
                        }
                        else
                        {
                            itemSlotList_Player[i].GetComponent<Image>().sprite = MainManager.Instance.GetItem(itemName).itemSpriteList[itemSizeCounter];
                        }
                    }

                    itemSizeCounter++;
                }
            }
        }

        //If it's a chest
        else
        {
            for (int i = 0; i < itemSlotList_Chest.Count; i++)
            {
                //Find item of correct ID
                if (itemSlotList_Chest[i].GetComponent<ItemSlot>().itemID == ID && itemSlotList_Chest[i].GetComponent<ItemSlot>().itemName == itemName
                    && itemSlotList_Chest[i].GetComponent<ItemSlot>().itemName != Items.None)
                {
                    //If cursor enters ItemSlot
                    if (activate)
                    {
                        if (itemSizeCounter <= 0)
                        {
                            itemSlotList_Chest[i].GetComponent<Image>().sprite = MainManager.Instance.GetItem(itemName).itemSelected_SpriteList[0];
                        }
                        else
                        {
                            itemSlotList_Chest[i].GetComponent<Image>().sprite = MainManager.Instance.GetItem(itemName).itemSelected_SpriteList[itemSizeCounter];
                        }
                    }

                    //If cursor exits ItemSlot
                    else
                    {
                        if (itemSizeCounter <= 0)
                        {
                            itemSlotList_Chest[i].GetComponent<Image>().sprite = MainManager.Instance.GetItem(itemName).itemSpriteList[0];
                        }
                        else
                        {
                            itemSlotList_Chest[i].GetComponent<Image>().sprite = MainManager.Instance.GetItem(itemName).itemSpriteList[itemSizeCounter];
                        }
                    }

                    itemSizeCounter++;
                }
            }
        }
    }


    //--------------------


    public int GetAmountOfItemInInventory(int inventory, Items itemName)
    {
        int counter = 0;

        for (int i = 0; i < inventories[inventory].itemsInInventory.Count; i++)
        {
            if (inventories[inventory].itemsInInventory[i].itemName == itemName)
            {
                counter++;
            }
        }

        return counter;
    }

    public bool GetInventoryRequirements(int inventory, List<CraftingRequirements> requirementList)
    {
        for (int i = 0; i < requirementList.Count; i++)
        {
            if (GetAmountOfItemInInventory(inventory, requirementList[i].itemName) < requirementList[i].amount)
            {
                return false;
            }
        }

        return true;
    }


    //--------------------


    public void SelectItemToHotbar(Items itemName, int ID)
    {
        //Get selected item
        //itemSlotList_Player.

        //Mark the item as selected to the hotbar (and add its hotbarNumber on the selected hotbar)

    }
    public void DeselectItemToHotbar(Items itemName, int ID)
    {
        //Get selected item


        //Remove the selected HotbarMark from this item

    }


    //--------------------


    public void PrepareInventoryUI(int inventory, bool isMovingItem)
    {
        int inventorySlots = (int)inventories[inventory].inventorySize.x * (int)inventories[inventory].inventorySize.y;

        //Add all InventorySlots for the inventory
        for (int i = 0; i < inventorySlots; i++)
        {
            //Add for the Player Inventory
            if (inventory <= 0)
            {
                itemSlotList_Player.Add(Instantiate(itemSlot_Prefab, Vector3.zero, Quaternion.identity) as GameObject);
                itemSlotList_Player[itemSlotList_Player.Count - 1].transform.SetParent(TabletManager.Instance.playerInventory_Parent.transform);
                itemSlotList_Player[itemSlotList_Player.Count - 1].GetComponent<ItemSlot>().inventoryIndex = inventory;
            }

            //Add for the Chest Inventory
            else
            {
                itemSlotList_Chest.Add(Instantiate(itemSlot_Prefab, Vector3.zero, Quaternion.identity) as GameObject);
                itemSlotList_Chest[itemSlotList_Chest.Count - 1].transform.SetParent(TabletManager.Instance.chestInventory_Parent.transform);
                itemSlotList_Chest[itemSlotList_Chest.Count - 1].GetComponent<ItemSlot>().inventoryIndex = inventory;
            }
        }

        //Set Fake Slots
        if (inventory <= 0)
        {
            SetFakeItemSlotAmount(inventorySlots, player_FakeSlot_List);
        }
        else
        {
            SetFakeItemSlotAmount(inventorySlots, chest_FakeSlot_List);
        }

        //Sort inventory so the biggest items are first
        SortInventory(inventory);

        //Setup the grid with all items
        SetupUIGrid(inventory, isMovingItem);
    }
    void SortInventory(int inventory)
    {
        //Perform Bubble sort of the inventory based on the highest size
        int n = inventories[inventory].itemsInInventory.Count;
        List<InventoryItem> item = inventories[inventory].itemsInInventory;

        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                // Swap if the element found is greater than the next element
                int size_j = (int)inventories[inventory].itemsInInventory[j].itemSize.x * (int)inventories[inventory].itemsInInventory[j].itemSize.y;
                int size_jp = (int)inventories[inventory].itemsInInventory[j + 1].itemSize.x * (int)inventories[inventory].itemsInInventory[j + 1].itemSize.y;

                //Compare based on size
                if (size_j < size_jp)
                {
                    InventoryItem temp = inventories[inventory].itemsInInventory[j];

                    inventories[inventory].itemsInInventory[j] = inventories[inventory].itemsInInventory[j + 1];
                    inventories[inventory].itemsInInventory[j + 1] = temp;
                }

                //If size is equal, compare based on the x-value
                else if (size_j == size_jp)
                {
                    if (inventories[inventory].itemsInInventory[j].itemSize.x > inventories[inventory].itemsInInventory[j + 1].itemSize.x)
                    {
                        InventoryItem temp = inventories[inventory].itemsInInventory[j];

                        inventories[inventory].itemsInInventory[j] = inventories[inventory].itemsInInventory[j + 1];
                        inventories[inventory].itemsInInventory[j + 1] = temp;
                    }
                }
            }
        }

        //Swap places if items are not of the same itemName

        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                // Compare adjacent strings and swap if they are in the wrong order
                if (item[j].itemSize == item[j + 1].itemSize && item[j].itemName > item[j + 1].itemName)
                {
                    InventoryItem temp = item[j];

                    item[j] = item[j + 1];
                    item[j + 1] = temp;
                }
            }
        }
    } // Under PrepareInventoryUI
    void SetupUIGrid(int inventory, bool isMovingItem)
    {
        //Set which Inventory UI to focus on
        #region
        List<GameObject> inventoryList = new List<GameObject>();
        if (inventory <= 0)
        {
            inventoryList = itemSlotList_Player;
        }
        else
        {
            inventoryList = itemSlotList_Chest;
        }
        #endregion

        //Get Inventory Sizes
        int inventorySizeX = (int)inventories[inventory].inventorySize.x;
        int inventorySizeY = (int)inventories[inventory].inventorySize.y;

        int itemPlaced = 0;

        //Setup Inventory items
        for (int j = 0; j < inventories[inventory].itemsInInventory.Count; j++)
        {
            //Go through all inventory slots
            for (int i = 0; i < inventoryList.Count; i++)
            {
                //If slot is empty, check if item can be placed in its range
                if (inventoryList[i].GetComponent<ItemSlot>().itemName == Items.None)
                {
                    int itemSizeX = (int)MainManager.Instance.GetItem(inventories[inventory].itemsInInventory[j].itemName).itemSize.x;
                    int itemSizeY = (int)MainManager.Instance.GetItem(inventories[inventory].itemsInInventory[j].itemName).itemSize.y;

                    //Check if Item's x-value is inside the x-size of the grid
                    #region
                    int leftOfGridX = inventorySizeX * inventorySizeY;

                    int temp = i;

                    while (temp >= inventorySizeX)
                    {
                        temp -= inventorySizeX;
                    }

                    int remainder = inventorySizeX - temp;

                    if (itemSizeX > remainder)
                    {
                        //Go to next position
                    }
                    else
                    {
                        //Check if Item's y-value is inside the y-size of the grid
                        #region
                        int leftOfGridY = inventorySizeX * inventorySizeY;
                        int rowCounterleft;
                        int spaceIn_Y_Direction = 1;
                        temp = i;

                        rowCounterleft = leftOfGridY - i; //8

                        while (rowCounterleft >= inventorySizeX)
                        {
                            rowCounterleft -= inventorySizeX;
                            spaceIn_Y_Direction++;
                        }

                        if (spaceIn_Y_Direction < itemSizeY)
                        {
                            //Go to next position
                        }
                        else
                        {
                            //Go through all positions where the item may fit
                            #region
                            int tempCount = 0;
                            List<int> posList = new List<int>();
                            for (int y = 0; y < itemSizeY; y++)
                            {
                                for (int x = 0; x < itemSizeX; x++)
                                {
                                    //Safty chack to see if item is inside gridBounds
                                    if ((i + x + (y * inventorySizeX)) <= inventoryList.Count - 1)
                                    {
                                        if (inventoryList[i + x + (y * inventorySizeX)].GetComponent<ItemSlot>().itemName == Items.None)
                                        {
                                            tempCount++;

                                            posList.Add(i + x + (y * inventorySizeX));
                                        }
                                    }
                                }
                            }
                            #endregion

                            //If all positions are empty, place the item there
                            #region
                            if (tempCount == itemSizeX * itemSizeY)
                            {
                                //print("2. posList = " + posList.Count + " | tempCount = " + tempCount + " | sizeX = " + itemSizeX + " | sizeY = " + itemSizeY);

                                itemPlaced++;

                                for (int k = 0; k < posList.Count; k++)
                                {
                                    inventoryList[posList[k]].GetComponent<ItemSlot>().itemName = inventories[inventory].itemsInInventory[j].itemName;
                                    inventoryList[posList[k]].GetComponent<ItemSlot>().itemID = inventories[inventory].itemsInInventory[j].itemID;

                                    inventoryList[posList[k]].GetComponent<Image>().color = new Color(1, 1, 1, 1);
                                    inventoryList[posList[k]].GetComponent<Image>().sprite = MainManager.Instance.GetItem(inventories[inventory].itemsInInventory[j].itemName).itemSpriteList[k];
                                }

                                break;
                            }
                            #endregion
                        }
                        #endregion
                    }
                    #endregion
                }
            }
        }

        //If there isn't enough room for the item in the inventory
        if (itemPlaced < inventories[inventory].itemsInInventory.Count)
        {
            //print("1. Inventory doesn't have enough room to place this item");

            //Remove the last picked up item from the inventory and spawn it into the world
            for (int i = 0; i < inventories[inventory].itemsInInventory.Count; i++)
            {
                if (inventories[inventory].itemsInInventory[i].itemName == lastItemToGet)
                {
                    //If Item was attempted moved into another inventory
                    if (isMovingItem)
                    {
                        RemoveItemFromInventory(inventory, lastItemToGet, inventories[inventory].itemsInInventory[i].itemID, true);

                        if (inventory <= 0)
                        {
                            AddItemToInventory(chestInventoryOpen, itemTemp, true);
                        }
                        else
                        {
                            AddItemToInventory(0, itemTemp, true);
                        }
                    }

                    //If Item was attemptd picked up
                    else
                    {
                        RemoveItemFromInventory(inventory, lastItemToGet, inventories[inventory].itemsInInventory[i].itemID);
                    }

                    break;
                }
            }
        }
    }

    public void RemoveInventoriesUI()
    {
        //print("Reset Both Inventories");

        for (int i = 0; i < itemSlotList_Player.Count; i++)
        {
            itemSlotList_Player[i].SetActive(true);
            itemSlotList_Player[i].GetComponent<ItemSlot>().DestroyItemSlot();
        }
        for (int i = 0; i < itemSlotList_Chest.Count; i++)
        {
            itemSlotList_Chest[i].SetActive(true);
            itemSlotList_Chest[i].GetComponent<ItemSlot>().DestroyItemSlot();
        }

        //Clear the lists for both inventory UIs
        itemSlotList_Player.Clear();
        itemSlotList_Chest.Clear();
    }

    public void SetFakeItemSlotAmount(int slotAmount, List<GameObject> itemSlotList)
    {
        for (int i = 0; i < itemSlotList.Count; i++)
        {
            itemSlotList[i].SetActive(false);
        }

        for (int i = 0; i < slotAmount; i++)
        {
            itemSlotList[i].SetActive(true);
        }
    }


    //--------------------


    #region Open/Close Inventory Menu
    public void OpenPlayerInventory()
    {
        if (inventoryIsOpen)
        {
            //ClosePlayerInventory();
        }
        else
        {
            //Cursor.lockState = CursorLockMode.None;
            //MainManager.Instance.menuStates = MenuStates.InventoryMenu;

            PrepareInventoryUI(0, false); //Prepare PLAYER Inventory

            TabletManager.Instance.playerInventory_Parent.GetComponent<RectTransform>().sizeDelta = inventories[0].inventorySize * cellsize;
            TabletManager.Instance.playerInventory_Parent.GetComponent<GridLayoutGroup>().cellSize = new Vector2(cellsize, cellsize);
            TabletManager.Instance.playerInventory_Parent.SetActive(true);

            playerInventory_Fake_Parent.GetComponent<RectTransform>().sizeDelta = inventories[0].inventorySize * cellsize;
            playerInventory_Fake_Parent.GetComponent<GridLayoutGroup>().cellSize = new Vector2(cellsize, cellsize);
            playerInventory_Fake_Parent.SetActive(true);

            inventoryIsOpen = true;
        }
    }
    public void ClosePlayerInventory()
    {
        TabletManager.Instance.playerInventory_Parent.SetActive(false);
        TabletManager.Instance.chestInventory_Parent.SetActive(false);
        playerInventory_Fake_Parent.SetActive(false);
        chestInventory_Fake_Parent.SetActive(false);

        RemoveInventoriesUI();

        //Cursor.lockState = CursorLockMode.Locked;
        //MainManager.Instance.menuStates = MenuStates.None;

        inventoryIsOpen = false;
    }
    #endregion
}


[Serializable]
public class Inventory
{
    [Header("General")]
    public int inventoryIndex;
    public Vector2 inventorySize;

    [Header("List of items in this inventory")]
    public List<InventoryItem> itemsInInventory = new List<InventoryItem>();
}

[Serializable]
public class InventoryItem
{
    public Items itemName;
    public Vector2 itemSize;

    public int inventoryIndex;
    public int itemID; //Find all other item in the UI grid with this ID
}