using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : Singleton<InventoryManager>
{
    #region Variables
    [Header("Inventory")]
    public Vector2 inventorySize;
    public int cellsize = 70;

    public Vector2 smallChest_Size = new Vector2(4, 4);
    public Vector2 bigChest_Size = new Vector2(7, 7);

    [Header("Item")]
    public Items lastItemToGet;
    public int lastIDToGet;

    public Items lastItemToHover;
    public int lastIDToHover;

    [SerializeField] TextMeshProUGUI player_ItemName_Display;
    [SerializeField] TextMeshProUGUI player_ItemDescription_Display;
    [SerializeField] TextMeshProUGUI chest_ItemName_Display;
    [SerializeField] TextMeshProUGUI chest_ItemDescription_Display;

    [Header("GameObjects")]
    public GameObject itemSlot_Prefab;

    public GameObject handDropPoint;
    public float currentHandDropPoint_Y;
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
    public float gravityTime = 2.5f;

    public bool itemDropped;

    [Header("Item Info")]
    public GameObject itemInfo_Parent;
    [HideInInspector] public InventoryItemInfo itemInfo;
    #endregion


    //--------------------


    private void Start()
    {
        TabletManager.Instance.playerInventory_Parent.SetActive(false);
        TabletManager.Instance.chestInventory_Parent.SetActive(false);
        playerInventory_Fake_Parent.SetActive(false);
        chestInventory_Fake_Parent.SetActive(false);

        PlayerButtonManager.isPressed_1 += QuickHotbarSelect_1;
        PlayerButtonManager.isPressed_2 += QuickHotbarSelect_2;
        PlayerButtonManager.isPressed_3 += QuickHotbarSelect_3;
        PlayerButtonManager.isPressed_4 += QuickHotbarSelect_4;
        PlayerButtonManager.isPressed_5 += QuickHotbarSelect_5;

        itemInfo = itemInfo_Parent.GetComponent<InventoryItemInfo>();
    }
    private void Update()
    {
        if (MainManager.Instance.menuStates == MenuStates.InventoryMenu
            || MainManager.Instance.menuStates == MenuStates.ChestMenu
            || MainManager.Instance.menuStates == MenuStates.EquipmentMenu
            || MainManager.Instance.menuStates == MenuStates.CraftingMenu)
        {
            DisplayInventoryItemInfo();
        }
    }


    //--------------------


    #region SaveLoad
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

        #region Chest Sizes
        smallChest_Size = DataManager.Instance.smallChest_Size_Store;
        bigChest_Size = DataManager.Instance.bigChest_Size_Store;
        #endregion
    }
    public void SaveData()
    {
        //All Inventories
        DataManager.Instance.Inventories_StoreList = inventories;

        //Chest Sizes (may be upgraded in the SkillTree)
        DataManager.Instance.smallChest_Size_Store = smallChest_Size;
        DataManager.Instance.bigChest_Size_Store = bigChest_Size;
}
    public void SaveData(ref GameData gameData)
    {
        SaveData();

        print("Save_Inventories");
    }
    #endregion


    //--------------------


    #region Add/Remove Inventory
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
    public void AddInventory(InteractableObject chest, Vector2 size)
    {
        AddInventory(size);

        //Add index to the chest to connect it to the inventoriesList
        chest.inventoryIndex = inventories.Count - 1;

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
    #endregion


    //--------------------


    #region Add/Remove item to Inventory
    public bool AddItemToInventory(int inventory, GameObject obj, bool itemIsMoved)
    {
        InventoryItem item = new InventoryItem();

        //If item is being moved to another inventory
        if (itemIsMoved)
        {
            item.inventoryIndex = inventory;
            item.itemName = obj.GetComponent<ItemSlot>().itemName;
            item.itemSize = MainManager.Instance.GetItem(obj.GetComponent<ItemSlot>().itemName).itemSize;
            item.durability_Current = obj.GetComponent<ItemSlot>().durabilityCurrent;
            item.itemID = obj.GetComponent<ItemSlot>().itemID;

            lastItemToGet = obj.GetComponent<ItemSlot>().itemName;
            lastIDToGet = obj.GetComponent<ItemSlot>().itemID;
            itemTemp = obj;
        }

        //If item is being picked up
        else
        {
            item.inventoryIndex = inventory;
            item.itemName = obj.GetComponent<InteractableObject>().itemName;
            item.itemSize = MainManager.Instance.GetItem(obj.GetComponent<InteractableObject>().itemName).itemSize;
            if (obj.GetComponent<InteractableObject>().durability_Current <= 0)
            {
                item.durability_Current = MainManager.Instance.GetItem(obj.GetComponent<InteractableObject>().itemName).durability_Max;
            }
            else
            {
                item.durability_Current = obj.GetComponent<InteractableObject>().durability_Current;
            }
            
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

            lastIDToGet = item.itemID;
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
        item.durability_Current = MainManager.Instance.GetItem(itemName).durability_Max;

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

        lastIDToGet = item.itemID;

        inventories[inventory].itemsInInventory.Add(item);

        RemoveInventoriesUI();
        PrepareInventoryUI(inventory, false);

        SetBuildingRequirement();

        return true;
    }
    public void RemoveItemFromInventory(int inventory, Items itemName, int ID)
    {
        //From inventory to World
        #region
        int index = -1;

        //Remove from Hotbar
        for (int i = inventories[inventory].itemsInInventory.Count - 1; i >= 0 ; i--)
        {
            for (int j = 0; j < HotbarManager.Instance.hotbarList.Count; j++)
            {
                if (inventories[inventory].itemsInInventory[i].itemName == itemName
                && inventories[inventory].itemsInInventory[i].itemID == ID)
                {
                    if (HotbarManager.Instance.hotbarList[j].itemName == itemName
                        && HotbarManager.Instance.hotbarList[j].itemID != ID)
                    {
                        index = i;

                        i = 0;
                        j = HotbarManager.Instance.hotbarList.Count;
                    }
                }
            }
        }

        if (index >= 0)
        {
            //Spawn item into the World, if the item has a WorldObject attached
            SpawnItemToWorld(itemName, handDropPoint, true, inventories[inventory].itemsInInventory[index], 0);

            inventories[inventory].itemsInInventory.RemoveAt(index);
        }
        else
        {
            for (int i = inventories[inventory].itemsInInventory.Count - 1; i >= 0; i--)
            {
                if (inventories[inventory].itemsInInventory[i].itemName == itemName
                    && inventories[inventory].itemsInInventory[i].itemID == ID)
                {
                    //Spawn item into the World, if the item has a WorldObject attached
                    SpawnItemToWorld(itemName, handDropPoint, true, inventories[inventory].itemsInInventory[i], 0);

                    inventories[inventory].itemsInInventory.RemoveAt(i);

                    break;
                }
            }
        }
        #endregion

        RemoveInventoriesUI();
        PrepareInventoryUI(inventory, false);

        //If item is removed from the inventory, update the Hotbar
        if (inventory <= 0)
        {
            CheckHotbarItemInInventory();
        }

        SetBuildingRequirement();

        SaveData();
    }
    IEnumerator SpawnedObjectGravityTime(GameObject spawnedObject)
    {
        //Wait for gravityTime-seconds
        yield return new WaitForSeconds(gravityTime);

        //Turn off Gravity
        //spawnedObject.GetComponent<Rigidbody>().isKinematic = true;
        //spawnedObject.GetComponent<Rigidbody>().useGravity = false;
    }
    public void RemoveItemFromInventory(int inventory, Items itemName, int ID, bool itemIsMovedOrRemoved)
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
        SoundManager.Instance.Play_Inventory_MoveItem_Clip();

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

        CheckHotbarItemInInventory();

        //Update the Hand to see if slot is empty
        HotbarManager.Instance.ChangeItemInHand();
    }
    
    public void SpawnItemToWorld(Items itemName, GameObject dropPos, bool dropSound, InventoryItem item, float spawnPos_Offset)
    {
        if (MainManager.Instance.GetItem(itemName).worldObjectPrefab)
        {
            if (item != null)
            {
                print("item: " + item.durability_Current);
            }
            

            //Play Drop-Sound
            if (dropSound)
            {
                SoundManager.Instance.Play_Inventory_DropItem_Clip();
            }
            
            if (dropPos == handDropPoint)
            {
                //If dropped from hand, have the same dropspot each time
                WorldObjectManager.Instance.worldObjectList.Add(Instantiate(MainManager.Instance.GetItem(itemName).worldObjectPrefab, dropPos.transform.position, Quaternion.identity) as GameObject);
            }
            else
            {
                //When dropping from other places, change the pos slightly
                float x = UnityEngine.Random.value / 2;
                float y = 0;
                float z = UnityEngine.Random.value / 2;

                if (UnityEngine.Random.Range(0, 1) == 1)
                {
                    x = -x;
                }
                if (UnityEngine.Random.Range(0, 1) == 1)
                {
                    y = -y;
                }
                if (UnityEngine.Random.Range(0, 1) == 1)
                {
                    z = -z;
                }

                Vector3 newSpawnPos = new Vector3(dropPos.transform.position.x + x, dropPos.transform.position.y + y, dropPos.transform.position.z + z);

                newSpawnPos += new Vector3(0, spawnPos_Offset, 0);

                WorldObjectManager.Instance.worldObjectList.Add(Instantiate(MainManager.Instance.GetItem(itemName).worldObjectPrefab, newSpawnPos, Quaternion.identity) as GameObject);
            }

            WorldObjectManager.Instance.worldObjectList[WorldObjectManager.Instance.worldObjectList.Count - 1].transform.parent = worldObject_Parent.transform;

            //Set Gravity true on the worldObject
            WorldObjectManager.Instance.worldObjectList[WorldObjectManager.Instance.worldObjectList.Count - 1].GetComponent<Rigidbody>().isKinematic = false;
            WorldObjectManager.Instance.worldObjectList[WorldObjectManager.Instance.worldObjectList.Count - 1].GetComponent<Rigidbody>().useGravity = true;

            //Set Durability
            if (item == null)
            {
                WorldObjectManager.Instance.worldObjectList[WorldObjectManager.Instance.worldObjectList.Count - 1].GetComponent<InteractableObject>().durability_Current = MainManager.Instance.GetItem(itemName).durability_Max;
            }
            else
            {
                WorldObjectManager.Instance.worldObjectList[WorldObjectManager.Instance.worldObjectList.Count - 1].GetComponent<InteractableObject>().durability_Current = item.durability_Current;
            }

            //Update item in the World
            WorldObjectManager.Instance.WorldObject_SaveState_AddObjectToWorld(itemName);

            StartGravityCoroutine(WorldObjectManager.Instance.worldObjectList[WorldObjectManager.Instance.worldObjectList.Count - 1]);

            WorldObjectManager.Instance.SaveWorldObjectPositions();

            currentHandDropPoint_Y = handDropPoint.transform.position.y;
            itemDropped = true;
        }
    }
    public void StartGravityCoroutine(GameObject obj)
    {
        //Stop gravity after gravityTime-seconds
        if (obj.GetComponent<Rigidbody>())
        {
            StartCoroutine(SpawnedObjectGravityTime(WorldObjectManager.Instance.worldObjectList[WorldObjectManager.Instance.worldObjectList.Count - 1]));
        }
    }
    #endregion


    //--------------------


    #region
    public void CheckHotbarItemInInventory()
    {
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
                HotbarManager.Instance.hotbarList[i].hotbar.GetComponent<HotbarSlot>().RemoveItemFromHotbar();
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
                    HotbarManager.Instance.hotbarList[i].hotbar.GetComponent<HotbarSlot>().RemoveItemFromHotbar();
                    HotbarManager.Instance.hotbarList[i].hotbar.GetComponent<HotbarSlot>().ResetHotbarItem();
                    HotbarManager.Instance.hotbarList[i].hotbar.GetComponent<HotbarSlot>().durabilityMeterParent.SetActive(false);

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
                    HotbarManager.Instance.hotbarList[i].hotbar.GetComponent<HotbarSlot>().RemoveItemFromHotbar();
                    HotbarManager.Instance.hotbarList[i].hotbar.GetComponent<HotbarSlot>().ResetHotbarItem();
                    HotbarManager.Instance.hotbarList[i].hotbar.GetComponent<HotbarSlot>().durabilityMeterParent.SetActive(false);

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
                player_ItemName_Display.text = SpaceTextConverting.Instance.SetText(itemName.ToString());
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
                chest_ItemName_Display.text = SpaceTextConverting.Instance.SetText(itemName.ToString());
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
    #endregion


    //--------------------


    #region
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
    #endregion


    //--------------------


    #region
    public void SelectItemInfoToHotbar()
    {
        //Assign HotbarInfo to the correct ItemSlot
        for (int i = 0; i < inventories[0].itemsInInventory.Count; i++)
        {
            for (int j = 0; j < itemSlotList_Player.Count; j++)
            {
                //Find relevant item
                if (itemSlotList_Player[j].GetComponent<ItemSlot>().itemName == inventories[0].itemsInInventory[i].itemName
                    && itemSlotList_Player[j].GetComponent<ItemSlot>().itemID == inventories[0].itemsInInventory[i].itemID)
                {
                    //If the selected item is in the Hotbar
                    for (int k = 0; k < HotbarManager.Instance.hotbarList.Count; k++)
                    {
                        if (HotbarManager.Instance.hotbarList[k].itemName == itemSlotList_Player[j].GetComponent<ItemSlot>().itemName
                            && HotbarManager.Instance.hotbarList[k].itemID == itemSlotList_Player[j].GetComponent<ItemSlot>().itemID)
                        {
                            itemSlotList_Player[j].GetComponent<ItemSlot>().ActivateHotbarInfoToItemSlot(k + 1);

                            //Go to the next item in the inventoryList, skipping the rest of the itemSlots for this item
                            j = itemSlotList_Player.Count;
                            break;
                        }
                    }
                }
            }
        }
        
    }
    public void SelectItemInfoToHotbar(int hotbarSlot, Items itemName, int ID)
    {
        //Assign HotbarInfo to the correct ItemSlot
        for (int i = 0; i < itemSlotList_Player.Count; i++)
        {
            if (itemSlotList_Player[i].GetComponent<ItemSlot>().itemName == itemName
                && itemSlotList_Player[i].GetComponent<ItemSlot>().itemID == ID)
            {
                itemSlotList_Player[i].GetComponent<ItemSlot>().ActivateHotbarInfoToItemSlot(hotbarSlot + 1);

                return;
            }
        }
    }
    public void DeselectItemInfoToHotbar()
    {
        //Hide HotbarInfo from the correct ItemSlot
        for (int i = 0; i < itemSlotList_Player.Count; i++)
        {
            itemSlotList_Player[i].GetComponent<ItemSlot>().DeactivateHotbarInfoToItemSlot();
        }
    }
    public void DeselectItemInfoToHotbar(Items itemName, int ID)
    {
        //Hide HotbarInfo from the correct ItemSlot
        for (int i = 0; i < itemSlotList_Player.Count; i++)
        {
            if (itemSlotList_Player[i].GetComponent<ItemSlot>().itemName == itemName
                && itemSlotList_Player[i].GetComponent<ItemSlot>().itemID == ID)
            {
                itemSlotList_Player[i].GetComponent<ItemSlot>().DeactivateHotbarInfoToItemSlot();

                return;
            }
        }
    }
    #endregion


    //--------------------


    #region
    public void SelectItemDurabilityDisplay()
    {
        //Assign HotbarInfo to the correct ItemSlot
        for (int i = inventories[0].itemsInInventory.Count - 1; i >= 0; i--)
        {
            for (int j = itemSlotList_Player.Count - 1; j >= 0; j--)
            {
                //Find relevant item
                if (itemSlotList_Player[j].GetComponent<ItemSlot>().itemName == inventories[0].itemsInInventory[i].itemName
                    && itemSlotList_Player[j].GetComponent<ItemSlot>().itemID == inventories[0].itemsInInventory[i].itemID
                    && MainManager.Instance.GetItem(itemSlotList_Player[j].GetComponent<ItemSlot>().itemName).durability_Max > 0)
                {
                    //If the selected item has a durability
                    itemSlotList_Player[j].GetComponent<ItemSlot>().ActivateDurabilityMeter();

                    j = 0;
                }
            }
        }
    }
    public void DeselectItemDurabilityDisplay()
    {
        //Hide HotbarInfo from the correct ItemSlot
        for (int i = 0; i < itemSlotList_Player.Count; i++)
        {
            itemSlotList_Player[i].GetComponent<ItemSlot>().DeactivateDurabilityMeter();
        }
    }
    #endregion


    //--------------------


    #region
    public void ChangeItemInfoBox(Items itemName, ItemSlot itemSlot)
    {
        //ItemSlot from Player Inventory
        if (itemSlot.inventoryIndex <= 0)
        {
            if (MainManager.Instance.menuStates == MenuStates.ResearchMenu)
            {
                if (MainManager.Instance.GetItem(itemName).isResearched)
                {
                    itemInfo.SetInfo_ResearchableItem(true);
                }
                else
                {
                    itemInfo.SetInfo_ResearchableItem(false);
                }
            }

            else
            {
                if (MainManager.Instance.GetItem(itemName).isEquipableInHand)
                {
                    itemInfo.SetInfo_EquipableHandItem(itemSlot);
                }
                else if (MainManager.Instance.GetItem(itemName).isEquipableClothes)
                {
                    itemInfo.SetInfo_EquipableClothesItem();
                }
                else if (MainManager.Instance.GetItem(itemName).isConsumeable)
                {
                    itemInfo.SetInfo_ConsumableItem();
                }
                else
                {
                    itemInfo.SetInfo_StaticItem();
                }
            }
        }

        //ItemSlot from Chest Inventory
        else
        {
            itemInfo.SetInfo_ChestItem();
        }

        itemInfo_Parent.SetActive(true);
    }
    public void ChangeItemInfoBox(bool isEntering)
    {
        if (isEntering)
        {
            itemInfo.SetInfo_EquippedItem();
        }
        else
        {
            itemInfo.HideInfo_EquippedItem();
        }
        
        itemInfo_Parent.SetActive(true);
    }
    public void DisplayInventoryItemInfo()
    {
        if (player_ItemName_Display.text == ""
            && chest_ItemName_Display.text == "")
        {
            itemInfo_Parent.SetActive(false);
        }
        else
        {
            itemInfo_Parent.SetActive(true);
        }
    }
    #endregion


    //--------------------


    #region HotbarSlots_Quick
    void QuickHotbarSelect_1()
    {
        for (int i = 0; i < itemSlotList_Player.Count; i++)
        {
            if (itemSlotList_Player[i].GetComponent<ItemSlot>())
            {
                if (itemSlotList_Player[i].GetComponent<ItemSlot>().itemID == lastIDToGet)
                {
                    itemSlotList_Player[i].GetComponent<ItemSlot>().AssignItemToHotbar(0);

                    break;
                }
            }
        }
    }
    void QuickHotbarSelect_2()
    {
        for (int i = 0; i < itemSlotList_Player.Count; i++)
        {
            if (itemSlotList_Player[i].GetComponent<ItemSlot>())
            {
                if (itemSlotList_Player[i].GetComponent<ItemSlot>().itemID == lastIDToGet)
                {
                    itemSlotList_Player[i].GetComponent<ItemSlot>().AssignItemToHotbar(1);

                    break;
                }
            }
        }
    }
    void QuickHotbarSelect_3()
    {
        for (int i = 0; i < itemSlotList_Player.Count; i++)
        {
            if (itemSlotList_Player[i].GetComponent<ItemSlot>())
            {
                if (itemSlotList_Player[i].GetComponent<ItemSlot>().itemID == lastIDToGet)
                {
                    itemSlotList_Player[i].GetComponent<ItemSlot>().AssignItemToHotbar(2);

                    break;
                }
            }
        }
    }
    void QuickHotbarSelect_4()
    {
        for (int i = 0; i < itemSlotList_Player.Count; i++)
        {
            if (itemSlotList_Player[i].GetComponent<ItemSlot>())
            {
                if (itemSlotList_Player[i].GetComponent<ItemSlot>().itemID == lastIDToGet)
                {
                    itemSlotList_Player[i].GetComponent<ItemSlot>().AssignItemToHotbar(3);

                    break;
                }
            }
        }
    }
    void QuickHotbarSelect_5()
    {
        for (int i = 0; i < itemSlotList_Player.Count; i++)
        {
            if (itemSlotList_Player[i].GetComponent<ItemSlot>())
            {
                if (itemSlotList_Player[i].GetComponent<ItemSlot>().itemID == lastIDToGet)
                {
                    itemSlotList_Player[i].GetComponent<ItemSlot>().AssignItemToHotbar(4);

                    break;
                }
            }
        }
    }
    #endregion


    //--------------------


    #region InventoryUI
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

        //Set HotbarInfo on inventoryItems
        SelectItemInfoToHotbar();
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

                                    inventoryList[posList[k]].GetComponent<ItemSlot>().durabilityMax = MainManager.Instance.GetItem(inventories[inventory].itemsInInventory[j].itemName).durability_Max;
                                    inventoryList[posList[k]].GetComponent<ItemSlot>().durabilityCurrent = inventories[inventory].itemsInInventory[j].durability_Current;

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
                if (inventories[inventory].itemsInInventory[i].itemName == lastItemToGet
                    && inventories[inventory].itemsInInventory[i].itemID == lastIDToGet)
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
                        SoundManager.Instance.Play_Inventory_InventoryIsFull_Clip();

                        print("attemptd picked up");
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
    #endregion


    //--------------------


    #region Open/Close Inventory Menu
    public void OpenPlayerInventory()
    {
        if (!inventoryIsOpen)
        {
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

        //Hide HotbarInfo on inventoryItems
        DeselectItemInfoToHotbar();

        //Hide Durability on inventoryItems
        DeselectItemDurabilityDisplay();

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
    public int itemID;

    public int durability_Current;
}