using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionSubPanel : MonoBehaviour
{
    public ItemSubCategories panelName;


    //--------------------


    private void Start()
    {
        //Change Pos/Rot
        gameObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(0f, 0f, 0f));
    }


    //--------------------


    public void SetDisplay()
    {
        gameObject.name = panelName.ToString();

        //Change Pos/Rot
        gameObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(0f, 0f, 0f));
    }
}
