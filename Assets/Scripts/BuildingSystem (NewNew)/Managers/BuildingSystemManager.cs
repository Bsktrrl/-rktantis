using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingSystemManager : Singleton<BuildingSystemManager>
{
    [Header("_SO")]
    [SerializeField] BuildingBlocks_SO buildingBlocks_SO;
    [SerializeField] Furniture_SO furniture_SO;
    [SerializeField] Machines_SO machines_SO;

    [Header("Active BuildingObject")] //From the BuildingObject Menu - Tablet
    public ActiveBuildingObject activeBuildingObject_Info;

    public List<bool> activeBuildingBlockObject_SOList = new List<bool>();
    public List<bool> activeFurnitureObject_SOList = new List<bool>();
    public List<bool> activeMachineObject_SOList = new List<bool>();

    [Header("Have enough items to Build?")]
    public bool enoughItemsToBuild;

    //--------------------


    private void Start()
    {
        //Setup _SO isActive Lists
        #region
        for (int i = 0; i < buildingBlocks_SO.buildingBlockObjectsList.Count; i++)
        {
            if (buildingBlocks_SO.buildingBlockObjectsList[i].objectInfo.isActive)
            {
                activeBuildingBlockObject_SOList.Add(true);
            }
            else
            {
                activeBuildingBlockObject_SOList.Add(false);
            }
        }

        for (int i = 0; i < furniture_SO.furnitureObjectsList.Count; i++)
        {
            if (furniture_SO.furnitureObjectsList[i].objectInfo.isActive)
            {
                activeFurnitureObject_SOList.Add(true);
            }
            else
            {
                activeFurnitureObject_SOList.Add(false);
            }
        }

        for (int i = 0; i < machines_SO.machineObjectsList.Count; i++)
        {
            if (machines_SO.machineObjectsList[i].objectInfo.isActive)
            {
                activeMachineObject_SOList.Add(true);
            }
            else
            {
                activeMachineObject_SOList.Add(false);
            }
        }
        #endregion
    }


    //--------------------


    public void LoadData()
    {
        activeBuildingObject_Info = DataManager.Instance.activeBuildingObject_Store;

        BuildingDisplayManager.Instance.UpdateScreenBuildingDisplayInfo();

        //Load _SOLists
        #region
        if (DataManager.Instance.activeBuildingBlockObject_SOList_Store.Count > 0)
        {
            activeBuildingBlockObject_SOList = DataManager.Instance.activeBuildingBlockObject_SOList_Store;

            for (int i = 0; i < buildingBlocks_SO.buildingBlockObjectsList.Count; i++)
            {
                buildingBlocks_SO.buildingBlockObjectsList[i].objectInfo.isActive = activeBuildingBlockObject_SOList[i];
            }
        }
        if (DataManager.Instance.activeFurnitureObject_SOList_Store.Count > 0)
        {
            activeFurnitureObject_SOList = DataManager.Instance.activeFurnitureObject_SOList_Store;

            for (int i = 0; i < furniture_SO.furnitureObjectsList.Count; i++)
            {
                furniture_SO.furnitureObjectsList[i].objectInfo.isActive = activeFurnitureObject_SOList[i];
            }
        }
        if (DataManager.Instance.activeMachineObject_SOList_Store.Count > 0)
        {
            activeMachineObject_SOList = DataManager.Instance.activeMachineObject_SOList_Store;

            for (int i = 0; i < machines_SO.machineObjectsList.Count; i++)
            {
                machines_SO.machineObjectsList[i].objectInfo.isActive = activeMachineObject_SOList[i];
            }
        }
        #endregion

        SaveData();
    }
    public void SaveData()
    {
        DataManager.Instance.activeBuildingObject_Store = activeBuildingObject_Info;

        DataManager.Instance.activeBuildingBlockObject_SOList_Store = activeBuildingBlockObject_SOList;
        DataManager.Instance.activeFurnitureObject_SOList_Store = activeFurnitureObject_SOList;
        DataManager.Instance.activeMachineObject_SOList_Store = activeMachineObject_SOList;
    }


    //--------------------


    //Get BuildingObjectInfo
    #region
    public BuildingBlockInfo GetBuildingObjectInfo(BuildingBlockObjectNames objectName, BuildingMaterial buildingMaterial)
    {
        for (int i = 0; i < buildingBlocks_SO.buildingBlockObjectsList.Count; i++)
        {
            if (buildingBlocks_SO.buildingBlockObjectsList[i].blockName == objectName
                && buildingBlocks_SO.buildingBlockObjectsList[i].buildingMaterial == buildingMaterial)
            {
                return buildingBlocks_SO.buildingBlockObjectsList[i];
            }
        }

        return null;
    }
    public FurnitureInfo GetBuildingObjectInfo(FurnitureObjectNames objectName)
    {
        for (int i = 0; i < furniture_SO.furnitureObjectsList.Count; i++)
        {
            if (furniture_SO.furnitureObjectsList[i].furnitureName == objectName)
            {
                return furniture_SO.furnitureObjectsList[i];
            }
        }

        return null;
    }
    public MachineInfo GetBuildingObjectInfo(MachineObjectNames objectName)
    {
        for (int i = 0; i < machines_SO.machineObjectsList.Count; i++)
        {
            if (machines_SO.machineObjectsList[i].machinesName == objectName)
            {
                return machines_SO.machineObjectsList[i];
            }
        }

        return null;
    }
    #endregion


    //--------------------

    public void SetActive_SOList(BuildingBlockObjectNames buildingBlockObjectName, BuildingMaterial buildingMaterial)
    {
        for (int i = 0; i < buildingBlocks_SO.buildingBlockObjectsList.Count; i++)
        {
            buildingBlocks_SO.buildingBlockObjectsList[i].objectInfo.isActive = true;
            activeBuildingBlockObject_SOList[i] = true;
        }

        SaveData();
    }
    public void SetActive_SOList(FurnitureObjectNames furnitureObjectName)
    {
        for (int i = 0; i < furniture_SO.furnitureObjectsList.Count; i++)
        {
            furniture_SO.furnitureObjectsList[i].objectInfo.isActive = true;
            activeFurnitureObject_SOList[i] = true;
        }

        SaveData();
    }
    public void SetActive_SOList(MachineObjectNames machineObjectName)
    {
        for (int i = 0; i < machines_SO.machineObjectsList.Count; i++)
        {
            machines_SO.machineObjectsList[i].objectInfo.isActive = true;
            activeMachineObject_SOList[i] = true;
        }

        SaveData();
    }
}

[Serializable]
public class ActiveBuildingObject
{
    public BuildingObjectTypes buildingObjectType_Active;
    public BuildingMaterial buildingMaterial_Active;

    public BuildingBlockObjectNames buildingBlockObjectName_Active;
    public FurnitureObjectNames furnitureObjectName_Active;
    public MachineObjectNames machineObjectName_Active;
}

public enum BuildingObjectTypes
{
    None,

    BuildingBlock,
    Furniture,
    Machine
}

public enum BuildingMaterial
{
    None,

    Wood,
    Stone,
    Cryonite
}

public enum BuildingBlockObjectNames
{
    None,

    Floor_Square,
    Floor_Triangle,

    Wall,
    Wall_Triangle,
    Wall_Diagonal,
    Wall_Window,
    Wall_Door,

    Fence,
    Fence_Diagonal,

    Ramp_Stair,
    Ramp_Ramp,
    Ramp_Triangle,
    Ramp_Corner
}
public enum FurnitureObjectNames
{
    [Description("None")][InspectorName("None")] None,

    [Description("Crafting Table")][InspectorName("Crafting Table")] CraftingTable,
    [Description("Research Table")][InspectorName("Research Table")] ResearchTable,
    [Description("Skill Table")][InspectorName("Skill Table")] SkillTreeTable,

    [Description("Chest Small")][InspectorName("Chest Small")] Chest_Small,
    [Description("Chest Medium")][InspectorName("Chest Medium")] Chest_Medium,
    [Description("Chest Big")][InspectorName("Chest Big")] Chest_Big,

    [Description("Lamp Area")][InspectorName("Lamp Area")] Lamp_Area,
    [Description("Lamp Spot")][InspectorName("Lamp Spot")] Lamp_Spot,
    [Description("Lamp Arídia Area")][InspectorName("Lamp Arídia Area")] Lamp_Arídia_Area,
    [Description("Lamp Arídia Spot")][InspectorName("Lamp Arídia Spot")] Lamp_Arídia_Spot,

    [Description("Other1")][InspectorName("Other1")] FU_Other1,
    [Description("Other2")][InspectorName("Other2")] FU_Other2,
    [Description("Other3")][InspectorName("Other3")] FU_Other3,
    [Description("Other4")][InspectorName("Other4")] FU_Other4,
    [Description("Other5")][InspectorName("Other5")] FU_Other5
}
public enum MachineObjectNames
{
    [Description("None")][InspectorName("None")] None,

    [Description("Ghost Tank")][InspectorName("Ghost Tank")] GhostTank,
    [Description("Energy Storage Tank")][InspectorName("Energy Storage Tank")] EnergyStorageTank,
    [Description("Ghost Repeller")][InspectorName("Ghost Repeller")] GhostRepeller,

    [Description("Extractor")][InspectorName("Extractor")] Extractor,
    [Description("Heat Regulator")][InspectorName("Heat Regulator")] HeatRegulator,
    [Description("Resource Converter")][InspectorName("Resource Converter")] ResourceConverter,
    [Description("Blender")][InspectorName("Blender")] Blender,

    [Description("Crop Plot Small")][InspectorName("Crop Plot Small")] CropPlot_Small,
    [Description("Crop Plot Medium")][InspectorName("Crop Plot Medium")] CropPlot_Medium,
    [Description("Crop Plot Big")][InspectorName("Crop Plot Big")] CropPlot_Big,

    [Description("Grill Small")][InspectorName("Grill Small")] Grill_Small,
    [Description("Grill Medium")][InspectorName("Grill Medium")] Grill_Medium,
    [Description("Grill Big")][InspectorName("Grill Big")] Grill_Big,

    [Description("Battery Small")][InspectorName("Battery Small")] Battery_Small,
    [Description("Battery Medium")][InspectorName("Battery Medium")] Battery_Medium,
    [Description("Battery Big")][InspectorName("Battery Big")] Battery_Big,

    [Description("Other1")][InspectorName("Other1")] MA_Other1,
    [Description("Other2")][InspectorName("Other2")] MA_Other2,
    [Description("Other3")][InspectorName("Other3")] MA_Other3,
    [Description("Other4")][InspectorName("Other4")] MA_Other4,
    [Description("Other5")][InspectorName("Other5")] MA_Other5
}