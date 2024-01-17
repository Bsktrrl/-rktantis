using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BuildingManager : Singleton<BuildingManager>
{
    public GameObject buildingBlock_Parent;
    public List<GameObject> buildingBlockList = new List<GameObject>();
    [HideInInspector] public List<BuildingBlockSaveList> buildingBlockSaveList = new List<BuildingBlockSaveList>();

    [Header("Ghost")]
    public GameObject buildingBlockLookingAt_Axe;
    public GameObject lastBuildingBlock_LookedAt;
    [HideInInspector] public GameObject old_lastBuildingBlock_LookedAt;
    public GameObject ghost_LookedAt;
    public bool buildingBlockCanBePlaced;

    public GameObject freeGhost_LookedAt;

    public string BlockTagName;
    public GameObject BlockDirection_LookedAt;

    Ray oldRay = new Ray();


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
    [SerializeField] List<GameObject> buildingRequirement_List = new List<GameObject>();
    [SerializeField] List<GameObject> buildingRemoveRequirement_List = new List<GameObject>();

    public bool enoughItemsToBuild;
    public GameObject tempBlock_Parent;
    public bool blockisPlacing;


    //--------------------


    private void Awake()
    {
        MoveableObjectManager.Instance.buildingType_Selected = BuildingType.None;
        MoveableObjectManager.Instance.buildingMaterial_Selected = BuildingMaterial.None;
    }

    private void Update()
    {
        if (Time.frameCount % MainManager.instance.updateInterval == 0 && HotbarManager.Instance.selectedItem == Items.BuildingHammer)
        {
            RaycastSetup_Hammer();
            buildingRequirement_Parent.SetActive(true);
            buildingRemoveRequirement_Parent.SetActive(false);

            buildingBlockLookingAt_Axe = null;

            if (!BuildingHammer_isActive)
            {
                BuildingHammer_isActive = true;
            }

            //Set BuildingRequirement UI
            if (MainManager.instance.menuStates == MenuStates.BuildingSystemMenu)
            {
                buildingRequirement_Parent.SetActive(false);
            }
            else if (MainManager.instance.menuStates == MenuStates.None)
            {
                buildingRequirement_Parent.SetActive(true);
            }
        }
        else if (Time.frameCount % MainManager.instance.updateInterval == 0 && HotbarManager.Instance.selectedItem == Items.Axe)
        {
            RaycastSetup_Axe();

            if (BuildingHammer_isActive)
            {
                BuildingHammer_isActive = false;
                buildingRequirement_Parent.SetActive(false);
                buildingRemoveRequirement_Parent.SetActive(true);
                SetAllGhostState_Off();
                SetAllDirectionObjectState_Off();

                print("1. Set Ghosts OFF");
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

                print("2. Set Ghosts OFF");
            }
        }
    }

    //--------------------


    public void LoadData()
    {
        //Set data based on what's saved
        #region
        MoveableObjectManager.Instance.buildingType_Selected = DataManager.Instance.buildingType_Store;
        MoveableObjectManager.Instance.buildingMaterial_Selected = DataManager.Instance.buildingMaterial_Store;
        #endregion

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
        for (int i = 0; i < BuildingSystemMenu.instance.buildingBlockUIList.Count; i++)
        {
            if (BuildingSystemMenu.instance.buildingBlockUIList[i].GetComponent<BuildingBlock_UI>().buildingType == MoveableObjectManager.Instance.buildingType_Selected
                && BuildingSystemMenu.instance.buildingBlockUIList[i].GetComponent<BuildingBlock_UI>().buildingMaterial == MoveableObjectManager.Instance.buildingMaterial_Selected)
            {
                BuildingSystemMenu.instance.SetSelectedImage(BuildingSystemMenu.instance.buildingBlockUIList[i].GetComponent<Image>().sprite);

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

        if (HotbarManager.Instance.selectedItem == Items.BuildingHammer)
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

        //Save selected Building Type and Material
        DataManager.Instance.buildingType_Store = MoveableObjectManager.Instance.buildingType_Selected;
        DataManager.Instance.buildingMaterial_Store = MoveableObjectManager.Instance.buildingMaterial_Selected;

        print("Save buildingBlocks");
    }


    //--------------------


    void RaycastSetup_Hammer()
    {
        //Only active when not in a menu
        if (!BuildingSystemMenu.instance.buildingSystemMenu_isOpen)
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
        if (MainManager.instance.menuStates == MenuStates.None)
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

                //If raycarsting is not on a BuidingDirectionMarkers or ghostBlock
                if ((blockDirection_X != BlockDirection_A.None
                    && blockDirection_Y != BlockDirection_B.None)
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
            if ((blockDirection_X != BlockDirection_A.None
                && blockDirection_Y != BlockDirection_B.None)
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
            return;
        }

        //Reset all ghost before setting a new one
        if (ghost_LookedAt != blockLookingAt.ghostList[i])
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
                    SoundManager.instance.PlayWood_Placed_Clip();
                }
                else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Stone)
                {
                    SoundManager.instance.PlayStone_Placed_Clip();
                }
                else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Iron)
                {
                    SoundManager.instance.PlayIron_Placed_Clip();
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
                            InventoryManager.Instance.RemoveItemFromInventory(0, tempParent.buildingRequirementList[i].itemName, false);
                        }
                    }
                }
                
                //Update the Hotbar
                InventoryManager.Instance.CheckHotbarItemInInventory();
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
                    SoundManager.instance.PlaybuildingBlock_CannotPlaceBlock();
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
                    SoundManager.instance.PlayWood_Placed_Clip();
                }
                else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Stone)
                {
                    SoundManager.instance.PlayStone_Placed_Clip();
                }
                else if (MoveableObjectManager.Instance.buildingMaterial_Selected == BuildingMaterial.Iron)
                {
                    SoundManager.instance.PlayIron_Placed_Clip();
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
                            InventoryManager.Instance.RemoveItemFromInventory(0, tempParent.buildingRequirementList[i].itemName, false);
                        }
                    }
                }

                //Update the Hotbar
                InventoryManager.Instance.CheckHotbarItemInInventory();
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
                    SoundManager.instance.PlaybuildingBlock_CannotPlaceBlock();
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
                if (buildingBlockLookingAt_Axe != hitTransform.gameObject)
                {
                    buildingBlockLookingAt_Axe = hitTransform.gameObject;

                    if (buildingBlockLookingAt_Axe.GetComponent<BuildingBlock>() != null)
                    {
                        if (buildingBlockLookingAt_Axe.GetComponent<BuildingBlock>().buidingBlock_Parent.GetComponent<BuildingBlock_Parent>() != null)
                        {
                            SetBuildingRemoveRequirements(buildingBlockLookingAt_Axe.GetComponent<BuildingBlock>().buidingBlock_Parent.GetComponent<BuildingBlock_Parent>());
                        }
                    }
                }
            }
            else
            {
                if (buildingBlockLookingAt_Axe != null)
                {
                    buildingBlockLookingAt_Axe = null;
                    buildingRemoveRequirement_Parent.SetActive(false);
                }
            }
        }
    }
    public void CutBlock()
    {
        if (Physics.Raycast(oldRay, out hit_Axe))
        {
            if (buildingBlockLookingAt_Axe != null)
            {
                if (buildingBlockLookingAt_Axe.GetComponent<BuildingBlock>().buidingBlock_Parent != null)
                {
                    if (buildingBlockLookingAt_Axe.GetComponent<BuildingBlock>().buidingBlock_Parent.GetComponent<BuildingBlock_Parent>() != null)
                    {
                        for (int i = 0; i < buildingBlockList.Count; i++)
                        {
                            if (buildingBlockLookingAt_Axe.GetComponent<BuildingBlock>().buidingBlock_Parent == buildingBlockList[i])
                            {
                                print("6. Destroy Block");

                                //Play remove sound
                                if (buildingBlockLookingAt_Axe.GetComponent<BuildingBlock>().buidingBlock_Parent.GetComponent<BuildingBlock_Parent>().buildingMaterial == BuildingMaterial.Wood)
                                {
                                    SoundManager.instance.PlayWood_Remove_Clip();
                                }
                                else if (buildingBlockLookingAt_Axe.GetComponent<BuildingBlock>().buidingBlock_Parent.GetComponent<BuildingBlock_Parent>().buildingMaterial == BuildingMaterial.Stone)
                                {
                                    SoundManager.instance.PlayStone_Remove_Clip();
                                }
                                else if (buildingBlockLookingAt_Axe.GetComponent<BuildingBlock>().buidingBlock_Parent.GetComponent<BuildingBlock_Parent>().buildingMaterial == BuildingMaterial.Iron)
                                {
                                    SoundManager.instance.PlayIron_Remove_Clip();
                                }

                                //Add items to inventory
                                for (int j = 0; j < buildingBlockLookingAt_Axe.GetComponent<BuildingBlock>().buidingBlock_Parent.GetComponent<BuildingBlock_Parent>().removeBuildingRequirementList.Count; j++)
                                {
                                    for (int k = 0; k < buildingBlockLookingAt_Axe.GetComponent<BuildingBlock>().buidingBlock_Parent.GetComponent<BuildingBlock_Parent>().removeBuildingRequirementList[j].amount; k++)
                                    {
                                        InventoryManager.Instance.AddItemToInventory(0, buildingBlockLookingAt_Axe.GetComponent<BuildingBlock>().buidingBlock_Parent.GetComponent<BuildingBlock_Parent>().removeBuildingRequirementList[j].itemName);
                                    }
                                }

                                //Remove Building Object
                                buildingBlockList.RemoveAt(i);
                                buildingBlockLookingAt_Axe.GetComponent<BuildingBlock>().buidingBlock_Parent.GetComponent<BuildingBlock_Parent>().DestroyThisObject();

                                //Reset parameters
                                buildingBlockLookingAt_Axe = null;
                                lastBuildingBlock_LookedAt = null;
                                old_lastBuildingBlock_LookedAt = null;

                                if (buildingBlockLookingAt_Axe != null)
                                {
                                    SetBuildingRemoveRequirements(buildingBlockLookingAt_Axe.GetComponent<BuildingBlock>().buidingBlock_Parent.GetComponent<BuildingBlock_Parent>());
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

    public void SetBuildingRequirements(BuildingBlock_Parent blockParent, GameObject ParentObject)
    {
        //Remove all childs
        for (int i = ParentObject.transform.childCount - 1; i >= 0; i--)
        {
            ParentObject.transform.GetChild(i).GetComponent<BuildingRequirementSlot>().DestroyThisObject();
        }
        buildingRequirement_List.Clear();

        //Set "enoughItemsToBuild" = true by default
        enoughItemsToBuild = true;

        //Setup new list of Requirements
        for (int i = 0; i < blockParent.buildingRequirementList.Count; i++)
        {
            //print("2. SetBuildingRequirements");
            buildingRequirement_List.Add(Instantiate(buildingRequirement_Prefab, ParentObject.transform) as GameObject);

            buildingRequirement_List[buildingRequirement_List.Count - 1].GetComponent<BuildingRequirementSlot>().requirement_image.sprite = MainManager.instance.GetItem(blockParent.buildingRequirementList[i].itemName).hotbarSprite;

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
                buildingRequirement_List[buildingRequirement_List.Count - 1].GetComponent<BuildingRequirementSlot>().requirement_BGimage.color = new Color(1, 1, 1, 1);
                //enoughItemsToBuild = true;
            }

            //If not having enough items in inventory
            else
            {
                buildingRequirement_List[buildingRequirement_List.Count - 1].GetComponent<BuildingRequirementSlot>().requirement_BGimage.color = new Color(1, 0, 0, 1);
                enoughItemsToBuild = false;
            }
        }
    }
    public void SetBuildingRequirements(MoveableObjectInfo moveableObject, GameObject ParentObject)
    {
        //Remove all childs
        for (int i = ParentObject.transform.childCount - 1; i >= 0; i--)
        {
            ParentObject.transform.GetChild(i).GetComponent<BuildingRequirementSlot>().DestroyThisObject();
        }
        buildingRequirement_List.Clear();

        //Set "enoughItemsToBuild" = true by default
        enoughItemsToBuild = true;

        //Setup new list of Requirements
        for (int i = 0; i < moveableObject.craftingRequirements.Count; i++)
        {
            //print("2. SetBuildingRequirements");
            buildingRequirement_List.Add(Instantiate(buildingRequirement_Prefab, ParentObject.transform) as GameObject);

            buildingRequirement_List[buildingRequirement_List.Count - 1].GetComponent<BuildingRequirementSlot>().requirement_image.sprite = MainManager.instance.GetItem(moveableObject.craftingRequirements[i].itemName).hotbarSprite;

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
                buildingRequirement_List[buildingRequirement_List.Count - 1].GetComponent<BuildingRequirementSlot>().requirement_BGimage.color = new Color(1, 1, 1, 1);
                //enoughItemsToBuild = true;
            }

            //If not having enough items in inventory
            else
            {
                buildingRequirement_List[buildingRequirement_List.Count - 1].GetComponent<BuildingRequirementSlot>().requirement_BGimage.color = new Color(1, 0, 0, 1);
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

        //Setup new list of Requirements
        for (int i = 0; i < blockParent.removeBuildingRequirementList.Count; i++)
        {
            buildingRemoveRequirement_List.Add(Instantiate(buildingRequirement_Prefab, buildingRemoveRequirement_Parent.transform) as GameObject);

            buildingRemoveRequirement_List[buildingRemoveRequirement_List.Count - 1].GetComponent<BuildingRequirementSlot>().requirement_image.sprite = MainManager.instance.GetItem(blockParent.removeBuildingRequirementList[i].itemName).hotbarSprite;

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