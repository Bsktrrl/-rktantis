using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftButton : MonoBehaviour
{
    [SerializeField] Button craftingButton;
    [SerializeField] Sprite craftingButtonColor_Inactive;
    [SerializeField] Sprite craftingButtonColor_Active;

    private void Update()
    {
        if (MainManager.Instance.menuStates == MenuStates.CraftingMenu)
        {
            if (CraftingManager.Instance.totalRequirementMet)
            {
                craftingButton.GetComponent<Image>().sprite = craftingButtonColor_Active;
            }
            else
            {
                craftingButton.GetComponent<Image>().sprite = craftingButtonColor_Inactive;
            }
        }
    }

    public void CraftingButton_OnClick()
    {
        if (CraftingManager.Instance.totalRequirementMet)
        {
            //print("CraftingButton - TotalRequirementMet = true");

            //Remove items from inventory
            for (int i = 0; i < CraftingManager.Instance.requirementPrefabList.Count; i++)
            {
                Items itemName = CraftingManager.Instance.requirementPrefabList[i].GetComponent<CraftingRequirementPrefab>().requirements.itemName;
                int amount = CraftingManager.Instance.requirementPrefabList[i].GetComponent<CraftingRequirementPrefab>().requirements.amount;

                for (int j = 0; j < amount; j++)
                {
                    InventoryManager.Instance.RemoveItemFromInventory(0, itemName, -1, false);
                }
            }

            InventoryManager.Instance.AddItemToInventory(0, CraftingManager.Instance.itemSelected.itemName);
            //InventoryManager.Instance.CheckHotbarItemInInventory();

            SoundManager.Instance.Playmenu_Crafting_Clip();
        }
        else
        {
            //print("CraftingButton - TotalRequirementMet = false");
            SoundManager.Instance.Playmenu_CanntoCraft_Clip();
        }
    }
}
