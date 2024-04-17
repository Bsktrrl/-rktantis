using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueprintManager : Singleton<BlueprintManager>
{
    public List<bool> activeBuildingBlockObject_SOList = new List<bool>();
    public List<bool> activeFurnitureObject_SOList = new List<bool>();
    public List<bool> activeMachineObject_SOList = new List<bool>();


    //--------------------


    public void LoadData()
    {
        //Load _SOBuildingLists
        if (DataManager.Instance.activeBuildingBlockObject_SOList_Store.Count > 0)
        {
            activeBuildingBlockObject_SOList = DataManager.Instance.activeBuildingBlockObject_SOList_Store;

            for (int i = 0; i < BuildingSystemManager.Instance.buildingBlocks_SO.buildingBlockObjectsList.Count; i++)
            {
                BuildingSystemManager.Instance.buildingBlocks_SO.buildingBlockObjectsList[i].objectInfo.isActive = activeBuildingBlockObject_SOList[i];
            }
        }
        else
        {
            SetStart_BuildingBock_BlueprintSet();
        }

        //Load _SOFurnitureLists
        if (DataManager.Instance.activeFurnitureObject_SOList_Store.Count > 0)
        {
            activeFurnitureObject_SOList = DataManager.Instance.activeFurnitureObject_SOList_Store;

            for (int i = 0; i < BuildingSystemManager.Instance.furniture_SO.furnitureObjectsList.Count; i++)
            {
                BuildingSystemManager.Instance.furniture_SO.furnitureObjectsList[i].objectInfo.isActive = activeFurnitureObject_SOList[i];
            }
        }
        else
        {
            SetStart_Furniture_BlueprintSet();
        }

        //Load _SOMachineLists
        if (DataManager.Instance.activeMachineObject_SOList_Store.Count > 0)
        {
            activeMachineObject_SOList = DataManager.Instance.activeMachineObject_SOList_Store;

            for (int i = 0; i < BuildingSystemManager.Instance.machines_SO.machineObjectsList.Count; i++)
            {
                BuildingSystemManager.Instance.machines_SO.machineObjectsList[i].objectInfo.isActive = activeMachineObject_SOList[i];
            }
        }
        else
        {
            SetStart_Machine_BlueprintSet();
        }

        SaveData();
    }
    public void SaveData()
    {
        DataManager.Instance.activeBuildingBlockObject_SOList_Store = activeBuildingBlockObject_SOList;
        DataManager.Instance.activeFurnitureObject_SOList_Store = activeFurnitureObject_SOList;
        DataManager.Instance.activeMachineObject_SOList_Store = activeMachineObject_SOList;
    }



    //--------------------


    public void SetStart_BuildingBock_BlueprintSet()
    {
        //Setup _SO isActive Lists
        for (int i = 0; i < BuildingSystemManager.Instance.buildingBlocks_SO.buildingBlockObjectsList.Count; i++)
        {
            if (BuildingSystemManager.Instance.buildingBlocks_SO.buildingBlockObjectsList[i].objectInfo.isActive)
            {
                activeBuildingBlockObject_SOList.Add(true);
            }
            else
            {
                activeBuildingBlockObject_SOList.Add(false);
            }
        }
    }
    public void SetStart_Furniture_BlueprintSet()
    {
        //Setup _SO isActive Lists
        for (int i = 0; i < BuildingSystemManager.Instance.furniture_SO.furnitureObjectsList.Count; i++)
        {
            if (BuildingSystemManager.Instance.furniture_SO.furnitureObjectsList[i].objectInfo.isActive)
            {
                activeFurnitureObject_SOList.Add(true);
            }
            else
            {
                activeFurnitureObject_SOList.Add(false);
            }
        }
    }
    public void SetStart_Machine_BlueprintSet()
    {
        //Setup _SO isActive Lists
        for (int i = 0; i < BuildingSystemManager.Instance.machines_SO.machineObjectsList.Count; i++)
        {
            if (BuildingSystemManager.Instance.machines_SO.machineObjectsList[i].objectInfo.isActive)
            {
                activeMachineObject_SOList.Add(true);
            }
            else
            {
                activeMachineObject_SOList.Add(false);
            }
        }
    }


    //--------------------


    public void AddBlueprint(BuildingBlockObjectNames buildingBlockObjectNames, BuildingMaterial buildingMaterial)
    {
        for (int i = 0; i < BuildingSystemManager.Instance.buildingBlocks_SO.buildingBlockObjectsList.Count; i++)
        {
            if (BuildingSystemManager.Instance.buildingBlocks_SO.buildingBlockObjectsList[i].blockName == buildingBlockObjectNames
                && BuildingSystemManager.Instance.buildingBlocks_SO.buildingBlockObjectsList[i].buildingMaterial == buildingMaterial)
            {
                BuildingSystemManager.Instance.buildingBlocks_SO.buildingBlockObjectsList[i].objectInfo.isActive = true;
                activeBuildingBlockObject_SOList[i] = true;

                break;
            }
        }

        SaveData();
    }
    public void AddBlueprint(FurnitureObjectNames furnitureObjectNames)
    {
        for (int i = 0; i < BuildingSystemManager.Instance.furniture_SO.furnitureObjectsList.Count; i++)
        {
            if (BuildingSystemManager.Instance.furniture_SO.furnitureObjectsList[i].furnitureName == furnitureObjectNames)
            {
                BuildingSystemManager.Instance.furniture_SO.furnitureObjectsList[i].objectInfo.isActive = true;
                activeFurnitureObject_SOList[i] = true;

                break;
            }
        }

        SaveData();
    }
    public void AddBlueprint(MachineObjectNames machineObjectNames)
    {
        for (int i = 0; i < BuildingSystemManager.Instance.machines_SO.machineObjectsList.Count; i++)
        {
            if (BuildingSystemManager.Instance.machines_SO.machineObjectsList[i].machinesName == machineObjectNames)
            {
                BuildingSystemManager.Instance.machines_SO.machineObjectsList[i].objectInfo.isActive = true;
                activeMachineObject_SOList[i] = true;

                break;
            }
        }

        SaveData();
    }
}
