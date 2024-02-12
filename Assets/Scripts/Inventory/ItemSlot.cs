using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Items itemName = Items.None;
    public int itemID;
    public int inventoryIndex;

    [Header("HotbarSelectInfo")]
    public GameObject hotbarSelectorParent;
    public TextMeshProUGUI hotbarIndex_Text;


    //--------------------


    public void OnPointerUp(PointerEventData eventData)
    {
        //If only player inventory is used
        if (MainManager.Instance.menuStates == MenuStates.InventoryMenu
            || MainManager.Instance.menuStates == MenuStates.CraftingMenu
            || MainManager.Instance.menuStates == MenuStates.EquipmentMenu)
        {
            //If the right Mouse button is pressed - Remove item from inventory
            #region
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                RemoveItemFromInventory();
            }
            #endregion

            //If the left Mouse button is pressed - Mark this item to an available Hotbar space
            #region
            else if (eventData.button == PointerEventData.InputButton.Left)
            {
                //If clicked item is a HandEqipable
                if (MainManager.Instance.GetItem(itemName).isEquipableInHand)
                {
                    AssignItemToHotbar();
                }

                //If clicked item is a Clothing
                else if (MainManager.Instance.GetItem(itemName).isEquipableClothes)
                {
                    AssignItemToClothingSlot();
                }
            }
            #endregion
        }

        //If player is in a chest
        else if (MainManager.Instance.menuStates == MenuStates.ChestMenu)
        {
            //If the right Mouse button is pressed - Move this item between the open inventories, if possible
            #region
            if (eventData.button == PointerEventData.InputButton.Right && itemName != Items.None)
            {
                MoveItemBetweenChests();
            }
            #endregion

            //If the left Mouse button is pressed on a playerInventory slot - Mark this item to an available Hotbar space
            #region
            else if (eventData.button == PointerEventData.InputButton.Left && inventoryIndex <= 0)
            {
                //If clicked item is a HandEqipable
                if (MainManager.Instance.GetItem(itemName).isEquipableInHand)
                {
                    AssignItemToHotbar();
                }

                //If clicked item is a Clothing
                else if (MainManager.Instance.GetItem(itemName).isEquipableClothes)
                {
                    AssignItemToClothingSlot();
                }
            }
            #endregion
        }

        if (itemName != Items.None)
        {
            InventoryManager.Instance.ChangeitemInfoBox(itemName, this);
        }
    }
    void RemoveItemFromInventory()
    {
        InventoryManager.Instance.RemoveItemFromInventory(inventoryIndex, itemName, itemID);
    }
    void AssignItemToHotbar()
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

                    InventoryManager.Instance.DeselectItemInfoToHotbar(itemName, itemID);

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

                    InventoryManager.Instance.SelectItemInfoToHotbar(i, itemName, itemID);

                    //Update the Hand to see if slot is empty
                    HotbarManager.Instance.ChangeItemInHand();

                    return;
                }
            }
        }
    }
    void MoveItemBetweenChests()
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
    void AssignItemToClothingSlot()
    {

    }


    //--------------------


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

        if (itemName != Items.None)
        {
            InventoryManager.Instance.ChangeitemInfoBox(itemName, this);
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

        InventoryManager.Instance.SetItemSelectedHighlight_Active(inventoryIndex, itemID, itemName, false);
    }


    //--------------------


    public void ActivateHotbarInfoToItemSlot(int hotbarIndex)
    {
        hotbarIndex_Text.text = hotbarIndex.ToString();
        hotbarSelectorParent.SetActive(true);
    }
    public void DeactivateHotbarInfoToItemSlot()
    {
        hotbarSelectorParent.SetActive(false);
    }


    //--------------------


    public void DestroyItemSlot()
    {
        Destroy(gameObject);
    }
}
