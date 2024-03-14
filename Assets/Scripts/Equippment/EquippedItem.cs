using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[RequireComponent(typeof(Animator))]
public class EquippedItem : MonoBehaviour
{
    public Animator animator;
    public ItemSubCategories subCategories;


    //--------------------


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    //--------------------


    public void HitAnimation()
    {
        GameObject selectedTree = SelectionManager.Instance.selectedTree;

        //if (selectedTree != null)
        //{
        //    selectedTree.GetComponent<ChoppableTree>().GetHit();
        //}

        animator.SetTrigger("hit");
    }

    public void Hit()
    {
        print("Hit EquippedItem");

        //The point in the animation where equipped item hits

        //If Axe is equipped
        if (subCategories == ItemSubCategories.Axe /*&& SelectionManager.Instance.selectedTree != null*/)
        {
            if (SelectionManager.Instance.selectedTree.GetComponent<ChoppableTree>().treeParent != null)
            {
                SelectionManager.Instance.selectedTree.GetComponent<ChoppableTree>().treeParent.gameObject.GetComponent<TreeParent>().ObjectInteraction();
            }
        }

        RemoveDurability();
    }

    public void RemoveDurability()
    {
        for (int i = 0; i < InventoryManager.Instance.inventories[0].itemsInInventory.Count; i++)
        {
            if (InventoryManager.Instance.inventories[0].itemsInInventory[i].itemID == HotbarManager.Instance.hotbarList[HotbarManager.Instance.selectedSlot].itemID)
            {
                //Reduce the Durability
                InventoryManager.Instance.inventories[0].itemsInInventory[i].durability_Current -= 1;

                //Update the Hotbar Display
                HotbarManager.Instance.hotbarList[i].durabilityCurrent = InventoryManager.Instance.inventories[0].itemsInInventory[i].durability_Current;

                //Check if durability <= 0 to remove the item from Hotbar and Inventory
                if (InventoryManager.Instance.inventories[0].itemsInInventory[i].durability_Current <= 0)
                {
                    //Play "Broken"-Sound
                    SoundManager.Instance.PlayItemIsBroken_Clip();

                    //Remove item from Hotbar
                    HotbarManager.Instance.hotbarList[i].hotbar.GetComponent<HotbarSlot>().RemoveItemFromHotbar();
                    HotbarManager.Instance.hotbarList[i].itemName = Items.None;
                    HotbarManager.Instance.hotbarList[i].itemID = -1;
                    HotbarManager.Instance.hotbarList[i].durabilityMax = 0;
                    HotbarManager.Instance.hotbarList[i].durabilityCurrent = 0;
                    HotbarManager.Instance.SetSelectedItem();
                    InventoryManager.Instance.DeselectItemInfoToHotbar(HotbarManager.Instance.hotbarList[HotbarManager.Instance.selectedSlot].itemName, HotbarManager.Instance.hotbarList[HotbarManager.Instance.selectedSlot].itemID);

                    //Update the Hand to see if slot is empty
                    HotbarManager.Instance.ChangeItemInHand();


                    //Remove item from Inventory
                    InventoryManager.Instance.RemoveItemFromInventory(0, InventoryManager.Instance.inventories[0].itemsInInventory[i].itemName, InventoryManager.Instance.inventories[0].itemsInInventory[i].itemID);
                }

                print("Remove Durability");
            }

            break;
        }
    }


    //--------------------


    public void DestroyObject()
    {
        //If Equipped Object is a BuildingHammer
        if (gameObject.GetComponent<BuildingHammer>() != null)
        {
            if (gameObject.GetComponent<BuildingHammer>().tempObj_Selected)
            {
                if (gameObject.GetComponent<BuildingHammer>().tempObj_Selected.GetComponent<InteractableObject>())
                {
                    gameObject.GetComponent<BuildingHammer>().tempObj_Selected.GetComponent<InteractableObject>().DestroyThisObject();
                    gameObject.GetComponent<BuildingHammer>().DestroyThisObject();
                }
                else
                {
                    Destroy(gameObject.GetComponent<BuildingHammer>().tempObj_Selected);
                }
            }
            
            gameObject.GetComponent<BuildingHammer>().tempObj_Selected = null;
        }

        //If Equipped Object is an Axe
        else if (gameObject.GetComponent<Axe>() != null)
        {
            gameObject.GetComponent<Axe>().DestroyThisObject();
        }

        Destroy(gameObject);
    }
}
