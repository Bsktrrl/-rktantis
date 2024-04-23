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
    [HideInInspector] public List<WorldBuildingObject> worldBuildingObjectInfoList_Save = new List<WorldBuildingObject>();
    [HideInInspector] public ActiveBuildingObject activeBuildingObject_Save = new ActiveBuildingObject();
    [HideInInspector] public List<bool> activeBuildingBlockObject_SOList_Save = new List<bool>();
    [HideInInspector] public List<bool> activeFurnitureObject_SOList_Save = new List<bool>();
    [HideInInspector] public List<bool> activeMachineObject_SOList_Save = new List<bool>();
    [HideInInspector] public List<bool> menuObjects_PlussSign_Save = new List<bool>();

    //Hotbar
    [HideInInspector] public int selectedSlot_Save = new int();
    [HideInInspector] public List<Hotbar> hotbarItem_SaveList = new List<Hotbar>();

    //Plants
    [HideInInspector] public List<ListOfPlantToSave> plantTypeObjectList_Save = new List<ListOfPlantToSave>();

    //Ores
    [HideInInspector] public List<ListOfOreToSave> oreTypeObjectList_Save = new List<ListOfOreToSave>();

    //Trees
    [HideInInspector] public List<ListOfTreeToSave> treeTypeObjectList_Save = new List<ListOfTreeToSave>();

    //Blueprints
    [HideInInspector] public List<ListOfBlueprintToSave> blueprintTypeObjectList_Save = new List<ListOfBlueprintToSave>();

    //Arídian Objects
    [HideInInspector] public List<ListOfArídianKeyToSave> arídianKeyTypeObjectList_Save= new List<ListOfArídianKeyToSave>();
    [HideInInspector] public List<ListOfAríditeCrystalToSave> aríditeCrystalTypeObjectList_Save = new List<ListOfAríditeCrystalToSave>();

    //HealthParameter
    [HideInInspector] public HealthToSave health_Save = new HealthToSave();

    //Time
    [HideInInspector] public float currentTime_Save = new float();
    [HideInInspector] public int day_Save = new int();

    //Journals
    [HideInInspector] public List<ListOfJournalPageToSave> journalPageTypeObjectList_Save = new List<ListOfJournalPageToSave>();
    [HideInInspector] public List<int> mentorStoryJournalPageIndexList_Save = new List<int>();
    [HideInInspector] public List<int> playerStoryJournalPageIndexList_Save = new List<int>();
    [HideInInspector] public List<int> personalStoryJournalPageIndexList_Save = new List<int>();
    [HideInInspector] public List<bool> journalPage_PlussSign_Mentor_Save = new List<bool>();
    [HideInInspector] public List<bool> journalPage_PlussSign_Player_Save = new List<bool>();
    [HideInInspector] public List<bool> journalPage_PlussSign_Personal_Save = new List<bool>();

    [HideInInspector] public MessagesConditionChecks messagesConditionChecks_Save = new MessagesConditionChecks();

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
    [HideInInspector] public GhostCapturerStats ghostCapturerStats_Save = new GhostCapturerStats();

    //Machines
    [HideInInspector] public List<GhostTankContent> ghostTankList_Save = new List<GhostTankContent>();

    //Perks
    [HideInInspector] public Perks perks_Save = new Perks();
    [HideInInspector] public PerkActivations perkActivations_Save = new PerkActivations();


    //--------------------


    public GameData()
    {
        //Input All Lists to clear
        this.worldObject_SaveList.Clear(); 

        this.activeBuildingBlockObject_SOList_Save.Clear();
        this.activeFurnitureObject_SOList_Save.Clear();
        this.activeMachineObject_SOList_Save.Clear();
        this.menuObjects_PlussSign_Save.Clear();

        this.Inventories_SaveList.Clear();
        this.hotbarItem_SaveList.Clear();

        this.menuEquipedItemList_SaveList.Clear();

        this.plantTypeObjectList_Save.Clear();
        this.oreTypeObjectList_Save.Clear();
        this.treeTypeObjectList_Save.Clear();
        this.blueprintTypeObjectList_Save.Clear();
        this.arídianKeyTypeObjectList_Save.Clear();
        this.aríditeCrystalTypeObjectList_Save.Clear();

        this.journalPageTypeObjectList_Save.Clear();
        this.mentorStoryJournalPageIndexList_Save.Clear();
        this.playerStoryJournalPageIndexList_Save.Clear();
        this.personalStoryJournalPageIndexList_Save.Clear();
        this.journalPage_PlussSign_Mentor_Save.Clear();
        this.journalPage_PlussSign_Player_Save.Clear();
        this.journalPage_PlussSign_Personal_Save.Clear();

        this.weatherTypeDayList_Save.Clear();

        this.researchedItemsListNames_Save.Clear();
        this.researched_SOItem_Save.Clear();

        this.ghostTankList_Save.Clear();

        this.itemStates_Save.Clear();
    }
}
