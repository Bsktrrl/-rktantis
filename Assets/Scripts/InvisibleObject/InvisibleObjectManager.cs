using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleObjectManager : Singleton<InvisibleObjectManager>
{
    public GameObject sphereObject_Prefab;

    public LayerMask invisibleObjectLayerMask;

    public float flashlight_Distance = 9.5f;
    public float ghostCapture_Distance = 7.5f;
    public float aríditeCrystal_Distance = 5.5f;
}
