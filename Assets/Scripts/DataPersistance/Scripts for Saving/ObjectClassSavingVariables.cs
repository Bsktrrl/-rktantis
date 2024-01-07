using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ObjectClassSavingVariables
{
    [Header("Universal")]
    //public ObjectType objectType;
    public Vector3 position;

    [Header("Inventory")]
    public int worldObjectIndex;
    public int inventoryIndex;
}