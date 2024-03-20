using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreManager : Singleton<OreManager>
{
    public float dormantTimer = 2;

    GameObject oreWorldObject_Parent;
    [SerializeField] List<OreToSave> oreTypeObjectList = new List<OreToSave>();

    [Header("Pickaxe Stats")]
    public float woodPickaxe_Droprate = 50;
    public float stonePickaxe_Droprate = 65;
    public float cryonitePickaxe_Droprate = 80;

    [Header("Hand Damage Stats")]
    public float handDamage = 0.5f;

    [Header("SkillTree Stats")]
    [HideInInspector] public int oreHealthReducer = 0;
    public int oreDropRateReducer = 10;


    //--------------------


    private void Awake()
    {
        oreWorldObject_Parent = GameObject.Find("Ore_Parent");
    }


    //--------------------


    public void LoadData()
    {
        oreTypeObjectList = DataManager.Instance.oreTypeObjectList_Store;

        SetupOreList();
    }
    public void SaveData()
    {
        DataManager.Instance.oreTypeObjectList_Store = oreTypeObjectList;
    }
    public void SaveData(ref GameData gameData)
    {
        SaveData();

        print("Save_OreList");
    }


    //--------------------


    void SetupOreList()
    {
        List<OreToSave> tempOreTypeObjectList = new List<OreToSave>();
        List<bool> checkedOres = new List<bool>();

        //Add elements to "checkedOres" as children
        for (int i = 0; i < oreWorldObject_Parent.transform.childCount; i++)
        {
            checkedOres.Add(false);
        }

        //Set Old Ores
        for (int i = 0; i < oreWorldObject_Parent.transform.childCount; i++)
        {
            if (oreWorldObject_Parent.transform.GetChild(i).GetComponent<Ore>())
            {
                for (int j = 0; j < oreTypeObjectList.Count; j++)
                {
                    if (oreTypeObjectList[j].orePos == oreWorldObject_Parent.transform.GetChild(i).transform.position)
                    {
                        //Setup Save_List
                        OreToSave tempOre = new OreToSave();

                        tempOre.isHacked = oreTypeObjectList[j].isHacked;
                        tempOre.dormantTimer = oreTypeObjectList[j].dormantTimer;
                        tempOre.oreIndex = j;
                        tempOre.percentageCheck = oreTypeObjectList[j].percentageCheck;
                        tempOre.oreHealth = oreTypeObjectList[j].oreHealth;
                        tempOre.orePos = oreTypeObjectList[j].orePos;

                        tempOreTypeObjectList.Add(tempOre);

                        checkedOres[i] = true;

                        //Set info in Child
                        oreWorldObject_Parent.transform.GetChild(i).GetComponent<Ore>().LoadOre(oreTypeObjectList[j].isHacked, oreTypeObjectList[j].dormantTimer, j, oreTypeObjectList[j].percentageCheck, oreTypeObjectList[j].oreHealth);

                        break;
                    }
                }
            }
        }

        //Set New Ores
        for (int i = 0; i < checkedOres.Count; i++)
        {
            if (!checkedOres[i])
            {
                print("New Ores: " + i);

                //Give all Legal Objects an index
                oreWorldObject_Parent.transform.GetChild(i).GetComponent<Ore>().oreIndex = i;

                //Make a OreTypeObjectList
                OreToSave tempOre = new OreToSave();

                tempOre.isHacked = oreWorldObject_Parent.transform.GetChild(i).GetComponent<Ore>().isHacked;
                tempOre.dormantTimer = oreWorldObject_Parent.transform.GetChild(i).GetComponent<Ore>().dormantTimer;
                tempOre.oreIndex = oreWorldObject_Parent.transform.GetChild(i).GetComponent<Ore>().oreIndex;
                tempOre.percentageCheck = oreWorldObject_Parent.transform.GetChild(i).GetComponent<Ore>().percentageCheck;
                tempOre.oreHealth = oreWorldObject_Parent.transform.GetChild(i).GetComponent<Ore>().oreHealth;
                tempOre.orePos = oreWorldObject_Parent.transform.GetChild(i).GetComponent<Ore>().transform.position;

                tempOreTypeObjectList.Add(tempOre);
            }
        }

        //Set New OreTypeObjectList
        oreTypeObjectList.Clear();
        oreTypeObjectList = tempOreTypeObjectList;

        SaveData();
    }

    public void ChangeOreInfo(bool _isHacked, float _dormantTimer, int _oreIndex, int _percentageCheck, float _oreHelath, Vector3 _orePos)
    {
        OreToSave tempOre = new OreToSave();

        tempOre.isHacked = _isHacked;
        tempOre.dormantTimer = _dormantTimer;
        tempOre.oreIndex = _oreIndex;
        tempOre.percentageCheck = _percentageCheck;
        tempOre.oreHealth = _oreHelath;
        tempOre.orePos = _orePos;

        oreTypeObjectList[_oreIndex] = tempOre;

        SaveData();
    }
}

[Serializable]
public class OreToSave
{
    public bool isHacked;
    public float dormantTimer;

    public int oreIndex;
    public float oreHealth;

    public int percentageCheck;

    public Vector3 orePos = new Vector3();
}
