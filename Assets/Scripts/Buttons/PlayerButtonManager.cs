using System;
using UnityEngine;

public class PlayerButtonManager : Singleton<PlayerButtonManager>
{
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

    public static Action isPressed_MoveableRotation_Right;
    public static Action isPressed_MoveableRotation_Left;

    //Equipment
    public static Action isPressed_EquipmentActivate;

    //Crafting
    public static Action isPressed_CloseCraftingMenu;

    //Testing Buttons
    public static Action T_isPressed;



    //--------------------


    private void Update()
    {
        //Exit Game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        //BuildingSystem
        #region
        else if (Input.GetKeyDown(KeyCode.Mouse1) && MainManager.Instance.gameStates == GameStates.Building
            && MainManager.Instance.menuStates == MenuStates.None)
        {
            isPressed_BuildingSystemMenu_Enter?.Invoke();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1) && MainManager.Instance.gameStates == GameStates.Building
            && (MainManager.Instance.menuStates == MenuStates.None || MainManager.Instance.menuStates == MenuStates.MoveableObjectMenu))
        {
            isPressed_BuildingSystemMenu_Exit?.Invoke();
        }
        #endregion

        //Equipment
        #region
        else if (Input.GetKeyDown(KeyCode.Mouse0) && MainManager.Instance.menuStates == MenuStates.None 
            && EquippmentManager.Instance.toolHolderParent.transform.childCount > 0
            && HotbarManager.Instance.selectedItem != Items.None)
        {
            if (EquippmentManager.Instance.toolHolderParent.GetComponentInChildren<EquippedItem>() != null)
            {
                isPressed_EquipmentActivate?.Invoke();
            }
        }
        #endregion

        //Crafting
        #region
        //else if ((Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape))
        //    && MainManager.Instance.menuStates == MenuStates.CraftingMenu)
        //{
        //    isPressed_CloseCraftingMenu?.Invoke();
        //}
        #endregion

        //PlayerInventory
        #region
        else if (Input.GetKeyDown(KeyCode.Tab) && MainManager.Instance.menuStates == MenuStates.None)
        {
            OpenPlayerInventory_isPressedDown?.Invoke();
        }
        else if ((Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape))
            && (MainManager.Instance.menuStates != MenuStates.None))
        {
            ClosePlayerInventory_isPressedDown?.Invoke();
        }  
        #endregion

        //Object Interraction
        #region
        else if (Input.GetKeyDown(KeyCode.E) && MainManager.Instance.menuStates == MenuStates.None)
        {
            objectInterraction_isPressedDown?.Invoke();
        }
        #endregion

        //Hotbar
        #region
        else if (Input.GetAxis("Mouse ScrollWheel") > 0 /*&& MainManager.Instance.menuStates != MenuStates.None*/)
        {
            hotbarSelectionDown_isPressed?.Invoke();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 /*&& MainManager.Instance.menuStates != MenuStates.None*/)
        {
            hotbarSelectionUp_isPressed?.Invoke();
        }

        //QuickSlots
        else if (Input.GetKey(KeyCode.Alpha1) && MainManager.Instance.menuStates != MenuStates.MoveableObjectMenu && MainManager.Instance.menuStates != MenuStates.SkillTreeMenu)
            isPressed_1?.Invoke();
        else if (Input.GetKey(KeyCode.Alpha2) && MainManager.Instance.menuStates != MenuStates.MoveableObjectMenu && MainManager.Instance.menuStates != MenuStates.SkillTreeMenu)
            isPressed_2?.Invoke();
        else if (Input.GetKey(KeyCode.Alpha3) && MainManager.Instance.menuStates != MenuStates.MoveableObjectMenu && MainManager.Instance.menuStates != MenuStates.SkillTreeMenu)
            isPressed_3?.Invoke();
        else if (Input.GetKey(KeyCode.Alpha4) && MainManager.Instance.menuStates != MenuStates.MoveableObjectMenu && MainManager.Instance.menuStates != MenuStates.SkillTreeMenu)
            isPressed_4?.Invoke();
        else if (Input.GetKey(KeyCode.Alpha5) && MainManager.Instance.menuStates != MenuStates.MoveableObjectMenu && MainManager.Instance.menuStates != MenuStates.SkillTreeMenu)
            isPressed_5?.Invoke();
        #endregion

        //MoveableObject Rotation
        #region
        else if (Input.GetKey(KeyCode.R) && MainManager.Instance.menuStates == MenuStates.None && MainManager.Instance.gameStates == GameStates.Building)
        {
            isPressed_MoveableRotation_Right?.Invoke();
        }
        else if (Input.GetKey(KeyCode.E) && MainManager.Instance.menuStates == MenuStates.None && MainManager.Instance.gameStates == GameStates.Building)
        {
            isPressed_MoveableRotation_Left?.Invoke();
        }
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

