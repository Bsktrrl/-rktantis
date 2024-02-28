using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingBlockObject", menuName = "BuildingBlockObject", order = 1)]
public class BuildingBlockObjects_SO : ScriptableObject
{
    public List<BuildingblockObject> buildingBlockObjectList = new List<BuildingblockObject>();
}

[Serializable]
public class BuildingblockObject
{
    [Header("Name")]
    public string Name;

    [Header("Type")]
    public BuildingType BuildingType = BuildingType.None;
    public BuildingMaterial buildingMaterial = BuildingMaterial.None;

    [Header("Sprite")]
    public Sprite objectSprite;

    [Header("ObjectToMove")]
    public GameObject objectToMove;

    [Header("Building Requirement")]
    public List<CraftingRequirements> craftingRequirements = new List<CraftingRequirements>();

    [Header("Requirement of Removing the Object")]
    public List<CraftingRequirements> RemoveCraftingRequirements = new List<CraftingRequirements>();
}