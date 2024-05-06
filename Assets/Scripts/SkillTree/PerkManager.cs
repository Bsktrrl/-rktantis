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
        if (perk.perkInfo.perkValues.playerInventory_Increase_Column > 0)
        {
            perkValues.playerInventory_Increase_Column = perk.perkInfo.perkValues.playerInventory_Increase_Column;

            InventoryManager.Instance.SetPlayerInventorySize();
        }
        if (perk.perkInfo.perkValues.healthByEating_Increasing_Percent > 0)
        {
            perkValues.healthByEating_Increasing_Percent = perk.perkInfo.perkValues.healthByEating_Increasing_Percent;
        }
        if (perk.perkInfo.perkValues.chestInventory_Increase_Row > 0)
        {
            perkValues.chestInventory_Increase_Row = perk.perkInfo.perkValues.chestInventory_Increase_Row;

            InventoryManager.Instance.SetChestSize();
        }
        if (perk.perkInfo.perkValues.chestInventory_Increase_Column > 0)
        {
            perkValues.chestInventory_Increase_Column = perk.perkInfo.perkValues.chestInventory_Increase_Column;

            InventoryManager.Instance.SetChestSize();
        }
        if (perk.perkInfo.perkValues.keepInventoryItemsOnGameOver_Check == true)
        {
            perkValues.keepInventoryItemsOnGameOver_Check = perk.perkInfo.perkValues.keepInventoryItemsOnGameOver_Check;
        }
        #endregion

        //Player
        #region
        if (perk.perkInfo.perkValues.upgradeableSuit_Check == true)
        {
            perkValues.upgradeableSuit_Check = perk.perkInfo.perkValues.upgradeableSuit_Check;
        }
        if (perk.perkInfo.perkValues.playerTemperatureBuff_Upgrade > 0)
        {
            perkValues.playerTemperatureBuff_Upgrade = perk.perkInfo.perkValues.playerTemperatureBuff_Upgrade;
        }
        if (perk.perkInfo.perkValues.weatherReport_Increase_ExtraDays > 0)
        {
            perkValues.weatherReport_Increase_ExtraDays = perk.perkInfo.perkValues.weatherReport_Increase_ExtraDays;

            WeatherManager.Instance.SetWeatherReportDisplay();
        }
        if (perk.perkInfo.perkValues.playerMovement_Increase_Percentage > 0)
        {
            perkValues.playerMovement_Increase_Percentage = perk.perkInfo.perkValues.playerMovement_Increase_Percentage;
        }
        if (perk.perkInfo.perkValues.playerRange_Increase_Percentage > 0)
        {
            perkValues.playerRange_Increase_Percentage = perk.perkInfo.perkValues.playerRange_Increase_Percentage;
        }
        if (perk.perkInfo.perkValues.healthResistance_Increase_Percentage > 0)
        {
            perkValues.healthResistance_Increase_Percentage = perk.perkInfo.perkValues.healthResistance_Increase_Percentage;

            HealthManager.Instance.SetPlayerHealthSpeed();
        }
        if (perk.perkInfo.perkValues.researchTime_Decrease_Percentage > 0)
        {
            perkValues.researchTime_Decrease_Percentage = perk.perkInfo.perkValues.researchTime_Decrease_Percentage;
        }
        #endregion

        //Tools
        #region
        if (perk.perkInfo.perkValues.oreVeinDurability_Decrease > 0)
        {
            perkValues.oreVeinDurability_Decrease = perk.perkInfo.perkValues.oreVeinDurability_Decrease;
        }
        if (perk.perkInfo.perkValues.treeDurability_Decrease > 0)
        {
            perkValues.treeDurability_Decrease = perk.perkInfo.perkValues.treeDurability_Decrease;
        }
        if (perk.perkInfo.perkValues.toolDurability_Increase_Percentage > 0)
        {
            perkValues.toolDurability_Increase_Percentage = perk.perkInfo.perkValues.toolDurability_Increase_Percentage;
        }
        if (perk.perkInfo.perkValues.resource_DropRate_Increase.x > 0 || perk.perkInfo.perkValues.resource_DropRate_Increase.y > 0)
        {
            perkValues.resource_DropRate_Increase = perk.perkInfo.perkValues.resource_DropRate_Increase;
        }
        if (perk.perkInfo.perkValues.toolsCooldown_Decrease_Percentage > 0)
        {
            perkValues.toolsCooldown_Decrease_Percentage = perk.perkInfo.perkValues.toolsCooldown_Decrease_Percentage;
        }
        #endregion

        //Arídean
        #region
        if (perk.perkInfo.perkValues.ghostCapturer_Slots_Increase > 0)
        {
            perkValues.ghostCapturer_Slots_Increase = perk.perkInfo.perkValues.ghostCapturer_Slots_Increase;
        }
        if (perk.perkInfo.perkValues.ghostCapturer_CaptureEfficiency_Increase_Percentage > 0)
        {
            perkValues.ghostCapturer_CaptureEfficiency_Increase_Percentage = perk.perkInfo.perkValues.ghostCapturer_CaptureEfficiency_Increase_Percentage;
        }
        if (perk.perkInfo.perkValues.arídeanLight_Range_Increase_Percentage > 0)
        {
            perkValues.arídeanLight_Range_Increase_Percentage = perk.perkInfo.perkValues.arídeanLight_Range_Increase_Percentage;
        }
        if (perk.perkInfo.perkValues.arídean_Visible_Check == true)
        {
            perkValues.arídean_Visible_Check = perk.perkInfo.perkValues.arídean_Visible_Check;
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

    public float healthByEating_Increasing_Percent = 0; //Complete

    public int chestInventory_Increase_Row = 0; //Complete
    public int chestInventory_Increase_Column = 0; //Complete

    public bool keepInventoryItemsOnGameOver_Check = false; //Complete
    #endregion

    [Header("Player")]
    #region
    public bool upgradeableSuit_Check = false; //Complete
    public int playerTemperatureBuff_Upgrade = 0; //Complete
    public int weatherReport_Increase_ExtraDays = 0; //Complete

    public float playerMovement_Increase_Percentage = 0; //Complete
    public float playerRange_Increase_Percentage = 0; //Complete
    public float healthResistance_Increase_Percentage = 0; //Complete

    public float researchTime_Decrease_Percentage = 0; //Complete
    #endregion

    [Header("Tools")]
    #region
    public int oreVeinDurability_Decrease = 0; //Round up to whole numbers in the code
    public int treeDurability_Decrease = 0; //Round up to whole numbers in the code
    public int toolDurability_Increase_Percentage = 0; //Round up to whole numbers in the code

    public Vector2 resource_DropRate_Increase = new Vector2(); 
    public float toolsCooldown_Decrease_Percentage = 0; 
    #endregion

    [Header("Arídean")]
    #region
    public int ghostCapturer_Slots_Increase = 0; 
    public float ghostCapturer_CaptureEfficiency_Increase_Percentage = 0; 
    public float arídeanLight_Range_Increase_Percentage = 0; 

    public bool arídean_Visible_Check; 


    //--------------------


    [Space(20)]

    public float ghostMovementReducer_Right = 0;
    public float ghostMovementReducer_Up = 0;
    public float ghostMovementReducer_Speed = 0;
    #endregion
}