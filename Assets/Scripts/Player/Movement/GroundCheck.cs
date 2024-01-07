using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public PlayerMovement PlayerMovement;

    public LayerMask groundMask;
    public LayerMask buildingBlockMask;

    private void OnCollisionEnter(Collision collision)
    {
        //print("1. OnCollisionEnter");
        PlayerMovement.isGrounded = true;


        //if (collision.gameObject.layer == groundMask || collision.gameObject.layer == buildingBlockMask)
        //{
        //    print("2. OnCollisionEnter");
            
        //}
    }

    private void OnCollisionExit(Collision collision)
    {
        //print("1. OnCollisionExit");
        PlayerMovement.isGrounded = false;

        //if (collision.gameObject.layer == groundMask || collision.gameObject.layer == buildingBlockMask)
        //{
        //    print("2. OnCollisionExit");
            
        //}
    }

}
