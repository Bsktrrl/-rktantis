using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    public void CutBlock()
    {
        BuildingManager.instance.CutBlock();
    }
}
