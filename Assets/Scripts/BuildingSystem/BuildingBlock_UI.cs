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
        MoveableObjectManager.Instance.moveableObjectType = objectType;
        MoveableObjectManager.Instance.machineType = machineType;
        MoveableObjectManager.Instance.furnitureType = furnitureType;

        MoveableObjectManager.Instance.buildingType_Selected = buildingType;
        MoveableObjectManager.Instance.buildingMaterial_Selected = buildingMaterial;

        BuildingSystemMenu.instance.SetSelectedImage(gameObject.GetComponent<Image>().sprite);

        //If selected Object is a BuildingBlock
        if (MoveableObjectManager.Instance.moveableObjectType == MoveableObjectType.BuildingBlock)
        {
            BuildingBlock_Parent tempParent = BuildingManager.Instance.GetBuildingBlock(buildingType, buildingMaterial);
            if (tempParent != null)
            {
                //Set requirements for both BuildingMenu and on main screen
                BuildingManager.Instance.SetBuildingRequirements(tempParent, BuildingSystemMenu.instance.buildingRequirement_Parent);
                BuildingManager.Instance.SetBuildingRequirements(BuildingManager.Instance.GetBuildingBlock(MoveableObjectManager.Instance.buildingType_Selected, MoveableObjectManager.Instance.buildingMaterial_Selected), BuildingManager.Instance.buildingRequirement_Parent);
            }

            //Update "Free Block" if Hammer is selected
            if (EquippmentManager.instance.toolHolderParent.GetComponentInChildren<BuildingHammer>() != null)
            {
                EquippmentManager.instance.toolHolderParent.GetComponentInChildren<BuildingHammer>().SetNewSelectedBlock();

                //print("2000. New Selected Block Set: Type: " + buildingType + " | Material: " + buildingMaterial);
            }
        }

        //If selected Object is a Machine
        else if (MoveableObjectManager.Instance.moveableObjectType == MoveableObjectType.Machine)
        {
            if (EquippmentManager.instance.toolHolderParent.GetComponentInChildren<BuildingHammer>() != null)
            {
                EquippmentManager.instance.toolHolderParent.GetComponentInChildren<BuildingHammer>().SetNewSelectedBlock();

                //Set requirements for both BuildingMenu and on main screen
                MoveableObjectInfo tempObject = MoveableObjectManager.Instance.GetMoveableObject_SO();
                BuildingManager.Instance.SetBuildingRequirements(tempObject, BuildingSystemMenu.instance.buildingRequirement_Parent);
                BuildingManager.Instance.SetBuildingRequirements(tempObject, BuildingManager.Instance.buildingRequirement_Parent);

            }
        }

        //If selected Object is a Furniture
        else if (MoveableObjectManager.Instance.moveableObjectType == MoveableObjectType.Furniture)
        {
            if (EquippmentManager.instance.toolHolderParent.GetComponentInChildren<BuildingHammer>() != null)
            {
                EquippmentManager.instance.toolHolderParent.GetComponentInChildren<BuildingHammer>().SetNewSelectedBlock();

                //Set requirements for both BuildingMenu and on main screen
                MoveableObjectInfo tempObject = MoveableObjectManager.Instance.GetMoveableObject_SO();
                BuildingManager.Instance.SetBuildingRequirements(tempObject, BuildingSystemMenu.instance.buildingRequirement_Parent);
                BuildingManager.Instance.SetBuildingRequirements(tempObject, BuildingManager.Instance.buildingRequirement_Parent);
            }
        }

        BuildingManager.Instance.SaveData();
    }
}
