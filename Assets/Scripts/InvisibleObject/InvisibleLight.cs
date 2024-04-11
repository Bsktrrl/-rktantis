using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleLight : MonoBehaviour
{
    public GameObject invisibleCalculationPoint;


    //--------------------


    private void Start()
    {
        invisibleCalculationPoint.transform.SetLocalPositionAndRotation(HotbarManager.Instance.EquipmentHolder.transform.localPosition, Quaternion.identity);
    }
}
