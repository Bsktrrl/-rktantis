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
    [Description("Small Chest")][InspectorName("Storage/SmallChest")] SmallChest,
    [Description("Medium Chest")][InspectorName("Storage/MediumChest")] MediumChest,

    //Machines
    [Description("Crafting Table")][InspectorName("Machines/CraftingTable")] CraftingTable,

    [Description("TempTest")][InspectorName("Other/TempTest")] tempTest

}
