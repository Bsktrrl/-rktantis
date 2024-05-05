using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PerkManager : Singleton<PerkManager>
{
    public PerkValues perkValues;
    //public PerkActivations perkActivations;


    //--------------------


    public void LoadData()
    {
        perkValues = DataManager.Instance.perks_Store;
        //perkActivations = DataManager.Instance.perkActivations_Store;
    }
    public void SaveData()
    {
        DataManager.Instance.perks_Store = perkValues;
        //DataManager.Instance.perkActivations_Store = perkActivations;
    }


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


    [Header("Arídite")]


    [Header("Ghost")]
    //MovementReducer
    public float ghostMovementReducer_Right = 0;
    public float ghostMovementReducer_Up = 0;
    public float ghostMovementReducer_Speed = 0;
}

//[Serializable]
//public class PerkActivations
//{

//    [Header("Player")]


//    [Header("Inventory")]


//    [Header("Tools")]


//    [Header("Arídite")]


//    [Header("Ghost")]
//    //MovementReducer
//    public bool ghost_T1_MovementReducer_Right = false;
//    public bool ghost_T1_MovementReducer_Up = false;
//    public bool ghost_T1_MovementReducer_Speed = false;

//    public bool ghost_T2_MovementReducer_Right = false;
//    public bool ghost_T2_MovementReducer_Up = false;
//    public bool ghost_T2_MovementReducer_Speed = false;

//    public bool ghost_T3_MovementReducer_Right = false;
//    public bool ghost_T3_MovementReducer_Up = false;
//    public bool ghost_T3_MovementReducer_Speed = false;
//}