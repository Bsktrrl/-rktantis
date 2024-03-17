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
    public ToolState toolState;
    public ToolRank toolRank;


    //--------------------


    public void GetEquipmentStates(Items selectedItem)
    {
        GetArmState(selectedItem);
        GetToolState(selectedItem);
        GetToolRank(selectedItem);
    }
    public void GetArmState(Items selectedItem)
    {
        if (selectedItem == Items.WoodBuildingHammer || selectedItem == Items.StoneBuildingHammer || selectedItem == Items.CryoniteBuildingHammer
            || selectedItem == Items.WoodAxe || selectedItem == Items.StoneAxe || selectedItem == Items.CryoniteAxe
            || selectedItem == Items.WoodPickaxe || selectedItem == Items.StonePickaxe || selectedItem == Items.CryonitePickaxe
            || selectedItem == Items.WoodSword || selectedItem == Items.StoneSword || selectedItem == Items.CryoniteSword)
        {
            armState = ArmState.Tools;
            arms.GetComponent<Arms>().anim.SetInteger("ItemCategory", 1);
            arms.GetComponent<Arms>().anim.SetTrigger("ItemUpdate");
        }
        else if (selectedItem == Items.Flashlight)
        {
            armState = ArmState.Flashlight;
            arms.GetComponent<Arms>().anim.SetInteger("ItemCategory", 2);
            arms.GetComponent<Arms>().anim.SetTrigger("ItemUpdate");
        }
        else if (selectedItem == Items.AríditeCrystal)
        {
            armState = ArmState.Crystal;
            arms.GetComponent<Arms>().anim.SetInteger("ItemCategory", 3);
            arms.GetComponent<Arms>().anim.SetTrigger("ItemUpdate");
        }
        else if (selectedItem == Items.Cup || HotbarManager.Instance.selectedItem == Items.Bottle)
        {
            armState = ArmState.Cup;
            arms.GetComponent<Arms>().anim.SetInteger("ItemCategory", 4);
            arms.GetComponent<Arms>().anim.SetTrigger("ItemUpdate");
        }
        else if (selectedItem == Items.Bucket)
        {
            armState = ArmState.Bucket;
            arms.GetComponent<Arms>().anim.SetInteger("ItemCategory", 5);
            arms.GetComponent<Arms>().anim.SetTrigger("ItemUpdate");
        }
        else if (selectedItem == Items.None)
        {
            armState = ArmState.None;
            arms.GetComponent<Arms>().anim.SetInteger("ItemCategory", 0);
            arms.GetComponent<Arms>().anim.SetTrigger("ItemUpdate");
        }

        else
        {
            armState = ArmState.None;
        }
    }
    public void GetToolState(Items selectedItem)
    {
        if (selectedItem == Items.WoodAxe || selectedItem == Items.StoneAxe || selectedItem == Items.CryoniteAxe)
        {
            toolState = ToolState.Axe;

            arms.GetComponent<Arms>().anim.SetInteger("ToolCategory", 1);
            arms.GetComponent<Arms>().anim.SetTrigger("ItemUpdate");
        }
        else if (selectedItem == Items.WoodPickaxe || selectedItem == Items.StonePickaxe || selectedItem == Items.CryonitePickaxe)
        {
            toolState = ToolState.Pickaxe;

            arms.GetComponent<Arms>().anim.SetInteger("ToolCategory", 2);
            arms.GetComponent<Arms>().anim.SetTrigger("ItemUpdate");
        }
        else if (selectedItem == Items.WoodBuildingHammer || selectedItem == Items.StoneBuildingHammer || selectedItem == Items.CryoniteBuildingHammer)
        {
            toolState = ToolState.BuildingHammer;

            arms.GetComponent<Arms>().anim.SetInteger("ToolCategory", 3);
            arms.GetComponent<Arms>().anim.SetTrigger("ItemUpdate");
        }
        else if (selectedItem == Items.WoodSword || selectedItem == Items.StoneSword || selectedItem == Items.CryoniteSword)
        {
            toolState = ToolState.Sword;

            arms.GetComponent<Arms>().anim.SetInteger("ToolCategory", 4);
            arms.GetComponent<Arms>().anim.SetTrigger("ItemUpdate");
        }

        else if (selectedItem == Items.GhostCapturer)
        {
            toolState = ToolState.GhostCapturer;

            arms.GetComponent<Arms>().anim.SetInteger("ToolCategory", 0);
            arms.GetComponent<Arms>().anim.SetTrigger("ItemUpdate");
        }

        else
        {
            toolState = ToolState.None;
        }
    }
    public void GetToolRank(Items selectedItem)
    {
        if (selectedItem == Items.WoodAxe || selectedItem == Items.WoodPickaxe || selectedItem == Items.WoodBuildingHammer || selectedItem == Items.WoodSword)
        {
            toolRank = ToolRank.Wood;

            arms.GetComponent<Arms>().anim.SetInteger("ToolRank", 1);
            arms.GetComponent<Arms>().anim.SetTrigger("ItemUpdate");
        }
        else if (selectedItem == Items.StoneAxe || selectedItem == Items.StonePickaxe || selectedItem == Items.StoneBuildingHammer || selectedItem == Items.StoneSword)
        {
            toolRank = ToolRank.Stone;

            arms.GetComponent<Arms>().anim.SetInteger("ToolRank", 2);
            arms.GetComponent<Arms>().anim.SetTrigger("ItemUpdate");
        }
        else if (selectedItem == Items.CryoniteAxe || selectedItem == Items.CryonitePickaxe || selectedItem == Items.CryoniteBuildingHammer || selectedItem == Items.CryoniteSword)
        {
            toolRank = ToolRank.Cryonite;

            arms.GetComponent<Arms>().anim.SetInteger("ToolRank", 3);
            arms.GetComponent<Arms>().anim.SetTrigger("ItemUpdate");
        }

        else
        {
            toolRank = ToolRank.None;
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

public enum ToolState
{
    None,

    Axe,
    Pickaxe,
    BuildingHammer,
    Sword,

    GhostCapturer
}

public enum ToolRank
{
    None,

    Wood,
    Stone,
    Cryonite
}