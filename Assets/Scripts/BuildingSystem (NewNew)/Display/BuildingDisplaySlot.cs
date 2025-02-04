using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingDisplaySlot : MonoBehaviour, IPointerEnterHandler
{
    public BuildingObjectTypes buildingObjectType;
    public BuildingMaterial buildingMaterial;

    public BuildingBlockObjectNames buildingBlockObjectName;
    public FurnitureObjectNames furnitureObjectName;
    public MachineObjectNames machineObjectName;

    [Header("+ Sign")]
    public GameObject Image_Pluss;


    //--------------------


    public void SetupIfPlussIsActive(bool isActive)
    {
        if (Image_Pluss)
        {
            Image_Pluss.SetActive(isActive);
        }
    }


    //--------------------


    public void SelectButton_isPressed()
    {
        SoundManager.Instance.Play_Crafting_ChangeCraftingMenu_Clip();

        BuildingDisplayManager.Instance.UpdateMenuPlussSignsSave(gameObject);

        if (Image_Pluss)
        {
            Image_Pluss.SetActive(false);
        }

        BuildingSystemManager.Instance.activeBuildingObject_Info.buildingObjectType_Active = buildingObjectType;
        BuildingSystemManager.Instance.activeBuildingObject_Info.buildingMaterial_Active = buildingMaterial;
        BuildingSystemManager.Instance.activeBuildingObject_Info.buildingBlockObjectName_Active = buildingBlockObjectName;
        BuildingSystemManager.Instance.activeBuildingObject_Info.furnitureObjectName_Active = furnitureObjectName;
        BuildingSystemManager.Instance.activeBuildingObject_Info.machineObjectName_Active = machineObjectName;

        if (buildingObjectType == BuildingObjectTypes.BuildingBlock)
        {
            BuildingDisplayManager.Instance.UpdateSelectedDisplay(BuildingSystemManager.Instance.GetBuildingObjectInfo(buildingBlockObjectName, buildingMaterial));
        }
        else if (buildingObjectType == BuildingObjectTypes.Furniture)
        {
            BuildingDisplayManager.Instance.UpdateSelectedDisplay(BuildingSystemManager.Instance.GetBuildingObjectInfo(furnitureObjectName));
        }
        else if (buildingObjectType == BuildingObjectTypes.Machine)
        {
            BuildingDisplayManager.Instance.UpdateSelectedDisplay(BuildingSystemManager.Instance.GetBuildingObjectInfo(machineObjectName));
        }
        else
        {
            BuildingDisplayManager.Instance.ResetDisplay();
        }

        //Remove CropPlot the correct way
        print("1. DestroyObject");
        if (BuildingSystemManager.Instance.ghostObject_Holding)
        {
            print("2. DestroyObject");
            if (BuildingSystemManager.Instance.ghostObject_Holding.GetComponent<CropPlot>())
            {
                print("3. DestroyObject");
                BuildingSystemManager.Instance.ghostObject_Holding.GetComponent<CropPlot>().DestroyThisCropPlotObject();
            }
        }

        BuildingDisplayManager.Instance.UpdateScreenBuildingRequirementDisplayInfo();
        BuildingSystemManager.Instance.SpawnNewSelectedBuildingObject();

        BuildingSystemManager.Instance.SaveData();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Instance.Play_Inventory_ItemHover_Clip();
    }
}
