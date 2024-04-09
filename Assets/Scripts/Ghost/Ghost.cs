using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public GhostStates ghostState;


    //--------------------


    private void Update()
    {
        float distanceFromPlayer = Vector3.Distance(gameObject.transform.position, MainManager.Instance.playerBody.transform.position);

        if (distanceFromPlayer >= GhostManager.Instance.despawnDistance)
        {
            GhostManager.Instance.ReturnGhostToPool(gameObject);
        }   
    }
}
