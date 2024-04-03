using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TabletManager : Singleton<TabletManager>
{
    #region Main Tablet Variables
    [Header("General")]
    public TabletMenuState tabletMenuState;
    public ObjectInteractingWith objectInteractingWith;
    public GameObject objectInteractingWith_Object;

    [Header("Menus")]
    public int menuAmount = 6;
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

    [Header("Buttons")]
    [SerializeField] GameObject menu_Inventory_Button;
    [SerializeField] GameObject menu_CraftingTable_Button;
    [SerializeField] GameObject menu_ResearchTable_Button;
    [SerializeField] GameObject menu_Skilltree_Button;
    [SerializeField] GameObject menu_MoveableObjects_Button;
    [SerializeField] GameObject menu_Journal_Button;
    [SerializeField] GameObject menu_Settings_Button;

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

        tablet_Parent.SetActive(false);
        tabletObject.SetActive(false);
        healthParameters_PlayerScreen_Parent.SetActive(true);
    }
    private void Update()
    {
        if (tablet_Parent.activeInHierarchy)
        {
            //Set Health Parameter Display
            HealthManager.Instance.SetHealthDisplay(hunger_Image, hungerValueMultiplier_Image,
                                 heatResistance_Image, heatResistanceValueMultiplier_Image,
                                 thirst_Image, thirstValueMultiplier_Image,
                                 mainHealth_Image);

            //Set Health Parameter Arrows
            HealthManager.Instance.SetHealthArrowDisplay(mainHealthValueMultiplier_Image);

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
        //SoundManager.Instance.Play_Tablet_ChangeMenu_Clip();

        MenuTransition(tabletMenuState, TabletMenuState.SkillTree);
    }
    public void MenuButton_MoveableObjects_onClick()
    {
        //Play Change Menu Sound
        SoundManager.Instance.Play_Tablet_ChangeMenu_Clip();

        MenuTransition(tabletMenuState, TabletMenuState.MoveableObjects);
    }
    public void MenuButton_Settings_onClick()
    {
        //Play Change Menu Sound
        SoundManager.Instance.Play_Tablet_ChangeMenu_Clip();

        MenuTransition(tabletMenuState, TabletMenuState.Settings);
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
    public void SkillTreeButton_Inventory_onClick()
    {
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
    public void SkillTreeButton_Equipment_onClick()
    {
        skillTree_Inventory_Parent.SetActive(false);
        skillTree_Equipment_Parent.SetActive(true);
        skillTree_GhostCapture_Parent.SetActive(false);
        skillTree_CrystalLight_Parent.SetActive(false);

        skillTree_Inventory_Button.GetComponent<Image>().sprite = menuButton_Passive;
        skillTree_Equipment_Button.GetComponent<Image>().sprite = menuButton_Active;
        skillTree_GhostCapture_Button.GetComponent<Image>().sprite = menuButton_Passive;
        skillTree_CrystalLight_Button.GetComponent<Image>().sprite = menuButton_Passive;

        SkillTreeManager.Instance.skillTreeMenu_Type = SkillTreeType.Equipment;

        SkillTreeManager.Instance.ResetSkillTree_Information();
    }
    public void SkillTreeButton_GhostCapture_onClick()
    {
        skillTree_Inventory_Parent.SetActive(false);
        skillTree_Equipment_Parent.SetActive(false);
        skillTree_GhostCapture_Parent.SetActive(true);
        skillTree_CrystalLight_Parent.SetActive(false);

        skillTree_Inventory_Button.GetComponent<Image>().sprite = menuButton_Passive;
        skillTree_Equipment_Button.GetComponent<Image>().sprite = menuButton_Passive;
        skillTree_GhostCapture_Button.GetComponent<Image>().sprite = menuButton_Active;
        skillTree_CrystalLight_Button.GetComponent<Image>().sprite = menuButton_Passive;

        SkillTreeManager.Instance.skillTreeMenu_Type = SkillTreeType.GhostCapture;

        SkillTreeManager.Instance.ResetSkillTree_Information();
    }
    public void SkillTreeButton_CrystalLight_onClick()
    {
        skillTree_Inventory_Parent.SetActive(false);
        skillTree_Equipment_Parent.SetActive(false);
        skillTree_GhostCapture_Parent.SetActive(false);
        skillTree_CrystalLight_Parent.SetActive(true);

        skillTree_Inventory_Button.GetComponent<Image>().sprite = menuButton_Passive;
        skillTree_Equipment_Button.GetComponent<Image>().sprite = menuButton_Passive;
        skillTree_GhostCapture_Button.GetComponent<Image>().sprite = menuButton_Passive;
        skillTree_CrystalLight_Button.GetComponent<Image>().sprite = menuButton_Active;

        SkillTreeManager.Instance.skillTreeMenu_Type = SkillTreeType.CrystalLight;

        SkillTreeManager.Instance.ResetSkillTree_Information();
    }
    #endregion


    //--------------------


    void MenuTransition(TabletMenuState currentMenu, TabletMenuState newMenu)
    {
        MainManager.Instance.menuStates = MenuStates.None;

        menu_Chest_Button.GetComponent<Image>().sprite = menuButton_Passive;
        menu_Equipment_Button.GetComponent<Image>().sprite = menuButton_Passive;
        menu_Inventory_Button.GetComponent<Image>().sprite = menuButton_Passive;
        menu_CraftingTable_Button.GetComponent<Image>().sprite = menuButton_Passive;
        menu_ResearchTable_Button.GetComponent<Image>().sprite = menuButton_Passive;
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
        menu_Skilltree.SetActive(false);
        menu_MoveableObjects.SetActive(false);
        journal_Parent.SetActive(false);
        settings_Parent.SetActive(false);

        ResearchManager.Instance.ResetResearchMarkers();
        ResearchManager.Instance.ResetResearchItemColor();
        ResearchManager.Instance.UpdateResearchMarkers();

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
                menu_ResearchTable.SetActive(false);
                break;
            case TabletMenuState.SkillTree:
                menu_Skilltree.SetActive(false);
                break;
            case TabletMenuState.MoveableObjects:
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

                playerInventory_MainParent.SetActive(true);
                chestInventory_MainParent.SetActive(false);
                equipInventory_MainParent.SetActive(false);
                menu_Inventory.SetActive(true);
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
            case TabletMenuState.MoveableObjects:
                MainManager.Instance.menuStates = MenuStates.MoveableObjectMenu;
                tabletMenuState = TabletMenuState.MoveableObjects;

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

                JournalManager.Instance.ResetInfoPage();

                journal_Parent.SetActive(true);

                menu_Equipment_Button.SetActive(false);
                menu_Chest_Button.SetActive(false);

                playerInventory_MainParent.SetActive(false);
                chestInventory_MainParent.SetActive(false);
                equipInventory_MainParent.SetActive(false);
                menu_Inventory.SetActive(false);

                JournalManager.Instance.MentorJournalButton_isPressed();

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
        tabletObject.SetActive(true);

        if (!menuObjectIsOpened)
        {
            tempMenuAmount = menuAmount;
        }

        Arms.Instance.OpenTabletAnimation();

        SoundManager.Instance.Play_Tablet_OpenTablet_Clip();

        InventoryManager.Instance.ClosePlayerInventory();

        //If BuildingHammer is in hand, open the MoveableObjectMenu
        if (HotbarManager.Instance.selectedItem == Items.WoodBuildingHammer || HotbarManager.Instance.selectedItem == Items.StoneBuildingHammer || HotbarManager.Instance.selectedItem == Items.CryoniteBuildingHammer)
        {
            MenuTransition(tabletMenuState, TabletMenuState.MoveableObjects);
        }

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
        }
        else
        {
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
        MainManager.Instance.menuStates = MenuStates.InventoryMenu;
    }

    //When Opening Tablet from an InteracteableObject
    public void OpenTablet(TabletMenuState menuToOpen)
    {
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

        MenuTransition(TabletMenuState.None, menuToOpen);

        if (InventoryManager.Instance.chestInventoryOpen != 0)
        {
            InventoryManager.Instance.PrepareInventoryUI(InventoryManager.Instance.chestInventoryOpen, false);
        }
    }
    
    public void CloseTablet()
    {
        Arms.Instance.CloseTabletAnimation();
        SoundManager.Instance.Play_Tablet_CloseTablet_Clip();

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

        tabletMenuState = TabletMenuState.None;
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

        //Hide all Requirement_Parents
        BuildingManager.Instance.buildingRequirement_Parent.SetActive(false);

        if (MainManager.Instance.gameStates != GameStates.Cutting)
        {
            BuildingManager.Instance.buildingRemoveRequirement_Parent.SetActive(false);
        }

        //Hide Buttons
        menu_CraftingTable_Button.SetActive(false);
        menu_ResearchTable_Button.SetActive(false);
        menu_Skilltree_Button.SetActive(false);
        tabletObject.SetActive(false);
    }

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
                case TabletMenuState.MoveableObjects:
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
}

public enum TabletMenuState
{
    None,

    Inventory,
    CraftingTable,
    SkillTree,
    MoveableObjects,
    ChestInventory,
    Equipment,
    Journal,
    Settings,
    ResearchTable
}
public enum ObjectInteractingWith
{
    None,

    Chest,
    CraftingTable,
    SkillTree,
    ResearchTable
}
