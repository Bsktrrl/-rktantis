using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class BuildingManager_v2 : Singleton<BuildingManager_v2>
{
    public GameObject buildingBlockHit;
    public BuildingBlockColliderDirection directionHit;

    [Header("BuildingBlocks Detected")]
    [SerializeField] GameObject BB_Normal;
    [SerializeField] GameObject BB_Up;
    [SerializeField] GameObject BB_Down;
    [SerializeField] GameObject BB_Left;
    [SerializeField] GameObject BB_Right;

    [Header("RayCasting")]
    public Ray ray;
    RaycastHit rayHit;
    [SerializeField] LayerMask mask;
    [SerializeField] float rayLength = 4;
    Transform hitTransform;

    [Header("BuilgingBlockGhost")]
    public GameObject buildingBlockGhost;
    [SerializeField] Material ghost_Can;
    [SerializeField] Material ghost_Cannot;

    [Header("Rotation")]
    float ghostRotationValue = 0;
    Quaternion ghostRotation = Quaternion.identity;


    //--------------------


    private void Start()
    {
        buildingBlockGhost.SetActive(false);

        PlayerButtonManager.isPressed_FixedRotation += RotateGhostBlock;
    }
    private void Update()
    {
        //Only run RayCast when "BuildingHammer" is in the hand
        if (HotbarManager.Instance.selectedItem == Items.WoodSword
            || HotbarManager.Instance.selectedItem == Items.StoneSword
            || HotbarManager.Instance.selectedItem == Items.CryoniteSword)
        {
            Raycast();
        }

        //If looking at a BuildingBlock, show a BuildingBlock_Ghost in the correct position based on the "directionHit"
        if (buildingBlockHit != null)
        {
            DisplayBuildingBlockGhost();

            buildingBlockGhost.SetActive(true);
        }
        else
        {
            buildingBlockGhost.SetActive(false);
        }
    }


    //--------------------


    public void Raycast()
    {
        // Cast a Ray from the mouse position to the world space
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //Make 5 Raycasts (Forward, Up, Down, Right, Left)
        RaycastDirection(RaycastDirections.Up, Color.cyan);
        RaycastDirection(RaycastDirections.Down, Color.cyan);
        RaycastDirection(RaycastDirections.Left, Color.blue);
        RaycastDirection(RaycastDirections.Right, Color.blue);
        RaycastDirection(RaycastDirections.Normal, Color.green);

        #region Update which BuildingBlock to look at
        //If looking directly at a BuildingBlock
        if ((BB_Normal != null && hitTransform != null))
        {
            //Get the direction of the object looking at
            hitTransform.gameObject.GetComponent<BuildingBlockDirection_v2>().EnterBlockDirection_BB_Normal();
        }

        else if (BB_Normal == null && BB_Right != null && hitTransform != null)
        {
            hitTransform.gameObject.GetComponent<BuildingBlockDirection_v2>().EnterBlockDirection_BB_Right();
        }
        else if (BB_Normal == null && BB_Left != null && hitTransform != null)
        {
            hitTransform.gameObject.GetComponent<BuildingBlockDirection_v2>().EnterBlockDirection_BB_Left();
        }
        else if (BB_Normal == null && BB_Up != null && hitTransform != null)
        {
            hitTransform.gameObject.GetComponent<BuildingBlockDirection_v2>().EnterBlockDirection_BB_Up();
        }
        else if (BB_Normal == null && BB_Down != null && hitTransform != null)
        {
            hitTransform.gameObject.GetComponent<BuildingBlockDirection_v2>().EnterBlockDirection_BB_Down();
        }

        //If not looking at a BuildingBlock at all
        else
        {
            directionHit = BuildingBlockColliderDirection.None;
            buildingBlockHit = null;
        }
        #endregion
    }
    void RaycastDirection(RaycastDirections _raycastDirection, Color color)
    {
        Vector3 direction = Vector3.zero;

        //Get the correct Direction
        switch (_raycastDirection)
        {
            case global::RaycastDirections.None:
                direction = Vector3.zero;
                break;

            case global::RaycastDirections.Normal:
                direction = Vector3.zero;
                break;
            case global::RaycastDirections.Up:
                direction = transform.up;
                break;
            case global::RaycastDirections.Down:
                direction = -transform.up;
                break;
            case global::RaycastDirections.Left:
                direction = -transform.right;
                break;
            case global::RaycastDirections.Right:
                direction = transform.right;
                break;

            default:
                direction = Vector3.zero;
                break;
        }
        Vector3 startPoint = ray.origin + direction * 0.75f;

        Debug.DrawRay(startPoint, Camera.main.transform.forward * rayLength, color);

        if (Physics.Raycast(startPoint, Camera.main.transform.forward * rayLength, out rayHit, rayLength, mask))
        {
            //Get the Transform of GameObject hit
            hitTransform = rayHit.transform;

            //Get the Object looking at
            if (hitTransform.gameObject.GetComponent<BuildingBlockDirection_v2>())
            {
                switch (_raycastDirection)
                {
                    case global::RaycastDirections.None:
                        BB_Normal = hitTransform.gameObject;
                        break;

                    case global::RaycastDirections.Normal:
                        BB_Normal = hitTransform.gameObject;
                        break;
                    case global::RaycastDirections.Up:
                        BB_Up = hitTransform.gameObject;
                        break;
                    case global::RaycastDirections.Down:
                        BB_Down = hitTransform.gameObject;
                        break;
                    case global::RaycastDirections.Left:
                        BB_Right = hitTransform.gameObject;
                        break;
                    case global::RaycastDirections.Right:
                        BB_Left = hitTransform.gameObject;
                        break;

                    default:
                        BB_Normal = hitTransform.gameObject;
                        break;
                }
            }
        }

        //Looking at something not a BuildingBlock
        else if (Physics.Raycast(startPoint, Camera.main.transform.forward * rayLength, out rayHit, rayLength))
        {
            switch (_raycastDirection)
            {
                case global::RaycastDirections.None:
                    BB_Normal = null;
                    break;

                case global::RaycastDirections.Normal:
                    BB_Normal = null;
                    break;
                case global::RaycastDirections.Up:
                    BB_Up = null;
                    break;
                case global::RaycastDirections.Down:
                    BB_Down = null;
                    break;
                case global::RaycastDirections.Left:
                    BB_Right = null;
                    break;
                case global::RaycastDirections.Right:
                    BB_Left = null;
                    break;

                default:
                    BB_Normal = null;
                    break;
            }
        }

        //Looking at Nothing (out in the thin air)
        else
        {
            switch (_raycastDirection)
            {
                case global::RaycastDirections.None:
                    BB_Normal = null;
                    break;

                case global::RaycastDirections.Normal:
                    BB_Normal = null;
                    break;
                case global::RaycastDirections.Up:
                    BB_Up = null;
                    break;
                case global::RaycastDirections.Down:
                    BB_Down = null;
                    break;
                case global::RaycastDirections.Left:
                    BB_Right = null;
                    break;
                case global::RaycastDirections.Right:
                    BB_Left = null;
                    break;

                default:
                    BB_Normal = null;
                    break;
            }
        }
    }

    void DisplayBuildingBlockGhost()
    {
        //Check if "buildingBlockHit" has a Script
        if (buildingBlockHit.GetComponent<BuildingBlock_v2>())
        {
            print("1. Got in");

            //Check if the Ghost has a MeshRenderer
            if (buildingBlockGhost.GetComponent<BuildingBlock_v2>())
            {
                print("2. Got in");

                //Set new Info in Ghost_Parent
                buildingBlockGhost.GetComponent<BuildingBlock_v2>().buildingType = buildingBlockHit.GetComponent<BuildingBlock_v2>().buildingType;
                buildingBlockGhost.GetComponent<BuildingBlock_v2>().buildingMaterial = buildingBlockHit.GetComponent<BuildingBlock_v2>().buildingMaterial;
                
                if (buildingBlockGhost.GetComponent<BuildingBlock_v2>().model)
                {
                    if (buildingBlockGhost.GetComponent<BuildingBlock_v2>().model.GetComponent<MeshFilter>()
                        && buildingBlockGhost.GetComponent<BuildingBlock_v2>().model.GetComponent<MeshRenderer>()
                        && buildingBlockGhost.GetComponent<BuildingBlock_v2>().model.GetComponent<MeshCollider>())
                    {
                        print("3. Got in");

                        //Set new Mesh on the Ghost
                        buildingBlockGhost.GetComponent<BuildingBlock_v2>().model.GetComponent<MeshFilter>().mesh = buildingBlockHit.GetComponent<BuildingBlock_v2>().model.GetComponent<MeshFilter>().mesh;

                        //Set new Material on the Ghost
                        buildingBlockGhost.GetComponent<BuildingBlock_v2>().model.GetComponent<MeshRenderer>().material = ghost_Can;

                        //Set new MeshCollider
                        buildingBlockGhost.GetComponent<BuildingBlock_v2>().model.GetComponent<MeshCollider>().sharedMesh = buildingBlockHit.GetComponent<BuildingBlock_v2>().model.GetComponent<MeshCollider>().sharedMesh;

                        //Set new Position of the Ghost
                        switch (directionHit)
                        {
                            case BuildingBlockColliderDirection.None:
                                buildingBlockGhost.SetActive(false);
                                break;

                            case BuildingBlockColliderDirection.Front:
                                buildingBlockGhost.transform.SetPositionAndRotation(buildingBlockHit.transform.position + Vector3.forward * 2, ghostRotation);
                                break;
                            case BuildingBlockColliderDirection.Back:
                                buildingBlockGhost.transform.SetPositionAndRotation(buildingBlockHit.transform.position - Vector3.forward * 2, ghostRotation);
                                break;
                            case BuildingBlockColliderDirection.Up:
                                buildingBlockGhost.transform.SetPositionAndRotation(buildingBlockHit.transform.position + Vector3.up * 2, ghostRotation);
                                break;
                            case BuildingBlockColliderDirection.Down:
                                buildingBlockGhost.transform.SetPositionAndRotation(buildingBlockHit.transform.position - Vector3.up * 2, ghostRotation);
                                break;
                            case BuildingBlockColliderDirection.Left:
                                buildingBlockGhost.transform.SetPositionAndRotation(buildingBlockHit.transform.position + Vector3.right * 2, ghostRotation);
                                break;
                            case BuildingBlockColliderDirection.Right:
                                buildingBlockGhost.transform.SetPositionAndRotation(buildingBlockHit.transform.position - Vector3.right * 2, ghostRotation);
                                break;

                            default:
                                buildingBlockGhost.SetActive(false);
                                break;
                        }

                        buildingBlockGhost.SetActive(true);
                    }
                }
            }
        }
    }


    //--------------------


    void RotateGhostBlock()
    {
        ghostRotationValue -= 90;

        if (ghostRotationValue <= -360)
        {
            ghostRotationValue = 0;
        }

        ghostRotation = Quaternion.Euler(0, ghostRotationValue, 0);
    }


    //--------------------


    public void placeBuildingBlock(Vector3 pos)
    {

    }
}

public enum RaycastDirections
{
    None,

    Normal,
    Up,
    Down,
    Left,
    Right
}

public enum BuildingBlockColliderDirection
{
    None,

    Front,
    Back,
    Up,
    Down,
    Left,
    Right
}