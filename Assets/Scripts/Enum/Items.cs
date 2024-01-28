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
    [Description("Ghost Tank")][InspectorName("Machines/Ghost Tank")] GhostTank

}
