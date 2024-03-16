using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectionSubButtonPrefab : MonoBehaviour, IPointerEnterHandler
{
    //public static Action categorySubButton_isClicked;

    [SerializeField] Image buttonImage;

    public Item item = new Item();


    //--------------------


    private void Start()
    {
        //categorySubButton_isClicked += OtherButtonClicked;
    }


    //--------------------

    void OtherButtonClicked()
    {
        //Set Frame Blue
        GetComponent<Image>().sprite = TabletManager.Instance.squareButton_Active;
    }


    //--------------------


    public void SetDisplay()
    {
        buttonImage.sprite = item.hotbarSprite;
        gameObject.name = item.itemName.ToString();
    }

    public void Button_OnClick()
    {
        //categorySubButton_isClicked?.Invoke();

        ////Set Frame Orange
        //GetComponent<Image>().sprite = TabletManager.Instance.squareButton_Passive;

        SoundManager.Instance.Play_Crafting_SelectCraftingItem_Clip();

        if (CraftingManager.Instance.activeItemToCraft != null)
        {
            CraftingManager.Instance.activeItemToCraft.GetComponent<Image>().sprite = TabletManager.Instance.squareButton_Active;
        }

        CraftingManager.Instance.activeItemToCraft = gameObject;

        SoundManager.Instance.Play_Crafting_ChangeCraftingMenu_Clip();

        CraftingManager.Instance.itemSelected = item;
        CraftingManager.Instance.SetupCraftingScreen(item);
        CraftingManager.Instance.craftingScreen.SetActive(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Instance.Play_Inventory_ItemHover_Clip();
    }


    //--------------------


    public void OnDestroySubCategoryObject()
    {
        //categorySubButton_isClicked -= OtherButtonClicked;

        DestroyImmediate(gameObject);
    }
}
