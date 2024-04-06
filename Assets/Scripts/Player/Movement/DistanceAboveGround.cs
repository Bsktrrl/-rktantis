using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceAboveGround : MonoBehaviour
{
    RaycastHit hit;

    [Header("Raycast Distance")]
    [SerializeField] float raycastDistance = 0.75f;

    [Header("RayCast Points")]
    public GameObject point_0;
    public GameObject point_1;
    public GameObject point_2;
    public GameObject point_3;
    public GameObject point_4;

    [Header("RayCast hits")]
    public bool point_0_Hit;
    public bool point_1_Hit;
    public bool point_2_Hit;
    public bool point_3_Hit;
    public bool point_4_Hit;

    [Header("isGrounded")]
    public bool isGrounded;


    //--------------------


    private void Update()
    {
        if (Raycast(point_0.transform.position))
        {
            isGrounded = true;
        }
        else if (Raycast(point_1.transform.position))
        {
            isGrounded = true;
        }
        else if (Raycast(point_2.transform.position))
        {
            isGrounded = true;
        }
        else if (Raycast(point_3.transform.position))
        {
            isGrounded = true;
        }
        else if (Raycast(point_4.transform.position))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        
        //Raycast(point_1.transform.position, point_1_Hit);
        //Raycast(point_2.transform.position, point_2_Hit);
        //Raycast(point_3.transform.position, point_3_Hit);
        //Raycast(point_4.transform.position, point_4_Hit);

        //if (point_0_Hit || point_1_Hit || point_2_Hit || point_3_Hit || point_4_Hit)
        //{
        //    isGrounded = true;
        //}
        //else
        //{
        //    isGrounded = false;
        //}
    }

    bool Raycast(Vector3 pos)
    {
        if (Physics.Raycast(pos, Vector3.down, out hit, raycastDistance))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
