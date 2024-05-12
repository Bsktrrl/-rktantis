using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PerkRequirementSlot : MonoBehaviour
{
    public Image requirement_image;
    public Image requirement_BGimage;
    public Image requirement_requirementImage;
    public TextMeshProUGUI requirement_Name;
    public TextMeshProUGUI requirement_amount;


    //--------------------


    public void SetRequirementSlot(Sprite sprite, Items itemName, int amount)
    {
        requirement_image.sprite = sprite;
        requirement_amount.text = "x" + InventoryManager.Instance.GetAmountOfItemInInventory(0, itemName) + "/" + amount.ToString();
        requirement_Name.text = SpaceTextConverting.Instance.SetText(itemName.ToString());

        if (amount > InventoryManager.Instance.GetAmountOfItemInInventory(0, itemName))
        {
            requirement_requirementImage.gameObject.SetActive(true);
        }
        else
        {
            requirement_requirementImage.gameObject.SetActive(false);
        }
    }

    public void DestroyThisObject()
    {
        Destroy(gameObject);
    }
}
