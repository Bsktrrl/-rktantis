using System;
using UnityEngine;

public class PerkManager : Singleton<PerkManager>
{
    public Perks perks;
    public PerkActivations perkActivations;


    //--------------------


    public void LoadData()
    {
        perks = DataManager.Instance.perks_Store;
        perkActivations = DataManager.Instance.perkActivations_Store;
    }
    public void SaveData()
    {
        DataManager.Instance.perks_Store = perks;
        DataManager.Instance.perkActivations_Store = perkActivations;
    }
}

[Serializable]
public class Perks
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

[Serializable]
public class PerkActivations
{
    [Header("Player")]


    [Header("Inventory")]


    [Header("Tools")]


    [Header("Arídite")]


    [Header("Ghost")]
    //MovementReducer
    public bool ghost_T1_MovementReducer_Right = false;
    public bool ghost_T1_MovementReducer_Up = false;
    public bool ghost_T1_MovementReducer_Speed = false;
    public bool ghost_T2_MovementReducer_Right = false;
    public bool ghost_T2_MovementReducer_Up = false;
    public bool ghost_T2_MovementReducer_Speed = false;
    public bool ghost_T3_MovementReducer_Right = false;
    public bool ghost_T3_MovementReducer_Up = false;
    public bool ghost_T3_MovementReducer_Speed = false;
}