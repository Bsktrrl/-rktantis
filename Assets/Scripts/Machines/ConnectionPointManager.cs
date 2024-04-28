using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionPointManager : Singleton<ConnectionPointManager>
{
    [SerializeField] GameObject CancelConnectionInfoText;

    public List<ConnectionInfo> connectionInfoList = new List<ConnectionInfo>();

    public ConnectionState connectionState;
    public ConnectionOngoingInfo connectionOngoing;


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


    void SetupConnections()
    {
        for (int i = 0; i < connectionInfoList.Count; i++)
        {
            connectionOngoing.connectionType_1_Index = connectionInfoList[i].connections.object1_Index;
            connectionOngoing.connectionType_2_Index = connectionInfoList[i].connections.object2_Index;

            FindWorldObjectsToConnect();
        }

        ClearConnection();
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

        ClearConnection();

        SaveData();
    }
    void FindWorldObjectsToConnect()
    {
        //Find WorldObject 1 - Add WorldObject 2
        MoveableObject move_1 = FindWorldObject(connectionOngoing.connectionType_1_Index);
        if (move_1)
        {
            print("1.0. FindWorldObject");
            if (move_1.connectionPointObject)
            {
                print("1.1. FindWorldObject");
                if (move_1.connectionPointObject.GetComponent<ConnectionPoint>())
                {
                    print("1.2. FindWorldObject");
                    move_1.connectionPointObject.GetComponent<ConnectionPoint>().worldObjectIndex_ConnectedWith = connectionOngoing.connectionType_2_Index;
                }
            }
        }
        
        
        //Find WorldObject 2 - Add WorldObject 1
        MoveableObject move_2 = FindWorldObject(connectionOngoing.connectionType_2_Index);
        if (move_2)
        {
            print("2.0. FindWorldObject");
            if (move_2.connectionPointObject)
            {
                print("2.1. FindWorldObject");
                if (move_2.connectionPointObject.GetComponent<ConnectionPoint>())
                {
                    print("2.3. FindWorldObject");
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