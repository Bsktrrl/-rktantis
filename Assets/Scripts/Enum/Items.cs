using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public enum Items
{
    [Description("")][InspectorName("None")] None,

    //Raw
    [Description("Stone")][InspectorName("Raw/Stone")] Stone,
    [Description("Plank")][InspectorName("Raw/Plank")] Plank,
    [Description("Leaf")][InspectorName("Raw/Leaf")] Leaf,

    //Tools
    [Description("Axe")][InspectorName("Tools/Axe")] Axe,
    [Description("Building Hammer")][InspectorName("Tools/Building Hammer")] BuildingHammer,

    //Chests/Storage
    [Description("Small Chest")][InspectorName("Storage/Small Chest")] SmallChest,
    [Description("Medium Chest")][InspectorName("Storage/Medium Chest")] MediumChest,

    //Machines
    [Description("Crafting Table")][InspectorName("Machines/Crafting Table")] CraftingTable,

    [Description("TempTest")][InspectorName("Other/TempTest")] tempTest,
    [Description("Flashlight")][InspectorName("Tools/Flashlight")] Flashlight,

    //Ghost Tank
    [Description("Ghost Tank")][InspectorName("Machines/Ghost Tank")] GhostTank,



    [Description("Extractor")][InspectorName("Machines/Extractor")] Extractor,
    [Description("Ghost Repeller")][InspectorName("Machines/Ghost Repeller")] GhostRepeller,
    [Description("HeatRegulator")][InspectorName("Machines/Heat Regulator")] HeatRegulator,
    [Description("Resource Converter")][InspectorName("Machines/Resource Converter")] ResourceConverter,

    [Description("Small Battery Charger")][InspectorName("Machines/Small Battery Charger")] BatteryCharger_1,
    [Description("Medium Battery Charger")][InspectorName("Machines/Medium Battery Charger")] BatteryCharger_2,
    [Description("Big Battery Charger")][InspectorName("Machines/Big Battery Charger")] BatteryCharger_3,

    [Description("Small Crop Plot")][InspectorName("Machines/Small Crop Plot")] CropPlot_1,
    [Description("Medium Crop Plot")][InspectorName("Machines/Medium Crop Plot")] CropPlot_2,
    [Description("Big Crop Plot")][InspectorName("Machines/Big Crop Plot")] CropPlot_3,

    [Description("Manual Small Grill")][InspectorName("Machines/Manual Small Grill")] Grill_Manual,
    [Description("Small Grill")][InspectorName("Machines/Small Grill")] Grill_1,
    [Description("Medium Grill")][InspectorName("Machines/Medium Grill")] Grill_2,
    [Description("Big Grill")][InspectorName("Machines/Big Grill")] Grill_4,

    [Description("Skill Tree Table")][InspectorName("Machines/Skill Tree Table")] SkillTreeTable
}
