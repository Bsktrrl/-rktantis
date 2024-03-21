using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    //Player Pos and Rotation
    [HideInInspector] public Vector3 playerPos_Save = new Vector3();
    [HideInInspector] public Quaternion playerRot_Save = new Quaternion();
    
    //Inventories
    public List<Inventory> Inventories_SaveList = new List<Inventory>();
    [HideInInspector] public Vector2 smallChest_Size_Save;
    [HideInInspector] public Vector2 bigChest_Size_Save;

    //MenuEquipment
    public List<Items> menuEquipedItemList_SaveList = new List<Items>();

    //WorldObjects
    [HideInInspector] public List<WorldObject> worldObject_SaveList = new List<WorldObject>();

    //BuidingSystem
    [HideInInspector] public List<BuildingBlockSaveList> buildingBlockList_SaveList = new List<BuildingBlockSaveList>();

    //Hotbar
    [HideInInspector] public int selectedSlot_Save = new int();
    public List<Hotbar> hotbarItem_SaveList = new List<Hotbar>();

    //MoveableObjects
    [HideInInspector] public List<MoveableObject_ToSave> placedMoveableObjectsList_SaveList = new List<MoveableObject_ToSave>();
    [HideInInspector] public MoveableObjectSelected_ToSave moveableObjectSelected_Save = new MoveableObjectSelected_ToSave();

    //Plants
    [HideInInspector] public List<ListOfPlantToSave> plantTypeObjectList_Save = new List<ListOfPlantToSave>();

    //Ore
    [HideInInspector] public List<ListOfOreToSave> oreTypeObjectList_Save = new List<ListOfOreToSave>();

    //HealthParameter
    [HideInInspector] public HealthToSave health_Save = new HealthToSave();

    //Time
    [HideInInspector] public float currentTime_Save = new float();
    [HideInInspector] public int day_Save = new int();


    //--------------------


    public GameData()
    {
        //Input All Lists to clear
        this.worldObject_SaveList.Clear();

        this.buildingBlockList_SaveList.Clear();

        this.Inventories_SaveList.Clear();
        this.hotbarItem_SaveList.Clear();
        this.placedMoveableObjectsList_SaveList.Clear();

        this.menuEquipedItemList_SaveList.Clear();

        this.plantTypeObjectList_Save.Clear();
        this.oreTypeObjectList_Save.Clear();
    }
}
