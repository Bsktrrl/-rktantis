using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class BuildingManager_v2 : Singleton<BuildingManager_v2>
{
    public GameObject buildingBlock_Parent;
    public List<GameObject> buildingBlockList = new List<GameObject>();

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

    [SerializeField] float rayHitAngle_Normal;
    [SerializeField] float rayHitAngle_Left;
    [SerializeField] float rayHitAngle_Right;
    [SerializeField] float rayHitAngle_Up;
    [SerializeField] float rayHitAngle_Down;

    [Header("BuilgingBlockGhost")]
    public GameObject buildingBlockGhost;
    [SerializeField] Material ghost_Can;
    [SerializeField] Material ghost_Cannot;
    [SerializeField] bool ghost_isActive;

    [Header("Rotation")]
    float ghostRotationValue = 0;
    float ghostRotationInternValue = 0;
    Quaternion ghostRotation = Quaternion.identity;


    //--------------------


    private void Start()
    {
        buildingBlockGhost.SetActive(false);

        PlayerButtonManager.isPressed_FixedRotation_Clockwise += RotateGhostBlock_Clockwise;
        PlayerButtonManager.isPressed_FixedRotation_CounterClockwise += RotateGhostBlock_CounterClockwise;

        PlayerButtonManager.isPressed_FixedRotation_Intern_Clockwise += RotateGhostBlock_Intern_Clockwise;
        PlayerButtonManager.isPressed_FixedRotation_Intern_CounterClockwise += RotateGhostBlock_Intern_CounterClockwise;

        PlayerButtonManager.T_isPressed += placeBuildingBlock;
    }
    private void Update()
    {
        //Only run RayCast when "BuildingHammer" is in the hand
        if (HotbarManager.Instance.selectedItem == Items.WoodSword
            || HotbarManager.Instance.selectedItem == Items.StoneSword
            || HotbarManager.Instance.selectedItem == Items.CryoniteSword)
        {
            MainManager.Instance.gameStates = GameStates.Building;

            Raycast();
        }
        else
        {
            ghost_isActive = false;
            buildingBlockGhost.SetActive(false);
        }

        //If looking at a BuildingBlock, show a BuildingBlock_Ghost in the correct position based on the "directionHit"
        if (buildingBlockHit != null)
        {
            DisplayBuildingBlockGhost();

            ghost_isActive = true;
            buildingBlockGhost.SetActive(true);
        }
        else if (MainManager.Instance.gameStates != GameStates.Building)
        {
            ghost_isActive = false;
            buildingBlockGhost.SetActive(false);
        }
        else
        {
            ghost_isActive = false;
            buildingBlockGhost.SetActive(false);
        }

        //Hide ghost when "BuildingHammer" is NOT in the hand
        if (HotbarManager.Instance.selectedItem != Items.WoodSword
            && HotbarManager.Instance.selectedItem != Items.StoneSword
            && HotbarManager.Instance.selectedItem != Items.CryoniteSword)
        {
            ghost_isActive = false;
            buildingBlockGhost.SetActive(false);
        }
    }


    //--------------------


    public void LoadData()
    {
        ////If data has not saved, set to "Wood Floor"
        //#region
        //if (MoveableObjectManager.Instance.buildingType_Selected == BuildingType.None || MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.None)
        //{
        //    MoveableObjectManager.Instance.buildingType_Selected = BuildingType.Floor;
        //    MoveableObjectManager.Instance.buildingMaterial_Selected = BuildingMaterial.Wood;
        //}
        //#endregion

        ////Set Preview image for the selected buildingBlock
        //#region
        //for (int i = 0; i < BuildingSystemMenu.Instance.buildingBlockUIList.Count; i++)
        //{
        //    if (BuildingSystemMenu.Instance.buildingBlockUIList[i].GetComponent<BuildingBlock_UI>().buildingType == MoveableObjectManager.Instance.buildingType_Selected
        //        && BuildingSystemMenu.Instance.buildingBlockUIList[i].GetComponent<BuildingBlock_UI>().buildingMaterial == MoveableObjectManager.Instance.buildingMaterial_Selected)
        //    {
        //        BuildingSystemMenu.Instance.SetSelectedImage(BuildingSystemMenu.Instance.buildingBlockUIList[i].GetComponent<BuildingBlock_UI>().parent.GetComponent<Image>().sprite);

        //        break;
        //    }
        //}
        //#endregion

        ////Setup BuildingBlockList
        //#region
        //for (int i = 0; i < buildingBlockList.Count; i++)
        //{
        //    Destroy(buildingBlockList[i]);
        //}
        //buildingBlockList.Clear();

        //buildingBlockSaveList = DataManager.Instance.buildingBlockList_StoreList;
        //for (int i = 0; i < buildingBlockSaveList.Count; i++)
        //{
        //    buildingBlockList.Add(Instantiate(SetupBuildingBlockFromSave(buildingBlockSaveList[i]), buildingBlockSaveList[i].buildingBlock_Position, buildingBlockSaveList[i].buildingBlock_Rotation) as GameObject);
        //    buildingBlockList[buildingBlockList.Count - 1].transform.parent = buildingBlock_Parent.transform;

        //    buildingBlockList[buildingBlockList.Count - 1].GetComponent<BuildingBlock_Parent>().blockID = buildingBlockSaveList[i].buildingID;
        //}
        //#endregion

        ////Set Building Requirements
        //#region
        //SetBuildingRequirements(GetBuildingBlock(MoveableObjectManager.Instance.buildingType_Selected, MoveableObjectManager.Instance.buildingMaterial_Selected), buildingRequirement_Parent);

        //if (HotbarManager.Instance.selectedItem == Items.WoodBuildingHammer
        //    || HotbarManager.Instance.selectedItem == Items.StoneBuildingHammer
        //    || HotbarManager.Instance.selectedItem == Items.CryoniteBuildingHammer)
        //{
        //    buildingRequirement_Parent.SetActive(true);
        //    BuildingHammer_isActive = true;
        //}
        //#endregion
    }
    public void SaveData()
    {
        //Save buildingBlocks into a saveable list
        //List<BuildingBlockSaveList> tempList = new List<BuildingBlockSaveList>();
        //for (int i = 0; i < buildingBlockList.Count; i++)
        //{
        //    BuildingBlockSaveList temp = new BuildingBlockSaveList();

        //    temp.buildingID = buildingBlockList[i].GetComponent<BuildingBlock_Parent>().blockID;
        //    temp.buildingBlock_Position = buildingBlockList[i].transform.position;
        //    temp.buildingBlock_Rotation = buildingBlockList[i].transform.rotation;

        //    tempList.Add(temp);
        //}
        //DataManager.Instance.buildingBlockList_StoreList = tempList;

        print("Save buildingBlocks");
    }


    //--------------------


    public void Raycast()
    {
        // Cast a Ray from the mouse position to the world space
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //Make 5 Raycasts (Forward, Up, Down, Right, Left)
        RaycastDirection(RaycastDirections.Right, Color.blue);
        RaycastDirection(RaycastDirections.Left, Color.blue);
        RaycastDirection(RaycastDirections.Down, Color.cyan);
        RaycastDirection(RaycastDirections.Up, Color.cyan);
        RaycastDirection(RaycastDirections.Normal, Color.green);

        #region Update which BuildingBlock to look at
        if (hitTransform != null)
        {
            //If looking directly at a BuildingBlock
            if ((BB_Normal != null))
            {
                //Get the direction of the object looking at
                hitTransform.gameObject.GetComponent<BuildingBlockDirection_v2>().EnterBlockDirection_BB_Normal();
            }

            else if (BB_Normal == null && BB_Up != null)
            {
                hitTransform.gameObject.GetComponent<BuildingBlockDirection_v2>().EnterBlockDirection_BB_Up();
            }
            else if (BB_Normal == null && BB_Down != null)
            {
                hitTransform.gameObject.GetComponent<BuildingBlockDirection_v2>().EnterBlockDirection_BB_Down();
            }

            //If Left/Right looks at the BuildingBlock with a smaller Angle
            else if (BB_Normal == null && BB_Left != null && rayHitAngle_Left <= 125)
            {
                hitTransform.gameObject.GetComponent<BuildingBlockDirection_v2>().EnterBlockDirection_BB_Normal();
            }
            else if (BB_Normal == null && BB_Right != null && rayHitAngle_Right <= 125)
            {
                hitTransform.gameObject.GetComponent<BuildingBlockDirection_v2>().EnterBlockDirection_BB_Normal();
            }

            //If Left/Right + Top looks at the BuildingBlock
            else if (BB_Normal == null && BB_Left != null && BB_Up != null && BB_Left != BB_Up)
            {
                hitTransform.gameObject.GetComponent<BuildingBlockDirection_v2>().EnterBlockDirection_BB_Normal();
            }
            else if (BB_Normal == null && BB_Right != null && BB_Up != null && BB_Right != BB_Up)
            {
                hitTransform.gameObject.GetComponent<BuildingBlockDirection_v2>().EnterBlockDirection_BB_Normal();
            }

            //If only 1 Ray looks at the BuildingBlock
            else if (BB_Normal == null && BB_Right != null)
            {
                hitTransform.gameObject.GetComponent<BuildingBlockDirection_v2>().EnterBlockDirection_BB_Right();
            }
            else if (BB_Normal == null && BB_Left != null)
            {
                hitTransform.gameObject.GetComponent<BuildingBlockDirection_v2>().EnterBlockDirection_BB_Left();
            }

            //If not looking at a BuildingBlock at all
            else
            {
                directionHit = BuildingBlockColliderDirection.None;
                buildingBlockHit = null;
            }
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
                        rayHitAngle_Normal = Vector3.Angle(ray.direction, rayHit.normal);
                        break;

                    case global::RaycastDirections.Normal:
                        BB_Normal = hitTransform.gameObject;
                        rayHitAngle_Normal = Vector3.Angle(ray.direction, rayHit.normal);
                        break;
                    case global::RaycastDirections.Up:
                        BB_Up = hitTransform.gameObject;
                        rayHitAngle_Up = Vector3.Angle(ray.direction, rayHit.normal);
                        break;
                    case global::RaycastDirections.Down:
                        BB_Down = hitTransform.gameObject;
                        rayHitAngle_Down = Vector3.Angle(ray.direction, rayHit.normal);
                        break;
                    case global::RaycastDirections.Left:
                        BB_Right = hitTransform.gameObject;
                        rayHitAngle_Right = Vector3.Angle(ray.direction, rayHit.normal);
                        break;
                    case global::RaycastDirections.Right:
                        BB_Left = hitTransform.gameObject;
                        rayHitAngle_Left = Vector3.Angle(ray.direction, rayHit.normal);
                        break;

                    default:
                        BB_Normal = hitTransform.gameObject;
                        rayHitAngle_Normal = Vector3.Angle(ray.direction, rayHit.normal);
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
                    rayHitAngle_Normal = 0;
                    break;

                case global::RaycastDirections.Normal:
                    rayHitAngle_Normal = 0;
                    BB_Normal = null;
                    break;
                case global::RaycastDirections.Up:
                    rayHitAngle_Up = 0;
                    BB_Up = null;
                    break;
                case global::RaycastDirections.Down:
                    rayHitAngle_Down = 0;
                    BB_Down = null;
                    break;
                case global::RaycastDirections.Left:
                    rayHitAngle_Right = 0;
                    BB_Right = null;
                    break;
                case global::RaycastDirections.Right:
                    BB_Left = null;
                    rayHitAngle_Left = 0;
                    break;

                default:
                    BB_Normal = null;
                    rayHitAngle_Normal = 0;
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
                    rayHitAngle_Normal = 0;
                    break;

                case global::RaycastDirections.Normal:
                    BB_Normal = null;
                    rayHitAngle_Normal = 0;
                    break;
                case global::RaycastDirections.Up:
                    BB_Up = null;
                    rayHitAngle_Up = 0;
                    break;
                case global::RaycastDirections.Down:
                    BB_Down = null;
                    rayHitAngle_Down = 0;
                    break;
                case global::RaycastDirections.Left:
                    BB_Right = null;
                    rayHitAngle_Right = 0;
                    break;
                case global::RaycastDirections.Right:
                    BB_Left = null;
                    rayHitAngle_Left = 0;
                    break;

                default:
                    BB_Normal = null;
                    rayHitAngle_Normal = 0;
                    break;
            }
        }
    }

    void DisplayBuildingBlockGhost()
    {
        //Check if "buildingBlockHit" has a Script
        if (buildingBlockHit.GetComponent<BuildingBlock_v2>())
        {
            //Check if the Ghost has a MeshRenderer
            if (buildingBlockGhost.GetComponent<BuildingBlock_v2>())
            {
                //Set new Info in Ghost_Parent
                buildingBlockGhost.GetComponent<BuildingBlock_v2>().buildingType = buildingBlockHit.GetComponent<BuildingBlock_v2>().buildingType;
                buildingBlockGhost.GetComponent<BuildingBlock_v2>().buildingMaterial = buildingBlockHit.GetComponent<BuildingBlock_v2>().buildingMaterial;
                
                if (buildingBlockGhost.GetComponent<BuildingBlock_v2>().model)
                {
                    if (buildingBlockGhost.GetComponent<BuildingBlock_v2>().model.GetComponent<MeshFilter>()
                        && buildingBlockGhost.GetComponent<BuildingBlock_v2>().model.GetComponent<MeshRenderer>()
                        && buildingBlockGhost.GetComponent<BuildingBlock_v2>().model.GetComponent<MeshCollider>())
                    {
                        //Set new Mesh on the Ghost
                        //buildingBlockGhost.GetComponent<BuildingBlock_v2>().model.GetComponent<MeshFilter>().mesh = buildingBlockHit.GetComponent<BuildingBlock_v2>().model.GetComponent<MeshFilter>().mesh;
                        if (MoveableObjectManager.Instance.GetBuildingBlock())
                        {
                            if (MoveableObjectManager.Instance.GetBuildingBlock().GetComponent<BuildingBlock_v2>())
                            {
                                buildingBlockGhost.GetComponent<BuildingBlock_v2>().model.GetComponent<MeshFilter>().sharedMesh = MoveableObjectManager.Instance.GetBuildingBlock().GetComponent<BuildingBlock_v2>().model.GetComponent<MeshFilter>().sharedMesh;
                            }
                        }

                        //Set new Material on the Ghost
                        buildingBlockGhost.GetComponent<BuildingBlock_v2>().model.GetComponent<MeshRenderer>().material = ghost_Can;

                        //Set new MeshCollider
                        //buildingBlockGhost.GetComponent<BuildingBlock_v2>().model.GetComponent<MeshCollider>().sharedMesh = buildingBlockHit.GetComponent<BuildingBlock_v2>().model.GetComponent<MeshCollider>().sharedMesh;
                        if (MoveableObjectManager.Instance.GetBuildingBlock())
                        {
                            if (MoveableObjectManager.Instance.GetBuildingBlock().GetComponent<BuildingBlock_v2>())
                            {
                                buildingBlockGhost.GetComponent<BuildingBlock_v2>().model.GetComponent<MeshCollider>().sharedMesh = MoveableObjectManager.Instance.GetBuildingBlock().GetComponent<BuildingBlock_v2>().model.GetComponent<MeshCollider>().sharedMesh;
                            }
                        }

                        //Set new Position of the Ghost
                        buildingBlockGhost.transform.SetPositionAndRotation(SetPosition(buildingBlockGhost), ghostRotation);
                    }
                }
            }
        }
    }

    Vector3 SetPosition(GameObject _object)
    {
        switch (directionHit)
        {
            case BuildingBlockColliderDirection.None:
                break;

            case BuildingBlockColliderDirection.Front:
                return buildingBlockHit.transform.position + Vector3.forward * 2;
            case BuildingBlockColliderDirection.Back:
                return buildingBlockHit.transform.position - Vector3.forward * 2;
            case BuildingBlockColliderDirection.Up:
                return buildingBlockHit.transform.position + Vector3.up * 2;
            case BuildingBlockColliderDirection.Down:
                return buildingBlockHit.transform.position - Vector3.up * 2;
            case BuildingBlockColliderDirection.Left:
                return buildingBlockHit.transform.position + Vector3.right * 2;
            case BuildingBlockColliderDirection.Right:
                return buildingBlockHit.transform.position - Vector3.right * 2;

            default:
                break;
        }

        return Vector3.zero;
    }


    //--------------------


    void RotateGhostBlock_Clockwise()
    {
        ghostRotationValue += 90;

        if (ghostRotationValue >= +360)
        {
            ghostRotationValue = 0;
        }

        ghostRotation = Quaternion.Euler(ghostRotationInternValue, ghostRotationValue, 0);
    }
    void RotateGhostBlock_CounterClockwise()
    {
        ghostRotationValue -= 90;

        if (ghostRotationValue <= -360)
        {
            ghostRotationValue = 0;
        }

        ghostRotation = Quaternion.Euler(ghostRotationInternValue, ghostRotationValue, 0);
    }
    void RotateGhostBlock_Intern_Clockwise()
    {
        ghostRotationInternValue += 90;

        if (ghostRotationInternValue >= +360)
        {
            ghostRotationInternValue = 0;
        }

        ghostRotation = Quaternion.Euler(0, ghostRotationValue, ghostRotationInternValue);
    }
    void RotateGhostBlock_Intern_CounterClockwise()
    {
        ghostRotationInternValue -= 90;

        if (ghostRotationInternValue <= -360)
        {
            ghostRotationInternValue = 0;
        }

        ghostRotation = Quaternion.Euler(0, ghostRotationValue, ghostRotationInternValue);
    }


    //--------------------


    public void placeBuildingBlock()
    {
        if (ghost_isActive && buildingBlockHit != null && directionHit != BuildingBlockColliderDirection.None)
        {
            //Play Sound
            #region Sound
            if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Wood)
            {
                SoundManager.Instance.PlayWood_Placed_Clip();
            }
            else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Stone)
            {
                SoundManager.Instance.PlayStone_Placed_Clip();
            }
            else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Iron)
            {
                SoundManager.Instance.PlayIron_Placed_Clip();
            }
            #endregion

            //Instantiate Block
            buildingBlockList.Add(Instantiate(MoveableObjectManager.Instance.GetBuildingBlock(), SetPosition(buildingBlockHit), ghostRotation) as GameObject);

            //Set Parent of the placed object
            buildingBlockList[buildingBlockList.Count - 1].transform.SetParent(buildingBlock_Parent.transform);

            //Remove items from inventory
            BuildingBlockObject tempParent = MoveableObjectManager.Instance.GetBuildingBlock_SO();
            if (tempParent.craftingRequirements != null)
            {
                for (int i = 0; i < tempParent.craftingRequirements.Count; i++)
                {
                    for (int k = 0; k < tempParent.craftingRequirements[i].amount; k++)
                    {
                        InventoryManager.Instance.RemoveItemFromInventory(0, tempParent.craftingRequirements[i].itemName, -1, false);
                    }
                }
            }

            SaveData();
        }
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