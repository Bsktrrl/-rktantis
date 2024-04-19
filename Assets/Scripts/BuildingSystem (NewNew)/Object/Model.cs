using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model : MonoBehaviour
{
    private void Update()
    {
        if (gameObject.transform.parent.gameObject == BuildingSystemManager.Instance.ghostObject_Holding
            && gameObject.transform.parent.gameObject.GetComponent<MoveableObject>().buildingObjectType == BuildingObjectTypes.BuildingBlock)
        {
            //Get the bounds of the MeshCollider
            Bounds colliderBounds = GetComponent<MeshCollider>().bounds;

            //Check for overlapping colliders within the bounds of the MeshCollider
            Collider[] collidersInside = Physics.OverlapBox(colliderBounds.center, colliderBounds.extents * 0.58f, Quaternion.identity);

            if (collidersInside.Length > 0)
            {
                bool isColliding = false;
                bool isSuperColliding = false;

                foreach (Collider collider in collidersInside)
                {
                    if (collider.gameObject != gameObject //Don't collide with itself
                        && collider.gameObject != MainManager.Instance.playerBody //Don't collide with player
                        && collider.gameObject != MainManager.Instance.player //Don't collide with player
                        && !collider.gameObject.CompareTag("Ground") //Don't collide with the ground
                        && collider.gameObject.layer != 10 //Don't collide with invisibleSphereColliders
                        && (collider.gameObject.layer == 7 || collider.gameObject.layer == 8)) //Don't collide with other BuildingBlocks
                    {
                        isSuperColliding = true;

                        break;
                    }

                    if (collider.gameObject != gameObject //Don't collide with itself
                        && collider.gameObject != MainManager.Instance.playerBody //Don't collide with player
                        && collider.gameObject != MainManager.Instance.player //Don't collide with player
                        && !collider.gameObject.CompareTag("Ground") //Don't collide with the ground
                        && collider.gameObject.layer != 10) //Don't collide with invisibleSphereColliders
                    {
                        isColliding = true;
                        BuildingSystemManager.Instance.CanPlaceBuildingObjectCheck();

                        break;
                    }
                }

                //Check the outcome - If colliding with anything special
                if (isSuperColliding)
                {
                    BuildingSystemManager.Instance.isCollidingWithBuildingBlock = true;
                    BuildingSystemManager.Instance.isColliding = true;
                    BuildingSystemManager.Instance.CanPlaceBuildingObjectCheck();
                }
                else if (isColliding)
                {
                    BuildingSystemManager.Instance.isColliding = true;
                    BuildingSystemManager.Instance.isCollidingWithBuildingBlock = false;
                    BuildingSystemManager.Instance.CanPlaceBuildingObjectCheck();
                }
                else
                {
                    BuildingSystemManager.Instance.isColliding = false;
                    BuildingSystemManager.Instance.isCollidingWithBuildingBlock = false;
                    BuildingSystemManager.Instance.CanPlaceBuildingObjectCheck();
                }
            }
            else
            {
                BuildingSystemManager.Instance.isColliding = false;
                BuildingSystemManager.Instance.isCollidingWithBuildingBlock = false;
                BuildingSystemManager.Instance.CanPlaceBuildingObjectCheck();
            }
        }
    }
}
