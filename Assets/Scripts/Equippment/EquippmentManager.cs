using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippmentManager : Singleton<EquippmentManager>
{
    [Header("Parent")]
    public GameObject toolHolderParent;


    //--------------------


    private void Start()
    {
        PlayerButtonManager.isPressed_EquipmentActivate += ActivateEquippedItem;
    }


    //--------------------


    void ActivateEquippedItem()
    {
        print("EquippedItem has been pressed");

        if (toolHolderParent.transform.childCount <= 0)
        {
            return;
        }

        //Check if the selected item has the required states
        if (MainManager.Instance.GetItem(HotbarManager.Instance.selectedItem).isEquipableInHand
            && toolHolderParent.GetComponentInChildren<EquippedItem>() != null)
        {
            //Play animation
            toolHolderParent.GetComponentInChildren<EquippedItem>().HitAnimation();
        }
    }
}
