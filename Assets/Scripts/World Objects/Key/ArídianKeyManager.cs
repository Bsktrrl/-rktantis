using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArídianKeyManager : Singleton<ArídianKeyManager>
{
    [Header("Folder Structure")]
    GameObject arídianKeyWorldObject_Parent;
    [SerializeField] List<List<ArídianKeyToSave>> arídianKeyTypeObjectList = new List<List<ArídianKeyToSave>>();


    //--------------------


    private void Awake()
    {
        arídianKeyWorldObject_Parent = GameObject.Find("Key_Parent");
    }


    //--------------------


    public void LoadData()
    {
        #region Save/Load Objects
        arídianKeyTypeObjectList.Clear();

        for (int i = 0; i < DataManager.Instance.arídianKeyTypeObjectList_Store.Count; i++)
        {
            List<ArídianKeyToSave> arídianKeyToSaveList = new List<ArídianKeyToSave>();

            arídianKeyTypeObjectList.Add(arídianKeyToSaveList);

            for (int j = 0; j < DataManager.Instance.arídianKeyTypeObjectList_Store[i].arídianKeyToSaveList.Count; j++)
            {
                arídianKeyTypeObjectList[arídianKeyTypeObjectList.Count - 1].Add(DataManager.Instance.arídianKeyTypeObjectList_Store[i].arídianKeyToSaveList[j]);
            }
        }

        SetupArídianKeyList();
        #endregion

        SaveData();
    }
    public void SaveData()
    {
        #region Save/Load Objects
        List<ListOfArídianKeyToSave> arídianKeyToSaveList = new List<ListOfArídianKeyToSave>();

        for (int i = 0; i < arídianKeyTypeObjectList.Count; i++)
        {
            ListOfArídianKeyToSave arídianKeyToSave = new ListOfArídianKeyToSave();

            arídianKeyToSaveList.Add(arídianKeyToSave);

            for (int j = 0; j < arídianKeyTypeObjectList[i].Count; j++)
            {
                arídianKeyToSaveList[arídianKeyToSaveList.Count - 1].arídianKeyToSaveList.Add(arídianKeyTypeObjectList[i][j]);
            }
        }

        DataManager.Instance.arídianKeyTypeObjectList_Store = arídianKeyToSaveList;
        #endregion
    }
    public void SaveData(ref GameData gameData)
    {
        SaveData();

        print("Save_ArídianKey");
    }


    //--------------------


    void SetupArídianKeyList()
    {
        List<List<ArídianKeyToSave>> tempArídianKeyTypeObjectList = new List<List<ArídianKeyToSave>>();
        List<List<bool>> checkedArídianKey = new List<List<bool>>();

        //Add elements to "checkedBlueprint" as children
        for (int i = 0; i < arídianKeyWorldObject_Parent.transform.childCount; i++)
        {
            List<ArídianKeyToSave> tempArídianKeytoSave = new List<ArídianKeyToSave>();
            tempArídianKeyTypeObjectList.Add(tempArídianKeytoSave);

            List<bool> templist_Outer = new List<bool>();
            checkedArídianKey.Add(templist_Outer);

            for (int j = 0; j < arídianKeyWorldObject_Parent.transform.GetChild(i).transform.childCount; j++)
            {
                bool templist_Inner = false;
                checkedArídianKey[i].Add(templist_Inner);
            }
        }

        //Set Old ArídianKey
        for (int i = 0; i < checkedArídianKey.Count; i++)
        {
            for (int k = 0; k < checkedArídianKey[i].Count; k++)
            {
                if (arídianKeyWorldObject_Parent.transform.GetChild(i).transform.GetChild(k).GetComponent<ArídianKey>())
                {
                    for (int j = 0; j < arídianKeyTypeObjectList.Count; j++)
                    {
                        for (int l = 0; l < arídianKeyTypeObjectList[j].Count; l++)
                        {
                            if (arídianKeyTypeObjectList[j][l].arídianKeyPos == arídianKeyWorldObject_Parent.transform.GetChild(i).transform.GetChild(k).GetComponent<ArídianKey>().transform.position)
                            {
                                //Setup Save_List
                                ArídianKeyToSave tempArídianKey = new ArídianKeyToSave();

                                tempArídianKey.isPicked = arídianKeyTypeObjectList[j][l].isPicked;
                                tempArídianKey.arídianKeyPos = arídianKeyTypeObjectList[j][l].arídianKeyPos;

                                tempArídianKeyTypeObjectList[i].Add(tempArídianKey);

                                checkedArídianKey[i][k] = true;

                                //Set info in Child
                                arídianKeyWorldObject_Parent.transform.GetChild(i).transform.GetChild(k).GetComponent<ArídianKey>().LoadArídianKey(arídianKeyTypeObjectList[j][l].isPicked, j, l);

                                l = arídianKeyTypeObjectList[j].Count;
                                j = arídianKeyTypeObjectList.Count;

                                break;
                            }
                        }
                    }
                }
            }
        }

        //Set New JournalPage
        for (int i = 0; i < checkedArídianKey.Count; i++)
        {
            for (int j = 0; j < checkedArídianKey[i].Count; j++)
            {
                if (!checkedArídianKey[i][j])
                {
                    if (arídianKeyWorldObject_Parent.transform.GetChild(i).transform.GetChild(j))
                    {
                        //print("New ArídianKey: [" + i + "][" + j + "]");
                        //Give all Legal Objects an index
                        arídianKeyWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<ArídianKey>().arídianKeyIndex_x = i;
                        arídianKeyWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<ArídianKey>().arídianKeyIndex_y = j;

                        //Make a TreeTypeObjectList
                        ArídianKeyToSave tempArídianKey = new ArídianKeyToSave();

                        tempArídianKey.isPicked = arídianKeyWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<ArídianKey>().isPicked;
                        tempArídianKey.arídianKeyIndex_x = arídianKeyWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<ArídianKey>().arídianKeyIndex_x;
                        tempArídianKey.arídianKeyIndex_y = arídianKeyWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<ArídianKey>().arídianKeyIndex_y;
                        tempArídianKey.arídianKeyPos = arídianKeyWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<ArídianKey>().transform.position;

                        tempArídianKeyTypeObjectList[i].Add(tempArídianKey);
                    }
                }
            }
        }

        //Set New BlueprintTypeObjectList
        arídianKeyTypeObjectList.Clear();
        arídianKeyTypeObjectList = tempArídianKeyTypeObjectList;

        SaveData();
    }
    public void ChangeArídianKeyInfo(bool _isPicked, int _arídianKeyIndex_j, int _arídianKeyIndex_l, Vector3 _arídianKeyPos)
    {
        ArídianKeyToSave arídianKeyTree = new ArídianKeyToSave();

        arídianKeyTree.isPicked = _isPicked;
        arídianKeyTree.arídianKeyIndex_x = _arídianKeyIndex_j;
        arídianKeyTree.arídianKeyIndex_y = _arídianKeyIndex_l;
        arídianKeyTree.arídianKeyPos = _arídianKeyPos;

        arídianKeyTypeObjectList[_arídianKeyIndex_j][_arídianKeyIndex_l] = arídianKeyTree;

        SaveData();
    }

}

[Serializable]
public class ArídianKeyToSave
{
    public bool isPicked;

    public int arídianKeyIndex_x;
    public int arídianKeyIndex_y;

    public Vector3 arídianKeyPos = new Vector3();
}

[Serializable]
public class ListOfArídianKeyToSave
{
    public List<ArídianKeyToSave> arídianKeyToSaveList = new List<ArídianKeyToSave>();
}