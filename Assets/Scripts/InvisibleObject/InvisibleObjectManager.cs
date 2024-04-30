using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleObjectManager : Singleton<InvisibleObjectManager>
{
    public GameObject sphereObject_Prefab;

    public LayerMask invisibleObjectLayerMask;

    public float flashlight_Distance = 9;
    public float ghostCapture_Distance = 7;
    public float ar�diteCrystal_Distance = 5f;
}
