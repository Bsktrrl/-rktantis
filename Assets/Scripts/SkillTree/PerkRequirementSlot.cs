using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PerkRequirementSlot : MonoBehaviour
{
    public Image requirement_image;
    public Image requirement_BGimage;
    public TextMeshProUGUI requirement_amount;


    //--------------------


    public void SetRequirementSlot(Sprite sprite, Items itemName, int amount)
    {
        requirement_image.sprite = sprite;
        requirement_amount.text = "x" + amount.ToString() + "/" + InventoryManager.Instance.GetAmountOfItemInInventory(0, itemName);
    }

    public void DestroyThisObject()
    {
        Destroy(gameObject);
    }
}
