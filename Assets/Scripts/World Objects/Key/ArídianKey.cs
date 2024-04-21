using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArídianKey : MonoBehaviour
{
    [Header("Save/Load Object")]
    public bool isPicked;

    public int arídianKeyIndex_x;
    public int arídianKeyIndex_y;


    //--------------------


    public void ArídianKeyInteraction()
    {
        if (isPicked) { return; }

        ArídianKeyManager.Instance.ChangeArídianKeyInfo(true, arídianKeyIndex_x, arídianKeyIndex_y, gameObject.transform.position);
    }
    public void LoadArídianKey(bool _isPicked, int _arídianKeyIndex_j, int _arídianKeyIndex_l)
    {
        print("Load ArídianKey: [" + _arídianKeyIndex_j + "][" + _arídianKeyIndex_l + "]: " + _isPicked);

        //Set Parameters
        isPicked = _isPicked;
        arídianKeyIndex_x = _arídianKeyIndex_j;
        arídianKeyIndex_y = _arídianKeyIndex_l;

        //Check if Animation and pickablePart should be hidden
        if (isPicked)
        {
            gameObject.SetActive(false);
        }
    }
}
