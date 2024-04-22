using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AríditeCrystal : MonoBehaviour
{
    [Header("Save/Load Object")]
    public bool isPicked;

    public int aríditeCrystalIndex_x;
    public int aríditeCrystalIndex_y;


    //--------------------


    public void AríditeCrystalInteraction()
    {
        if (isPicked) { return; }

        AríditeCrystalManager.Instance.ChangeAríditeCrystalInfo(true, aríditeCrystalIndex_x, aríditeCrystalIndex_y, gameObject.transform.position);
    }
    public void LoadAríditeCrystal(bool _isPicked, int _aríditeCrystalIndex_j, int _aríditeCrystalIndex_l)
    {
        //print("Load AríditeCrystal: [" + _aríditeCrystalIndex_j + "][" + _aríditeCrystalIndex_l + "]: " + _isPicked);

        //Set Parameters
        isPicked = _isPicked;
        aríditeCrystalIndex_x = _aríditeCrystalIndex_j;
        aríditeCrystalIndex_y = _aríditeCrystalIndex_l;

        //Check if Animation and pickablePart should be hidden
        if (isPicked)
        {
            gameObject.SetActive(false);
        }
    }
}
