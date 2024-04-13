using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
    [Description("None")][InspectorName("None")] None,

    [Description("Crafting Table")][InspectorName("Crafting Table")] CraftingTable,
    [Description("Skill Table")][InspectorName("Skill Table")] SkillTreeTable,

    [Description("Small Chest")][InspectorName("Small Chest")] SmallChest,
    [Description("Big Chest")][InspectorName("Big Chest")] BigChest,

    [Description("Lamp")][InspectorName("Lamp")] Lamp,
    [Description("Spotlight")][InspectorName("Spotlight")] Spotlight,

    [Description("Research Table")][InspectorName("Research Table")] ResearchTable,

    [Description("Other2")][InspectorName("Other2")] FU_Other2,
    [Description("Other3")][InspectorName("Other3")] FU_Other3,
    [Description("Other4")][InspectorName("Other4")] FU_Other4,
    [Description("Other5")][InspectorName("Other5")] FU_Other5
}

public enum MachineType
{
    [Description("None")][InspectorName("None")] None,

    [Description("Crop Plot x1")][InspectorName("Crop Plot x1")] CropPlot_x1,
    [Description("Crop Plot x2")][InspectorName("Crop Plot x2")] CropPlot_x2,
    [Description("Crop Plot x4")][InspectorName("Crop Plot x4")] CropPlot_x4,

    [Description("Grill x1")][InspectorName("Grill x1")] Grill_x1,
    [Description("Grill x2")][InspectorName("Grill x2")] Grill_x2,
    [Description("Grill x4")][InspectorName("Grill x4")] Grill_x4,

    [Description("Extractor")][InspectorName("Extractor")] Extractor,
    [Description("Heat Regulator")][InspectorName("Heat Regulator")] HeatRegulator,
    [Description("Blender")][InspectorName("Blender")] Blender,
    [Description("Resource Converter")][InspectorName("Resource Converter")] ResourceConverter,
    [Description("Ghost Tank")][InspectorName("Ghost Tank")] GhostTank,
    [Description("Energy Storage Tank")][InspectorName("Energy Storage Tank")] EnergyStorageTank,
    [Description("Ghost Repeller")][InspectorName("Ghost Repeller")] GhostRepeller,

    [Description("Battery x1")][InspectorName("Battery x1")] Battery_x1,
    [Description("Battery x2")][InspectorName("Battery x2")] Battery_x2,
    [Description("Battery x3")][InspectorName("Battery x3")] Battery_x3,

    [Description("Other1")][InspectorName("Other1")] MA_Other1,
    [Description("Other2")][InspectorName("Other2")] MA_Other2,
    [Description("Other3")][InspectorName("Other3")] MA_Other3,
    [Description("Other4")][InspectorName("Other4")] MA_Other4,
    [Description("Other5")][InspectorName("Other5")] MA_Other5
}