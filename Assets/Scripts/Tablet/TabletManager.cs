using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TabletManager : Singleton<TabletManager>
{
    #region Main Tablet Variables
    [Header("General")]
    public TabletMenuState tabletMenuState;
    public TabletMenuState tabletMenuState_Save;
    public ObjectInteractingWith objectInteractingWith;
    public GameObject objectInteractingWith_Object;

    [Header("Menus")]
    public int menuAmount = 3;
    int tempMenuAmount;
    bool menuObjectIsOpened;

    [SerializeField] GameObject menu_Inventory;
    public GameObject playerInventory_MainParent;
    public GameObject chestInventory_MainParent;
    public GameObject equipInventory_MainParent;
    public GameObject playerInventory_Parent;
    public GameObject chestInventory_Parent;
    public GameObject journal_Parent;
    public GameObject settings_Parent;

    [SerializeField] GameObject menu_CraftingTable;
    [SerializeField] GameObject menu_ResearchTable;
    [SerializeField] GameObject menu_Skilltree;
    [SerializeField] GameObject menu_MoveableObjects;
    [SerializeField] GameObject menu_CropPlot;

    [Header("Buttons")]
    [SerializeField] GameObject menu_Inventory_Button;
    [SerializeField] GameObject menu_Inventory_PlussIcon;
    [SerializeField] GameObject menu_CraftingTable_Button;
    [SerializeField] GameObject menu_CraftingTable_PlussIcon;
    [SerializeField] GameObject menu_ResearchTable_Button;
    [SerializeField] GameObject menu_ResearchTable_PlussIcon;
    [SerializeField] GameObject menu_Skilltree_Button;
    [SerializeField] GameObject menu_Skilltree_PlussIcon;
    [SerializeField] GameObject menu_MoveableObjects_Button;
    [SerializeField] GameObject menu_MoveableObjects_PlussIcon;
    [SerializeField] GameObject menu_Journal_Button;
    [SerializeField] GameObject menu_Journal_PlussIcon;
    [SerializeField] GameObject menu_Settings_Button;
    [SerializeField] GameObject menu_Settings_PlussIcon;

    [SerializeField] GameObject menu_CropPlot_Button;

    [SerializeField] GameObject menu_Equipment_Button;
    [SerializeField] GameObject menu_Chest_Button;

    [Header("Button Images")]
    public Sprite menuButton_Passive;
    public Sprite menuButton_Active;
    public Sprite squareButton_Passive;
    public Sprite squareButton_Active;

    [Header("Tablet")]
    [SerializeField] GameObject tablet_Parent;
    [SerializeField] GameObject tablet_BG;
    [SerializeField] GameObject menuButton_Background;

    [Header("Health Parameters")]
    [SerializeField] GameObject healthParameters_Tablet_Parent;
    [SerializeField] GameObject healthParameters_PlayerScreen_Parent;

    [SerializeField] Image hunger_Image;
    [SerializeField] Image hungerIcon_Image;
    [SerializeField] List<GameObject> hungerValueMultiplier_Image = new List<GameObject>();

    [SerializeField] Image heatResistance_Image;
    [SerializeField] Image heatResistanceIcon_Image;
    [SerializeField] List<GameObject> heatResistanceValueMultiplier_Image = new List<GameObject>();

    [SerializeField] Image thirst_Image;
    [SerializeField] Image thirstIcon_Image;
    [SerializeField] List<GameObject> thirstValueMultiplier_Image = new List<GameObject>();

    [SerializeField] Image mainHealth_Image;
    [SerializeField] Image mainHealthIcon_Image;
    [SerializeField] List<GameObject> mainHealthValueMultiplier_Image = new List<GameObject>();

    [Header("Timer")]
    [SerializeField] TextMeshProUGUI clockText;
    [SerializeField] TextMeshProUGUI dayText;

    [Header("Temperature")]
    [SerializeField] TextMeshProUGUI temperatureDisplay;
    [SerializeField] TextMeshProUGUI playerTemperatureDisplay;

    [Header("TemperatureDisplay")]
    public GameObject termostatDisplay_Parent;
    [SerializeField] Image termostat_Image;
    [SerializeField] Image pointer_Image;

    [Header("WeatherDisplay")]
    public GameObject weatherDisplay_Parent;
    public GameObject weatherImageDisplay_Day1_Parent;
    [SerializeField] Image weatherImage_Day1;

    [Header("Hotbar")]
    [SerializeField] List<Image> hotbarFrameImageList_Tablet = new List<Image>();
    [SerializeField] List<Image> hotbarIconImageList_Tablet = new List<Image>();
    [SerializeField] List<GameObject> hotbarItemDurabilityListParent_Tablet = new List<GameObject>();
    [SerializeField] List<Image> hotbarItemDurabilityList_Tablet = new List<Image>();

    [Header("TabletObject")]
    public GameObject tabletObject;
    #endregion
    #region SkillTree Menu Variables
    [Header("SkillTree Menu")]
    public GameObject skillTree_Inventory_Parent;
    public GameObject skillTree_Equipment_Parent;
    public GameObject skillTree_GhostCapture_Parent;
    public GameObject skillTree_CrystalLight_Parent;

    public GameObject skillTree_Inventory_Button;
    public GameObject skillTree_Equipment_Button;
    public GameObject skillTree_GhostCapture_Button;
    public GameObject skillTree_CrystalLight_Button;
    #endregion


    //--------------------


    private void Start()
    {
        PlayerButtonManager.OpenPlayerInventory_isPressedDown += OpenTablet;
        PlayerButtonManager.ClosePlayerInventory_isPressedDown += CloseTablet;

        menuAmount = 3;

        tablet_Parent.SetActive(false);
        tabletObject.SetActive(false);
        healthParameters_PlayerScreen_Parent.SetActive(true);
    }
    private void Update()
    {
        if (!DataManager.Instance.hasLoaded) { return; }
        if (PauseGameManager.Instance.GetPause()) { return; }
        if (MainManager.Instance.gameStates == GameStates.GameOver) { return; }


        if (tablet_Parent.activeInHierarchy)
        {
            SetPlussIcons();

            //Set Health Parameter Display
            HealthManager.Instance.SetHealthDisplay(hunger_Image, hungerValueMultiplier_Image,
                                 heatResistance_Image, heatResistanceValueMultiplier_Image,
                                 thirst_Image, thirstValueMultiplier_Image,
                                 mainHealth_Image);

            //Set Health Parameter Arrows
            HealthManager.Instance.SetMainHealthArrowDisplay(mainHealthValueMultiplier_Image);

            //Set Health Icon colors
            hungerIcon_Image.color = HealthManager.Instance.hungerIcon_Image.color;
            heatResistanceIcon_Image.color = HealthManager.Instance.heatResistanceIcon_Image.color;
            thirstIcon_Image.color = HealthManager.Instance.thirstIcon_Image.color;
            mainHealthIcon_Image.color = HealthManager.Instance.mainHealthIcon_Image.color;

            if (MainManager.Instance.menuStates != MenuStates.ResearchMenu)
            {
                //Set DurabilityInfo on inventoryItems
                InventoryManager.Instance.SelectItemDurabilityDisplay();
                InventoryManager.Instance.SelectItemInfoToHotbar();
            }
            else
            {
                //Hide DurabilityInfo on inventoryItems
                InventoryManager.Instance.DeselectItemDurabilityDisplay();
                InventoryManager.Instance.DeselectItemInfoToHotbar();
            }

            //Set Timer Display
            TimeManager.Instance.SetTimerDisplay(clockText, dayText);

            //Set Temperature Display
            //WeatherManager.Instance.SetTemperatureDisplay(temperatureDisplay, playerTemperatureDisplay);
            SetTemperatureDisplay();
            SetWeatherDisplay();

            //Set the images of the Hotbar
            SetHotbarImages();
        }
    }


    //--------------------


    #region Menu Buttons
    public void MenuButton_Inventory_onClick()
    {
        //Play Change Menu Sound
        SoundManager.Instance.Play_Tablet_ChangeMenu_Clip();

        if (objectInteractingWith == ObjectInteractingWith.Chest)
        {
            MenuTransition(tabletMenuState, TabletMenuState.ChestInventory);
        }
        else
        {
            MenuTransition(tabletMenuState, TabletMenuState.Inventory);
        }
    }
    public void MenuButton_Journal_onClick()
    {
        //Play Change Menu Sound
        SoundManager.Instance.Play_Tablet_ChangeMenu_Clip();

        MenuTransition(tabletMenuState, TabletMenuState.Journal);
    }
    public void MenuButton_CraftingTable_onClick()
    {
        //Play Change Menu Sound
        SoundManager.Instance.Play_Tablet_ChangeMenu_Clip();

        MenuTransition(tabletMenuState, TabletMenuState.CraftingTable);
    }
    public void MenuButton_ResearchTable_onClick()
    {
        //Play Change Menu Sound
        SoundManager.Instance.Play_Tablet_ChangeMenu_Clip();

        MenuTransition(tabletMenuState, TabletMenuState.ResearchTable);
    }
    public void MenuButton_Skilltree_onClick()
    {
        //Play Change Menu Sound
        SoundManager.Instance.Play_Tablet_ChangeMenu_Clip();

        MenuTransition(tabletMenuState, TabletMenuState.SkillTree);
    }
    public void MenuButton_MoveableObjects_onClick()
    {
        //Play Change Menu Sound
        SoundManager.Instance.Play_Tablet_ChangeMenu_Clip();

        MenuTransition(tabletMenuState, TabletMenuState.BuildingObject);
    }
    public void MenuButton_Settings_onClick()
    {
        //Play Change Menu Sound
        SoundManager.Instance.Play_Tablet_ChangeMenu_Clip();

        MenuTransition(tabletMenuState, TabletMenuState.Settings);
    }
    public void MenuButton_CropPlot_onClick()
    {
        //Play Change Menu Sound
        SoundManager.Instance.Play_Tablet_ChangeMenu_Clip();

        MenuTransition(tabletMenuState, TabletMenuState.CropPlot);
    }

    public void MenuButton_Chest_onClick()
    {
        //Play Change Menu Sound
        SoundManager.Instance.Play_Tablet_ChangeMenu_Clip();

        MenuTransition(tabletMenuState, TabletMenuState.ChestInventory);

        menu_Equipment_Button.GetComponent<Image>().sprite = menuButton_Passive;
        menu_Chest_Button.GetComponent<Image>().sprite = menuButton_Active;
    }
    public void MenuButton_Equipment_onClick()
    {
        //Play Change Menu Sound
        SoundManager.Instance.Play_Tablet_ChangeMenu_Clip();

        MenuTransition(tabletMenuState, TabletMenuState.Equipment);

        menu_Equipment_Button.GetComponent<Image>().sprite = menuButton_Active;
        menu_Chest_Button.GetComponent<Image>().sprite = menuButton_Passive;
    }
    #endregion

    #region SkillTree Buttons
    public void SkillTreeButton_Inventory_onClick() //Inventory
    {
        //Play Change Menu Sound
        SoundManager.Instance.Play_Tablet_ChangeMenu_Clip();

        skillTree_Inventory_Parent.SetActive(true);
        skillTree_Equipment_Parent.SetActive(false);
        skillTree_GhostCapture_Parent.SetActive(false);
        skillTree_CrystalLight_Parent.SetActive(false);

        skillTree_Inventory_Button.GetComponent<Image>().sprite = menuButton_Active;
        skillTree_Equipment_Button.GetComponent<Image>().sprite = menuButton_Passive;
        skillTree_GhostCapture_Button.GetComponent<Image>().sprite = menuButton_Passive;
        skillTree_CrystalLight_Button.GetComponent<Image>().sprite = menuButton_Passive;

        SkillTreeManager.Instance.skillTreeMenu_Type = SkillTreeType.Inventory;

        SkillTreeManager.Instance.ResetSkillTree_Information();
    }
    public void SkillTreeButton_Player_onClick() //Player
    {
        //Play Change Menu Sound
        SoundManager.Instance.Play_Tablet_ChangeMenu_Clip();

        skillTree_Inventory_Parent.SetActive(false);
        skillTree_Equipment_Parent.SetActive(true);
        skillTree_GhostCapture_Parent.SetActive(false);
        skillTree_CrystalLight_Parent.SetActive(false);

        skillTree_Inventory_Button.GetComponent<Image>().sprite = menuButton_Passive;
        skillTree_Equipment_Button.GetComponent<Image>().sprite = menuButton_Active;
        skillTree_GhostCapture_Button.GetComponent<Image>().sprite = menuButton_Passive;
        skillTree_CrystalLight_Button.GetComponent<Image>().sprite = menuButton_Passive;

        SkillTreeManager.Instance.skillTreeMenu_Type = SkillTreeType.Player;

        SkillTreeManager.Instance.ResetSkillTree_Information();
    }
    public void SkillTreeButton_Tools_onClick() //Tools
    {
        //Play Change Menu Sound
        SoundManager.Instance.Play_Tablet_ChangeMenu_Clip();

        skillTree_Inventory_Parent.SetActive(false);
        skillTree_Equipment_Parent.SetActive(false);
        skillTree_GhostCapture_Parent.SetActive(true);
        skillTree_CrystalLight_Parent.SetActive(false);

        skillTree_Inventory_Button.GetComponent<Image>().sprite = menuButton_Passive;
        skillTree_Equipment_Button.GetComponent<Image>().sprite = menuButton_Passive;
        skillTree_GhostCapture_Button.GetComponent<Image>().sprite = menuButton_Active;
        skillTree_CrystalLight_Button.GetComponent<Image>().sprite = menuButton_Passive;

        SkillTreeManager.Instance.skillTreeMenu_Type = SkillTreeType.Tools;

        SkillTreeManager.Instance.ResetSkillTree_Information();
    }
    public void SkillTreeButton_Ar�dean_onClick() //Ar�dean
    {
        //Play Change Menu Sound
        SoundManager.Instance.Play_Tablet_ChangeMenu_Clip();

        skillTree_Inventory_Parent.SetActive(false);
        skillTree_Equipment_Parent.SetActive(false);
        skillTree_GhostCapture_Parent.SetActive(false);
        skillTree_CrystalLight_Parent.SetActive(true);

        skillTree_Inventory_Button.GetComponent<Image>().sprite = menuButton_Passive;
        skillTree_Equipment_Button.GetComponent<Image>().sprite = menuButton_Passive;
        skillTree_GhostCapture_Button.GetComponent<Image>().sprite = menuButton_Passive;
        skillTree_CrystalLight_Button.GetComponent<Image>().sprite = menuButton_Active;

        SkillTreeManager.Instance.skillTreeMenu_Type = SkillTreeType.Ar�dean;

        SkillTreeManager.Instance.ResetSkillTree_Information();
    }
    #endregion


    //--------------------


    //Open/Close Tablet
    #region
    void MenuTransition(TabletMenuState currentMenu, TabletMenuState newMenu)
    {
        if (newMenu == TabletMenuState.Inventory
            || newMenu == TabletMenuState.BuildingObject
            || newMenu == TabletMenuState.Journal
            || newMenu == TabletMenuState.Settings)
        {
            tabletMenuState_Save = newMenu;
        }

        MainManager.Instance.menuStates = MenuStates.None;

        menu_Chest_Button.GetComponent<Image>().sprite = menuButton_Passive;
        menu_Equipment_Button.GetComponent<Image>().sprite = menuButton_Passive;
        menu_Inventory_Button.GetComponent<Image>().sprite = menuButton_Passive;
        menu_CraftingTable_Button.GetComponent<Image>().sprite = menuButton_Passive;
        menu_ResearchTable_Button.GetComponent<Image>().sprite = menuButton_Passive;
        menu_CropPlot_Button.GetComponent<Image>().sprite = menuButton_Passive;
        menu_Skilltree_Button.GetComponent<Image>().sprite = menuButton_Passive;
        menu_MoveableObjects_Button.GetComponent<Image>().sprite = menuButton_Passive;
        menu_Journal_Button.GetComponent<Image>().sprite = menuButton_Passive;
        menu_Settings_Button.GetComponent<Image>().sprite = menuButton_Passive;

        playerInventory_MainParent.SetActive(false);
        chestInventory_MainParent.SetActive(false);
        equipInventory_MainParent.SetActive(false);
        menu_Inventory.SetActive(false);
        menu_CraftingTable.SetActive(false);
        menu_ResearchTable.SetActive(false);
        menu_CropPlot.SetActive(false);
        menu_Skilltree.SetActive(false);
        menu_MoveableObjects.SetActive(false);
        journal_Parent.SetActive(false);
        settings_Parent.SetActive(false);

        ResearchManager.Instance.ResetResearchMarkers();
        ResearchManager.Instance.ResetResearchItemColor();
        ResearchManager.Instance.UpdateResearchMarkers();

        //CropPlotManager.Instance.ResetCropPlotMarkers();
        //CropPlotManager.Instance.ResetCropPlotItemColor();
        //CropPlotManager.Instance.UpdateCropPlotMarkers();

        InventoryManager.Instance.DisplayInventoryItemInfo();

        //Exit current menu
        switch (currentMenu)
        {
            case TabletMenuState.None:
                break;

            case TabletMenuState.ChestInventory:
                menu_Inventory.SetActive(false);
                break;
            case TabletMenuState.Equipment:
                equipInventory_MainParent.SetActive(false);
                menu_Inventory.SetActive(false);
                break;
            case TabletMenuState.Inventory:
                menu_Inventory.SetActive(false);
                break;
            case TabletMenuState.CraftingTable:
                menu_CraftingTable.SetActive(false);
                break;
            case TabletMenuState.ResearchTable:
                InventoryManager.Instance.itemInfo.research_Parent.SetActive(false);
                menu_ResearchTable.SetActive(false);
                break;
            case TabletMenuState.CropPlot:
                InventoryManager.Instance.itemInfo.research_Parent.SetActive(false);
                menu_CropPlot.SetActive(false);
                break;
            case TabletMenuState.SkillTree:
                menu_Skilltree.SetActive(false);
                break;
            case TabletMenuState.BuildingObject:
                menu_MoveableObjects.SetActive(false);
                BuildingSystemMenu.Instance.BuildingBlockSelecter_Exit();
                break;
            case TabletMenuState.Journal:
                journal_Parent.SetActive(false);
                break;
            case TabletMenuState.Settings:
                settings_Parent.SetActive(false);
                break;

            default:
                break;
        }

        //Enter new menu
        switch (newMenu)
        {
            case TabletMenuState.ChestInventory:
                MainManager.Instance.menuStates = MenuStates.ChestMenu;
                tabletMenuState = TabletMenuState.ChestInventory;
                
                playerInventory_MainParent.SetActive(true);
                chestInventory_MainParent.SetActive(true);
                playerInventory_Parent.SetActive(true);
                chestInventory_Parent.SetActive(true);
                InventoryManager.Instance.playerInventory_Fake_Parent.SetActive(true);
                InventoryManager.Instance.chestInventory_Fake_Parent.SetActive(true);

                //Set DurabilityInfo on inventoryItems
                InventoryManager.Instance.SelectItemDurabilityDisplay();

                equipInventory_MainParent.SetActive(false);
                
                menu_Inventory.SetActive(true);

                menu_Equipment_Button.GetComponent<Image>().sprite = menuButton_Passive;
                menu_Chest_Button.GetComponent<Image>().sprite = menuButton_Active;
                menu_Equipment_Button.SetActive(true);
                menu_Chest_Button.SetActive(true);

                menu_Inventory_Button.GetComponent<Image>().sprite = menuButton_Active;
                break;
            case TabletMenuState.Equipment:
                MainManager.Instance.menuStates = MenuStates.EquipmentMenu;

                //InventoryManager.Instance.OpenPlayerInventory();
                chestInventory_MainParent.SetActive(false);
                tabletMenuState = TabletMenuState.Equipment;

                //Set DurabilityInfo on inventoryItems
                InventoryManager.Instance.SelectItemDurabilityDisplay();

                playerInventory_MainParent.SetActive(true);
                equipInventory_MainParent.SetActive(true);
                menu_Inventory.SetActive(true);

                menu_Inventory_Button.GetComponent<Image>().sprite = menuButton_Active;
                break;
            case TabletMenuState.Inventory:
                MainManager.Instance.menuStates = MenuStates.InventoryMenu;
                tabletMenuState = TabletMenuState.Inventory;

                chestInventory_MainParent.SetActive(false);
                equipInventory_MainParent.SetActive(true);

                //Set DurabilityInfo on inventoryItems
                InventoryManager.Instance.SelectItemDurabilityDisplay();

                menu_Chest_Button.SetActive(false);
                menu_Equipment_Button.SetActive(false);

                playerInventory_MainParent.SetActive(true);
                menu_Inventory.SetActive(true);

                menu_Inventory_Button.GetComponent<Image>().sprite = menuButton_Active;
                break;

            case TabletMenuState.CraftingTable:
                CraftingManager.Instance.startup = true;

                MainManager.Instance.menuStates = MenuStates.CraftingMenu;
                tabletMenuState = TabletMenuState.CraftingTable;

                CraftingManager.Instance.SetupSelectionScreen();

                CraftingManager.Instance.OpenCraftingScreen();
                menu_CraftingTable.SetActive(true);

                //Set DurabilityInfo on inventoryItems
                InventoryManager.Instance.SelectItemDurabilityDisplay();

                menu_Equipment_Button.SetActive(false);
                menu_Chest_Button.SetActive(false);

                menu_Inventory_Button.GetComponent<Image>().sprite = menuButton_Passive;
                menu_CraftingTable_Button.GetComponent<Image>().sprite = menuButton_Active;

                //Set "New" Crafting items Display
                for (int i = 0; i < CraftingManager.Instance.itemStateList.Count; i++)
                {
                    CraftingManager.Instance.UpdateItemState(i, false);
                }
                CraftingManager.Instance.UpdateCategoryButtonDisplay();

                playerInventory_MainParent.SetActive(true);
                chestInventory_MainParent.SetActive(false);
                equipInventory_MainParent.SetActive(false);
                menu_Inventory.SetActive(true);

                CraftingManager.Instance.startup = false;
                break;
            case TabletMenuState.ResearchTable:
                MainManager.Instance.menuStates = MenuStates.ResearchMenu;
                tabletMenuState = TabletMenuState.ResearchTable;

                menu_ResearchTable.SetActive(true);

                menu_Equipment_Button.SetActive(false);
                menu_Chest_Button.SetActive(false);

                menu_Inventory_Button.GetComponent<Image>().sprite = menuButton_Passive;
                menu_ResearchTable_Button.GetComponent<Image>().sprite = menuButton_Active;

                ResearchManager.Instance.UpdateResearchItemColor();
                ResearchManager.Instance.UpdateResearchMarkers();
                ResearchManager.Instance.SetResearchItemInfo(Items.None);

                playerInventory_MainParent.SetActive(true);
                chestInventory_MainParent.SetActive(false);
                equipInventory_MainParent.SetActive(false);
                menu_Inventory.SetActive(true);
                break;
            case TabletMenuState.CropPlot:
                MainManager.Instance.menuStates = MenuStates.CropPlotMenu;
                tabletMenuState = TabletMenuState.CropPlot;

                menu_CropPlot.SetActive(true);

                menu_Equipment_Button.SetActive(false);
                menu_Chest_Button.SetActive(false);

                menu_Inventory_Button.GetComponent<Image>().sprite = menuButton_Passive;
                menu_CropPlot_Button.GetComponent<Image>().sprite = menuButton_Active;

                CropPlotManager.Instance.SetupCropPlotSlots();

                CropPlotManager.Instance.UpdateCropPlotItemColor();
                ResearchManager.Instance.UpdateResearchMarkers();

                playerInventory_MainParent.SetActive(true);
                chestInventory_MainParent.SetActive(false);
                equipInventory_MainParent.SetActive(false);
                menu_Inventory.SetActive(true);
                break;
            case TabletMenuState.SkillTree:
                MainManager.Instance.menuStates = MenuStates.SkillTreeMenu;
                tabletMenuState = TabletMenuState.SkillTree;

                menu_Skilltree.SetActive(true);

                menu_Equipment_Button.SetActive(false);
                menu_Chest_Button.SetActive(false);

                playerInventory_MainParent.SetActive(false);
                chestInventory_MainParent.SetActive(false);
                equipInventory_MainParent.SetActive(false);
                menu_Inventory.SetActive(false);

                menu_Inventory_Button.GetComponent<Image>().sprite = menuButton_Passive;
                menu_Skilltree_Button.GetComponent<Image>().sprite = menuButton_Active;
                break;
            
            case TabletMenuState.BuildingObject:
                MainManager.Instance.menuStates = MenuStates.MoveableObjectMenu;
                tabletMenuState = TabletMenuState.BuildingObject;

                BuildingDisplayManager.Instance.OpenTablet();

                menu_MoveableObjects.SetActive(true);

                menu_Equipment_Button.SetActive(false);
                menu_Chest_Button.SetActive(false);

                playerInventory_MainParent.SetActive(false);
                chestInventory_MainParent.SetActive(false);
                equipInventory_MainParent.SetActive(false);
                menu_Inventory.SetActive(false);

                menu_Inventory_Button.GetComponent<Image>().sprite = menuButton_Passive;
                menu_MoveableObjects_Button.GetComponent<Image>().sprite = menuButton_Active;

                BuildingSystemMenu.Instance.BuildingBlockSelecter_Enter();
                break;
            
            case TabletMenuState.Journal:
                MainManager.Instance.menuStates = MenuStates.JournalMenu;
                tabletMenuState = TabletMenuState.Journal;

                //JournalManager.Instance.ResetInfoPage();

                journal_Parent.SetActive(true);

                menu_Equipment_Button.SetActive(false);
                menu_Chest_Button.SetActive(false);

                playerInventory_MainParent.SetActive(false);
                chestInventory_MainParent.SetActive(false);
                equipInventory_MainParent.SetActive(false);
                menu_Inventory.SetActive(false);

                if (JournalManager.Instance.journalMenuState == JournalMenuState.MentorJournal)
                {
                    JournalManager.Instance.MentorJournalButton_isPressed();
                }
                else if (JournalManager.Instance.journalMenuState == JournalMenuState.PlayerJournal)
                {
                    JournalManager.Instance.PlayerJournalButton_isPressed();
                }
                else if (JournalManager.Instance.journalMenuState == JournalMenuState.PersonalJournal)
                {
                    JournalManager.Instance.PersonalJournalButton_isPressed();
                }

                menu_Inventory_Button.GetComponent<Image>().sprite = menuButton_Passive;
                menu_Journal_Button.GetComponent<Image>().sprite = menuButton_Active;
                break;
            case TabletMenuState.Settings:
                MainManager.Instance.menuStates = MenuStates.SettingsMenu;
                tabletMenuState = TabletMenuState.Settings;

                settings_Parent.SetActive(true);

                menu_Equipment_Button.SetActive(false);
                menu_Chest_Button.SetActive(false);

                playerInventory_MainParent.SetActive(false);
                chestInventory_MainParent.SetActive(false);
                equipInventory_MainParent.SetActive(false);
                menu_Inventory.SetActive(false);

                menu_Inventory_Button.GetComponent<Image>().sprite = menuButton_Passive;
                menu_Settings_Button.GetComponent<Image>().sprite = menuButton_Active;
                break;


            //-----


            default: //Inventory
                MainManager.Instance.menuStates = MenuStates.InventoryMenu;
                tabletMenuState = TabletMenuState.Inventory;

                chestInventory_MainParent.SetActive(false);
                equipInventory_MainParent.SetActive(true);

                menu_Chest_Button.SetActive(false);
                menu_Equipment_Button.SetActive(false);

                playerInventory_MainParent.SetActive(true);
                menu_Inventory.SetActive(true);

                menu_Inventory_Button.GetComponent<Image>().sprite = menuButton_Active;
                break;
        }
    }

    //When Opening Tablet from hand
    public void OpenTablet()
    {
        if (MainManager.Instance.gameStates == GameStates.GameOver) { return; }

        tabletObject.SetActive(true);

        BuildingDisplayManager.Instance.OpenTablet();

        if (!menuObjectIsOpened)
        {
            tempMenuAmount = menuAmount;
        }

        Arms.Instance.OpenTabletAnimation();

        SoundManager.Instance.Play_Tablet_OpenTablet_Clip();

        InventoryManager.Instance.ClosePlayerInventory();

        //Set the MenuButtonBackround size
        menuButton_Background.GetComponent<RectTransform>().sizeDelta = new Vector2((150 * tempMenuAmount) + 35, menuButton_Background.GetComponent<RectTransform>().sizeDelta.y) ;

        //Set ItemTextInfo for both player and chest
        InventoryManager.Instance.SetPlayerItemInfo(Items.None, true);
        InventoryManager.Instance.SetPlayerItemInfo(Items.None, false);

        chestInventory_MainParent.SetActive(false);

        //Open correct menu
        if (tabletMenuState == TabletMenuState.None)
        {
            MenuTransition(TabletMenuState.None, TabletMenuState.Inventory);
            MenuTransition(TabletMenuState.None, TabletMenuState.Inventory);
        }
        else
        {
            MenuTransition(TabletMenuState.None, tabletMenuState);
            MenuTransition(TabletMenuState.None, tabletMenuState);
        }

        tablet_Parent.SetActive(true);
        //SetMenuDisplay(true);
        InventoryManager.Instance.OpenPlayerInventory();

        //Hide InteracteableInfo
        LookAtManager.Instance.LookAt_Parent.SetActive(false);

        //Rearrange the HealthParameter Displays
        healthParameters_Tablet_Parent.SetActive(true);
        healthParameters_PlayerScreen_Parent.SetActive(false);
        MainManager.Instance.centerImage.SetActive(false);

        //Hide Player Hotbar
        HotbarManager.Instance.hotbar_Parent.SetActive(false);

        //Hide Equipment_Button
        menu_Chest_Button.SetActive(false);
        menu_Equipment_Button.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        //MainManager.Instance.menuStates = MenuStates.InventoryMenu;

        //Update Researched Icons
        InventoryManager.Instance.UpdateResearchedIcon();
    }

    //When Opening Tablet from an InteracteableObject
    public void OpenTablet(TabletMenuState menuToOpen)
    {
        if (MainManager.Instance.gameStates == GameStates.GameOver) { return; }

        menuObjectIsOpened = true;

        if (menuToOpen != TabletMenuState.ChestInventory)
        {
            tempMenuAmount = menuAmount + 1;
        }
        else
        {
            tempMenuAmount = menuAmount;
        }
        
        OpenTablet();

        menuObjectIsOpened = false;

        //Tablet Menus - Additionals
        if (menuToOpen == TabletMenuState.CraftingTable)
        {
            menu_CraftingTable_Button.SetActive(true);
        }
        else if (menuToOpen == TabletMenuState.SkillTree)
        {
            menu_Skilltree_Button.SetActive(true);
        }
        else if (menuToOpen == TabletMenuState.ResearchTable)
        {
            menu_ResearchTable_Button.SetActive(true);
        }
        else if (menuToOpen == TabletMenuState.CropPlot)
        {
            menu_CropPlot_Button.SetActive(true);
        }

        MenuTransition(TabletMenuState.None, menuToOpen);
        MenuTransition(TabletMenuState.None, menuToOpen);

        if (InventoryManager.Instance.chestInventoryOpen != 0)
        {
            InventoryManager.Instance.PrepareInventoryUI(InventoryManager.Instance.chestInventoryOpen, false);
        }

        //Update Researched Icons
        InventoryManager.Instance.UpdateResearchedIcon();
    }
    
    public void CloseTablet()
    {
        Arms.Instance.CloseTabletAnimation();
        SoundManager.Instance.Play_Tablet_CloseTablet_Clip();

        InventoryManager.Instance.itemInfo.research_Parent.SetActive(false);

        //Turn everyting off after animation is finished
        StartCoroutine(CloseTabletCoroutine(0.2f));
    }
    IEnumerator CloseTabletCoroutine(float time)
    {
        yield return new WaitForSeconds(time);

        //Deselect DurabilityInfo on inventoryItems
        InventoryManager.Instance.DeselectItemDurabilityDisplay();

        //Close all menus
        tablet_Parent.SetActive(false);

        //Remove all Items from the SelectedSubList
        //CraftingManager.Instance.ResetSelectedList();

        //SetMenuDisplay(false);

        InventoryManager.Instance.ClosePlayerInventory();
        BuildingSystemMenu.Instance.BuildingBlockSelecter_Exit();

        //Rearrange the HealthParameter Displays
        healthParameters_Tablet_Parent.SetActive(false);
        healthParameters_PlayerScreen_Parent.SetActive(true);
        MainManager.Instance.centerImage.SetActive(true);

        //Activate Player Hotbar
        HotbarManager.Instance.hotbar_Parent.SetActive(true);

        //Set InteracteableInfo to be displayed again
        LookAtManager.Instance.LookAt_Parent.SetActive(true);

        if (tabletMenuState != TabletMenuState.Inventory
            && tabletMenuState != TabletMenuState.BuildingObject
            && tabletMenuState != TabletMenuState.Journal
            && tabletMenuState != TabletMenuState.Settings)
        {
            tabletMenuState = tabletMenuState_Save;
        }

        //tabletMenuState = TabletMenuState.None;
        objectInteractingWith = ObjectInteractingWith.None;

        //Stop Animation when not used anymore
        if (objectInteractingWith_Object)
        {
            if (objectInteractingWith_Object.GetComponent<Animations_Objects>())
            {
                objectInteractingWith_Object.GetComponent<Animations_Objects>().StopAnimation();
            }
        }

        objectInteractingWith_Object = null;

        Cursor.lockState = CursorLockMode.Locked;
        MainManager.Instance.menuStates = MenuStates.None;

        //Hide Buttons
        menu_CraftingTable_Button.SetActive(false);
        menu_ResearchTable_Button.SetActive(false);
        menu_CropPlot_Button.SetActive(false);
        menu_Skilltree_Button.SetActive(false);
        tabletObject.SetActive(false);
    }
    #endregion

    void SetMenuDisplay(bool state)
    {
        //Reset all Menu Displays
        menu_Inventory.SetActive(false);
        menu_CraftingTable.SetActive(false);
        menu_Skilltree.SetActive(false);
        menu_MoveableObjects.SetActive(false);

        menu_Inventory_Button.GetComponent<Image>().sprite = menuButton_Passive;
        menu_CraftingTable_Button.GetComponent<Image>().sprite = menuButton_Passive;
        menu_Skilltree_Button.GetComponent<Image>().sprite = menuButton_Passive;
        menu_MoveableObjects_Button.GetComponent<Image>().sprite = menuButton_Passive;

        if (state)
        {
            //Set active Menu visible
            switch (tabletMenuState)
            {
                case TabletMenuState.Inventory:
                    menu_Inventory_Button.GetComponent<Image>().sprite = menuButton_Active;
                    menu_Inventory.SetActive(true);
                    break;
                case TabletMenuState.CraftingTable:
                    menu_CraftingTable_Button.GetComponent<Image>().sprite = menuButton_Active;
                    menu_CraftingTable.SetActive(true);
                    break;
                case TabletMenuState.SkillTree:
                    menu_Skilltree_Button.GetComponent<Image>().sprite = menuButton_Active;
                    menu_Skilltree.SetActive(true);
                    break;
                case TabletMenuState.BuildingObject:
                    menu_MoveableObjects_Button.GetComponent<Image>().sprite = menuButton_Active;
                    menu_MoveableObjects.SetActive(true);
                    break;

                default: //Inventory
                    menu_Inventory_Button.GetComponent<Image>().sprite = menuButton_Active;
                    menu_Inventory.SetActive(true);
                    break;
            }
        }
    }


    //--------------------


    void SetTemperatureDisplay()
    {
        if (WeatherManager.Instance.termostatDisplay_isUpgraded)
        {
            termostatDisplay_Parent.SetActive(true);

            termostat_Image.color = WeatherManager.Instance.termostat_Image.color;
            pointer_Image.transform.SetLocalPositionAndRotation(WeatherManager.Instance.pointer_Image.transform.localPosition, WeatherManager.Instance.pointer_Image.transform.localRotation);
        }
        else
        {
            if (termostatDisplay_Parent.activeInHierarchy)
            {
                termostatDisplay_Parent.SetActive(false);
            }
        }
    }
    void SetWeatherDisplay()
    {
        if (WeatherManager.Instance.weatherImageDisplay_Day1_isUpgraded)
        {
            weatherImageDisplay_Day1_Parent.SetActive(true);
            weatherDisplay_Parent.SetActive(true);

            weatherImage_Day1.sprite = WeatherManager.Instance.weatherImage_Day1.sprite;
        }
        else
        {
            if (weatherDisplay_Parent.activeInHierarchy)
            {
                weatherDisplay_Parent.SetActive(false);
                weatherImageDisplay_Day1_Parent.SetActive(false);
            }
        }
    }


    //--------------------


    void SetHotbarImages()
    {
        for (int i = 0; i < 5; i++)
        {
            //Image
            if (HotbarManager.Instance.selectedSlot == i)
            {
                hotbarFrameImageList_Tablet[i].sprite = HotbarManager.Instance.hotbarList[i].hotbar.transform.GetChild(1).GetComponent<Image>().sprite;
            }
            else
            {
                hotbarFrameImageList_Tablet[i].sprite = HotbarManager.Instance.hotbarList[i].hotbar.GetComponent<Image>().sprite;
            }
            
            hotbarIconImageList_Tablet[i].sprite = HotbarManager.Instance.hotbarList[i].hotbar.transform.GetChild(0).GetComponent<Image>().sprite;

            //Durability
            if (HotbarManager.Instance.hotbarList[i].itemName != Items.None)
            {
                if (HotbarManager.Instance.hotbarList[i].durabilityMax > 0)
                {
                    //float tempFill = HotbarManager.Instance.hotbarList[i].durabilityCurrent / HotbarManager.Instance.hotbarList[i].durabilityMax;
                    hotbarItemDurabilityList_Tablet[i].fillAmount = HotbarManager.Instance.hotbarList[i].hotbar.GetComponent<HotbarSlot>().durabilityMeterImage.fillAmount;
                    hotbarItemDurabilityListParent_Tablet[i].SetActive(true);
                }
                else
                {
                    hotbarItemDurabilityListParent_Tablet[i].SetActive(false);
                }
            }
            else
            {
                hotbarItemDurabilityListParent_Tablet[i].SetActive(false);
            }
        }
    }


    //--------------------


    void SetPlussIcons()
    {
        bool plussIconcheck = false;

        //JournalPage
        #region
        if (JournalManager.Instance.mentorJournalPageList.Count > JournalManager.Instance.journalPage_PlussSign_Mentor.Count)
        {
            plussIconcheck = true;
        }
        else
        {
            for (int i = 0; i < JournalManager.Instance.journalPage_PlussSign_Mentor.Count; i++)
            {
                if (JournalManager.Instance.journalPage_PlussSign_Mentor[i])
                {
                    plussIconcheck = true;
                }
            }
        }

        if (JournalManager.Instance.playerJournalPageList.Count > JournalManager.Instance.journalPage_PlussSign_Player.Count)
        {
            plussIconcheck = true;
        }
        else
        {
            for (int i = 0; i < JournalManager.Instance.journalPage_PlussSign_Player.Count; i++)
            {
                if (JournalManager.Instance.journalPage_PlussSign_Player[i])
                {
                    plussIconcheck = true;
                }
            }
        }

        if (JournalManager.Instance.personalJournalPageList.Count > JournalManager.Instance.journalPage_PlussSign_Personal.Count)
        {
            plussIconcheck = true;
        }
        else
        {
            for (int i = 0; i < JournalManager.Instance.journalPage_PlussSign_Personal.Count; i++)
            {
                if (JournalManager.Instance.journalPage_PlussSign_Personal[i])
                {
                    plussIconcheck = true;
                }
            }
        }

        if (plussIconcheck)
        {
            menu_Journal_PlussIcon.SetActive(true);

            if (!menu_Journal_PlussIcon.activeInHierarchy)
            {
                menu_Journal_PlussIcon.SetActive(true);
            }
        }
        else
        {
            menu_Journal_PlussIcon.SetActive(false);

            if (menu_Journal_PlussIcon.activeInHierarchy)
            {
                menu_Journal_PlussIcon.SetActive(false);
            }
        }
        #endregion

        //Blueprints
        #region
        plussIconcheck = false;

        int counter = -1;

        if (BuildingDisplayManager.Instance.menuObjects_PlussSign.Count > 0)
        {
            for (int i = 0; i < BuildingDisplayManager.Instance.buildingObjectList.Count; i++)
            {
                for (int j = 0; j < BuildingDisplayManager.Instance.buildingObjectList[i].buildingObjectChildList.Count; j++)
                {
                    counter++;

                    if (BuildingDisplayManager.Instance.buildingObjectList[i].buildingObjectChildList[j].GetComponent<BuildingDisplaySlot>())
                    {
                        //BuildingBlockInfo
                        BuildingBlockInfo buildingBlockInfo = BuildingSystemManager.Instance.GetBuildingObjectInfo(BuildingDisplayManager.Instance.buildingObjectList[i].buildingObjectChildList[j].GetComponent<BuildingDisplaySlot>().buildingBlockObjectName, BuildingDisplayManager.Instance.buildingObjectList[i].buildingObjectChildList[j].GetComponent<BuildingDisplaySlot>().buildingMaterial);

                        if (buildingBlockInfo != null)
                        {
                            if (buildingBlockInfo.objectInfo.isActive)
                            {
                                if (BuildingDisplayManager.Instance.menuObjects_PlussSign[counter])
                                {
                                    plussIconcheck = true;

                                    i = BuildingDisplayManager.Instance.buildingObjectList.Count;

                                    break;
                                }
                            }
                        }

                        //FurnitureInfo
                        FurnitureInfo furnitureInfo = BuildingSystemManager.Instance.GetBuildingObjectInfo(BuildingDisplayManager.Instance.buildingObjectList[i].buildingObjectChildList[j].GetComponent<BuildingDisplaySlot>().furnitureObjectName);

                        if (furnitureInfo != null)
                        {
                            if (furnitureInfo.objectInfo.isActive)
                            {
                                if (BuildingDisplayManager.Instance.menuObjects_PlussSign[counter])
                                {
                                    plussIconcheck = true;

                                    i = BuildingDisplayManager.Instance.buildingObjectList.Count;

                                    break;
                                }
                            }
                        }

                        //MachineInfo
                        MachineInfo machineInfo = BuildingSystemManager.Instance.GetBuildingObjectInfo(BuildingDisplayManager.Instance.buildingObjectList[i].buildingObjectChildList[j].GetComponent<BuildingDisplaySlot>().machineObjectName);

                        if (machineInfo != null)
                        {
                            if (machineInfo.objectInfo.isActive)
                            {
                                if (BuildingDisplayManager.Instance.menuObjects_PlussSign[counter])
                                {
                                    plussIconcheck = true;

                                    i = BuildingDisplayManager.Instance.buildingObjectList.Count;

                                    break;
                                }
                            }
                        }
                    }
                }
            }







            //for (int i = 0; i < BuildingSystemManager.Instance.buildingBlocks_SO.buildingBlockObjectsList.Count; i++)
            //{
            //    if (BuildingSystemManager.Instance.buildingBlocks_SO.buildingBlockObjectsList[i].objectInfo.isActive)
            //    {
            //        if (!BuildingDisplayManager.Instance.menuObjects_PlussSign[i])
            //        {
            //            print("1. True");
            //            plussIconcheck = true;

            //            break;
            //        }
            //    }
            //}

            //if (!plussIconcheck)
            //{
            //    for (int i = 0; i < BuildingSystemManager.Instance.furniture_SO.furnitureObjectsList.Count; i++)
            //    {
            //        if (!BuildingSystemManager.Instance.furniture_SO.furnitureObjectsList[i].objectInfo.isActive)
            //        {
            //            if (BuildingDisplayManager.Instance.menuObjects_PlussSign[BuildingSystemManager.Instance.buildingBlocks_SO.buildingBlockObjectsList.Count - 1 + i])
            //            {
            //                print("2. True");
            //                plussIconcheck = true;

            //                break;
            //            }
            //        }
            //    }
            //}
            
            //if (!plussIconcheck)
            //{
            //    for (int i = 0; i < BuildingSystemManager.Instance.machines_SO.machineObjectsList.Count; i++)
            //    {
            //        if (!BuildingSystemManager.Instance.machines_SO.machineObjectsList[i].objectInfo.isActive)
            //        {
            //            if (BuildingDisplayManager.Instance.menuObjects_PlussSign[BuildingSystemManager.Instance.buildingBlocks_SO.buildingBlockObjectsList.Count - 1 + BuildingSystemManager.Instance.furniture_SO.furnitureObjectsList.Count - 1 + i])
            //            {
            //                print("3. True");
            //                plussIconcheck = true;

            //                break;
            //            }
            //        }
            //    }
            //}
        }

        if (plussIconcheck)
        {
            menu_MoveableObjects_PlussIcon.SetActive(true);

            if (!menu_MoveableObjects_PlussIcon.activeInHierarchy)
            {
                menu_MoveableObjects_PlussIcon.SetActive(true);
            }
        }
        else
        {
            menu_MoveableObjects_PlussIcon.SetActive(false);

            if (menu_MoveableObjects_PlussIcon.activeInHierarchy)
            {
                menu_MoveableObjects_PlussIcon.SetActive(false);
            }
        }
        #endregion
        
        //CraftingTable
        #region
        plussIconcheck = false;

        for (int i = 0; i < CraftingManager.Instance.itemStateList.Count; i++)
        {
            if (CraftingManager.Instance.itemStateList[i].itemState == CraftingItemState.New)
            {
                plussIconcheck = true;
            }
        }

        if (plussIconcheck)
        {
            if (!menu_CraftingTable_PlussIcon.activeInHierarchy)
            {
                menu_CraftingTable_PlussIcon.SetActive(true);
            }
        }
        else
        {
            if (menu_CraftingTable_PlussIcon.activeInHierarchy)
            {
                menu_CraftingTable_PlussIcon.SetActive(false);
            }
        }
        #endregion
    }
}

public enum TabletMenuState
{
    None,

    Inventory,
    CraftingTable,
    SkillTree,
    BuildingObject,
    ChestInventory,
    Equipment,
    Journal,
    Settings,
    ResearchTable,

    CropPlot
}
public enum ObjectInteractingWith
{
    None,

    Chest,
    CraftingTable,
    SkillTree,
    ResearchTable,

    CropPlot
}
