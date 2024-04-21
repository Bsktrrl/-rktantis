using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceAboveGround : MonoBehaviour
{
    RaycastHit hit;

    [Header("RaycastHit Distance")]
    [SerializeField] float raycastDistance_CheckGround = 0.1f;

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
    public GameObject GroundLookingAt;


    //--------------------


    private void Start()
    {
        raycastDistance_CheckGround = 0.1f;

        //point_0.transform.SetLocalPositionAndRotation(new Vector3(0, -1, 0), Quaternion.identity);
        //point_1.transform.SetLocalPositionAndRotation(new Vector3(0, -1, 0.5f), Quaternion.identity);
        //point_2.transform.SetLocalPositionAndRotation(new Vector3(0, -1, -0.5f), Quaternion.identity);
        //point_3.transform.SetLocalPositionAndRotation(new Vector3(0.5f, -1, 0), Quaternion.identity);
        //point_4.transform.SetLocalPositionAndRotation(new Vector3(-0.5f, -1, 0), Quaternion.identity);
    }
    private void Update()
    {
        if (MainManager.Instance.gameStates == GameStates.GameOver) { return; }

        point_0_Hit = false;
        point_1_Hit = false;
        point_2_Hit = false;
        point_3_Hit = false;
        point_4_Hit = false;

        if (RaycastHit(point_0.transform.position))
        {
            point_0_Hit = true;
            isGrounded = true;
        }
        else if (RaycastHit(point_1.transform.position))
        {
            point_1_Hit = true;
            isGrounded = true;
        }
        else if (RaycastHit(point_2.transform.position))
        {
            point_2_Hit = true;
            isGrounded = true;
        }
        else if (RaycastHit(point_3.transform.position))
        {
            point_3_Hit = true;
            isGrounded = true;
        }
        else if (RaycastHit(point_4.transform.position))
        {
            point_4_Hit = true;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    bool RaycastHit(Vector3 pos)
    {
        if (Physics.Raycast(pos, Vector3.down, out hit, raycastDistance_CheckGround))
        {
            GroundLookingAt = hit.transform.gameObject;

            if (GroundLookingAt.GetComponent<Model>())
            {
                PlayerManager.Instance.UpdatePlayerDyingPos(GroundLookingAt.transform);
            }

            return true;
        }
        else
        {
            GroundLookingAt = null;

            return false;
        }
    }
}
