using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBlock_Parent : MonoBehaviour
{
    public int blockID;
    public BuildingType buildingType;
    public BuildingSubType buildingSubType;
    public BuildingMaterial buildingMaterial;

    public GameObject BuildingBlock;

    public List<GameObject> directionObjectList = new List<GameObject>();
    public List<GameObject> ghostList = new List<GameObject>();

    public List<BuildingBlockRequirement> buildingRequirementList = new List<BuildingBlockRequirement>();
    public List<BuildingBlockRequirement> removeBuildingRequirementList = new List<BuildingBlockRequirement>();


    //--------------------


    private void Start()
    {
        //Let the player don't collide with all directionObjects
        for (int i = 0; i < directionObjectList.Count; i++)
        {
            Physics.IgnoreCollision(MainManager.instance.player.GetComponent<CharacterController>(), directionObjectList[i].GetComponent<Collider>(), true);
        }

        //Let the player don't collide with all ghostListObjects
        for (int i = 0; i < ghostList.Count; i++)
        {
            Physics.IgnoreCollision(MainManager.instance.player.GetComponent<CharacterController>(), ghostList[i].GetComponent<Collider>(), true);
        }
    }

    public void DestroyThisObject()
    {
        Destroy(gameObject);
    }
}

[Serializable]
public class BlockPlaced
{
    public GameObject buildingBlock;
    public BuildingType buildingType;
    public BuildingSubType buildingSubType;

    public bool isStrange;
}


[Serializable]
public class BuildingBlockRequirement
{
    public Items itemName;
    public int amount;
}