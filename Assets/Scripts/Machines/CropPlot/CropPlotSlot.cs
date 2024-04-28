using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropPlotSlot : MonoBehaviour
{
    public GameObject parent;
    public int slotIndex;

    public GameObject plantSpot_Parent;


    //--------------------


    private void Update()
    {
        if (BuildingSystemManager.Instance.ghostObject_Holding == parent) { return; }

        if (!parent.gameObject.GetComponent<CropPlot>().hasLoaded) { return; }

        if (parent.gameObject.GetComponent<CropPlot>())
        {
            if (parent.gameObject.GetComponent<CropPlot>().cropPlotInfo != null)
            {
                if (parent.gameObject.GetComponent<CropPlot>().cropPlotInfo.cropPlotSlotList.Count >= slotIndex)
                {
                    //When the Plant grows
                    if (parent.gameObject.GetComponent<CropPlot>().cropPlotInfo.cropPlotSlotList[slotIndex].cropState == CropState.Growing)
                    {
                        if (plantSpot_Parent.transform.childCount > 0)
                        {
                            SetPlantGrowth();
                        }
                    }

                    //When the plant gets picked
                    else if (parent.gameObject.GetComponent<CropPlot>().cropPlotInfo.cropPlotSlotList[slotIndex].cropState == CropState.Finished)
                    {
                        if (plantSpot_Parent.transform.childCount > 0)
                        {
                            SetPlantGrowth();
                        }

                        if (plantSpot_Parent.transform.childCount <= 0)
                        {
                            PickUpPlant_Aftermath();
                        }
                    }
                }
            }
        }
    }


    //--------------------


    public void SetupPlantInCropSlotPlot()
    {
        if (parent.gameObject.GetComponent<CropPlot>().cropPlotInfo.cropPlotSlotList[slotIndex].cropState != CropState.Empty)
        {
            //print("Error - Slotindex: " + slotIndex);
            CropPlotManager.Instance.AddPlantToCropPlotSlot(parent.gameObject.GetComponent<CropPlot>().cropPlotInfo.cropPlotSlotList[slotIndex].seedName_Input, plantSpot_Parent, slotIndex);

            if (parent.gameObject.GetComponent<CropPlot>().cropPlotInfo.cropPlotSlotList[slotIndex].cropState == CropState.Finished)
            {
                if (plantSpot_Parent.transform.childCount > 0)
                {
                    plantSpot_Parent.transform.GetChild(0).gameObject.GetComponent<Plant>().plantIsReadyInCropPlot = true;
                }
            }
        }
    }


    //--------------------


    public void InteractWithCropPlotSlot()
    {
        switch (GetCropPlotSlotInfo().cropState)
        {
            case CropState.Empty:
                OpenCropPlotTabletMenu();
                break;
            case CropState.Growing:
                OpenCropPlotTabletMenu();
                break;

            case CropState.Finished:
                PickUpPlantFromTheCropPlot();
                break;

            default:
                break;
        }
    }


    //--------------------


    void OpenCropPlotTabletMenu()
    {
        //Update the Manager with new info
        CropPlotManager.Instance.CropPlotInfo_Interacting = parent.GetComponent<CropPlot>().cropPlotInfo;

        //Open Tablet
        TabletManager.Instance.objectInteractingWith_Object = gameObject;
        TabletManager.Instance.OpenTablet(TabletMenuState.CropPlot);
        TabletManager.Instance.objectInteractingWith = ObjectInteractingWith.CropPlot;
    }
    void PickUpPlantFromTheCropPlot()
    {
        if (plantSpot_Parent.transform.childCount > 0)
        {
            if (plantSpot_Parent.transform.GetChild(0))
            {
                if (plantSpot_Parent.transform.GetChild(0).gameObject.GetComponent<Plant>())
                {
                    if (plantSpot_Parent.transform.GetChild(0).gameObject.GetComponent<Plant>().plantIsReadyInCropPlot)
                    {
                        SoundManager.Instance.Play_Inventory_PickupItem_Clip();

                        //Check If item can be added
                        InventoryManager.Instance.AddItemToInventory(0, GetPlantItem());

                        plantSpot_Parent.transform.GetChild(0).gameObject.GetComponent<Plant>().pickablePart.GetComponent<InteractableObject>().DestroyThisInteractableObject();
                        plantSpot_Parent.transform.GetChild(0).gameObject.GetComponent<Plant>().DestroyThisPlantObject();
                    }

                    InventoryManager.Instance.AddItemToInventory(0, GetCropPlotSlotInfo().plantName_Output);
                }
            }
        }
    }
    void PickUpPlant_Aftermath()
    {
        parent.gameObject.GetComponent<CropPlot>().cropPlotInfo.cropPlotSlotList[slotIndex].current_GrowthTime = 0;
        parent.gameObject.GetComponent<CropPlot>().cropPlotInfo.cropPlotSlotList[slotIndex].cropState = CropState.Empty;
        parent.gameObject.GetComponent<CropPlot>().cropPlotInfo.cropPlotSlotList[slotIndex].seedName_Input = Items.None;
        parent.gameObject.GetComponent<CropPlot>().cropPlotInfo.cropPlotSlotList[slotIndex].plantName_Output = Items.None;

        BuildingSystemManager.Instance.UpdateWorldBuildingObjectInfoList_ToSave(parent.GetComponent<MoveableObject>());
    }

    Items GetPlantItem()
    {
        switch (plantSpot_Parent.transform.GetChild(0).gameObject.GetComponent<Plant>().plantType)
        {
            case PlantType.AdrídisFlower:
                return Items.ArídisFlower;
            case PlantType.FreezeFruit:
                return Items.FreezeFruit;
            case PlantType.CrimsonCloudBush:
                return Items.Cotton;
            case PlantType.GluePlant:
                return Items.GlueStick;
            case PlantType.HeatFruit:
                return Items.HeatFruit;
            case PlantType.PuddingCactus:
                return Items.PuddingCactus;
            case PlantType.RedCottonPlant:
                return Items.Cotton;
            case PlantType.SandTubes:
                return Items.TubePlastic;
            case PlantType.SpikeOilFruit:
                return Items.SpikOil;
            case PlantType.StalkFruit:
                return Items.StalkFruit;
            case PlantType.StemCactus:
                return Items.Cactus;
            case PlantType.ThriPod:
                return Items.TripodFruit;
            case PlantType.TwistCap:
                return Items.TwistedMushroom;
            case PlantType.WartShroom:
                return Items.TwistedMushroom;

            default:
                return Items.GlueStick;
        }
    }

    void SetPlantGrowth()
    {
        //print("Plant - Growing: Percentage: " + parent.gameObject.GetComponent<CropPlot>().cropPlotInfo.cropPlotSlotList[slotIndex].current_GrowthTime / parent.gameObject.GetComponent<CropPlot>().cropPlotInfo.cropPlotSlotList[slotIndex].max_GrowthTime);

        //GrowthTimer
        parent.gameObject.GetComponent<CropPlot>().cropPlotInfo.cropPlotSlotList[slotIndex].current_GrowthTime += Time.deltaTime;

        Vector3 scale_1 = new Vector3(0.1f, 2.8f, -0.1f);
        Vector3 scale_2 = new Vector3(0.8f, 2.8f, -0.2f);
        Vector3 scale_3 = new Vector3(0.5f, 2.8f, 0.3f);

        float percentage = parent.gameObject.GetComponent<CropPlot>().cropPlotInfo.cropPlotSlotList[slotIndex].current_GrowthTime / parent.gameObject.GetComponent<CropPlot>().cropPlotInfo.cropPlotSlotList[slotIndex].max_GrowthTime;

        //Growing
        if (percentage < 1)
        {
            parent.gameObject.GetComponent<CropPlot>().cropPlotInfo.cropPlotSlotList[slotIndex].cropState = CropState.Growing;

            if (parent.gameObject.GetComponent<CropPlot>().cropPlotInfo.cropPlotSlotList.Count == 1)
            {
                plantSpot_Parent.transform.GetChild(0).gameObject.transform.localScale = (Vector3.one * percentage) + (scale_1 * percentage);
            }
            else if (parent.gameObject.GetComponent<CropPlot>().cropPlotInfo.cropPlotSlotList.Count == 2)
            {
                plantSpot_Parent.transform.GetChild(0).gameObject.transform.localScale = (Vector3.one * percentage) + (scale_2 * percentage);
            }
            else if (parent.gameObject.GetComponent<CropPlot>().cropPlotInfo.cropPlotSlotList.Count == 4)
            {
                plantSpot_Parent.transform.GetChild(0).gameObject.transform.localScale = (Vector3.one * percentage) + (scale_3 * percentage);
            }

            if (plantSpot_Parent.transform.childCount > 0)
            {
                if (plantSpot_Parent.transform.GetChild(0).gameObject.GetComponent<Plant>())
                {
                    plantSpot_Parent.transform.GetChild(0).gameObject.GetComponent<Plant>().plantIsReadyInCropPlot = true;
                }
            }
        }

        //Finished
        else
        {
            parent.gameObject.GetComponent<CropPlot>().cropPlotInfo.cropPlotSlotList[slotIndex].cropState = CropState.Finished;

            if (parent.gameObject.GetComponent<CropPlot>().cropPlotInfo.cropPlotSlotList.Count == 1)
            {
                plantSpot_Parent.transform.GetChild(0).gameObject.transform.localScale = Vector3.one + scale_1;
            }
            else if (parent.gameObject.GetComponent<CropPlot>().cropPlotInfo.cropPlotSlotList.Count == 2)
            {
                plantSpot_Parent.transform.GetChild(0).gameObject.transform.localScale = Vector3.one + scale_2;
            }
            else if (parent.gameObject.GetComponent<CropPlot>().cropPlotInfo.cropPlotSlotList.Count == 4)
            {
                plantSpot_Parent.transform.GetChild(0).gameObject.transform.localScale = Vector3.one + scale_3;
            }
        }
    }


    //--------------------


    public CropPlotSlots GetCropPlotSlotInfo()
    {
        return parent.GetComponent<CropPlot>().cropPlotInfo.cropPlotSlotList[slotIndex];
    }
}

public enum CropState
{
    Empty,
    Growing,
    Finished
}