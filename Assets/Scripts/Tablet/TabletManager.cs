using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TabletManager : Singleton<TabletManager>
{
    [Header("General")]
    public TabletMenuState tabletMenuState;

    [Header("Menus")]
    int menuAmount = 4;
    [SerializeField] GameObject menu_Inventory;
    [SerializeField] GameObject menu_CraftingTable;
    [SerializeField] GameObject menu_Skilltree;
    [SerializeField] GameObject menu_MoveableObjects;

    [Header("Buttons")]
    [SerializeField] GameObject menu_Inventory_Button;
    [SerializeField] GameObject menu_CraftingTable_Button;
    [SerializeField] GameObject menu_Skilltree_Button;
    [SerializeField] GameObject menu_MoveableObjects_Button;

    [Header("Button Images")]
    [SerializeField] Sprite menuButton_Passive;
    [SerializeField] Sprite menuButton_Active;

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
        }
    }


    //--------------------


    #region Menu Buttons
    public void MenuButton_Inventory_onClick()
    {
        if (!BuildingSystemMenu.Instance.buildingSystemMenu_isOpen)
        {
            MenuTransition(tabletMenuState, TabletMenuState.Inventory);
        }
    }
    public void MenuButton_CraftingTable_onClick()
    {
        if (!BuildingSystemMenu.Instance.buildingSystemMenu_isOpen)
        {
            MenuTransition(tabletMenuState, TabletMenuState.CraftingTable);
        }
    }
    public void MenuButton_Skilltree_onClick()
    {
        if (!BuildingSystemMenu.Instance.buildingSystemMenu_isOpen)
        {
            MenuTransition(tabletMenuState, TabletMenuState.SkillTree);
        }
    }
    public void MenuButton_MoveableObjects_onClick()
    {
        if (!BuildingSystemMenu.Instance.buildingSystemMenu_isOpen)
        {
            MenuTransition(tabletMenuState, TabletMenuState.MoveableObjects);
        }
    }
    #endregion


    //--------------------


    void MenuTransition(TabletMenuState currentMenu, TabletMenuState newMenu)
    {
        //Turn of current menu
        switch (currentMenu)
        {
            case TabletMenuState.Inventory:
                menu_Inventory.SetActive(false);
                menu_Inventory_Button.GetComponent<Image>().sprite = menuButton_Passive;
                break;
            case TabletMenuState.CraftingTable:
                menu_CraftingTable.SetActive(false);
                menu_CraftingTable_Button.GetComponent<Image>().sprite = menuButton_Passive;
                break;
            case TabletMenuState.SkillTree:
                menu_Skilltree.SetActive(false);
                menu_Skilltree_Button.GetComponent<Image>().sprite = menuButton_Passive;
                break;
            case TabletMenuState.MoveableObjects:
                menu_MoveableObjects.SetActive(false);
                menu_MoveableObjects_Button.GetComponent<Image>().sprite = menuButton_Passive;
                break;

            default:
                break;
        }

        //Turn on new menu
        switch (newMenu)
        {
            case TabletMenuState.Inventory:
                menu_Inventory.SetActive(true);
                tabletMenuState = TabletMenuState.Inventory;
                menu_Inventory_Button.GetComponent<Image>().sprite = menuButton_Active;
                break;
            case TabletMenuState.CraftingTable:
                menu_CraftingTable.SetActive(true);
                tabletMenuState = TabletMenuState.CraftingTable;
                menu_CraftingTable_Button.GetComponent<Image>().sprite = menuButton_Active;
                break;
            case TabletMenuState.SkillTree:
                menu_Skilltree.SetActive(true);
                tabletMenuState = TabletMenuState.SkillTree;
                menu_Skilltree_Button.GetComponent<Image>().sprite = menuButton_Active;
                break;
            case TabletMenuState.MoveableObjects:
                menu_MoveableObjects.SetActive(true);
                tabletMenuState = TabletMenuState.MoveableObjects;
                menu_MoveableObjects_Button.GetComponent<Image>().sprite = menuButton_Active;
                break;

            default:
                break;
        }
    }

    public void OpenTablet()
    {
        //Set the MenuButtonBackround size
        menuButton_Background.GetComponent<RectTransform>().sizeDelta = new Vector2((150 * menuAmount) + 35, menuButton_Background.GetComponent<RectTransform>().sizeDelta.y) ;

        //Open correct menu
        tablet_Parent.SetActive(true);
        SetMenuDisplay(true);

        //Rearrange the HealthParameter Displays
        healthParameters_Tablet_Parent.SetActive(true);
        healthParameters_PlayerScreen_Parent.SetActive(false);
        MainManager.Instance.centerImage.SetActive(false);

        //Hide Player Hotbar
        HotbarManager.Instance.hotbar_Parent.SetActive(false);
    }
    public void CloseTablet()
    {
        //Close all menus
        tablet_Parent.SetActive(false);
        SetMenuDisplay(false);

        //Rearrange the HealthParameter Displays
        healthParameters_Tablet_Parent.SetActive(false);
        healthParameters_PlayerScreen_Parent.SetActive(true);
        MainManager.Instance.centerImage.SetActive(true);

        //Activate Player Hotbar
        HotbarManager.Instance.hotbar_Parent.SetActive(true);
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

                default:
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
            if (HotbarManager.Instance.selectedSlot == i)
            {
                hotbarFrameImageList_Tablet[i].sprite = HotbarManager.Instance.hotbarList[i].transform.GetChild(1).GetComponent<Image>().sprite;
            }
            else
            {
                hotbarFrameImageList_Tablet[i].sprite = HotbarManager.Instance.hotbarList[i].GetComponent<Image>().sprite;
            }
            
            hotbarIconImageList_Tablet[i].sprite = HotbarManager.Instance.hotbarList[i].transform.GetChild(0).GetComponent<Image>().sprite;
        }
      }
}

public enum TabletMenuState
{
    Inventory,
    CraftingTable,
    SkillTree,
    MoveableObjects
}