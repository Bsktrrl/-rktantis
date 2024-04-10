using System;
using UnityEngine;

public class PlayerButtonManager : Singleton<PlayerButtonManager>
{
    //Tablet
    public static Action OpenPlayerInventory_isPressedDown;
    public static Action ClosePlayerInventory_isPressedDown;

    //ObjectInteraction
    public static Action objectInterraction_isPressedDown;

    //Hotbar
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

    public static Action isPressed_FixedRotation_Clockwise;
    public static Action isPressed_FixedRotation_CounterClockwise;
    public static Action isPressed_FixedRotation_Intern_Clockwise;
    public static Action isPressed_FixedRotation_Intern_CounterClockwise;

    public static Action isPressed_MoveableRotation_Right;
    public static Action isPressed_MoveableRotation_Left;

    //Equipment
    public static Action isPressed_EquipmentActivate;
    public static Action isPressed_EquipmentDeactivate;

    //Crafting
    public static Action isPressed_CloseCraftingMenu;

    //Drink
    public static Action refillBottle_isPressed;



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
        
        //Refill WaterConsumable
        #region
        else if (Input.GetKeyDown(KeyCode.E)
            && (HotbarManager.Instance.selectedItem == Items.Cup || HotbarManager.Instance.selectedItem == Items.Bottle || HotbarManager.Instance.selectedItem == Items.Bucket)
            && SelectionManager.Instance.tag == "Water")
        {
            refillBottle_isPressed?.Invoke();
        }
        #endregion

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

        //Equipment Usage
        #region
        else if (Input.GetKeyDown(KeyCode.Mouse0) && MainManager.Instance.menuStates == MenuStates.None
            && 
            
            (HotbarManager.Instance.selectedItem == Items.Flashlight || HotbarManager.Instance.selectedItem == Items.AríditeCrystal || HotbarManager.Instance.selectedItem == Items.None
            || HotbarManager.Instance.selectedItem == Items.WoodPickaxe || HotbarManager.Instance.selectedItem == Items.StonePickaxe || HotbarManager.Instance.selectedItem == Items.CryonitePickaxe
            || HotbarManager.Instance.selectedItem == Items.WoodAxe || HotbarManager.Instance.selectedItem == Items.StoneAxe || HotbarManager.Instance.selectedItem == Items.CryoniteAxe

            || HotbarManager.Instance.selectedItem == Items.Cup || HotbarManager.Instance.selectedItem == Items.Bottle || HotbarManager.Instance.selectedItem == Items.Bucket
            || HotbarManager.Instance.selectedItem == Items.GhostCapturer
            ))
        {
            isPressed_EquipmentActivate?.Invoke();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0) && MainManager.Instance.menuStates == MenuStates.None
            &&

            (HotbarManager.Instance.selectedItem == Items.Flashlight || HotbarManager.Instance.selectedItem == Items.AríditeCrystal || HotbarManager.Instance.selectedItem == Items.None
            || HotbarManager.Instance.selectedItem == Items.WoodPickaxe || HotbarManager.Instance.selectedItem == Items.StonePickaxe || HotbarManager.Instance.selectedItem == Items.CryonitePickaxe
            || HotbarManager.Instance.selectedItem == Items.WoodAxe || HotbarManager.Instance.selectedItem == Items.StoneAxe || HotbarManager.Instance.selectedItem == Items.CryoniteAxe

            || HotbarManager.Instance.selectedItem == Items.Cup || HotbarManager.Instance.selectedItem == Items.Bottle || HotbarManager.Instance.selectedItem == Items.Bucket
            || HotbarManager.Instance.selectedItem == Items.GhostCapturer
            ))
        {
            isPressed_EquipmentDeactivate?.Invoke();
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

        //Object Interaction
        #region
        else if (Input.GetKeyDown(KeyCode.E) && MainManager.Instance.menuStates == MenuStates.None /*&& BuildingManager_v2.Instance.buildingBlockGhost == null*/)
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
        else if (Input.GetKeyDown(KeyCode.Alpha1) && MainManager.Instance.menuStates != MenuStates.MoveableObjectMenu && MainManager.Instance.menuStates != MenuStates.SkillTreeMenu)
            isPressed_1?.Invoke();
        else if (Input.GetKeyDown(KeyCode.Alpha2) && MainManager.Instance.menuStates != MenuStates.MoveableObjectMenu && MainManager.Instance.menuStates != MenuStates.SkillTreeMenu)
            isPressed_2?.Invoke();
        else if (Input.GetKeyDown(KeyCode.Alpha3) && MainManager.Instance.menuStates != MenuStates.MoveableObjectMenu && MainManager.Instance.menuStates != MenuStates.SkillTreeMenu)
            isPressed_3?.Invoke();
        else if (Input.GetKeyDown(KeyCode.Alpha4) && MainManager.Instance.menuStates != MenuStates.MoveableObjectMenu && MainManager.Instance.menuStates != MenuStates.SkillTreeMenu)
            isPressed_4?.Invoke();
        else if (Input.GetKeyDown(KeyCode.Alpha5) && MainManager.Instance.menuStates != MenuStates.MoveableObjectMenu && MainManager.Instance.menuStates != MenuStates.SkillTreeMenu)
            isPressed_5?.Invoke();
        #endregion

        //MoveableObject Rotation
        #region
        else if (Input.GetKeyDown(KeyCode.Z) && MainManager.Instance.gameStates == GameStates.Building && BuildingManager_v2.Instance.buildingBlockGhost != null)
        {
            isPressed_FixedRotation_Clockwise?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.X) && MainManager.Instance.gameStates == GameStates.Building && BuildingManager_v2.Instance.buildingBlockGhost != null)
        {
            isPressed_FixedRotation_CounterClockwise?.Invoke();
        }

        else if (Input.GetKeyDown(KeyCode.E) && MainManager.Instance.gameStates == GameStates.Building && BuildingManager_v2.Instance.buildingBlockGhost != null)
        {
            isPressed_FixedRotation_Intern_Clockwise?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.R) && MainManager.Instance.gameStates == GameStates.Building && BuildingManager_v2.Instance.buildingBlockGhost != null)
        {
            isPressed_FixedRotation_Intern_CounterClockwise?.Invoke();
        }

        else if (Input.GetKey(KeyCode.R) && MainManager.Instance.menuStates == MenuStates.None && MainManager.Instance.gameStates == GameStates.Building)
        {
            isPressed_MoveableRotation_Right?.Invoke();
        }
        else if (Input.GetKey(KeyCode.E) && MainManager.Instance.menuStates == MenuStates.None && MainManager.Instance.gameStates == GameStates.Building)
        {
            isPressed_MoveableRotation_Left?.Invoke();
        }
        #endregion


        //--------------------


        //Testing
        #region
        //Place BuildingBlock - Test
        else if (Input.GetKeyDown(KeyCode.T) && MainManager.Instance.gameStates == GameStates.Building && BuildingManager_v2.Instance.buildingBlockGhost != null)
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

