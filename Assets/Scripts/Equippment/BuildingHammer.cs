using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class BuildingHammer : MonoBehaviour, EquippeableItem_Interface
{
    [SerializeField] LayerMask layerMask_Ground;
    [SerializeField] LayerMask layerMask_BuildingBlock;
    public GameObject tempObj_Selected = null;

    [SerializeField] float rotationSpeed = 75;

    Ray ray;
    RaycastHit hit;


    //--------------------


    private void Start()
    {
        PlayerButtonManager.isPressed_MoveableRotation_Right += ManipulateObjectRotation_Right;
        PlayerButtonManager.isPressed_MoveableRotation_Left += ManipulateObjectRotation_Left;

        rotationSpeed = 150;

        SetNewSelectedBlock();
    }
    private void Update()
    {
        UpdateSelectedBlockPosition();
        UpdateObjectToMovePosition();
    }


    //--------------------


    public void SetNewSelectedBlock()
    {
        BuildingManager.Instance.buildingRequirement_Parent.SetActive(true);

        if (tempObj_Selected)
        {
            if (tempObj_Selected.GetComponent<InteractableObject>())
            {
                tempObj_Selected.GetComponent<InteractableObject>().DestroyThisObject();
                tempObj_Selected = null;
            }
            else
            {
                Destroy(tempObj_Selected);
                tempObj_Selected = null;
            }
        }

        //If selected Object is Empty
        if (MoveableObjectManager.Instance.moveableObjectType == MoveableObjectType.None)
        {
            tempObj_Selected = null;
        }

        //If selected Object is a BuildingBlock
        else if (MoveableObjectManager.Instance.moveableObjectType == MoveableObjectType.BuildingBlock)
        {
            if (MoveableObjectManager.Instance.buildingType_Selected != BuildingType.None && MoveableObjectManager.Instance.buildingMaterial_Selected != BuildingMaterial.None)
            {
                //Instantiate new tempBlock as this BuildingBlock
                BuildingBlock_Parent temp = BuildingManager.Instance.GetBuildingBlock(MoveableObjectManager.Instance.buildingType_Selected, MoveableObjectManager.Instance.buildingMaterial_Selected);

                for (int i = 0; i < temp.ghostList.Count; i++)
                {
                    if (temp.ghostList[i].GetComponent<Building_Ghost>().buildingType == MoveableObjectManager.Instance.buildingType_Selected)
                    {
                        tempObj_Selected = Instantiate(temp.ghostList[i], InventoryManager.Instance.handDropPoint.transform.position, Quaternion.identity) as GameObject;
                        tempObj_Selected.transform.parent = BuildingManager.Instance.tempBlock_Parent.transform;

                        //Get the correct mesh
                        tempObj_Selected.GetComponent<MeshFilter>().mesh = BuildingManager.Instance.GetCorrectGhostMesh(tempObj_Selected);
                        tempObj_Selected.GetComponent<MeshRenderer>().material = BuildingManager.Instance.canPlace_Material;

                        //Remove its BoxCollider
                        if (tempObj_Selected.GetComponent<BoxCollider>())
                        {
                            tempObj_Selected.GetComponent<BoxCollider>().enabled = !tempObj_Selected.GetComponent<BoxCollider>().enabled;
                        }
                        break;
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

                tempObj_Selected = Instantiate(moveableObject, InventoryManager.Instance.handDropPoint.transform.position, Quaternion.identity) as GameObject;
                tempObj_Selected.transform.parent = BuildingManager.Instance.tempBlock_Parent.transform;

                //Set selected For Movement
                tempObj_Selected.GetComponent<MoveableObject>().isSelectedForMovement = true;

                //Get the correct material
                //tempObj_Selected.GetComponent<MoveableObject>().meshRenderer.material = BuildingManager.Instance.canPlace_Material;

                //Remove its BoxCollider
                if (tempObj_Selected.GetComponent<BoxCollider>())
                {
                    tempObj_Selected.GetComponent<BoxCollider>().enabled = !tempObj_Selected.GetComponent<BoxCollider>().enabled;
                }
            }
        }
    }


    //--------------------


    public void UpdateSelectedBlockPosition()
    {
        #region Return Section
        if (tempObj_Selected == null) { return; }

        //If selected block is a MoveableObject, return
        if (tempObj_Selected.GetComponent<MoveableObject>())
        {
            return;
        }

        //If any menu is open, return
        if (MainManager.Instance.menuStates != MenuStates.None)
        {
            if (tempObj_Selected != null)
            {
                BuildingManager.Instance.freeGhost_LookedAt = null;
                tempObj_Selected.SetActive(false);
            }

            return;
        }

        //If player is looking at a Ghost, return
        if (BuildingManager.Instance.ghost_LookedAt != null)
        {
            if (tempObj_Selected != null)
            {
                BuildingManager.Instance.freeGhost_LookedAt = null;
                tempObj_Selected.SetActive(false);
            }

            return;
        }

        //If player is looking at a buildingBlock
        if (BuildingManager.Instance.BlockTagName == "BuildingBlock")
        {
            if (tempObj_Selected != null)
            {
                BuildingManager.Instance.freeGhost_LookedAt = null;
                tempObj_Selected.SetActive(false);
            }

            return;
        }

        if (BuildingManager.Instance.blockisPlacing)
        {
            return;
        }
        #endregion


        //-----


        //Check if item can be placed and change its material accordingly
        if (BuildingManager.Instance.enoughItemsToBuild)
        {
            tempObj_Selected.GetComponent<MeshRenderer>().material = BuildingManager.Instance.canPlace_Material;
        }
        else
        {
            tempObj_Selected.GetComponent<MeshRenderer>().material = BuildingManager.Instance.cannotPlace_Material;
        }


        //-----


        //Set Object's Position and Rotation
        if (tempObj_Selected.GetComponent<Building_Ghost>() != null)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, BuildingManager.Instance.BuildingDistance.x, layerMask_Ground))
            {
                //Set the object's position to the ground height
                BuildingManager.Instance.freeGhost_LookedAt = tempObj_Selected;
                tempObj_Selected.SetActive(true);

                tempObj_Selected.transform.SetPositionAndRotation(new Vector3(hit.point.x, hit.point.y + 1f, hit.point.z), Quaternion.Euler(0.0f, BuildingManager.Instance.rotationValue, 0.0f));
            }
            else
            {
                BuildingManager.Instance.freeGhost_LookedAt = null;
                tempObj_Selected.SetActive(false);
            }
        }
        else
        {
            BuildingManager.Instance.freeGhost_LookedAt = null;
            tempObj_Selected.SetActive(false);
        }
    }


    //--------------------


    public void UpdateObjectToMovePosition()
    {
        #region Return Section
        //If tempObj_Selected isn't selected, return
        if (tempObj_Selected == null) { return; }

        //If selected block is a BuildingBlock, return
        if (tempObj_Selected.GetComponent<Building_Ghost>())
        {
            return;
        }

        //If any menu is open, return
        if (MainManager.Instance.menuStates != MenuStates.None)
        {
            if (tempObj_Selected != null)
            {
                tempObj_Selected.SetActive(false);
            }

            return;
        }
        #endregion


        //-----


        //Set Object's Position and Rotation
        if (tempObj_Selected != null)
        {
            tempObj_Selected.SetActive(true);

            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //Get the selected MoveableObject
            MoveableObjectInfo tempInfo = MoveableObjectManager.Instance.GetMoveableObject_SO();

            //If Player inventory has the required Items
            if (InventoryManager.Instance.GetInventoryRequirements(0, tempInfo.craftingRequirements))
            {
                tempObj_Selected.GetComponent<MoveableObject>().enoughItemsToBuild = true;

                if (Physics.Raycast(ray, out hit, BuildingManager.Instance.BuildingDistance.x, layerMask_BuildingBlock))
                {
                    //print("333. canPlace_Material");

                    tempObj_Selected.GetComponent<MoveableObject>().canBePlaced = true;

                    SetObjectMaterial(BuildingManager.Instance.canPlace_Material);
                    SetMoveableObjectPositionAndRotation();
                }
                else if (Physics.Raycast(ray, out hit, BuildingManager.Instance.BuildingDistance.x, layerMask_Ground))
                {
                    //print("333. cannotPlace_Material");

                    SetObjectMaterial(BuildingManager.Instance.cannotPlace_Material);
                    SetMoveableObjectPositionAndRotation();
                }
                else
                {
                    SetObjectMaterial(BuildingManager.Instance.cannotPlace_Material);
                    tempObj_Selected.SetActive(false);
                    tempObj_Selected.GetComponent<MoveableObject>().canBePlaced = false;
                }
            }

            //If Player inventory doesn't have the required Items
            else
            {
                tempObj_Selected.GetComponent<MoveableObject>().enoughItemsToBuild = false;
                tempObj_Selected.GetComponent<MoveableObject>().canBePlaced = false;

                if (Physics.Raycast(ray, out hit, BuildingManager.Instance.BuildingDistance.x, layerMask_BuildingBlock))
                {
                    SetObjectMaterial(BuildingManager.Instance.cannotPlace_Material);
                    SetMoveableObjectPositionAndRotation();
                }
                else if (Physics.Raycast(ray, out hit, BuildingManager.Instance.BuildingDistance.x, layerMask_Ground))
                {
                    SetObjectMaterial(BuildingManager.Instance.cannotPlace_Material);
                    SetMoveableObjectPositionAndRotation();
                }
                else
                {
                    SetObjectMaterial(BuildingManager.Instance.cannotPlace_Material);
                    tempObj_Selected.SetActive(false);
                    tempObj_Selected.GetComponent<MoveableObject>().canBePlaced = false;
                }
            }
        }
        else
        {
            tempObj_Selected.GetComponent<MoveableObject>().enoughItemsToBuild = false;

            tempObj_Selected.GetComponent<MoveableObject>().canBePlaced = false;
            tempObj_Selected.SetActive(false);
        }
    }
    void SetObjectMaterial(Material material)
    {
        List<Material> objectMaterials = tempObj_Selected.GetComponent<MoveableObject>().meshRenderer.materials.ToList();
        
        print("MaterialCount: " + objectMaterials.Count + " | Material: " + material.name);

        for (int i = 0; i < objectMaterials.Count; i++)
        {
            objectMaterials[i] = material;
        }

        tempObj_Selected.GetComponent<MoveableObject>().meshRenderer.materials = objectMaterials.ToArray();
    }
    void SetMoveableObjectPositionAndRotation()
    {
        tempObj_Selected.transform.SetPositionAndRotation(new Vector3(hit.point.x, hit.point.y - 0.03f /*+ tempHeight*/, hit.point.z), Quaternion.Euler(0.0f, BuildingManager.Instance.rotationValue, 0.0f));
    }


    //--------------------


    void ManipulateObjectRotation_Right()
    {
        print("Rotating Right");

        BuildingManager.Instance.rotationValue += rotationSpeed * Time.deltaTime;
    }
    void ManipulateObjectRotation_Left()
    {
        print("Rotating Left");

        BuildingManager.Instance.rotationValue -= rotationSpeed * Time.deltaTime;
    }


    //--------------------


    public void PlaceBlock()
    {
        if (MoveableObjectManager.Instance.moveableObjectType == MoveableObjectType.BuildingBlock)
        {
            BuildingManager.Instance.PlaceBlock();
        }
        else if (MoveableObjectManager.Instance.moveableObjectType == MoveableObjectType.Machine
            || MoveableObjectManager.Instance.moveableObjectType == MoveableObjectType.Furniture)
        {
            if (tempObj_Selected)
            {
                if (tempObj_Selected.GetComponent<MoveableObject>())
                {
                    MoveableObjectManager.Instance.PlaceObjectToMove(tempObj_Selected.GetComponent<MoveableObject>());
                }
            }
        }
    }


    //--------------------


    public void DestroyThisObject()
    {
        PlayerButtonManager.isPressed_MoveableRotation_Right -= ManipulateObjectRotation_Right;
        PlayerButtonManager.isPressed_MoveableRotation_Left -= ManipulateObjectRotation_Left;

        BuildingManager.Instance.buildingRequirement_Parent.SetActive(false);

        MoveableObjectManager.Instance.objectToMove = null;
    }
}
