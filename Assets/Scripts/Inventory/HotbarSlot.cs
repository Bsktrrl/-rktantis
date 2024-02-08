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


    //--------------------


    public void SetHotbarSlotImage()
    {
        if (hotbarItemName != Items.None)
        {
            image.sprite = MainManager.Instance.GetItem(hotbarItemName).hotbarSprite;
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
        }
    }

    public void ResetHotbarItem()
    {
        hotbarItemName = Items.None;
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
