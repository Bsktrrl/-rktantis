using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingBlockDirection_v2 : MonoBehaviour
{
    public GameObject buildingBlock_Parent;
    public BuildingBlockColliderDirection buildingBlockColliderDirection;

    public void EnterBlockDirection_BB_Normal()
    {
        BuildingManager_v2.Instance.buildingBlockHit = buildingBlock_Parent;
        BuildingManager_v2.Instance.directionHit = buildingBlockColliderDirection;
    }

    public void EnterBlockDirection_BB_Left()
    {
        BuildingManager_v2.Instance.buildingBlockHit = buildingBlock_Parent;

        switch (buildingBlockColliderDirection)
        {
            case BuildingBlockColliderDirection.None:
                BuildingManager_v2.Instance.directionHit = buildingBlockColliderDirection;
                break;

            case BuildingBlockColliderDirection.Front:
                BuildingManager_v2.Instance.directionHit = BuildingBlockColliderDirection.Right;
                break;
            case BuildingBlockColliderDirection.Back:
                BuildingManager_v2.Instance.directionHit = BuildingBlockColliderDirection.Left;
                break;
            case BuildingBlockColliderDirection.Up:
                break;
            case BuildingBlockColliderDirection.Down:
                break;
            case BuildingBlockColliderDirection.Left:
                BuildingManager_v2.Instance.directionHit = BuildingBlockColliderDirection.Front;
                break;
            case BuildingBlockColliderDirection.Right:
                BuildingManager_v2.Instance.directionHit = BuildingBlockColliderDirection.Back;
                break;

            default:
                BuildingManager_v2.Instance.directionHit = buildingBlockColliderDirection;
                break;
        }
    }
    public void EnterBlockDirection_BB_Right()
    {
        BuildingManager_v2.Instance.buildingBlockHit = buildingBlock_Parent;

        switch (buildingBlockColliderDirection)
        {
            case BuildingBlockColliderDirection.None:
                BuildingManager_v2.Instance.directionHit = buildingBlockColliderDirection;
                break;

            case BuildingBlockColliderDirection.Front:
                BuildingManager_v2.Instance.directionHit = BuildingBlockColliderDirection.Left;
                break;
            case BuildingBlockColliderDirection.Back:
                BuildingManager_v2.Instance.directionHit = BuildingBlockColliderDirection.Right;
                break;
            case BuildingBlockColliderDirection.Up:
                break;
            case BuildingBlockColliderDirection.Down:
                break;
            case BuildingBlockColliderDirection.Left:
                BuildingManager_v2.Instance.directionHit = BuildingBlockColliderDirection.Back;
                break;
            case BuildingBlockColliderDirection.Right:
                BuildingManager_v2.Instance.directionHit = BuildingBlockColliderDirection.Front;
                break;

            default:
                BuildingManager_v2.Instance.directionHit = buildingBlockColliderDirection;
                break;
        }
    }
    public void EnterBlockDirection_BB_Up()
    {
        BuildingManager_v2.Instance.buildingBlockHit = buildingBlock_Parent;
        BuildingManager_v2.Instance.directionHit = buildingBlockColliderDirection;
    }
    public void EnterBlockDirection_BB_Down()
    {
        BuildingManager_v2.Instance.buildingBlockHit = buildingBlock_Parent;
        BuildingManager_v2.Instance.directionHit = buildingBlockColliderDirection;
    }
}
