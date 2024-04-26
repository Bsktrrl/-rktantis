using System.ComponentModel;
using UnityEngine;

public enum ItemCategories
{
    [Description("")] None,

    [Description("Materials")] Materials,
    [Description("Plant Materials")] PlantMaterials,
    [Description("Seeds")] Seeds,

    //Crafting Categories
    [Description("Crafting Materials")] CraftingMaterials,
    [Description("Tools")] Tools,
    [Description("Clothing")] Clothing,
    [Description("Food")] Food,
    [Description("Misc")] Misc,

    [Description("Machines")] Machines,
    [Description("Furniture")] Furniture,
    [Description("BuildingBlock")] BuildingBlock,


    [Description("Arídian Key")] ArídianKey,
    [Description("Drinkable")] Drinkable


    #region Old
    //[Description("Raw")] Raw,

    //[Description("Other")] Other,
    //[Description("Weapons")] Weapons,
    //[Description("Equipments")] Equipments,
    //[Description("Resources")] Resources,
    //[Description("Navigation")] Navigation,
    //[Description("Decoration")] Decoration
    #endregion
}

public enum ItemSubCategories
{
    //None
    [Description("")] None,


    //Materials
    [Description("Raw")][InspectorName("Materials/Raw")] Raw,
    [Description("Building Material")][InspectorName("Materials/Building Material")] BuildingMaterial,


    //PlantMaterials
    [Description("Liquid")][InspectorName("Plant Materials/Liquid")] Liquid,
    [Description("Cloth")][InspectorName("Plant Materials/Cloth")] Cloth,
    [Description("Flowers")][InspectorName("Plant Materials/Flower")] Flower,


    //Seeds
    [Description("Material Plants")][InspectorName("Seeds/Material Plants")] MaterialPlants,
    [Description("Food Plants")][InspectorName("Seeds/Food Plants")] FoodPlants,


    //CraftingMaterials
    [Description("Machine Parts")][InspectorName("Crafting Materials/Machine Parts")] MachineParts,
    [Description("Bio")][InspectorName("Crafting Materials/Bio")] Bio,
    [Description("Material")][InspectorName("Crafting Materials/Material")] Material,


    //Tools
    [Description("Building Hammer")][InspectorName("Tools/Building Hammer")] BuildingHammer,
    [Description("Axe")][InspectorName("Tools/Axe")] Axe,
    [Description("Pickaxe")][InspectorName("Tools/Pickaxe")] Pickaxe,
    [Description("Sword")][InspectorName("Tools/Sword")] Sword,
    [Description("Ghost Capturer")][InspectorName("Tools/Ghost Capturer")] GhostCapturer,


    //Clothing
    [Description("Head")][InspectorName("Clothing/Head")] Head,
    [Description("Hands")][InspectorName("Clothing/Hands")] Hands,
    [Description("Feet")][InspectorName("Clothing/Feet")] Feet,


    //Food
    [Description("Fruits")][InspectorName("Food/Fruits")] Fruits,
    [Description("Juice")][InspectorName("Food/Juice")] Juice,
    [Description("Grilled")][InspectorName("Food/Grilled")] Grilled,


    //Mics
    [Description("Perk Cubes")][InspectorName("Mics/Perk Cubes")] PerkCubes,
    [Description("Arídis")][InspectorName("Mics/Arídis")] Arídis,
    [Description("Other1")][InspectorName("Mics/Other1")] Other1,
    [Description("Other2")][InspectorName("Mics/Other2")] Other2,
    [Description("Other3")][InspectorName("Mics/Other3")] Other3,
    [Description("Other4")][InspectorName("Mics/Other4")] Other4,


    //Machines
    [Description("Tablet Connectors")][InspectorName("Machines/Tablet Connectors")] TabletConnectors,
    [Description("Ghost Essence")][InspectorName("Machines/Ghost Essence")] GhostEssence,
    [Description("Base Machines")][InspectorName("Machines/Base Machines")] BaseMachines,
    [Description("Grill")][InspectorName("Machines/Grill")] Grill,
    [Description("Crop Plots")][InspectorName("Machines/Crop Plots")] CropPlots,
    [Description("Battery")][InspectorName("Machines/Battery")] Battery,


    //Furniture
    [Description("Chests")][InspectorName("Furniture/Chests")] Chests,
    [Description("OnFloor")][InspectorName("Furniture/OnFloor")] OnFloor,
    [Description("OnWall")][InspectorName("Furniture/OnWall")] OnWall,


    //BuildingBlock
    [Description("Floor_Square")][InspectorName("BuildingBlock/Floor_Square")] Floor,
    [Description("Floor_Square Triangle")][InspectorName("BuildingBlock/Floor_Square Triangle")] FloorTriangle,

    [Description("Wall")][InspectorName("BuildingBlock/Wall")] Wall,
    [Description("Wall Triangle")][InspectorName("BuildingBlock/Wall Triangle")] WallTriangle,
    [Description("Wall_Door")][InspectorName("BuildingBlock/Wall_Door")] Door,
    [Description("Wall_Window")][InspectorName("BuildingBlock/Wall_Window")] Window,

    [Description("Ramp_Stair")][InspectorName("BuildingBlock/Ramp_Stair")] Stair,
    [Description("Ramp_Ramp")][InspectorName("BuildingBlock/Ramp_Ramp")] Ramp,
    [Description("Ramp_Ramp Triangle")][InspectorName("BuildingBlock/Ramp_Ramp Triangle")] RampTriangle,
    [Description("Ramp_Ramp Corner")][InspectorName("BuildingBlock/Ramp_Ramp Corner")] RampCorner,

    [Description("Fence")][InspectorName("BuildingBlock/Fence")] Fence,

    //Tools
    [Description("Light")][InspectorName("Tools/Light")] Light,

    //Drinking
    [Description("Drinking")][InspectorName("Tools/Drinking")] Drinking,

    //Tablet
    [Description("Tablet")][InspectorName("Tools/Tablet")] Tablet,

    //Tablet
    [Description("Arídian Key")][InspectorName("Arídian Key/Arídian Key")] ArídianKey,

    //Drinkable
    [Description("Cup")][InspectorName("Drinkable/Cup")] Cup,
    [Description("Bottle")][InspectorName("Drinkable/Bottle")] Bottle,
    [Description("Bucket")][InspectorName("Drinkable/Bucket")] Bucket,


    //--------------------


    #region Old
    ////Food
    //[Description("F_A")][InspectorName("Food/F_A")] F_A,
    //[Description("F_B")][InspectorName("Food/F_B")] F_B,
    //[Description("F_C")][InspectorName("Food/F_C")] F_C,
    //[Description("F_D")][InspectorName("Food/F_D")] F_D,

    ////Tools
    //[Description("Axe")][InspectorName("Tools/Axe")] Axe,
    //[Description("Building Hammer")][InspectorName("Tools/Building Hammer")] BuildingHammer,
    //[Description("Flashlight")][InspectorName("Tools/Flashlight")] Flashlight,

    ////Weapons
    //[Description("Weapons")][InspectorName("Weapons/Weapons")] Weapons,
    //[Description("W_B")][InspectorName("Weapons/W_B")] W_B,

    ////Other
    //[Description("Chests")][InspectorName("Chests/Chests")] Chests
    #endregion
}
