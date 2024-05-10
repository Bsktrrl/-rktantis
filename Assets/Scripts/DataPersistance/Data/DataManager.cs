using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public class DataManager : Singleton<DataManager>, IDataPersistance
{
    public GameData gameData;

    public static Action dataIsSaving;
    public static Action datahasLoaded;

    public GameObject loadingMenu_Parent;
    public Image loadingImage;
    public Image loadingImage_Icon;
    public TextMeshProUGUI loadingText;

    public bool hasLoaded;

    bool fading;
    bool hasCompletedFading;
    float fadingNotificationImageValue_Icon;
    float fadingNotificationImageValue = 1;
    bool towardsVisible;


    //--------------------


    #region Variables
    //Player Stats
    [HideInInspector] public PlayerStats playerStats_Store = new PlayerStats();

    //WorldObjects
    [HideInInspector] public List<WorldObject> worldObject_StoreList = new List<WorldObject>();

    //Inventories
    [HideInInspector] public List<Inventory> Inventories_StoreList = new List<Inventory>();
    [HideInInspector] public Vector2 smallChest_Size_Store;
    [HideInInspector] public Vector2 mediumChest_Size_Store;
    [HideInInspector] public Vector2 bigChest_Size_Store;

    //MenuEquipment
    [HideInInspector] public List<Items> menuEquipedItemList_StoreList = new List<Items>();

    //Hotbar
    [HideInInspector] public int selectedSlot_Store = new int();
    [HideInInspector] public List<Hotbar> hotbarItem_StoreList = new List<Hotbar>();

    //BuidingSystem
    [HideInInspector] public List<WorldBuildingObject> worldBuildingObjectInfoList_Store = new List<WorldBuildingObject>();
    [HideInInspector] public ActiveBuildingObject activeBuildingObject_Store;
    [HideInInspector] public List<bool> activeBuildingBlockObject_SOList_Store = new List<bool>();
    [HideInInspector] public List<bool> activeFurnitureObject_SOList_Store = new List<bool>();
    [HideInInspector] public List<bool> activeMachineObject_SOList_Store = new List<bool>();
    [HideInInspector] public List<bool> menuObjects_PlussSign_Store = new List<bool>();

    //Plants
    [HideInInspector] public List<ListOfPlantToSave> plantTypeObjectList_Store = new List<ListOfPlantToSave>();

    //Ores
    [HideInInspector] public List<ListOfOreToSave> oreTypeObjectList_Store = new List<ListOfOreToSave>();

    //Trees
    [HideInInspector] public List<ListOfTreeToSave> treeTypeObjectList_Store = new List<ListOfTreeToSave>();

    //Blueprints
    [HideInInspector] public List<ListOfBlueprintToSave> blueprintTypeObjectList_Store = new List<ListOfBlueprintToSave>();

    //Arídian Objects
    [HideInInspector] public List<ListOfArídianKeyToSave> arídianKeyTypeObjectList_Store = new List<ListOfArídianKeyToSave>();
    [HideInInspector] public List<ListOfAríditeCrystalToSave> aríditeCrystalTypeObjectList_Store = new List<ListOfAríditeCrystalToSave>();

    //HealthParameter
    [HideInInspector] public HealthToSave health_Store = new HealthToSave();

    //Time
    [HideInInspector] public float currentTime_Store = new float();
    [HideInInspector] public int day_Store = new int();

    //Journals
    [HideInInspector] public List<ListOfJournalPageToSave> journalPageTypeObjectList_Store = new List<ListOfJournalPageToSave>();
    [HideInInspector] public List<int> mentorStoryJournalPageIndexList_Store = new List<int>();
    [HideInInspector] public List<int> playerStoryJournalPageIndexList_Store = new List<int>();
    [HideInInspector] public List<int> personalStoryJournalPageIndexList_Store = new List<int>();
    [HideInInspector] public List<bool> journalPage_PlussSign_Mentor_Store = new List<bool>();
    [HideInInspector] public List<bool> journalPage_PlussSign_Player_Store = new List<bool>();
    [HideInInspector] public List<bool> journalPage_PlussSign_Personal_Store = new List<bool>();

    [HideInInspector] public MessagesConditionChecks messagesConditionChecks_Store = new MessagesConditionChecks();

    //Settings
    [HideInInspector] public SettingsValues settingsValues_Store = new SettingsValues();

    //Weather
    [HideInInspector] public List<WeatherType> weatherTypeDayList_Store = new List<WeatherType>();

    //Research
    [HideInInspector] public List<Items> researchedItemsListNames_Store = new List<Items>();
    [HideInInspector] public List<bool> researched_SOItem_Store = new List<bool>();

    //Crafting
    [HideInInspector] public List<CraftingItem> itemStates_Store = new List<CraftingItem>();

    //GhostCapturer
    [HideInInspector] public GhostCapturerStats ghostCapturerStats_Store = new GhostCapturerStats();

    //Machines
    [HideInInspector] public List<GhostTankContent> ghostTankList_Store = new List<GhostTankContent>();

    //Perks
    [HideInInspector] public PerkValues perks_Store = new PerkValues();
    [HideInInspector] public List<bool> perkActivationList_Store = new List<bool>();

    //Connections
    [HideInInspector] public List<ConnectionInfo> connectionInfoList_Store = new List<ConnectionInfo>();

    //CutScenes
    [HideInInspector] public CutScenes cutScenes_Store = new CutScenes();
    #endregion


    //--------------------


    private void Start()
    {
        if (SceneManager.GetActiveScene().name != "Landscape")
        {
            Destroy(gameObject);
        }
        else
        {
            fadingNotificationImageValue = 1;

            loadingMenu_Parent.SetActive(true);
        }
    }
    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "Landscape") { return; }

        //When loading
        if (!hasLoaded)
        {
            //loadingMenu_Parent.SetActive(true);

            //Fading In/Out Icon
            FadingInOutIcon();

            print("1. Loading");
        }

        //After loading
        if (fading)
        {
            print("2. Fading");

            //Fading the screen
            FadingOutLoadingScreen();
        }
    }


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
        this.mediumChest_Size_Store = gameData.mediumChest_Size_Save;
        this.bigChest_Size_Store = gameData.bigChest_Size_Save;

        this.hotbarItem_StoreList = gameData.hotbarItem_SaveList;
        this.selectedSlot_Store = gameData.selectedSlot_Save;

        this.worldBuildingObjectInfoList_Store = gameData.worldBuildingObjectInfoList_Save;
        this.activeBuildingObject_Store = gameData.activeBuildingObject_Save;
        this.activeBuildingBlockObject_SOList_Store = gameData.activeBuildingBlockObject_SOList_Save;
        this.activeFurnitureObject_SOList_Store = gameData.activeFurnitureObject_SOList_Save;
        this.activeMachineObject_SOList_Store = gameData.activeMachineObject_SOList_Save;
        this.menuObjects_PlussSign_Store = gameData.menuObjects_PlussSign_Save;

        this.health_Store = gameData.health_Save;

        this.currentTime_Store = gameData.currentTime_Save;
        this.day_Store = gameData.day_Save;

        this.plantTypeObjectList_Store = gameData.plantTypeObjectList_Save;
        this.oreTypeObjectList_Store = gameData.oreTypeObjectList_Save;
        this.treeTypeObjectList_Store = gameData.treeTypeObjectList_Save;
        this.blueprintTypeObjectList_Store = gameData.blueprintTypeObjectList_Save;
        this.arídianKeyTypeObjectList_Store = gameData.arídianKeyTypeObjectList_Save;
        this.aríditeCrystalTypeObjectList_Store = gameData.aríditeCrystalTypeObjectList_Save;

        this.journalPageTypeObjectList_Store = gameData.journalPageTypeObjectList_Save;
        this.mentorStoryJournalPageIndexList_Store = gameData.mentorStoryJournalPageIndexList_Save;
        this.playerStoryJournalPageIndexList_Store = gameData.playerStoryJournalPageIndexList_Save;
        this.personalStoryJournalPageIndexList_Store = gameData.personalStoryJournalPageIndexList_Save;
        this.journalPage_PlussSign_Mentor_Store = gameData.journalPage_PlussSign_Mentor_Save;
        this.journalPage_PlussSign_Player_Store = gameData.journalPage_PlussSign_Player_Save;
        this.journalPage_PlussSign_Personal_Store = gameData.journalPage_PlussSign_Personal_Save;

        this.messagesConditionChecks_Store = gameData.messagesConditionChecks_Save;

        this.settingsValues_Store = gameData.settingsValues_Save;

        this.weatherTypeDayList_Store = gameData.weatherTypeDayList_Save;

        this.researchedItemsListNames_Store = gameData.researchedItemsListNames_Save;
        this.researched_SOItem_Store = gameData.researched_SOItem_Save;

        this.itemStates_Store = gameData.itemStates_Save;

        this.ghostCapturerStats_Store = gameData.ghostCapturerStats_Save;

        this.ghostTankList_Store = gameData.ghostTankList_Save;

        this.perks_Store = gameData.perks_Save;
        this.perkActivationList_Store = gameData.perkActivationList_Save;

        this.connectionInfoList_Store = gameData.connectionInfoList_Save;

        this.cutScenes_Store = gameData.cutScenes_Save;
        #endregion

        //Load the saved data into the project
        #region
        //datahasLoaded?.Invoke();
        print("0. Data has Loaded");

        SettingsManager.Instance.LoadData();
        print("1. SettingsManager has Loaded");

        PerkManager.Instance.LoadData();
        print("2. PerkValues has Loaded");

        SkillTreeManager.Instance.LoadData();
        print("3. SkillTreeManager has Loaded");

        PlayerManager.Instance.LoadData();
        print("4. PlayerManager has Loaded");

        InventoryManager.Instance.LoadData();
        print("5. InventoryManager has Loaded");

        MenuEquipmentManager.Instance.LoadData();
        print("6. MenuEquipmentManager has Loaded");

        HotbarManager.Instance.LoadData();
        print("7. HotbarManager has Loaded");

        BuildingSystemManager.Instance.LoadData();
        print("8. BuildingSystemManager has Loaded");

        BuildingDisplayManager.Instance.LoadData();
        print("9. BuildingDisplayManager has Loaded");

        WorldObjectManager.Instance.LoadData();
        print("10. WorldObjectManager has Loaded");

        BlueprintManager.Instance.LoadData();
        print("11. BlueprintManager has Loaded");

        HealthManager.Instance.LoadData();
        print("12. HealthManager has Loaded");

        TimeManager.Instance.LoadData();
        print("13. TimeManager has Loaded");

        PlantManager.Instance.LoadData();
        print("14. Plants has Loaded");

        OreManager.Instance.LoadData();
        print("15. Ores has Loaded");

        TreeManager.Instance.LoadData();
        print("16. Trees has Loaded");

        JournalManager.Instance.LoadData();
        print("17. Journals has Loaded");

        JournalMessagesManager.Instance.LoadData();
        print("18. JournalMessagesManager has Loaded");

        WeatherManager.Instance.LoadData();
        print("19. Weather has Loaded");

        ResearchManager.Instance.LoadData(this.researched_SOItem_Store);
        print("20. Research has Loaded");

        CraftingManager.Instance.LoadData();
        print("21. Crafting has Loaded");

        GhostManager.Instance.LoadData();
        print("22. Ghost has Loaded");

        ArídianKeyManager.Instance.LoadData();
        print("23. AríditeKeyManager has Loaded");

        AríditeCrystalManager.Instance.LoadData();
        print("24. AríditeCrystalManager has Loaded");

        ConnectionPointManager.Instance.LoadData();
        print("25. ConnectionPointManager has Loaded");

        CutSceneManager.Instance.LoadData();
        print("26. CutSceneManager has Loaded");
        #endregion

        StartCoroutine(LoadingDelay(1.5f));
    }
    IEnumerator LoadingDelay(float time)
    {
        yield return new WaitForSeconds(time);

        print("------------------------------");

        fading = true;
        hasLoaded = true;
    }

    public void SaveData(ref GameData gameData)
    {
        dataIsSaving?.Invoke();

        //Input what to save
        gameData.playerStats_Save = this.playerStats_Store;

        gameData.worldObject_SaveList = this.worldObject_StoreList;

        gameData.Inventories_SaveList = this.Inventories_StoreList;
        gameData.smallChest_Size_Save = this.smallChest_Size_Store;
        gameData.mediumChest_Size_Save = this.mediumChest_Size_Store;
        gameData.bigChest_Size_Save = this.bigChest_Size_Store;

        gameData.menuEquipedItemList_SaveList = this.menuEquipedItemList_StoreList;

        gameData.hotbarItem_SaveList = this.hotbarItem_StoreList;
        gameData.selectedSlot_Save = this.selectedSlot_Store;

        gameData.worldBuildingObjectInfoList_Save = this.worldBuildingObjectInfoList_Store;
        gameData.activeBuildingObject_Save = this.activeBuildingObject_Store;
        gameData.activeBuildingBlockObject_SOList_Save = this.activeBuildingBlockObject_SOList_Store;
        gameData.activeFurnitureObject_SOList_Save = this.activeFurnitureObject_SOList_Store;
        gameData.activeMachineObject_SOList_Save = this.activeMachineObject_SOList_Store;
        gameData.menuObjects_PlussSign_Save = this.menuObjects_PlussSign_Store;

        gameData.health_Save = this.health_Store;

        gameData.currentTime_Save = this.currentTime_Store;
        gameData.day_Save = this.day_Store;

        gameData.plantTypeObjectList_Save = this.plantTypeObjectList_Store;
        gameData.oreTypeObjectList_Save = this.oreTypeObjectList_Store;
        gameData.treeTypeObjectList_Save = this.treeTypeObjectList_Store;
        gameData.blueprintTypeObjectList_Save = this.blueprintTypeObjectList_Store;
        gameData.arídianKeyTypeObjectList_Save = this.arídianKeyTypeObjectList_Store;
        gameData.aríditeCrystalTypeObjectList_Save = this.aríditeCrystalTypeObjectList_Store;

        gameData.journalPageTypeObjectList_Save = this.journalPageTypeObjectList_Store;
        gameData.mentorStoryJournalPageIndexList_Save = this.mentorStoryJournalPageIndexList_Store;
        gameData.playerStoryJournalPageIndexList_Save = this.playerStoryJournalPageIndexList_Store;
        gameData.personalStoryJournalPageIndexList_Save = this.personalStoryJournalPageIndexList_Store;
        gameData.journalPage_PlussSign_Mentor_Save = this.journalPage_PlussSign_Mentor_Store;
        gameData.journalPage_PlussSign_Player_Save = this.journalPage_PlussSign_Player_Store;
        gameData.journalPage_PlussSign_Personal_Save = this.journalPage_PlussSign_Personal_Store;

        gameData.messagesConditionChecks_Save = this.messagesConditionChecks_Store;

        gameData.settingsValues_Save = this.settingsValues_Store;

        gameData.weatherTypeDayList_Save = this.weatherTypeDayList_Store;

        gameData.researchedItemsListNames_Save = this.researchedItemsListNames_Store;
        gameData.researched_SOItem_Save = this.researched_SOItem_Store;

        gameData.itemStates_Save = this.itemStates_Store;

        gameData.ghostCapturerStats_Save = this.ghostCapturerStats_Store;

        gameData.ghostTankList_Save = this.ghostTankList_Store;

        gameData.perks_Save = this.perks_Store;
        gameData.perkActivationList_Save = this.perkActivationList_Store;

        gameData.connectionInfoList_Save = this.connectionInfoList_Store;

        gameData.cutScenes_Save = this.cutScenes_Store;

        print("Data has Saved");
    }



    //--------------------


    #region LoadingScreen
    void FadingInOutIcon()
    {
        //Change Visible Value
        if (towardsVisible)
        {
            fadingNotificationImageValue_Icon += Time.deltaTime;
        }
        else
        {
            fadingNotificationImageValue_Icon -= Time.deltaTime;
        }

        //Set if Visibility is going Up or Down
        if (fadingNotificationImageValue_Icon >= 1 && towardsVisible)
        {
            towardsVisible = false;
        }
        else if (fadingNotificationImageValue_Icon <= 0 && !towardsVisible)
        {
            towardsVisible = true;
        }

        //Change Visibility
        loadingImage_Icon.color = new Color(1, 1, 1, fadingNotificationImageValue_Icon);
    }
    void FadingOutLoadingScreen()
    {
        fadingNotificationImageValue -= Time.deltaTime * 2;

        loadingImage.color = new Color(loadingImage.color.r, loadingImage.color.g, loadingImage.color.b, fadingNotificationImageValue);
        loadingImage_Icon.color = new Color(loadingImage_Icon.color.r, loadingImage_Icon.color.g, loadingImage_Icon.color.b, fadingNotificationImageValue * fadingNotificationImageValue_Icon);
        loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, fadingNotificationImageValue);

        if (fadingNotificationImageValue <= 0)
        {
            fading = false;

            loadingMenu_Parent.SetActive(false);
        }
    }
    #endregion
}