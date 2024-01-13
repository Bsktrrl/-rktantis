using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveableObjectManager : Singleton<MoveableObjectManager>
{
    [Header("Main Type")]
    public MoveableObjectType moveableObjectType = MoveableObjectType.None;

    [Header("FurnitureType Type")]
    public FurnitureType furnitureType = FurnitureType.None;

    [Header("MachineType Type")]
    public MachineType machineType = MachineType.None;

    [Header("Building Type")]
    public BuildingType buildingType_Selected = BuildingType.None;
    public BuildingMaterial buildingMaterial_Selected = BuildingMaterial.None;
}
