using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveableObjectManager : Singleton<MoveableObjectManager>
{
    [Header("moveableObject_SO")]
    public MoveableObject_SO moveableObject_SO;
    public GameObject moveableObject_Parent;

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

    [Header("Object Placed List")]
    public List<GameObject> placedMoveableObjectsList = new List<GameObject>();


    //--------------------


    public GameObject GetMoveableObject()
    {
        //Machine
        if (moveableObjectType == MoveableObjectType.Machine)
        {
            for (int i = 0; i < moveableObject_SO.moveableObjectList.Count; i++)
            {
                if (moveableObject_SO.moveableObjectList[i].moveableObjectType == moveableObjectType
                    && moveableObject_SO.moveableObjectList[i].machineType == machineType)
                {
                    return moveableObject_SO.moveableObjectList[i].objectToMove;
                }
            }
        }

        //Furniture
        else if (moveableObjectType == MoveableObjectType.Furniture)
        {
            for (int i = 0; i < moveableObject_SO.moveableObjectList.Count; i++)
            {
                if (moveableObject_SO.moveableObjectList[i].moveableObjectType == moveableObjectType
                    && moveableObject_SO.moveableObjectList[i].furnitureType == furnitureType)
                {
                    return moveableObject_SO.moveableObjectList[i].objectToMove;
                }
            }
        }

        return null;
    }
    public MoveableObjectInfo GetMoveableObject_SO()
    {
        //Machine
        if (moveableObjectType == MoveableObjectType.Machine)
        {
            for (int i = 0; i < moveableObject_SO.moveableObjectList.Count; i++)
            {
                if (moveableObject_SO.moveableObjectList[i].moveableObjectType == moveableObjectType
                    && moveableObject_SO.moveableObjectList[i].machineType == machineType)
                {
                    return moveableObject_SO.moveableObjectList[i];
                }
            }
        }

        //Furniture
        else if (moveableObjectType == MoveableObjectType.Furniture)
        {
            for (int i = 0; i < moveableObject_SO.moveableObjectList.Count; i++)
            {
                if (moveableObject_SO.moveableObjectList[i].moveableObjectType == moveableObjectType
                    && moveableObject_SO.moveableObjectList[i].furnitureType == furnitureType)
                {
                    return moveableObject_SO.moveableObjectList[i];
                }
            }
        }

        return null;
    }


    //--------------------

    public void PlaceObjectToMove(MoveableObject moveableObject)
    {
        if (moveableObject.canBePlaced && moveableObject.isSelectedForMovement && moveableObject.enoughItemsToBuild)
        {
            print("0. Place MoveableObject");
            SoundManager.instance.PlayMoveableObject_Placed();

            objectToMove.GetComponent<MoveableObject>().isSelectedForMovement = false;

            placedMoveableObjectsList.Add(Instantiate(GetMoveableObject_SO().objectToMove, moveableObject.gameObject.transform.position, moveableObject.gameObject.transform.rotation) as GameObject);
            placedMoveableObjectsList[placedMoveableObjectsList.Count - 1].transform.SetParent(moveableObject_Parent.transform);

            //Remove Items from inventory
            MoveableObjectInfo tempInfo = GetMoveableObject_SO();
            if (tempInfo.craftingRequirements != null)
            {
                for (int i = 0; i < tempInfo.craftingRequirements.Count; i++)
                {
                    for (int k = 0; k < tempInfo.craftingRequirements[i].amount; k++)
                    {
                        InventoryManager.Instance.RemoveItemFromInventory(0, tempInfo.craftingRequirements[i].itemName, false);
                    }
                }
            }

            //Update the Hotbar
            InventoryManager.Instance.CheckHotbarItemInInventory();
            InventoryManager.Instance.RemoveInventoriesUI();
        }
        else
        {
            print("1. Don't Place MoveableObject");
            SoundManager.instance.PlaybuildingBlock_CannotPlaceBlock();
        }
    }
}
