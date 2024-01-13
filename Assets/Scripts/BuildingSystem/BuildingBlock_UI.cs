using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuildingBlock_UI : MonoBehaviour, IPointerEnterHandler
{
    [Header("Main Type")]
    public MoveableObjectType objectType;

    [Header("Building Type")]
    public BuildingType buildingType;
    public BuildingMaterial buildingMaterial;

    [Header("Machine Type")]
    public MachineType machineType;

    [Header("FurnitureType Type")]
    public FurnitureType furnitureType;


    public void OnPointerEnter(PointerEventData eventData)
    {
        BuildingManager.instance.buildingType_Selected = buildingType;
        BuildingManager.instance.buildingMaterial_Selected = buildingMaterial;

        BuildingSystemMenu.instance.SetSelectedImage(gameObject.GetComponent<Image>().sprite);

        BuildingBlock_Parent tempParent = BuildingManager.instance.GetBuildingBlock(buildingType, buildingMaterial);
        if (tempParent != null)
        {
            //Set requirements for both BuildingMenu and on main screen
            BuildingManager.instance.SetBuildingRequirements(tempParent, BuildingSystemMenu.instance.buildingRequirement_Parent);
            BuildingManager.instance.SetBuildingRequirements(BuildingManager.instance.GetBuildingBlock(BuildingManager.instance.buildingType_Selected, BuildingManager.instance.buildingMaterial_Selected), BuildingManager.instance.buildingRequirement_Parent);
        }

        //Update "Free Block" if Hammer is selected
        if (EquippmentManager.instance.toolHolderParent.GetComponentInChildren<BuildingHammer>() != null)
        {
            EquippmentManager.instance.toolHolderParent.GetComponentInChildren<BuildingHammer>().SetNewSelectedBlock();

            print("200. New Selected Block Set: Type: " + buildingType + " | Material: " + buildingMaterial);
        }

        BuildingManager.instance.SaveData();
    }
}
