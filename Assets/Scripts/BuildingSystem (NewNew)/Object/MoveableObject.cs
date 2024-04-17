using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObject : MonoBehaviour
{
    [Header("General")]
    [HideInInspector] public bool canBePlaced;
    [HideInInspector] public bool isSelectedForMovement;
    [HideInInspector] public bool enoughItemsToBuild;

    [Header("MoveableObject Type")]
    public BuildingObjectTypes buildingObjectType;
    public BuildingMaterial buildingMaterial;
    [Space(5)]
    public BuildingBlockObjectNames buildingBlockObjectName;
    public FurnitureObjectNames furnitureObjectName;
    public MachineObjectNames machineObjectName;

    [Header("Mesh")]
    public SkinnedMeshRenderer meshRenderer;


    //--------------------


    private void OnTriggerEnter(Collider other)
    {
        //if (other.tag == "BuildingBlock")
        //{
        //    if (other.gameObject.GetComponent<BuildingBlock>().buidingBlock_Parent.GetComponent<BuildingBlock_Parent>().buildingType == BuildingBlockObjectNames.Floor_Square
        //        || other.gameObject.GetComponent<BuildingBlock>().buidingBlock_Parent.GetComponent<BuildingBlock_Parent>().buildingType == BuildingBlockObjectNames.Floor_Triangle)
        //    {
        //        canBePlaced = true;
        //    }
        //    else
        //    {
        //        canBePlaced = false;
        //    }
        //}
        //else
        //{
        //    canBePlaced = false;
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        canBePlaced = false;
    }
}
