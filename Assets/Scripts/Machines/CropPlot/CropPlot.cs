using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropPlot : MonoBehaviour
{
    public CropPlotInfo cropPlotInfo = new CropPlotInfo();


    //--------------------


    private void Update()
    {
        SaveGrowthProgress();
    }


    //--------------------

    public void SetupCropPlot()
    {
        //Get child of CropPlot - CropPlotSlot - to set its index for future reference
        for (int j = 0; j < transform.childCount; j++)
        {
            if (transform.GetChild(j).gameObject.GetComponent<CropPlotSlot>())
            {
                transform.GetChild(j).gameObject.GetComponent<CropPlotSlot>().slotIndex = j;
            }
        }

        //Save Object Setup
        BuildingSystemManager.Instance.UpdateWorldBuildingObjectInfoList_ToSave(GetComponent<MoveableObject>());
    }
    public void SetupCropPlot(CropPlotInfo cropPlotInfo)
    {
        //Get child of CropPlot - CropPlotSlot - to set its index for future reference
        for (int j = 0; j < transform.childCount; j++)
        {
            if (transform.GetChild(j).gameObject.GetComponent<CropPlotSlot>())
            {
                transform.GetChild(j).gameObject.GetComponent<CropPlotSlot>().slotIndex = j;
            }
        }

        //Save Object Setup
        BuildingSystemManager.Instance.UpdateWorldBuildingObjectInfoList_ToSave(GetComponent<MoveableObject>());
    }


    //--------------------


    void SaveGrowthProgress()
    {

    }
}

[Serializable]
public class CropPlotInfo
{
    public List<CropPlotSlots> cropPlotSlotList = new List<CropPlotSlots>();
}

[Serializable]
public class CropPlotSlots
{
    public CropPlotSlot slot;
    public int slotIndex;

    public Items seedName_Input;
    public Items plantName_Output;

    public float max_GrowthTime;
    public float current_GrowthTime;
}
