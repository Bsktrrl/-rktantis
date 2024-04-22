using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PlantManager : Singleton<PlantManager>
{
    public float growthGlobalTimerMultiplier = 1f;

    GameObject plantWorldObject_Parent;
    [SerializeField] List<List<PlantToSave>> plantTypeObjectList = new List<List<PlantToSave>>();
    

    //--------------------


    private void Awake()
    {
        plantWorldObject_Parent = GameObject.Find("Plant_Parent");
    }


    //--------------------


    public void LoadData()
    {
        plantTypeObjectList.Clear();

        for (int i = 0; i < DataManager.Instance.plantTypeObjectList_Store.Count; i++)
        {
            List<PlantToSave> plantToSaveList = new List<PlantToSave>();

            plantTypeObjectList.Add(plantToSaveList);

            for (int j = 0; j < DataManager.Instance.plantTypeObjectList_Store[i].plantToSaveList.Count; j++)
            {
                plantTypeObjectList[plantTypeObjectList.Count - 1].Add(DataManager.Instance.plantTypeObjectList_Store[i].plantToSaveList[j]);
            }
        }

        SetupPlantList();
    }
    public void SaveData()
    {
        List<ListOfPlantToSave> plantToSaveList = new List<ListOfPlantToSave>();

        for (int i = 0; i < plantTypeObjectList.Count; i++)
        {
            ListOfPlantToSave plantToSave = new ListOfPlantToSave();

            plantToSaveList.Add(plantToSave);

            for (int j = 0; j < plantTypeObjectList[i].Count; j++)
            {
                plantToSaveList[plantToSaveList.Count - 1].plantToSaveList.Add(plantTypeObjectList[i][j]);
            }
        }

        DataManager.Instance.plantTypeObjectList_Store = plantToSaveList;
    }
    public void SaveData(ref GameData gameData)
    {
        //SaveData();

        print("Save_PlantList");
    }


    //--------------------


    void SetupPlantList()
    {
        List<List<PlantToSave>> tempPlantTypeObjectList = new List<List<PlantToSave>>();
        List<List<bool>> checkedPlants = new List<List<bool>>();

        //Add elements to "checkedPlants" as children
        for (int i = 0; i < plantWorldObject_Parent.transform.childCount; i++)
        {
            List<PlantToSave> tempPlanttoSave = new List<PlantToSave>();
            tempPlantTypeObjectList.Add(tempPlanttoSave);

            List<bool> templist_Outer = new List<bool>();
            checkedPlants.Add(templist_Outer);

            for (int j = 0; j < plantWorldObject_Parent.transform.GetChild(i).transform.childCount; j++)
            {
                bool templist_Inner = false;
                checkedPlants[i].Add(templist_Inner);
            }
        }

        //Set Old Plants
        for (int i = 0; i < checkedPlants.Count; i++)
        {
            for (int k = 0; k < checkedPlants[i].Count; k++)
            {
                if (plantWorldObject_Parent.transform.GetChild(i).transform.GetChild(k).GetComponent<Plant>())
                {
                    for (int j = 0; j < plantTypeObjectList.Count; j++)
                    {
                        for (int l = 0; l < plantTypeObjectList[j].Count; l++)
                        {
                            if (plantTypeObjectList[j][l].plantPos == plantWorldObject_Parent.transform.GetChild(i).transform.GetChild(k).GetComponent<Plant>().transform.position)
                            {
                                //Setup Save_List
                                PlantToSave tempPlant = new PlantToSave();

                                tempPlant.isPicked = plantTypeObjectList[j][l].isPicked;
                                tempPlant.growthTimer = plantTypeObjectList[j][l].growthTimer;
                                tempPlant.plantIndex_x = j;
                                tempPlant.plantIndex_y = l;
                                tempPlant.percentageCheck = plantTypeObjectList[j][l].percentageCheck;
                                tempPlant.plantPos = plantTypeObjectList[j][l].plantPos;

                                tempPlantTypeObjectList[i].Add(tempPlant);

                                checkedPlants[i][k] = true;

                                //Set info in Child
                                plantWorldObject_Parent.transform.GetChild(i).transform.GetChild(k).GetComponent<Plant>().LoadPlant(plantTypeObjectList[j][l].isPicked, plantTypeObjectList[j][l].growthTimer, j, l, plantTypeObjectList[j][l].percentageCheck);

                                l = plantTypeObjectList[j].Count;
                                j = plantTypeObjectList.Count;

                                break;
                            }
                        }
                    }
                }
            }
        }

        //Set New Plants
        for (int i = 0; i < checkedPlants.Count; i++)
        {
            for (int j = 0; j < checkedPlants[i].Count; j++)
            {
                if (!checkedPlants[i][j])
                {
                    //print("New Plants: [" + i + "][" + j + "]");

                    //Give all Legal Objects an index
                    plantWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<Plant>().plantIndex_x = i;
                    plantWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<Plant>().plantIndex_y = j;

                    //Make a PlantTypeObjectList
                    PlantToSave tempPlant = new PlantToSave();

                    tempPlant.isPicked = plantWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<Plant>().isPicked;
                    tempPlant.growthTimer = plantWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<Plant>().growthTimer;
                    tempPlant.plantIndex_x = plantWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<Plant>().plantIndex_x;
                    tempPlant.plantIndex_y = plantWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<Plant>().plantIndex_y;
                    tempPlant.percentageCheck = plantWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<Plant>().percentageCheck;
                    tempPlant.plantPos = plantWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<Plant>().transform.position;

                    tempPlantTypeObjectList[i].Add(tempPlant);
                }
            }
        }

        //Set New OreTypeObjectList
        plantTypeObjectList.Clear();
        plantTypeObjectList = tempPlantTypeObjectList;

        SaveData();
    }
    
    public void ChangePlantInfo(bool _isPicked, float _growthTimer, int _plantIndex_j, int _plantIndex_l, int _precentageCheck, Vector3 _plantPos)
    {
        PlantToSave tempPlant = new PlantToSave();

        tempPlant.isPicked = _isPicked;
        tempPlant.growthTimer = _growthTimer;
        tempPlant.plantIndex_x = _plantIndex_j;
        tempPlant.plantIndex_y = _plantIndex_l;
        tempPlant.percentageCheck = _precentageCheck;
        tempPlant.plantPos = _plantPos;

        plantTypeObjectList[_plantIndex_j][_plantIndex_l] = tempPlant;

        SaveData();
    }
}

[Serializable]
public class PlantToSave
{
    public bool isPicked;
    public float growthTimer;

    public int plantIndex_x;
    public int plantIndex_y;

    public int percentageCheck;

    public Vector3 plantPos = new Vector3();
}

[Serializable]
public class ListOfPlantToSave
{
    public List<PlantToSave> plantToSaveList = new List<PlantToSave>();
}

public enum PlantType
{
    [Description("")][InspectorName("Standing")] None,

    [Description("Adrídis Flower")][InspectorName("Adrídis Flower")] AdrídisFlower,
    [Description("Cactus 1")][InspectorName("Cactus 1")] Cactus1,
    [Description("Cactus 2")][InspectorName("Cactus 2")] Cactus2,
    [Description("Cactus 3")][InspectorName("Cactus 3")] Cactus3,
    [Description("Freeze Fruit")][InspectorName("Freeze Fruit")] FreezeFruit,
    [Description("Crimson Cloud Bush")][InspectorName("Crimson Cloud Bush")] CrimsonCloudBush,
    [Description("Fire Lily")][InspectorName("Fire Lily")] FireLily,
    [Description("Forst Lily")][InspectorName("Forst Lily")] ForstLily,
    [Description("Glue Plant")][InspectorName("Glue Plant")] GluePlant,
    [Description("Grass")][InspectorName("Grass")] Grass,
    [Description("Heat Fruit")][InspectorName("Heat Fruit")] HeatFruit,
    [Description("Petal Plant")][InspectorName("Petal Plant")] PetalPlant,
    [Description("Pudding Cactus")][InspectorName("Pudding Cactus")] PuddingCactus,
    [Description("Red Cotton Plant")][InspectorName("Red Cotton Plant")] RedCottonPlant,
    [Description("Sand Lily")][InspectorName("Sand Lily")] SandLily,
    [Description("Sand Tubes")][InspectorName("Sand Tubes")] SandTubes,
    [Description("Spike Oil Fruit")][InspectorName("Spike Oil Fruit")] SpikeOilFruit,
    [Description("Stalk Fruit")][InspectorName("Stalk Fruit")] StalkFruit,
    [Description("Stem Cactus")][InspectorName("Stem Cactus")] StemCactus,
    [Description("ThriPod")][InspectorName("ThriPod")] ThriPod,
    [Description("TwistCap")][InspectorName("TwistCap")] TwistCap,
    [Description("WartShroom")][InspectorName("WartShroom")] WartShroom
}
