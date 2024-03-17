using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEditor.Progress;

[RequireComponent(typeof(Animator))]
public class EquippedItem : MonoBehaviour
{
    public Animator animator;
    public Items itemName;
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
        print("Hit EquippedItem - " + subCategories + " [" + itemName.ToString() + "]");

        //The point in the animation where equipped item hits

        //If Axe is equipped
        if (subCategories == ItemSubCategories.Axe /*&& SelectionManager.Instance.selectedTree != null*/)
        {
            //if (SelectionManager.Instance.selectedTree.GetComponent<ChoppableTree>().treeParent != null)
            //{
            //    SelectionManager.Instance.selectedTree.GetComponent<ChoppableTree>().treeParent.gameObject.GetComponent<TreeParent>().ObjectInteraction();
            //}
        }
        else if (subCategories == ItemSubCategories.Drinking)
        {
            //Heal thirst parameter
            if (MainManager.Instance.GetItem(itemName).thirstHealthHeal > 0 && HotbarManager.Instance.hotbarList[HotbarManager.Instance.selectedSlot].durabilityCurrent > 0)
            {
                //Play Drinking Sound
                SoundManager.Instance.Play_Inventory_DrinkItem_Clip();

                float percentage = (float)MainManager.Instance.GetItem(itemName).thirstHealthHeal / 100;
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

        RemoveDurability();

        BucketWaterlevel(gameObject.GetComponent<WaterContainer>().waterMesh);
    }

    public void BucketWaterlevel(GameObject waterMesh)
    {
        //If WaterContainer is a Bucket, change the water level based on the amount left in the Bucket
        if (HotbarManager.Instance.selectedItem == Items.Bucket)
        {
            //Set Water level to the bottom
            waterMesh.transform.localPosition = new Vector3(0, -0.15f, 0);
            waterMesh.transform.localScale = new Vector3(0.7f, 1, 0.7f);

            //Fill the bucket, based on the amount left in it
            float pos_Percentage = 0.15f / (float)MainManager.Instance.GetItem(Items.Bucket).durability_Max;
            float scale_Percentage = 0.3f / (float)MainManager.Instance.GetItem(Items.Bucket).durability_Max;

            for (int i = 0; i < HotbarManager.Instance.hotbarList[HotbarManager.Instance.selectedSlot].durabilityCurrent; i++)
            {
                waterMesh.transform.localPosition += new Vector3(0, pos_Percentage, 0);
                waterMesh.transform.localScale += new Vector3(scale_Percentage, 0, scale_Percentage);
            }
        }

        //If WaterContainer is empty, hide its mesh
        if (gameObject.GetComponent<WaterContainer>())
        {
            if (HotbarManager.Instance.hotbarList[HotbarManager.Instance.selectedSlot].durabilityCurrent <= 0)
            {
                gameObject.GetComponent<WaterContainer>().DeactivateMesh();
            }
            else
            {
                gameObject.GetComponent<WaterContainer>().ActivateMesh();
            }
        }
    }

    public void FillDrink()
    {
        if (SelectionManager.Instance.tag == "Water")
        {
            //Play Refill Drink Sound
            SoundManager.Instance.Play_Inventory_RefillDrink_Clip();

            //Update Durability
            for (int i = 0; i < InventoryManager.Instance.inventories[0].itemsInInventory.Count; i++)
            {
                if (InventoryManager.Instance.inventories[0].itemsInInventory[i].itemID == HotbarManager.Instance.hotbarList[HotbarManager.Instance.selectedSlot].itemID)
                {
                    //Reduce the Durability
                    InventoryManager.Instance.inventories[0].itemsInInventory[i].durability_Current = MainManager.Instance.GetItem(itemName).durability_Max;

                    //Update the Hotbar Display
                    HotbarManager.Instance.hotbarList[HotbarManager.Instance.selectedSlot].durabilityCurrent = InventoryManager.Instance.inventories[0].itemsInInventory[i].durability_Current;

                    break;
                }
            }

            //Display Water Mesh
            if (gameObject.GetComponent<WaterContainer>())
            {
                gameObject.transform.localPosition = Vector3.zero;
                gameObject.GetComponent<WaterContainer>().waterMesh.transform.localPosition = Vector3.zero;
                gameObject.GetComponent<WaterContainer>().waterMesh.transform.localScale = Vector3.one;
                gameObject.GetComponent<WaterContainer>().ActivateMesh();
            }
        }
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
                HotbarManager.Instance.hotbarList[HotbarManager.Instance.selectedSlot].durabilityCurrent = InventoryManager.Instance.inventories[0].itemsInInventory[i].durability_Current;

                //Check if durability <= 0 to remove the item from Hotbar and Inventory
                if (InventoryManager.Instance.inventories[0].itemsInInventory[i].durability_Current <= 0)
                {
                    if (subCategories != ItemSubCategories.Drinking)
                    {
                        //Play "Broken"-Sound
                        SoundManager.Instance.Play_EquippedItems_EquippedItemIsBroken_Clip();

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
                }

                break;
            }
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
