using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropPlot : MonoBehaviour
{
    public List<GameObject> slotObjectList;

    public CropPlotInfo cropPlotInfo = new CropPlotInfo();

    public bool hasLoaded = false;


    //--------------------


    private void Update()
    {
        CheckAnimation();
    }


    //--------------------

    public void SetupCropPlot() //New CropPlot
    {
        //Get child of CropPlot - CropPlotSlot - to set its index for future reference
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.GetComponent<CropPlotSlot>())
            {
                transform.GetChild(i).gameObject.GetComponent<CropPlotSlot>().slotIndex = i;

                CropPlotSlots cropPlotSlots = new CropPlotSlots();
                cropPlotSlots.slotIndex = i;
                cropPlotSlots.cropState = CropState.Empty;

                cropPlotInfo.cropPlotSlotList.Add(cropPlotSlots);
            }
        }

        //Save Object Setup
        BuildingSystemManager.Instance.UpdateWorldBuildingObjectInfoList_ToSave(GetComponent<MoveableObject>());

        hasLoaded = true;
    }
    public void SetupCropPlot(CropPlotInfo _cropPlotInfo) //Loaded CropPlot
    {
        //Set Info
        for (int i = 0; i < _cropPlotInfo.cropPlotSlotList.Count; i++)
        {
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

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            if (gameObject.transform.GetChild(i).gameObject.GetComponent<CropPlotSlot>())
            {
                gameObject.transform.GetChild(i).gameObject.GetComponent<CropPlotSlot>().SetupPlantInCropSlotPlot();
            }
        }

        hasLoaded = true;
    }


    //--------------------


    void CheckAnimation()
    {
        for (int i = 0; i < cropPlotInfo.cropPlotSlotList.Count; i++)
        {
            if (cropPlotInfo.cropPlotSlotList[i].cropState == CropState.Growing)
            {
                GetComponent<Animations_Objects>().StartAnimation();

                return;
            }
        }

        GetComponent<Animations_Objects>().StopAnimation();
    }


    //--------------------


    public void DestroyThisObject()
    {
        print("4. DestroyObject");

        for (int i = slotObjectList.Count - 1; i >= 0; i--)
        {
            if (slotObjectList[i].GetComponent<InteractableObject>())
            {
                print("5. DestroyObject: No: " + i);
                slotObjectList[i].GetComponent<InteractableObject>().DestroyThisObject();
            }
        }

        Destroy(gameObject);
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
