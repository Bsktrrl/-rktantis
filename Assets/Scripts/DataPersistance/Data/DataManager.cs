using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

[Serializable]
public class DataManager : Singleton<DataManager>, IDataPersistance
{
    public static Action dataIsSaving;
    public static Action datahasLoaded;


    //--------------------


    //Player Stats
    [HideInInspector] public PlayerStats playerStats_Store = new PlayerStats();

    //Player Pos and Rotation
    //[HideInInspector] public Vector3 playerPos_Store = new Vector3();
    //[HideInInspector] public Quaternion playerRot_Store = new Quaternion();

    //WorldObjects
    [HideInInspector] public List<WorldObject> worldObject_StoreList = new List<WorldObject>();

    //Inventories
    [HideInInspector] public List<Inventory> Inventories_StoreList = new List<Inventory>();
    [HideInInspector] public Vector2 smallChest_Size_Store;
    [HideInInspector] public Vector2 bigChest_Size_Store;

    //MenuEquipment
    [HideInInspector] public List<Items> menuEquipedItemList_StoreList = new List<Items>();

    //Hotbar
    [HideInInspector] public int selectedSlot_Store = new int();
    [HideInInspector] public List<Hotbar> hotbarItem_StoreList = new List<Hotbar>();

    //BuidingSystem
    [HideInInspector] public List<BuildingBlockSaveList> buildingBlockList_StoreList = new List<BuildingBlockSaveList>();

    //MoveableObjects
    [HideInInspector] public List<MoveableObject_ToSave> placedMoveableObjectsList_StoreList = new List<MoveableObject_ToSave>();
    [HideInInspector] public MoveableObjectSelected_ToSave moveableObjectSelected_Store = new MoveableObjectSelected_ToSave();

    //Plants
    [HideInInspector] public List<ListOfPlantToSave> plantTypeObjectList_Store = new List<ListOfPlantToSave>();

    //Ores
    [HideInInspector] public List<ListOfOreToSave> oreTypeObjectList_Store = new List<ListOfOreToSave>();

    //Trees
    [HideInInspector] public List<ListOfTreeToSave> treeTypeObjectList_Store = new List<ListOfTreeToSave>();

    //HealthParameter
    [HideInInspector] public HealthToSave health_Store = new HealthToSave();

    //Time
    [HideInInspector] public float currentTime_Store = new float();
    [HideInInspector] public int day_Store = new int();

    //Journals
    [HideInInspector] public List<int> mentorStoryJournalPageIndexList_Store = new List<int>();
    [HideInInspector] public List<int> playerStoryJournalPageIndexList_Store = new List<int>();
    [HideInInspector] public List<int> personalStoryJournalPageIndexList_Store = new List<int>();

    //Settings
    [HideInInspector] public SettingsValues settingsValues_Store = new SettingsValues();

    //Weather
    [HideInInspector] public List<WeatherType> weatherTypeDayList_Store = new List<WeatherType>();

    //Research
    [HideInInspector] public List<Items> researchedItemsListNames_Store = new List<Items>();
    [HideInInspector] public List<bool> researched_SOItem_Store = new List<bool>();

    //Crafting
    [HideInInspector] public List<CraftingItem> itemStates_Store = new List<CraftingItem>();

    //Player Movement


    //--------------------


    public void LoadData(GameData gameData)
    {
        //Get saved data from file to be loaded into the project
        #region
        this.playerStats_Store = gameData.playerStats_Save;

        this.worldObject_StoreList = gameData.worldObject_SaveList;

        this.Inventories_StoreList = gameData.Inventories_SaveList;

        this.menuEquipedItemList_StoreList = gameData.menuEquipedItemList_SaveList;

        this.smallChest_Size_Store = gameData.smallChest_Size_Save;
        this.bigChest_Size_Store = gameData.bigChest_Size_Save;

        this.hotbarItem_StoreList = gameData.hotbarItem_SaveList;
        this.selectedSlot_Store = gameData.selectedSlot_Save;

        this.buildingBlockList_StoreList = gameData.buildingBlockList_SaveList;

        this.placedMoveableObjectsList_StoreList = gameData.placedMoveableObjectsList_SaveList;
        this.moveableObjectSelected_Store = gameData.moveableObjectSelected_Save;

        this.health_Store = gameData.health_Save;

        this.currentTime_Store = gameData.currentTime_Save;
        this.day_Store = gameData.day_Save;

        this.plantTypeObjectList_Store = gameData.plantTypeObjectList_Save;
        this.oreTypeObjectList_Store = gameData.oreTypeObjectList_Save;
        this.treeTypeObjectList_Store = gameData.treeTypeObjectList_Save;

        this.mentorStoryJournalPageIndexList_Store = gameData.mentorStoryJournalPageIndexList_Save;
        this.playerStoryJournalPageIndexList_Store = gameData.playerStoryJournalPageIndexList_Save;
        this.personalStoryJournalPageIndexList_Store = gameData.personalStoryJournalPageIndexList_Save;

        this.settingsValues_Store = gameData.settingsValues_Save;

        this.weatherTypeDayList_Store = gameData.weatherTypeDayList_Save;

        this.researchedItemsListNames_Store = gameData.researchedItemsListNames_Save;
        this.researched_SOItem_Store = gameData.researched_SOItem_Save;

        this.itemStates_Store = gameData.itemStates_Save;
        #endregion

        //Load the saved data into the project
        #region
        //datahasLoaded?.Invoke();
        print("0. Data has Loaded");

        SettingsManager.Instance.LoadData();
        print("1. SettingsManager has Loaded");

        PlayerManager.Instance.LoadData();
        print("2. PlayerManager has Loaded");

        InventoryManager.Instance.LoadData();
        print("3. InventoryManager has Loaded");

        MenuEquipmentManager.Instance.LoadData();
        print("4. MenuEquipmentManager has Loaded");
        
        BuildingManager.Instance.LoadData();
        print("5. BuildingManager has Loaded");

        HotbarManager.Instance.LoadData();
        print("6. HotbarManager has Loaded");

        WorldObjectManager.Instance.LoadData();
        print("7. WorldObjectManager has Loaded");

        MoveableObjectManager.Instance.LoadData();
        print("8. MoveableObjectManager has Loaded");

        HealthManager.Instance.LoadData();
        print("9. HealthManager has Loaded");

        TimeManager.Instance.LoadData();
        print("10. TimeManager has Loaded");

        PlantManager.Instance.LoadData();
        print("11. Plants has Loaded");

        OreManager.Instance.LoadData();
        print("12. Ores has Loaded");

        TreeManager.Instance.LoadData();
        print("13. Trees has Loaded");

        JournalManager.Instance.LoadData();
        print("14. Journals has Loaded");

        WeatherManager.Instance.LoadData();
        print("15. Weather has Loaded");

        ResearchManager.Instance.LoadData(this.researched_SOItem_Store);
        print("16. Research has Loaded");

        CraftingManager.Instance.LoadData();
        print("17. Crafting has Loaded");
        #endregion

        print("------------------------------");
    }

    public void SaveData(ref GameData gameData)
    {
        dataIsSaving?.Invoke();

        //Input what to save
        gameData.playerStats_Save = this.playerStats_Store;

        gameData.worldObject_SaveList = this.worldObject_StoreList;

        gameData.Inventories_SaveList = this.Inventories_StoreList;
        gameData.smallChest_Size_Save = this.smallChest_Size_Store;
        gameData.bigChest_Size_Save = this.bigChest_Size_Store;

        gameData.menuEquipedItemList_SaveList = this.menuEquipedItemList_StoreList;

        gameData.hotbarItem_SaveList = this.hotbarItem_StoreList;
        gameData.selectedSlot_Save = this.selectedSlot_Store;

        gameData.buildingBlockList_SaveList = this.buildingBlockList_StoreList;

        gameData.placedMoveableObjectsList_SaveList = this.placedMoveableObjectsList_StoreList;
        gameData.moveableObjectSelected_Save = this.moveableObjectSelected_Store;

        gameData.health_Save = this.health_Store;

        gameData.currentTime_Save = this.currentTime_Store;
        gameData.day_Save = this.day_Store;

        gameData.plantTypeObjectList_Save = this.plantTypeObjectList_Store;
        gameData.oreTypeObjectList_Save = this.oreTypeObjectList_Store;
        gameData.treeTypeObjectList_Save = this.treeTypeObjectList_Store;

        gameData.mentorStoryJournalPageIndexList_Save = this.mentorStoryJournalPageIndexList_Store;
        gameData.playerStoryJournalPageIndexList_Save = this.playerStoryJournalPageIndexList_Store;
        gameData.personalStoryJournalPageIndexList_Save = this.personalStoryJournalPageIndexList_Store;

        gameData.settingsValues_Save = this.settingsValues_Store;

        gameData.weatherTypeDayList_Save = this.weatherTypeDayList_Store;

        gameData.researchedItemsListNames_Save = this.researchedItemsListNames_Store;
        gameData.researched_SOItem_Save = this.researched_SOItem_Store;

        gameData.itemStates_Save = this.itemStates_Store;

        print("Data has Saved");
    }
}