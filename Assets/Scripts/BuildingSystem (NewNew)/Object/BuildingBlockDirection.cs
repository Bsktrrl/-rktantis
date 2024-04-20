using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBlockDirection : MonoBehaviour
{
    public GameObject buildingBlock_Parent;
    public BuildingBlockColliderDirection buildingBlockColliderDirection;

    public void EnterBlockDirection_BB_Normal()
    {
        BuildingSystemManager.Instance.buildingBlockHit = buildingBlock_Parent;
        BuildingSystemManager.Instance.directionHit = buildingBlockColliderDirection;
    }

    public void EnterBlockDirection_BB_Left()
    {
        BuildingSystemManager.Instance.buildingBlockHit = buildingBlock_Parent;

        switch (buildingBlockColliderDirection)
        {
            case BuildingBlockColliderDirection.None:
                BuildingSystemManager.Instance.directionHit = buildingBlockColliderDirection;
                break;

            case BuildingBlockColliderDirection.Front:
                BuildingSystemManager.Instance.directionHit = BuildingBlockColliderDirection.Right;
                break;
            case BuildingBlockColliderDirection.Back:
                BuildingSystemManager.Instance.directionHit = BuildingBlockColliderDirection.Left;
                break;
            case BuildingBlockColliderDirection.Up:
                break;
            case BuildingBlockColliderDirection.Down:
                break;
            case BuildingBlockColliderDirection.Left:
                BuildingSystemManager.Instance.directionHit = BuildingBlockColliderDirection.Front;
                break;
            case BuildingBlockColliderDirection.Right:
                BuildingSystemManager.Instance.directionHit = BuildingBlockColliderDirection.Back;
                break;

            default:
                BuildingSystemManager.Instance.directionHit = buildingBlockColliderDirection;
                break;
        }
    }
    public void EnterBlockDirection_BB_Right()
    {
        BuildingSystemManager.Instance.buildingBlockHit = buildingBlock_Parent;

        switch (buildingBlockColliderDirection)
        {
            case BuildingBlockColliderDirection.None:
                BuildingSystemManager.Instance.directionHit = buildingBlockColliderDirection;
                break;

            case BuildingBlockColliderDirection.Front:
                BuildingSystemManager.Instance.directionHit = BuildingBlockColliderDirection.Left;
                break;
            case BuildingBlockColliderDirection.Back:
                BuildingSystemManager.Instance.directionHit = BuildingBlockColliderDirection.Right;
                break;
            case BuildingBlockColliderDirection.Up:
                break;
            case BuildingBlockColliderDirection.Down:
                break;
            case BuildingBlockColliderDirection.Left:
                BuildingSystemManager.Instance.directionHit = BuildingBlockColliderDirection.Back;
                break;
            case BuildingBlockColliderDirection.Right:
                BuildingSystemManager.Instance.directionHit = BuildingBlockColliderDirection.Front;
                break;

            default:
                BuildingSystemManager.Instance.directionHit = buildingBlockColliderDirection;
                break;
        }
    }
    public void EnterBlockDirection_BB_Up()
    {
        BuildingSystemManager.Instance.buildingBlockHit = buildingBlock_Parent;
        BuildingSystemManager.Instance.directionHit = buildingBlockColliderDirection;
    }
    public void EnterBlockDirection_BB_Down()
    {
        BuildingSystemManager.Instance.buildingBlockHit = buildingBlock_Parent;
        BuildingSystemManager.Instance.directionHit = buildingBlockColliderDirection;
    }
}
