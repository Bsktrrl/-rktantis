using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleLight : MonoBehaviour, EquippeableItem_Interface
{
    public GameObject invisibleCalculationPoint;


    //--------------------


    private void Start()
    {
        invisibleCalculationPoint.transform.SetLocalPositionAndRotation(HotbarManager.Instance.EquipmentHolder.transform.localPosition, Quaternion.identity);
        //invisibleCalculationPoint.transform.SetLocalPositionAndRotation((Vector3.forward * maxCollissionDistance), Quaternion.identity);
    }


    //--------------------


    public void DestroyThisObject()
    {
        
    }
}
