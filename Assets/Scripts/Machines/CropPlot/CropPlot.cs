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

    public void SetupCropPlot() //New CropPlot
    {
        print("1. SetupCropPlot - New CropPlotMenu | ChildCount: " + transform.childCount);

        //Get child of CropPlot - CropPlotSlot - to set its index for future reference
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.GetComponent<CropPlotSlot>())
            {
                print("11. SetupCropPlot Count" + i);
                transform.GetChild(i).gameObject.GetComponent<CropPlotSlot>().slotIndex = i;

                CropPlotSlots cropPlotSlots = new CropPlotSlots();
                cropPlotSlots.slotIndex = i;
                cropPlotSlots.cropState = CropState.Empty;

                cropPlotInfo.cropPlotSlotList.Add(cropPlotSlots);
            }
        }

        //Save Object Setup
        BuildingSystemManager.Instance.UpdateWorldBuildingObjectInfoList_ToSave(GetComponent<MoveableObject>());
    }
    public void SetupCropPlot(CropPlotInfo _cropPlotInfo) //Loaded CropPlot
    {
        print("2. SetupCropPlot - Loaded CropPlotMenu");

        //Set Info
        for (int i = 0; i < _cropPlotInfo.cropPlotSlotList.Count; i++)
        {
            print("22. SetupCropPlot Count" + i);

            CropPlotSlots cropPlotSlots = new CropPlotSlots();

            cropPlotSlots.slotIndex = _cropPlotInfo.cropPlotSlotList[i].slotIndex;

            cropPlotSlots.cropState = _cropPlotInfo.cropPlotSlotList[i].cropState;
            cropPlotSlots.seedName_Input = _cropPlotInfo.cropPlotSlotList[i].seedName_Input;
            cropPlotSlots.plantName_Output = _cropPlotInfo.cropPlotSlotList[i].plantName_Output;

            cropPlotSlots.max_GrowthTime = _cropPlotInfo.cropPlotSlotList[i].max_GrowthTime;
            cropPlotSlots.current_GrowthTime = _cropPlotInfo.cropPlotSlotList[i].current_GrowthTime;

            cropPlotInfo.cropPlotSlotList.Add(cropPlotSlots);
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
    public int slotIndex;

    public CropState cropState;

    public Items seedName_Input;
    public Items plantName_Output;

    public float max_GrowthTime;
    public float current_GrowthTime;
}
