using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuEquipmentSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image slotImage;
    [SerializeField] Sprite slotSpriteActive;
    [SerializeField] Sprite slotSpriteUnactive;

    public Items itemName;


    //--------------------


    private void Start()
    {
        slotImage.sprite = slotSpriteUnactive;
    }


    //--------------------


    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Instance.Play_Inventory_ItemHover_Clip();

        slotImage.sprite = slotSpriteActive;

        if (itemName != Items.None)
        {
            InventoryManager.Instance.SetPlayerItemInfo(itemName, true);
            InventoryManager.Instance.ChangeItemInfoBox(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnPointerExit();
    }
    public void OnPointerExit()
    {
        slotImage.sprite = slotSpriteUnactive;

        InventoryManager.Instance.SetPlayerItemInfo(Items.None, true);
        InventoryManager.Instance.ChangeItemInfoBox(false);
    }
}
