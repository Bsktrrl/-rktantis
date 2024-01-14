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
            BuildingBlock_Parent tempParent = BuildingManager.instance.GetBuildingBlock(buildingType, buildingMaterial);
            if (tempParent != null)
            {
                //Set requirements for both BuildingMenu and on main screen
                BuildingManager.instance.SetBuildingRequirements(tempParent, BuildingSystemMenu.instance.buildingRequirement_Parent);
                BuildingManager.instance.SetBuildingRequirements(BuildingManager.instance.GetBuildingBlock(MoveableObjectManager.Instance.buildingType_Selected, MoveableObjectManager.Instance.buildingMaterial_Selected), BuildingManager.instance.buildingRequirement_Parent);
            }

            //Update "Free Block" if Hammer is selected
            if (EquippmentManager.instance.toolHolderParent.GetComponentInChildren<BuildingHammer>() != null)
            {
                EquippmentManager.instance.toolHolderParent.GetComponentInChildren<BuildingHammer>().SetNewSelectedBlock();

                print("2000. New Selected Block Set: Type: " + buildingType + " | Material: " + buildingMaterial);
            }
        }

        //If selected Object is a Machine
        else if (MoveableObjectManager.Instance.moveableObjectType == MoveableObjectType.Machine)
        {
            if (EquippmentManager.instance.toolHolderParent.GetComponentInChildren<BuildingHammer>() != null)
            {
                EquippmentManager.instance.toolHolderParent.GetComponentInChildren<BuildingHammer>().SetNewSelectedBlock();

                print("2000. Selected Object is a Machine");
            }
        }

        //If selected Object is a Furniture
        else if (MoveableObjectManager.Instance.moveableObjectType == MoveableObjectType.Furniture)
        {
            if (EquippmentManager.instance.toolHolderParent.GetComponentInChildren<BuildingHammer>() != null)
            {
                EquippmentManager.instance.toolHolderParent.GetComponentInChildren<BuildingHammer>().SetNewSelectedBlock();

                print("2000. Selected Object is a Furniture");
            }
        }


        BuildingManager.instance.SaveData();
    }
}
