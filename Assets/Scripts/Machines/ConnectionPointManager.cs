using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ConnectionPointManager : Singleton<ConnectionPointManager>
{
    [SerializeField] GameObject CancelConnectionInfoText;

    public List<ConnectionInfo> connectionInfoList = new List<ConnectionInfo>();

    public ConnectionState connectionState;
    public ConnectionOngoingInfo connectionOngoing;

    public GameObject linePrefab;
    public GameObject cubeInstance;


    //--------------------


    private void Awake()
    {
        connectionOngoing.connectionType_1 = ConnectionTypes.None;
        connectionOngoing.connectionType_1_Index = -1;

        connectionOngoing.connectionType_2 = ConnectionTypes.None;
        connectionOngoing.connectionType_2_Index = -1;
    }
    private void Start()
    {
        PlayerButtonManager.releaseConnection_isPressed += ClearConnection;
    }
    private void Update()
    {
        if (!DataManager.Instance.hasLoaded) { return; }

        if (MainManager.Instance.menuStates == MenuStates.None)
        {
            if (connectionState == ConnectionState.isConnecting)
            {
                CancelConnectionInfoText.SetActive(true);
            }
            else
            {
                CancelConnectionInfoText.SetActive(false);
            }
        }
        else
        {
            CancelConnectionInfoText.SetActive(false);
        }
    }


    //--------------------


    public void LoadData()
    {
        connectionInfoList = DataManager.Instance.connectionInfoList_Store;

        SetupConnections();
    }
    public void SaveData()
    {
        if (!DataManager.Instance.hasLoaded) { return; }

        DataManager.Instance.connectionInfoList_Store = connectionInfoList;
    }


    //--------------------


    public void SetupConnections()
    {
        for (int i = 0; i < connectionInfoList.Count; i++)
        {
            connectionOngoing.connectionType_1_Index = connectionInfoList[i].connections.object1_Index;
            connectionOngoing.connectionType_2_Index = connectionInfoList[i].connections.object2_Index;

            FindWorldObjectsToConnect();

            SpawnLine(connectionInfoList[i].connections.object1_Index, connectionInfoList[i].connections.object2_Index);
        }

        ClearConnection();
    }
    public void UpdateConnectionsAfterRemovingBuildingObject(int index)
    {
        //Decrease indexes - Works
        for (int i = connectionInfoList.Count - 1; i >= 0; i--)
        {
            //Set all connectionObjects with a higher index -1 to keep its connection
            if (connectionInfoList[i].connections.object1_Index > index)
            {
                connectionInfoList[i].connections.object1_Index -= 1;
            }
            if (connectionInfoList[i].connections.object2_Index > index)
            {
                connectionInfoList[i].connections.object2_Index -= 1;
            }

            //Set all connectedObjects with a higher index -1 to keep its connection
            if (connectionInfoList[i].connections.object1_ConnectedToIndex > index)
            {
                connectionInfoList[i].connections.object1_ConnectedToIndex -= 1;
            }
            if (connectionInfoList[i].connections.object2_ConnectedToIndex > index)
            {
                connectionInfoList[i].connections.object2_ConnectedToIndex -= 1;
            }
        }
        
        SaveData();
    }
    public void RemoveBrokenConnections(int index)
    {
        for (int i = 0; i < connectionInfoList.Count; i++)
        {
            if (connectionInfoList[i].connections.object1_ConnectedToIndex == index
                || connectionInfoList[i].connections.object2_ConnectedToIndex == index)
            {
                connectionInfoList.RemoveAt(i);

                break;
            }
        }
    }
    public void ResetWorldObjectConnection()
    {
        for (int i = 0; i < BuildingSystemManager.Instance.worldBuildingObjectListSpawned.Count; i++)
        {
            if (BuildingSystemManager.Instance.worldBuildingObjectListSpawned[i].GetComponent<MoveableObject>())
            {
                if (BuildingSystemManager.Instance.worldBuildingObjectListSpawned[i].GetComponent<MoveableObject>().connectionPointObject)
                {
                    if (BuildingSystemManager.Instance.worldBuildingObjectListSpawned[i].GetComponent<MoveableObject>().connectionPointObject.GetComponent<ConnectionPoint>())
                    {
                        BuildingSystemManager.Instance.worldBuildingObjectListSpawned[i].GetComponent<MoveableObject>().connectionPointObject.GetComponent<ConnectionPoint>().worldObjectIndex_ConnectedWith = -1;
                    }
                }
            }
        }
    }


    //--------------------



    public void ConnectObjects()
    {
        ConnectionInfo connectionInfo = new ConnectionInfo();

        connectionInfo.connections.object1_Index = connectionOngoing.connectionType_1_Index;
        connectionInfo.connections.object2_Index = connectionOngoing.connectionType_2_Index;

        connectionInfo.connections.object1_ConnectedToIndex = connectionOngoing.connectionType_2_Index;
        connectionInfo.connections.object2_ConnectedToIndex = connectionOngoing.connectionType_1_Index;

        connectionInfoList.Add(connectionInfo);

        FindWorldObjectsToConnect();

        SpawnLine(connectionOngoing.connectionType_1_Index, connectionOngoing.connectionType_2_Index);

        ClearConnection();

        SaveData();
    }
    void FindWorldObjectsToConnect()
    {
        //Find WorldObject 1 - Add WorldObject 2
        MoveableObject move_1 = FindWorldObject(connectionOngoing.connectionType_1_Index);
        if (move_1)
        {
            if (move_1.connectionPointObject)
            {
                if (move_1.connectionPointObject.GetComponent<ConnectionPoint>())
                {
                    move_1.connectionPointObject.GetComponent<ConnectionPoint>().worldObjectIndex_ConnectedWith = connectionOngoing.connectionType_2_Index;
                }
            }
        }
        
        
        //Find WorldObject 2 - Add WorldObject 1
        MoveableObject move_2 = FindWorldObject(connectionOngoing.connectionType_2_Index);
        if (move_2)
        {
            if (move_2.connectionPointObject)
            {
                if (move_2.connectionPointObject.GetComponent<ConnectionPoint>())
                {
                    move_2.connectionPointObject.GetComponent<ConnectionPoint>().worldObjectIndex_ConnectedWith = connectionOngoing.connectionType_1_Index;
                }
            }
        }
    }


    public void RemoveConnection(int a)
    {
        for (int i = 0; i < connectionInfoList.Count; i++)
        {
            if (connectionInfoList[i].connections.object1_Index == a
                || connectionInfoList[i].connections.object2_Index == a)
            {
                //Remove ConnectionInfo
                connectionInfoList.RemoveAt(i);

                //Remove connection in both WorldObjects
                MoveableObject move = FindWorldObject(a);
                if (move)
                {
                    //Remove connected connection to interacted Connection
                    MoveableObject connection = FindWorldObject(move.connectionPointObject.GetComponent<ConnectionPoint>().worldObjectIndex_ConnectedWith);
                    if (connection)
                    {
                        connection.connectionPointObject.GetComponent<ConnectionPoint>().worldObjectIndex_ConnectedWith = -1;
                    }

                    //Remove interacted Connection
                    move.connectionPointObject.GetComponent<ConnectionPoint>().worldObjectIndex_ConnectedWith = -1;
                }
            }

            break;
        }

        SaveData();
    }
    MoveableObject FindWorldObject(int a)
    {
        for (int i = 0; i < BuildingSystemManager.Instance.worldBuildingObjectListSpawned.Count; i++)
        {
            if (BuildingSystemManager.Instance.worldBuildingObjectListSpawned[i].GetComponent<MoveableObject>())
            {
                if (BuildingSystemManager.Instance.worldBuildingObjectListSpawned[i].GetComponent<MoveableObject>().index == a)
                {
                    return BuildingSystemManager.Instance.worldBuildingObjectListSpawned[i].GetComponent<MoveableObject>();
                }
            }
        }

        return null;
    }

    public void ClearConnection()
    {
        connectionOngoing.connectionType_1 = ConnectionTypes.None;
        connectionOngoing.connectionType_1_Index = -1;

        connectionOngoing.connectionType_2 = ConnectionTypes.None;
        connectionOngoing.connectionType_2_Index = -1;

        connectionState = ConnectionState.None;

        SaveData();
    }

    public void SpawnLine(int a, int b)
    {
        Vector3 pos1 = new Vector3();
        Vector3 pos2 = new Vector3();

        GameObject pos2Object = new GameObject();

        #region Get Object Positions
        //Find WorldObject 1
        MoveableObject move_1 = FindWorldObject(a);
        if (move_1)
        {
            if (move_1.connectionPointObject)
            {
                if (move_1.connectionPointObject.GetComponent<ConnectionPoint>())
                {
                    pos1 = move_1.connectionPointObject.GetComponent<ConnectionPoint>().gameObject.transform.position;
                }
            }
        }


        //Find WorldObject 2
        MoveableObject move_2 = FindWorldObject(b);
        if (move_2)
        {
            if (move_2.connectionPointObject)
            {
                if (move_2.connectionPointObject.GetComponent<ConnectionPoint>())
                {
                    pos2 = move_2.connectionPointObject.GetComponent<ConnectionPoint>().gameObject.transform.position;

                    pos2Object = move_2.connectionPointObject.GetComponent<ConnectionPoint>().gameObject;
                }
            }
        }
        #endregion

        print("pos1: "+ pos1);
        print("pos2: " + pos2);

        cubeInstance = Instantiate(linePrefab, transform.position, Quaternion.identity);

        //Spawn Line
        float distance = Vector3.Distance(pos1, pos2);

        // Set the scale of the cube to stretch between the two objects
        cubeInstance.transform.localScale = new Vector3(0.05f, 0.05f, distance);

        // Position the cube halfway between the two objects
        cubeInstance.transform.position = (pos1 + pos2) / 2f;

        // Rotate the cube to align with the line between the two objects
        cubeInstance.transform.LookAt(pos2Object.transform);
    }
}

[Serializable]
public class Connections
{
    public int object1_Index;
    public int object1_ConnectedToIndex;

    public int object2_Index;
    public int object2_ConnectedToIndex;
}

[Serializable]
public class ConnectionInfo
{
    public Connections connections = new Connections();
}

[Serializable]
public class ConnectionOngoingInfo
{
    public ConnectionTypes connectionType_1 = new ConnectionTypes();
    public int connectionType_1_Index;

    public ConnectionTypes connectionType_2 = new ConnectionTypes();
    public int connectionType_2_Index;
}
public enum ConnectionTypes
{
    None,
    GhostTank,
    NotGhostTank
}
public enum ConnectionState
{
    None,
    isConnecting
}