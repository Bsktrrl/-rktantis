using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public GhostStates ghostState;


    //--------------------


    private void Update()
    {
        //Check for Despawn distance
        float distanceFromPlayer = Vector3.Distance(gameObject.transform.position, MainManager.Instance.playerBody.transform.position);
        if (distanceFromPlayer >= GhostManager.Instance.despawnDistance)
        {
            GhostManager.Instance.ReturnGhostToPool(gameObject);
        }

        //Behavior Tree
        switch (ghostState)
        {
            //Get the action from the selected State
            case GhostStates.Idle:
                Idle();
                break;
            case GhostStates.Moving:
                Move();
                break;
            case GhostStates.Attacking:
                Attack();
                break;
            case GhostStates.Fleeing:
                Flee();
                break;
            case GhostStates.Captured:
                Captured();
                break;
        }
    }


    //--------------------


    void Idle()
    {

    }

    void Move()
    {

    }

    void Attack()
    {

    }

    void Flee()
    {

    }

    void Captured()
    {

    }
}
