using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CategoryButton : MonoBehaviour, IPointerEnterHandler
{
    public ItemCategories categoryType;

    public void CategoryButton_OnClick()
    {
        print("Category selected");

        SoundManager.Instance.PlayChangeCraftingScreen_Clip();

        CraftingManager.instance.activeCategory = categoryType;
        CraftingManager.instance.SetupSelectionScreen();

        CraftingManager.instance.craftingScreen.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Instance.PlaySelect_Clip();
    }
}
