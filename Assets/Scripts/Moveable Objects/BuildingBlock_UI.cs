using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingBlock_UI : MonoBehaviour, IPointerEnterHandler
{
    public static Action objectClicked;

    [Header("Main Type")]
    public BuildingObjectTypes objectType;

    [Header("Building Type")]
    public BuildingBlockObjectNames buildingType;
    public BuildingMaterial buildingMaterial;

    [Header("Machine Type")]
    public MachineObjectNames machineType;

    [Header("FurnitureObjectNames Type")]
    public FurnitureObjectNames furnitureType;

    [Header("Parent")]
    public GameObject parent;


    //--------------------


    private void Start()
    {
        objectClicked += OtherButtonClicked;
    }


    //--------------------


    void OtherButtonClicked()
    {
        //Set Frame Blue
        GetComponent<Image>().sprite = TabletManager.Instance.squareButton_Active;
    }
    public void OnObjectClicked()
    {
        objectClicked?.Invoke();

        //Set Frame Orange
        GetComponent<Image>().sprite = TabletManager.Instance.squareButton_Passive;

        MoveableObjectManager.Instance.moveableObjectType = objectType;
        MoveableObjectManager.Instance.machineType = machineType;
        MoveableObjectManager.Instance.furnitureType = furnitureType;

        MoveableObjectManager.Instance.buildingType_Selected = buildingType;
        MoveableObjectManager.Instance.buildingMaterial_Selected = buildingMaterial;

        if (parent.GetComponent<Image>())
        {
            BuildingSystemMenu.Instance.SetSelectedImage(parent.GetComponent<Image>().sprite);
        }

        //Hide Panel if Object is Empty
        if (objectType == BuildingObjectTypes.None)
        {
            BuildingSystemMenu.Instance.buildingRequirement_Parent.SetActive(false);
        }
        else
        {
            BuildingSystemMenu.Instance.buildingRequirement_Parent.SetActive(true);
        }

        //If selected Object is Empty
        if (MoveableObjectManager.Instance.moveableObjectType == BuildingObjectTypes.None)
        {
            //Update "Free Block" if Hammer is selected
            //BuildingManager.Instance.SetNewSelectedBlock();
            //if (EquippmentManager.Instance.toolHolderParent.GetComponentInChildren<BuildingHammer>() != null)
            //{
            //    EquippmentManager.Instance.toolHolderParent.GetComponentInChildren<BuildingHammer>().SetNewSelectedBlock();
            //}
        }

        //If selected Object is a BuildingBlock
        else if (MoveableObjectManager.Instance.moveableObjectType == BuildingObjectTypes.BuildingBlock)
        {
            //BuildingBlock_Parent tempParent = BuildingManager.Instance.GetBuildingBlock(buildingType, buildingMaterial);
            //if (tempParent != null)
            //{
            //    //Set requirements for both BuildingMenu and on main screen
            //    //BuildingManager.Instance.SetBuildingRequirements(tempParent, BuildingSystemMenu.Instance.buildingRequirement_Parent);
            //    //BuildingManager.Instance.SetBuildingRequirements(BuildingManager.Instance.GetBuildingBlock(MoveableObjectManager.Instance.buildingType_Selected, MoveableObjectManager.Instance.buildingMaterial_Selected), BuildingManager.Instance.buildingRequirement_Parent);
            //}

            //Update "Free Block" if Hammer is selected
            //BuildingManager.Instance.SetNewSelectedBlock();
            //if (EquippmentManager.Instance.toolHolderParent.GetComponentInChildren<BuildingHammer>() != null)
            //{
            //    EquippmentManager.Instance.toolHolderParent.GetComponentInChildren<BuildingHammer>().SetNewSelectedBlock();
            //}
        }

        //If selected Object is a Machine
        else if (MoveableObjectManager.Instance.moveableObjectType == BuildingObjectTypes.Machine)
        {
            //BuildingManager.Instance.SetNewSelectedBlock();

            //Set requirements for both BuildingMenu and on main screen
            MoveableObjectInfo tempObject = MoveableObjectManager.Instance.GetMoveableObject_SO();
            //BuildingManager.Instance.SetBuildingRequirements(tempObject, BuildingSystemMenu.Instance.buildingRequirement_Parent);
            //BuildingManager.Instance.SetBuildingRequirements(tempObject, BuildingManager.Instance.buildingRequirement_Parent);

            //if (EquippmentManager.Instance.toolHolderParent.GetComponentInChildren<BuildingHammer>() != null)
            //{
            //    EquippmentManager.Instance.toolHolderParent.GetComponentInChildren<BuildingHammer>().SetNewSelectedBlock();

            //    //Set requirements for both BuildingMenu and on main screen
            //    MoveableObjectInfo tempObject = MoveableObjectManager.Instance.GetMoveableObject_SO();
            //    BuildingManager.Instance.SetBuildingRequirements(tempObject, BuildingSystemMenu.Instance.buildingRequirement_Parent);
            //    BuildingManager.Instance.SetBuildingRequirements(tempObject, BuildingManager.Instance.buildingRequirement_Parent);
            //}
        }

        //If selected Object is a Furniture
        else if (MoveableObjectManager.Instance.moveableObjectType == BuildingObjectTypes.Furniture)
        {
            //BuildingManager.Instance.SetNewSelectedBlock();

            //Set requirements for both BuildingMenu and on main screen
            MoveableObjectInfo tempObject = MoveableObjectManager.Instance.GetMoveableObject_SO();
            //BuildingManager.Instance.SetBuildingRequirements(tempObject, BuildingSystemMenu.Instance.buildingRequirement_Parent);
            //BuildingManager.Instance.SetBuildingRequirements(tempObject, BuildingManager.Instance.buildingRequirement_Parent);

            //if (EquippmentManager.Instance.toolHolderParent.GetComponentInChildren<BuildingHammer>() != null)
            //{
            //    EquippmentManager.Instance.toolHolderParent.GetComponentInChildren<BuildingHammer>().SetNewSelectedBlock();

            //    //Set requirements for both BuildingMenu and on main screen
            //    MoveableObjectInfo tempObject = MoveableObjectManager.Instance.GetMoveableObject_SO();
            //    BuildingManager.Instance.SetBuildingRequirements(tempObject, BuildingSystemMenu.Instance.buildingRequirement_Parent);
            //    BuildingManager.Instance.SetBuildingRequirements(tempObject, BuildingManager.Instance.buildingRequirement_Parent);
            //}
        }

        //BuildingManager.Instance.SaveData();
        MoveableObjectManager.Instance.SaveData();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Instance.Play_Inventory_ItemHover_Clip();
    }
}
