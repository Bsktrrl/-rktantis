using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectionSubButtonPrefab : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] Image buttonImage;

    public Item item = new Item();

    public void SetDisplay()
    {
        buttonImage.sprite = item.hotbarSprite;
        gameObject.name = item.itemName.ToString();
    }

    public void Button_OnClick()
    {
        SoundManager.Instance.PlayChangeCraftingScreen_Clip();

        CraftingManager.Instance.itemSelected = item;
        CraftingManager.Instance.SetupCraftingScreen(item);
        CraftingManager.Instance.craftingScreen.SetActive(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Instance.PlaySelect_Clip();
    }
}
