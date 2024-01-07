using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

[System.Serializable]
public class DataManager : MonoBehaviour, IDataPersistance
{
    public static DataManager instance { get; private set; } //Singleton

    public static Action dataIsSaving;
    public static Action datahasLoaded;


    //--------------------


    //Player Pos and Rotation
    [HideInInspector] public Vector3 playerPos_Store = new Vector3();
    [HideInInspector] public Quaternion playerRot_Store = new Quaternion();

    //WorldObjects
    [HideInInspector] public List<WorldObject> worldObject_StoreList = new List<WorldObject>();

    //Inventories
    [HideInInspector] public List<Inventory> Inventories_StoreList = new List<Inventory>();

    //Hotbar
    [HideInInspector] public int selectedSlot_Store = new int();
    public List<Items> hotbarItem_StoreList = new List<Items>();

    //BuidingSystem
    [HideInInspector] public List<BuildingBlockSaveList> buildingBlockList_StoreList = new List<BuildingBlockSaveList>();
    [HideInInspector] public BuildingType buildingType_Store = new BuildingType();
    [HideInInspector] public BuildingMaterial buildingMaterial_Store = new BuildingMaterial();



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


    //--------------------


    public void LoadData(GameData gameData)
    {
        //Input what to store
        this.playerPos_Store = gameData.playerPos_Save;
        this.playerRot_Store = gameData.playerRot_Save;

        this.worldObject_StoreList = gameData.worldObject_SaveList;

        this.Inventories_StoreList = gameData.Inventories_SaveList;

        this.hotbarItem_StoreList = gameData.hotbarItem_SaveList;
        this.selectedSlot_Store = gameData.selectedSlot_Save;

        this.buildingBlockList_StoreList = gameData.buildingBlockList_SaveList;
        this.buildingType_Store = gameData.buildingType_Save;
        this.buildingMaterial_Store = gameData.buildingMaterial_Save;

        datahasLoaded?.Invoke();

        print("Data has Loaded");
    }

    public void SaveData(ref GameData gameData)
    {
        dataIsSaving?.Invoke();

        //Input what to save
        gameData.playerPos_Save = MainManager.instance.player.transform.position;
        gameData.playerRot_Save = MainManager.instance.player.transform.rotation;

        gameData.worldObject_SaveList = this.worldObject_StoreList;

        gameData.Inventories_SaveList = this.Inventories_StoreList;

        gameData.hotbarItem_SaveList = this.hotbarItem_StoreList;
        gameData.selectedSlot_Save = this.selectedSlot_Store;

        gameData.buildingBlockList_SaveList = this.buildingBlockList_StoreList;
        gameData.buildingType_Save = this.buildingType_Store;
        gameData.buildingMaterial_Save = this.buildingMaterial_Store;

        print("Data has Saved");
    }
}