using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingBlockObject", menuName = "BuildingBlockObject", order = 1)]
public class BuildingBlockObjects_SO : ScriptableObject
{
    public List<BuildingBlockObject> buildingBlockObjectList = new List<BuildingBlockObject>();
}

[Serializable]
public class BuildingBlockObject
{
    [Header("Name")]
    public string Name;

    [Header("Type")]
    public BuildingBlockObjectNames BuildingType = BuildingBlockObjectNames.None;
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