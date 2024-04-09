using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBehaviourTree : MonoBehaviour
{
    private void Update()
    {
        if (gameObject.GetComponent<Ghost>())
        {
            switch (gameObject.GetComponent<Ghost>().ghostState)
            {
                case GhostStates.Idle:
                    break;
                case GhostStates.Moving:
                    break;
                case GhostStates.Attacking:
                    break;
                case GhostStates.Fleeing:
                    break;
                case GhostStates.Captured:
                    break;

                default:
                    break;
            }
        }
    }
}
