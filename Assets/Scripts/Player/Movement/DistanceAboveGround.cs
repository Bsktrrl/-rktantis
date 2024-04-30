using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceAboveGround : Singleton<DistanceAboveGround>
{
    RaycastHit hit;

    [Header("RaycastHit Distance")]
    [SerializeField] float raycastDistance_CheckGround = 0.25f;

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

    public bool isInside;
    [SerializeField] LayerMask roof_LayerMask;
    [SerializeField] LayerMask player_LayerMask;


    //--------------------


    private void Start()
    {
        raycastDistance_CheckGround = 0.25f;
    }
    private void Update()
    {
        if (!DataManager.Instance.hasLoaded) { return; }

        if (MainManager.Instance.gameStates == GameStates.GameOver) { return; }

        if (!MainManager.Instance.deleyedStart)
        {
            return;
        }

        isInside = CheckIfInside();

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

            if (GroundLookingAt)
            {
                if (GroundLookingAt.GetComponent<Model>())
                {
                    if (GroundLookingAt.GetComponent<Model>().gameObject.transform.parent.GetComponent<MoveableObject>())
                    {
                        PlayerManager.Instance.UpdatePlayerDyingPos(GroundLookingAt.transform);
                    }
                }
            }

            return true;
        }
        else
        {
            GroundLookingAt = null;

            return false;
        }
    }

    bool CheckIfInside()
    {
        Vector3 startPos = new Vector3(MainManager.Instance.playerBody.transform.position.x, MainManager.Instance.playerBody.transform.position.y + 0.5f, MainManager.Instance.playerBody.transform.position.z);

        //Debug.DrawRay(startPos, Vector3.up, Color.white, 2.5f);

        if (Physics.Raycast(startPos, Vector3.up, out hit, 2.5f, ~player_LayerMask))
        {
            if (hit.collider.gameObject.layer == 8
                || hit.collider.gameObject.layer == 3)
            {
                return true;
            }
        }

        return false;
    }
}
