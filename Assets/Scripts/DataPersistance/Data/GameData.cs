using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    //Player Stats
    [HideInInspector] public PlayerStats playerStats_Save = new PlayerStats();

    //Inventories
    [HideInInspector] public List<Inventory> Inventories_SaveList = new List<Inventory>();
    [HideInInspector] public Vector2 smallChest_Size_Save;
    [HideInInspector] public Vector2 bigChest_Size_Save;

    //MenuEquipment
    [HideInInspector] public List<Items> menuEquipedItemList_SaveList = new List<Items>();

    //WorldObjects
    [HideInInspector] public List<WorldObject> worldObject_SaveList = new List<WorldObject>();

    //BuidingSystem
    [HideInInspector] public ActiveBuildingObject activeBuildingObject_Save;
    //[HideInInspector] public List<BuildingBlockSaveList> buildingBlockList_SaveList = new List<BuildingBlockSaveList>();

    //Hotbar
    [HideInInspector] public int selectedSlot_Save = new int();
    [HideInInspector] public List<Hotbar> hotbarItem_SaveList = new List<Hotbar>();

    //MoveableObjects
    //[HideInInspector] public List<MoveableObject_ToSave> placedMoveableObjectsList_SaveList = new List<MoveableObject_ToSave>();
    //[HideInInspector] public MoveableObjectSelected_ToSave moveableObjectSelected_Save = new MoveableObjectSelected_ToSave();

    //Plants
    [HideInInspector] public List<ListOfPlantToSave> plantTypeObjectList_Save = new List<ListOfPlantToSave>();

    //Ores
    [HideInInspector] public List<ListOfOreToSave> oreTypeObjectList_Save = new List<ListOfOreToSave>();

    //Trees
    [HideInInspector] public List<ListOfTreeToSave> treeTypeObjectList_Save = new List<ListOfTreeToSave>();

    //HealthParameter
    [HideInInspector] public HealthToSave health_Save = new HealthToSave();

    //Time
    [HideInInspector] public float currentTime_Save = new float();
    [HideInInspector] public int day_Save = new int();

    //Journals
    [HideInInspector] public List<int> mentorStoryJournalPageIndexList_Save = new List<int>();
    [HideInInspector] public List<int> playerStoryJournalPageIndexList_Save = new List<int>();
    [HideInInspector] public List<int> personalStoryJournalPageIndexList_Save = new List<int>();

    //Settings
    [HideInInspector] public SettingsValues settingsValues_Save = new SettingsValues();

    //Weather
    [HideInInspector] public List<WeatherType> weatherTypeDayList_Save = new List<WeatherType>();

    //Research
    [HideInInspector] public List<Items> researchedItemsListNames_Save = new List<Items>();
    [HideInInspector] public List<bool> researched_SOItem_Save = new List<bool>();

    //Crafting
    [HideInInspector] public List<CraftingItem> itemStates_Save = new List<CraftingItem>();

    //GhostCapturer
    [HideInInspector] public GhostCapturerStats ghostCapturerStats_Save;

    //Machines
    [HideInInspector] public List<GhostTankContent> ghostTankList_Save = new List<GhostTankContent>();

    //Perks
    [HideInInspector] public Perks perks_Save;
    [HideInInspector] public PerkActivations perkActivations_Save;


    //--------------------


    public GameData()
    {
        //Input All Lists to clear
        this.worldObject_SaveList.Clear();

        //this.buildingBlockList_SaveList.Clear();

        this.Inventories_SaveList.Clear();
        this.hotbarItem_SaveList.Clear();
        //this.placedMoveableObjectsList_SaveList.Clear();

        this.menuEquipedItemList_SaveList.Clear();

        this.plantTypeObjectList_Save.Clear();
        this.oreTypeObjectList_Save.Clear();
        this.treeTypeObjectList_Save.Clear();

        this.mentorStoryJournalPageIndexList_Save.Clear();
        this.playerStoryJournalPageIndexList_Save.Clear();
        this.personalStoryJournalPageIndexList_Save.Clear();

        this.weatherTypeDayList_Save.Clear();

        this.researchedItemsListNames_Save.Clear();
        this.researched_SOItem_Save.Clear();

        this.ghostTankList_Save.Clear();

        this.itemStates_Save.Clear();
    }
}
