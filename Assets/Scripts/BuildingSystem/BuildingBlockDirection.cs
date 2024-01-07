using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBlockDirection : MonoBehaviour
{
    public GameObject parentBlock;

    public BlockDirection_A blockDirection_A;
    public BlockDirection_B blockDirection_B;
    public BuildingType BuildingType;
    public BuildingSubType buildingSubType;
}

public enum BlockDirection_A
{
    None,

    North,
    East,
    South,
    West,

    Cross_A,
    Cross_B
}

public enum BlockDirection_B
{
    None,

    Up,
    Right,
    Down,
    Left
}
