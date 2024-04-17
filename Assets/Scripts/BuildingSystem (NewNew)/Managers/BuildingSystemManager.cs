using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingSystemManager : Singleton<BuildingSystemManager>
{
    [Header("_SO")]
    public BuildingBlocks_SO buildingBlocks_SO;
    public Furniture_SO furniture_SO;
    public Machines_SO machines_SO;

    [Header("Active BuildingObject")] //From the BuildingObject Menu - Tablet
    public ActiveBuildingObject activeBuildingObject_Info; //Active BuildingObject right now
    public List<WorldBuildingObject> worldBuildingObjectInfoList = new List<WorldBuildingObject>(); //Info about all Objects in the world

    [Header("World Objects")]
    public GameObject worldObject_Parent;
    public List<GameObject> worldBuildingObjectListSpawned = new List<GameObject>(); //All Physical Objects in the world

    public GameObject WorldObjectGhost_Parent;

    [Header("Have enough items to Build?")]
    public bool enoughItemsToBuild;


    //--------------------


    private void Update()
    {
        MoveWorldBuildingObject();
    }


    //--------------------


    public void LoadData()
    {
        //Set activeBuildingObject_Info
        activeBuildingObject_Info = DataManager.Instance.activeBuildingObject_Store;
        BuildingDisplayManager.Instance.UpdateScreenBuildingDisplayInfo();

        //Set worldBuildingObjectInfoList
        worldBuildingObjectInfoList = DataManager.Instance.worldBuildingObjectInfoList_Store;

        for (int i = 0; i < worldBuildingObjectInfoList.Count; i++)
        {
            if (worldBuildingObjectInfoList[i].buildingObjectType_Active == BuildingObjectTypes.BuildingBlock)
            {
                worldBuildingObjectListSpawned.Add(Instantiate(GetBuildingObjectInfo(activeBuildingObject_Info.buildingBlockObjectName_Active, activeBuildingObject_Info.buildingMaterial_Active).objectInfo.worldObject) as GameObject);
    }
            else if (worldBuildingObjectInfoList[i].buildingObjectType_Active == BuildingObjectTypes.Furniture)
            {
                worldBuildingObjectListSpawned.Add(Instantiate(GetBuildingObjectInfo(activeBuildingObject_Info.furnitureObjectName_Active).objectInfo.worldObject) as GameObject);
            }
            else if (worldBuildingObjectInfoList[i].buildingObjectType_Active == BuildingObjectTypes.Machine)
            {
                worldBuildingObjectListSpawned.Add(Instantiate(GetBuildingObjectInfo(activeBuildingObject_Info.machineObjectName_Active).objectInfo.worldObject) as GameObject);
            }

            worldBuildingObjectListSpawned[worldBuildingObjectListSpawned.Count - 1].transform.parent = worldObject_Parent.transform;
            worldBuildingObjectListSpawned[worldBuildingObjectListSpawned.Count - 1].transform.SetPositionAndRotation(worldBuildingObjectInfoList[i].objectPos, worldBuildingObjectInfoList[i].objectRot);
        }

        SpawnNewBuildingObject();

        SaveData();
    }
    public void SaveData()
    {
        DataManager.Instance.activeBuildingObject_Store = activeBuildingObject_Info;
        DataManager.Instance.worldBuildingObjectInfoList_Store = worldBuildingObjectInfoList;
    }


    //--------------------


    public void SpawnNewBuildingObject()
    {
        //Remove previous child
        RemoveBuildingObjectChild();

        //Get a new Child of the selected BuildingObject
        GameObject newObject = new GameObject();
        if (activeBuildingObject_Info.buildingObjectType_Active == BuildingObjectTypes.BuildingBlock)
        {
            newObject = Instantiate(GetBuildingObjectInfo(activeBuildingObject_Info.buildingBlockObjectName_Active, activeBuildingObject_Info.buildingMaterial_Active).objectInfo.worldObject);
        }
        else if (activeBuildingObject_Info.buildingObjectType_Active == BuildingObjectTypes.Furniture)
        {
            newObject = Instantiate(GetBuildingObjectInfo(activeBuildingObject_Info.furnitureObjectName_Active).objectInfo.worldObject);
        }
        else if (activeBuildingObject_Info.buildingObjectType_Active == BuildingObjectTypes.Machine)
        {
            newObject = Instantiate(GetBuildingObjectInfo(activeBuildingObject_Info.machineObjectName_Active).objectInfo.worldObject);
        }
        else
        {
            return;
        }

        //Set new Parent and Pos/Rot
        if (newObject)
        {
            newObject.transform.parent = WorldObjectGhost_Parent.transform;

            newObject.transform.localPosition = Vector3.zero;
            newObject.transform.localRotation = Quaternion.identity;
        }
    }
    public void RemoveBuildingObjectChild()
    {
        for (int i = WorldObjectGhost_Parent.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(WorldObjectGhost_Parent.transform.GetChild(i));
        }
    }
    public void MoveWorldBuildingObject()
    {
        if (WorldObjectGhost_Parent.transform.childCount > 0
            && MainManager.Instance.menuStates == MenuStates.None
            && (HotbarManager.Instance.selectedItem == Items.WoodBuildingHammer || HotbarManager.Instance.selectedItem == Items.StoneBuildingHammer || HotbarManager.Instance.selectedItem == Items.CryoniteBuildingHammer))
        {
            print("Move BuildingObject");

            WorldObjectGhost_Parent.SetActive(true);
        }
        else
        {
            print("Don't Move BuildingObject");

            WorldObjectGhost_Parent.SetActive(false);
        }
    }


    //--------------------


    public void PlaceWorldBuildingObject()
    {
        if (WorldObjectGhost_Parent.activeInHierarchy && WorldObjectGhost_Parent.transform.childCount > 0
            && MainManager.Instance.menuStates == MenuStates.None
            && (HotbarManager.Instance.selectedItem == Items.WoodBuildingHammer || HotbarManager.Instance.selectedItem == Items.StoneBuildingHammer || HotbarManager.Instance.selectedItem == Items.CryoniteBuildingHammer))
        {
            //Spawn correct item
            if (activeBuildingObject_Info.buildingObjectType_Active == BuildingObjectTypes.BuildingBlock)
            {
                worldBuildingObjectListSpawned.Add(Instantiate(GetBuildingObjectInfo(activeBuildingObject_Info.buildingBlockObjectName_Active, activeBuildingObject_Info.buildingMaterial_Active).objectInfo.worldObject) as GameObject);
            }
            else if (activeBuildingObject_Info.buildingObjectType_Active == BuildingObjectTypes.Furniture)
            {
                worldBuildingObjectListSpawned.Add(Instantiate(GetBuildingObjectInfo(activeBuildingObject_Info.furnitureObjectName_Active).objectInfo.worldObject) as GameObject);
            }
            else if (activeBuildingObject_Info.buildingObjectType_Active == BuildingObjectTypes.Machine)
            {
                worldBuildingObjectListSpawned.Add(Instantiate(GetBuildingObjectInfo(activeBuildingObject_Info.machineObjectName_Active).objectInfo.worldObject) as GameObject);
            }

            worldBuildingObjectListSpawned[worldBuildingObjectListSpawned.Count - 1].transform.parent = worldObject_Parent.transform;

            //Set position and Rotation to be the same as the Ghost
            if (WorldObjectGhost_Parent.transform.childCount > 0)
            {
                worldBuildingObjectListSpawned[worldBuildingObjectListSpawned.Count - 1].transform.SetPositionAndRotation(WorldObjectGhost_Parent.transform.GetChild(0).transform.position, WorldObjectGhost_Parent.transform.GetChild(0).transform.rotation);
            }
        }
    }
    public void RemoveWorldBuildingObject()
    {

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

}

[Serializable]
public class ActiveBuildingObject
{
    public BuildingObjectTypes buildingObjectType_Active;
    public BuildingMaterial buildingMaterial_Active;
    [Space(5)]
    public BuildingBlockObjectNames buildingBlockObjectName_Active;
    public FurnitureObjectNames furnitureObjectName_Active;
    public MachineObjectNames machineObjectName_Active;
}

[Serializable]
public class WorldBuildingObject
{
    public Vector3 objectPos;
    public Quaternion objectRot;

    public BuildingObjectTypes buildingObjectType_Active;
    public BuildingMaterial buildingMaterial_Active;
    [Space(5)]
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