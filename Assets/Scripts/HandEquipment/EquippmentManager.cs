using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippmentManager : Singleton<EquippmentManager>
{
    [Header("ToolHolder")]
    public GameObject toolHolderParent;

    [Header("Arm Model")]
    public GameObject arms;

    [Header("States")]
    public ArmState armState;


    //--------------------


    private void Start()
    {
        PlayerButtonManager.isPressed_EquipmentActivate += ActivateEquippedItem;
    }


    //--------------------


    public void GetArmState(Items selectedItem)
    {
        if (selectedItem == Items.WoodBuildingHammer || selectedItem == Items.StoneBuildingHammer || selectedItem == Items.CryoniteBuildingHammer
            || selectedItem == Items.WoodAxe || selectedItem == Items.StoneAxe || selectedItem == Items.CryoniteAxe
            || selectedItem == Items.WoodPickaxe || selectedItem == Items.StonePickaxe || selectedItem == Items.CryonitePickaxe
            || selectedItem == Items.WoodSword || selectedItem == Items.StoneSword || selectedItem == Items.CryoniteSword)
        {
            print("444. Tool Category");

            armState = ArmState.Tools;
            arms.GetComponent<ArmsTest>().anim.SetInteger("ItemCategory", 1);
            arms.GetComponent<ArmsTest>().anim.SetTrigger("ItemUpdate");
        }
        else if (selectedItem == Items.Flashlight)
        {
            print("444. Flashlight Category");

            armState = ArmState.Flashlight;
            arms.GetComponent<ArmsTest>().anim.SetInteger("ItemCategory", 2);
            arms.GetComponent<ArmsTest>().anim.SetTrigger("ItemUpdate");
        }
        else if (selectedItem == Items.AríditeCrystal)
        {
            print("444. AríditeCrystal Category");

            armState = ArmState.Crystal;
            arms.GetComponent<ArmsTest>().anim.SetInteger("ItemCategory", 3);
            arms.GetComponent<ArmsTest>().anim.SetTrigger("ItemUpdate");
        }
        else if (selectedItem == Items.Cup || HotbarManager.Instance.selectedItem == Items.Bottle)
        {
            print("444. Cup Category");

            armState = ArmState.Cup;
            arms.GetComponent<ArmsTest>().anim.SetInteger("ItemCategory", 4);
            arms.GetComponent<ArmsTest>().anim.SetTrigger("ItemUpdate");
        }
        else if (selectedItem == Items.Bucket)
        {
            print("444. Bucket Category");

            armState = ArmState.Bucket;
            arms.GetComponent<ArmsTest>().anim.SetInteger("ItemCategory", 5);
            arms.GetComponent<ArmsTest>().anim.SetTrigger("ItemUpdate");
        }
        else if (selectedItem == Items.None)
        {
            print("444. Nothing Category");

            armState = ArmState.None;
            arms.GetComponent<ArmsTest>().anim.SetInteger("ItemCategory", 0);
            arms.GetComponent<ArmsTest>().anim.SetTrigger("ItemUpdate");
        }
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

public enum ArmState
{
    None,

    Tools,
    Flashlight,
    Crystal,
    Cup,
    Bucket
}