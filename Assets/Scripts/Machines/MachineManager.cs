using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineManager : Singleton<MachineManager>
{
    [Header("Parent")]
    GameObject ghostTank_Parent;

    [Header("Machine Lists")]
    public List<GhostTankContent> ghostTankList = new List<GhostTankContent>();
    public List<GameObject> ghostTankObjectList = new List<GameObject>();


    //--------------------


    public void LoadData()
    {
        if (DataManager.Instance.ghostTankList_Store.Count > 0)
        {
            //GhostTank
            ghostTankList = DataManager.Instance.ghostTankList_Store;
            for (int i = 0; i < ghostTankList.Count; i++)
            {
                ghostTankObjectList.Add(Instantiate(MainManager.Instance.GetMovableObject(FurnitureObjectNames.None, MachineObjectNames.GhostTank).objectToMove));

                GameObject tempTank = ghostTankObjectList[ghostTankObjectList.Count - 1];
                tempTank.transform.SetPositionAndRotation(ghostTankList[i].machinePos, ghostTankList[i].machineRot);
                tempTank.GetComponent<GhostTank>().SetupGhostTank(ghostTankList[i].GhostElement, ghostTankList[i].elementalFuelAmount);
            }
            

            //...
        }
        
        SaveData();
    }
    public void SaveData()
    {
        DataManager.Instance.ghostTankList_Store = ghostTankList;
    }
}
