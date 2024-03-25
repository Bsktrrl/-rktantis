using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CategoryButton : MonoBehaviour, IPointerEnterHandler
{
    public static Action categoryButton_isClicked;

    public ItemCategories categoryType;


    //--------------------


    private void Start()
    {
        categoryButton_isClicked += OtherButtonClicked;

        //Change Pos/Rot
        gameObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(0f, 0f, 0f));

        //Set Frame Blue
        GetComponent<Image>().sprite = TabletManager.Instance.squareButton_Active;
    }


    //--------------------


    void OtherButtonClicked()
    {
        //Set Frame Blue
        GetComponent<Image>().sprite = TabletManager.Instance.squareButton_Active;
    }


    //--------------------


    public void CategoryButton_OnClick()
    {
        categoryButton_isClicked?.Invoke();

        //Set Frame Orange
        GetComponent<Image>().sprite = TabletManager.Instance.squareButton_Passive;


        SoundManager.Instance.Play_Crafting_ChangeCraftingMenu_Clip();

        CraftingManager.Instance.activeCategory = categoryType;
        CraftingManager.Instance.SetupSelectionScreen();

        CraftingManager.Instance.craftingScreen.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Instance.Play_Inventory_ItemHover_Clip();
    }

    public void OnDestroyCategoryObject()
    {
        categoryButton_isClicked -= OtherButtonClicked;

        DestroyImmediate(gameObject);
    }
}
