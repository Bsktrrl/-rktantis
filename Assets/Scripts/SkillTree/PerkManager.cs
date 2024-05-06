using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PerkManager : Singleton<PerkManager>
{
    public PerkValues perkValues;


    //--------------------


    #region Save/Load
    public void LoadData()
    {
        perkValues = DataManager.Instance.perks_Store;
    }
    public void SaveData()
    {
        DataManager.Instance.perks_Store = perkValues;
    }
    #endregion


    //--------------------


    public void UpdatePerkValues(Perk perk) //Make a long "else if"-list of ALL Perks, to be enabled
    {
        //Inventory
        #region
        if (perk.perkInfo.perkValues.playerInventory_Increase_Row > 0)
        {
            perkValues.playerInventory_Increase_Row = perk.perkInfo.perkValues.playerInventory_Increase_Row;

            InventoryManager.Instance.SetPlayerInventorySize();
        }
        else if (perk.perkInfo.perkValues.playerInventory_Increase_Column > 0)
        {
            perkValues.playerInventory_Increase_Column = perk.perkInfo.perkValues.playerInventory_Increase_Column;

            InventoryManager.Instance.SetPlayerInventorySize();
        }
        else if (perk.perkInfo.perkValues.healthByEating_Increasing_Percent > 0)
        {
            perkValues.healthByEating_Increasing_Percent = perk.perkInfo.perkValues.healthByEating_Increasing_Percent;
        }
        else if (perk.perkInfo.perkValues.chestInventory_Increase_Row > 0)
        {
            perkValues.chestInventory_Increase_Row = perk.perkInfo.perkValues.chestInventory_Increase_Row;

            InventoryManager.Instance.SetChestSize();
        }
        else if (perk.perkInfo.perkValues.chestInventory_Increase_Column > 0)
        {
            perkValues.chestInventory_Increase_Column = perk.perkInfo.perkValues.chestInventory_Increase_Column;

            InventoryManager.Instance.SetChestSize();
        }
        else if (perk.perkInfo.perkValues.keepInventoryItemsOnGameOver_Check == true)
        {
            perkValues.keepInventoryItemsOnGameOver_Check = perk.perkInfo.perkValues.keepInventoryItemsOnGameOver_Check;
        }
        #endregion

        //Player
        #region
        else if (perk.perkInfo.perkValues.upgradeableSuit_Check == true)
        {
            perkValues.upgradeableSuit_Check = perk.perkInfo.perkValues.upgradeableSuit_Check;
        }
        else if (perk.perkInfo.perkValues.playerTemperatureBuff_Upgrade > 0)
        {
            perkValues.playerTemperatureBuff_Upgrade = perk.perkInfo.perkValues.playerTemperatureBuff_Upgrade;
        }
        else if (perk.perkInfo.perkValues.weatherReport_Increase_ExtraDays > 0)
        {
            perkValues.weatherReport_Increase_ExtraDays = perk.perkInfo.perkValues.weatherReport_Increase_ExtraDays;
        }
        else if (perk.perkInfo.perkValues.playerMovement_Increase_Percentage > 0)
        {
            perkValues.playerMovement_Increase_Percentage = perk.perkInfo.perkValues.playerMovement_Increase_Percentage;
        }
        else if (perk.perkInfo.perkValues.playerRange_Increase_Percentage > 0)
        {
            perkValues.playerRange_Increase_Percentage = perk.perkInfo.perkValues.playerRange_Increase_Percentage;
        }
        else if (perk.perkInfo.perkValues.healthResistance_Increase_Percentage > 0)
        {
            perkValues.healthResistance_Increase_Percentage = perk.perkInfo.perkValues.healthResistance_Increase_Percentage;
        }
        else if (perk.perkInfo.perkValues.researchTime_Decrease_Percentage > 0)
        {
            perkValues.researchTime_Decrease_Percentage = perk.perkInfo.perkValues.researchTime_Decrease_Percentage;
        }
        #endregion

        //Tools
        #region
        else if (perk.perkInfo.perkValues.oreVeinDurability_Decrease > 0)
        {
            perkValues.oreVeinDurability_Decrease = perk.perkInfo.perkValues.oreVeinDurability_Decrease;
        }
        else if (perk.perkInfo.perkValues.treeDurability_Decrease > 0)
        {
            perkValues.treeDurability_Decrease = perk.perkInfo.perkValues.treeDurability_Decrease;
        }
        else if (perk.perkInfo.perkValues.toolDurability_Increase_Percentage > 0)
        {
            perkValues.toolDurability_Increase_Percentage = perk.perkInfo.perkValues.toolDurability_Increase_Percentage;
        }
        else if (perk.perkInfo.perkValues.resource_DropRate_Increase.x > 0 || perk.perkInfo.perkValues.resource_DropRate_Increase.y > 0)
        {
            perkValues.resource_DropRate_Increase = perk.perkInfo.perkValues.resource_DropRate_Increase;
        }
        else if (perk.perkInfo.perkValues.toolsCooldown_Decrease_Percentage > 0)
        {
            perkValues.toolsCooldown_Decrease_Percentage = perk.perkInfo.perkValues.toolsCooldown_Decrease_Percentage;
        }
        #endregion

        //Ar�dean
        #region
        else if (perk.perkInfo.perkValues.ghostCapturer_Slots_Increase > 0)
        {
            perkValues.ghostCapturer_Slots_Increase = perk.perkInfo.perkValues.ghostCapturer_Slots_Increase;
        }
        else if (perk.perkInfo.perkValues.ghostCapturer_CaptureEfficiency_Increase_Percentage > 0)
        {
            perkValues.ghostCapturer_CaptureEfficiency_Increase_Percentage = perk.perkInfo.perkValues.ghostCapturer_CaptureEfficiency_Increase_Percentage;
        }
        else if (perk.perkInfo.perkValues.ar�deanLight_Range_Increase_Percentage > 0)
        {
            perkValues.ar�deanLight_Range_Increase_Percentage = perk.perkInfo.perkValues.ar�deanLight_Range_Increase_Percentage;
        }
        else if (perk.perkInfo.perkValues.ar�dean_Visible_Check == true)
        {
            perkValues.ar�dean_Visible_Check = perk.perkInfo.perkValues.ar�dean_Visible_Check;
        }
        #endregion


        //--------------------


        SaveData();
    }
}

[Serializable]
public class PerkValues
{
    [Header("Inventory")]
    #region
    public int playerInventory_Increase_Row = 0; //Complete
    public int playerInventory_Increase_Column = 0; //Complete

    public float healthByEating_Increasing_Percent = 0;

    public int chestInventory_Increase_Row = 0; //Complete
    public int chestInventory_Increase_Column = 0; //Complete

    public bool keepInventoryItemsOnGameOver_Check = false;
    #endregion

    [Header("Player")]
    #region
    public bool upgradeableSuit_Check = false;
    public int playerTemperatureBuff_Upgrade = 0;
    public int weatherReport_Increase_ExtraDays = 0;

    public float playerMovement_Increase_Percentage = 0;
    public float playerRange_Increase_Percentage = 0;
    public float healthResistance_Increase_Percentage = 0;

    public float researchTime_Decrease_Percentage = 0;
    #endregion

    [Header("Tools")]
    #region
    public int oreVeinDurability_Decrease = 0;
    public int treeDurability_Decrease = 0;
    public int toolDurability_Increase_Percentage = 0; //Round up to whole numbers in the code

    public Vector2 resource_DropRate_Increase = new Vector2();
    public float toolsCooldown_Decrease_Percentage = 0;
    #endregion

    [Header("Ar�dean")]
    #region
    public int ghostCapturer_Slots_Increase = 0;
    public float ghostCapturer_CaptureEfficiency_Increase_Percentage = 0;
    public float ar�deanLight_Range_Increase_Percentage = 0;

    public bool ar�dean_Visible_Check;


    //--------------------


    [Space(20)]

    public float ghostMovementReducer_Right = 0;
    public float ghostMovementReducer_Up = 0;
    public float ghostMovementReducer_Speed = 0;
    #endregion
}