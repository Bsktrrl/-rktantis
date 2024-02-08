using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Items itemName = Items.None;
    public int itemID;
    public int inventoryIndex;


    //--------------------


    public void OnPointerUp(PointerEventData eventData)
    {
        //print("You clicked on item: " + itemName + " with index: " + itemID + " and from inventory: " + inventoryIndex);

        //If only player inventory is used
        if (MainManager.Instance.menuStates == MenuStates.InventoryMenu
            || MainManager.Instance.menuStates == MenuStates.CraftingMenu
            || MainManager.Instance.menuStates == MenuStates.EquipmentMenu)
        {
            //If the right Mouse button is pressed - Remove item from inventory
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                //print("PlayerInventory - Right");
                InventoryManager.Instance.RemoveItemFromInventory(inventoryIndex, itemName, itemID);
            }

            //If the left Mouse button is pressed - Mark this item to an available Hotbar space
            else if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (itemName != Items.None)
                {
                    //Check if item is already on the Hotbar, to remove it
                    for (int i = 0; i < HotbarManager.Instance.hotbarList.Count; i++)
                    {
                        if (HotbarManager.Instance.hotbarList[i].hotbar.GetComponent<HotbarSlot>().hotbarItemName != Items.None
                            && HotbarManager.Instance.hotbarList[i].hotbar.GetComponent<HotbarSlot>().hotbarItemName == itemName
                            && HotbarManager.Instance.hotbarList[i].hotbar.GetComponent<HotbarSlot>().hotbarItemsID == itemID)
                        {
                            HotbarManager.Instance.hotbarList[i].hotbar.GetComponent<HotbarSlot>().RemoveItemFromHotbar();

                            HotbarManager.Instance.hotbarList[i].itemName = Items.None;
                            HotbarManager.Instance.hotbarList[i].itemID = -1;

                            HotbarManager.Instance.SetSelectedItem();

                            InventoryManager.Instance.DeselectItemToHotbar(itemName, itemID);

                            //Update the Hand to see if slot is empty
                            HotbarManager.Instance.ChangeItemInHand();

                            return;
                        }
                    }

                    //Add item to the Hotbar if item isn't already on the Hotbar
                    for (int i = 0; i < HotbarManager.Instance.hotbarList.Count; i++)
                    {
                        if (HotbarManager.Instance.hotbarList[i].itemName == Items.None)
                        {
                            HotbarManager.Instance.hotbarList[i].hotbar.GetComponent<HotbarSlot>().hotbarItemName = itemName;
                            HotbarManager.Instance.hotbarList[i].hotbar.GetComponent<HotbarSlot>().SetHotbarSlotImage();
                            HotbarManager.Instance.hotbarList[i].itemName = itemName;
                            HotbarManager.Instance.hotbarList[i].itemID = itemID;
                            HotbarManager.Instance.hotbarList[i].hotbar.GetComponent<HotbarSlot>().SetHotbarItemID(itemID);
                            HotbarManager.Instance.SetSelectedItem();

                            InventoryManager.Instance.SelectItemToHotbar(itemName, itemID);

                            //Update the Hand to see if slot is empty
                            HotbarManager.Instance.ChangeItemInHand();

                            return;
                        }
                    }
                }
            }
        }

        //If player is in a chest
        else if (MainManager.Instance.menuStates == MenuStates.ChestMenu)
        {
            //If the right Mouse button is pressed - Move this item between the open inventories, if possible
            if (eventData.button == PointerEventData.InputButton.Right && itemName != Items.None)
            {
                //print("ChestInventory - Right");
                //Move from Player Inventory to chest
                if (inventoryIndex <= 0)
                {
                    InventoryManager.Instance.MoveItemToInventory(inventoryIndex, gameObject, itemID);
                }

                //Move from chest to Player Inventory
                else
                {
                    InventoryManager.Instance.MoveItemToInventory(inventoryIndex, gameObject, itemID);
                }
            }

            //If the left Mouse button is pressed - Mark this item to an available Hotbar space
            else if (eventData.button == PointerEventData.InputButton.Left && inventoryIndex <= 0)
            {
                //print("PlayerInventory - Left");
                if (itemName != Items.None)
                {
                    //Check if item is already on the Hotbar
                    for (int i = 0; i < HotbarManager.Instance.hotbarList.Count; i++)
                    {
                        if (HotbarManager.Instance.hotbarList[i].itemName != Items.None
                            && HotbarManager.Instance.hotbarList[i].itemName == itemName)
                        {
                            HotbarManager.Instance.hotbarList[i].hotbar.GetComponent<HotbarSlot>().hotbarItemName = Items.None;
                            HotbarManager.Instance.hotbarList[i].hotbar.GetComponent<HotbarSlot>().RemoveItemFromHotbar();
                            HotbarManager.Instance.hotbarList[i].itemName = Items.None;
                            HotbarManager.Instance.hotbarList[i].itemID = -1;
                            HotbarManager.Instance.SetSelectedItem();

                            //Update the Hand to see if slot is empty
                            HotbarManager.Instance.ChangeItemInHand();

                            return;
                        }
                    }

                    //Add item to the Hotbar if item isn't already on the Hotbar
                    for (int i = 0; i < HotbarManager.Instance.hotbarList.Count; i++)
                    {
                        if (HotbarManager.Instance.hotbarList[i].itemName == Items.None)
                        {
                            HotbarManager.Instance.hotbarList[i].hotbar.GetComponent<HotbarSlot>().hotbarItemName = itemName;
                            HotbarManager.Instance.hotbarList[i].hotbar.GetComponent<HotbarSlot>().SetHotbarSlotImage();
                            HotbarManager.Instance.hotbarList[i].itemName = itemName;
                            HotbarManager.Instance.hotbarList[i].itemID = itemID;
                            HotbarManager.Instance.SetSelectedItem();

                            //Update the Hand to see if slot is empty
                            HotbarManager.Instance.ChangeItemInHand();

                            return;
                        }
                    }
                }
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (inventoryIndex <= 0)
        {
            InventoryManager.Instance.SetPlayerItemInfo(itemName, true);
        }
        else
        {
            InventoryManager.Instance.SetPlayerItemInfo(itemName, false);
        }

        InventoryManager.Instance.SetItemSelectedHighlight_Active(inventoryIndex, itemID, itemName, true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (inventoryIndex <= 0)
        {
            InventoryManager.Instance.SetPlayerItemInfo(Items.None, true);
        }
        else
        {
            InventoryManager.Instance.SetPlayerItemInfo(Items.None, false);
        }

        InventoryManager.Instance.SetItemSelectedHighlight_Active(inventoryIndex, itemID, itemName, false);
    }


    //--------------------


    public void DestroyItemSlot()
    {
        Destroy(gameObject);
    }
}
