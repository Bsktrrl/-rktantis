using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObject : MonoBehaviour
{
    [Header("General")]
    public bool canBePlaced;

    [Header("FurnitureType Type")]
    public FurnitureType furnitureType = FurnitureType.None;

    [Header("MachineType Type")]
    public MachineType machineType = MachineType.None;


    //--------------------


    private void Update()
    {
        if (canBePlaced)
        {
            gameObject.GetComponent<MeshRenderer>().material = BuildingManager.Instance.canPlace_Material;
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().material = BuildingManager.Instance.cannotPlace_Material;
        }
    }


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
