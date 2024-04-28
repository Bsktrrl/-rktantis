using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropPlot : MonoBehaviour
{
    public List<GameObject> slotObjectList;

    public CropPlotInfo cropPlotInfo = new CropPlotInfo();

    public bool hasLoaded = false;

    public bool growthConditions;


    //--------------------


    private void Update()
    {
        if (!DataManager.Instance.hasLoaded) { return; }

        CheckAnimation();

        growthConditions = GetIfGhostTankHasGhost();
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
            if (cropPlotInfo.cropPlotSlotList[i].cropState == CropState.Growing && growthConditions)
            {
                GetComponent<Animations_Objects>().StartAnimation();

                return;
            }
        }

        GetComponent<Animations_Objects>().StopAnimation();
    }


    //--------------------


    public void DestroyThisCropPlotObject()
    {
        print("444444. DestroyCropPlotObject");

        for (int i = slotObjectList.Count - 1; i >= 0; i--)
        {
            print("--------------------------------");

            if (slotObjectList[i].GetComponent<CropPlotSlot>())
            {
                if (slotObjectList[i].GetComponent<CropPlotSlot>().plantSpot_Parent.transform.childCount > 0)
                {
                    if (slotObjectList[i].GetComponent<CropPlotSlot>().plantSpot_Parent.transform.GetChild(0).gameObject.GetComponent<Plant>())
                    {
                        //Destroy PickableObject from plant
                        if (slotObjectList[i].GetComponent<CropPlotSlot>().plantSpot_Parent.transform.GetChild(0).gameObject.GetComponent<Plant>().pickablePart)
                        {
                            if (slotObjectList[i].GetComponent<CropPlotSlot>().plantSpot_Parent.transform.GetChild(0).gameObject.GetComponent<Plant>().pickablePart.GetComponent<InteractableObject>())
                            {
                                slotObjectList[i].GetComponent<CropPlotSlot>().plantSpot_Parent.transform.GetChild(0).gameObject.GetComponent<Plant>().pickablePart.GetComponent<InteractableObject>().DestroyThisInteractableObject();
                            }
                        }

                        //Destroy Plant
                        slotObjectList[i].GetComponent<CropPlotSlot>().plantSpot_Parent.transform.GetChild(0).gameObject.GetComponent<Plant>().DestroyThisPlantObject();
                    }
                }

                //Destroy CropPlotSlot
                slotObjectList[i].GetComponent<InteractableObject>().DestroyThisInteractableObject();
            }
        }

        print("--------------------------------");

        //Destroy Connection
        if (gameObject.GetComponent<MoveableObject>())
        {
            if (gameObject.GetComponent<MoveableObject>().connectionPointObject)
            {
                if (gameObject.GetComponent<MoveableObject>().connectionPointObject.GetComponent<ConnectionPoint>())
                {
                    gameObject.GetComponent<MoveableObject>().connectionPointObject.GetComponent<ConnectionPoint>().DestroyThisConnectionObject();
                }
            }
        }

        print("--------------------------------");

        if (gameObject.GetComponent<InteractableObject>())
        {
            gameObject.GetComponent<InteractableObject>().DestroyThisInteractableObject();
        }

        print("--------------------------------");

        print("Destroy GameObject: " + gameObject.name);
        //Destroy CropPlot
        Destroy(gameObject);
    }


    //--------------------


    bool GetIfGhostTankHasGhost()
    {
        if (gameObject.GetComponent<MoveableObject>().connectionPointObject)
        {
            if (gameObject.GetComponent<MoveableObject>().connectionPointObject.GetComponent<ConnectionPoint>().worldObjectIndex_ConnectedWith >= 0)
            {
                //Get Connected BuildingObject
                GameObject obj = BuildingSystemManager.Instance.worldBuildingObjectListSpawned[gameObject.GetComponent<MoveableObject>().connectionPointObject.GetComponent<ConnectionPoint>().worldObjectIndex_ConnectedWith];

                //If getting a CropPlot
                if (obj.GetComponent<GhostTank>())
                {
                    if (obj.GetComponent<GhostTank>().ghostTankContent.GhostElement != GhostElement.None)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
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
