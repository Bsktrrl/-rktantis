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
        if (MainManager.Instance.gameStates == GameStates.GameOver) { return; }

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

            //If crafting a seed, get x2
            if (CraftingManager.Instance.itemSelected.itemName == Items.ArídisPlantSeed
                || CraftingManager.Instance.itemSelected.itemName == Items.GluePlantSeed
                || CraftingManager.Instance.itemSelected.itemName == Items.CrimsonCloudBushSeed
                || CraftingManager.Instance.itemSelected.itemName == Items.RedCottonPlantSeed
                || CraftingManager.Instance.itemSelected.itemName == Items.SpikPlantSeed
                || CraftingManager.Instance.itemSelected.itemName == Items.SmallCactusplantSeed
                || CraftingManager.Instance.itemSelected.itemName == Items.PuddingCactusSeed
                || CraftingManager.Instance.itemSelected.itemName == Items.StalkFruitSeed
                || CraftingManager.Instance.itemSelected.itemName == Items.TripodFruitSeed
                || CraftingManager.Instance.itemSelected.itemName == Items.HeatFruitSeed
                || CraftingManager.Instance.itemSelected.itemName == Items.FreezeFruitSeed
                || CraftingManager.Instance.itemSelected.itemName == Items.TwistedMushroomSeed
                || CraftingManager.Instance.itemSelected.itemName == Items.GroundMushroomSeed
                || CraftingManager.Instance.itemSelected.itemName == Items.SandTubesSeed)
            {
                InventoryManager.Instance.AddItemToInventory(0, CraftingManager.Instance.itemSelected.itemName);
                InventoryManager.Instance.AddItemToInventory(0, CraftingManager.Instance.itemSelected.itemName);
            }

            //If not crafting a Seed, get x1
            else
            {
                InventoryManager.Instance.AddItemToInventory(0, CraftingManager.Instance.itemSelected.itemName);
            }

            //InventoryManager.Instance.CheckHotbarItemInInventory();
            SoundManager.Instance.Play_Crafting_PerformCrafting_Clip();
        }
        else
        {
            //print("CraftingButton - TotalRequirementMet = false");
            SoundManager.Instance.Play_Crafting_CannotCraft_Clip();
        }
    }
}
