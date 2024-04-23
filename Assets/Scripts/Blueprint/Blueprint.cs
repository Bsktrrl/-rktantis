using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blueprint : MonoBehaviour
{
    public bool isPicked;

    public int blueprintIndex_x;
    public int blueprintIndex_y;


    //--------------------


    public void BlueprintInteraction()
    {
        if (isPicked) { return; }

        BlueprintManager.Instance.ChangeBlueprintInfo(true, blueprintIndex_x, blueprintIndex_y, gameObject.transform.position);
    }


    //--------------------


    public void LoadBlueprint(bool _isPicked, int _blueprintIndex_j, int _blueprintIndex_l)
    {
        //print("Load Blueprint: [" + _blueprintIndex_j + "][" + _blueprintIndex_l + "]: " + _isPicked);

        //Set Parameters
        isPicked = _isPicked;
        blueprintIndex_x = _blueprintIndex_j;
        blueprintIndex_y = _blueprintIndex_l;

        //Check if Animation and pickablePart should be hidden
        if (isPicked)
        {
            gameObject.SetActive(false);
        }
    }
}
