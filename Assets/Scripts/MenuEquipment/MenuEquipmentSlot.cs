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
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        slotImage.sprite = slotSpriteUnactive;
    }
}
