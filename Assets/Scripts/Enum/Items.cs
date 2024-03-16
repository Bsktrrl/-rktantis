using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public enum Items
{
    //None
    #region
    [Description("")][InspectorName("None")] None,
    #endregion


    //Materials
    #region
    [Description("Tungsten")][InspectorName("Materials/Tungsten")] Tungsten,
    [Description("Gold")][InspectorName("Materials/Gold")] Gold,
    [Description("Viridian")][InspectorName("Materials/Viridian")] Viridian,
    [Description("Magnetite")][InspectorName("Materials/Magnetite")] Magnetite,
    [Description("Arídite Crystal")][InspectorName("Materials/Arídite Crystal")] AríditeCrystal,
    [Description("Wood")][InspectorName("Materials/Wood")] Wood,
    [Description("Stone")][InspectorName("Materials/Stone")] Stone,
    [Description("Cryonite")][InspectorName("Materials/Cryonite")] Cryonite,

    [Description("Other1")][InspectorName("Materials/Other1")] M_Other1,
    [Description("Other2")][InspectorName("Materials/Other2")] M_Other2,
    [Description("Other3")][InspectorName("Materials/Other3")] M_Other3,
    [Description("Other4")][InspectorName("Materials/Other4")] M_Other4,
    [Description("Other5")][InspectorName("Materials/Other5")] M_Other5,
    #endregion


    //Crafting Materials
    #region
    [Description("Perk Cube Tier 1")][InspectorName("Crafting Materials/Perk Cube Tier 1")] PerkCube_Tier1,
    [Description("Perk Cube Tier 2")][InspectorName("Crafting Materials/Perk Cube Tier 2")] PerkCube_Tier2,
    [Description("Perk Cube Tier 3")][InspectorName("Crafting Materials/Perk Cube Tier 3")] PerkCube_Tier3,
    [Description("Power Core")][InspectorName("Crafting Materials/Power Core")] PowerCore,
    [Description("Shard of Arídis")][InspectorName("Crafting Materials/Shard of Arídis")] ShardOfArídis,

    [Description("Other1")][InspectorName("Crafting Materials/Other1")] CM_Other1,
    [Description("Other2")][InspectorName("Crafting Materials/Other2")] CM_Other2,
    [Description("Other3")][InspectorName("Crafting Materials/Other3")] CM_Other3,
    [Description("Other4")][InspectorName("Crafting Materials/Other4")] CM_Other4,
    [Description("Other5")][InspectorName("Crafting Materials/Other5")] CM_Other5,
    [Description("Other6")][InspectorName("Crafting Materials/Other6")] CM_Other6,
    [Description("Other7")][InspectorName("Crafting Materials/Other7")] CM_Other7,
    [Description("Other8")][InspectorName("Crafting Materials/Other8")] CM_Other8,
    [Description("Other9")][InspectorName("Crafting Materials/Other9")] CM_Other9,
    [Description("Other10")][InspectorName("Crafting Materials/Other10")] CM_Other10,
    [Description("Other11")][InspectorName("Crafting Materials/Other11")] CM_Other11,
    [Description("Other12")][InspectorName("Crafting Materials/Other12")] CM_Other12,
    [Description("Other13")][InspectorName("Crafting Materials/Other13")] CM_Other13,
    [Description("Other14")][InspectorName("Crafting Materials/Other14")] CM_Other14,
    [Description("Other15")][InspectorName("Crafting Materials/Other15")] CM_Other15,
    #endregion


    //Tools
    #region
    [Description("Wood Axe")][InspectorName("Tools/Wood Axe")] WoodAxe,
    [Description("Wood Pickaxe")][InspectorName("Tools/Wood Pickaxe")] WoodPickaxe,
    [Description("Wood Sword")][InspectorName("Tools/Wood Sword")] WoodSword,
    [Description("Wood Building Hammer")][InspectorName("Tools/Wood Building Hammer")] WoodBuildingHammer,

    [Description("Stone Axe")][InspectorName("Tools/Stone Axe")] StoneAxe,
    [Description("Stone Pickaxe")][InspectorName("Tools/Stone Pickaxe")] StonePickaxe,
    [Description("Stone Sword")][InspectorName("Tools/Stone Sword")] StoneSword,
    [Description("Stone Building Hammer")][InspectorName("Tools/Stone Building Hammer")] StoneBuildingHammer,

    [Description("Cryonite Axe")][InspectorName("Tools/Cryonite Axe")] CryoniteAxe,
    [Description("Cryonite Pickaxe")][InspectorName("Tools/Cryonite Pickaxe")] CryonitePickaxe,
    [Description("Cryonite Sword")][InspectorName("Tools/Cryonite Sword")] CryoniteSword,
    [Description("Cryonite Building Hammer")][InspectorName("Tools/Cryonite Building Hammer")] CryoniteBuildingHammer,

    [Description("Flashlight")][InspectorName("Tools/Flashlight")] Flashlight,

    [Description("Other2")][InspectorName("Tools/Other2")] T_Other2,
    [Description("Other3")][InspectorName("Tools/Other3")] T_Other3,
    [Description("Other4")][InspectorName("Tools/Other4")] T_Other4,
    [Description("Other5")][InspectorName("Tools/Other5")] T_Other5,
    #endregion


    //Clothing
    #region
    [Description("Auto Feeder")][InspectorName("Clothing/Auto Feeder")] AutoFeeder,
    [Description("Headlight")][InspectorName("Clothing/Headlight")] Headlight,
    [Description("Helmet")][InspectorName("Clothing/Helmet")] Helmet,

    [Description("Mining Gloves")][InspectorName("Clothing/Mining Gloves")] MiningGloves,
    [Description("Power Gloves")][InspectorName("Clothing/Power Gloves")] PowerGloves,
    [Description("Construction Gloves")][InspectorName("Clothing/Construction Gloves")] ConstructionGloves,

    [Description("Running Shoes")][InspectorName("Clothing/Running Shoes")] RunningShoes,
    [Description("Light Shoes")][InspectorName("Clothing/Light Shoes")] LightShoes,
    [Description("Slippers")][InspectorName("Clothing/Slippers")] Slippers,

    [Description("Cloth")][InspectorName("Clothing/Cloth")] Cloth,
    [Description("Other2")][InspectorName("Clothing/Other2")] CL_Other2,
    [Description("Other3")][InspectorName("Clothing/Other3")] CL_Other3,
    [Description("Other4")][InspectorName("Clothing/Other4")] CL_Other4,
    [Description("Other5")][InspectorName("Clothing/Other5")] CL_Other5,
    #endregion


    //Plant Material
    #region
    [Description("Plant Fiber")][InspectorName("Plant Material/Plant Fiber")] PlantFiber,
    [Description("Glue Stick")][InspectorName("Plant Material/Glue Stick")] GlueStick,
    [Description("Cotton")][InspectorName("Plant Material/Cotton")] Cotton,
    [Description("Spik Oil")][InspectorName("Plant Material/Spik Oil")] SpikOil,
    [Description("Arídis Flower")][InspectorName("Plant Material/Arídis Flower")] ArídisFlower,
    [Description("Twisted Mushroom")][InspectorName("Plant Material/Twisted Mushroom")] TwistedMushroom,
    [Description("TubePlastic")][InspectorName("Plant Material/Tube Plastic")] TubePlastic,

    [Description("Other2")][InspectorName("Plant Material/Other2")] PM_Other2,
    [Description("Other3")][InspectorName("Plant Material/Other3")] PM_Other3,
    [Description("Other4")][InspectorName("Plant Material/Other4")] PM_Other4,
    [Description("Other5")][InspectorName("Plant Material/Other5")] PM_Other5,
    #endregion


    //Seed
    #region
    [Description("Arídis Plant Seed")][InspectorName("Seed/Arídis Plant Seed")] ArídisPlantSeed,
    [Description("Glue Plant Seed")][InspectorName("Seed/Glue Plant Seed")] GluePlantSeed,
    [Description("Crimson Cloud Bush Seed")][InspectorName("Seed/Crimson Cloud Bush Seed")] CrimsonCloudBushSeed,
    [Description("Red Cotton Plant Seed")][InspectorName("Seed/Red Cotton Plant Seed")] RedCottonPlantSeed,
    [Description("Spik Plant Seed")][InspectorName("Seed/Spik Plant Seed")] SpikPlantSeed,

    [Description("Small Cactus Plant Seed")][InspectorName("Seed/Small Cactus Plant Seed")] SmallCactusplantSeed,
    [Description("Large Cactus Plant Seed")][InspectorName("Seed/Large Cactus Plant Seed")] LargeCactusplantSeed,
    [Description("Pudding Cactus Seed")][InspectorName("Seed/Pudding Cactus Seed")] PuddingCactusSeed,
    [Description("Stalk Fruit Seed")][InspectorName("Seed/Stalk Fruit Seed")] StalkFruitSeed,
    [Description("Tripod Fruit Seed")][InspectorName("Seed/Tripod Fruit Seed")] TripodFruitSeed,
    [Description("Heat Fruit Seed")][InspectorName("Seed/Heat Fruit Seed")] HeatFruitSeed,
    [Description("Freeze Fruit Seed")][InspectorName("Seed/Freeze Fruit Seed")] FreezeFruitSeed,

    [Description("Twisted Mushroom Seed")][InspectorName("Seed/Twisted Mushroom Seed")] TwistedMushroomSeed,
    [Description("Ground Mushroom Seed")][InspectorName("Seed/Ground Mushroom Seed")] GroundMushroomSeed,

    [Description("Other1")][InspectorName("Seed/Other1")] S_Other1,
    [Description("Other2")][InspectorName("Seed/Other2")] S_Other2,
    [Description("Other3")][InspectorName("Seed/Other3")] S_Other3,
    [Description("Other4")][InspectorName("Seed/Other4")] S_Other4,
    [Description("Other5")][InspectorName("Seed/Other5")] S_Other5,
    #endregion


    //Food
    #region
    [Description("Cactus")][InspectorName("Food/Cactus")] Cactus,
    [Description("Pudding Cactus")][InspectorName("Food/Pudding Cactus")] PuddingCactus,
    [Description("Stalk Fruit")][InspectorName("Food/Stalk Fruit")] StalkFruit,
    [Description("Tripod Fruit")][InspectorName("Food/Tripod Fruit")] TripodFruit,
    [Description("Heat Fruit")][InspectorName("Food/Heat Fruit")] HeatFruit,
    [Description("Freeze Fruit")][InspectorName("Food/Freeze Fruit")] FreezeFruit,

    [Description("Other1")][InspectorName("Food/Other1")] F_Other1,
    [Description("Other2")][InspectorName("Food/Other2")] F_Other2,
    [Description("Other3")][InspectorName("Food/Other3")] F_Other3,
    [Description("Other4")][InspectorName("Food/Other4")] F_Other4,
    [Description("Other5")][InspectorName("Food/Other5")] F_Other5,
    #endregion


    //Juice
    #region
    [Description("Cactus Juice")][InspectorName("Juice/Cactus Juice")] CactusJuice,
    [Description("Thripod Fruit Juice")][InspectorName("Juice/Thripod Fruit Juice")] TripodFruitJuice,
    [Description("Pudding Cactus Juice")][InspectorName("Juice/Pudding Cactus Juice")] PuddingCactusJuice,
    [Description("Stalk Fruit Juice")][InspectorName("Juice/Stalk Fruit Juice")] StalkFruitJuice,
    [Description("Heat Fruit Juice")][InspectorName("Juice/Heat Fruit Juice")] HeatFruitJuice,
    [Description("Freeze Fruit Juice")][InspectorName("Juice/Freeze Fruit Juice")] FreezeFruitJuice,

    [Description("Other1")][InspectorName("Juice/Other1")] J_Other1,
    [Description("Other2")][InspectorName("Juice/Other2")] J_Other2,
    [Description("Other3")][InspectorName("Juice/Other3")] J_Other3,
    [Description("Other4")][InspectorName("Juice/Other4")] J_Other4,
    [Description("Other5")][InspectorName("Juice/Other5")] J_Other5,
    #endregion


    //Grilled
    #region
    [Description("Grilled Cactus")][InspectorName("Grilled/Grilled Cactus")] GrilledCactus,
    [Description("Grilled Tripod Fruit")][InspectorName("Grilled/Grilled Tripod Fruit")] GrilledTripodFruit,
    [Description("Grilled Pudding Cactus")][InspectorName("Grilled/Grilled Pudding Cactus")] GrilledPuddingCactus,
    [Description("Grilled Stalk Fruit")][InspectorName("Grilled/Grilled Stalk Fruit")] GrilledStalkFruit,
    [Description("Grilled Heat Fruit")][InspectorName("Grilled/Grilled Heat Fruit")] GrilledHeatFruit,
    [Description("Grilled Freeze Fruit")][InspectorName("Grilled/Grilled Freeze Fruit")] GrilledFreezeFruit,

    [Description("Other1")][InspectorName("Grilled/Other1")] G_Other1,
    [Description("Other2")][InspectorName("Grilled/Other2")] G_Other2,
    [Description("Other3")][InspectorName("Grilled/Other3")] G_Other3,
    [Description("Other4")][InspectorName("Grilled/Other4")] G_Other4,
    [Description("Other5")][InspectorName("Grilled/Other5")] G_Other5,
    #endregion


    //Machines
    #region
    [Description("Small Crop Plot")][InspectorName("Machines/Small Crop Plot")] SmallCropPlot,
    [Description("Medium Crop Plot")][InspectorName("Machines/Medium Crop Plot")] MediumCropPlot,
    [Description("Large Crop Plot")][InspectorName("Machines/Large Crop Plot")] LargeCropPlot,

    [Description("Small Grill")][InspectorName("Machines/Small Grill")] SmallGrill,
    [Description("Medium Grill")][InspectorName("Machines/Medium Grill")] MediumGrill,
    [Description("Large Grill")][InspectorName("Machines/Large Grill")] LargeGrill,

    [Description("Extractor")][InspectorName("Machines/Extractor")] Extractor,
    [Description("Heat Regulator")][InspectorName("Machines/Heat Regulator")] HeatRegulator,
    [Description("Blender")][InspectorName("Machines/Blender")] Blender,
    [Description("Resource Converter")][InspectorName("Machines/Resource Converter")] ResourceConverter,
    [Description("Ghost Tank")][InspectorName("Machines/Ghost Tank")] GhostTank,
    [Description("Energy Storage Tank")][InspectorName("Machines/Energy Storage Tank")] EnergyStorageTank,
    [Description("Ghost Repeller")][InspectorName("Machines/Ghost Repeller")] GhostRepeller,

    [Description("Small Battery")][InspectorName("Machines/Small Battery")] SmallBattery,
    [Description("Medium Battery")][InspectorName("Machines/Medium Battery")] MediumBattery,
    [Description("Large Battery")][InspectorName("Machines/Large Battery")] LargeBattery,

    [Description("Other1")][InspectorName("Machines/Other1")] MA_Other1,
    [Description("Other2")][InspectorName("Machines/Other2")] MA_Other2,
    [Description("Other3")][InspectorName("Machines/Other3")] MA_Other3,
    [Description("Other4")][InspectorName("Machines/Other4")] MA_Other4,
    [Description("Other5")][InspectorName("Machines/Other5")] MA_Other5,
    #endregion


    //Furniture
    #region
    [Description("Crafting Table")][InspectorName("Furniture/Crafting Table")] CraftingTable,
    [Description("Skill Table")][InspectorName("Furniture/Skill Table")] SkillTable,

    [Description("Small Chest")][InspectorName("Furniture/Small Chest")] SmallChest,
    [Description("Big Chest")][InspectorName("Furniture/Big Chest")] BigChest,

    [Description("Lamp")][InspectorName("Furniture/Lamp")] Lamp,
    [Description("Spotlight")][InspectorName("Furniture/Spotlight")] Spotlight,

    [Description("Medium Chest")][InspectorName("Furniture/Medium Chest")] MediumChest,
    [Description("Other2")][InspectorName("Furniture/Other2")] FU_Other2,
    [Description("Other3")][InspectorName("Furniture/Other3")] FU_Other3,
    [Description("Other4")][InspectorName("Furniture/Other4")] FU_Other4,
    [Description("Other5")][InspectorName("Furniture/Other5")] FU_Other5,
    #endregion
    
    //Drinking
    #region
    [Description("Cup")][InspectorName("Drinking/Cup")] Cup,
    [Description("Bottle")][InspectorName("Drinking/Bottle")] Bottle,
    [Description("Bucket")][InspectorName("Drinking/Bucket")] Bucket,

    [Description("Other1")][InspectorName("Drinking/Other1")] D_Other1,
    [Description("Other2")][InspectorName("Drinking/Other2")] D_Other2,
    [Description("Other3")][InspectorName("Drinking/Other3")] D_Other3,
    [Description("Other4")][InspectorName("Drinking/Other4")] D_Other4,
    #endregion
}
