using System;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : Singleton<TreeManager>
{
    public float dormantTimer = 2;

    [Header("Folder Structure")]
    GameObject treeWorldObject_Parent;
    [SerializeField] List<List<TreeToSave>> treeTypeObjectList = new List<List<TreeToSave>>();

    [Header("Pickaxe Stats")]
    public float woodAxe_Droprate = 55;
    public float stoneAxe_Droprate = 65;
    public float cryoniteAxe_Droprate = 75;

    [Header("Hand Damage Stats")]
    public float handDamage = 0.5f;

    [Header("SkillTree Stats")]
    [HideInInspector] public int treeHealthReducer = 0;
    public int treeDropRateReducer = 15;


    //--------------------


    private void Awake()
    {
        treeWorldObject_Parent = GameObject.Find("Tree_Parent");
    }


    //--------------------

    public void LoadData()
    {
        treeTypeObjectList.Clear();

        for (int i = 0; i < DataManager.Instance.treeTypeObjectList_Store.Count; i++)
        {
            List<TreeToSave> treeToSaveList = new List<TreeToSave>();

            treeTypeObjectList.Add(treeToSaveList);

            for (int j = 0; j < DataManager.Instance.treeTypeObjectList_Store[i].treeToSaveList.Count; j++)
            {
                treeTypeObjectList[treeTypeObjectList.Count - 1].Add(DataManager.Instance.treeTypeObjectList_Store[i].treeToSaveList[j]);
            }
        }

        SetupTreeList();
    }
    public void SaveData()
    {
        List<ListOfTreeToSave> treeToSaveList = new List<ListOfTreeToSave>();

        for (int i = 0; i < treeTypeObjectList.Count; i++)
        {
            ListOfTreeToSave treeToSave = new ListOfTreeToSave();

            treeToSaveList.Add(treeToSave);

            for (int j = 0; j < treeTypeObjectList[i].Count; j++)
            {
                treeToSaveList[treeToSaveList.Count - 1].treeToSaveList.Add(treeTypeObjectList[i][j]);
            }
        }

        DataManager.Instance.treeTypeObjectList_Store = treeToSaveList;
    }
    public void SaveData(ref GameData gameData)
    {
        SaveData();

        print("Save_TreeList");
    }


    //--------------------


    void SetupTreeList()
    {
        List<List<TreeToSave>> tempTreeTypeObjectList = new List<List<TreeToSave>>();
        List<List<bool>> checkedTrees = new List<List<bool>>();

        //Add elements to "checkedTrees" as children
        for (int i = 0; i < treeWorldObject_Parent.transform.childCount; i++)
        {
            List<TreeToSave> tempTreetoSave = new List<TreeToSave>();
            tempTreeTypeObjectList.Add(tempTreetoSave);

            List<bool> templist_Outer = new List<bool>();
            checkedTrees.Add(templist_Outer);

            for (int j = 0; j < treeWorldObject_Parent.transform.GetChild(i).transform.childCount; j++)
            {
                bool templist_Inner = false;
                checkedTrees[i].Add(templist_Inner);
            }
        }

        //Set Old Trees
        for (int i = 0; i < checkedTrees.Count; i++)
        {
            for (int k = 0; k < checkedTrees[i].Count; k++)
            {
                if (treeWorldObject_Parent.transform.GetChild(i).transform.GetChild(k).GetComponent<Tree>())
                {
                    for (int j = 0; j < treeTypeObjectList.Count; j++)
                    {
                        for (int l = 0; l < treeTypeObjectList[j].Count; l++)
                        {
                            if (treeTypeObjectList[j][l].treePos == treeWorldObject_Parent.transform.GetChild(i).transform.GetChild(k).GetComponent<Tree>().transform.position)
                            {
                                //Setup Save_List
                                TreeToSave tempTree = new TreeToSave();

                                tempTree.isCut = treeTypeObjectList[j][l].isCut;
                                tempTree.dormantTimer = treeTypeObjectList[j][l].dormantTimer;
                                tempTree.treeIndex_x = j;
                                tempTree.treeIndex_y = l;
                                tempTree.percentageCheck = treeTypeObjectList[j][l].percentageCheck;
                                tempTree.treeHealth = treeTypeObjectList[j][l].treeHealth;
                                tempTree.treePos = treeTypeObjectList[j][l].treePos;

                                tempTreeTypeObjectList[i].Add(tempTree);

                                checkedTrees[i][k] = true;

                                //Set info in Child
                                treeWorldObject_Parent.transform.GetChild(i).transform.GetChild(k).GetComponent<Tree>().LoadTree(treeTypeObjectList[j][l].isCut, treeTypeObjectList[j][l].dormantTimer, j, l, treeTypeObjectList[j][l].percentageCheck, treeTypeObjectList[j][l].treeHealth);

                                l = treeTypeObjectList[j].Count;
                                j = treeTypeObjectList.Count;

                                break;
                            }
                        }
                    }
                }
            }
        }

        //Set New Trees
        for (int i = 0; i < checkedTrees.Count; i++)
        {
            for (int j = 0; j < checkedTrees[i].Count; j++)
            {
                if (!checkedTrees[i][j])
                {
                    print("New Trees: [" + i + "][" + j + "]");

                    if (treeWorldObject_Parent.transform.GetChild(i).transform.GetChild(j))
                    {
                        //Give all Legal Objects an index
                        treeWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<Tree>().treeIndex_x = i;
                        treeWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<Tree>().treeIndex_y = j;

                        //Make a TreeTypeObjectList
                        TreeToSave tempTree = new TreeToSave();

                        tempTree.isCut = treeWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<Tree>().isCut;
                        tempTree.dormantTimer = treeWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<Tree>().dormantTimer;
                        tempTree.treeIndex_x = treeWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<Tree>().treeIndex_x;
                        tempTree.treeIndex_y = treeWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<Tree>().treeIndex_y;
                        tempTree.percentageCheck = treeWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<Tree>().percentageCheck;
                        tempTree.treeHealth = treeWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<Tree>().treeHealth;
                        tempTree.treePos = treeWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<Tree>().transform.position;

                        tempTreeTypeObjectList[i].Add(tempTree);
                    }
                }
            }
        }

        //Set New TreeTypeObjectList
        treeTypeObjectList.Clear();
        treeTypeObjectList = tempTreeTypeObjectList;

        //Set Mesh visibility
        for (int i = 0; i < checkedTrees.Count; i++)
        {
            for (int j = 0; j < checkedTrees[i].Count; j++)
            {
                if (treeWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<Tree>().isCut)
                {
                    treeWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<Tree>().mesh.SetActive(false);
                    if (treeWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<Tree>().treeObject_LOD0)
                        treeWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<Tree>().treeObject_LOD0.SetActive(false);
                    if (treeWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<Tree>().treeObject_LOD1)
                        treeWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<Tree>().treeObject_LOD1.SetActive(false);
                    if (treeWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<Tree>().treeObject_LOD2)
                        treeWorldObject_Parent.transform.GetChild(i).transform.GetChild(j).GetComponent<Tree>().treeObject_LOD2.SetActive(false);
                }
            }
        }

        SaveData();
    }

    public void ChangeTreeInfo(bool _isCut, float _dormantTimer, int _treeIndex_j, int _treeIndex_l, int _percentageCheck, float _treeHealth, Vector3 _treePos)
    {
        TreeToSave tempTree = new TreeToSave();

        tempTree.isCut = _isCut;
        tempTree.dormantTimer = _dormantTimer;
        tempTree.treeIndex_x = _treeIndex_j;
        tempTree.treeIndex_y = _treeIndex_l;
        tempTree.percentageCheck = _percentageCheck;
        tempTree.treeHealth = _treeHealth;
        tempTree.treePos = _treePos;

        treeTypeObjectList[_treeIndex_j][_treeIndex_l] = tempTree;

        SaveData();
    }
}

[Serializable]
public class TreeToSave
{
    public bool isCut;
    public float dormantTimer;

    public int treeIndex_x;
    public int treeIndex_y;
    public float treeHealth;

    public int percentageCheck;

    public Vector3 treePos = new Vector3();
}

[Serializable]
public class ListOfTreeToSave
{
    public List<TreeToSave> treeToSaveList = new List<TreeToSave>();
}