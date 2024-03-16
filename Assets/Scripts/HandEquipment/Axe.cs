using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour, EquippeableItem_Interface
{
    private void Start()
    {
        if (BuildingManager.Instance.Axe_buildingBlockLookingAt)
        {
            BuildingManager.Instance.buildingRemoveRequirement_Parent.SetActive(true);
        }
        else
        {
            BuildingManager.Instance.buildingRemoveRequirement_Parent.SetActive(false);
        }
    }


    //--------------------


    public void CutBlock()
    {
        BuildingManager.Instance.CutBlock();
    }

    public void DestroyThisObject()
    {
        BuildingManager.Instance.buildingRemoveRequirement_Parent.SetActive(false);
    }
}
