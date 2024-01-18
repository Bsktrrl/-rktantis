using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleLight : MonoBehaviour, EquippeableItem_Interface
{
    public SphereCollider sphereCollider;
    public Vector3 spherePos;


    //--------------------


    private void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        spherePos = transform.TransformPoint(sphereCollider.center);
    }


    //--------------------


    public void DestroyThisObject()
    {
        
    }
}
