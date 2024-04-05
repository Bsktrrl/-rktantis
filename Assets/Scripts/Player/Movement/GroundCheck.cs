using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public PlayerMovement PlayerMovement;

    public LayerMask groundMask;
    public LayerMask buildingBlockMask;

    public string groundMask_Tag;
    public string buildingBlockMask_Tag;

    public bool isGrounded;
    public LayerMask currentUnderPlayer;


    //--------------------


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == groundMask || collision.gameObject.layer == buildingBlockMask)
        {
            currentUnderPlayer = collision.gameObject.layer;
            print("collision.gameObject.layer == groundMask || collision.gameObject.layer == buildingBlockMask");
        }
        else
        {
            currentUnderPlayer = LayerMask.GetMask("Nothing");
            print("currentUnderPlayer = LayerMask.GetMask(\"Nothing\")");
        }

        if (collision.gameObject.layer == groundMask || collision.gameObject.layer == buildingBlockMask
            || collision.gameObject.tag == groundMask_Tag || collision.gameObject.tag == buildingBlockMask_Tag)
        {
            //print("2. OnCollisionEnter | Layer: " + collision.gameObject.layer + " | Tag: " + collision.gameObject.tag);
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //currentUnderPlayer = LayerMask.GetMask("Default");

        //if (collision.gameObject.layer == groundMask || collision.gameObject.layer == buildingBlockMask)
        //{
        //    currentUnderPlayer = collision.gameObject.layer;
        //}

        //print("1. OnCollisionExit | Layer: " + collision.gameObject.layer + " | Tag: " + collision.gameObject.tag);

        if (collision.gameObject.layer == groundMask || collision.gameObject.layer == buildingBlockMask
            || collision.gameObject.tag == groundMask_Tag || collision.gameObject.tag == buildingBlockMask_Tag)
        {
            //print("2. OnCollisionExit | Layer: " + collision.gameObject.layer + " | Tag: " + collision.gameObject.tag);
            isGrounded = false;
        }
    }

}
