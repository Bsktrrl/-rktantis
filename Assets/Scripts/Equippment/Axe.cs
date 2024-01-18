using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour, EquippeableItem_Interface
{
    public void CutBlock()
    {
        BuildingManager.Instance.CutBlock();
    }

    public void DestroyThisObject()
    {
        
    }
}
