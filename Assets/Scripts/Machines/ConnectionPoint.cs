using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionPoint : MonoBehaviour
{
    public GameObject parent;

    [SerializeField] ConnectionTypes connectionType;

    public int worldObjectIndex_ConnectedWith = -1;


    //--------------------


    private void Awake()
    {
        worldObjectIndex_ConnectedWith = -1;
    }


    //--------------------


    public void AddConnectPoint()
    {
        //Add First Connection
        if (ConnectionPointManager.Instance.connectionOngoing.connectionType_1_Index < 0)
        {
            ConnectionPointManager.Instance.connectionOngoing.connectionType_1_Index = parent.GetComponent<MoveableObject>().index;
            ConnectionPointManager.Instance.connectionOngoing.connectionType_1 = connectionType;

            ConnectionPointManager.Instance.connectionState = ConnectionState.isConnecting;
        }

        //Add Second Connection
        else
        {
            if (ConnectionPointManager.Instance.connectionOngoing.connectionType_1 != connectionType)
            {
                ConnectionPointManager.Instance.connectionOngoing.connectionType_2_Index = parent.GetComponent<MoveableObject>().index;
                ConnectionPointManager.Instance.connectionOngoing.connectionType_2 = connectionType;

                ConnectionPointManager.Instance.connectionState = ConnectionState.None;

                ConnectionPointManager.Instance.ConnectObjects();
            }
        }
    }
    public void RemoveConnectPoint()
    {
        if (worldObjectIndex_ConnectedWith >= 0)
        {
            ConnectionPointManager.Instance.RemoveConnection(worldObjectIndex_ConnectedWith);
        }
    }


    //--------------------


    public void DestroyThisConnectionObject()
    {
        if (gameObject.GetComponent<InteractableObject>())
        {
            print("Destroy Connection: " + gameObject.name);

            gameObject.GetComponent<InteractableObject>().DestroyThisInteractableObject();
        }

        Destroy(gameObject);
    }
}
