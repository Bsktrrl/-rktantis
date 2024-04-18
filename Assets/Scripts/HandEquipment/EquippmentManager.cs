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
        print("0. Hit Something with the use of a " + equippedItem.itemName);

        #region
        //If Pickaxe is equipped - For Mining
        #region
        if (equippedItem.subCategories == ItemSubCategories.Pickaxe)
        {
            if (equippedItem.itemName == Items.WoodPickaxe || equippedItem.itemName == Items.StonePickaxe || equippedItem.itemName == Items.CryonitePickaxe)
            {
                if (SelectionManager.Instance.selectedObject)
                {
                    if (SelectionManager.Instance.selectedObject.GetComponent<Ore>())
                    {
                        SelectionManager.Instance.selectedObject.GetComponent<Ore>().OreInteraction(equippedItem.itemName);
                    }
                }
            }
        }
        else if (equippedItem.itemName == Items.Flashlight || equippedItem.itemName == Items.AríditeCrystal || equippedItem.itemName == Items.None)
        {
            if (SelectionManager.Instance.selectedObject)
            {
                if (SelectionManager.Instance.selectedObject.GetComponent<Ore>())
                {
                    SelectionManager.Instance.selectedObject.GetComponent<Ore>().OreInteraction(equippedItem.itemName);
                }
            }
        }
        #endregion

        //If Hammer is equipped - For Building
        #region
        if (equippedItem.subCategories == ItemSubCategories.BuildingHammer)
        {
            if (equippedItem.itemName == Items.WoodBuildingHammer || equippedItem.itemName == Items.StoneBuildingHammer || equippedItem.itemName == Items.CryoniteBuildingHammer)
            {
                BuildingSystemManager.Instance.PlaceWorldBuildingObject();
            }
        }
        #endregion

        //If Axe is equipped - For "Removing BuildingObjects" and "Cutting"
        #region
        //Removing BuildObjects
        if (SelectionManager.Instance.selectedMovableObjectToRemove)
        {
            if (SelectionManager.Instance.selectedMovableObjectToRemove.GetComponent<MoveableObject>())
            {
                if (equippedItem.subCategories == ItemSubCategories.Axe)
                {
                    if (equippedItem.itemName == Items.WoodAxe || equippedItem.itemName == Items.StoneAxe || equippedItem.itemName == Items.CryoniteAxe)
                    {
                        //Add Items to inventory from the removal
                        if (SelectionManager.Instance.selectedMovableObjectToRemove.GetComponent<MoveableObject>().buildingObjectType == BuildingObjectTypes.BuildingBlock)
                        {
                            //Play Placement Sound
                            if (SelectionManager.Instance.selectedMovableObjectToRemove.GetComponent<MoveableObject>().buildingMaterial == BuildingMaterial.Wood)
                            {
                                SoundManager.Instance.Play_Building_Remove_Wood_Clip();
                            }
                            else if (SelectionManager.Instance.selectedMovableObjectToRemove.GetComponent<MoveableObject>().buildingMaterial == BuildingMaterial.Stone)
                            {
                                SoundManager.Instance.Play_Building_Remove_Stone_Clip();
                            }
                            else if (SelectionManager.Instance.selectedMovableObjectToRemove.GetComponent<MoveableObject>().buildingMaterial == BuildingMaterial.Cryonite)
                            {
                                SoundManager.Instance.Play_Building_Remove_Cryonite_Clip();
                            }

                            BuildingBlockInfo buildingBlockInfo = MainManager.Instance.GetMovableObject(SelectionManager.Instance.selectedMovableObjectToRemove.GetComponent<MoveableObject>().buildingBlockObjectName, SelectionManager.Instance.selectedMovableObjectToRemove.GetComponent<MoveableObject>().buildingMaterial);

                            for (int i = 0; i < buildingBlockInfo.objectInfo.removingReward.Count; i++)
                            {
                                for (int j = 0; j < buildingBlockInfo.objectInfo.removingReward[i].amount; j++)
                                {
                                    InventoryManager.Instance.AddItemToInventory(0, buildingBlockInfo.objectInfo.removingReward[i].itemName);
                                }
                            }
                        }
                        else if (SelectionManager.Instance.selectedMovableObjectToRemove.GetComponent<MoveableObject>().buildingObjectType == BuildingObjectTypes.Furniture)
                        {
                            //Play Placement Sound
                            if (SelectionManager.Instance.selectedMovableObjectToRemove.GetComponent<MoveableObject>().buildingMaterial == BuildingMaterial.Wood)
                            {
                                SoundManager.Instance.Play_Building_Remove_MoveableObject_Clip();
                            }

                            FurnitureInfo furnitureInfo = MainManager.Instance.GetMovableObject(SelectionManager.Instance.selectedMovableObjectToRemove.GetComponent<MoveableObject>().furnitureObjectName);

                            for (int i = 0; i < furnitureInfo.objectInfo.removingReward.Count; i++)
                            {
                                for (int j = 0; j < furnitureInfo.objectInfo.removingReward[i].amount; j++)
                                {
                                    InventoryManager.Instance.AddItemToInventory(0, furnitureInfo.objectInfo.removingReward[i].itemName);
                                }
                            }
                        }
                        else if (SelectionManager.Instance.selectedMovableObjectToRemove.GetComponent<MoveableObject>().buildingObjectType == BuildingObjectTypes.Machine)
                        {
                            //Play Placement Sound
                            if (SelectionManager.Instance.selectedMovableObjectToRemove.GetComponent<MoveableObject>().buildingMaterial == BuildingMaterial.Wood)
                            {
                                SoundManager.Instance.Play_Building_Remove_MoveableObject_Clip();
                            }

                            MachineInfo machineInfo = MainManager.Instance.GetMovableObject(SelectionManager.Instance.selectedMovableObjectToRemove.GetComponent<MoveableObject>().machineObjectName);

                            for (int i = 0; i < machineInfo.objectInfo.removingReward.Count; i++)
                            {
                                for (int j = 0; j < machineInfo.objectInfo.removingReward[i].amount; j++)
                                {
                                    InventoryManager.Instance.AddItemToInventory(0, machineInfo.objectInfo.removingReward[i].itemName);
                                }
                            }
                        }

                        //Remove BuidlingObject from World and SaveList
                        BuildingSystemManager.Instance.RemoveWorldBuildingObject(SelectionManager.Instance.selectedMovableObjectToRemove.GetComponent<MoveableObject>());
                    }
                }
            }
        }

        //Cutting
        else if (SelectionManager.Instance.selectedObject)
        {
            if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>())
            {
                if (equippedItem.subCategories == ItemSubCategories.Axe)
                {
                    if (equippedItem.itemName == Items.WoodAxe || equippedItem.itemName == Items.StoneAxe || equippedItem.itemName == Items.CryoniteAxe)
                    {
                        if (SelectionManager.Instance.selectedObject)
                        {
                            if (SelectionManager.Instance.selectedObject.GetComponent<Tree>())
                            {
                                SelectionManager.Instance.selectedObject.GetComponent<Tree>().TreeInteraction(equippedItem.itemName);
                            }
                        }
                    }
                }
                else if (equippedItem.itemName == Items.Flashlight || equippedItem.itemName == Items.AríditeCrystal || equippedItem.itemName == Items.None)
                {
                    if (SelectionManager.Instance.selectedObject)
                    {
                        if (SelectionManager.Instance.selectedObject.GetComponent<Tree>())
                        {
                            SelectionManager.Instance.selectedObject.GetComponent<Tree>().TreeInteraction(equippedItem.itemName);
                        }
                    }
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

        //Remove Durability
        if (equippedItem.itemName == Items.Flashlight || equippedItem.itemName == Items.AríditeCrystal || equippedItem.itemName == Items.None)
        {
            float percentage;

            if (SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>().interactableType == InteracteableType.Cactus)
            {
                //Damage the player when hitting ore vein with the Hand
                percentage = OreManager.Instance.handDamage / 50;

                SoundManager.Instance.Play_Player_FallDamage_Clip();
            }
            else
            {
                //Damage the player when hitting ore vein with the Hand
                percentage = OreManager.Instance.handDamage / 100;

                SoundManager.Instance.Play_Player_FallDamage_Clip();
            }

            HealthManager.Instance.mainHealthValue -= percentage;
        }
        else
        {
            equippedItem.RemoveDurability();
        }

        //Change Water level in water container
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