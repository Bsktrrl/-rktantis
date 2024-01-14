using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;

public class BuildingHammer : MonoBehaviour
{
    [SerializeField] LayerMask layerMask_Ground;
    public GameObject tempObj_Selected = null;

    Ray ray;
    RaycastHit hit;


    //--------------------


    private void Update()
    {
        UpdateSelectedBlockPosition();
    }


    //--------------------


    public void SetNewSelectedBlock()
    {
        Destroy(tempObj_Selected);
        tempObj_Selected = null;

        //If selected Object is a BuildingBlock
        if (MoveableObjectManager.Instance.moveableObjectType == MoveableObjectType.BuildingBlock)
        {
            print("1000. selected Object is a BuildingBlock");

            if (MoveableObjectManager.Instance.buildingType_Selected != BuildingType.None && MoveableObjectManager.Instance.buildingMaterial_Selected != BuildingMaterial.None)
            {
                //Instantiate new tempBlock as this BuildingBlock
                BuildingBlock_Parent temp = BuildingManager.instance.GetBuildingBlock(MoveableObjectManager.Instance.buildingType_Selected, MoveableObjectManager.Instance.buildingMaterial_Selected);

                for (int i = 0; i < temp.ghostList.Count; i++)
                {
                    if (temp.ghostList[i].GetComponent<Building_Ghost>().buildingType == MoveableObjectManager.Instance.buildingType_Selected)
                    {
                        tempObj_Selected = Instantiate(temp.ghostList[i], InventoryManager.instance.handDropPoint.transform.position, Quaternion.identity) as GameObject;
                        tempObj_Selected.transform.parent = BuildingManager.instance.tempBlock_Parent.transform;

                        //Get the correct mesh
                        tempObj_Selected.GetComponent<MeshFilter>().mesh = BuildingManager.instance.GetCorrectGhostMesh(tempObj_Selected);
                        tempObj_Selected.GetComponent<MeshRenderer>().material = BuildingManager.instance.canPlace_Material;
                        break;
                    }
                }
            }
        }

        //If selected Object is a Machine or furniture
        else if (MoveableObjectManager.Instance.moveableObjectType == MoveableObjectType.Machine
            || MoveableObjectManager.Instance.moveableObjectType == MoveableObjectType.Furniture)
        {
            print("1000. Selected Object is a Machine");

            GameObject moveableObject = MoveableObjectManager.Instance.GetMoveableObject(MoveableObjectManager.Instance.moveableObjectType);
            MoveableObjectManager.Instance.objectToMove = moveableObject;

            tempObj_Selected = Instantiate(moveableObject, InventoryManager.instance.handDropPoint.transform.position, Quaternion.identity) as GameObject;
            tempObj_Selected.transform.parent = BuildingManager.instance.tempBlock_Parent.transform;

            //Get the correct mesh
            tempObj_Selected.GetComponent<MeshRenderer>().material = BuildingManager.instance.canPlace_Material;
        }
    }

    public void UpdateSelectedBlockPosition()
    {
        if (tempObj_Selected == null) { return; }

        //If any menu is open, return
        if (MainManager.instance.menuStates != MenuStates.None)
        {
            if (tempObj_Selected != null)
            {
                BuildingManager.instance.freeGhost_LookedAt = null;
                tempObj_Selected.SetActive(false);
            }

            return;
        }

        //If player is looking at a Ghost, return
        if (BuildingManager.instance.ghost_LookedAt != null)
        {
            if (tempObj_Selected != null)
            {
                BuildingManager.instance.freeGhost_LookedAt = null;
                tempObj_Selected.SetActive(false);
            }

            return;
        }

        //If player is looking at a buildingBlock
        if (BuildingManager.instance.BlockTagName == "BuildingBlock")
        {
            if (tempObj_Selected != null)
            {
                BuildingManager.instance.freeGhost_LookedAt = null;
                tempObj_Selected.SetActive(false);
            }

            return;
        }

        if (BuildingManager.instance.blockisPlacing)
        {
            return;
        }


        //-----


        //Check if item can be placed and change its material accordingly
        if (BuildingManager.instance.enoughItemsToBuild)
        {
            tempObj_Selected.GetComponent<MeshRenderer>().material = BuildingManager.instance.canPlace_Material;
        }
        else
        {
            tempObj_Selected.GetComponent<MeshRenderer>().material = BuildingManager.instance.cannotPlace_Material;
        }


        //-----


        //Set Object's Position and Rotation
        if (tempObj_Selected.GetComponent<Building_Ghost>() != null)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, BuildingManager.instance.BuildingDistance.x, layerMask_Ground))
            {
                //Set the object's position to the ground height
                BuildingManager.instance.freeGhost_LookedAt = tempObj_Selected;
                tempObj_Selected.SetActive(true);

                tempObj_Selected.transform.SetPositionAndRotation(new Vector3(hit.point.x, hit.point.y + 1f, hit.point.z), Quaternion.LookRotation(new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z)));
            }
            else
            {
                BuildingManager.instance.freeGhost_LookedAt = null;
                tempObj_Selected.SetActive(false);
            }
        }
        else
        {
            BuildingManager.instance.freeGhost_LookedAt = null;
            tempObj_Selected.SetActive(false);
        }
    }


    //--------------------


    public void PlaceBlock()
    {
        if (MoveableObjectManager.Instance.moveableObjectType == MoveableObjectType.BuildingBlock)
        {
            BuildingManager.instance.PlaceBlock();
        }
        else if (MoveableObjectManager.Instance.moveableObjectType == MoveableObjectType.Machine)
        {
            MoveableObjectManager.Instance.PlaceObjectToMove();
        }
        else if (MoveableObjectManager.Instance.moveableObjectType == MoveableObjectType.Furniture)
        {
            MoveableObjectManager.Instance.PlaceObjectToMove();
        }
    }
}
