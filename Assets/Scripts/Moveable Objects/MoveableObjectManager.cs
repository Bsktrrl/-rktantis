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
    public List<GameObject> moveableObjectList = new List<GameObject>();


    //--------------------


    private void Update()
    {
        if (objectToMove)
        {
            UpdateObjectToMovePosition();
        }
    }


    //--------------------


    public GameObject GetMoveableObject(MoveableObjectType moveableObjectType)
    {
        //Machine
        if (moveableObjectType == MoveableObjectType.Machine)
        {
            for (int i = 0; i < moveableObjectList.Count; i++)
            {
                if (moveableObjectList[i].GetComponent<MoveableObject>().machineType == machineType)
                {
                    return moveableObjectList[i];
                }
            }
        }

        //Furniture
        else if (moveableObjectType == MoveableObjectType.Furniture)
        {
            for (int i = 0; i < moveableObjectList.Count; i++)
            {
                if (moveableObjectList[i].GetComponent<MoveableObject>().furnitureType == furnitureType)
                {
                    return moveableObjectList[i];
                }
            }
        }

        return null;
    }


    //--------------------


    public void UpdateObjectToMovePosition()
    {

    }

    public void PlaceObjectToMove()
    {

    }
}
