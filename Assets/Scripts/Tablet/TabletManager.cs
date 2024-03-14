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
    int menuAmount = 4;
    [SerializeField] GameObject menu_Inventory;
    public GameObject playerInventory_MainParent;
    public GameObject chestInventory_MainParent;
    public GameObject equipInventory_MainParent;
    public GameObject playerInventory_Parent;
    public GameObject chestInventory_Parent;

    [SerializeField] GameObject menu_CraftingTable;
    [SerializeField] GameObject menu_Skilltree;
    [SerializeField] GameObject menu_MoveableObjects;

    [Header("Buttons")]
    [SerializeField] GameObject menu_Inventory_Button;
    [SerializeField] GameObject menu_CraftingTable_Button;
    [SerializeField] GameObject menu_Skilltree_Button;
    [SerializeField] GameObject menu_MoveableObjects_Button;

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
    [SerializeField] List<GameObject> hungerValueMultiplier_Image = new List<GameObject>();

    [SerializeField] Image heatResistance_Image;
    [SerializeField] List<GameObject> heatResistanceValueMultiplier_Image = new List<GameObject>();

    [SerializeField] Image thirst_Image;
    [SerializeField] List<GameObject> thirstValueMultiplier_Image = new List<GameObject>();

    [SerializeField] Image mainHealth_Image;
    [SerializeField] List<GameObject> mainHealthValueMultiplier_Image = new List<GameObject>();

    [Header("Timer")]
    [SerializeField] TextMeshProUGUI clockText;
    [SerializeField] TextMeshProUGUI dayText;

    [Header("Temperature")]
    [SerializeField] TextMeshProUGUI temperatureDisplay;
    [SerializeField] TextMeshProUGUI playerTemperatureDisplay;

    [Header("Hotbar")]
    [SerializeField] List<Image> hotbarFrameImageList_Tablet = new List<Image>();
    [SerializeField] List<Image> hotbarIconImageList_Tablet = new List<Image>();
    [SerializeField] List<GameObject> hotbarItemDurabilityListParent_Tablet = new List<GameObject>();
    [SerializeField] List<Image> hotbarItemDurabilityList_Tablet = new List<Image>();
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
            HealthManager.Instance.SetHealthArrowDisplay(mainHealthValueMultiplier_Image);

            //Set Timer Diplay
            TimeManager.Instance.SetTimerDisplay(clockText, dayText);

            //Set Temperature Display
            WeatherManager.Instance.SetTemperatureDisplay(temperatureDisplay, playerTemperatureDisplay);

            //Set the images of the Hotbar
            SetHotbarImages();

            //Set DurabilityInfo on inventoryItems
            InventoryManager.Instance.SelectItemDurabilityDisplay();
        }
    }


    //--------------------


    #region Menu Buttons
    public void MenuButton_Inventory_onClick()
    {
        if (objectInteractingWith == ObjectInteractingWith.Chest)
        {
            MenuTransition(tabletMenuState, TabletMenuState.ChestInventory);
        }
        else
        {
            MenuTransition(tabletMenuState, TabletMenuState.Inventory);
        }
    }
    public void MenuButton_CraftingTable_onClick()
    {
        MenuTransition(tabletMenuState, TabletMenuState.CraftingTable);
    }
    public void MenuButton_Skilltree_onClick()
    {
        MenuTransition(tabletMenuState, TabletMenuState.SkillTree);
    }
    public void MenuButton_MoveableObjects_onClick()
    {
        MenuTransition(tabletMenuState, TabletMenuState.MoveableObjects);
    }

    public void MenuButton_Chest_onClick()
    {
        MenuTransition(tabletMenuState, TabletMenuState.ChestInventory);

        menu_Equipment_Button.GetComponent<Image>().sprite = menuButton_Passive;
        menu_Chest_Button.GetComponent<Image>().sprite = menuButton_Active;
    }
    public void MenuButton_Equipment_onClick()
    {
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
        menu_Skilltree_Button.GetComponent<Image>().sprite = menuButton_Passive;
        menu_MoveableObjects_Button.GetComponent<Image>().sprite = menuButton_Passive;

        playerInventory_MainParent.SetActive(false);
        chestInventory_MainParent.SetActive(false);
        equipInventory_MainParent.SetActive(false);
        menu_Inventory.SetActive(false);
        menu_CraftingTable.SetActive(false);
        menu_Skilltree.SetActive(false);
        menu_MoveableObjects.SetActive(false);

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
            case TabletMenuState.SkillTree:
                menu_Skilltree.SetActive(false);
                break;
            case TabletMenuState.MoveableObjects:
                menu_MoveableObjects.SetActive(false);
                BuildingSystemMenu.Instance.BuildingBlockSelecter_Exit();
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

                menu_Chest_Button.SetActive(false);
                menu_Equipment_Button.SetActive(false);

                playerInventory_MainParent.SetActive(true);
                menu_Inventory.SetActive(true);

                menu_Inventory_Button.GetComponent<Image>().sprite = menuButton_Active;
                break;

            case TabletMenuState.CraftingTable:
                MainManager.Instance.menuStates = MenuStates.CraftingMenu;
                tabletMenuState = TabletMenuState.CraftingTable;

                CraftingManager.Instance.OpenCraftingScreen();
                menu_CraftingTable.SetActive(true);

                menu_Equipment_Button.SetActive(false);
                menu_Chest_Button.SetActive(false);

                menu_Inventory_Button.GetComponent<Image>().sprite = menuButton_Passive;
                menu_CraftingTable_Button.GetComponent<Image>().sprite = menuButton_Active;

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
        InventoryManager.Instance.ClosePlayerInventory();

        //If BuildingHammer is in hand, open the MoveableObjectMenu
        if (MainManager.Instance.gameStates == GameStates.Building)
        {
            MenuTransition(tabletMenuState, TabletMenuState.MoveableObjects);
        }

        //Set the MenuButtonBackround size
        menuButton_Background.GetComponent<RectTransform>().sizeDelta = new Vector2((150 * menuAmount) + 35, menuButton_Background.GetComponent<RectTransform>().sizeDelta.y) ;

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
        OpenTablet();

        MenuTransition(TabletMenuState.None, menuToOpen);

        if (InventoryManager.Instance.chestInventoryOpen != 0)
        {
            InventoryManager.Instance.PrepareInventoryUI(InventoryManager.Instance.chestInventoryOpen, false);
        }
    }
    
    public void CloseTablet()
    {
        //Deselect DurabilityInfo on inventoryItems
        InventoryManager.Instance.DeselectItemDurabilityDisplay();

        //Close all menus
        tablet_Parent.SetActive(false);
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
    Equipment
}
public enum ObjectInteractingWith
{
    None,

    Chest,
    CraftingTable,
    SkillTree
}
