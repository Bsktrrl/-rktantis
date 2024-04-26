using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObject : MonoBehaviour
{
    [Header("General")]
    public bool canBePlaced;
    public bool isSelectedForMovement;
    public bool enoughItemsToBuild;

    [Header("MoveableObject Type")]
    public BuildingObjectTypes buildingObjectType;
    public BuildingMaterial buildingMaterial;
    [Space(5)]
    public BuildingBlockObjectNames buildingBlockObjectName;
    public FurnitureObjectNames furnitureObjectName;
    public MachineObjectNames machineObjectName;

    [Header("Mesh")]
    public List<GameObject> modelList = new List<GameObject>();

    [Header("Colliders")]
    public GameObject collidersOnObject;


    //--------------------



    private void OnTriggerExit(Collider other)
    {
        canBePlaced = false;
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
