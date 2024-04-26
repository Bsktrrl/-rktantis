using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model : MonoBehaviour
{
    private void Update()
    {
        //BuildingBlock
        if (gameObject.transform.parent.gameObject == BuildingSystemManager.Instance.ghostObject_Holding
            && gameObject.transform.parent.gameObject.GetComponent<MoveableObject>().buildingObjectType == BuildingObjectTypes.BuildingBlock)
        {
            if (BuildingSystemManager.Instance.isSnapping)
            {
                BuildingSystemManager.Instance.isColliding = false;
                BuildingSystemManager.Instance.isCollidingWithBuildingBlock = false;
            }
            else
            {
                //Get the bounds of the MeshCollider
                Bounds colliderBounds = GetComponent<MeshCollider>().bounds;

                //Check for overlapping colliders within the bounds of the MeshCollider
                Collider[] collidersInside = Physics.OverlapBox(colliderBounds.center, colliderBounds.extents/* * 0.58f*/, Quaternion.identity);

                if (collidersInside.Length > 0)
                {
                    bool isCollidingCheck = false;
                    bool isSuperCollidingCheck = false;

                    foreach (Collider collider in collidersInside)
                    {
                        if (collider.gameObject != gameObject //Don't collide with itself
                            && collider.gameObject != MainManager.Instance.playerBody //Don't collide with player
                            && collider.gameObject != MainManager.Instance.player //Don't collide with player
                            && !collider.gameObject.CompareTag("Ground") //Don't collide with the ground
                            && collider.gameObject.layer != 10 //Don't collide with invisibleSphereColliders
                            && collider.gameObject.layer != 7) //Don't collide with other BuildingBlocks
                        {
                            isSuperCollidingCheck = true;

                            break;
                        }

                        if (collider.gameObject != gameObject //Don't collide with itself
                            && collider.gameObject != MainManager.Instance.playerBody //Don't collide with player
                            && collider.gameObject != MainManager.Instance.player //Don't collide with player
                            && !collider.gameObject.CompareTag("Ground") //Don't collide with the ground
                            && collider.gameObject.layer != 10 //Don't collide with invisibleSphereColliders
                            && collider.gameObject.layer != 7 //Don't collide with other BuildingBlocks
                            )
                        {
                            isCollidingCheck = true;

                            break;
                        }
                    }

                    //Check the outcome - If colliding with anything special
                    if (isSuperCollidingCheck)
                    {
                        BuildingSystemManager.Instance.isColliding = false;
                        BuildingSystemManager.Instance.isCollidingWithBuildingBlock = true;
                    }
                    else if (isCollidingCheck)
                    {
                        BuildingSystemManager.Instance.isColliding = true;
                        BuildingSystemManager.Instance.isCollidingWithBuildingBlock = false;
                    }
                    else
                    {
                        BuildingSystemManager.Instance.isColliding = false;
                        BuildingSystemManager.Instance.isCollidingWithBuildingBlock = false;
                    }
                }
                else
                {
                    BuildingSystemManager.Instance.isColliding = false;
                    BuildingSystemManager.Instance.isCollidingWithBuildingBlock = false;
                }
            }
        }
    }
}
