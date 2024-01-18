using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class WorldObjectManager : Singleton<WorldObjectManager>
{
    //public float objectColliderRadius = 5;

    [Header("WorldObjects Parent")]
    [SerializeField] GameObject worldObjectParent;

    [Header("WorldObjects To Be Saved")]
    [SerializeField] GameObject chestPrefab;
    public List<GameObject> worldObjectsList = new List<GameObject>();
    public List<ObjectClassSavingVariables> worldObjectsInfoList = new List<ObjectClassSavingVariables>();


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
    //    //Remove selected object from both lists
    //    worldObjectsList.RemoveAt(worldObjectIndex);
    //    worldObjectsInfoList.RemoveAt(worldObjectIndex);

    //    //If object is an Inventory also execute this
    //    if (obj == ObjectType.Inventory && inventoryIndex > 1) //>1 because of Main inventory (0) and SelectPanel (1)
    //    {
    //        InventoryManager.instance.RemoveInventory(inventoryIndex);

    //        for (int i = worldObjectIndex; i < worldObjectsList.Count; i++)
    //        {
    //            worldObjectsList[i].GetComponent<InventoryObject>().inventoryIndex -= 1;
    //            worldObjectsInfoList[i].inventoryIndex -= 1;
    //        }
    //    }

    //    //Shift the index of all GameObjects -1 to match the new list
    //    for (int i = worldObjectIndex; i < worldObjectsList.Count; i++)
    //    {
    //        worldObjectsList[i].GetComponent<InventoryObject>().objectIndex -= 1;
    //        worldObjectsInfoList[i].worldObjectIndex -= 1;
    //    }

    //    Save();
    //}


    //--------------------


    void SaveData()
    {
        //DataManager.instance.worldObjects_StoreList = worldObjectsInfoList;
        
        //print("WorldObjectManager - All data Saved");
    }
    public void LoadData()
    {
        //DataPersistanceManager.instance.LoadGame();
        //worldObjectsInfoList = DataManager.instance.worldObjects_StoreList;

        BuildObjectsFromLoad();

        //print("WorldObjectManager - All data Loaded");
    }

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
}