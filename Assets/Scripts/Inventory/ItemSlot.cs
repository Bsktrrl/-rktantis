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
        if (MainManager.Instance.menuStates == MenuStates.InventoryMenu)
        {
            //If the right Mouse button is pressed - Remove item from inventory
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                print("PlayerInventory - Right");
                InventoryManager.Instance.RemoveItemFromInventory(inventoryIndex, itemName);
            }

            //If the left Mouse button is pressed - Mark this item to an available Hotbar space
            else if (eventData.button == PointerEventData.InputButton.Left)
            {
                print("PlayerInventory - Left");
                if (itemName != Items.None)
                {
                    //Check if item is already on the Hotbar
                    for (int i = 0; i < HotbarManager.Instance.hotbarList.Count; i++)
                    {
                        if (HotbarManager.Instance.hotbarList[i].GetComponent<HotbarSlot>().hotbarItemName != Items.None
                            && HotbarManager.Instance.hotbarList[i].GetComponent<HotbarSlot>().hotbarItemName == itemName)
                        {
                            HotbarManager.Instance.hotbarList[i].GetComponent<HotbarSlot>().hotbarItemName = Items.None;
                            HotbarManager.Instance.hotbarList[i].GetComponent<HotbarSlot>().RemoveHotbarSlotImage();
                            HotbarManager.Instance.SetSelectedItem();

                            //Update the Hand to see if slot is empty
                            HotbarManager.Instance.ChangeItemInHand();

                            return;
                        }
                    }

                    //Add item to the Hotbar if item isn't already on the Hotbar
                    for (int i = 0; i < HotbarManager.Instance.hotbarList.Count; i++)
                    {
                        if (HotbarManager.Instance.hotbarList[i].GetComponent<HotbarSlot>().hotbarItemName == Items.None)
                        {
                            HotbarManager.Instance.hotbarList[i].GetComponent<HotbarSlot>().hotbarItemName = itemName;
                            HotbarManager.Instance.hotbarList[i].GetComponent<HotbarSlot>().SetHotbarSlotImage();
                            HotbarManager.Instance.SetSelectedItem();

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
            //If the left Mouse button is pressed - Move this item between the open inventories, if possible
            if (eventData.button == PointerEventData.InputButton.Right && itemName != Items.None)
            {
                print("ChestInventory - Right");
                //Move from Player Inventory to chest
                if (inventoryIndex <= 0)
                {
                    InventoryManager.Instance.MoveItemToInventory(inventoryIndex, gameObject);
                }

                //Move from chest to Player Inventory
                else
                {
                    InventoryManager.Instance.MoveItemToInventory(inventoryIndex, gameObject);
                }
            }

            //If the left Mouse button is pressed - Mark this item to an available Hotbar space
            else if (eventData.button == PointerEventData.InputButton.Left && inventoryIndex <= 0)
            {
                print("PlayerInventory - Left");
                if (itemName != Items.None)
                {
                    //Check if item is already on the Hotbar
                    for (int i = 0; i < HotbarManager.Instance.hotbarList.Count; i++)
                    {
                        if (HotbarManager.Instance.hotbarList[i].GetComponent<HotbarSlot>().hotbarItemName != Items.None
                            && HotbarManager.Instance.hotbarList[i].GetComponent<HotbarSlot>().hotbarItemName == itemName)
                        {
                            HotbarManager.Instance.hotbarList[i].GetComponent<HotbarSlot>().hotbarItemName = Items.None;
                            HotbarManager.Instance.hotbarList[i].GetComponent<HotbarSlot>().RemoveHotbarSlotImage();
                            HotbarManager.Instance.SetSelectedItem();

                            //Update the Hand to see if slot is empty
                            HotbarManager.Instance.ChangeItemInHand();

                            return;
                        }
                    }

                    //Add item to the Hotbar if item isn't already on the Hotbar
                    for (int i = 0; i < HotbarManager.Instance.hotbarList.Count; i++)
                    {
                        if (HotbarManager.Instance.hotbarList[i].GetComponent<HotbarSlot>().hotbarItemName == Items.None)
                        {
                            HotbarManager.Instance.hotbarList[i].GetComponent<HotbarSlot>().hotbarItemName = itemName;
                            HotbarManager.Instance.hotbarList[i].GetComponent<HotbarSlot>().SetHotbarSlotImage();
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


    //--------------------


    public void DestroyItemSlot()
    {
        Destroy(gameObject);
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
    }
}
