using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropPlotSlot : MonoBehaviour
{
    public GameObject parent;
    public int slotIndex;

    public GameObject plantSpot;


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
                PickUpPlant();
                break;

            default:
                break;
        }
    }


    //--------------------


    void OpenCropPlotTabletMenu()
    {
        TabletManager.Instance.objectInteractingWith_Object = gameObject;

        //Open the crafting menu
        TabletManager.Instance.OpenTablet(TabletMenuState.CropPlot);

        TabletManager.Instance.objectInteractingWith = ObjectInteractingWith.CropPlot;
    }

    void AddSeed()
    {
        //Insert correct PlantPrefab into the CropPlot
        //Remove and set the correct image in the Tablet
        switch (HotbarManager.Instance.selectedItem)
        {
            case Items.ArídisPlantSeed:
                plantSpot = CropPlotManager.Instance.ArídisPlantObject;
                break;
            case Items.GluePlantSeed:
                plantSpot = CropPlotManager.Instance.GluePlantObject;
                break;
            case Items.CrimsonCloudBushSeed:
                plantSpot = CropPlotManager.Instance.CrimsonCloudBushObject;
                break;
            case Items.RedCottonPlantSeed:
                plantSpot = CropPlotManager.Instance.RedCottonPlantObject;
                break;
            case Items.SpikPlantSeed:
                plantSpot = CropPlotManager.Instance.SpikPlantObject;
                break;
            case Items.SmallCactusplantSeed:
                plantSpot = CropPlotManager.Instance.SmallCactusPlantObject;
                break;
            case Items.LargeCactusplantSeed:
                plantSpot = CropPlotManager.Instance.LargeCactusPlantObject;
                break;
            case Items.PuddingCactusSeed:
                plantSpot = CropPlotManager.Instance.PuddingCactusObject;
                break;
            case Items.StalkFruitSeed:
                plantSpot = CropPlotManager.Instance.StalkFruitObject;
                break;
            case Items.TripodFruitSeed:
                plantSpot = CropPlotManager.Instance.TripodFruitObject;
                break;
            case Items.HeatFruitSeed:
                plantSpot = CropPlotManager.Instance.HeatFruitObject;
                break;
            case Items.FreezeFruitSeed:
                plantSpot = CropPlotManager.Instance.FreezeFruitObject;
                break;
            case Items.TwistedMushroomSeed:
                plantSpot = CropPlotManager.Instance.TwistedMushroomObject;
                break;
            case Items.GroundMushroomSeed:
                plantSpot = CropPlotManager.Instance.GroundMushroomObject;
                break;
            case Items.SandTubesSeed:
                plantSpot = CropPlotManager.Instance.SandTubesObject;
                break;
            case Items.PalmTreeSeed:
                plantSpot = CropPlotManager.Instance.PalmTreeObject;
                break;
            case Items.BloodTreeSeed:
                plantSpot = CropPlotManager.Instance.BloodTreeObject;
                break;

            default:
                break;
        }
    }
    void PickUpPlant()
    {
        InventoryManager.Instance.AddItemToInventory(0, GetCropPlotSlotInfo().plantName_Output);
    }


    void SetPlantGrowth()
    {
        //GrowthTimer



        //Scale based on GrowthTimer


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