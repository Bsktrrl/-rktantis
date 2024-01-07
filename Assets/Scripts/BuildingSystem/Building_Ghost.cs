using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Ghost : MonoBehaviour
{
    [Header("Type")]
    public BuildingType buildingType;
    public BuildingSubType buildingSubType;

    [Header("Directions")]
    public BlockDirection_A blockDirection_A;
    public BlockDirection_B blockDirection_B;

    [Header("Other")]
    public GameObject blockParent;
    public bool isSelected;
}
