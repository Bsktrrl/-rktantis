using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PlantManager : Singleton<PlantManager>
{
    public float growthTimer = 300;

    GameObject plantWorldObject_Parent;
    [SerializeField] List<PlantToSave> plantTypeObjectList = new List<PlantToSave>();

    //--------------------


    private void Awake()
    {
        plantWorldObject_Parent = GameObject.Find("Plant_Parent");
    }


    //--------------------


    public void LoadData()
    {
        plantTypeObjectList = DataManager.Instance.plantTypeObjectList_Store;

        SetupPlantList();
    }
    public void SaveData()
    {
        DataManager.Instance.plantTypeObjectList_Store = plantTypeObjectList;
    }
    public void SaveData(ref GameData gameData)
    {
        //SaveData();

        print("Save_PlantList");
    }


    //--------------------


    void SetupPlantList()
    {
        List<PlantToSave> tempPlantTypeObjectList = new List<PlantToSave>();
        List<bool> checkedPlants = new List<bool>();

        //Add elements to "checkedPlants" as children
        for (int i = 0; i < plantWorldObject_Parent.transform.childCount; i++)
        {
            checkedPlants.Add(false);
        }

        //Set Old Plants
        for (int i = 0; i < plantWorldObject_Parent.transform.childCount; i++)
        {
            if (plantWorldObject_Parent.transform.GetChild(i).GetComponent<Plant>())
            {
                for (int j = 0; j < plantTypeObjectList.Count; j++)
                {
                    if (plantTypeObjectList[j].plantPos == plantWorldObject_Parent.transform.GetChild(i).transform.position)
                    {
                        print("Old Plants: " + i);

                        //Setup Save_List
                        PlantToSave tempPlant = new PlantToSave();

                        tempPlant.isPicked = plantTypeObjectList[j].isPicked;
                        tempPlant.growthTimer = plantTypeObjectList[j].growthTimer;
                        tempPlant.plantIndex = j;
                        tempPlant.precentageCheck = plantTypeObjectList[j].precentageCheck;
                        tempPlant.plantPos = plantTypeObjectList[j].plantPos;

                        tempPlantTypeObjectList.Add(tempPlant);

                        checkedPlants[i] = true;

                        //Set info in Child
                        plantWorldObject_Parent.transform.GetChild(i).GetComponent<Plant>().LoadPlant(plantTypeObjectList[j].isPicked, plantTypeObjectList[j].growthTimer, j, plantTypeObjectList[j].precentageCheck);
                        
                        break;
                    }
                }
            }
        }

        //Set New Plants
        for (int i = 0; i < checkedPlants.Count; i++)
        {
            if (!checkedPlants[i])
            {
                print("New Plants: " + i);

                //Give all Legal Objects an index
                plantWorldObject_Parent.transform.GetChild(i).GetComponent<Plant>().plantIndex = i;

                //Make a plantTypeObjectList
                PlantToSave tempPlant = new PlantToSave();

                tempPlant.isPicked = plantWorldObject_Parent.transform.GetChild(i).GetComponent<Plant>().isPicked;
                tempPlant.growthTimer = plantWorldObject_Parent.transform.GetChild(i).GetComponent<Plant>().growthTimer;
                tempPlant.plantIndex = plantWorldObject_Parent.transform.GetChild(i).GetComponent<Plant>().plantIndex;
                tempPlant.precentageCheck = plantWorldObject_Parent.transform.GetChild(i).GetComponent<Plant>().precentageCheck;
                tempPlant.plantPos = plantWorldObject_Parent.transform.GetChild(i).GetComponent<Plant>().transform.position;

                tempPlantTypeObjectList.Add(tempPlant);
            }
        }

        //Set New plantTypeObjectList
        plantTypeObjectList.Clear();
        plantTypeObjectList = tempPlantTypeObjectList;

        SaveData();
    }
    
    public void ChangePlantInfo(bool _isPicked, float _growthTimer, int _plantIndex, int _precentageCheck, Vector3 _plantPos)
    {
        PlantToSave tempPlant = new PlantToSave();

        tempPlant.isPicked = _isPicked;
        tempPlant.growthTimer = _growthTimer;
        tempPlant.plantIndex = _plantIndex;
        tempPlant.precentageCheck = _precentageCheck;
        tempPlant.plantPos = _plantPos;

        plantTypeObjectList[_plantIndex] = tempPlant;

        SaveData();
    }
}

[Serializable]
public class PlantToSave
{
    public bool isPicked;
    public float growthTimer;

    public int plantIndex;

    public int precentageCheck;

    public Vector3 plantPos = new Vector3();
}

public enum PlantType
{
    [Description("")][InspectorName("None")] None,

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
