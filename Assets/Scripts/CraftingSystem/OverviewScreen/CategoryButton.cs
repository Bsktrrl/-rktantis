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


        SoundManager.Instance.PlayChangeCraftingScreen_Clip();

        CraftingManager.Instance.activeCategory = categoryType;
        CraftingManager.Instance.SetupSelectionScreen();

        CraftingManager.Instance.craftingScreen.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Instance.PlaySelect_Clip();
    }

    public void OnDestroyCategoryObject()
    {
        categoryButton_isClicked -= OtherButtonClicked;

        DestroyImmediate(gameObject);
    }
}
