using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectionSubButtonPrefab : MonoBehaviour, IPointerEnterHandler
{
    //public static Action categorySubButton_isClicked;

    [SerializeField] Image buttonImage;

    public Item item = new Item();

    public GameObject newItemObject;


    //--------------------


    private void Start()
    {
        //Change Pos/Rot
        gameObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(0f, 0f, 0f));

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

        UpdateStates(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Instance.Play_Inventory_ItemHover_Clip();
    }


    //--------------------


    public void UpdateStates(bool changeStates)
    {
        CraftingManager.Instance.UpdateItemStates(this, changeStates);
        CraftingManager.Instance.UpdateCategoryButtonDisplay();
    }
    public void SetNewSubButtonItemDisplay(CraftingItem craftingItem)
    {
        if (craftingItem.itemState == CraftingItemState.Unactive)
        {
            newItemObject.SetActive(false);
        }
        else if (craftingItem.itemState == CraftingItemState.New)
        {
            newItemObject.SetActive(true);
        }
        else if (craftingItem.itemState == CraftingItemState.Active)
        {
            newItemObject.SetActive(false);
        }
    }


    //--------------------


    public void OnDestroySubCategoryObject()
    {
        //categorySubButton_isClicked -= OtherButtonClicked;

        DestroyImmediate(gameObject);
    }
}
