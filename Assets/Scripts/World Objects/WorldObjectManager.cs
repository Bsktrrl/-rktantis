using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldObjectManager : Singleton<WorldObjectManager>
{
    [Header("WorldObjects Parent")]
    [SerializeField] GameObject worldObjectParent;

    [Header("WorldObjects To Be Saved")]
    public List<GameObject> worldObjectList = new List<GameObject>();
    public List<WorldObject> worldObjectList_ToSave = new List<WorldObject>();

    [Header("WorldObjects to be placed into the world")]
    [SerializeField] GameObject chestPrefab;

    float itemUpdateTimer;
    float itemPos_Y;
    float underTerrainPos_Y;


    //--------------------


    private void Update()
    {
        if (!DataManager.Instance.hasLoaded) { return; }
        if (PauseGameManager.Instance.GetPause()) { return; }
        if (MainManager.Instance.gameStates == GameStates.GameOver) { return; }

        UpdateItemUnderTerrain();
    }


    //--------------------


    public void LoadData()
    {
        //Load all Objects that's not MoveableObjects
        #region 
        //Safty reset the list
        for (int i = 0; i < worldObjectList.Count; i++)
        {
            Destroy(worldObjectList[i]);
        }
        worldObjectList.Clear();

        //Insert saved info to list
        worldObjectList_ToSave.Clear();
        worldObjectList_ToSave = DataManager.Instance.worldObject_StoreList;

        //Instantiate Objects into the world
        for (int i = 0; i < worldObjectList_ToSave.Count; i++)
        {
            worldObjectList.Add(Instantiate(GetSavedObject(worldObjectList_ToSave[i]), worldObjectList_ToSave[i].objectPosition, worldObjectList_ToSave[i].objectRotation) as GameObject);
            worldObjectList[worldObjectList.Count - 1].transform.parent = worldObjectParent.transform;

            //If Object is a Pickup
            if (worldObjectList[worldObjectList.Count - 1].GetComponent<InteractableObject>().interactableType == InteracteableType.Item)
            {
                //Activate Gravity
                worldObjectList[worldObjectList.Count - 1].GetComponent<Rigidbody>().isKinematic = false;
                worldObjectList[worldObjectList.Count - 1].GetComponent<Rigidbody>().useGravity = true;

                //Set Durability
                worldObjectList[worldObjectList.Count - 1].GetComponent<InteractableObject>().durability_Current = worldObjectList_ToSave[i].durability;
            }
        }
        #endregion

        //Load all MoveableObjects
        BuildObjectsFromLoad();
    }
    void SaveData()
    {
        DataManager.Instance.worldObject_StoreList = worldObjectList_ToSave;
    }


    //--------------------


    public void WorldObject_SaveState_AddObjectToWorld(Items itemName)
    {
        WorldObject tempObject = new WorldObject();

        tempObject.objectName = itemName;
        tempObject.objectPosition = worldObjectList[worldObjectList.Count - 1].gameObject.transform.position;
        tempObject.objectRotation = worldObjectList[worldObjectList.Count - 1].gameObject.transform.rotation;
        if (worldObjectList[worldObjectList.Count - 1].GetComponent<InteractableObject>())
        {
            tempObject.durability = worldObjectList[worldObjectList.Count - 1].GetComponent<InteractableObject>().durability_Current;
        }
       
        worldObjectList_ToSave.Add(tempObject);

        SaveData();
    }
    public void WorldObject_SaveState_RemoveObjectFromWorld(GameObject objectToRemove)
    {
        for (int i = 0; i < worldObjectList.Count; i++)
        {
            if (worldObjectList[i] == objectToRemove)
            {
                worldObjectList.RemoveAt(i);
                worldObjectList_ToSave.RemoveAt(i);

                break;
            }
        }

        SaveData();
    }


    //--------------------


    public void SaveWorldObjectPositions()
    {
        //print("worldObjectList_ToSave: " + worldObjectList_ToSave.Count + " | worldObjectList.Count: " + worldObjectList.Count);

        if (worldObjectList_ToSave.Count == worldObjectList.Count)
        {
            for(int i = 0; i < worldObjectList.Count; i++)
            {
                worldObjectList_ToSave[i].objectPosition = worldObjectList[i].transform.position;
                worldObjectList_ToSave[i].objectRotation = worldObjectList[i].transform.rotation;
            }
        }

        SaveData();
    }


    //--------------------


    public void AddChestIntoWorld(int size)
    {
        ////Instatiate worldObjectsList
        //worldObjectsList.Add(Instantiate(chestPrefab) as GameObject);
        //worldObjectsList[worldObjectsList.Count - 1].transform.SetParent(worldObjectParent.transform);

        //worldObjectsList[worldObjectsList.Count - 1].GetComponent<InventoryObject>().SetIndex(worldObjectsList.Count - 1);
        //worldObjectsList[worldObjectsList.Count - 1].transform.position = MainManager.instance.player.transform.position + new Vector3(0, -1, 2);

        //InventoryManager.instance.AddInventory(worldObjectsList[worldObjectsList.Count - 1].GetComponent<InventoryObject>() , size);

        ////Instatiate worldObjectsInfoList
        //ObjectClassSavingVariables objToAdd = new ObjectClassSavingVariables();
        //worldObjectsInfoList.Add(objToAdd);

        //StoreObjectToSave(worldObjectsList.Count - 1);
        //Save();
    }
    //public void DeleteObjectFromTheWorld(ObjectType obj, int worldObjectIndex, int inventoryIndex)
    //{
    //    ////Remove selected object from both lists
    //    //worldObjectsList.RemoveAt(worldObjectIndex);
    //    //worldObjectsInfoList.RemoveAt(worldObjectIndex);

    //    ////If object is an Inventory also execute this
    //    //if (obj == ObjectType.Inventory && inventoryIndex > 1) //>1 because of Main inventory (0) and SelectPanel (1)
    //    //{
    //    //    InventoryManager.instance.RemoveInventory(inventoryIndex);

    //    //    for (int i = worldObjectIndex; i < worldObjectsList.Count; i++)
    //    //    {
    //    //        worldObjectsList[i].GetComponent<InventoryObject>().inventoryIndex -= 1;
    //    //        worldObjectsInfoList[i].inventoryIndex -= 1;
    //    //    }
    //    //}

    //    ////Shift the index of all GameObjects -1 to match the new list
    //    //for (int i = worldObjectIndex; i < worldObjectsList.Count; i++)
    //    //{
    //    //    worldObjectsList[i].GetComponent<InventoryObject>().objectIndex -= 1;
    //    //    worldObjectsInfoList[i].worldObjectIndex -= 1;
    //    //}

    //    //Save();
    //}


    //--------------------

    void StoreObjectToSave(int i)
    {
        //worldObjectsInfoList[i].objectType = ObjectType.Inventory;
        //worldObjectsInfoList[i].inventoryIndex = worldObjectsList[worldObjectsList.Count - 1].GetComponent<InventoryObject>().inventoryIndex;
        //worldObjectsInfoList[i].worldObjectIndex = worldObjectsList[worldObjectsList.Count - 1].GetComponent<InventoryObject>().objectIndex;
        //worldObjectsInfoList[i].position = worldObjectsList[i].transform.position;
    }
    void BuildObjectsFromLoad()
    {
        //print("Building InventoryObjectsList.Count = " + worldObjectsInfoList.Count);

        //worldObjectsList.Clear();

        //for (int i = 0; i < worldObjectsInfoList.Count; i++)
        //{
        //    //If Object loaded is a Chest
        //    if (worldObjectsInfoList[i].objectType == ObjectType.Inventory)
        //    {
        //        worldObjectsList.Add(Instantiate(chestPrefab) as GameObject);
        //        worldObjectsList[worldObjectsList.Count - 1].transform.SetParent(worldObjectParent.transform);

        //        //Insert saved info back to the InventoryObject
        //        InventoryObject tempObj = worldObjectsList[worldObjectsList.Count - 1].GetComponent<InventoryObject>();
        //        tempObj.objectIndex = worldObjectsInfoList[i].worldObjectIndex;
        //        tempObj.inventoryIndex = worldObjectsInfoList[i].inventoryIndex;
        //        tempObj.transform.position = worldObjectsInfoList[i].position;
        //    }
        //}
    }


    //--------------------


    void UpdateItemUnderTerrain()
    {
        //Only trigger if item is dropped
        if (InventoryManager.Instance.itemDropped)
        {
            itemUpdateTimer += Time.deltaTime;

            if (itemUpdateTimer > 5)
            {
                InventoryManager.Instance.itemDropped = false;
                itemUpdateTimer = 0;

                //Transform ItemPos
                for (int i = 0; i < worldObjectList.Count; i++)
                {
                    if (worldObjectList[i].transform.position.y < -100)
                    {
                        worldObjectList[i].transform.SetPositionAndRotation(new Vector3(worldObjectList[i].transform.position.x, InventoryManager.Instance.currentHandDropPoint_Y, worldObjectList[i].transform.position.z), worldObjectList[i].transform.rotation);
                    }
                }
            }
        }
    }


    //--------------------


    GameObject GetSavedObject(WorldObject worldObj)
    {
        if (worldObj.objectName != Items.None)
        {
            return MainManager.Instance.GetItem(worldObj.objectName).worldObjectPrefab;
        }
        else
        {
            return MainManager.Instance.GetItem(Items.None).worldObjectPrefab;
        }
    }
}

[Serializable]
public class WorldObject
{
    [Header("General")]
    public Items objectName;

    [Header("Position")]
    public Vector3 objectPosition;
    public Quaternion objectRotation;

    [Header("Durability")]
    public int durability;
}