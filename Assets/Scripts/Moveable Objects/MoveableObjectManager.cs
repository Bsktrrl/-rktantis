using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveableObjectManager : Singleton<MoveableObjectManager>
{
    #region Variables
    [Header("moveableObject_SO")]
    public BuildingBlockObjects_SO buildingBlockObjects_SO;
    public MoveableObject_SO moveableObject_SO;
    public GameObject moveableObject_Parent;

    [Header("Main Type")]
    public BuildingObjectTypes moveableObjectType = BuildingObjectTypes.None;

    [Header("FurnitureObjectNames Type")]
    public FurnitureObjectNames furnitureType = FurnitureObjectNames.None;

    [Header("MachineObjectNames Type")]
    public MachineObjectNames machineType = MachineObjectNames.None;

    [Header("Building Type")]
    public BuildingBlockObjectNames buildingType_Selected = BuildingBlockObjectNames.None;
    public BuildingMaterial buildingMaterial_Selected = BuildingMaterial.None;

    [Header("ObjectToMove")]
    public GameObject objectToMove;

    [Header("Object Placed List")]
    public List<GameObject> placedMoveableWorldObjectsList = new List<GameObject>();
    public List<MoveableObject_ToSave> placedMoveableObjectsList_ToSave = new List<MoveableObject_ToSave>();

    public MoveableObjectSelected_ToSave moveableObjectSelected_ToSave = new MoveableObjectSelected_ToSave();
    #endregion


    //--------------------


    public void LoadData()
    {
        //Load placedMoveableObjectsList_ToSave
        #region
        placedMoveableObjectsList_ToSave.Clear();
        //placedMoveableObjectsList_ToSave = DataManager.Instance.placedMoveableObjectsList_StoreList;

        //Place MoveableObjects into the World
        for (int i = 0; i < placedMoveableObjectsList_ToSave.Count; i++)
        {
            //Find MoveableObject from _SO
            GameObject tempObject = GetObjectFromMoveableObject_SO(placedMoveableObjectsList_ToSave[i].moveableObjectType, placedMoveableObjectsList_ToSave[i].machineType, placedMoveableObjectsList_ToSave[i].furnitureType);
            if (tempObject)
            {
                //Add it to the list
                placedMoveableWorldObjectsList.Add(Instantiate(tempObject, placedMoveableObjectsList_ToSave[i].objectPos, placedMoveableObjectsList_ToSave[i].objectRot) as GameObject);
                placedMoveableWorldObjectsList[placedMoveableWorldObjectsList.Count - 1].transform.parent = moveableObject_Parent.transform;

                //If a chest is loaded
                if (placedMoveableObjectsList_ToSave[i].furnitureType == FurnitureObjectNames.Chest_Small
                    || placedMoveableObjectsList_ToSave[i].furnitureType == FurnitureObjectNames.Chest_Big)
                {
                    if (placedMoveableWorldObjectsList[placedMoveableWorldObjectsList.Count - 1].GetComponent<InteractableObject>())
                    {
                        placedMoveableWorldObjectsList[placedMoveableWorldObjectsList.Count - 1].GetComponent<InteractableObject>().inventoryIndex = placedMoveableObjectsList_ToSave[i].chestIndex;
                    }
                }
            }
        }
        #endregion

        //Load State for MoveableObject selected from BuildingHammer
        #region
        MoveableObjectSelected_ToSave temp = new MoveableObjectSelected_ToSave();
        //temp = DataManager.Instance.moveableObjectSelected_Store;
        moveableObjectType = temp.moveableObjectType;
        machineType = temp.machineType;
        furnitureType = temp.furnitureType;
        buildingType_Selected = temp.buildingType;
        buildingMaterial_Selected = temp.buildingMaterial;

        //Set BuildingHammer UpToDate
        if (EquippmentManager.Instance.toolHolderParent)
        {
            //if (EquippmentManager.Instance.toolHolderParent.GetComponentInChildren<BuildingHammer>())
            //{
            //    BuildingManager.Instance.SetNewSelectedBlock();
            //    //EquippmentManager.Instance.toolHolderParent.GetComponentInChildren<BuildingHammer>().SetNewSelectedBlock();
            //}
        }
        #endregion

    }
    public void SaveData()
    {
        print("placedMoveableObjectsList_ToSave SAVED");

        //Save all MoveableObjects placed into the world
        //DataManager.Instance.placedMoveableObjectsList_StoreList = placedMoveableObjectsList_ToSave;

        //Save State for MoveableObject selected from BuildingHammer
        MoveableObjectSelected_ToSave temp = new MoveableObjectSelected_ToSave();
        temp.moveableObjectType = moveableObjectType;
        temp.machineType = machineType;
        temp.furnitureType = furnitureType;
        temp.buildingType = buildingType_Selected;
        temp.buildingMaterial = buildingMaterial_Selected;

        //DataManager.Instance.moveableObjectSelected_Store = temp;
    }


    //--------------------


    public void ResetInfo()
    {
        moveableObjectType = BuildingObjectTypes.None;
        buildingType_Selected = BuildingBlockObjectNames.None;
        buildingMaterial_Selected = BuildingMaterial.None;
        machineType = MachineObjectNames.None;
        furnitureType = FurnitureObjectNames.None;
    }
    public void SetMachineInfo(MachineObjectNames _machineType)
    {
        moveableObjectType = BuildingObjectTypes.Machine;
        buildingType_Selected = BuildingBlockObjectNames.None;
        buildingMaterial_Selected = BuildingMaterial.None;
        machineType = _machineType;
        furnitureType = FurnitureObjectNames.None;
    }
    public void SetFurnitureInfo(FurnitureObjectNames _furnitureType)
    {
        moveableObjectType = BuildingObjectTypes.Furniture;
        buildingType_Selected = BuildingBlockObjectNames.None;
        buildingMaterial_Selected = BuildingMaterial.None;
        machineType = MachineObjectNames.None;
        furnitureType = _furnitureType;
    }
    public void SetBuildingBlockInfo(BuildingBlockObjectNames _buildingType_Selected, BuildingMaterial _buildingMaterial_Selected)
    {
        moveableObjectType = BuildingObjectTypes.BuildingBlock;
        buildingType_Selected = _buildingType_Selected;
        buildingMaterial_Selected = _buildingMaterial_Selected;
        machineType = MachineObjectNames.None;
        furnitureType = FurnitureObjectNames.None;
    }


    //--------------------


    public GameObject GetObjectFromMoveableObject_SO(BuildingObjectTypes moveableObjectType, MachineObjectNames machineType, FurnitureObjectNames furnitureType)
    {
        for (int i = 0; i < moveableObject_SO.moveableObjectList.Count; i++)
        {
            if (moveableObject_SO.moveableObjectList[i].moveableObjectType == moveableObjectType
                && moveableObject_SO.moveableObjectList[i].machineType == machineType
                && moveableObject_SO.moveableObjectList[i].furnitureType == furnitureType)
            {
                return moveableObject_SO.moveableObjectList[i].objectToMove;
            }
        }

        return null;
    }
    public MoveableObjectInfo GetObjectInfoFromMoveableObject_SO(MachineObjectNames machineType, FurnitureObjectNames furnitureType)
    {
        for (int i = 0; i < moveableObject_SO.moveableObjectList.Count; i++)
        {
            if (moveableObject_SO.moveableObjectList[i].machineType == machineType
                && moveableObject_SO.moveableObjectList[i].furnitureType == furnitureType)
            {
                return moveableObject_SO.moveableObjectList[i];
            }
        }

        return null;
    }


    //--------------------


    public GameObject GetBuildingBlock()
    {
        if (moveableObjectType == BuildingObjectTypes.BuildingBlock)
        {
            for (int i = 0; i < buildingBlockObjects_SO.buildingBlockObjectList.Count; i++)
            {
                if (buildingBlockObjects_SO.buildingBlockObjectList[i].BuildingType == buildingType_Selected
                    && buildingBlockObjects_SO.buildingBlockObjectList[i].buildingMaterial == buildingMaterial_Selected)
                {
                    return buildingBlockObjects_SO.buildingBlockObjectList[i].objectToMove;
                }
            }
        }

        return null;
    }
    public BuildingBlockObject GetBuildingBlock_SO()
    {
        if (moveableObjectType == BuildingObjectTypes.BuildingBlock)
        {
            for (int i = 0; i < buildingBlockObjects_SO.buildingBlockObjectList.Count; i++)
            {
                if (buildingBlockObjects_SO.buildingBlockObjectList[i].BuildingType == buildingType_Selected
                    && buildingBlockObjects_SO.buildingBlockObjectList[i].buildingMaterial == buildingMaterial_Selected)
                {
                    return buildingBlockObjects_SO.buildingBlockObjectList[i];
                }
            }
        }

        return null;
    }
    public GameObject GetMoveableObject()
    {
        //Machine
        if (moveableObjectType == BuildingObjectTypes.Machine)
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
        else if (moveableObjectType == BuildingObjectTypes.Furniture)
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
    public MoveableObjectInfo GetMoveableObjectInfo(MoveableObject moveableObject)
    {
        //Machine
        //if (moveableObject.machineType != MachineObjectNames.None)
        //{
        //    for (int i = 0; i < moveableObject_SO.moveableObjectList.Count; i++)
        //    {
        //        if (moveableObject_SO.moveableObjectList[i].machineType == moveableObject.machineType)
        //        {
        //            return moveableObject_SO.moveableObjectList[i];
        //        }
        //    }
        //}

        //Furniture
        //else if (moveableObject.furnitureType != FurnitureObjectNames.None)
        //{
        //    for (int i = 0; i < moveableObject_SO.moveableObjectList.Count; i++)
        //    {
        //        if (moveableObject_SO.moveableObjectList[i].furnitureType == moveableObject.furnitureType)
        //        {
        //            return moveableObject_SO.moveableObjectList[i];
        //        }
        //    }
        //}

        return null;
    }
    public MoveableObjectInfo GetMoveableObject_SO()
    {
        //Machine
        if (moveableObjectType == BuildingObjectTypes.Machine)
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
        else if (moveableObjectType == BuildingObjectTypes.Furniture)
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
            SoundManager.Instance.Play_Building_Place_MoveableObject_Clip();

            objectToMove.GetComponent<MoveableObject>().isSelectedForMovement = false;

            MoveableObjectInfo tempInfo = GetMoveableObject_SO();

            //Instantiate a MoveableObejct
            placedMoveableWorldObjectsList.Add(Instantiate(tempInfo.objectToMove, moveableObject.gameObject.transform.position, moveableObject.gameObject.transform.rotation) as GameObject);
            placedMoveableWorldObjectsList[placedMoveableWorldObjectsList.Count - 1].transform.SetParent(moveableObject_Parent.transform);

            //Add MoveableObjectList to save
            #region
            MoveableObject_ToSave tempToSave = new MoveableObject_ToSave();
            tempToSave.moveableObjectType = tempInfo.moveableObjectType;
            tempToSave.machineType = tempInfo.machineType;
            tempToSave.furnitureType = tempInfo.furnitureType;
            tempToSave.objectPos = placedMoveableWorldObjectsList[placedMoveableWorldObjectsList.Count - 1].transform.position;
            tempToSave.objectRot = placedMoveableWorldObjectsList[placedMoveableWorldObjectsList.Count - 1].transform.rotation;
            
            //If a small chest, update inventory info
            if (tempToSave.furnitureType == FurnitureObjectNames.Chest_Small)
            {
                InventoryManager.Instance.AddInventory(placedMoveableWorldObjectsList[placedMoveableWorldObjectsList.Count - 1].GetComponent<InteractableObject>(), InventoryManager.Instance.smallChest_Size);

                tempToSave.chestIndex = InventoryManager.Instance.inventories.Count - 1;
            }

            //If a big chest, update inventory info
            else if (tempToSave.furnitureType == FurnitureObjectNames.Chest_Big)
            {
                InventoryManager.Instance.AddInventory(placedMoveableWorldObjectsList[placedMoveableWorldObjectsList.Count - 1].GetComponent<InteractableObject>(), InventoryManager.Instance.bigChest_Size);

                tempToSave.chestIndex = InventoryManager.Instance.inventories.Count - 1;
            }

            //Add all info to the list
            placedMoveableObjectsList_ToSave.Add(tempToSave);
            #endregion

            print("2. WorldObject: " + placedMoveableWorldObjectsList.Count + " | SaveObject: " + placedMoveableObjectsList_ToSave.Count);

            SaveData();

            //Remove Items from inventory
            if (tempInfo.craftingRequirements != null)
            {
                for (int i = 0; i < tempInfo.craftingRequirements.Count; i++)
                {
                    for (int k = 0; k < tempInfo.craftingRequirements[i].amount; k++)
                    {
                        InventoryManager.Instance.RemoveItemFromInventory(0, tempInfo.craftingRequirements[i].itemName, -1, false);
                    }
                }
            }

            //Update the Hotbar
            //InventoryManager.Instance.CheckHotbarItemInInventory();

            InventoryManager.Instance.RemoveInventoriesUI();
        }
        else
        {
            print("1. Don't Place MoveableObject");
            SoundManager.Instance.Play_Building_CannotPlaceBlock_Clip();
        }
    }
}

[Serializable]
public class MoveableObject_ToSave
{
    public BuildingObjectTypes moveableObjectType = BuildingObjectTypes.None;
    public FurnitureObjectNames furnitureType = FurnitureObjectNames.None;
    public MachineObjectNames machineType = MachineObjectNames.None;

    public Vector3 objectPos = new Vector3();
    public Quaternion objectRot = new Quaternion();

    public int chestIndex = 0;
}

[Serializable]
public class MoveableObjectSelected_ToSave
{
    public BuildingObjectTypes moveableObjectType = BuildingObjectTypes.None;
    public MachineObjectNames machineType = MachineObjectNames.None;
    public FurnitureObjectNames furnitureType = FurnitureObjectNames.None;

    public BuildingBlockObjectNames buildingType = BuildingBlockObjectNames.None;
    public BuildingMaterial buildingMaterial = BuildingMaterial.None;
}