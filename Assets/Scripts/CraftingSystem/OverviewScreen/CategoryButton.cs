using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CategoryButton : MonoBehaviour, IPointerEnterHandler
{
    public static Action categoryButton_isClicked;

    public ItemCategories categoryType;

    public GameObject newItemObject;


    //--------------------


    private void Start()
    {
        categoryButton_isClicked += OtherButtonClicked;

        //Change Pos/Rot
        gameObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(0f, 0f, 0f));

        //Set Frame Blue
        GetComponent<Image>().sprite = TabletManager.Instance.squareButton_Active;
    }
    private void Update()
    {
        SetNewCategoryButtonItemDisplay();
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


    //--------------------

    public void SetNewCategoryButtonItemDisplay()
    {
        newItemObject.SetActive(false);

        for (int i = 0; i < CraftingManager.Instance.itemStateList.Count; i++)
        {
            if (CraftingManager.Instance.itemStateList[i].itemCategory == categoryType
                && CraftingManager.Instance.itemStateList[i].itemState == CraftingItemState.New)
            {
                if (CraftingManager.Instance.itemStateList[i].itemName == Items.WoodSword
                    || CraftingManager.Instance.itemStateList[i].itemName == Items.StoneSword
                    || CraftingManager.Instance.itemStateList[i].itemName == Items.CryoniteSword)
                {
                    CraftingManager.Instance.itemStateList[i].itemState = CraftingItemState.Unactive;
                }
                else
                {
                    newItemObject.SetActive(true);
                }

                break;
            }
        }
    }
}
