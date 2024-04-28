using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CropPlotManager : Singleton<CropPlotManager>
{
    [Header("Plant Objects")]
    public GameObject ArídisPlantObject;
    public GameObject GluePlantObject;
    public GameObject CrimsonCloudBushObject;
    public GameObject RedCottonPlantObject;
    public GameObject SpikPlantObject;
    public GameObject SmallCactusPlantObject;
    public GameObject LargeCactusPlantObject;
    public GameObject PuddingCactusObject;
    public GameObject StalkFruitObject;
    public GameObject TripodFruitObject;
    public GameObject HeatFruitObject;
    public GameObject FreezeFruitObject;
    public GameObject TwistedMushroomObject;
    public GameObject GroundMushroomObject;
    public GameObject SandTubesObject;
    public GameObject PalmTreeObject;
    public GameObject BloodTreeObject;

    [Header("Plant Sprites")]
    public Sprite ArídisPlantSprite;
    public Sprite GluePlantSprite;
    public Sprite CrimsonCloudBushSprite;
    public Sprite RedCottonPlantSprite;
    public Sprite SpikPlantSprite;
    public Sprite SmallCactusPlantSprite;
    public Sprite LargeCactusPlantSprite;
    public Sprite PuddingCactusSprite;
    public Sprite StalkFruitSprite;
    public Sprite TripodFruitSprite;
    public Sprite HeatFruitSprite;
    public Sprite FreezeFruitSprite;
    public Sprite TwistedMushroomSprite;
    public Sprite GroundMushroomSprite;
    public Sprite SandTubesSprite;
    public Sprite PalmTreeSprite;
    public Sprite BloodTreeSprite;

    public Sprite emptyImage;
    public Sprite fillImage;

    [Header("Markers and Durability Icons")]
    List<int> hotbarMarkerInt = new List<int>();
    List<int> durabilityMarkerInt = new List<int>();

    [Header("Colors")]
    [SerializeField] Color seedColor;
    [SerializeField] Color notSeedColor;
    [SerializeField] Color visibleColor;
    [SerializeField] Color unvisibleColor;

    [Header("CropPlotMenuSlot")]
    [SerializeField] GameObject cropPlotSlot_Parent;
    [SerializeField] GameObject cropPlotSlot_Prefab;
    [SerializeField] List<GameObject> cropPlotSlotList = new List<GameObject>();
    public CropPlotInfo CropPlotInfo_Interacting;

    [Header("Growth")]
    public float cropPlot_GrowthTime_Max = 180;


    //--------------------


    private void Start()
    {
        cropPlot_GrowthTime_Max = 10;
    }
    private void Update()
    {
        if (MainManager.Instance.menuStates == MenuStates.CropPlotMenu)
        {
            UpdateCropPlotSlotsInfo();
        }
    }


    //--------------------


    public void AddSeed(Items seedName, int cropPlotSlotIndex)
    {
        //Set SeedName
        CropPlotInfo_Interacting.cropPlotSlotList[cropPlotSlotIndex].seedName_Input = seedName;

        //Set Timers
        CropPlotInfo_Interacting.cropPlotSlotList[cropPlotSlotIndex].current_GrowthTime = 0;
        CropPlotInfo_Interacting.cropPlotSlotList[cropPlotSlotIndex].max_GrowthTime = cropPlot_GrowthTime_Max;

        //Set Plant
        AddPlantToCropPlotSlot(seedName, SelectionManager.Instance.selectedObject.GetComponent<CropPlotSlot>().parent.GetComponent<CropPlot>().slotObjectList[cropPlotSlotIndex].GetComponent<CropPlotSlot>().plantSpot_Parent, cropPlotSlotIndex);

        SelectionManager.Instance.selectedObject.GetComponent<CropPlotSlot>().parent.GetComponent<Animations_Objects>().StartAnimation();

        //Set CropState
        CropPlotInfo_Interacting.cropPlotSlotList[cropPlotSlotIndex].cropState = CropState.Growing;

        BuildingSystemManager.Instance.UpdateWorldBuildingObjectInfoList_ToSave(SelectionManager.Instance.selectedObject.GetComponent<CropPlotSlot>().parent.gameObject.GetComponent<MoveableObject>());
        BuildingSystemManager.Instance.SaveData();
    }

    public void AddPlantToCropPlotSlot(Items seedName, GameObject plantSpot, int cropPlotSlotIndex)
    {
        GameObject plant = new GameObject();

        //Insert correct PlantPrefab into the CropPlot
        switch (seedName)
        {
            case Items.ArídisPlantSeed:
                plant = Instantiate(ArídisPlantObject, plantSpot.transform.forward, plantSpot.transform.rotation, plantSpot.transform);
                break;
            case Items.GluePlantSeed:
                plant = Instantiate(GluePlantObject, plantSpot.transform.forward, plantSpot.transform.rotation, plantSpot.transform);
                break;
            case Items.CrimsonCloudBushSeed:
                plant = Instantiate(CrimsonCloudBushObject, plantSpot.transform.forward, plantSpot.transform.rotation, plantSpot.transform);
                break;
            case Items.RedCottonPlantSeed:
                plant = Instantiate(RedCottonPlantObject, plantSpot.transform.forward, plantSpot.transform.rotation, plantSpot.transform);
                break;
            case Items.SpikPlantSeed:
                plant = Instantiate(SpikPlantObject, plantSpot.transform.forward, plantSpot.transform.rotation, plantSpot.transform);
                break;
            case Items.SmallCactusplantSeed:
                plant = Instantiate(SmallCactusPlantObject, plantSpot.transform.forward, plantSpot.transform.rotation, plantSpot.transform);
                break;
            case Items.LargeCactusplantSeed:
                plant = Instantiate(LargeCactusPlantObject, plantSpot.transform.forward, plantSpot.transform.rotation, plantSpot.transform);
                break;
            case Items.PuddingCactusSeed:
                plant = Instantiate(PuddingCactusObject, plantSpot.transform.forward, plantSpot.transform.rotation, plantSpot.transform);
                break;
            case Items.StalkFruitSeed:
                plant = Instantiate(StalkFruitObject, plantSpot.transform.forward, plantSpot.transform.rotation, plantSpot.transform);
                break;
            case Items.TripodFruitSeed:
                plant = Instantiate(TripodFruitObject, plantSpot.transform.forward, plantSpot.transform.rotation, plantSpot.transform);
                break;
            case Items.HeatFruitSeed:
                plant = Instantiate(HeatFruitObject, plantSpot.transform.forward, plantSpot.transform.rotation, plantSpot.transform);
                break;
            case Items.FreezeFruitSeed:
                plant = Instantiate(FreezeFruitObject, plantSpot.transform.forward, plantSpot.transform.rotation, plantSpot.transform);
                break;
            case Items.TwistedMushroomSeed:
                plant = Instantiate(TwistedMushroomObject, plantSpot.transform.forward, plantSpot.transform.rotation, plantSpot.transform);
                break;
            case Items.GroundMushroomSeed:
                plant = Instantiate(GroundMushroomObject, plantSpot.transform.forward, plantSpot.transform.rotation, plantSpot.transform);
                break;
            case Items.SandTubesSeed:
                plant = Instantiate(SandTubesObject, plantSpot.transform.forward, plantSpot.transform.rotation, plantSpot.transform);
                break;
            case Items.PalmTreeSeed:
                plant = Instantiate(PalmTreeObject, plantSpot.transform.forward, plantSpot.transform.rotation, plantSpot.transform);
                break;
            case Items.BloodTreeSeed:
                plant = Instantiate(BloodTreeObject, plantSpot.transform.forward, plantSpot.transform.rotation, plantSpot.transform);
                break;

            default:
                break;
        }

        //Set Plant info
        if (plant.GetComponent<Plant>())
        {
            plant.GetComponent<Plant>().plantIsReadyInCropPlot = false;
            plant.GetComponent<Plant>().isInCropPlot = true;
            plant.GetComponent<Plant>().CropPlotSlotIndex = cropPlotSlotIndex;

            plant.transform.SetLocalPositionAndRotation(new Vector3(0, 0.25f, 0), Quaternion.identity);
        }
    }
    public string GetPlantName(Items seedName)
    {
        switch (seedName)
        {
            case Items.ArídisPlantSeed:
                return PlantType.AdrídisFlower.ToString();
            case Items.GluePlantSeed:
                return PlantType.GluePlant.ToString();
            case Items.CrimsonCloudBushSeed:
                return PlantType.CrimsonCloudBush.ToString();
            case Items.RedCottonPlantSeed:
                return PlantType.RedCottonPlant.ToString();
            case Items.SpikPlantSeed:
                return PlantType.SpikeOilFruit.ToString();
            case Items.SmallCactusplantSeed:
                return PlantType.StemCactus.ToString();
            case Items.LargeCactusplantSeed:
                return "Cactus";
            case Items.PuddingCactusSeed:
                return PlantType.PuddingCactus.ToString();
            case Items.StalkFruitSeed:
                return PlantType.StalkFruit.ToString();
            case Items.TripodFruitSeed:
                return PlantType.ThriPod.ToString();
            case Items.HeatFruitSeed:
                return PlantType.HeatFruit.ToString();
            case Items.FreezeFruitSeed:
                return PlantType.FreezeFruit.ToString();
            case Items.TwistedMushroomSeed:
                return PlantType.TwistCap.ToString();
            case Items.GroundMushroomSeed:
                return PlantType.WartShroom.ToString();
            case Items.SandTubesSeed:
                return PlantType.SandTubes.ToString();
            case Items.PalmTreeSeed:
                return "Palm Tree";
            case Items.BloodTreeSeed:
                return "Palm Tree";

            default:
                return "";
        }
    }

    //--------------------


    public void SetupCropPlotSlots()
    {
        //Reset cropPlotSlotList
        for (int i = cropPlotSlotList.Count - 1; i >= 0; i--)
        {
            Destroy(cropPlotSlotList[i]);
        }
        cropPlotSlotList.Clear();

        //Make new cropPlotSlotList
        for (int i = 0; i < CropPlotInfo_Interacting.cropPlotSlotList.Count; i++)
        {
            cropPlotSlotList.Add(Instantiate(cropPlotSlot_Prefab, cropPlotSlot_Parent.transform) as GameObject);
            cropPlotSlotList[cropPlotSlotList.Count - 1].transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }
    }
    void UpdateCropPlotSlotsInfo()
    {
        for (int i = 0; i < CropPlotInfo_Interacting.cropPlotSlotList.Count; i++)
        {
            if (cropPlotSlotList[i].GetComponent<CropMenuSlot>())
            {
                //If empty
                if (CropPlotInfo_Interacting.cropPlotSlotList[i].cropState == CropState.Empty)
                {
                    //Set Text
                    cropPlotSlotList[i].GetComponent<CropMenuSlot>().plantName.text = "";

                    //Set Plant Image
                    cropPlotSlotList[i].GetComponent<CropMenuSlot>().plant_Image.sprite = emptyImage;

                    //Turn off FillImage
                    cropPlotSlotList[i].GetComponent<CropMenuSlot>().fill_Image.sprite = emptyImage;
                }
                else if (CropPlotInfo_Interacting.cropPlotSlotList[i].cropState == CropState.Growing)
                {
                    //Set Text
                    cropPlotSlotList[i].GetComponent<CropMenuSlot>().plantName.text = GetPlantName(CropPlotInfo_Interacting.cropPlotSlotList[i].seedName_Input);

                    //Set Plant Image
                    cropPlotSlotList[i].GetComponent<CropMenuSlot>().plant_Image.sprite = GetPlantImage(CropPlotInfo_Interacting.cropPlotSlotList[i].seedName_Input);

                    //Turn on FillImage
                    cropPlotSlotList[i].GetComponent<CropMenuSlot>().fill_Image.sprite = fillImage;

                    //Set Fill Image FillAmount
                    cropPlotSlotList[i].GetComponent<CropMenuSlot>().fill_Image.fillAmount = CropPlotInfo_Interacting.cropPlotSlotList[i].current_GrowthTime / CropPlotInfo_Interacting.cropPlotSlotList[i].max_GrowthTime;
                }
                else if (CropPlotInfo_Interacting.cropPlotSlotList[i].cropState == CropState.Finished)
                {
                    //Set Text
                    cropPlotSlotList[i].GetComponent<CropMenuSlot>().plantName.text = GetPlantName(CropPlotInfo_Interacting.cropPlotSlotList[i].seedName_Input);

                    //Set Plant Image
                    cropPlotSlotList[i].GetComponent<CropMenuSlot>().plant_Image.sprite = GetPlantImage(CropPlotInfo_Interacting.cropPlotSlotList[i].seedName_Input);

                    //Turn off FillImage
                    cropPlotSlotList[i].GetComponent<CropMenuSlot>().fill_Image.sprite = emptyImage;
                }
            }
        }
    }


    //--------------------


    Sprite GetPlantImage(Items itemName)
    {
        switch (itemName)
        {
            case Items.ArídisPlantSeed:
                return ArídisPlantSprite;
            case Items.GluePlantSeed:
                return GluePlantSprite;
            case Items.CrimsonCloudBushSeed:
                return CrimsonCloudBushSprite;
            case Items.RedCottonPlantSeed:
                return RedCottonPlantSprite;
            case Items.SpikPlantSeed:
                return SpikPlantSprite;
            case Items.SmallCactusplantSeed:
                return SmallCactusPlantSprite;
            case Items.LargeCactusplantSeed:
                return LargeCactusPlantSprite;
            case Items.PuddingCactusSeed:
                return PuddingCactusSprite;
            case Items.StalkFruitSeed:
                return StalkFruitSprite;
            case Items.TripodFruitSeed:
                return TripodFruitSprite;
            case Items.HeatFruitSeed:
                return HeatFruitSprite;
            case Items.FreezeFruitSeed:
                return FreezeFruitSprite;
            case Items.TwistedMushroomSeed:
                return TwistedMushroomSprite;
            case Items.GroundMushroomSeed:
                return GroundMushroomSprite;
            case Items.SandTubesSeed:
                return SandTubesSprite;
            case Items.PalmTreeSeed:
                return PalmTreeSprite;
            case Items.BloodTreeSeed:
                return BloodTreeSprite;

            default:
                return null;
        }
    }


    //--------------------


    public void UpdateCropPlotItemColor()
    {
        for (int i = 0; i < InventoryManager.Instance.itemSlotList_Player.Count; i++)
        {
            if (InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>())
            {
                //Get Item in slot
                Items itemName = InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().itemName;

                //Set Image Color to Dark
                if (InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().gameObject.GetComponent<Image>())
                {
                    if (itemName != Items.None)
                    {
                        InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().ChangeImageColor(notSeedColor);
                    }
                }

                //Reset color if seed
                if (itemName == Items.ArídisPlantSeed
                    || itemName == Items.GluePlantSeed
                    || itemName == Items.CrimsonCloudBushSeed
                    || itemName == Items.RedCottonPlantSeed
                    || itemName == Items.SpikPlantSeed
                    || itemName == Items.SmallCactusplantSeed
                    || itemName == Items.LargeCactusplantSeed
                    || itemName == Items.PuddingCactusSeed
                    || itemName == Items.StalkFruitSeed
                    || itemName == Items.TripodFruitSeed
                    || itemName == Items.HeatFruitSeed
                    || itemName == Items.FreezeFruitSeed
                    || itemName == Items.TwistedMushroomSeed
                    || itemName == Items.GroundMushroomSeed
                    || itemName == Items.SandTubesSeed
                    || itemName == Items.PalmTreeSeed
                    || itemName == Items.BloodTreeSeed)
                {
                    InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().ChangeImageColor(seedColor);
                }
            }
        }
    }
    public void UpdateCropPlotMarkers()
    {
        hotbarMarkerInt.Clear();
        durabilityMarkerInt.Clear();

        for (int i = 0; i < InventoryManager.Instance.itemSlotList_Player.Count; i++)
        {
            if (InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>())
            {
                //Hide Hotbar Marker
                if (InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().hotbarSelectorParent.activeInHierarchy)
                {
                    hotbarMarkerInt.Add(i);

                    InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().hotbarSelectorParent.SetActive(false);
                }

                //Hide Durability Marker
                if (InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().durabilityMeterParent.activeInHierarchy)
                {
                    durabilityMarkerInt.Add(i);

                    InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().durabilityMeterImage.gameObject.SetActive(false);
                    InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().durabilityMeterParent.SetActive(false);
                }
            }
        }
    }


    //--------------------


    public void ResetCropPlotMarkers()
    {
        for (int i = 0; i < InventoryManager.Instance.itemSlotList_Player.Count; i++)
        {
            for (int j = 0; j < hotbarMarkerInt.Count; j++)
            {
                if (hotbarMarkerInt[j] == i)
                {
                    if (InventoryManager.Instance.itemSlotList_Player[i].GetComponent<Image>() && InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>())
                    {
                        //Show Hotbar Marker
                        if (!InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().hotbarSelectorParent.activeInHierarchy)
                        {
                            InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().hotbarSelectorParent.SetActive(true);
                        }
                    }

                    break;
                }
            }

            for (int j = 0; j < durabilityMarkerInt.Count; j++)
            {
                if (durabilityMarkerInt[j] == i)
                {
                    if (InventoryManager.Instance.itemSlotList_Player[i].GetComponent<Image>() && InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>())
                    {
                        //Show Durability Marker
                        if (!InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().durabilityMeterParent.activeInHierarchy)
                        {
                            InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().durabilityMeterImage.gameObject.SetActive(true);
                            InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().durabilityMeterParent.SetActive(true);
                        }
                    }

                    break;
                }
            }
        }
    }
    public void ResetCropPlotItemColor()
    {
        for (int i = 0; i < InventoryManager.Instance.itemSlotList_Player.Count; i++)
        {
            //Set Image Color
            if (InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().gameObject.GetComponent<Image>())
            {
                if (InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().itemName == Items.None)
                {
                    InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().ChangeImageColor(unvisibleColor /*new Color(255, 255, 255, 0)*/);
                }
                else
                {
                    InventoryManager.Instance.itemSlotList_Player[i].GetComponent<ItemSlot>().ChangeImageColor(visibleColor /*new Color(255, 255, 255, 255)*/);
                }
            }
        }
    }
}
