using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingType
{
    None,

    Floor,
    Floor_Triangle,

    Wall,
    Wall_Diagonaly,

    Ramp,
    Ramp_Corner,
    Wall_Triangle,

    Fence,
    Fence_Diagonaly,

    Window,
    Door,
    Stair,
    Ramp_Triangle
}
public enum BuildingSubType
{
    None,

    Diagonaly
}

public enum BuildingMaterial
{
    None,

    Wood,
    Stone,
    Iron
}

public enum MoveableObjectType
{
    None,

    BuildingBlock,
    Furniture,
    Machine
}

public enum FurnitureType
{
    None,

    Small_StorageChest,
    Medium_StorageChest,
    Big_StorageChest,
}

public enum MachineType
{
    None,

    CraftingTable,
    SkillTreeTable,
    GhostTank
}