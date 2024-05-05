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
    [Header("Player")]


    [Header("Inventory")]


    [Header("Tools")]


    [Header("Arídean")]
    //MovementReducer
    public float ghostMovementReducer_Right = 0;
    public float ghostMovementReducer_Up = 0;
    public float ghostMovementReducer_Speed = 0;
}