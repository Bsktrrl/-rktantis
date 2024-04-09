using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuildingManager : Singleton<BuildingManager>
{
    public GameObject buildingBlock_Parent;
    public List<GameObject> buildingBlockList = new List<GameObject>();
    [HideInInspector] public List<BuildingBlockSaveList> buildingBlockSaveList = new List<BuildingBlockSaveList>();

    [Header("Ghost")]
    public GameObject Axe_buildingBlockLookingAt;
    public GameObject lastBuildingBlock_LookedAt;
    [HideInInspector] public GameObject old_lastBuildingBlock_LookedAt;
    public GameObject ghost_LookedAt;
    public bool buildingBlockCanBePlaced;

    public GameObject freeGhost_LookedAt;

    public string BlockTagName;
    public GameObject BlockDirection_LookedAt;

    Ray oldRay = new Ray();

    public float rotationValue = 0;

    public Vector2 BuildingDistance;
    public BlockDirection_A blockDirection_X;
    public BlockDirection_B blockDirection_Y;
    public BuildingType buildingType;
    public BuildingSubType buildingSubType;

    #region BuildingBlocks List
    [Header("BuildingBlocks List - Wood")]
    [SerializeField] GameObject builingBlock_Wood_Floor;
    [SerializeField] GameObject builingBlock_Wood_Floor_Triangle;
    [SerializeField] GameObject builingBlock_Wood_Wall;
    [SerializeField] GameObject builingBlock_Wood_Wall_Diagonal;
    [SerializeField] GameObject builingBlock_Wood_Ramp;
    [SerializeField] GameObject builingBlock_Wood_Ramp_Corner;
    [SerializeField] GameObject builingBlock_Wood_Ramp_Triangle;
    [SerializeField] GameObject builingBlock_Wood_Wall_Triangle;
    [SerializeField] GameObject builingBlock_Wood_Fence;
    [SerializeField] GameObject builingBlock_Wood_Fence_Diagonaly;
    [SerializeField] GameObject builingBlock_Wood_Window;
    [SerializeField] GameObject builingBlock_Wood_Door;
    [SerializeField] GameObject builingBlock_Wood_Stair;

    [Header("BuildingBlocks List - Stone")]
    [SerializeField] GameObject builingBlock_Stone_Floor;
    [SerializeField] GameObject builingBlock_Stone_Floor_Triangle;
    [SerializeField] GameObject builingBlock_Stone_Wall;
    [SerializeField] GameObject builingBlock_Stone_Wall_Diagonal;
    [SerializeField] GameObject builingBlock_Stone_Ramp;
    [SerializeField] GameObject builingBlock_Stone_Ramp_Corner;
    [SerializeField] GameObject builingBlock_Stone_Ramp_Triangle;
    [SerializeField] GameObject builingBlock_Stone_Wall_Triangle;
    [SerializeField] GameObject builingBlock_Stone_Fence;
    [SerializeField] GameObject builingBlock_Stone_Fence_Diagonaly;
    [SerializeField] GameObject builingBlock_Stone_Window;
    [SerializeField] GameObject builingBlock_Stone_Door;
    [SerializeField] GameObject builingBlock_Stone_Stair;

    [Header("BuildingBlocks List - Iron")]
    [SerializeField] GameObject builingBlock_Iron_Floor;
    [SerializeField] GameObject builingBlock_Iron_Floor_Triangle;
    [SerializeField] GameObject builingBlock_Iron_Wall;
    [SerializeField] GameObject builingBlock_Iron_Wall_Diagonal;
    [SerializeField] GameObject builingBlock_Iron_Ramp;
    [SerializeField] GameObject builingBlock_Iron_Ramp_Corner;
    [SerializeField] GameObject builingBlock_Iron_Ramp_Triangle;
    [SerializeField] GameObject builingBlock_Iron_Wall_Triangle;
    [SerializeField] GameObject builingBlock_Iron_Fence;
    [SerializeField] GameObject builingBlock_Iron_Fence_Diagonaly;
    [SerializeField] GameObject builingBlock_Iron_Window;
    [SerializeField] GameObject builingBlock_Iron_Door;
    [SerializeField] GameObject builingBlock_Iron_Stair;
    #endregion

    [Header("Materials List")]
    public Material invisible_Material;
    public Material ghost_Material;
    public Material canPlace_Material;
    public Material cannotPlace_Material;

    float timer = 0;

    //When true, shift buildingGhosts available to the mirrored version
    public bool mirroredBlocks;

    Ray ray_Hammer;
    RaycastHit hit_Hammer;

    Ray ray_Axe;
    RaycastHit hit_Axe;

    public bool BuildingHammer_isActive;

    public GameObject buildingRemoveRequirement_Parent;
    public GameObject buildingRequirement_Parent;
    public GameObject buildingRequirement_Prefab;
    public GameObject buildingRequirementHeader_Prefab;
    [SerializeField] List<GameObject> buildingRequirement_List = new List<GameObject>();
    [SerializeField] List<GameObject> buildingRemoveRequirement_List = new List<GameObject>();

    public bool enoughItemsToBuild;
    public GameObject tempBlock_Parent;
    public bool blockisPlacing;

    public List<GameObject> buildingBlock_UIList = new List<GameObject>();

    [Header("RequiermentColors")]
    public Color requirementMetColor;
    public Color requirementNOTMetColor;
    public GameObject selectedMovableObjectInMenu = null;


    //--------------------


    private void Awake()
    {
        MoveableObjectManager.Instance.buildingType_Selected = BuildingType.None;
        MoveableObjectManager.Instance.buildingMaterial_Selected = BuildingMaterial.None;
    }

    private void Start()
    {
        buildingRemoveRequirement_Parent.SetActive(false);
        buildingRequirement_Parent.SetActive(false);
    }

    private void Update()
    {
        if (MainManager.Instance.menuStates == MenuStates.None
            && ((Time.frameCount % MainManager.Instance.updateInterval == 0 && HotbarManager.Instance.selectedItem == Items.WoodBuildingHammer)
            || (Time.frameCount % MainManager.Instance.updateInterval == 0 && HotbarManager.Instance.selectedItem == Items.StoneBuildingHammer)
            || (Time.frameCount % MainManager.Instance.updateInterval == 0 && HotbarManager.Instance.selectedItem == Items.CryoniteBuildingHammer)))
        {
            RaycastSetup_Hammer();

            if (MoveableObjectManager.Instance.moveableObjectType == MoveableObjectType.None)
            {
                buildingRequirement_Parent.SetActive(false);
            }
            else
            {
                buildingRequirement_Parent.SetActive(true);
            }

            buildingRemoveRequirement_Parent.SetActive(false);

            Axe_buildingBlockLookingAt = null;

            if (!BuildingHammer_isActive)
            {
                BuildingHammer_isActive = true;
            }

            //Set BuildingRequirement UI
            if (MainManager.Instance.menuStates == MenuStates.MoveableObjectMenu)
            {
                buildingRequirement_Parent.SetActive(false);
            }
            else if (MainManager.Instance.menuStates == MenuStates.None)
            {
                if (MoveableObjectManager.Instance.moveableObjectType == MoveableObjectType.None)
                {
                    buildingRequirement_Parent.SetActive(false);
                }
                else
                {
                    buildingRequirement_Parent.SetActive(true);
                }
            }
        }
        else if ((Time.frameCount % MainManager.Instance.updateInterval == 0 && HotbarManager.Instance.selectedItem == Items.WoodAxe)
            || (Time.frameCount % MainManager.Instance.updateInterval == 0 && HotbarManager.Instance.selectedItem == Items.StoneAxe)
            || (Time.frameCount % MainManager.Instance.updateInterval == 0 && HotbarManager.Instance.selectedItem == Items.CryoniteAxe))
        {
            RaycastSetup_Axe();

            if (BuildingHammer_isActive)
            {
                BuildingHammer_isActive = false;
                buildingRequirement_Parent.SetActive(false);
                buildingRemoveRequirement_Parent.SetActive(true);
                SetAllGhostState_Off();
                SetAllDirectionObjectState_Off();

                //print("1. Set Ghosts OFF");
            }
        }
        else
        {
            lastBuildingBlock_LookedAt = null;

            if (BuildingHammer_isActive)
            {
                BuildingHammer_isActive = false;
                buildingRequirement_Parent.SetActive(false);
                buildingRemoveRequirement_Parent.SetActive(false);
                SetAllGhostState_Off();
                SetAllDirectionObjectState_Off();

                //print("2. Set Ghosts OFF");
            }
        }
    }

    //--------------------


    public void LoadData()
    {
        //If data has not saved, set to "Wood Floor"
        #region
        if (MoveableObjectManager.Instance.buildingType_Selected == BuildingType.None || MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.None)
        {
            MoveableObjectManager.Instance.buildingType_Selected = BuildingType.Floor;
            MoveableObjectManager.Instance.buildingMaterial_Selected = BuildingMaterial.Wood;
        }
        #endregion

        //Set Preview image for the selected buildingBlock
        #region
        for (int i = 0; i < BuildingSystemMenu.Instance.buildingBlockUIList.Count; i++)
        {
            if (BuildingSystemMenu.Instance.buildingBlockUIList[i].GetComponent<BuildingBlock_UI>().buildingType == MoveableObjectManager.Instance.buildingType_Selected
                && BuildingSystemMenu.Instance.buildingBlockUIList[i].GetComponent<BuildingBlock_UI>().buildingMaterial == MoveableObjectManager.Instance.buildingMaterial_Selected)
            {
                BuildingSystemMenu.Instance.SetSelectedImage(BuildingSystemMenu.Instance.buildingBlockUIList[i].GetComponent<BuildingBlock_UI>().parent.GetComponent<Image>().sprite);

                break;
            }
        }
        #endregion

        //Setup BuildingBlockList
        #region
        for (int i = 0; i < buildingBlockList.Count; i++)
        {
            Destroy(buildingBlockList[i]);
        }
        buildingBlockList.Clear();

        buildingBlockSaveList = DataManager.Instance.buildingBlockList_StoreList;
        for (int i = 0; i < buildingBlockSaveList.Count; i++)
        {
            buildingBlockList.Add(Instantiate(SetupBuildingBlockFromSave(buildingBlockSaveList[i]), buildingBlockSaveList[i].buildingBlock_Position, buildingBlockSaveList[i].buildingBlock_Rotation) as GameObject);
            buildingBlockList[buildingBlockList.Count - 1].transform.parent = buildingBlock_Parent.transform;

            buildingBlockList[buildingBlockList.Count - 1].GetComponent<BuildingBlock_Parent>().blockID = buildingBlockSaveList[i].buildingID;
        }
        #endregion

        //Set Building Requirements
        #region
        SetBuildingRequirements(GetBuildingBlock(MoveableObjectManager.Instance.buildingType_Selected, MoveableObjectManager.Instance.buildingMaterial_Selected), buildingRequirement_Parent);

        if (HotbarManager.Instance.selectedItem == Items.WoodBuildingHammer
            || HotbarManager.Instance.selectedItem == Items.StoneBuildingHammer
            || HotbarManager.Instance.selectedItem == Items.CryoniteBuildingHammer)
        {
            buildingRequirement_Parent.SetActive(true);
            BuildingHammer_isActive = true;
        }
        #endregion
    }
    public void SaveData()
    {
        //Save buildingBlocks into a saveable list
        List<BuildingBlockSaveList> tempList = new List<BuildingBlockSaveList>();
        for (int i = 0; i < buildingBlockList.Count; i++)
        {
            BuildingBlockSaveList temp = new BuildingBlockSaveList();

            temp.buildingID = buildingBlockList[i].GetComponent<BuildingBlock_Parent>().blockID;
            temp.buildingBlock_Position = buildingBlockList[i].transform.position;
            temp.buildingBlock_Rotation = buildingBlockList[i].transform.rotation;

            tempList.Add(temp);
        }
        DataManager.Instance.buildingBlockList_StoreList = tempList;

        print("Save buildingBlocks");
    }


    //--------------------


    void RaycastSetup_Hammer()
    {
        //Only active when not in a menu
        if (!BuildingSystemMenu.Instance.buildingSystemMenu_isOpen)
        {
            RaycastBuildingDirectionMarkers();
        }
        else
        {
            //When BuildingHammer isn't in the hand anymore
            if ((blockDirection_X != BlockDirection_A.None && blockDirection_Y != BlockDirection_B.None)
                || blockDirection_X != BlockDirection_A.None
                || blockDirection_Y != BlockDirection_B.None)
            {
                blockDirection_X = BlockDirection_A.None;
                blockDirection_Y = BlockDirection_B.None;
                SetAllGhostState_Off();

                if (ghost_LookedAt != null)
                {
                    ghost_LookedAt.SetActive(false);
                    ghost_LookedAt = null;
                }
            }
        }
    }
    void RaycastBuildingDirectionMarkers()
    {
        if (MainManager.Instance.menuStates == MenuStates.None)
        {
            ray_Hammer = Camera.main.ScreenPointToRay(Input.mousePosition);
            oldRay = ray_Hammer;
        }

        if (Physics.Raycast(oldRay, out hit_Hammer))
        {
            //Get the Transform of GameObject hit
            var hitTransform = hit_Hammer.transform;

            if (hitTransform.tag != "Player")
            {
                BlockTagName = hitTransform.tag;
            }

            //Check BuidingDirectionMarkers
            if (hitTransform.gameObject.CompareTag("BuidingDirectionMarkers"))
            {
                BlockDirection_LookedAt = hitTransform.gameObject;
            }
            else
            {
                BlockDirection_LookedAt = null;
            }

            if ((hitTransform.gameObject.CompareTag("BuidingDirectionMarkers")
                || hitTransform.gameObject.CompareTag("BuildingBlock_Ghost")
                || hitTransform.gameObject.CompareTag("BuildingBlock"))
                && hit_Hammer.distance > BuildingDistance.x)
            {
                SetAllGhostState_Off();
                return;
            }
            else if ((hitTransform.gameObject.CompareTag("BuidingDirectionMarkers")
                || hitTransform.gameObject.CompareTag("BuildingBlock_Ghost")
                || hitTransform.gameObject.CompareTag("BuildingBlock"))
                && hit_Hammer.distance < BuildingDistance.y)
            {
                SetAllGhostState_Off();
                return;
            }

            //Get the BuildingBlockDirection
            if (hitTransform.gameObject.CompareTag("BuildingBlock"))
            {
                SetAllGhostState_Off();
                lastBuildingBlock_LookedAt = hitTransform.gameObject.GetComponent<BuildingBlock>().buidingBlock_Parent;

                //Show selected DirectionBoxes
                if (lastBuildingBlock_LookedAt != null && lastBuildingBlock_LookedAt != old_lastBuildingBlock_LookedAt)
                {
                    //Activate relevant directionObject from the list
                    for (int i = 0; i < lastBuildingBlock_LookedAt.GetComponent<BuildingBlock_Parent>().directionObjectList.Count; i++)
                    {
                        if (lastBuildingBlock_LookedAt.GetComponent<BuildingBlock_Parent>().directionObjectList[i].GetComponent<BuildingBlockDirection>().BuildingType == MoveableObjectManager.Instance.buildingType_Selected)
                        {
                            lastBuildingBlock_LookedAt.GetComponent<BuildingBlock_Parent>().directionObjectList[i].SetActive(true);
                        }
                        else
                        {
                            lastBuildingBlock_LookedAt.GetComponent<BuildingBlock_Parent>().directionObjectList[i].SetActive(false);
                        }
                    }

                    //Deactivate old directionObjectList
                    if (old_lastBuildingBlock_LookedAt != null)
                    {
                        for (int i = 0; i < old_lastBuildingBlock_LookedAt.GetComponent<BuildingBlock_Parent>().directionObjectList.Count; i++)
                        {
                            if (old_lastBuildingBlock_LookedAt.GetComponent<BuildingBlock_Parent>().directionObjectList[i].activeInHierarchy)
                            {
                                old_lastBuildingBlock_LookedAt.GetComponent<BuildingBlock_Parent>().directionObjectList[i].SetActive(false);
                            }
                        }
                    }
                }

                old_lastBuildingBlock_LookedAt = lastBuildingBlock_LookedAt;
            }

            //Get the BuidingDirectionMarkers
            else if (hitTransform.gameObject.CompareTag("BuidingDirectionMarkers") || hitTransform.gameObject.CompareTag("BuildingBlock_Ghost"))
            {
                if (hitTransform.gameObject.CompareTag("BuidingDirectionMarkers"))
                {
                    //Set parameters based on the block looked at
                    switch (hitTransform.gameObject.GetComponent<BuildingBlockDirection>().blockDirection_A)
                    {
                        case BlockDirection_A.None:
                            break;

                        case BlockDirection_A.North:
                            if (blockDirection_X != BlockDirection_A.North)
                            {
                                blockDirection_X = BlockDirection_A.North;
                            }
                            break;
                        case BlockDirection_A.East:
                            if (blockDirection_X != BlockDirection_A.East)
                            {
                                blockDirection_X = BlockDirection_A.East;
                            }
                            break;
                        case BlockDirection_A.South:
                            if (blockDirection_X != BlockDirection_A.South)
                            {
                                blockDirection_X = BlockDirection_A.South;
                            }
                            break;
                        case BlockDirection_A.West:
                            if (blockDirection_X != BlockDirection_A.West)
                            {
                                blockDirection_X = BlockDirection_A.West;
                            }
                            break;
                        case BlockDirection_A.Cross_A:
                            if (blockDirection_X != BlockDirection_A.Cross_A)
                            {
                                blockDirection_X = BlockDirection_A.Cross_A;
                            }
                            break;
                        case BlockDirection_A.Cross_B:
                            if (blockDirection_X != BlockDirection_A.Cross_B)
                            {
                                blockDirection_X = BlockDirection_A.Cross_B;
                            }
                            break;

                        default:
                            break;
                    }
                    switch (hitTransform.gameObject.GetComponent<BuildingBlockDirection>().blockDirection_B)
                    {
                        case BlockDirection_B.None:
                            break;

                        case BlockDirection_B.Up:
                            if (blockDirection_Y != BlockDirection_B.Up)
                            {
                                blockDirection_Y = BlockDirection_B.Up;
                            }
                            break;
                        case BlockDirection_B.Right:
                            if (blockDirection_Y != BlockDirection_B.Right)
                            {
                                blockDirection_Y = BlockDirection_B.Right;
                            }
                            break;
                        case BlockDirection_B.Down:
                            if (blockDirection_Y != BlockDirection_B.Down)
                            {
                                blockDirection_Y = BlockDirection_B.Down;
                            }
                            break;
                        case BlockDirection_B.Left:
                            if (blockDirection_Y != BlockDirection_B.Left)
                            {
                                blockDirection_Y = BlockDirection_B.Left;
                            }
                            break;

                        default:
                            break;
                    }
                    switch (hitTransform.gameObject.GetComponent<BuildingBlockDirection>().BuildingType)
                    {
                        case BuildingType.None:
                            if (buildingType != BuildingType.None)
                            {
                                buildingType = BuildingType.None;
                            }
                            break;

                        case BuildingType.Floor:
                            if (buildingType != BuildingType.Floor)
                            {
                                buildingType = BuildingType.Floor;
                            }
                            break;
                        case BuildingType.Floor_Triangle:
                            if (buildingType != BuildingType.Floor_Triangle)
                            {
                                buildingType = BuildingType.Floor_Triangle;
                            }
                            break;
                        case BuildingType.Wall:
                            if (buildingType != BuildingType.Wall)
                            {
                                buildingType = BuildingType.Wall;
                            }
                            break;
                        case BuildingType.Wall_Diagonaly:
                            if (buildingType != BuildingType.Wall_Diagonaly)
                            {
                                buildingType = BuildingType.Wall_Diagonaly;
                            }
                            break;
                        case BuildingType.Ramp:
                            if (buildingType != BuildingType.Ramp)
                            {
                                buildingType = BuildingType.Ramp;
                            }
                            break;
                        case BuildingType.Ramp_Corner:
                            if (buildingType != BuildingType.Ramp_Corner)
                            {
                                buildingType = BuildingType.Ramp_Corner;
                            }
                            break;
                        case BuildingType.Wall_Triangle:
                            if (buildingType != BuildingType.Wall_Triangle)
                            {
                                buildingType = BuildingType.Wall_Triangle;
                            }
                            break;
                        case BuildingType.Fence:
                            if (buildingType != BuildingType.Fence)
                            {
                                buildingType = BuildingType.Fence;
                            }
                            break;
                        case BuildingType.Fence_Diagonaly:
                            if (buildingType != BuildingType.Fence_Diagonaly)
                            {
                                buildingType = BuildingType.Fence_Diagonaly;
                            }
                            break;
                        case BuildingType.Window:
                            if (buildingType != BuildingType.Window)
                            {
                                buildingType = BuildingType.Window;
                            }
                            break;
                        case BuildingType.Door:
                            if (buildingType != BuildingType.Door)
                            {
                                buildingType = BuildingType.Door;
                            }
                            break;
                        case BuildingType.Stair:
                            if (buildingType != BuildingType.Stair)
                            {
                                buildingType = BuildingType.Stair;
                            }
                            break;
                        case BuildingType.Ramp_Triangle:
                            if (buildingType != BuildingType.Ramp_Triangle)
                            {
                                buildingType = BuildingType.Ramp_Triangle;
                            }
                            break;

                        default:
                            break;
                    }
                    switch (hitTransform.gameObject.GetComponent<BuildingBlockDirection>().buildingSubType)
                    {
                        case BuildingSubType.None:
                            buildingSubType = BuildingSubType.None;
                            break;
                        case BuildingSubType.Diagonaly:
                            buildingSubType = BuildingSubType.Diagonaly;
                            break;

                        default:
                            break;
                    }

                    //FindGhostDirection(hitTransform.gameObject.GetComponent<BuildingBlockDirection>().parentBlock.GetComponent<BuildingBlock_Parent>());
                    GhostSelection(hitTransform.gameObject.GetComponent<BuildingBlockDirection>().parentBlock.GetComponent<BuildingBlock_Parent>(), buildingType, blockDirection_X, blockDirection_Y, buildingSubType);
                }

                if (hitTransform.gameObject.CompareTag("BuildingBlock"))
                {
                    if (hitTransform.gameObject.GetComponent<BuildingBlock>() != null)
                    {
                        lastBuildingBlock_LookedAt = hitTransform.gameObject.GetComponent<BuildingBlock>().buidingBlock_Parent;
                    }
                }
                else if (hitTransform.gameObject.CompareTag("BuidingDirectionMarkers"))
                {
                    if (hitTransform.gameObject.GetComponent<BuildingBlockDirection>() != null)
                    {
                        lastBuildingBlock_LookedAt = hitTransform.gameObject.GetComponent<BuildingBlockDirection>().parentBlock;
                    }
                }
                else if (hitTransform.gameObject.CompareTag("BuildingBlock_Ghost"))
                {
                    if (hitTransform.gameObject.GetComponent<Building_Ghost>() != null)
                    {
                        lastBuildingBlock_LookedAt = hitTransform.gameObject.GetComponent<Building_Ghost>().blockParent;
                    }
                }
            }
            else
            {
                if (hitTransform.gameObject.CompareTag("BuildingBlock"))
                {
                    if (hitTransform.gameObject.GetComponent<BuildingBlock>() != null)
                    {
                        lastBuildingBlock_LookedAt = hitTransform.gameObject.GetComponent<BuildingBlock>().buidingBlock_Parent;
                    }
                }
                else if (hitTransform.gameObject.CompareTag("BuidingDirectionMarkers"))
                {
                    if (hitTransform.gameObject.GetComponent<BuildingBlockDirection>() != null)
                    {
                        lastBuildingBlock_LookedAt = hitTransform.gameObject.GetComponent<BuildingBlockDirection>().parentBlock;
                    }
                }
                else if (hitTransform.gameObject.CompareTag("BuildingBlock_Ghost"))
                {
                    if (hitTransform.gameObject.GetComponent<Building_Ghost>() != null)
                    {
                        lastBuildingBlock_LookedAt = hitTransform.gameObject.GetComponent<Building_Ghost>().blockParent;
                    }
                }

                //If raycasting isn't on a BuidingDirectionMarkers or ghostBlock
                if ((blockDirection_X != BlockDirection_A.None && blockDirection_Y != BlockDirection_B.None)
                    || blockDirection_X != BlockDirection_A.None
                    || blockDirection_Y != BlockDirection_B.None)
                {
                    blockDirection_X = BlockDirection_A.None;
                    blockDirection_Y = BlockDirection_B.None;
                    SetAllGhostState_Off();

                    if (ghost_LookedAt != null)
                    {
                        ghost_LookedAt.SetActive(false);
                        ghost_LookedAt = null;
                    }
                }
            }
        }
        else
        {
            //When raycast doesn't hit any BuildingObjects
            if ((blockDirection_X != BlockDirection_A.None && blockDirection_Y != BlockDirection_B.None)
                || blockDirection_X != BlockDirection_A.None
                || blockDirection_Y != BlockDirection_B.None)
            {
                blockDirection_X = BlockDirection_A.None;
                blockDirection_Y = BlockDirection_B.None;
                SetAllGhostState_Off();

                if (ghost_LookedAt != null)
                {
                    ghost_LookedAt.SetActive(false);
                    ghost_LookedAt = null;
                }
            }
        }
    }
    void GhostSelection(BuildingBlock_Parent blockLookingAt, BuildingType buildingType, BlockDirection_A blockDirection_A, BlockDirection_B blockDirection_B, BuildingSubType buildingSubType)
    {
        for (int i = 0; i < blockLookingAt.ghostList.Count; i++)
        {
            if (blockLookingAt.ghostList[i].GetComponent<Building_Ghost>().blockDirection_A == blockDirection_A
                && blockLookingAt.ghostList[i].GetComponent<Building_Ghost>().blockDirection_B == blockDirection_B
                && blockLookingAt.ghostList[i].GetComponent<Building_Ghost>().buildingType == buildingType
                && blockLookingAt.ghostList[i].GetComponent<Building_Ghost>().buildingSubType == buildingSubType)
            {
                SetGhostState_ON(blockLookingAt, i);
            }
            else
            {
                SetGhostState_OFF(blockLookingAt, i);
            }
        }
    }

    public void SetGhostState_ON(BuildingBlock_Parent blockLookingAt, int i)
    {
        //Floor
        if (MoveableObjectManager.Instance.buildingType_Selected == BuildingType.Floor)
        {
            //Wood
            BuidingBlockCanBePlacedCheck(blockLookingAt, i, BuildingType.Floor, BuildingSubType.None, canPlace_Material, cannotPlace_Material); //Change Material when Mesh is ready

            //Stone

            //Iron
        }

        //Floor_Triangle
        else if (MoveableObjectManager.Instance.buildingType_Selected == BuildingType.Floor_Triangle)
        {
            //Wood
            BuidingBlockCanBePlacedCheck(blockLookingAt, i, BuildingType.Floor_Triangle, BuildingSubType.None, canPlace_Material, cannotPlace_Material); //Change Material when Mesh is ready

            //Stone


            //Iron


        }

        //Wall_Diagonaly
        else if (MoveableObjectManager.Instance.buildingType_Selected == BuildingType.Wall && (blockLookingAt.buildingSubType == BuildingSubType.Diagonaly || blockDirection_X == BlockDirection_A.Cross_A || blockDirection_X == BlockDirection_A.Cross_B))
        {
            //Wood
            BuidingBlockCanBePlacedCheck(blockLookingAt, i, BuildingType.Wall, BuildingSubType.Diagonaly, canPlace_Material, cannotPlace_Material); //Change Material when Mesh is ready

            //Stone


            //Iron


        }
        
        //Wall
        else if (MoveableObjectManager.Instance.buildingType_Selected == BuildingType.Wall && blockLookingAt.buildingSubType == BuildingSubType.None)
        {
            //Wood
            BuidingBlockCanBePlacedCheck(blockLookingAt, i, BuildingType.Wall, BuildingSubType.None, canPlace_Material, cannotPlace_Material); //Change Material when Mesh is ready

            //Stone


            //Iron


        }

        //Ramp
        else if (MoveableObjectManager.Instance.buildingType_Selected == BuildingType.Ramp)
        {
            //Wood
            BuidingBlockCanBePlacedCheck(blockLookingAt, i, BuildingType.Ramp, BuildingSubType.None, canPlace_Material, cannotPlace_Material); //Change Material when Mesh is ready

            //Stone


            //Iron


        }

        //Ramp_Corner
        else if (MoveableObjectManager.Instance.buildingType_Selected == BuildingType.Ramp_Corner)
        {
            //Wood
            BuidingBlockCanBePlacedCheck(blockLookingAt, i, BuildingType.Ramp_Corner, BuildingSubType.None, canPlace_Material, cannotPlace_Material); //Change Material when Mesh is ready

            //Stone


            //Iron


        }

        //Ramp_Triangle
        else if (MoveableObjectManager.Instance.buildingType_Selected == BuildingType.Ramp_Triangle)
        {
            //Wood
            BuidingBlockCanBePlacedCheck(blockLookingAt, i, BuildingType.Ramp_Triangle, BuildingSubType.None, canPlace_Material, cannotPlace_Material); //Change Material when Mesh is ready

            //Stone


            //Iron


        }

        //Wall_Triangle
        else if (MoveableObjectManager.Instance.buildingType_Selected == BuildingType.Wall_Triangle)
        {
            //Wood
            BuidingBlockCanBePlacedCheck(blockLookingAt, i, BuildingType.Wall_Triangle, BuildingSubType.None, canPlace_Material, cannotPlace_Material); //Change Material when Mesh is ready

            //Stone


            //Iron


        }

        //Fence_Diagonaly
        else if (MoveableObjectManager.Instance.buildingType_Selected == BuildingType.Fence && (blockLookingAt.buildingSubType == BuildingSubType.Diagonaly || blockDirection_X == BlockDirection_A.Cross_A || blockDirection_X == BlockDirection_A.Cross_B))
        {
            //Wood
            BuidingBlockCanBePlacedCheck(blockLookingAt, i, BuildingType.Fence, BuildingSubType.Diagonaly, canPlace_Material, cannotPlace_Material); //Change Material when Mesh is ready

            //Stone


            //Iron


        }
        
        //Fence
        else if (MoveableObjectManager.Instance.buildingType_Selected == BuildingType.Fence && blockLookingAt.buildingSubType == BuildingSubType.None)
        {
            //Wood
            BuidingBlockCanBePlacedCheck(blockLookingAt, i, BuildingType.Fence, BuildingSubType.None, canPlace_Material, cannotPlace_Material); //Change Material when Mesh is ready

            //Stone


            //Iron


        }

        //Window
        else if (MoveableObjectManager.Instance.buildingType_Selected == BuildingType.Window)
        {
            //Wood
            BuidingBlockCanBePlacedCheck(blockLookingAt, i, BuildingType.Window, BuildingSubType.None, canPlace_Material, cannotPlace_Material); //Change Material when Mesh is ready

            //Stone


            //Iron


        }

        //Door
        else if (MoveableObjectManager.Instance.buildingType_Selected == BuildingType.Door)
        {
            //Wood
            BuidingBlockCanBePlacedCheck(blockLookingAt, i, BuildingType.Door, BuildingSubType.None, canPlace_Material, cannotPlace_Material); //Change Material when Mesh is ready

            //Stone


            //Iron


        }

        //Stair
        else if (MoveableObjectManager.Instance.buildingType_Selected == BuildingType.Stair)
        {
            //Wood
            BuidingBlockCanBePlacedCheck(blockLookingAt, i, BuildingType.Stair, BuildingSubType.None, canPlace_Material, cannotPlace_Material); //Change Material when Mesh is ready

            //Stone


            //Iron


        }

        //Turn off
        else
        {
            print("1. Turn OFF");
            SetGhostState_OFF(blockLookingAt, i);
        }
    }
    void SetGhostState_OFF(BuildingBlock_Parent blockLookingAt, int j)
    {
        if (blockLookingAt != null)
        {
            blockLookingAt.ghostList[j].SetActive(false);
            blockLookingAt.ghostList[j].GetComponent<MeshRenderer>().material = invisible_Material;
            blockLookingAt.ghostList[j].GetComponent<Building_Ghost>().isSelected = false;
        }
    }
    public void SetAllGhostState_Off()
    {
        for (int i = 0; i < buildingBlockList.Count; i++)
        {
            for (int j = 0; j < buildingBlockList[i].GetComponent<BuildingBlock_Parent>().ghostList.Count; j++)
            {
                SetGhostState_OFF(buildingBlockList[i].GetComponent<BuildingBlock_Parent>(), j);
            }
        }

        buildingBlockCanBePlaced = false;
        ghost_LookedAt = null;
    }
    void SetDirectionObjectState_OFF(BuildingBlock_Parent blockLookingAt, int j)
    {
        if (blockLookingAt != null)
        {
            blockLookingAt.directionObjectList[j].SetActive(false);
        }
    }
    public void SetAllDirectionObjectState_Off()
    {
        for (int i = 0; i < buildingBlockList.Count; i++)
        {
            for (int j = 0; j < buildingBlockList[i].GetComponent<BuildingBlock_Parent>().directionObjectList.Count; j++)
            {
                SetDirectionObjectState_OFF(buildingBlockList[i].GetComponent<BuildingBlock_Parent>(), j);
            }
        }
    }

    void BuidingBlockCanBePlacedCheck(BuildingBlock_Parent blockLookingAt, int i, BuildingType buildingType, BuildingSubType buildingSubType, Material material_Can, Material material_Cannot)
    {
        if (blockLookingAt.ghostList[i].GetComponent<Building_Ghost>().buildingType != buildingType)
        {
            //print("100000. BuildingBlock return without setting a Ghost");

            return;
        }

        //print("100001. BuildingBlock return with a Ghost - Success");

        //Reset all ghost before setting a new one
        if (true /*ghost_LookedAt != blockLookingAt.ghostList[i]*/)
        {
            SetAllGhostState_Off();
            ghost_LookedAt = blockLookingAt.ghostList[i];
        }

        //Prevent glitching BuildingBlock ghosts
        if (buildingBlockCanBePlaced)
        {
            timer = 0.2f;
        }
        else
        {
            timer += Time.smoothDeltaTime;
        }

        //Can be placed
        if (/*!CheckOverlappingGhost() &&*/ timer >= 0.2f)
        {
            blockLookingAt.ghostList[i].GetComponent<MeshFilter>().mesh = GetCorrectGhostMesh(ghost_LookedAt);

            //Check if item can be placed
            if (enoughItemsToBuild)
            {
                blockLookingAt.ghostList[i].GetComponent<MeshRenderer>().material = material_Can;
            }
            else
            {
                blockLookingAt.ghostList[i].GetComponent<MeshRenderer>().material = material_Cannot;
            }

            blockLookingAt.ghostList[i].GetComponent<Building_Ghost>().isSelected = true;
            blockLookingAt.ghostList[i].SetActive(true);
            buildingBlockCanBePlaced = true;
        }

        else if (timer >= 0.2f)
        {
            timer = 0;
        }
        //Cannot be placed
        else
        {
            buildingBlockCanBePlaced = false;
            SetAllGhostState_Off();
        }
    }
    public Mesh GetCorrectGhostMesh(GameObject ghost_LookedAt)
    {
        Mesh chosenMesh = new Mesh();

        //Get correct Mesh on a ghost based on the selected material
        //Wood
        if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Wood)
        {
            if (ghost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Door)
                chosenMesh = builingBlock_Wood_Door.GetComponent<BuildingBlock_Parent>().BuildingBlock.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
            else if (ghost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Fence && buildingSubType == BuildingSubType.None)
                chosenMesh = builingBlock_Wood_Fence.GetComponent<BuildingBlock_Parent>().BuildingBlock.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
            else if (ghost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Fence && buildingSubType == BuildingSubType.Diagonaly)
                chosenMesh = builingBlock_Wood_Fence_Diagonaly.GetComponent<BuildingBlock_Parent>().BuildingBlock.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
            else if(ghost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Floor)
                chosenMesh = builingBlock_Wood_Floor.GetComponent<BuildingBlock_Parent>().BuildingBlock.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
            else if (ghost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Floor_Triangle)
                chosenMesh = builingBlock_Wood_Floor_Triangle.GetComponent<BuildingBlock_Parent>().BuildingBlock.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
            else if (ghost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Ramp)
                chosenMesh = builingBlock_Wood_Ramp.GetComponent<BuildingBlock_Parent>().BuildingBlock.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
            
            else if (ghost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Ramp_Triangle)
                chosenMesh = builingBlock_Wood_Ramp_Triangle.GetComponent<BuildingBlock_Parent>().BuildingBlock.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
            
            else if (ghost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Ramp_Corner)
                chosenMesh = builingBlock_Wood_Ramp_Corner.GetComponent<BuildingBlock_Parent>().BuildingBlock.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
            else if (ghost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Stair)
                chosenMesh = builingBlock_Wood_Stair.GetComponent<BuildingBlock_Parent>().BuildingBlock.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
            else if (ghost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Wall && buildingSubType == BuildingSubType.None)
                chosenMesh = builingBlock_Wood_Wall.GetComponent<BuildingBlock_Parent>().BuildingBlock.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
            else if (ghost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Wall && buildingSubType == BuildingSubType.Diagonaly)
                chosenMesh = builingBlock_Wood_Wall_Diagonal.GetComponent<BuildingBlock_Parent>().BuildingBlock.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
            else if (ghost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Wall_Triangle)
                chosenMesh = builingBlock_Wood_Wall_Triangle.GetComponent<BuildingBlock_Parent>().BuildingBlock.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
            else if (ghost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Window)
                chosenMesh = builingBlock_Wood_Window.GetComponent<BuildingBlock_Parent>().BuildingBlock.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;

            else
                chosenMesh = null;
        }

        //Stone
        else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Stone)
        {
            if (ghost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Door)
                chosenMesh = builingBlock_Stone_Door.GetComponent<BuildingBlock_Parent>().BuildingBlock.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
            else if (ghost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Fence && buildingSubType == BuildingSubType.None)
                chosenMesh = builingBlock_Stone_Fence.GetComponent<BuildingBlock_Parent>().BuildingBlock.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
            else if (ghost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Fence && buildingSubType == BuildingSubType.Diagonaly)
                chosenMesh = builingBlock_Stone_Fence_Diagonaly.GetComponent<BuildingBlock_Parent>().BuildingBlock.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
            else if(ghost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Floor)
                chosenMesh = builingBlock_Stone_Floor.GetComponent<BuildingBlock_Parent>().BuildingBlock.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
            else if (ghost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Floor_Triangle)
                chosenMesh = builingBlock_Stone_Floor_Triangle.GetComponent<BuildingBlock_Parent>().BuildingBlock.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
            else if (ghost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Ramp)
                chosenMesh = builingBlock_Stone_Ramp.GetComponent<BuildingBlock_Parent>().BuildingBlock.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
            
            else if (ghost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Ramp_Triangle)
                chosenMesh = builingBlock_Stone_Ramp_Triangle.GetComponent<BuildingBlock_Parent>().BuildingBlock.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
            
            else if (ghost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Ramp_Corner)
                chosenMesh = builingBlock_Stone_Ramp_Corner.GetComponent<BuildingBlock_Parent>().BuildingBlock.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
            else if (ghost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Stair)
                chosenMesh = builingBlock_Stone_Stair.GetComponent<BuildingBlock_Parent>().BuildingBlock.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
            else if (ghost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Wall && buildingSubType == BuildingSubType.None)
                chosenMesh = builingBlock_Stone_Wall.GetComponent<BuildingBlock_Parent>().BuildingBlock.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
            else if (ghost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Wall && buildingSubType == BuildingSubType.Diagonaly)
                chosenMesh = builingBlock_Stone_Wall_Diagonal.GetComponent<BuildingBlock_Parent>().BuildingBlock.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
            else if (ghost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Wall_Triangle)
                chosenMesh = builingBlock_Stone_Wall_Triangle.GetComponent<BuildingBlock_Parent>().BuildingBlock.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
            else if (ghost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Window)
                chosenMesh = builingBlock_Stone_Window.GetComponent<BuildingBlock_Parent>().BuildingBlock.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;

            else
                chosenMesh = null;
        }

        //Iron
        else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Iron)
        {
            if (ghost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Door)
                chosenMesh = builingBlock_Iron_Door.GetComponent<BuildingBlock_Parent>().BuildingBlock.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
            else if (ghost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Fence && buildingSubType == BuildingSubType.None)
                chosenMesh = builingBlock_Iron_Fence.GetComponent<BuildingBlock_Parent>().BuildingBlock.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
            else if (ghost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Fence && buildingSubType == BuildingSubType.Diagonaly)
                chosenMesh = builingBlock_Iron_Fence_Diagonaly.GetComponent<BuildingBlock_Parent>().BuildingBlock.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
            else if(ghost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Floor)
                chosenMesh = builingBlock_Iron_Floor.GetComponent<BuildingBlock_Parent>().BuildingBlock.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
            else if (ghost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Floor_Triangle)
                chosenMesh = builingBlock_Iron_Floor_Triangle.GetComponent<BuildingBlock_Parent>().BuildingBlock.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
            else if (ghost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Ramp)
                chosenMesh = builingBlock_Iron_Ramp.GetComponent<BuildingBlock_Parent>().BuildingBlock.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
            
            else if (ghost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Ramp_Triangle)
                chosenMesh = builingBlock_Iron_Ramp_Triangle.GetComponent<BuildingBlock_Parent>().BuildingBlock.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
            
            else if (ghost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Ramp_Corner)
                chosenMesh = builingBlock_Iron_Ramp_Corner.GetComponent<BuildingBlock_Parent>().BuildingBlock.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
            else if (ghost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Stair)
                chosenMesh = builingBlock_Iron_Stair.GetComponent<BuildingBlock_Parent>().BuildingBlock.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
            else if (ghost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Wall && buildingSubType == BuildingSubType.None)
                chosenMesh = builingBlock_Iron_Wall.GetComponent<BuildingBlock_Parent>().BuildingBlock.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
            else if (ghost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Wall && buildingSubType == BuildingSubType.Diagonaly)
                chosenMesh = builingBlock_Iron_Wall_Diagonal.GetComponent<BuildingBlock_Parent>().BuildingBlock.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
            else if (ghost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Wall_Triangle)
                chosenMesh = builingBlock_Iron_Wall_Triangle.GetComponent<BuildingBlock_Parent>().BuildingBlock.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
            else if (ghost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Window)
                chosenMesh = builingBlock_Iron_Window.GetComponent<BuildingBlock_Parent>().BuildingBlock.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;

            else
                chosenMesh = null;
        }

        return chosenMesh;
    }


    //--------------------


    public void PlaceBlock()
    {
        //If you place a BuildingBlock
        if (ghost_LookedAt != null && buildingBlockCanBePlaced)
        {
            //If buildingBlock can be placed
            if (enoughItemsToBuild)
            {
                print("1. Place Block");

                //Play Sound
                if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Wood)
                {
                    SoundManager.Instance.Play_Building_Place_Wood_Clip();
                }
                else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Stone)
                {
                    SoundManager.Instance.Play_Building_Place_Stone_Clip();
                }
                else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Iron)
                {
                    SoundManager.Instance.Play_Building_Place_Cryonite_Clip();
                }

                //SetRotation of BuildingBlock
                Quaternion rotation = new Quaternion(ghost_LookedAt.transform.rotation.x, ghost_LookedAt.transform.rotation.y, ghost_LookedAt.transform.rotation.z, ghost_LookedAt.transform.rotation.w);

                #region Place correct BuildingBlock and Material
                //Floor
                if (ghost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Floor && ghost_LookedAt.GetComponent<Building_Ghost>().isSelected)
                {
                    //Wood
                    if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Wood)
                    {
                        //print("Placed: Wood Floor");
                        buildingBlockList.Add(Instantiate(builingBlock_Wood_Floor, ghost_LookedAt.transform.position, rotation) as GameObject);
                    }

                    //Stone
                    else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Stone)
                    {
                        //print("Placed: Stone Floor");
                        buildingBlockList.Add(Instantiate(builingBlock_Stone_Floor, ghost_LookedAt.transform.position, rotation) as GameObject);

                    }

                    //Iron
                    else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Iron)
                    {
                        //print("Placed: Iron Floor");
                        buildingBlockList.Add(Instantiate(builingBlock_Iron_Floor, ghost_LookedAt.transform.position, rotation) as GameObject);

                    }
                }

                //Floor - Triangle
                else if (ghost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Floor_Triangle && ghost_LookedAt.GetComponent<Building_Ghost>().isSelected)
                {
                    //Wood
                    if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Wood)
                    {
                        //print("Placed: Wood Triangle");
                        buildingBlockList.Add(Instantiate(builingBlock_Wood_Floor_Triangle, ghost_LookedAt.transform.position, rotation) as GameObject);
                    }

                    //Stone
                    else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Stone)
                    {
                        //print("Placed: Stone Triangle");
                        buildingBlockList.Add(Instantiate(builingBlock_Stone_Floor_Triangle, ghost_LookedAt.transform.position, rotation) as GameObject);

                    }

                    //Iron
                    else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Iron)
                    {
                        //print("Placed: Iron Triangle");
                        buildingBlockList.Add(Instantiate(builingBlock_Iron_Floor_Triangle, ghost_LookedAt.transform.position, rotation) as GameObject);

                    }
                }

                //Wall_Diagonal
                else if (ghost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Wall && ghost_LookedAt.GetComponent<Building_Ghost>().buildingSubType == BuildingSubType.Diagonaly && ghost_LookedAt.GetComponent<Building_Ghost>().isSelected)
                {
                    BlockDirection_A a = ghost_LookedAt.GetComponent<Building_Ghost>().blockDirection_A;
                    BlockDirection_B b = ghost_LookedAt.GetComponent<Building_Ghost>().blockDirection_B;

                    if ((a == BlockDirection_A.North && b == BlockDirection_B.Left) || (a == BlockDirection_A.South && b == BlockDirection_B.Left) || (a == BlockDirection_A.North && b == BlockDirection_B.Right) || (a == BlockDirection_A.South && b == BlockDirection_B.Right))
                    {
                        //Wood
                        if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Wood)
                        {
                            //print("Placed: Wood Wall_Diagonal");
                            buildingBlockList.Add(Instantiate(builingBlock_Wood_Wall, ghost_LookedAt.transform.position, rotation) as GameObject);
                        }

                        //Stone
                        else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Stone)
                        {
                            //print("Placed: Stone Wall_Diagonal");
                            buildingBlockList.Add(Instantiate(builingBlock_Stone_Wall, ghost_LookedAt.transform.position, rotation) as GameObject);

                        }

                        //Iron
                        else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Iron)
                        {
                            //print("Placed: Iron Wall_Diagonal");
                            buildingBlockList.Add(Instantiate(builingBlock_Iron_Wall, ghost_LookedAt.transform.position, rotation) as GameObject);

                        }
                    }
                    else
                    {
                        //Wood
                        if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Wood)
                        {
                            //print("Placed: Wood Wall_Diagonal");
                            buildingBlockList.Add(Instantiate(builingBlock_Wood_Wall_Diagonal, ghost_LookedAt.transform.position, rotation) as GameObject);
                        }

                        //Stone
                        else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Stone)
                        {
                            //print("Placed: Stone Wall_Diagonal");
                            buildingBlockList.Add(Instantiate(builingBlock_Stone_Wall_Diagonal, ghost_LookedAt.transform.position, rotation) as GameObject);

                        }

                        //Iron
                        else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Iron)
                        {
                            //print("Placed: Iron Wall_Diagonal");
                            buildingBlockList.Add(Instantiate(builingBlock_Iron_Wall_Diagonal, ghost_LookedAt.transform.position, rotation) as GameObject);
                        }
                    }
                }

                //Wall
                else if (ghost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Wall && ghost_LookedAt.GetComponent<Building_Ghost>().buildingSubType == BuildingSubType.None && ghost_LookedAt.GetComponent<Building_Ghost>().isSelected)
                {
                    //Wood
                    if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Wood)
                    {
                        //print("Placed: Wood Wall");
                        buildingBlockList.Add(Instantiate(builingBlock_Wood_Wall, ghost_LookedAt.transform.position, rotation) as GameObject);
                    }

                    //Stone
                    else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Stone)
                    {
                        //print("Placed: Stone Wall");
                        buildingBlockList.Add(Instantiate(builingBlock_Stone_Wall, ghost_LookedAt.transform.position, rotation) as GameObject);
                    }

                    //Iron
                    else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Iron)
                    {
                        //print("Placed: Iron Wall");
                        buildingBlockList.Add(Instantiate(builingBlock_Iron_Wall, ghost_LookedAt.transform.position, rotation) as GameObject);
                    }
                }

                //Ramp
                else if (ghost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Ramp && ghost_LookedAt.GetComponent<Building_Ghost>().isSelected)
                {
                    //Wood
                    if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Wood)
                    {
                        //print("Placed: Wood Angle");
                        buildingBlockList.Add(Instantiate(builingBlock_Wood_Ramp, ghost_LookedAt.transform.position, rotation) as GameObject);
                    }

                    //Stone
                    else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Stone)
                    {
                        //print("Placed: Stone Angle");
                        buildingBlockList.Add(Instantiate(builingBlock_Stone_Ramp, ghost_LookedAt.transform.position, rotation) as GameObject);
                    }

                    //Iron
                    else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Iron)
                    {
                        //print("Placed: Iron Angle");
                        buildingBlockList.Add(Instantiate(builingBlock_Iron_Ramp, ghost_LookedAt.transform.position, rotation) as GameObject);
                    }
                }

                //Ramp_Corner
                else if (ghost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Ramp_Corner && ghost_LookedAt.GetComponent<Building_Ghost>().isSelected)
                {
                    //Wood
                    if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Wood)
                    {
                        //print("Placed: Wood Angle");
                        buildingBlockList.Add(Instantiate(builingBlock_Wood_Ramp_Corner, ghost_LookedAt.transform.position, rotation) as GameObject);
                    }

                    //Stone
                    else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Stone)
                    {
                        //print("Placed: Stone Angle");
                        buildingBlockList.Add(Instantiate(builingBlock_Stone_Ramp_Corner, ghost_LookedAt.transform.position, rotation) as GameObject);
                    }

                    //Iron
                    else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Iron)
                    {
                        //print("Placed: Iron Angle");
                        buildingBlockList.Add(Instantiate(builingBlock_Iron_Ramp_Corner, ghost_LookedAt.transform.position, rotation) as GameObject);
                    }
                }

                //Ramp_Triangle
                else if (ghost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Ramp_Triangle && ghost_LookedAt.GetComponent<Building_Ghost>().isSelected)
                {
                    //Wood
                    if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Wood)
                    {
                        //print("Placed: Wood Angle");
                        buildingBlockList.Add(Instantiate(builingBlock_Wood_Ramp_Triangle, ghost_LookedAt.transform.position, rotation) as GameObject);
                    }

                    //Stone
                    else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Stone)
                    {
                        //print("Placed: Stone Angle");
                        buildingBlockList.Add(Instantiate(builingBlock_Stone_Ramp_Triangle, ghost_LookedAt.transform.position, rotation) as GameObject);
                    }

                    //Iron
                    else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Iron)
                    {
                        //print("Placed: Iron Angle");
                        buildingBlockList.Add(Instantiate(builingBlock_Iron_Ramp_Triangle, ghost_LookedAt.transform.position, rotation) as GameObject);
                    }
                }

                //Wall_Triangle
                else if (ghost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Wall_Triangle && ghost_LookedAt.GetComponent<Building_Ghost>().isSelected)
                {
                    //Wood
                    if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Wood)
                    {
                        //print("Placed: Wood Angle");
                        buildingBlockList.Add(Instantiate(builingBlock_Wood_Wall_Triangle, ghost_LookedAt.transform.position, rotation) as GameObject);
                    }

                    //Stone
                    else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Stone)
                    {
                        //print("Placed: Stone Angle");
                        buildingBlockList.Add(Instantiate(builingBlock_Stone_Wall_Triangle, ghost_LookedAt.transform.position, rotation) as GameObject);
                    }

                    //Iron
                    else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Iron)
                    {
                        //print("Placed: Iron Angle");
                        buildingBlockList.Add(Instantiate(builingBlock_Iron_Wall_Triangle, ghost_LookedAt.transform.position, rotation) as GameObject);
                    }
                }

                //Fence_Diagonal
                else if (ghost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Fence && ghost_LookedAt.GetComponent<Building_Ghost>().buildingSubType == BuildingSubType.Diagonaly && ghost_LookedAt.GetComponent<Building_Ghost>().isSelected)
                {
                    BlockDirection_A a = ghost_LookedAt.GetComponent<Building_Ghost>().blockDirection_A;
                    BlockDirection_B b = ghost_LookedAt.GetComponent<Building_Ghost>().blockDirection_B;

                    if ((a == BlockDirection_A.North && b == BlockDirection_B.Left) || (a == BlockDirection_A.South && b == BlockDirection_B.Left) || (a == BlockDirection_A.North && b == BlockDirection_B.Right) || (a == BlockDirection_A.South && b == BlockDirection_B.Right))
                    {
                        //Wood
                        if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Wood)
                        {
                            //print("Placed: Wood Fence_Diagonal");
                            buildingBlockList.Add(Instantiate(builingBlock_Wood_Fence, ghost_LookedAt.transform.position, rotation) as GameObject);
                        }

                        //Stone
                        else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Stone)
                        {
                            //print("Placed: Stone Fence_Diagonal");
                            buildingBlockList.Add(Instantiate(builingBlock_Stone_Fence, ghost_LookedAt.transform.position, rotation) as GameObject);
                        }

                        //Iron
                        else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Iron)
                        {
                            //print("Placed: Iron Fence_Diagonal");
                            buildingBlockList.Add(Instantiate(builingBlock_Iron_Fence, ghost_LookedAt.transform.position, rotation) as GameObject);
                        }
                    }
                    else
                    {
                        //Wood
                        if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Wood)
                        {
                            //print("Placed: Wood Fence_Diagonal");
                            buildingBlockList.Add(Instantiate(builingBlock_Wood_Fence_Diagonaly, ghost_LookedAt.transform.position, rotation) as GameObject);
                        }

                        //Stone
                        else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Stone)
                        {
                            //print("Placed: Stone Fence_Diagonal");
                            buildingBlockList.Add(Instantiate(builingBlock_Stone_Fence_Diagonaly, ghost_LookedAt.transform.position, rotation) as GameObject);
                        }

                        //Iron
                        else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Iron)
                        {
                            //print("Placed: Iron Fence_Diagonal");
                            buildingBlockList.Add(Instantiate(builingBlock_Iron_Fence_Diagonaly, ghost_LookedAt.transform.position, rotation) as GameObject);
                        }
                    }
                }

                //Fence
                else if (ghost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Fence && ghost_LookedAt.GetComponent<Building_Ghost>().buildingSubType == BuildingSubType.None && ghost_LookedAt.GetComponent<Building_Ghost>().isSelected)
                {
                    //Wood
                    if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Wood)
                    {
                        //print("Placed: Wood Angle");
                        buildingBlockList.Add(Instantiate(builingBlock_Wood_Fence, ghost_LookedAt.transform.position, rotation) as GameObject);
                    }

                    //Stone
                    else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Stone)
                    {
                        //print("Placed: Stone Angle");
                        buildingBlockList.Add(Instantiate(builingBlock_Stone_Fence, ghost_LookedAt.transform.position, rotation) as GameObject);
                    }

                    //Iron
                    else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Iron)
                    {
                        //print("Placed: Iron Angle");
                        buildingBlockList.Add(Instantiate(builingBlock_Iron_Fence, ghost_LookedAt.transform.position, rotation) as GameObject);
                    }
                }

                //Window
                else if (ghost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Window && ghost_LookedAt.GetComponent<Building_Ghost>().isSelected)
                {
                    //Wood
                    if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Wood)
                    {
                        //print("Placed: Wood Angle");
                        buildingBlockList.Add(Instantiate(builingBlock_Wood_Window, ghost_LookedAt.transform.position, rotation) as GameObject);
                    }

                    //Stone
                    else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Stone)
                    {
                        //print("Placed: Stone Angle");
                        buildingBlockList.Add(Instantiate(builingBlock_Stone_Window, ghost_LookedAt.transform.position, rotation) as GameObject);
                    }

                    //Iron
                    else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Iron)
                    {
                        //print("Placed: Iron Angle");
                        buildingBlockList.Add(Instantiate(builingBlock_Iron_Window, ghost_LookedAt.transform.position, rotation) as GameObject);
                    }
                }

                //Door
                else if (ghost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Door && ghost_LookedAt.GetComponent<Building_Ghost>().isSelected)
                {
                    //Wood
                    if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Wood)
                    {
                        //print("Placed: Wood Angle");
                        buildingBlockList.Add(Instantiate(builingBlock_Wood_Door, ghost_LookedAt.transform.position, rotation) as GameObject);
                    }

                    //Stone
                    else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Stone)
                    {
                        //print("Placed: Stone Angle");
                        buildingBlockList.Add(Instantiate(builingBlock_Stone_Door, ghost_LookedAt.transform.position, rotation) as GameObject);
                    }

                    //Iron
                    else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Iron)
                    {
                        //print("Placed: Iron Angle");
                        buildingBlockList.Add(Instantiate(builingBlock_Iron_Door, ghost_LookedAt.transform.position, rotation) as GameObject);
                    }
                }

                //Stair
                else if (ghost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Stair && ghost_LookedAt.GetComponent<Building_Ghost>().isSelected)
                {
                    //Wood
                    if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Wood)
                    {
                        //print("Placed: Wood Angle");
                        buildingBlockList.Add(Instantiate(builingBlock_Wood_Stair, ghost_LookedAt.transform.position, rotation) as GameObject);
                    }

                    //Stone
                    else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Stone)
                    {
                        //print("Placed: Stone Angle");
                        buildingBlockList.Add(Instantiate(builingBlock_Stone_Stair, ghost_LookedAt.transform.position, rotation) as GameObject);
                    }

                    //Iron
                    else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Iron)
                    {
                        //print("Placed: Iron Angle");
                        buildingBlockList.Add(Instantiate(builingBlock_Iron_Stair, ghost_LookedAt.transform.position, rotation) as GameObject);
                    }
                }
                #endregion

                //Set Parent of the placed object
                buildingBlockList[buildingBlockList.Count - 1].transform.SetParent(buildingBlock_Parent.transform);

                #region Setup the Placed Block
                //Set info on the Placed Block
                BlockPlaced blockPlaced = new BlockPlaced();
                blockPlaced.buildingBlock = lastBuildingBlock_LookedAt;
                blockPlaced.buildingType = lastBuildingBlock_LookedAt.GetComponent<BuildingBlock_Parent>().buildingType;
                blockPlaced.buildingSubType = lastBuildingBlock_LookedAt.GetComponent<BuildingBlock_Parent>().buildingSubType;
                #endregion
                #region Setup the Block that got a Block Placed on It
                //Set info on the Block that got a block placed on it
                BlockPlaced blockGotPlacedOn = new BlockPlaced();
                blockGotPlacedOn.buildingBlock = buildingBlockList[buildingBlockList.Count - 1];
                blockGotPlacedOn.buildingType = MoveableObjectManager.Instance.buildingType_Selected;
                blockGotPlacedOn.buildingSubType = buildingBlockList[buildingBlockList.Count - 1].GetComponent<BuildingBlock_Parent>().buildingSubType;
                #endregion

                //Remove items from inventory
                BuildingBlock_Parent tempParent = GetBuildingBlock(MoveableObjectManager.Instance.buildingType_Selected, MoveableObjectManager.Instance.buildingMaterial_Selected);
                if (tempParent.buildingRequirementList != null)
                {
                    for (int i = 0; i < tempParent.buildingRequirementList.Count; i++)
                    {
                        for (int k = 0; k < tempParent.buildingRequirementList[i].amount; k++)
                        {
                            InventoryManager.Instance.RemoveItemFromInventory(0, tempParent.buildingRequirementList[i].itemName, -1, false);
                        }
                    }
                }
                
                //Update the Hotbar
                //InventoryManager.Instance.CheckHotbarItemInInventory();
                InventoryManager.Instance.RemoveInventoriesUI();

                //Reset parameters
                lastBuildingBlock_LookedAt = null;
                buildingBlockCanBePlaced = false;
                SetAllGhostState_Off();

                SaveData();
            }

            //If buildingBlock cannot be placed
            else
            {
                print("1. Don't Place Block");

                //Play Sound
                if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Wood)
                {
                    SoundManager.Instance.Play_Building_CannotPlaceBlock_Clip();
                }
            }
        }
        
        //If you place a freeBuildingBlock
        else if (freeGhost_LookedAt)
        {
            //If buildingBlock can be placed
            if (enoughItemsToBuild)
            {
                print("2. Place Block");

                blockisPlacing = true;

                //Play Sound
                if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Wood)
                {
                    SoundManager.Instance.Play_Building_Place_Wood_Clip();
                }
                else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Stone)
                {
                    SoundManager.Instance.Play_Building_Place_Stone_Clip();
                }
                else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Iron)
                {
                    SoundManager.Instance.Play_Building_Place_Cryonite_Clip();
                }

                //SetRotation of BuildingBlock
                Quaternion rotation = freeGhost_LookedAt.transform.rotation; /*new Quaternion(freeGhost_LookedAt.transform.rotation.x, freeGhost_LookedAt.transform.rotation.y, freeGhost_LookedAt.transform.rotation.z, freeGhost_LookedAt.transform.rotation.w);*/

                #region Place correct BuildingBlock and Material
                //Floor
                if (freeGhost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Floor)
                {
                    //Wood
                    if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Wood)
                    {
                        //print("Placed: Wood Floor");
                        buildingBlockList.Add(Instantiate(builingBlock_Wood_Floor, freeGhost_LookedAt.transform.position, rotation) as GameObject);
                    }

                    //Stone
                    else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Stone)
                    {
                        //print("Placed: Stone Floor");
                        buildingBlockList.Add(Instantiate(builingBlock_Stone_Floor, freeGhost_LookedAt.transform.position, rotation) as GameObject);

                    }

                    //Iron
                    else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Iron)
                    {
                        //print("Placed: Iron Floor");
                        buildingBlockList.Add(Instantiate(builingBlock_Iron_Floor, freeGhost_LookedAt.transform.position, rotation) as GameObject);

                    }
                }

                //Floor - Triangle
                else if (freeGhost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Floor_Triangle)
                {
                    //Wood
                    if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Wood)
                    {
                        //print("Placed: Wood Triangle");
                        buildingBlockList.Add(Instantiate(builingBlock_Wood_Floor_Triangle, freeGhost_LookedAt.transform.position, rotation) as GameObject);
                    }

                    //Stone
                    else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Stone)
                    {
                        //print("Placed: Stone Triangle");
                        buildingBlockList.Add(Instantiate(builingBlock_Stone_Floor_Triangle, freeGhost_LookedAt.transform.position, rotation) as GameObject);

                    }

                    //Iron
                    else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Iron)
                    {
                        //print("Placed: Iron Triangle");
                        buildingBlockList.Add(Instantiate(builingBlock_Iron_Floor_Triangle, freeGhost_LookedAt.transform.position, rotation) as GameObject);

                    }
                }

                //Wall_Diagonal
                else if (freeGhost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Wall && freeGhost_LookedAt.GetComponent<Building_Ghost>().buildingSubType == BuildingSubType.Diagonaly)
                {
                    BlockDirection_A a = freeGhost_LookedAt.GetComponent<Building_Ghost>().blockDirection_A;
                    BlockDirection_B b = freeGhost_LookedAt.GetComponent<Building_Ghost>().blockDirection_B;

                    if ((a == BlockDirection_A.North && b == BlockDirection_B.Left) || (a == BlockDirection_A.South && b == BlockDirection_B.Left) || (a == BlockDirection_A.North && b == BlockDirection_B.Right) || (a == BlockDirection_A.South && b == BlockDirection_B.Right))
                    {
                        //Wood
                        if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Wood)
                        {
                            //print("Placed: Wood Wall_Diagonal");
                            buildingBlockList.Add(Instantiate(builingBlock_Wood_Wall, freeGhost_LookedAt.transform.position, rotation) as GameObject);
                        }

                        //Stone
                        else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Stone)
                        {
                            //print("Placed: Stone Wall_Diagonal");
                            buildingBlockList.Add(Instantiate(builingBlock_Stone_Wall, freeGhost_LookedAt.transform.position, rotation) as GameObject);

                        }

                        //Iron
                        else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Iron)
                        {
                            //print("Placed: Iron Wall_Diagonal");
                            buildingBlockList.Add(Instantiate(builingBlock_Iron_Wall, freeGhost_LookedAt.transform.position, rotation) as GameObject);

                        }
                    }
                    else
                    {
                        //Wood
                        if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Wood)
                        {
                            //print("Placed: Wood Wall_Diagonal");
                            buildingBlockList.Add(Instantiate(builingBlock_Wood_Wall_Diagonal, freeGhost_LookedAt.transform.position, rotation) as GameObject);
                        }

                        //Stone
                        else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Stone)
                        {
                            //print("Placed: Stone Wall_Diagonal");
                            buildingBlockList.Add(Instantiate(builingBlock_Stone_Wall_Diagonal, freeGhost_LookedAt.transform.position, rotation) as GameObject);

                        }

                        //Iron
                        else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Iron)
                        {
                            //print("Placed: Iron Wall_Diagonal");
                            buildingBlockList.Add(Instantiate(builingBlock_Iron_Wall_Diagonal, freeGhost_LookedAt.transform.position, rotation) as GameObject);
                        }
                    }
                }

                //Wall
                else if (freeGhost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Wall && freeGhost_LookedAt.GetComponent<Building_Ghost>().buildingSubType == BuildingSubType.None)
                {
                    //Wood
                    if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Wood)
                    {
                        //print("Placed: Wood Wall");
                        buildingBlockList.Add(Instantiate(builingBlock_Wood_Wall, freeGhost_LookedAt.transform.position, rotation) as GameObject);
                    }

                    //Stone
                    else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Stone)
                    {
                        //print("Placed: Stone Wall");
                        buildingBlockList.Add(Instantiate(builingBlock_Stone_Wall, freeGhost_LookedAt.transform.position, rotation) as GameObject);
                    }

                    //Iron
                    else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Iron)
                    {
                        //print("Placed: Iron Wall");
                        buildingBlockList.Add(Instantiate(builingBlock_Iron_Wall, freeGhost_LookedAt.transform.position, rotation) as GameObject);
                    }
                }

                //Ramp
                else if (freeGhost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Ramp)
                {
                    //Wood
                    if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Wood)
                    {
                        //print("Placed: Wood Angle");
                        buildingBlockList.Add(Instantiate(builingBlock_Wood_Ramp, freeGhost_LookedAt.transform.position, rotation) as GameObject);
                    }

                    //Stone
                    else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Stone)
                    {
                        //print("Placed: Stone Angle");
                        buildingBlockList.Add(Instantiate(builingBlock_Stone_Ramp, freeGhost_LookedAt.transform.position, rotation) as GameObject);
                    }

                    //Iron
                    else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Iron)
                    {
                        //print("Placed: Iron Angle");
                        buildingBlockList.Add(Instantiate(builingBlock_Iron_Ramp, freeGhost_LookedAt.transform.position, rotation) as GameObject);
                    }
                }

                //Ramp_Corner
                else if (freeGhost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Ramp_Corner)
                {
                    //Wood
                    if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Wood)
                    {
                        //print("Placed: Wood Angle");
                        buildingBlockList.Add(Instantiate(builingBlock_Wood_Ramp_Corner, freeGhost_LookedAt.transform.position, rotation) as GameObject);
                    }

                    //Stone
                    else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Stone)
                    {
                        //print("Placed: Stone Angle");
                        buildingBlockList.Add(Instantiate(builingBlock_Stone_Ramp_Corner, freeGhost_LookedAt.transform.position, rotation) as GameObject);
                    }

                    //Iron
                    else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Iron)
                    {
                        //print("Placed: Iron Angle");
                        buildingBlockList.Add(Instantiate(builingBlock_Iron_Ramp_Corner, freeGhost_LookedAt.transform.position, rotation) as GameObject);
                    }
                }

                //Ramp_Triangle
                else if (freeGhost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Ramp_Triangle)
                {
                    //Wood
                    if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Wood)
                    {
                        //print("Placed: Wood Angle");
                        buildingBlockList.Add(Instantiate(builingBlock_Wood_Ramp_Triangle, freeGhost_LookedAt.transform.position, rotation) as GameObject);
                    }

                    //Stone
                    else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Stone)
                    {
                        //print("Placed: Stone Angle");
                        buildingBlockList.Add(Instantiate(builingBlock_Stone_Ramp_Triangle, freeGhost_LookedAt.transform.position, rotation) as GameObject);
                    }

                    //Iron
                    else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Iron)
                    {
                        //print("Placed: Iron Angle");
                        buildingBlockList.Add(Instantiate(builingBlock_Iron_Ramp_Triangle, freeGhost_LookedAt.transform.position, rotation) as GameObject);
                    }
                }

                //Wall_Triangle
                else if (freeGhost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Wall_Triangle)
                {
                    //Wood
                    if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Wood)
                    {
                        //print("Placed: Wood Angle");
                        buildingBlockList.Add(Instantiate(builingBlock_Wood_Wall_Triangle, freeGhost_LookedAt.transform.position, rotation) as GameObject);
                    }

                    //Stone
                    else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Stone)
                    {
                        //print("Placed: Stone Angle");
                        buildingBlockList.Add(Instantiate(builingBlock_Stone_Wall_Triangle, freeGhost_LookedAt.transform.position, rotation) as GameObject);
                    }

                    //Iron
                    else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Iron)
                    {
                        //print("Placed: Iron Angle");
                        buildingBlockList.Add(Instantiate(builingBlock_Iron_Wall_Triangle, freeGhost_LookedAt.transform.position, rotation) as GameObject);
                    }
                }

                //Fence_Diagonal
                else if (freeGhost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Fence && freeGhost_LookedAt.GetComponent<Building_Ghost>().buildingSubType == BuildingSubType.Diagonaly)
                {
                    BlockDirection_A a = freeGhost_LookedAt.GetComponent<Building_Ghost>().blockDirection_A;
                    BlockDirection_B b = freeGhost_LookedAt.GetComponent<Building_Ghost>().blockDirection_B;

                    if ((a == BlockDirection_A.North && b == BlockDirection_B.Left) || (a == BlockDirection_A.South && b == BlockDirection_B.Left) || (a == BlockDirection_A.North && b == BlockDirection_B.Right) || (a == BlockDirection_A.South && b == BlockDirection_B.Right))
                    {
                        //Wood
                        if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Wood)
                        {
                            //print("Placed: Wood Fence_Diagonal");
                            buildingBlockList.Add(Instantiate(builingBlock_Wood_Fence, freeGhost_LookedAt.transform.position, rotation) as GameObject);
                        }

                        //Stone
                        else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Stone)
                        {
                            //print("Placed: Stone Fence_Diagonal");
                            buildingBlockList.Add(Instantiate(builingBlock_Stone_Fence, freeGhost_LookedAt.transform.position, rotation) as GameObject);
                        }

                        //Iron
                        else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Iron)
                        {
                            //print("Placed: Iron Fence_Diagonal");
                            buildingBlockList.Add(Instantiate(builingBlock_Iron_Fence, freeGhost_LookedAt.transform.position, rotation) as GameObject);
                        }
                    }
                    else
                    {
                        //Wood
                        if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Wood)
                        {
                            //print("Placed: Wood Fence_Diagonal");
                            buildingBlockList.Add(Instantiate(builingBlock_Wood_Fence_Diagonaly, freeGhost_LookedAt.transform.position, rotation) as GameObject);
                        }

                        //Stone
                        else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Stone)
                        {
                            //print("Placed: Stone Fence_Diagonal");
                            buildingBlockList.Add(Instantiate(builingBlock_Stone_Fence_Diagonaly, freeGhost_LookedAt.transform.position, rotation) as GameObject);
                        }

                        //Iron
                        else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Iron)
                        {
                            //print("Placed: Iron Fence_Diagonal");
                            buildingBlockList.Add(Instantiate(builingBlock_Iron_Fence_Diagonaly, freeGhost_LookedAt.transform.position, rotation) as GameObject);
                        }
                    }
                }

                //Fence
                else if (freeGhost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Fence && freeGhost_LookedAt.GetComponent<Building_Ghost>().buildingSubType == BuildingSubType.None)
                {
                    //Wood
                    if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Wood)
                    {
                        //print("Placed: Wood Angle");
                        buildingBlockList.Add(Instantiate(builingBlock_Wood_Fence, freeGhost_LookedAt.transform.position, rotation) as GameObject);
                    }

                    //Stone
                    else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Stone)
                    {
                        //print("Placed: Stone Angle");
                        buildingBlockList.Add(Instantiate(builingBlock_Stone_Fence, freeGhost_LookedAt.transform.position, rotation) as GameObject);
                    }

                    //Iron
                    else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Iron)
                    {
                        //print("Placed: Iron Angle");
                        buildingBlockList.Add(Instantiate(builingBlock_Iron_Fence, freeGhost_LookedAt.transform.position, rotation) as GameObject);
                    }
                }

                //Window
                else if (freeGhost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Window)
                {
                    //Wood
                    if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Wood)
                    {
                        //print("Placed: Wood Angle");
                        buildingBlockList.Add(Instantiate(builingBlock_Wood_Window, freeGhost_LookedAt.transform.position, rotation) as GameObject);
                    }

                    //Stone
                    else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Stone)
                    {
                        //print("Placed: Stone Angle");
                        buildingBlockList.Add(Instantiate(builingBlock_Stone_Window, freeGhost_LookedAt.transform.position, rotation) as GameObject);
                    }

                    //Iron
                    else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Iron)
                    {
                        //print("Placed: Iron Angle");
                        buildingBlockList.Add(Instantiate(builingBlock_Iron_Window, freeGhost_LookedAt.transform.position, rotation) as GameObject);
                    }
                }

                //Door
                else if (freeGhost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Door)
                {
                    //Wood
                    if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Wood)
                    {
                        //print("Placed: Wood Angle");
                        buildingBlockList.Add(Instantiate(builingBlock_Wood_Door, freeGhost_LookedAt.transform.position, rotation) as GameObject);
                    }

                    //Stone
                    else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Stone)
                    {
                        //print("Placed: Stone Angle");
                        buildingBlockList.Add(Instantiate(builingBlock_Stone_Door, freeGhost_LookedAt.transform.position, rotation) as GameObject);
                    }

                    //Iron
                    else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Iron)
                    {
                        //print("Placed: Iron Angle");
                        buildingBlockList.Add(Instantiate(builingBlock_Iron_Door, freeGhost_LookedAt.transform.position, rotation) as GameObject);
                    }
                }

                //Stair
                else if (freeGhost_LookedAt.GetComponent<Building_Ghost>().buildingType == BuildingType.Stair)
                {
                    //Wood
                    if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Wood)
                    {
                        //print("Placed: Wood Angle");
                        buildingBlockList.Add(Instantiate(builingBlock_Wood_Stair, freeGhost_LookedAt.transform.position, rotation) as GameObject);
                    }

                    //Stone
                    else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Stone)
                    {
                        //print("Placed: Stone Angle");
                        buildingBlockList.Add(Instantiate(builingBlock_Stone_Stair, freeGhost_LookedAt.transform.position, rotation) as GameObject);
                    }

                    //Iron
                    else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Iron)
                    {
                        //print("Placed: Iron Angle");
                        buildingBlockList.Add(Instantiate(builingBlock_Iron_Stair, freeGhost_LookedAt.transform.position, rotation) as GameObject);
                    }
                }
                #endregion

                //Set Parent of the placed object
                buildingBlockList[buildingBlockList.Count - 1].transform.SetParent(buildingBlock_Parent.transform);

                #region Setup the Placed Block
                //Set info on the Placed Block
                BlockPlaced blockPlaced = new BlockPlaced();
                blockPlaced.buildingBlock = freeGhost_LookedAt.GetComponent<Building_Ghost>().blockParent;
                blockPlaced.buildingType = freeGhost_LookedAt.GetComponent<Building_Ghost>().blockParent.GetComponent<BuildingBlock_Parent>().buildingType;
                blockPlaced.buildingSubType = freeGhost_LookedAt.GetComponent<Building_Ghost>().blockParent.GetComponent<BuildingBlock_Parent>().buildingSubType;
                #endregion
                #region Setup the Block that got a Block Placed on It
                //Set info on the Block that got a block placed on it
                BlockPlaced blockGotPlacedOn = new BlockPlaced();
                blockGotPlacedOn.buildingBlock = buildingBlockList[buildingBlockList.Count - 1];
                blockGotPlacedOn.buildingType = MoveableObjectManager.Instance.buildingType_Selected;
                blockGotPlacedOn.buildingSubType = buildingBlockList[buildingBlockList.Count - 1].GetComponent<BuildingBlock_Parent>().buildingSubType;
                #endregion

                //Remove items from inventory
                BuildingBlock_Parent tempParent = GetBuildingBlock(MoveableObjectManager.Instance.buildingType_Selected, MoveableObjectManager.Instance.buildingMaterial_Selected);
                if (tempParent.buildingRequirementList != null)
                {
                    for (int i = 0; i < tempParent.buildingRequirementList.Count; i++)
                    {
                        for (int k = 0; k < tempParent.buildingRequirementList[i].amount; k++)
                        {
                            InventoryManager.Instance.RemoveItemFromInventory(0, tempParent.buildingRequirementList[i].itemName, -1, false);
                        }
                    }
                }

                //Update the Hotbar
                //InventoryManager.Instance.CheckHotbarItemInInventory();
                InventoryManager.Instance.RemoveInventoriesUI();

                //Reset parameters
                freeGhost_LookedAt = null;
                SetAllGhostState_Off();

                blockisPlacing = false;

                SaveData();
            }

            //If buildingBlock cannot be placed
            else
            {
                print("1. Don't Place Block");

                //Play Sound
                if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Wood)
                {
                    SoundManager.Instance.Play_Building_CannotPlaceBlock_Clip();
                }
            }
        }
    }


    //--------------------


    void RaycastSetup_Axe()
    {
        ray_Axe = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray_Axe, out hit_Axe))
        {
            //Get the Transform of GameObject hit
            var hitTransform = hit_Axe.transform;

            if (hitTransform.gameObject.CompareTag("BuildingBlock"))
            {
                if (Axe_buildingBlockLookingAt != hitTransform.gameObject)
                {
                    Axe_buildingBlockLookingAt = hitTransform.gameObject;

                    if (Axe_buildingBlockLookingAt.GetComponent<BuildingBlock>() != null)
                    {
                        if (Axe_buildingBlockLookingAt.GetComponent<BuildingBlock>().buidingBlock_Parent.GetComponent<BuildingBlock_Parent>() != null)
                        {
                            SetBuildingRemoveRequirements(Axe_buildingBlockLookingAt.GetComponent<BuildingBlock>().buidingBlock_Parent.GetComponent<BuildingBlock_Parent>());
                        }
                    }
                }
            }

            else if (hitTransform.gameObject.CompareTag("Machine") || hitTransform.gameObject.CompareTag("Furniture"))
            {
                if (Axe_buildingBlockLookingAt != hitTransform.gameObject)
                {
                    Axe_buildingBlockLookingAt = hitTransform.gameObject;

                    if (Axe_buildingBlockLookingAt.GetComponent<MoveableObject>() != null)
                    {
                        SetBuildingRemoveRequirements(MoveableObjectManager.Instance.GetMoveableObjectInfo(Axe_buildingBlockLookingAt.GetComponent<MoveableObject>()));
                    }
                }
            }
            else
            {
                if (Axe_buildingBlockLookingAt != null)
                {
                    Axe_buildingBlockLookingAt = null;
                    buildingRemoveRequirement_Parent.SetActive(false);
                }
            }
        }
    }
    public void CutBlock()
    {
        ray_Axe = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray_Axe, out hit_Axe))
        {
            var hitTransform = hit_Axe.transform;

            if (Axe_buildingBlockLookingAt != null)
            {
                //If looking at a BuildingBlock
                if (hitTransform.gameObject.CompareTag("BuildingBlock"))
                {
                    if (Axe_buildingBlockLookingAt.GetComponent<BuildingBlock>().buidingBlock_Parent != null)
                    {
                        if (Axe_buildingBlockLookingAt.GetComponent<BuildingBlock>().buidingBlock_Parent.GetComponent<BuildingBlock_Parent>() != null)
                        {
                            for (int i = 0; i < buildingBlockList.Count; i++)
                            {
                                if (Axe_buildingBlockLookingAt.GetComponent<BuildingBlock>().buidingBlock_Parent == buildingBlockList[i])
                                {
                                    print("6. Destroy BuildingBlock");

                                    //Play remove sound
                                    if (Axe_buildingBlockLookingAt.GetComponent<BuildingBlock>().buidingBlock_Parent.GetComponent<BuildingBlock_Parent>().buildingMaterial == BuildingMaterial.Wood)
                                    {
                                        SoundManager.Instance.Play_Building_Remove_Wood_Clip();
                                    }
                                    else if (Axe_buildingBlockLookingAt.GetComponent<BuildingBlock>().buidingBlock_Parent.GetComponent<BuildingBlock_Parent>().buildingMaterial == BuildingMaterial.Stone)
                                    {
                                        SoundManager.Instance.Play_Building_Remove_Stone_Clip();
                                    }
                                    else if (Axe_buildingBlockLookingAt.GetComponent<BuildingBlock>().buidingBlock_Parent.GetComponent<BuildingBlock_Parent>().buildingMaterial == BuildingMaterial.Iron)
                                    {
                                        SoundManager.Instance.Play_Building_Remove_Cryonite_Clip();
                                    }

                                    //Add items to inventory
                                    for (int j = 0; j < Axe_buildingBlockLookingAt.GetComponent<BuildingBlock>().buidingBlock_Parent.GetComponent<BuildingBlock_Parent>().removeBuildingRequirementList.Count; j++)
                                    {
                                        for (int k = 0; k < Axe_buildingBlockLookingAt.GetComponent<BuildingBlock>().buidingBlock_Parent.GetComponent<BuildingBlock_Parent>().removeBuildingRequirementList[j].amount; k++)
                                        {
                                            InventoryManager.Instance.AddItemToInventory(0, Axe_buildingBlockLookingAt.GetComponent<BuildingBlock>().buidingBlock_Parent.GetComponent<BuildingBlock_Parent>().removeBuildingRequirementList[j].itemName);
                                        }
                                    }

                                    //Remove Building Object
                                    buildingBlockList.RemoveAt(i);
                                    Axe_buildingBlockLookingAt.GetComponent<BuildingBlock>().buidingBlock_Parent.GetComponent<BuildingBlock_Parent>().DestroyThisObject();

                                    //Reset parameters
                                    Axe_buildingBlockLookingAt = null;
                                    lastBuildingBlock_LookedAt = null;
                                    old_lastBuildingBlock_LookedAt = null;

                                    if (Axe_buildingBlockLookingAt != null)
                                    {
                                        SetBuildingRemoveRequirements(Axe_buildingBlockLookingAt.GetComponent<BuildingBlock>().buidingBlock_Parent.GetComponent<BuildingBlock_Parent>());
                                    }
                                    else
                                    {
                                        buildingRemoveRequirement_Parent.SetActive(false);
                                    }

                                    SaveData();

                                    break;
                                }
                            }
                        }
                    }
                }

                //If looking at a Machine or Furniture
                else if (hitTransform.gameObject.CompareTag("Machine") || hitTransform.gameObject.CompareTag("Furniture"))
                {
                    print("7. Tried to cut a Machine or Furniture");

                    //If the object has a MoveableObject attached
                    if (hitTransform.gameObject.GetComponent<MoveableObject>())
                    {
                        for (int i = 0; i < MoveableObjectManager.Instance.placedMoveableWorldObjectsList.Count; i++)
                        {
                            //If we have chosen the correct movableObject
                            if (hitTransform.gameObject == MoveableObjectManager.Instance.placedMoveableWorldObjectsList[i])
                            {
                                MoveableObjectInfo tempMovableObject_SO = MoveableObjectManager.Instance.GetObjectInfoFromMoveableObject_SO(hitTransform.gameObject.GetComponent<MoveableObject>().machineType, hitTransform.gameObject.GetComponent<MoveableObject>().furnitureType);

                                //If object is a chest
                                #region
                                if (tempMovableObject_SO.machineType == MachineType.None
                                    &&
                                    (tempMovableObject_SO.furnitureType == FurnitureType.SmallChest
                                    || tempMovableObject_SO.furnitureType == FurnitureType.BigChest))
                                {
                                    if (MoveableObjectManager.Instance.placedMoveableWorldObjectsList[i].GetComponent<InteractableObject>())
                                    {
                                        //If chest contain items, don't remove it
                                        if (InventoryManager.Instance.inventories[MoveableObjectManager.Instance.placedMoveableWorldObjectsList[i].GetComponent<InteractableObject>().inventoryIndex].itemsInInventory.Count > 0)
                                        {
                                            SoundManager.Instance.Play_Building_CannotPlaceBlock_Clip();
                                            return;
                                        }
                                        else
                                        {
                                            InventoryManager.Instance.RemoveInventory(MoveableObjectManager.Instance.placedMoveableWorldObjectsList[i].GetComponent<InteractableObject>().inventoryIndex);
                                        }
                                    }
                                }
                                #endregion

                                //If Object can be removed
                                #region
                                print("8. Machine or Furniture");

                                //Add items to playerInventory
                                for (int j = 0; j < tempMovableObject_SO.RemoveCraftingRequirements.Count; j++)
                                {
                                    for (int k = 0; k < tempMovableObject_SO.RemoveCraftingRequirements[j].amount; k++)
                                    {
                                        InventoryManager.Instance.AddItemToInventory(0, tempMovableObject_SO.RemoveCraftingRequirements[j].itemName);
                                    }
                                }

                                if (Axe_buildingBlockLookingAt.GetComponent<InteractableObject>())
                                {
                                    //Remove MovableObject
                                    MoveableObjectManager.Instance.placedMoveableObjectsList_ToSave.RemoveAt(i);
                                    MoveableObjectManager.Instance.placedMoveableWorldObjectsList.RemoveAt(i);

                                    Axe_buildingBlockLookingAt.GetComponent<InteractableObject>().DestroyThisObject();
                                }

                                //Reset parameters
                                Axe_buildingBlockLookingAt = null;
                                lastBuildingBlock_LookedAt = null;
                                old_lastBuildingBlock_LookedAt = null;

                                buildingRemoveRequirement_Parent.SetActive(false);
                                buildingRequirement_Parent.SetActive(false);

                                //Play Remove-Sound
                                SoundManager.Instance.Play_Building_Remove_MoveableObject_Clip();

                                SaveData();
                                MoveableObjectManager.Instance.SaveData();

                                break;
                                #endregion
                            }
                        }
                    }
                }
                else
                {
                    print("Doesn't hit at all");
                }
            }
            else
            {
                print("buildingBlockLookingAt_Axe == null");
            }
        }
        else
        {
            print("Physics.RaycastHit Isn't hit");
        }
    }


    //--------------------


    public BuildingBlock_Parent GetBuildingBlock(BuildingType buildingType, BuildingMaterial buildingMaterial)
    {
        if (buildingType == BuildingType.None || buildingMaterial == BuildingMaterial.None)
        {
            return null;
        }

        if (builingBlock_Wood_Floor.GetComponent<BuildingBlock_Parent>().buildingType == buildingType && builingBlock_Wood_Floor.GetComponent<BuildingBlock_Parent>().buildingMaterial == buildingMaterial)
            return builingBlock_Wood_Floor.GetComponent<BuildingBlock_Parent>();
        else if (builingBlock_Wood_Floor_Triangle.GetComponent<BuildingBlock_Parent>().buildingType == buildingType && builingBlock_Wood_Floor_Triangle.GetComponent<BuildingBlock_Parent>().buildingMaterial == buildingMaterial)
            return builingBlock_Wood_Floor_Triangle.GetComponent<BuildingBlock_Parent>();
        else if (builingBlock_Wood_Wall.GetComponent<BuildingBlock_Parent>().buildingType == buildingType && builingBlock_Wood_Wall.GetComponent<BuildingBlock_Parent>().buildingMaterial == buildingMaterial)
            return builingBlock_Wood_Wall.GetComponent<BuildingBlock_Parent>();
        else if (builingBlock_Wood_Wall_Diagonal.GetComponent<BuildingBlock_Parent>().buildingType == buildingType && builingBlock_Wood_Wall_Diagonal.GetComponent<BuildingBlock_Parent>().buildingMaterial == buildingMaterial)
            return builingBlock_Wood_Wall_Diagonal.GetComponent<BuildingBlock_Parent>();
        else if (builingBlock_Wood_Ramp.GetComponent<BuildingBlock_Parent>().buildingType == buildingType && builingBlock_Wood_Ramp.GetComponent<BuildingBlock_Parent>().buildingMaterial == buildingMaterial)
            return builingBlock_Wood_Ramp.GetComponent<BuildingBlock_Parent>();
        else if (builingBlock_Wood_Ramp_Corner.GetComponent<BuildingBlock_Parent>().buildingType == buildingType && builingBlock_Wood_Ramp_Corner.GetComponent<BuildingBlock_Parent>().buildingMaterial == buildingMaterial)
            return builingBlock_Wood_Ramp_Corner.GetComponent<BuildingBlock_Parent>();
        else if (builingBlock_Wood_Ramp_Triangle.GetComponent<BuildingBlock_Parent>().buildingType == buildingType && builingBlock_Wood_Ramp_Triangle.GetComponent<BuildingBlock_Parent>().buildingMaterial == buildingMaterial)
            return builingBlock_Wood_Ramp_Triangle.GetComponent<BuildingBlock_Parent>();
        else if (builingBlock_Wood_Wall_Triangle.GetComponent<BuildingBlock_Parent>().buildingType == buildingType && builingBlock_Wood_Wall_Triangle.GetComponent<BuildingBlock_Parent>().buildingMaterial == buildingMaterial)
            return builingBlock_Wood_Wall_Triangle.GetComponent<BuildingBlock_Parent>();
        else if (builingBlock_Wood_Fence.GetComponent<BuildingBlock_Parent>().buildingType == buildingType && builingBlock_Wood_Fence.GetComponent<BuildingBlock_Parent>().buildingMaterial == buildingMaterial)
            return builingBlock_Wood_Fence.GetComponent<BuildingBlock_Parent>();
        else if (builingBlock_Wood_Fence_Diagonaly.GetComponent<BuildingBlock_Parent>().buildingType == buildingType && builingBlock_Wood_Fence_Diagonaly.GetComponent<BuildingBlock_Parent>().buildingMaterial == buildingMaterial)
            return builingBlock_Wood_Fence_Diagonaly.GetComponent<BuildingBlock_Parent>();
        else if (builingBlock_Wood_Window.GetComponent<BuildingBlock_Parent>().buildingType == buildingType && builingBlock_Wood_Window.GetComponent<BuildingBlock_Parent>().buildingMaterial == buildingMaterial)
            return builingBlock_Wood_Window.GetComponent<BuildingBlock_Parent>();
        else if (builingBlock_Wood_Door.GetComponent<BuildingBlock_Parent>().buildingType == buildingType && builingBlock_Wood_Door.GetComponent<BuildingBlock_Parent>().buildingMaterial == buildingMaterial)
            return builingBlock_Wood_Door.GetComponent<BuildingBlock_Parent>();
        else if (builingBlock_Wood_Stair.GetComponent<BuildingBlock_Parent>().buildingType == buildingType && builingBlock_Wood_Stair.GetComponent<BuildingBlock_Parent>().buildingMaterial == buildingMaterial)
            return builingBlock_Wood_Stair.GetComponent<BuildingBlock_Parent>();

        else if (builingBlock_Stone_Floor.GetComponent<BuildingBlock_Parent>().buildingType == buildingType && builingBlock_Stone_Floor.GetComponent<BuildingBlock_Parent>().buildingMaterial == buildingMaterial)
            return builingBlock_Stone_Floor.GetComponent<BuildingBlock_Parent>();
        else if (builingBlock_Stone_Floor_Triangle.GetComponent<BuildingBlock_Parent>().buildingType == buildingType && builingBlock_Stone_Floor_Triangle.GetComponent<BuildingBlock_Parent>().buildingMaterial == buildingMaterial)
            return builingBlock_Stone_Floor_Triangle.GetComponent<BuildingBlock_Parent>();
        else if (builingBlock_Stone_Wall.GetComponent<BuildingBlock_Parent>().buildingType == buildingType && builingBlock_Stone_Wall.GetComponent<BuildingBlock_Parent>().buildingMaterial == buildingMaterial)
            return builingBlock_Stone_Wall.GetComponent<BuildingBlock_Parent>();
        else if (builingBlock_Stone_Wall_Diagonal.GetComponent<BuildingBlock_Parent>().buildingType == buildingType && builingBlock_Stone_Wall_Diagonal.GetComponent<BuildingBlock_Parent>().buildingMaterial == buildingMaterial)
            return builingBlock_Stone_Wall_Diagonal.GetComponent<BuildingBlock_Parent>();
        else if (builingBlock_Stone_Ramp.GetComponent<BuildingBlock_Parent>().buildingType == buildingType && builingBlock_Stone_Ramp.GetComponent<BuildingBlock_Parent>().buildingMaterial == buildingMaterial)
            return builingBlock_Stone_Ramp.GetComponent<BuildingBlock_Parent>();
        else if (builingBlock_Stone_Ramp_Corner.GetComponent<BuildingBlock_Parent>().buildingType == buildingType && builingBlock_Stone_Ramp_Corner.GetComponent<BuildingBlock_Parent>().buildingMaterial == buildingMaterial)
            return builingBlock_Stone_Ramp_Corner.GetComponent<BuildingBlock_Parent>();
        else if (builingBlock_Stone_Ramp_Triangle.GetComponent<BuildingBlock_Parent>().buildingType == buildingType && builingBlock_Stone_Ramp_Triangle.GetComponent<BuildingBlock_Parent>().buildingMaterial == buildingMaterial)
            return builingBlock_Stone_Ramp_Triangle.GetComponent<BuildingBlock_Parent>();
        else if (builingBlock_Stone_Wall_Triangle.GetComponent<BuildingBlock_Parent>().buildingType == buildingType && builingBlock_Stone_Wall_Triangle.GetComponent<BuildingBlock_Parent>().buildingMaterial == buildingMaterial)
            return builingBlock_Stone_Wall_Triangle.GetComponent<BuildingBlock_Parent>();
        else if (builingBlock_Stone_Fence.GetComponent<BuildingBlock_Parent>().buildingType == buildingType && builingBlock_Stone_Fence.GetComponent<BuildingBlock_Parent>().buildingMaterial == buildingMaterial)
            return builingBlock_Stone_Fence.GetComponent<BuildingBlock_Parent>();
        else if (builingBlock_Stone_Fence_Diagonaly.GetComponent<BuildingBlock_Parent>().buildingType == buildingType && builingBlock_Stone_Fence_Diagonaly.GetComponent<BuildingBlock_Parent>().buildingMaterial == buildingMaterial)
            return builingBlock_Stone_Fence_Diagonaly.GetComponent<BuildingBlock_Parent>();
        else if (builingBlock_Stone_Window.GetComponent<BuildingBlock_Parent>().buildingType == buildingType && builingBlock_Stone_Window.GetComponent<BuildingBlock_Parent>().buildingMaterial == buildingMaterial)
            return builingBlock_Stone_Window.GetComponent<BuildingBlock_Parent>();
        else if (builingBlock_Stone_Door.GetComponent<BuildingBlock_Parent>().buildingType == buildingType && builingBlock_Stone_Door.GetComponent<BuildingBlock_Parent>().buildingMaterial == buildingMaterial)
            return builingBlock_Stone_Door.GetComponent<BuildingBlock_Parent>();
        else if (builingBlock_Stone_Stair.GetComponent<BuildingBlock_Parent>().buildingType == buildingType && builingBlock_Stone_Stair.GetComponent<BuildingBlock_Parent>().buildingMaterial == buildingMaterial)
            return builingBlock_Stone_Stair.GetComponent<BuildingBlock_Parent>();

        else if (builingBlock_Iron_Floor.GetComponent<BuildingBlock_Parent>().buildingType == buildingType && builingBlock_Iron_Floor.GetComponent<BuildingBlock_Parent>().buildingMaterial == buildingMaterial)
            return builingBlock_Iron_Floor.GetComponent<BuildingBlock_Parent>();
        else if (builingBlock_Iron_Floor_Triangle.GetComponent<BuildingBlock_Parent>().buildingType == buildingType && builingBlock_Iron_Floor_Triangle.GetComponent<BuildingBlock_Parent>().buildingMaterial == buildingMaterial)
            return builingBlock_Iron_Floor_Triangle.GetComponent<BuildingBlock_Parent>();
        else if (builingBlock_Iron_Wall.GetComponent<BuildingBlock_Parent>().buildingType == buildingType && builingBlock_Iron_Wall.GetComponent<BuildingBlock_Parent>().buildingMaterial == buildingMaterial)
            return builingBlock_Iron_Wall.GetComponent<BuildingBlock_Parent>();
        else if (builingBlock_Iron_Wall_Diagonal.GetComponent<BuildingBlock_Parent>().buildingType == buildingType && builingBlock_Iron_Wall_Diagonal.GetComponent<BuildingBlock_Parent>().buildingMaterial == buildingMaterial)
            return builingBlock_Iron_Wall_Diagonal.GetComponent<BuildingBlock_Parent>();
        else if (builingBlock_Iron_Ramp.GetComponent<BuildingBlock_Parent>().buildingType == buildingType && builingBlock_Iron_Ramp.GetComponent<BuildingBlock_Parent>().buildingMaterial == buildingMaterial)
            return builingBlock_Iron_Ramp.GetComponent<BuildingBlock_Parent>();
        else if (builingBlock_Iron_Ramp_Corner.GetComponent<BuildingBlock_Parent>().buildingType == buildingType && builingBlock_Iron_Ramp_Corner.GetComponent<BuildingBlock_Parent>().buildingMaterial == buildingMaterial)
            return builingBlock_Iron_Ramp_Corner.GetComponent<BuildingBlock_Parent>();
        else if (builingBlock_Iron_Ramp_Triangle.GetComponent<BuildingBlock_Parent>().buildingType == buildingType && builingBlock_Iron_Ramp_Triangle.GetComponent<BuildingBlock_Parent>().buildingMaterial == buildingMaterial)
            return builingBlock_Iron_Ramp_Triangle.GetComponent<BuildingBlock_Parent>();
        else if (builingBlock_Iron_Wall_Triangle.GetComponent<BuildingBlock_Parent>().buildingType == buildingType && builingBlock_Iron_Wall_Triangle.GetComponent<BuildingBlock_Parent>().buildingMaterial == buildingMaterial)
            return builingBlock_Iron_Wall_Triangle.GetComponent<BuildingBlock_Parent>();
        else if (builingBlock_Iron_Fence.GetComponent<BuildingBlock_Parent>().buildingType == buildingType && builingBlock_Iron_Fence.GetComponent<BuildingBlock_Parent>().buildingMaterial == buildingMaterial)
            return builingBlock_Iron_Fence.GetComponent<BuildingBlock_Parent>();
        else if (builingBlock_Iron_Fence_Diagonaly.GetComponent<BuildingBlock_Parent>().buildingType == buildingType && builingBlock_Iron_Fence_Diagonaly.GetComponent<BuildingBlock_Parent>().buildingMaterial == buildingMaterial)
            return builingBlock_Iron_Fence_Diagonaly.GetComponent<BuildingBlock_Parent>();
        else if (builingBlock_Iron_Window.GetComponent<BuildingBlock_Parent>().buildingType == buildingType && builingBlock_Iron_Window.GetComponent<BuildingBlock_Parent>().buildingMaterial == buildingMaterial)
            return builingBlock_Iron_Window.GetComponent<BuildingBlock_Parent>();
        else if (builingBlock_Iron_Door.GetComponent<BuildingBlock_Parent>().buildingType == buildingType && builingBlock_Iron_Door.GetComponent<BuildingBlock_Parent>().buildingMaterial == buildingMaterial)
            return builingBlock_Iron_Door.GetComponent<BuildingBlock_Parent>();
        else if (builingBlock_Iron_Stair.GetComponent<BuildingBlock_Parent>().buildingType == buildingType && builingBlock_Iron_Stair.GetComponent<BuildingBlock_Parent>().buildingMaterial == buildingMaterial)
            return builingBlock_Iron_Stair.GetComponent<BuildingBlock_Parent>();

        return null;
    }


    //--------------------


    //BuildingBlocks
    public void SetBuildingRequirements(BuildingBlock_Parent blockParent, GameObject ParentObject)
    {
        #region Setup
        //If Selected Object is Empty
        if (MoveableObjectManager.Instance.moveableObjectType == MoveableObjectType.None)
        {
            buildingRequirement_Parent.SetActive(false);

            return;
        }

        if (HotbarManager.Instance.selectedItem == Items.WoodAxe
            || HotbarManager.Instance.selectedItem == Items.StoneAxe
            || HotbarManager.Instance.selectedItem == Items.CryoniteAxe)
        {
            buildingRequirement_Parent.SetActive(false);

            return;
        }

        buildingRequirement_Parent.SetActive(true);

        //if (MainManager.Instance.gameStates == GameStates.Building)
        //{
        //    buildingRequirement_Parent.SetActive(true);
        //}
        //else
        //{
        //    buildingRequirement_Parent.SetActive(false);
        //}

        //Remove all childs
        for (int i = ParentObject.transform.childCount - 1; i >= 0; i--)
        {
            ParentObject.transform.GetChild(i).GetComponent<BuildingRequirementSlot>().DestroyThisObject();
        }
        buildingRequirement_List.Clear();
        #endregion

        //Set "enoughItemsToBuild" = true by default
        enoughItemsToBuild = true;

        //Setup Header of Requirements
        #region
        buildingRequirement_List.Add(Instantiate(buildingRequirementHeader_Prefab, ParentObject.transform) as GameObject);

        for (int i = 0; i < buildingBlock_UIList.Count; i++)
        {
            if (buildingBlock_UIList[i].GetComponent<BuildingBlock_UI>().buildingType == blockParent.buildingType
                && buildingBlock_UIList[i].GetComponent<BuildingBlock_UI>().buildingMaterial == blockParent.buildingMaterial)
            {
                buildingRequirement_List[buildingRequirement_List.Count - 1].GetComponent<BuildingRequirementSlot>().requirement_image.sprite = buildingBlock_UIList[i].GetComponent<BuildingBlock_UI>().parent.GetComponent<Image>().sprite;
                buildingRequirement_List[buildingRequirement_List.Count - 1].GetComponentInChildren<TextMeshProUGUI>().text = SpaceTextConverting.Instance.SetText(buildingBlock_UIList[i].GetComponent<BuildingBlock_UI>().buildingMaterial.ToString()) + " " + SpaceTextConverting.Instance.SetText(buildingBlock_UIList[i].GetComponent<BuildingBlock_UI>().buildingType.ToString()) /*blockParent.buildingMaterial.ToString() + " " + blockParent.buildingType.ToString()*/;

                break;
            }
        }
        #endregion

        //Setup new list of Requirements
        for (int i = 0; i < blockParent.buildingRequirementList.Count; i++)
        {
            //print("2. SetBuildingRequirements");
            buildingRequirement_List.Add(Instantiate(buildingRequirement_Prefab, ParentObject.transform) as GameObject);

            buildingRequirement_List[buildingRequirement_List.Count - 1].GetComponent<BuildingRequirementSlot>().requirement_image.sprite = MainManager.Instance.GetItem(blockParent.buildingRequirementList[i].itemName).hotbarSprite;
            buildingRequirement_List[buildingRequirement_List.Count - 1].GetComponent<BuildingRequirementSlot>().requirement_ItemName.text = SpaceTextConverting.Instance.SetText(MainManager.Instance.GetItem(blockParent.buildingRequirementList[i].itemName).itemName.ToString());

            int counter = 0;

            for (int k = 0; k < InventoryManager.Instance.inventories[0].itemsInInventory.Count; k++)
            {
                if (blockParent.buildingRequirementList[i].itemName == InventoryManager.Instance.inventories[0].itemsInInventory[k].itemName)
                {
                    counter++;
                }
            }

            //Display Amount required + Amount in inventory
            buildingRequirement_List[buildingRequirement_List.Count - 1].GetComponent<BuildingRequirementSlot>().requirement_amount.text = "x" + counter + " / " + blockParent.buildingRequirementList[i].amount.ToString();

            //If having enough items in inventory
            if (counter >= blockParent.buildingRequirementList[i].amount)
            {
                buildingRequirement_List[buildingRequirement_List.Count - 1].GetComponent<BuildingRequirementSlot>().requirement_BGimage.color = requirementMetColor;
                //enoughItemsToBuild = true;
            }

            //If not having enough items in inventory
            else
            {
                buildingRequirement_List[buildingRequirement_List.Count - 1].GetComponent<BuildingRequirementSlot>().requirement_BGimage.color = requirementNOTMetColor;
                enoughItemsToBuild = false;
            }
        }
    } 
    public void SetBuildingRemoveRequirements(BuildingBlock_Parent blockParent)
    {
        //Remove all childs
        for (int i = buildingRemoveRequirement_Parent.transform.childCount - 1; i >= 0; i--)
        {
            buildingRemoveRequirement_Parent.transform.GetChild(i).GetComponent<BuildingRequirementSlot>().DestroyThisObject();
        }
        buildingRemoveRequirement_List.Clear();

        //Setup Header of Requirements
        #region
        buildingRequirement_List.Add(Instantiate(buildingRequirementHeader_Prefab, buildingRemoveRequirement_Parent.transform) as GameObject);

        for (int i = 0; i < buildingBlock_UIList.Count; i++)
        {
            if (buildingBlock_UIList[i].GetComponent<BuildingBlock_UI>().buildingType == blockParent.buildingType
                && buildingBlock_UIList[i].GetComponent<BuildingBlock_UI>().buildingMaterial == blockParent.buildingMaterial)
            {
                buildingRequirement_List[buildingRequirement_List.Count - 1].GetComponent<BuildingRequirementSlot>().requirement_image.sprite = buildingBlock_UIList[i].GetComponent<BuildingBlock_UI>().parent.GetComponent<Image>().sprite;
                buildingRequirement_List[buildingRequirement_List.Count - 1].GetComponentInChildren<TextMeshProUGUI>().text = SpaceTextConverting.Instance.SetText(buildingBlock_UIList[i].GetComponent<BuildingBlock_UI>().buildingMaterial.ToString()) + " " + SpaceTextConverting.Instance.SetText(buildingBlock_UIList[i].GetComponent<BuildingBlock_UI>().buildingType.ToString())  /*blockParent.buildingMaterial.ToString() + " " + blockParent.buildingType.ToString()*/;

                break;
            }
        }
        #endregion

        //Setup new list of Requirements
        for (int i = 0; i < blockParent.removeBuildingRequirementList.Count; i++)
        {
            buildingRemoveRequirement_List.Add(Instantiate(buildingRequirement_Prefab, buildingRemoveRequirement_Parent.transform) as GameObject);

            buildingRemoveRequirement_List[buildingRemoveRequirement_List.Count - 1].GetComponent<BuildingRequirementSlot>().requirement_image.sprite = MainManager.Instance.GetItem(blockParent.removeBuildingRequirementList[i].itemName).hotbarSprite;
            buildingRemoveRequirement_List[buildingRemoveRequirement_List.Count - 1].GetComponent<BuildingRequirementSlot>().requirement_ItemName.text = SpaceTextConverting.Instance.SetText(MainManager.Instance.GetItem(blockParent.removeBuildingRequirementList[i].itemName).itemName.ToString());

            int counter = 0;

            for (int k = 0; k < InventoryManager.Instance.inventories[0].itemsInInventory.Count; k++)
            {
                if (blockParent.removeBuildingRequirementList[i].itemName == InventoryManager.Instance.inventories[0].itemsInInventory[k].itemName)
                {
                    counter++;
                }
            }

            //Display Amount required + Amount in inventory
            buildingRemoveRequirement_List[buildingRemoveRequirement_List.Count - 1].GetComponent<BuildingRequirementSlot>().requirement_amount.text = "x" + blockParent.removeBuildingRequirementList[i].amount.ToString() + " / " + counter;
        }

        buildingRemoveRequirement_Parent.SetActive(true);
    }

    //MovableObjects
    public void SetBuildingRequirements(MoveableObjectInfo moveableObject, GameObject ParentObject)
    {
        #region Setup
        //If Selected Object is Empty
        if (MoveableObjectManager.Instance.moveableObjectType == MoveableObjectType.None)
        {
            buildingRequirement_Parent.SetActive(false);

            return;
        }

        buildingRequirement_Parent.SetActive(true);

        if (HotbarManager.Instance.selectedItem == Items.WoodAxe
            || HotbarManager.Instance.selectedItem == Items.StoneAxe
            || HotbarManager.Instance.selectedItem == Items.CryoniteAxe)
        {
            buildingRequirement_Parent.SetActive(false);

            return;
        }

        //if (MainManager.Instance.gameStates != GameStates.Building)
        //{
        //    buildingRequirement_Parent.SetActive(false);

        //    return;
        //}

        //Remove all childs
        for (int i = ParentObject.transform.childCount - 1; i >= 0; i--)
        {
            ParentObject.transform.GetChild(i).GetComponent<BuildingRequirementSlot>().DestroyThisObject();
        }
        buildingRequirement_List.Clear();
        #endregion

        //Set "enoughItemsToBuild" = true by default
        enoughItemsToBuild = true;

        //Setup Header of Requirements
        #region
        buildingRequirement_List.Add(Instantiate(buildingRequirementHeader_Prefab, ParentObject.transform) as GameObject);

        buildingRequirement_List[buildingRequirement_List.Count - 1].GetComponent<BuildingRequirementSlot>().requirement_image.sprite = moveableObject.objectSprite;
        //buildingRequirement_List[buildingRequirement_List.Count - 1].GetComponentInChildren<Image>().sprite = moveableObject.objectSprite;

        if (MoveableObjectManager.Instance.moveableObjectType == MoveableObjectType.Machine)
        {
            buildingRequirement_List[buildingRequirement_List.Count - 1].GetComponentInChildren<TextMeshProUGUI>().text = SpaceTextConverting.Instance.SetText(moveableObject.machineType.ToString());
        }
        else if (MoveableObjectManager.Instance.moveableObjectType == MoveableObjectType.Furniture)
        {
            buildingRequirement_List[buildingRequirement_List.Count - 1].GetComponentInChildren<TextMeshProUGUI>().text = SpaceTextConverting.Instance.SetText(moveableObject.furnitureType.ToString());
        }
        #endregion

        //Setup new list of Requirements
        for (int i = 0; i < moveableObject.craftingRequirements.Count; i++)
        {
            //print("2. SetBuildingRequirements");
            buildingRequirement_List.Add(Instantiate(buildingRequirement_Prefab, ParentObject.transform) as GameObject);

            buildingRequirement_List[buildingRequirement_List.Count - 1].GetComponent<BuildingRequirementSlot>().requirement_image.sprite = MainManager.Instance.GetItem(moveableObject.craftingRequirements[i].itemName).hotbarSprite;
            buildingRequirement_List[buildingRequirement_List.Count - 1].GetComponent<BuildingRequirementSlot>().requirement_ItemName.text = SpaceTextConverting.Instance.SetText(MainManager.Instance.GetItem(moveableObject.craftingRequirements[i].itemName).itemName.ToString());

            int counter = 0;

            for (int k = 0; k < InventoryManager.Instance.inventories[0].itemsInInventory.Count; k++)
            {
                if (moveableObject.craftingRequirements[i].itemName == InventoryManager.Instance.inventories[0].itemsInInventory[k].itemName)
                {
                    counter++;
                }
            }

            //Display Amount required + Amount in inventory
            buildingRequirement_List[buildingRequirement_List.Count - 1].GetComponent<BuildingRequirementSlot>().requirement_amount.text = "x" + counter + " / " + moveableObject.craftingRequirements[i].amount.ToString();

            //If having enough items in inventory
            if (counter >= moveableObject.craftingRequirements[i].amount)
            {
                buildingRequirement_List[buildingRequirement_List.Count - 1].GetComponent<BuildingRequirementSlot>().requirement_BGimage.color = requirementMetColor;
                //enoughItemsToBuild = true;
            }

            //If not having enough items in inventory
            else
            {
                buildingRequirement_List[buildingRequirement_List.Count - 1].GetComponent<BuildingRequirementSlot>().requirement_BGimage.color = requirementNOTMetColor;
                enoughItemsToBuild = false;
            }
        }
    }
    public void SetBuildingRemoveRequirements(MoveableObjectInfo moveableObject)
    {
        //Remove all childs
        for (int i = buildingRemoveRequirement_Parent.transform.childCount - 1; i >= 0; i--)
        {
            buildingRemoveRequirement_Parent.transform.GetChild(i).GetComponent<BuildingRequirementSlot>().DestroyThisObject();
        }
        buildingRemoveRequirement_List.Clear();

        //Setup Header of Requirements
        #region
        buildingRequirement_List.Add(Instantiate(buildingRequirementHeader_Prefab, buildingRemoveRequirement_Parent.transform) as GameObject);

        buildingRequirement_List[buildingRequirement_List.Count - 1].GetComponent<BuildingRequirementSlot>().requirement_image.sprite = moveableObject.objectSprite;

        if (MoveableObjectManager.Instance.moveableObjectType == MoveableObjectType.Machine)
        {
            buildingRequirement_List[buildingRequirement_List.Count - 1].GetComponentInChildren<TextMeshProUGUI>().text = SpaceTextConverting.Instance.SetText(moveableObject.machineType.ToString());
        }
        else if (MoveableObjectManager.Instance.moveableObjectType == MoveableObjectType.Furniture)
        {
            buildingRequirement_List[buildingRequirement_List.Count - 1].GetComponentInChildren<TextMeshProUGUI>().text = SpaceTextConverting.Instance.SetText(moveableObject.furnitureType.ToString());
        }
        #endregion

        //Setup new list of Requirements
        for (int i = 0; i < moveableObject.RemoveCraftingRequirements.Count; i++)
        {
            buildingRemoveRequirement_List.Add(Instantiate(buildingRequirement_Prefab, buildingRemoveRequirement_Parent.transform) as GameObject);

            buildingRemoveRequirement_List[buildingRemoveRequirement_List.Count - 1].GetComponent<BuildingRequirementSlot>().requirement_image.sprite = MainManager.Instance.GetItem(moveableObject.RemoveCraftingRequirements[i].itemName).hotbarSprite;
            buildingRemoveRequirement_List[buildingRemoveRequirement_List.Count - 1].GetComponent<BuildingRequirementSlot>().requirement_ItemName.text = SpaceTextConverting.Instance.SetText(MainManager.Instance.GetItem(moveableObject.RemoveCraftingRequirements[i].itemName).itemName.ToString());

            int counter = 0;

            for (int k = 0; k < InventoryManager.Instance.inventories[0].itemsInInventory.Count; k++)
            {
                if (moveableObject.RemoveCraftingRequirements[i].itemName == InventoryManager.Instance.inventories[0].itemsInInventory[k].itemName)
                {
                    counter++;
                }
            }

            //Display Amount required + Amount in inventory
            buildingRemoveRequirement_List[buildingRemoveRequirement_List.Count - 1].GetComponent<BuildingRequirementSlot>().requirement_amount.text = "x" + moveableObject.RemoveCraftingRequirements[i].amount.ToString() + " / " + counter;
        }

        buildingRemoveRequirement_Parent.SetActive(true);
    }


    //--------------------


    public void SetNewSelectedBlock()
    {
        if (selectedMovableObjectInMenu)
        {
            if (selectedMovableObjectInMenu.GetComponent<InteractableObject>())
            {
                selectedMovableObjectInMenu.GetComponent<InteractableObject>().DestroyThisObject();
                selectedMovableObjectInMenu = null;
            }
            else
            {
                Destroy(selectedMovableObjectInMenu);
                selectedMovableObjectInMenu = null;
            }
        }

        //If selected Object is Empty
        if (MoveableObjectManager.Instance.moveableObjectType == MoveableObjectType.None)
        {
            selectedMovableObjectInMenu = null;
        }

        //If selected Object is a BuildingBlock
        else if (MoveableObjectManager.Instance.moveableObjectType == MoveableObjectType.BuildingBlock)
        {
            if (MoveableObjectManager.Instance.buildingType_Selected != BuildingType.None && MoveableObjectManager.Instance.buildingMaterial_Selected != BuildingMaterial.None)
            {
                //Instantiate new tempBlock as this BuildingBlock
                BuildingBlock_Parent temp = GetBuildingBlock(MoveableObjectManager.Instance.buildingType_Selected, MoveableObjectManager.Instance.buildingMaterial_Selected);

                for (int i = 0; i < temp.ghostList.Count; i++)
                {
                    if (temp.ghostList[i].GetComponent<Building_Ghost>().buildingType == MoveableObjectManager.Instance.buildingType_Selected)
                    {
                        if (HotbarManager.Instance.selectedItem == Items.WoodBuildingHammer
                            || HotbarManager.Instance.selectedItem == Items.StoneBuildingHammer
                            || HotbarManager.Instance.selectedItem == Items.CryoniteBuildingHammer)
                        {
                            selectedMovableObjectInMenu = Instantiate(temp.ghostList[i], InventoryManager.Instance.handDropPoint.transform.position, Quaternion.identity) as GameObject;
                            selectedMovableObjectInMenu.transform.parent = tempBlock_Parent.transform;

                            //Get the correct mesh
                            selectedMovableObjectInMenu.GetComponent<MeshFilter>().mesh = GetCorrectGhostMesh(selectedMovableObjectInMenu);
                            selectedMovableObjectInMenu.GetComponent<MeshRenderer>().material = canPlace_Material;

                            //Remove its BoxCollider
                            if (selectedMovableObjectInMenu.GetComponent<BoxCollider>())
                            {
                                selectedMovableObjectInMenu.GetComponent<BoxCollider>().enabled = !selectedMovableObjectInMenu.GetComponent<BoxCollider>().enabled;
                            }
                            break;
                        }
                    }
                }

                MoveableObjectManager.Instance.objectToMove = null;
            }
        }

        //If selected Object is a Machine or furniture
        else if (MoveableObjectManager.Instance.moveableObjectType == MoveableObjectType.Machine
            || MoveableObjectManager.Instance.moveableObjectType == MoveableObjectType.Furniture)
        {
            GameObject moveableObject = MoveableObjectManager.Instance.GetMoveableObject();

            if (moveableObject)
            {
                MoveableObjectManager.Instance.objectToMove = moveableObject;

                if (HotbarManager.Instance.selectedItem == Items.WoodBuildingHammer
                            || HotbarManager.Instance.selectedItem == Items.StoneBuildingHammer
                            || HotbarManager.Instance.selectedItem == Items.CryoniteBuildingHammer)
                {
                    selectedMovableObjectInMenu = Instantiate(moveableObject, InventoryManager.Instance.handDropPoint.transform.position, Quaternion.identity) as GameObject;
                    selectedMovableObjectInMenu.transform.parent = tempBlock_Parent.transform;

                    //Set selected For Movement
                    selectedMovableObjectInMenu.GetComponent<MoveableObject>().isSelectedForMovement = true;

                    //Get the correct material
                    //tempObj_Selected.GetComponent<MoveableObject>().meshRenderer.material = BuildingManager.Instance.canPlace_Material;

                    //Remove its BoxCollider
                    if (selectedMovableObjectInMenu.GetComponent<BoxCollider>())
                    {
                        selectedMovableObjectInMenu.GetComponent<BoxCollider>().enabled = !selectedMovableObjectInMenu.GetComponent<BoxCollider>().enabled;
                    }
                }
            }
        }

        buildingRequirement_Parent.SetActive(true);
    }


    //--------------------


    GameObject SetupBuildingBlockFromSave(BuildingBlockSaveList block)
    {
        switch (block.buildingID)
        {
            //Wood
            case 0:
                return builingBlock_Wood_Floor;
            case 1:
                return builingBlock_Wood_Floor_Triangle;
            case 2:
                return builingBlock_Wood_Wall;
            case 3:
                return builingBlock_Wood_Wall_Diagonal;
            case 4:
                return builingBlock_Wood_Ramp;
            case 5:
                return builingBlock_Wood_Ramp_Corner;
            case 6:
                return builingBlock_Wood_Ramp_Triangle;
            case 7:
                return builingBlock_Wood_Wall_Triangle;
            case 8:
                return builingBlock_Wood_Fence;
            case 9:
                return builingBlock_Wood_Fence_Diagonaly;
            case 10:
                return builingBlock_Wood_Window;
            case 11:
                return builingBlock_Wood_Door;
            case 12:
                return builingBlock_Wood_Stair;

            //Stone
            case 13:
                return builingBlock_Stone_Floor;
            case 14:
                return builingBlock_Stone_Floor_Triangle;
            case 15:
                return builingBlock_Stone_Wall;
            case 16:
                return builingBlock_Stone_Wall_Diagonal;
            case 17:
                return builingBlock_Stone_Ramp;
            case 18:
                return builingBlock_Stone_Ramp_Corner;
            case 19:
                return builingBlock_Stone_Ramp_Triangle;
            case 20:
                return builingBlock_Stone_Wall_Triangle;
            case 21:
                return builingBlock_Stone_Fence;
            case 22:
                return builingBlock_Stone_Fence_Diagonaly;
            case 23:
                return builingBlock_Stone_Window;
            case 24:
                return builingBlock_Stone_Door;
            case 25:
                return builingBlock_Stone_Stair;

            //Iron
            case 26:
                return builingBlock_Iron_Floor;
            case 27:
                return builingBlock_Iron_Floor_Triangle;
            case 28:
                return builingBlock_Iron_Wall;
            case 29:
                return builingBlock_Iron_Wall_Diagonal;
            case 30:
                return builingBlock_Iron_Ramp;
            case 31:
                return builingBlock_Iron_Ramp_Corner;
            case 32:
                return builingBlock_Iron_Ramp_Triangle;
            case 33:
                return builingBlock_Iron_Wall_Triangle;
            case 34:
                return builingBlock_Iron_Fence;
            case 35:
                return builingBlock_Iron_Fence_Diagonaly;
            case 36:
                return builingBlock_Iron_Window;
            case 37:
                return builingBlock_Iron_Door;
            case 38:
                return builingBlock_Iron_Stair;

            default:
                return null;
        }
    }
}

[Serializable]
public struct BuildingBlockSaveList
{
    public int buildingID;
    public Vector3 buildingBlock_Position;
    public Quaternion buildingBlock_Rotation;
}