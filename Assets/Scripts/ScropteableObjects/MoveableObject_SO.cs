using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveableObject", menuName = "MoveableObject", order = 1)]
public class MoveableObject_SO : ScriptableObject
{
    public List<MoveableObjectInfo> moveableObjectList = new List<MoveableObjectInfo>();
}

[Serializable]
public class MoveableObjectInfo
{
    [Header("Name")]
    public string Name;

    [Header("MoveableObjectTypes")]
    public BuildingObjectTypes moveableObjectType = BuildingObjectTypes.None;
    public FurnitureObjectNames furnitureType = FurnitureObjectNames.None;
    public MachineObjectNames machineType = MachineObjectNames.None;

    [Header("Sprite")]
    public Sprite objectSprite;

    [Header("ObjectToMove")]
    public GameObject objectToMove;

    [Header("Building Requirement")]
    public List<CraftingRequirements> craftingRequirements = new List<CraftingRequirements>();

    [Header("Requirement of Removing the Object")]
    public List<CraftingRequirements> RemoveCraftingRequirements = new List<CraftingRequirements>();
}