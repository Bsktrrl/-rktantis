using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueprintManager : Singleton<BlueprintManager>
{
    public List<bool> activeBuildingBlockObject_SOList = new List<bool>();
    public List<bool> activeFurnitureObject_SOList = new List<bool>();
    public List<bool> activeMachineObject_SOList = new List<bool>();


    [Header("Folder Structure")]
    GameObject blueprintWorldObject_Parent;
    [SerializeField] List<List<BlueprintToSave>> blueprintTypeObjectList = new List<List<BlueprintToSave>>();


    //--------------------


    private void Awake()
    {
        blueprintWorldObject_Parent = GameObject.Find("Blueprint_Parent");
    }


    //--------------------


    public void LoadData()
    {
        #region Load _SO
        check_SOBuildingObjectLists();
        #endregion

        #region Save/Load Objects
        blueprintTypeObjectList.Clear();

        for (int i = 0; i < DataManager.Instance.blueprintTypeObjectList_Store.Count; i++)
        {
            List<BlueprintToSave> blueprintToSaveList = new List<BlueprintToSave>();

            blueprintTypeObjectList.Add(blueprintToSaveList);

            for (int j = 0; j < DataManager.Instance.blueprintTypeObjectList_Store[i].blueprintToSaveList.Count; j++)
            {
                blueprintTypeObjectList[blueprintTypeObjectList.Count - 1].Add(DataManager.Instance.blueprintTypeObjectList_Store[i].blueprintToSaveList[j]);
            }
        }

        SetupBlueprintList();
        #endregion

        SaveData();
    }
    public void SaveData()
    {
        DataManager.Instance.activeBuildingBlockObject_SOList_Store = activeBuildingBlockObject_SOList;
        DataManager.Instance.activeFurnitureObject_SOList_Store = activeFurnitureObject_SOList;
        DataManager.Instance.activeMachineObject_SOList_Store = activeMachineObject_SOList;

        #region Save/Load Objects
        List<ListOfBlueprintToSave> blueprintToSaveList = new List<ListOfBlueprintToSave>();

        for (int i = 0; i < blueprintTypeObjectList.Count; i++)
        {
            ListOfBlueprintToSave blueprintToSave = new ListOfBlueprintToSave();

            blueprintToSaveList.Add(blueprintToSave);

            for (int j = 0; j < blueprintTypeObjectList[i].Count; j++)
            {
                blueprintToSaveList[blueprintToSaveList.Count - 1].blueprintToSaveList.Add(blueprintTypeObjectList[i][j]);
            }
        }

        DataManager.Instance.blueprintTypeObjectList_Store = blueprintToSaveList;
        #endregion
    }


    //--------------------


    //BluePrint WorldObjects
    #region
    void SetupBlueprintList()
    {
        List<List<BlueprintToSave>> tempBlueprintTypeObjectList = new List<List<BlueprintToSave>>();
        List<List<bool>> checkedBlueprint = new List<List<bool>>();

        //Add elements to "checkedBlueprint" as children
        for (int i = 0; i < blueprintWorldObject_Parent.transform.childCount; i++)
        {
            List<BlueprintToSave> tempBlueprinttoSave = new List<BlueprintToSave>();
            tempBlueprintTypeObjectList.Add(tempBlueprinttoSave);

            List<bool> templist_Outer = new List<bool>();
            checkedBlueprint.Add(templist_Outer);

            for (int j = 0; j < blueprintWorldObject_Parent.transform.GetChild(i).transform.childCount; j++)
            {
                bool templist_Inner = false;
                checkedBlueprint[i].Add(templist_Inner);
            }
        }

        //Set Old Blueprints
        for (int i = 0; i < checkedBlueprint.Count; i++)
        {
            for (int k = 0; k < checkedBlueprint[i].Count; k++)
            {
                if (blueprintWorldObject_Parent.transform.GetChild(i).transform.GetChild(k).GetComponent<Blueprint>())
                {
                    for (int j = 0; j < blueprintTypeObjectList.Count; j++)
                    {
                        for (int l = 0; l < blueprintTypeObjectList[j].Count; l++)
                        {
                            if (blueprintTypeObjectList[j][l].blueprintPos == blueprintWorldObject_Parent.transform.GetChild(i).transform.GetChild(k).GetComponent<Blueprint>().transform.position)
                            {
                                //Setup Save_List
                                BlueprintToSave tempBlueprint = new BlueprintToSave();

                                tempBlueprint.isPicked = blueprintTypeObjectList[j][l].isPicked;
                                tempBlueprint.blueprintPos = blueprintTypeObjectList[j][l].blueprintPos;

                                tempBlueprintTypeObjectList[i].Add(tempBlueprint);

                                checkedBlueprint[i][k] = true;

                                //Set info in Child
                                blueprintWorldObject_Parent.transform.GetChild(i).transform.GetChild(k).GetComponent<Blueprint>().LoadBlueprint(blueprintTypeObjectList[j][l].isPicked, j, l);

                                l = blueprintTypeObjectList[j].Count;
                                j = blueprintTypeObjectList.Count;

                                break;
                            }
                        }
                    }
                }
            }
        }

        //Set New Blueprint
        for (int i = 0; i < checkedBlueprint.Count; i++)
        {
            for (int j = 0; j < checkedBlueprint[i].Count; j++)
            {
                if (!checkedBlueprint[i][j])
                {
                    if (blueprintWorldObject_Parent.transform.GetChild(i).transform.GetChild(j))
                    {
                        print("New Blueprint: [" + i + "][" + j + "]");
                        //Give all Legal Objects an index
                        blueprintWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<Blueprint>().blueprintIndex_x = i;
                        blueprintWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<Blueprint>().blueprintIndex_y = j;

                        //Make a TreeTypeObjectList
                        BlueprintToSave tempBlueprint = new BlueprintToSave();

                        tempBlueprint.isPicked = blueprintWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<Blueprint>().isPicked;
                        tempBlueprint.blueprintIndex_x = blueprintWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<Blueprint>().blueprintIndex_x;
                        tempBlueprint.blueprintIndex_y = blueprintWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<Blueprint>().blueprintIndex_y;
                        tempBlueprint.blueprintPos = blueprintWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<Blueprint>().transform.position;

                        tempBlueprintTypeObjectList[i].Add(tempBlueprint);
                    }
                }
            }
        }

        //Set New BlueprintTypeObjectList
        blueprintTypeObjectList.Clear();
        blueprintTypeObjectList = tempBlueprintTypeObjectList;

        SaveData();
    }
    public void ChangeBlueprintInfo(bool _isPicked, int _blueprintIndex_j, int _blueprintIndex_l, Vector3 _blueprintPos)
    {
        BlueprintToSave blueprintTree = new BlueprintToSave();

        blueprintTree.isPicked = _isPicked;
        blueprintTree.blueprintIndex_x = _blueprintIndex_j;
        blueprintTree.blueprintIndex_y = _blueprintIndex_l;
        blueprintTree.blueprintPos = _blueprintPos;

        blueprintTypeObjectList[_blueprintIndex_j][_blueprintIndex_l] = blueprintTree;

        SaveData();
    }
    #endregion


    //--------------------


    public void check_SOBuildingObjectLists()
    {//Load _SOBuildingLists
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
    }


    //--------------------


    //SetStart
    #region
    public void SetStart_BuildingBock_BlueprintSet()
    {
        //Setup _SO isActive Lists
        for (int i = 0; i < BuildingSystemManager.Instance.buildingBlocks_SO.buildingBlockObjectsList.Count; i++)
        {
            activeBuildingBlockObject_SOList.Add(false);

            //if (BuildingSystemManager.Instance.buildingBlocks_SO.buildingBlockObjectsList[i].objectInfo.isActive)
            //{
            //    activeBuildingBlockObject_SOList.Add(true);
            //}
            //else
            //{
            //    activeBuildingBlockObject_SOList.Add(false);
            //}
        }

        //Set what to display at the start
        activeBuildingBlockObject_SOList[0] = true;
        activeBuildingBlockObject_SOList[1] = true;
        activeBuildingBlockObject_SOList[2] = true;
        activeBuildingBlockObject_SOList[3] = true;
        activeBuildingBlockObject_SOList[4] = true;
        activeBuildingBlockObject_SOList[5] = true;
        activeBuildingBlockObject_SOList[6] = true;
        activeBuildingBlockObject_SOList[7] = true;
        activeBuildingBlockObject_SOList[8] = true;
        activeBuildingBlockObject_SOList[9] = true;
        activeBuildingBlockObject_SOList[10] = true;
    }
    public void SetStart_Furniture_BlueprintSet()
    {
        //Setup _SO isActive Lists
        for (int i = 0; i < BuildingSystemManager.Instance.furniture_SO.furnitureObjectsList.Count; i++)
        {
            activeFurnitureObject_SOList.Add(false);

            //if (BuildingSystemManager.Instance.furniture_SO.furnitureObjectsList[i].objectInfo.isActive)
            //{
            //    activeFurnitureObject_SOList.Add(true);
            //}
            //else
            //{
            //    activeFurnitureObject_SOList.Add(false);
            //}
        }
    }
    public void SetStart_Machine_BlueprintSet()
    {
        //Setup _SO isActive Lists
        for (int i = 0; i < BuildingSystemManager.Instance.machines_SO.machineObjectsList.Count; i++)
        {
            activeMachineObject_SOList.Add(false);

            //if (BuildingSystemManager.Instance.machines_SO.machineObjectsList[i].objectInfo.isActive)
            //{
            //    activeMachineObject_SOList.Add(true);
            //}
            //else
            //{
            //    activeMachineObject_SOList.Add(false);
            //}
        }
    }
    #endregion


    //--------------------


    //AddBlueprint
    #region
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
    #endregion
}

[Serializable]
public class BlueprintToSave
{
    public bool isPicked;

    public int blueprintIndex_x;
    public int blueprintIndex_y;

    public Vector3 blueprintPos = new Vector3();
}

[Serializable]
public class ListOfBlueprintToSave
{
    public List<BlueprintToSave> blueprintToSaveList = new List<BlueprintToSave>();
}