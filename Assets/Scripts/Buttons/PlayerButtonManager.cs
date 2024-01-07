using System;
using UnityEngine;

public class PlayerButtonManager : MonoBehaviour
{
    //Singleton
    public static PlayerButtonManager instance { get; set; } //Singleton
    
    public static Action OpenPlayerInventory_isPressedDown;
    public static Action ClosePlayerInventory_isPressedDown;
    public static Action objectInterraction_isPressedDown;

    //HandSelected
    public static Action hotbarSelectionDown_isPressed;
    public static Action hotbarSelectionUp_isPressed;

    public static Action isPressed_1;
    public static Action isPressed_2;
    public static Action isPressed_3;
    public static Action isPressed_4;
    public static Action isPressed_5;

    //BuildingSystem
    public static Action isPressed_BuildingSystemMenu_Enter;
    public static Action isPressed_BuildingSystemMenu_Exit;
    public static Action isPressed_BuildingRotate;

    //Equipment
    public static Action isPressed_EquipmentActivate;

    //Crafting
    public static Action isPressed_CloseCraftingMenu;

    //Testing Buttons
    public static Action T_isPressed;



    //--------------------


    private void Awake()
    {
        //Singleton
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    private void Update()
    {
        //BuildingSystem
        #region
        if (Input.GetKey(KeyCode.R) && MainManager.instance.gameStates == GameStates.Building && MainManager.instance.menuStates == MenuStates.None)
        {
            isPressed_BuildingRotate?.Invoke();
        }

        else if (Input.GetKeyDown(KeyCode.Mouse1) && MainManager.instance.gameStates == GameStates.Building
            && MainManager.instance.menuStates == MenuStates.None)
        {
            isPressed_BuildingSystemMenu_Enter?.Invoke();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1) && MainManager.instance.gameStates == GameStates.Building
            && (MainManager.instance.menuStates == MenuStates.None || MainManager.instance.menuStates == MenuStates.BuildingSystemMenu))
        {
            isPressed_BuildingSystemMenu_Exit?.Invoke();
        }
        #endregion

        //Equipment
        #region
        else if (Input.GetKeyDown(KeyCode.Mouse0) && MainManager.instance.menuStates == MenuStates.None 
            && EquippmentManager.instance.toolHolderParent.transform.childCount > 0
            && HotbarManager.instance.selectedItem != Items.None)
        {
            if (EquippmentManager.instance.toolHolderParent.GetComponentInChildren<EquippedItem>() != null)
            {
                isPressed_EquipmentActivate?.Invoke();
            }
        }
        #endregion

        //Crafting
        #region
        else if ((Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape))
            && MainManager.instance.menuStates == MenuStates.CraftingMenu)
        {
            isPressed_CloseCraftingMenu?.Invoke();
        }
        #endregion

        //PlayerInventory
        #region
        else if (Input.GetKeyDown(KeyCode.Tab) && MainManager.instance.menuStates == MenuStates.None)
        {
            OpenPlayerInventory_isPressedDown?.Invoke();
        }
        else if ((Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape))
            && (MainManager.instance.menuStates == MenuStates.InventoryMenu || MainManager.instance.menuStates == MenuStates.chestMenu))
        {
            ClosePlayerInventory_isPressedDown?.Invoke();
        }  
        #endregion

        //Object Interraction
        #region
        else if (Input.GetKeyDown(KeyCode.E) && MainManager.instance.menuStates == MenuStates.None)
        {
            objectInterraction_isPressedDown?.Invoke();
        }
        #endregion

        //Hotbar
        #region
        else if (Input.GetAxis("Mouse ScrollWheel") > 0 && MainManager.instance.menuStates == MenuStates.None)
        {
            hotbarSelectionDown_isPressed?.Invoke();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && MainManager.instance.menuStates == MenuStates.None)
        {
            hotbarSelectionUp_isPressed?.Invoke();
        }

        //QuickSlots
        else if (Input.GetKey(KeyCode.Alpha1) && MainManager.instance.menuStates == MenuStates.None)
            isPressed_1?.Invoke();
        else if (Input.GetKey(KeyCode.Alpha2) && MainManager.instance.menuStates == MenuStates.None)
            isPressed_2?.Invoke();
        else if (Input.GetKey(KeyCode.Alpha3) && MainManager.instance.menuStates == MenuStates.None)
            isPressed_3?.Invoke();
        else if (Input.GetKey(KeyCode.Alpha4) && MainManager.instance.menuStates == MenuStates.None)
            isPressed_4?.Invoke();
        else if (Input.GetKey(KeyCode.Alpha5) && MainManager.instance.menuStates == MenuStates.None)
            isPressed_5?.Invoke();
        #endregion

        //Left Mouse
        #region
        //else if (Input.GetKeyDown(KeyCode.Mouse0))
        //{
        //    leftMouse_isPressedDown?.Invoke();
        //}
        //else if (Input.GetKeyUp(KeyCode.Mouse0))
        //{
        //    leftMouse_isPressedUp?.Invoke();
        //}
        #endregion

        //Right Mouse
        #region
        //else if (Input.GetKeyDown(KeyCode.Mouse1))
        //{
        //    rightMouse_isPressedDown?.Invoke();
        //}
        //else if (Input.GetKeyUp(KeyCode.Mouse1))
        //{
        //    rightMouse_isPressedUp?.Invoke();
        //}
        #endregion

        //Testing
        #region
        else if (Input.GetKeyDown(KeyCode.T))
        {
            T_isPressed?.Invoke();
        }
        #endregion

    }
}
public enum ButtonClickedState
{
    None,

    leftMouse,
    rightMouse,
    middleMouse,

    tab,
    shift,
    Esc,
    C,
    E
}
public enum InventoryButtonState
{
    None,

    leftMouse,
    rightMouse,
    Shift_RightMouse,
    ScrollWheel,
    QuickClick
}

