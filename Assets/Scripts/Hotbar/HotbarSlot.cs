using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotbarSlot : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Image selectedImage;

    public Items hotbarItemName;
    public int hotbarItemsID;

    [Header("DurabilityMeter")]
    public GameObject durabilityMeterParent;
    public Image durabilityMeterImage;


    //--------------------


    private void Start()
    {
        SetHotbarSlotDisplay();
    }


    //--------------------


    public void SetHotbarSlotDisplay()
    {
        if (hotbarItemName != Items.None)
        {
            //Set Image
            image.sprite = MainManager.Instance.GetItem(hotbarItemName).hotbarSprite;

            //Set DurabilityMeter
            for (int i = 0; i < InventoryManager.Instance.inventories[0].itemsInInventory.Count; i++)
            {
                if (MainManager.Instance.GetItem(InventoryManager.Instance.inventories[0].itemsInInventory[i].itemName).durability_Max > 0
                && hotbarItemsID == InventoryManager.Instance.inventories[0].itemsInInventory[i].itemID)
                {
                    float tempFill = (float)InventoryManager.Instance.inventories[0].itemsInInventory[i].durability_Current / MainManager.Instance.GetItem(InventoryManager.Instance.inventories[0].itemsInInventory[i].itemName).durability_Max;

                    durabilityMeterImage.fillAmount = tempFill;

                    durabilityMeterParent.SetActive(true);

                    break;
                }
                else
                {
                    durabilityMeterParent.SetActive(false);
                }
            }
        }
    }
    public void SetHotbarItemID(int id)
    {
        hotbarItemsID = id;
    }

    public void RemoveItemFromHotbar()
    {
        if (hotbarItemName != Items.None)
        {
            image.sprite = MainManager.Instance.GetItem(0).hotbarSprite;
            hotbarItemName = Items.None;
            hotbarItemsID = 0;

            durabilityMeterParent.SetActive(false);
        }
    }

    public void ResetHotbarItem()
    {
        hotbarItemName = Items.None;

        durabilityMeterParent.SetActive(false);
    }

    public void SetHotbarSlotActive()
    {
        selectedImage.gameObject.SetActive(true);
    }
    public void SetHotbarSlotUnactive()
    {
        selectedImage.gameObject.SetActive(false);
    }
}
