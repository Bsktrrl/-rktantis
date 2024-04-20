using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingObjects", menuName = "BuildingObjects", order = 1)]
public class BuildingBlocks_SO : ScriptableObject
{
    public List<BuildingBlockInfo> buildingBlockObjectsList = new List<BuildingBlockInfo>();
}

[Serializable]
public class BuildingBlockInfo
{
    [Header("General")]
    public string objectName;
    public BuildingBlockObjectNames blockName;
    public BuildingObjectTypes buildingObjectType = BuildingObjectTypes.BuildingBlock;
    public BuildingMaterial buildingMaterial;

    public BuildingObjectsInfo objectInfo;
}

[Serializable]
public class BuildingObjectsInfo
{
    public bool isActive;

    [Header("Sprite")]
    public Sprite objectSprite;

    [Header("Object")]
    public GameObject worldObject;

    [Header("Resources")]
    public List<CraftingRequirements> buildingRequirements = new List<CraftingRequirements>();
    public List<CraftingRequirements> removingReward = new List<CraftingRequirements>();
}