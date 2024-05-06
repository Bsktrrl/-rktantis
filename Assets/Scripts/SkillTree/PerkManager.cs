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
        //floats
        if (perk.perkInfo.perkValues.ghostMovementReducer_Right > 0)
        {
            perkValues.ghostMovementReducer_Right = perk.perkInfo.perkValues.ghostMovementReducer_Right;
        }
        else if (perk.perkInfo.perkValues.ghostMovementReducer_Up > 0)
        {
            perkValues.ghostMovementReducer_Up = perk.perkInfo.perkValues.ghostMovementReducer_Up;
        }
        else if (perk.perkInfo.perkValues.ghostMovementReducer_Speed > 0)
        {
            perkValues.ghostMovementReducer_Speed = perk.perkInfo.perkValues.ghostMovementReducer_Speed;
        }

        //Bools



        //--------------------


        SaveData();
    }
}

[Serializable]
public class PerkValues
{
    [Header("Inventory")]
    public int playerInventory_Increase_Row = 0;
    public int playerInventory_Increase_Column = 0;

    public float healthByEating_Increasing_Percent = 0;

    public int chestInventory_Increase_Row = 0;
    public int chestInventory_Increase_Column = 0;

    public bool keepInventoryItemsOnGameOver_Check = false;

    [Header("Player")]
    public bool upgradeableSuit_Check = false;
    public int playerTemperatureBuff_Upgrade = 0;
    public int weatherReport_Increase_ExtraDays = 0;

    public float playerMovement_Increase_Percentage = 0;
    public float playerRange_Increase_Percentage = 0;
    public float healthResistance_Increase_Percentage = 0;

    public float researchTime_Decrease_Percentage = 0;

    [Header("Tools")]
    public int oreVeinDurability_Decrease = 0;
    public int treeDurability_Decrease = 0;
    public int toolDurability_Increase_Percentage = 0; //Round up to whole numbers in the code

    public Vector2 resource_DropRate_Increase = new Vector2();
    public float toolsCooldown_Decrease_Percentage = 0;

    [Header("Arídean")]
    public int ghostCapturer_Slots_Increase = 0;
    public float ghostCapturer_CaptureEfficiency_Increase_Percentage = 0;
    public float arídeanLight_Range_Increase_Percentage = 0;

    public bool arídean_Visible_Check;

    [Space(20)]

    public float ghostMovementReducer_Right = 0;
    public float ghostMovementReducer_Up = 0;
    public float ghostMovementReducer_Speed = 0;
}