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

    Bed
}

public enum MachineType
{
    None,

    CraftingTable,
    SkillTreeTable,
    GhostTank,
    Extractor,
    GhostRepeller,
    HeatRegulator,
    ResourceConverter,

    BatteryCharger_1,
    BatteryCharger_2,
    BatteryCharger_3,

    CropPlot_1,
    CropPlot_2,
    CropPlot_3,

    Grill_Manual,
    Grill_1,
    Grill_2,
    Grill_4
}