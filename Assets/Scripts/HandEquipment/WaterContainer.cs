using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterContainer : MonoBehaviour
{
    public GameObject waterMesh;


    //--------------------


    public void SetupWaterContainer()
    {
        //Check if the waterContainer is empty
        if (HotbarManager.Instance.hotbarList[HotbarManager.Instance.selectedSlot].durabilityCurrent > 0)
        {
            if (gameObject.GetComponent<EquippedItem>())
            {
                gameObject.GetComponent<EquippedItem>().BucketWaterlevel(waterMesh);
            }

            ActivateMesh();
        }
        else
        {
            DeactivateMesh();
        }
    }

    public void ActivateMesh()
    {
        waterMesh.SetActive(true);
    }
    public void DeactivateMesh()
    {
        waterMesh.SetActive(false);
    }
}
