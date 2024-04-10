using System.Collections;
using System.Collections.Generic;
using System.IO;
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
        if (selectedItem == Items.None)
        {
            armState = ArmState.Hand;
            arms.GetComponent<Arms>().anim.SetInteger("ItemCategory", 0);
            arms.GetComponent<Arms>().anim.SetTrigger("ItemUpdate");
        }

        else if (selectedItem == Items.WoodBuildingHammer || selectedItem == Items.StoneBuildingHammer || selectedItem == Items.CryoniteBuildingHammer
            || selectedItem == Items.WoodAxe || selectedItem == Items.StoneAxe || selectedItem == Items.CryoniteAxe
            || selectedItem == Items.WoodPickaxe || selectedItem == Items.StonePickaxe || selectedItem == Items.CryonitePickaxe
            || selectedItem == Items.WoodSword || selectedItem == Items.StoneSword || selectedItem == Items.CryoniteSword
            || selectedItem == Items.GhostCapturer)
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
        else if (selectedItem == Items.Cup || selectedItem == Items.Bottle)
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
        else if (selectedItem == Items.GhostCapturer)
        {
            armState = ArmState.Bucket;
            arms.GetComponent<Arms>().anim.SetInteger("ItemCategory", 5);
            arms.GetComponent<Arms>().anim.SetTrigger("ItemUpdate");
        }

        else
        {
            armState = ArmState.None;
        }
    }
    public void GetToolState(Items selectedItem)
    {
        if (selectedItem == Items.Flashlight || selectedItem == Items.AríditeCrystal || selectedItem == Items.None
            || selectedItem == Items.GhostCapturer)
        {
            toolState = ToolState.Hand;

            arms.GetComponent<Arms>().anim.SetInteger("ToolCategory", 0);
            arms.GetComponent<Arms>().anim.SetTrigger("ItemUpdate");
        }

        else if (selectedItem == Items.WoodAxe || selectedItem == Items.StoneAxe || selectedItem == Items.CryoniteAxe)
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


    //--------------------


    //The point in the animation where equipped item hits
    public void Hit(EquippedItem equippedItem)
    {
        print("0. Interact with a Tree");
        //print("Hit EquippedItem - " + subCategories + " [" + itemName.ToString() + "]");

        #region
        //If Pickaxe is equipped - For Mining
        #region
        if (equippedItem.subCategories == ItemSubCategories.Pickaxe)
        {
            if (equippedItem.itemName == Items.WoodPickaxe || equippedItem.itemName == Items.StonePickaxe || equippedItem.itemName == Items.CryonitePickaxe)
            {
                if (SelectionManager.Instance.selecedObject)
                {
                    if (SelectionManager.Instance.selecedObject.GetComponent<Ore>())
                    {
                        SelectionManager.Instance.selecedObject.GetComponent<Ore>().OreInteraction(equippedItem.itemName);
                    }
                }
            }
        }
        else if (equippedItem.itemName == Items.Flashlight || equippedItem.itemName == Items.AríditeCrystal || equippedItem.itemName == Items.None)
        {
            if (SelectionManager.Instance.selecedObject)
            {
                if (SelectionManager.Instance.selecedObject.GetComponent<Ore>())
                {
                    SelectionManager.Instance.selecedObject.GetComponent<Ore>().OreInteraction(equippedItem.itemName);
                }
            }
        }
        #endregion

        //If Axe is equipped
        #region
        print("1. Interact with a Tree");
        if (equippedItem.subCategories == ItemSubCategories.Axe)
        {
            if (equippedItem.itemName == Items.WoodAxe || equippedItem.itemName == Items.StoneAxe || equippedItem.itemName == Items.CryoniteAxe)
            {
                if (SelectionManager.Instance.selecedObject)
                {
                    if (SelectionManager.Instance.selecedObject.GetComponent<Tree>())
                    {
                        SelectionManager.Instance.selecedObject.GetComponent<Tree>().TreeInteraction(equippedItem.itemName);
                    }
                }
            }
        }
        else if (equippedItem.itemName == Items.Flashlight || equippedItem.itemName == Items.AríditeCrystal || equippedItem.itemName == Items.None)
        {
            print("2. Interact with a Tree");
            if (SelectionManager.Instance.selecedObject)
            {
                print("3. Interact with a Tree");
                if (SelectionManager.Instance.selecedObject.GetComponent<Tree>())
                {
                    print("4. Interact with a Tree");
                    SelectionManager.Instance.selecedObject.GetComponent<Tree>().TreeInteraction(equippedItem.itemName);
                }
            }
        }
        #endregion

        //If WaterContainer is equipped
        #region
        else if (equippedItem.subCategories == ItemSubCategories.Drinking)
        {
            //Heal thirst parameter
            if (MainManager.Instance.GetItem(equippedItem.itemName).thirstHealthHeal > 0 && HotbarManager.Instance.hotbarList[HotbarManager.Instance.selectedSlot].durabilityCurrent > 0)
            {
                //Play Drinking Sound
                SoundManager.Instance.Play_Inventory_DrinkItem_Clip();

                float percentage = (float)MainManager.Instance.GetItem(equippedItem.itemName).thirstHealthHeal / 100;
                HealthManager.Instance.thirstValue += percentage;

                if (HealthManager.Instance.thirstValue > 1)
                {
                    HealthManager.Instance.thirstValue = 1;
                }
            }
            else
            {
                //Play Drinking Empty Sound
                SoundManager.Instance.Play_Inventory_DrinkEmptyItem_Clip();
            }
        }
        #endregion

        //If GhostCapturer is equipped - For capturing Ghosts
        #region
        else if (equippedItem.subCategories == ItemSubCategories.GhostCapturer)
        {

        }
        #endregion

        #endregion

        if (equippedItem.itemName == Items.Flashlight || equippedItem.itemName == Items.AríditeCrystal || equippedItem.itemName == Items.None)
        {
            //Damage the player when hitting ore vein with the Hand
            float percentage = OreManager.Instance.handDamage / 100;
            HealthManager.Instance.mainHealthValue -= percentage;
        }
        else
        {
            equippedItem.RemoveDurability();
        }

        if (HotbarManager.Instance.selectedItem == Items.Bucket || HotbarManager.Instance.selectedItem == Items.Cup)
        {
            if (equippedItem.gameObject.GetComponent<WaterContainer>())
            {
                equippedItem.BucketWaterlevel(equippedItem.gameObject.GetComponent<WaterContainer>().waterMesh);
            }
        }
    }
}

public enum ArmState
{
    None,

    Hand,

    Tools,
    Flashlight,
    Crystal,
    Cup,
    Bucket
}

public enum ToolState
{
    None,

    Hand,

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