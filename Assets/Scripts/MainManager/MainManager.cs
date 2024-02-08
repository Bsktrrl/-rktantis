using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : Singleton<MainManager>
{
    [Header("Player")]
    public GameObject player;

    [Header("Game States")]
    public MenuStates menuStates;
    public GameStates gameStates;

    [Header("Item_SO")]
    public Item_SO item_SO;

    [Header("Parents")]
    public GameObject treeParent;

    //Update Delayer
    public int updateInterval = 10;

    //Interactable Distance - Distace between player and raycast interactable
    public float InteractableDistance = 2.5f;

    //CenterImage
    public GameObject centerImage;


    //--------------------


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        centerImage.SetActive(true);
    }
    private void Update()
    {
        UpdateGameStates();
    }


    //--------------------


    void UpdateGameStates()
    {
        //Set to Building

        if(HotbarManager.Instance.selectedItem != Items.None)
        {
            if (GetItem(HotbarManager.Instance.selectedItem).subCategoryName == ItemSubCategories.BuildingHammer)
            {
                gameStates = GameStates.Building;
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

    
    void SaveData()
    {
        DataPersistanceManager.instance.SaveGame();
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
    SkillTreeMenu
}

public enum GameStates
{
    None,

    Building,
    Cutting
}