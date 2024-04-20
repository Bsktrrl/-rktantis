using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MachineObjects", menuName = "MachineObjects", order = 1)]
public class Machines_SO : ScriptableObject
{
    public List<MachineInfo> machineObjectsList = new List<MachineInfo>();
}

[Serializable]
public class MachineInfo
{
    [Header("General")]
    public string objectName;
    public MachineObjectNames machinesName;
    public BuildingObjectTypes buildingObjectType = BuildingObjectTypes.Machine;

    public BuildingObjectsInfo objectInfo;
}
