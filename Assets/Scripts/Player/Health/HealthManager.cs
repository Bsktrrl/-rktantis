using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : Singleton<HealthManager>
{
    public GameObject health_Parent;

    [SerializeField] Image hunger_Image;
    [SerializeField] Image heatResistance_Image;
    [SerializeField] Image thirst_Image;
    [SerializeField] Image mainHealth_Image;

    public float hungerValue = 1;
    public float hungerValueSpeed = -0.01f;

    public float heatResistanceValue = 1;
    public float heatResistanceValueSpeed = -0.03f;

    public float thirstValue = 1;
    public float thirstValueSpeed = -0.02f;

    public float mainHealthValue = 1;
    public float mainHealthValueSpeed = -0.01f;


    //--------------------


    private void Start()
    {
        health_Parent.SetActive(true);
    }
    private void Update()
    {
        SetHealthValues();
        SetHealthDisplay();
    }


    //--------------------

    
    void SetHealthValues()
    {
        #region SpeedChecks
        hungerValue += hungerValueSpeed;
        if (hungerValue <= 0)
            hungerValue = 0;
        else if (hungerValue >= 1)
            hungerValue = 1;

        heatResistanceValue += heatResistanceValueSpeed;
        if (heatResistanceValue <= 0)
            heatResistanceValue = 0;
        else if (heatResistanceValue >= 1)
            heatResistanceValue = 1;

        thirstValue += thirstValueSpeed;
        if (thirstValue <= 0)
            thirstValue = 0;
        else if (thirstValue >= 1)
            thirstValue = 1;
        #endregion

        //Main Health Parameter
        int counter = 0;
        if (hungerValue <= 0)
            counter++;

        if (heatResistanceValue <= 0)
            counter++;

        if (thirstValue <= 0)
            counter++;

        mainHealthValue += -Mathf.Abs(mainHealthValueSpeed * counter);
        if (mainHealthValue <= 0)
            mainHealthValue = 0;
        else if (mainHealthValue >= 1)
            mainHealthValue = 1;
    }

    void SetHealthDisplay()
    {
        hunger_Image.fillAmount = hungerValue;
        heatResistance_Image.fillAmount = heatResistanceValue;
        thirst_Image.fillAmount = thirstValue;
        mainHealth_Image.fillAmount = mainHealthValue;
    }
}
