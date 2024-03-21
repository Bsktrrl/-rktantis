using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreManager : Singleton<OreManager>
{
    public float dormantTimer = 2;

    GameObject oreWorldObject_Parent;
    [SerializeField] List<List<OreToSave>> oreTypeObjectList = new List<List<OreToSave>>();

    [Header("Pickaxe Stats")]
    public float woodPickaxe_Droprate = 55;
    public float stonePickaxe_Droprate = 65;
    public float cryonitePickaxe_Droprate =  75;

    [Header("Hand Damage Stats")]
    public float handDamage = 0.5f;

    [Header("SkillTree Stats")]
    [HideInInspector] public int oreHealthReducer = 0;
    public int oreDropRateReducer = 15;


    //--------------------


    private void Awake()
    {
        oreWorldObject_Parent = GameObject.Find("Ore_Parent");
    }


    //--------------------


    public void LoadData()
    {
        oreTypeObjectList.Clear();

        for (int i = 0; i < DataManager.Instance.oreTypeObjectList_Store.Count; i++)
        {
            List<OreToSave> oreToSaveList = new List<OreToSave>();

            oreTypeObjectList.Add(oreToSaveList);

            for (int j = 0; j < DataManager.Instance.oreTypeObjectList_Store[i].oreToSaveList.Count; j++)
            {
                oreTypeObjectList[oreTypeObjectList.Count - 1].Add(DataManager.Instance.oreTypeObjectList_Store[i].oreToSaveList[j]);
            }
        }

        SetupOreList();
    }
    public void SaveData()
    {
        List<ListOfOreToSave> oreToSaveList = new List<ListOfOreToSave>();

        for (int i = 0; i < oreTypeObjectList.Count; i++)
        {
            ListOfOreToSave oreToSave = new ListOfOreToSave();

            oreToSaveList.Add(oreToSave);

            for (int j = 0; j < oreTypeObjectList[i].Count; j++)
            {
                oreToSaveList[oreToSaveList.Count - 1].oreToSaveList.Add(oreTypeObjectList[i][j]);
            }
        }

        DataManager.Instance.oreTypeObjectList_Store = oreToSaveList;
    }
    public void SaveData(ref GameData gameData)
    {
        SaveData();

        print("Save_OreList");
    }


    //--------------------


    void SetupOreList()
    {
        List<List<OreToSave>> tempOreTypeObjectList = new List<List<OreToSave>>();
        List<List<bool>> checkedOres = new List<List<bool>>();

        //Add elements to "checkedOres" as children
        for (int i = 0; i < oreWorldObject_Parent.transform.childCount; i++)
        {
            List<OreToSave> temporetoSave = new List<OreToSave>();
            tempOreTypeObjectList.Add(temporetoSave);

            List<bool> templist_Outer = new List<bool>();
            checkedOres.Add(templist_Outer);

            for (int j = 0; j < oreWorldObject_Parent.transform.GetChild(i).transform.childCount; j++)
            {
                bool templist_Inner = false;
                checkedOres[i].Add(templist_Inner);
            }
        }

        //Set Old Ores
        for (int i = 0; i < checkedOres.Count; i++)
        {
            for (int k = 0; k < checkedOres[i].Count; k++)
            {
                if (oreWorldObject_Parent.transform.GetChild(i).transform.GetChild(k).GetComponent<Ore>())
                {
                    for (int j = 0; j < oreTypeObjectList.Count; j++)
                    {
                        for (int l = 0; l < oreTypeObjectList[j].Count; l++)
                        {
                            if (oreTypeObjectList[j][l].orePos == oreWorldObject_Parent.transform.GetChild(i).transform.GetChild(k).GetComponent<Ore>().transform.position)
                            {
                                //Setup Save_List
                                OreToSave tempOre = new OreToSave();

                                tempOre.isHacked = oreTypeObjectList[j][l].isHacked;
                                tempOre.dormantTimer = oreTypeObjectList[j][l].dormantTimer;
                                tempOre.oreIndex_x = j;
                                tempOre.oreIndex_y = l;
                                tempOre.percentageCheck = oreTypeObjectList[j][l].percentageCheck;
                                tempOre.oreHealth = oreTypeObjectList[j][l].oreHealth;
                                tempOre.orePos = oreTypeObjectList[j][l].orePos;

                                tempOreTypeObjectList[i].Add(tempOre);

                                checkedOres[i][k] = true;

                                //Set info in Child
                                oreWorldObject_Parent.transform.GetChild(i).transform.GetChild(k).GetComponent<Ore>().LoadOre(oreTypeObjectList[j][l].isHacked, oreTypeObjectList[j][l].dormantTimer, j, l, oreTypeObjectList[j][l].percentageCheck, oreTypeObjectList[j][l].oreHealth);

                                l = oreTypeObjectList[j].Count;
                                j = oreTypeObjectList.Count;

                                break;
                            }
                        }
                    }
                }
            }
        }

        //Set New Ores
        for (int i = 0; i < checkedOres.Count; i++)
        {
            for (int j = 0; j < checkedOres[i].Count; j++)
            {
                if (!checkedOres[i][j])
                {
                    print("New Ores: [" + i + "][" + j + "]");

                    //Give all Legal Objects an index
                    oreWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<Ore>().oreIndex_x = i;
                    oreWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<Ore>().oreIndex_y = j;

                    //Make a OreTypeObjectList
                    OreToSave tempOre = new OreToSave();

                    tempOre.isHacked = oreWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<Ore>().isHacked;
                    tempOre.dormantTimer = oreWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<Ore>().dormantTimer;
                    tempOre.oreIndex_x = oreWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<Ore>().oreIndex_x;
                    tempOre.oreIndex_y = oreWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<Ore>().oreIndex_y;
                    tempOre.percentageCheck = oreWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<Ore>().percentageCheck;
                    tempOre.oreHealth = oreWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<Ore>().oreHealth;
                    tempOre.orePos = oreWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<Ore>().transform.position;

                    tempOreTypeObjectList[i].Add(tempOre);
                }
            }
        }

        //Set New OreTypeObjectList
        oreTypeObjectList.Clear();
        oreTypeObjectList = tempOreTypeObjectList;

        SaveData();
    }

    public void ChangeOreInfo(bool _isHacked, float _dormantTimer, int _oreIndex_j, int _oreIndex_l, int _percentageCheck, float _oreHealth, Vector3 _orePos)
    {
        OreToSave tempOre = new OreToSave();

        tempOre.isHacked = _isHacked;
        tempOre.dormantTimer = _dormantTimer;
        tempOre.oreIndex_x = _oreIndex_j;
        tempOre.oreIndex_y = _oreIndex_l;
        tempOre.percentageCheck = _percentageCheck;
        tempOre.oreHealth = _oreHealth;
        tempOre.orePos = _orePos;

        oreTypeObjectList[_oreIndex_j][_oreIndex_l] = tempOre;

        SaveData();
    }
}

[Serializable]
public class OreToSave
{
    public bool isHacked;
    public float dormantTimer;

    public int oreIndex_x;
    public int oreIndex_y;
    public float oreHealth;

    public int percentageCheck;

    public Vector3 orePos = new Vector3();
}

[Serializable]
public class ListOfOreToSave
{
    public List<OreToSave> oreToSaveList = new List<OreToSave>();
}
