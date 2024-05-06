using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : Singleton<MainManager>
{
    [Header("Player")]
    public GameObject player;
    public GameObject playerBody;
    public Camera mainCamera;
    public Camera mainMainCamera;

    public Canvas mainCanvas;
    public Canvas mainTabletCanvas;

    [Header("Game States")]
    public MenuStates menuStates;
    public GameStates gameStates;

    [Header("_SO")]
    public Item_SO item_SO;
    public MoveableObject_SO moveableObject_SO;

    [Header("_SO")]
    public Color mainColor_Blue;
    public Color mainColor_Orange;

    //Update Delayer
    public int updateInterval = 10;

    //CenterImage
    public GameObject centerImage;

    public bool deleyedStart;

    [Header("Demo Ending")]
    public GameObject demoEndingText;

    [Header("Player")]
    public int toolDurability_Wood = 40;
    public int toolDurability_Stone = 100;
    public int toolDurability_Cryonite = 200;


    //--------------------


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        centerImage.SetActive(true);
        demoEndingText.SetActive(false);
    }
    private void Update()
    {
        if (!DataManager.Instance.hasLoaded) { return; }

        UpdateGameStates();

        StartCoroutine(DelayStart(0.25f));
    }


    //--------------------


    IEnumerator DelayStart(float time)
    {
        yield return new WaitForSeconds(time);

        deleyedStart = true;
    }


    //--------------------


    void UpdateGameStates()
    {
        if (gameStates == GameStates.GameOver) { return; }

        //Set to Building

        if (HotbarManager.Instance.selectedItem != Items.None)
        {
            if (GetItem(HotbarManager.Instance.selectedItem).subCategoryName == ItemSubCategories.BuildingHammer)
            {
                gameStates = GameStates.Building;
            }
            else if (GetItem(HotbarManager.Instance.selectedItem).subCategoryName == ItemSubCategories.Axe)
            {
                gameStates = GameStates.Cutting;
            }
        }

        //Set to None
        else
        {
            gameStates = GameStates.None;
        }
    }


    //--------------------


    public Item GetItem(Items itemName)
    {
        for (int i = 0; i < item_SO.itemList.Count; i++)
        {
            if (item_SO.itemList[i].itemName == itemName)
            {
                return item_SO.itemList[i];
            }
        }

        return null;
    }


    //--------------------


    public BuildingBlockInfo GetMovableObject(BuildingBlockObjectNames buildingBlockObjectNames, BuildingMaterial buildingMaterial)
    {
        for (int i = 0; i < BuildingSystemManager.Instance.buildingBlocks_SO.buildingBlockObjectsList.Count; i++)
        {
            if (BuildingSystemManager.Instance.buildingBlocks_SO.buildingBlockObjectsList[i].blockName == buildingBlockObjectNames
                && BuildingSystemManager.Instance.buildingBlocks_SO.buildingBlockObjectsList[i].buildingMaterial == buildingMaterial)
            {
                return BuildingSystemManager.Instance.buildingBlocks_SO.buildingBlockObjectsList[i];
            }
        }

        return null;
    }
    public FurnitureInfo GetMovableObject(FurnitureObjectNames furnitureObjectName)
    {
        for (int i = 0; i < BuildingSystemManager.Instance.furniture_SO.furnitureObjectsList.Count; i++)
        {
            if (BuildingSystemManager.Instance.furniture_SO.furnitureObjectsList[i].furnitureName == furnitureObjectName)
            {
                return BuildingSystemManager.Instance.furniture_SO.furnitureObjectsList[i];
            }
        }

        return null;
    }
    public MachineInfo GetMovableObject(MachineObjectNames machineObjectName)
    {
        for (int i = 0; i < BuildingSystemManager.Instance.machines_SO.machineObjectsList.Count; i++)
        {
            if (BuildingSystemManager.Instance.machines_SO.machineObjectsList[i].machinesName == machineObjectName)
            {
                return BuildingSystemManager.Instance.machines_SO.machineObjectsList[i];
            }
        }

        return null;
    }


    //--------------------


    public MoveableObjectInfo GetMovableObject(FurnitureObjectNames furnitureType, MachineObjectNames machineType)
    {
        for (int i = 0; i < moveableObject_SO.moveableObjectList.Count; i++)
        {
            if (moveableObject_SO.moveableObjectList[i].furnitureType == furnitureType
                && moveableObject_SO.moveableObjectList[i].machineType == machineType)
            {
                return moveableObject_SO.moveableObjectList[i];
            }
        }

        return null;
    }


    //--------------------


    public void SaveData()
    {
        DataPersistanceManager.instance.SaveGame();
    }


    //--------------------


    public void SetDemoEndingText()
    {
        demoEndingText.SetActive(true);

        StartCoroutine(RemoveDemoEndingText(5));
    }

    IEnumerator RemoveDemoEndingText(float time)
    {
        yield return new WaitForSeconds(time);

        demoEndingText.SetActive(false);
    }
}

public enum MenuStates
{
    None,

    MainMenu,
    PauseMenu,
    SettingsMenu,
    InventoryMenu,
    ChestMenu,
    EquipmentMenu,
    MoveableObjectMenu,
    CraftingMenu,
    SkillTreeMenu,
    JournalMenu,
    ResearchMenu,

    CropPlotMenu
}

public enum GameStates
{
    None,

    Building,
    Cutting,

    GameOver
}