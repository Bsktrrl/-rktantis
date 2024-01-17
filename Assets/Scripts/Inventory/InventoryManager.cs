using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : Singleton<InventoryManager>
{
    [Header("Inventory")]
    public Vector2 inventorySize;
    public int cellsize = 100;

    [Header("Item")]
    public Items lastItemToGet;

    [Header("GameObjects")]
    public GameObject playerInventory_Parent;
    public GameObject chestInventory_Parent;

    public GameObject itemSlot_Prefab;

    public GameObject handDropPoint;
    public GameObject worldObject_Parent;

    [Header("Lists")]
    public List<Inventory> inventories = new List<Inventory>();
    public List<GameObject> worldObjectList = new List<GameObject>();
    public List<WorldObject> worldObjectSaveList = new List<WorldObject>();

    public List<GameObject> itemSlotList_Player = new List<GameObject>();
    public List<GameObject> itemSlotList_Chest = new List<GameObject>();

    bool inventoryIsOpen;

    public int chestInventoryOpen;
    GameObject itemTemp;


    //--------------------


    private void Start()
    {
        PlayerButtonManager.OpenPlayerInventory_isPressedDown += OpenPlayerInventory;
        PlayerButtonManager.ClosePlayerInventory_isPressedDown += ClosePlayerInventory;

        playerInventory_Parent.SetActive(false);
        chestInventory_Parent.SetActive(false);
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
        MainManager.instance.player.transform.SetPositionAndRotation(DataManager.Instance.playerPos_Store, DataManager.Instance.playerRot_Store);
        #endregion

        #region WorldObjectList
        //Setup WorldObjectList
        for (int i = 0; i < worldObjectList.Count; i++)
        {
            Destroy(worldObjectList[i]);
        }
        worldObjectList.Clear();

        worldObjectSaveList = DataManager.Instance.worldObject_StoreList;
        for (int i = 0; i < worldObjectSaveList.Count; i++)
        {
            worldObjectList.Add(Instantiate(SetupWorldObjectFromSave(worldObjectSaveList[i]), worldObjectSaveList[i].objectPosition, worldObjectSaveList[i].objectRotation) as GameObject);
            worldObjectList[worldObjectList.Count - 1].transform.parent = worldObject_Parent.transform;

            //If it's not a stationary Object, activate Gravity
            if (!worldObjectList[worldObjectList.Count - 1].GetComponent<InteractableObject>().isMachine)
            {
                worldObjectList[worldObjectList.Count - 1].GetComponent<Rigidbody>().isKinematic = false;
                worldObjectList[worldObjectList.Count - 1].GetComponent<Rigidbody>().useGravity = true;
            }
        }
        #endregion
    }
    public void SaveData()
    {
        DataManager.Instance.Inventories_StoreList = inventories;

        //Save WorldObject into a saveable list
        List<WorldObject> tempList = new List<WorldObject>();
        for (int i = 0; i < worldObjectList.Count; i++)
        {
            WorldObject temp = new WorldObject();

            temp.objectName = worldObjectList[i].GetComponent<InteractableObject>().itemName;
            temp.objectPosition = worldObjectList[i].transform.position;
            temp.objectRotation = worldObjectList[i].transform.rotation;

            tempList.Add(temp);
        }
        DataManager.Instance.worldObject_StoreList = tempList;
    }
    public void SaveData(ref GameData gameData)
    {
        DataManager.Instance.Inventories_StoreList = inventories;
        DataManager.Instance.worldObject_StoreList = worldObjectSaveList;

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
            item.itemSize = MainManager.instance.GetItem(obj.GetComponent<ItemSlot>().itemName).itemSize;
            item.itemID = obj.GetComponent<ItemSlot>().itemID;

            lastItemToGet = obj.GetComponent<ItemSlot>().itemName;
            itemTemp = obj;
        }

        //If item is being picked up
        else
        {
            item.inventoryIndex = inventory;
            item.itemName = obj.GetComponent<InteractableObject>().itemName;
            item.itemSize = MainManager.instance.GetItem(obj.GetComponent<InteractableObject>().itemName).itemSize;

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

        PrepareInventoryUI(inventory, itemIsMoved);
        RemoveInventoriesUI();

        SetBuildingRequirement();

        return true;
    }
    public bool AddItemToInventory(int inventory, Items itemName)
    {
        InventoryItem item = new InventoryItem();

        item.inventoryIndex = inventory;
        item.itemName = itemName;
        item.itemSize = MainManager.instance.GetItem(itemName).itemSize;

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

        PrepareInventoryUI(inventory, false);
        RemoveInventoriesUI();

        SetBuildingRequirement();

        return true;
    }
    public void RemoveItemFromInventory(int inventory, Items itemName)
    {
        //print("100. item : " + itemName + " is removed from inventory: " + inventory);

        for (int i = 0; i < inventories[inventory].itemsInInventory.Count; i++)
        {
            if (inventories[inventory].itemsInInventory[i].itemName == itemName)
            {
                inventories[inventory].itemsInInventory.RemoveAt(i);

                break;
            }
        }

        RemoveInventoriesUI();
        PrepareInventoryUI(inventory, false);

        //Spawn item into the World
        worldObjectList.Add(Instantiate(MainManager.instance.GetItem(itemName).worldObjectPrefab, handDropPoint.transform.position, Quaternion.identity) as GameObject);
        worldObjectList[worldObjectList.Count - 1].transform.parent = worldObject_Parent.transform;

        //Set Gravity true on the worldObject
        worldObjectList[worldObjectList.Count - 1].GetComponent<Rigidbody>().isKinematic = false;
        worldObjectList[worldObjectList.Count - 1].GetComponent<Rigidbody>().useGravity = true;

        //If item is removed from the inventory, update the Hotbar
        if (inventory <= 0)
        {
            CheckHotbarItemInInventory();
        }

        SetBuildingRequirement();

        SaveData();
    }
    public void RemoveItemFromInventory(int inventory, Items itemName, bool itemIsMoved)
    {
        //print("200. item : " + itemName + " is removed from inventory: " + inventory);

        for (int i = 0; i < inventories[inventory].itemsInInventory.Count; i++)
        {
            if (inventories[inventory].itemsInInventory[i].itemName == itemName)
            {
                inventories[inventory].itemsInInventory.RemoveAt(i);

                break;
            }
        }

        RemoveInventoriesUI();
        PrepareInventoryUI(inventory, true);

        SetBuildingRequirement();
    }

    public void MoveItemToInventory(int inventory, GameObject obj)
    {
        //Move item to Player Inventory
        if (inventory <= 0)
        {
            RemoveItemFromInventory(0, obj.GetComponent<ItemSlot>().itemName, true);
            AddItemToInventory(chestInventoryOpen, obj, true);
        }

        //Move item to Chest Inventory
        else
        {
            RemoveItemFromInventory(chestInventoryOpen, obj.GetComponent<ItemSlot>().itemName, true);
            AddItemToInventory(0, obj, true);
        }


        RemoveInventoriesUI();
        PrepareInventoryUI(0, true);
        PrepareInventoryUI(chestInventoryOpen, true);

        CheckHotbarItemInInventory();
    }

    public void CheckHotbarItemInInventory()
    {
        //print("100. CheckHotbarItemInInventory");

        for (int i = 0; i < HotbarManager.Instance.hotbarList.Count; i++)
        {
            bool isIncluded = false;

            //Check if HotbarItem is in the inventory
            for (int j = 0; j < inventories[0].itemsInInventory.Count; j++)
            {
                if (HotbarManager.Instance.hotbarList[i].GetComponent<HotbarSlot>().hotbarItemName != Items.None
                    && HotbarManager.Instance.hotbarList[i].GetComponent<HotbarSlot>().hotbarItemName == inventories[0].itemsInInventory[j].itemName)
                {
                    isIncluded = true;

                    break;
                }
            }

            //If HotbarItem isn't in the inventory, remove it from the Hotbar
            if (!isIncluded)
            {
                if (HotbarManager.Instance.selectedItem == HotbarManager.Instance.hotbarList[i].GetComponent<HotbarSlot>().hotbarItemName)
                {
                    HotbarManager.Instance.selectedItem = Items.None;
                }

                HotbarManager.Instance.hotbarList[i].GetComponent<HotbarSlot>().RemoVeHotbarSlotImage();
                HotbarManager.Instance.hotbarList[i].GetComponent<HotbarSlot>().ResetHotbarItem();
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
                itemSlotList_Player[itemSlotList_Player.Count - 1].transform.parent = playerInventory_Parent.transform;
                itemSlotList_Player[itemSlotList_Player.Count - 1].GetComponent<ItemSlot>().inventoryIndex = inventory;
            }

            //Add for the Chest Inventory
            else
            {
                itemSlotList_Chest.Add(Instantiate(itemSlot_Prefab, Vector3.zero, Quaternion.identity) as GameObject);
                itemSlotList_Chest[itemSlotList_Chest.Count - 1].transform.parent = chestInventory_Parent.transform;
                itemSlotList_Chest[itemSlotList_Chest.Count - 1].GetComponent<ItemSlot>().inventoryIndex = inventory;
            }
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
                    int itemSizeX = (int)MainManager.instance.GetItem(inventories[inventory].itemsInInventory[j].itemName).itemSize.x;
                    int itemSizeY = (int)MainManager.instance.GetItem(inventories[inventory].itemsInInventory[j].itemName).itemSize.y;

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
                                    inventoryList[posList[k]].GetComponent<Image>().sprite = MainManager.instance.GetItem(inventories[inventory].itemsInInventory[j].itemName).itemSpriteList[k];
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

        //print("20. itemPlaced: " + itemPlaced + " | inventory.Count: " + inventories[inventory].itemsInInventory.Count);

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
                        RemoveItemFromInventory(inventory, lastItemToGet, true);

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
                        RemoveItemFromInventory(inventory, lastItemToGet);
                    }

                    break;
                }
            }
        }
    } // Under PrepareInventoryUI

    public void RemoveInventoriesUI()
    {
        print("Reset Both Inventories");

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


    //--------------------


    GameObject SetupWorldObjectFromSave(WorldObject worldObj)
    {
        if (worldObj.objectName != Items.None)
        {
            return MainManager.instance.GetItem(worldObj.objectName).worldObjectPrefab;
        }
        else
        {
            return null;
        }
    }


    //--------------------


    #region Open/Close Inventory Menu
    public void OpenPlayerInventory()
    {
        if (inventoryIsOpen)
        {
            ClosePlayerInventory();
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            MainManager.instance.menuStates = MenuStates.InventoryMenu;

            PrepareInventoryUI(0, false); //Prepare PLAYER Inventory

            playerInventory_Parent.GetComponent<RectTransform>().sizeDelta = inventories[0].inventorySize * cellsize;
            playerInventory_Parent.GetComponent<GridLayoutGroup>().cellSize = new Vector2(cellsize, cellsize);
            playerInventory_Parent.SetActive(true);

            inventoryIsOpen = true;
        }
    }
    void ClosePlayerInventory()
    {
        playerInventory_Parent.SetActive(false);
        chestInventory_Parent.SetActive(false);

        RemoveInventoriesUI();

        Cursor.lockState = CursorLockMode.Locked;
        MainManager.instance.menuStates = MenuStates.None;

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

[Serializable]
public struct WorldObject
{
    public Items objectName;
    public Vector3 objectPosition;
    public Quaternion objectRotation;
}