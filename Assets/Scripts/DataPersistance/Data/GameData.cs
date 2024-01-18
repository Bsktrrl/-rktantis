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

    //WorldObjects
    [HideInInspector] public List<WorldObject> worldObject_SaveList = new List<WorldObject>();

    //BuidingSystem
    [HideInInspector] public List<BuildingBlockSaveList> buildingBlockList_SaveList = new List<BuildingBlockSaveList>();

    //Hotbar
    [HideInInspector] public int selectedSlot_Save = new int();
    public List<Items> hotbarItem_SaveList = new List<Items>();

    //MoveableObjects
    [HideInInspector] public List<MoveableObject_ToSave> placedMoveableObjectsList_SaveList = new List<MoveableObject_ToSave>();
    [HideInInspector] public MoveableObjectSelected_ToSave moveableObjectSelected_Save = new MoveableObjectSelected_ToSave();


    //--------------------


    public GameData()
    {
        //Input All Lists to clear
        this.worldObject_SaveList.Clear();

        this.buildingBlockList_SaveList.Clear();

        this.Inventories_SaveList.Clear();
        this.hotbarItem_SaveList.Clear();
        this.placedMoveableObjectsList_SaveList.Clear();
    }
}
