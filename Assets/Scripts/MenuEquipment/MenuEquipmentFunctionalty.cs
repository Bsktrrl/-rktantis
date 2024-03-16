using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuEquipmentFunctionalty : MonoBehaviour
{
    [SerializeField] bool equipped_Head_AutoFeeder;
    [SerializeField] bool equipped_Head_Headlight;
    [SerializeField] bool equipped_Head_Helmet;

    [SerializeField] bool equipped_Hand_ConstructionGloves;
    [SerializeField] bool equipped_Hand_MiningGloves;
    [SerializeField] bool equipped_Hand_PowerGloves;

    [SerializeField] bool equipped_Foot_RunningShoes;
    [SerializeField] bool equipped_Foot_LightShoes;
    [SerializeField] bool equipped_Foot_Slippers;


    //--------------------


    private void Start()
    {
        MenuEquipmentManager.updateEquipment += UpdateEquipmentToRun;
    }
    private void Update()
    {

    }


    //--------------------


    void UpdateEquipmentToRun()
    {
        //Head Equipment
        if (MenuEquipmentManager.Instance.HeadItemSelected == Items.AutoFeeder)
        {
            equipped_Head_AutoFeeder = true;
            equipped_Head_Headlight = false;
            equipped_Head_Helmet = false;
        }
        else if (MenuEquipmentManager.Instance.HeadItemSelected == Items.Headlight)
        {
            equipped_Head_AutoFeeder = false;
            equipped_Head_Headlight = true;
            equipped_Head_Helmet = false;
        }
        else if (MenuEquipmentManager.Instance.HeadItemSelected == Items.Helmet)
        {
            equipped_Head_AutoFeeder = false;
            equipped_Head_Headlight = false;
            equipped_Head_Helmet = true;
        }
        else
        {
            equipped_Head_AutoFeeder = false;
            equipped_Head_Headlight = false;
            equipped_Head_Helmet = false;
        }

        //Hand Equipment
        if (MenuEquipmentManager.Instance.HandItemSelected == Items.ConstructionGloves)
        {
            equipped_Hand_ConstructionGloves = true;
            equipped_Hand_MiningGloves = false;
            equipped_Hand_PowerGloves = false;
        }
        else if (MenuEquipmentManager.Instance.HandItemSelected == Items.MiningGloves)
        {
            equipped_Hand_ConstructionGloves = false;
            equipped_Hand_MiningGloves = true;
            equipped_Hand_PowerGloves = false;
        }
        else if (MenuEquipmentManager.Instance.HandItemSelected == Items.PowerGloves)
        {
            equipped_Hand_ConstructionGloves = false;
            equipped_Hand_MiningGloves = false;
            equipped_Hand_PowerGloves = true;
        }
        else
        {
            equipped_Hand_ConstructionGloves = false;
            equipped_Hand_MiningGloves = false;
            equipped_Hand_PowerGloves = false;
        }

        //Foot Equipment
        if (MenuEquipmentManager.Instance.FootItemSelected == Items.RunningShoes)
        {
            equipped_Foot_RunningShoes = true;
            equipped_Foot_LightShoes = false;
            equipped_Foot_Slippers = false;
        }
        else if (MenuEquipmentManager.Instance.FootItemSelected == Items.LightShoes)
        {
            equipped_Foot_RunningShoes = false;
            equipped_Foot_LightShoes = true;
            equipped_Foot_Slippers = false;
        }
        else if (MenuEquipmentManager.Instance.FootItemSelected == Items.Slippers)
        {
            equipped_Foot_RunningShoes = false;
            equipped_Foot_LightShoes = false;
            equipped_Foot_Slippers = true;
        }
        else
        {
            equipped_Foot_RunningShoes = false;
            equipped_Foot_LightShoes = false;
            equipped_Foot_Slippers = false;
        }
    }
}
