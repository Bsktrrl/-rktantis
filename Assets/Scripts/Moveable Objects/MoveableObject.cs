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
    public FurnitureType furnitureType = FurnitureType.None;
    public MachineType machineType = MachineType.None;

    [Header("Mesh")]
    public SkinnedMeshRenderer meshRenderer;


    //--------------------


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "BuildingBlock")
        {
            if (other.gameObject.GetComponent<BuildingBlock>().buidingBlock_Parent.GetComponent<BuildingBlock_Parent>().buildingType == BuildingType.Floor
                || other.gameObject.GetComponent<BuildingBlock>().buidingBlock_Parent.GetComponent<BuildingBlock_Parent>().buildingType == BuildingType.Floor_Triangle)
            {
                canBePlaced = true;
            }
            else
            {
                canBePlaced = false;
            }
        }
        else
        {
            canBePlaced = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        canBePlaced = false;
    }
}
