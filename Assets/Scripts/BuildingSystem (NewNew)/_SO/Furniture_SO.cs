using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FurnitureObjects", menuName = "FurnitureObjects", order = 1)]
public class Furniture_SO : ScriptableObject
{
    public List<FurnitureInfo> furnitureObjectsList = new List<FurnitureInfo>();
}

[Serializable]
public class FurnitureInfo
{
    [Header("General")]
    public string objectName;
    public FurnitureObjectNames furnitureName;
    public BuildingObjectTypes buildingObjectType = BuildingObjectTypes.Furniture;

    public BuildingObjectsInfo objectInfo;
}