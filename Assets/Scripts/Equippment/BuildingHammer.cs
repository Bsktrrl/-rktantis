using System.Collections;
using UnityEngine;

public class BuildingHammer : MonoBehaviour
{
    [SerializeField] LayerMask layerMask_Ground;
    [SerializeField] LayerMask layerMask_BuildingBlock;
    public GameObject tempObj_Selected = null;

    [SerializeField] float rotationSpeed = 100;
    [SerializeField] float rotationValue = 0;

    Ray ray;
    RaycastHit hit;


    //--------------------


    private void Start()
    {
        PlayerButtonManager.isPressed_MoveableRotation_Right += ManipulateObjectRotation_Right;
        PlayerButtonManager.isPressed_MoveableRotation_Left += ManipulateObjectRotation_Left;

        rotationSpeed = 150;
    }
    private void Update()
    {
        UpdateSelectedBlockPosition();
        UpdateObjectToMovePosition();
    }


    //--------------------


    public void SetNewSelectedBlock()
    {
        //Reset Rotation
        rotationValue = 0;

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
        
        //If selected Object is a BuildingBlock
        if (MoveableObjectManager.Instance.moveableObjectType == MoveableObjectType.BuildingBlock)
        {
            print("1000. selected Object is a BuildingBlock");

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
            print("1000. Selected Object is a Machine or Furniture");

            GameObject moveableObject = MoveableObjectManager.Instance.GetMoveableObject();
            MoveableObjectManager.Instance.objectToMove = moveableObject;

            tempObj_Selected = Instantiate(moveableObject, InventoryManager.Instance.handDropPoint.transform.position, Quaternion.identity) as GameObject;
            tempObj_Selected.transform.parent = BuildingManager.Instance.tempBlock_Parent.transform;

            //Set selected For Movement
            tempObj_Selected.GetComponent<MoveableObject>().isSelectedForMovement = true;

            //Get the correct mesh
            tempObj_Selected.GetComponent<MeshRenderer>().material = BuildingManager.Instance.canPlace_Material;
        }
    }


    //--------------------


    public void UpdateSelectedBlockPosition()
    {
        if (tempObj_Selected == null) { return; }

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

                tempObj_Selected.transform.SetPositionAndRotation(new Vector3(hit.point.x, hit.point.y + 1f, hit.point.z), Quaternion.LookRotation(new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z)));
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
        //If tempObj_Selected isn't selected, return
        if (tempObj_Selected == null) { return; }

        //If any menu is open, return
        if (MainManager.Instance.menuStates != MenuStates.None)
        {
            if (tempObj_Selected != null)
            {
                tempObj_Selected.SetActive(false);
            }

            return;
        }


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
                    tempObj_Selected.GetComponent<MoveableObject>().canBePlaced = true;

                    SetObjectMaterial(BuildingManager.Instance.canPlace_Material);
                    SetMoveableObjectPositionAndRotation();
                }
                else if (Physics.Raycast(ray, out hit, BuildingManager.Instance.BuildingDistance.x, layerMask_Ground))
                {
                    SetObjectMaterial(BuildingManager.Instance.cannotPlace_Material);
                    SetMoveableObjectPositionAndRotation();
                }
                else
                {
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
        //Change "if object can be placed"-material
        tempObj_Selected.GetComponent<MeshRenderer>().material = material;
    }
    void SetMoveableObjectPositionAndRotation()
    {
        //Set the object's position to the buildingBlock height
        float tempHeight = tempObj_Selected.transform.localScale.y / 2;
        tempObj_Selected.transform.SetPositionAndRotation(new Vector3(hit.point.x, hit.point.y + tempHeight, hit.point.z), Quaternion.Euler(0.0f, rotationValue, 0.0f));
    }
    void ManipulateObjectRotation_Right()
    {
        print("Rotating Right");

        rotationValue += rotationSpeed * Time.deltaTime;
    }
    void ManipulateObjectRotation_Left()
    {
        print("Rotating Left");

        rotationValue -= rotationSpeed * Time.deltaTime;
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
    }
}
