using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippmentManager : MonoBehaviour
{
    public static EquippmentManager instance { get; private set; } //Singleton

    [Header("Parent")]
    public GameObject toolHolderParent;


    //--------------------


    private void Awake()
    {
        //Singleton
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
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
        if (MainManager.instance.GetItem(HotbarManager.instance.selectedItem).isEquipable
            && toolHolderParent.GetComponentInChildren<EquippedItem>() != null)
        {
            //Play animation
            toolHolderParent.GetComponentInChildren<EquippedItem>().HitAnimation();
        }
    }
}
