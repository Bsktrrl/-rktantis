using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveableObjectManager : Singleton<MoveableObjectManager>
{
    [Header("Main Type")]
    public MoveableObjectType moveableObjectType = MoveableObjectType.None;

    [Header("FurnitureType Type")]
    public FurnitureType furnitureType = FurnitureType.None;

    [Header("MachineType Type")]
    public MachineType machineType = MachineType.None;

    [Header("Building Type")]
    public BuildingType buildingType_Selected = BuildingType.None;
    public BuildingMaterial buildingMaterial_Selected = BuildingMaterial.None;

    [Header("ObjectToMove")]
    public GameObject objectToMove;

    [Header("MoveableObjectList")]
    public List<GameObject> moveableObject_Machine_List = new List<GameObject>();
    public List<GameObject> moveableObject_Furniture_List = new List<GameObject>();


    //--------------------


    public GameObject GetMoveableObject(MoveableObjectType moveableObjectType)
    {
        //Machine
        if (moveableObjectType == MoveableObjectType.Machine)
        {
            for (int i = 0; i < moveableObject_Machine_List.Count; i++)
            {
                if (moveableObject_Machine_List[i].GetComponent<MoveableObject>().machineType == machineType)
                {
                    return moveableObject_Machine_List[i];
                }
            }
        }

        //Furniture
        else if (moveableObjectType == MoveableObjectType.Furniture)
        {
            for (int i = 0; i < moveableObject_Furniture_List.Count; i++)
            {
                if (moveableObject_Furniture_List[i].GetComponent<MoveableObject>().furnitureType == furnitureType)
                {
                    return moveableObject_Furniture_List[i];
                }
            }
        }

        return null;
    }


    //--------------------


    

    public void PlaceObjectToMove()
    {
        objectToMove.GetComponent<MoveableObject>().isSelectedForMovement = false;
    }
}
