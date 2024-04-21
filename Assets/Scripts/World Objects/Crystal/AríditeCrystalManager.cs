using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AríditeCrystalManager : Singleton<AríditeCrystalManager>
{
    [Header("Folder Structure")]
    GameObject aríditeCrystalWorldObject_Parent;
    [SerializeField] List<List<AríditeCrystalToSave>> aríditeCrystalTypeObjectList = new List<List<AríditeCrystalToSave>>();


    //--------------------


    private void Awake()
    {
        aríditeCrystalWorldObject_Parent = GameObject.Find("Crystal_Parent");
    }


    //--------------------


    public void LoadData()
    {
        #region Save/Load Objects
        aríditeCrystalTypeObjectList.Clear();

        for (int i = 0; i < DataManager.Instance.aríditeCrystalTypeObjectList_Store.Count; i++)
        {
            List<AríditeCrystalToSave> arídianKeyToSaveList = new List<AríditeCrystalToSave>();

            aríditeCrystalTypeObjectList.Add(arídianKeyToSaveList);

            for (int j = 0; j < DataManager.Instance.aríditeCrystalTypeObjectList_Store[i].aríditeCrystalToSaveList.Count; j++)
            {
                aríditeCrystalTypeObjectList[aríditeCrystalTypeObjectList.Count - 1].Add(DataManager.Instance.aríditeCrystalTypeObjectList_Store[i].aríditeCrystalToSaveList[j]);
            }
        }

        SetupAríditeCrystalList();
        #endregion

        SaveData();
    }
    public void SaveData()
    {
        #region Save/Load Objects
        List<ListOfAríditeCrystalToSave> aríditeCrystalToSaveList = new List<ListOfAríditeCrystalToSave>();

        for (int i = 0; i < aríditeCrystalTypeObjectList.Count; i++)
        {
            ListOfAríditeCrystalToSave aríditeCrystalToSave = new ListOfAríditeCrystalToSave();

            aríditeCrystalToSaveList.Add(aríditeCrystalToSave);

            for (int j = 0; j < aríditeCrystalTypeObjectList[i].Count; j++)
            {
                aríditeCrystalToSaveList[aríditeCrystalToSaveList.Count - 1].aríditeCrystalToSaveList.Add(aríditeCrystalTypeObjectList[i][j]);
            }
        }

        DataManager.Instance.aríditeCrystalTypeObjectList_Store = aríditeCrystalToSaveList;
        #endregion
    }
    public void SaveData(ref GameData gameData)
    {
        SaveData();

        print("Save_AríditeCrystal");
    }


    //--------------------


    void SetupAríditeCrystalList()
    {
        List<List<AríditeCrystalToSave>> tempAríditeCrystalTypeObjectList = new List<List<AríditeCrystalToSave>>();
        List<List<bool>> checkedAríditeCrystal = new List<List<bool>>();

        //Add elements to "checkedBlueprint" as children
        for (int i = 0; i < aríditeCrystalWorldObject_Parent.transform.childCount; i++)
        {
            List<AríditeCrystalToSave> tempAríditeCrystaltoSave = new List<AríditeCrystalToSave>();
            tempAríditeCrystalTypeObjectList.Add(tempAríditeCrystaltoSave);

            List<bool> templist_Outer = new List<bool>();
            checkedAríditeCrystal.Add(templist_Outer);

            for (int j = 0; j < aríditeCrystalWorldObject_Parent.transform.GetChild(i).transform.childCount; j++)
            {
                bool templist_Inner = false;
                checkedAríditeCrystal[i].Add(templist_Inner);
            }
        }

        //Set Old AríditeCrystal
        for (int i = 0; i < checkedAríditeCrystal.Count; i++)
        {
            for (int k = 0; k < checkedAríditeCrystal[i].Count; k++)
            {
                if (aríditeCrystalWorldObject_Parent.transform.GetChild(i).transform.GetChild(k).GetComponent<AríditeCrystal>())
                {
                    for (int j = 0; j < aríditeCrystalTypeObjectList.Count; j++)
                    {
                        for (int l = 0; l < aríditeCrystalTypeObjectList[j].Count; l++)
                        {
                            if (aríditeCrystalTypeObjectList[j][l].aríditeCrystalPos == aríditeCrystalWorldObject_Parent.transform.GetChild(i).transform.GetChild(k).GetComponent<AríditeCrystal>().transform.position)
                            {
                                //Setup Save_List
                                AríditeCrystalToSave tempAríditeCrystal = new AríditeCrystalToSave();

                                tempAríditeCrystal.isPicked = aríditeCrystalTypeObjectList[j][l].isPicked;
                                tempAríditeCrystal.aríditeCrystalPos = aríditeCrystalTypeObjectList[j][l].aríditeCrystalPos;

                                tempAríditeCrystalTypeObjectList[i].Add(tempAríditeCrystal);

                                checkedAríditeCrystal[i][k] = true;

                                //Set info in Child
                                aríditeCrystalWorldObject_Parent.transform.GetChild(i).transform.GetChild(k).GetComponent<AríditeCrystal>().LoadAríditeCrystal(aríditeCrystalTypeObjectList[j][l].isPicked, j, l);

                                l = aríditeCrystalTypeObjectList[j].Count;
                                j = aríditeCrystalTypeObjectList.Count;

                                break;
                            }
                        }
                    }
                }
            }
        }

        //Set New AríditeCrystal
        for (int i = 0; i < checkedAríditeCrystal.Count; i++)
        {
            for (int j = 0; j < checkedAríditeCrystal[i].Count; j++)
            {
                if (!checkedAríditeCrystal[i][j])
                {
                    if (aríditeCrystalWorldObject_Parent.transform.GetChild(i).transform.GetChild(j))
                    {
                        print("New AríditeCrystal: [" + i + "][" + j + "]");
                        //Give all Legal Objects an index
                        aríditeCrystalWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<AríditeCrystal>().aríditeCrystalIndex_x = i;
                        aríditeCrystalWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<AríditeCrystal>().aríditeCrystalIndex_y = j;

                        //Make a AríditeCrystalTypeObjectList
                        AríditeCrystalToSave tempAríditeCrystal = new AríditeCrystalToSave();

                        tempAríditeCrystal.isPicked = aríditeCrystalWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<AríditeCrystal>().isPicked;
                        tempAríditeCrystal.aríditeCrystalIndex_x = aríditeCrystalWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<AríditeCrystal>().aríditeCrystalIndex_x;
                        tempAríditeCrystal.aríditeCrystalIndex_y = aríditeCrystalWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<AríditeCrystal>().aríditeCrystalIndex_y;
                        tempAríditeCrystal.aríditeCrystalPos = aríditeCrystalWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<AríditeCrystal>().transform.position;

                        tempAríditeCrystalTypeObjectList[i].Add(tempAríditeCrystal);
                    }
                }
            }
        }

        //Set New BlueprintTypeObjectList
        aríditeCrystalTypeObjectList.Clear();
        aríditeCrystalTypeObjectList = tempAríditeCrystalTypeObjectList;

        SaveData();
    }
    public void ChangeAríditeCrystalInfo(bool _isPicked, int _aríditeCrystalIndex_j, int _aríditeCrystalIndex_l, Vector3 _aríditeCrystalPos)
    {
        AríditeCrystalToSave aríditeCrystalTree = new AríditeCrystalToSave();

        aríditeCrystalTree.isPicked = _isPicked;
        aríditeCrystalTree.aríditeCrystalIndex_x = _aríditeCrystalIndex_j;
        aríditeCrystalTree.aríditeCrystalIndex_y = _aríditeCrystalIndex_l;
        aríditeCrystalTree.aríditeCrystalPos = _aríditeCrystalPos;

        aríditeCrystalTypeObjectList[_aríditeCrystalIndex_j][_aríditeCrystalIndex_l] = aríditeCrystalTree;

        SaveData();
    }

}

[Serializable]
public class AríditeCrystalToSave
{
    public bool isPicked;

    public int aríditeCrystalIndex_x;
    public int aríditeCrystalIndex_y;

    public Vector3 aríditeCrystalPos = new Vector3();
}

[Serializable]
public class ListOfAríditeCrystalToSave
{
    public List<AríditeCrystalToSave> aríditeCrystalToSaveList = new List<AríditeCrystalToSave>();
}