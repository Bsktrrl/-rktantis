using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : Singleton<HealthManager>
{
    #region Variables
    public GameObject health_Parent;

    public HealthToSave health_ToSave;

    public float healthSpeed;

    [Header("Hunger Parameter")]
    [SerializeField] Image hunger_Image;
    public float hungerValue = 1;
    [SerializeField] List<GameObject> hungerValueMultiplier_Image = new List<GameObject>();
    public HealthValueMultiplier hungerValueMultiplier_Check = HealthValueMultiplier.None;
    public int healthValueMultiplier;

    [Header("HeatResistance Parameter")]
    [SerializeField] Image heatResistance_Image;
    public float heatResistanceValue = 1;
    [SerializeField] List<GameObject> heatResistanceValueMultiplier_Image = new List<GameObject>();
    public HealthValueMultiplier heatResistanceValueMultiplier_Check = HealthValueMultiplier.None;
    public int heatResistanceValueMultiplier;

    [Header("Thirst Parameter")]
    [SerializeField] Image thirst_Image;
    public float thirstValue = 1;
    [SerializeField] List<GameObject> thirstValueMultiplier_Image = new List<GameObject>();
    public HealthValueMultiplier thirstValueMultiplier_Check = HealthValueMultiplier.None;
    public int thirstValueMultiplier;

    [Header("MainHealth Parameter")]
    [SerializeField] Image mainHealth_Image;
    public float mainHealthValue = 1;
    [SerializeField] List<GameObject> mainHealthValueMultiplier_Image = new List<GameObject>();
    public HealthValueMultiplier mainHealthValueMultiplier_Check = HealthValueMultiplier.None;
    public int mainHealthValueMultiplier;

    bool dataHasLoaded = false;
    #endregion


    //--------------------


    private void Start()
    {
        health_Parent.SetActive(true);

        //Set Arrows unactive
        #region
        for (int i = 0; i < hungerValueMultiplier_Image.Count; i++)
        {
            hungerValueMultiplier_Image[i].SetActive(false);
        }
        for (int i = 0; i < heatResistanceValueMultiplier_Image.Count; i++)
        {
            heatResistanceValueMultiplier_Image[i].SetActive(false);
        }
        for (int i = 0; i < thirstValueMultiplier_Image.Count; i++)
        {
            thirstValueMultiplier_Image[i].SetActive(false);
        }
        for (int i = 0; i < mainHealthValueMultiplier_Image.Count; i++)
        {
            mainHealthValueMultiplier_Image[i].SetActive(false);
        }
        #endregion

        //Set All Multipliers to 0
        //hungerValueMultiplier_Check = HealthValueMultiplier.None;
        //heatResistanceValueMultiplier_Check = HealthValueMultiplier.None;
        //thirstValueMultiplier_Check = HealthValueMultiplier.None;
        //mainHealthValueMultiplier_Check = HealthValueMultiplier.None;
    }
    private void Update()
    {
        if (dataHasLoaded)
        {
            SetHealthValues();
            SetHealthDisplay();

            SaveData();
        }
    }


    //--------------------


    public void LoadData()
    {
        HealthToSave tempHealth = DataManager.Instance.health_Store;

        hungerValue = tempHealth.hungerValue;
        hungerValueMultiplier_Check = tempHealth.hungerValueMultiplier_Check;
        healthValueMultiplier = tempHealth.healthValueMultiplier;

        heatResistanceValue = tempHealth.heatResistanceValue;
        heatResistanceValueMultiplier_Check = tempHealth.heatResistanceValueMultiplier_Check;
        heatResistanceValueMultiplier = tempHealth.heatResistanceValueMultiplier;

        thirstValue = tempHealth.thirstValue;
        thirstValueMultiplier_Check = tempHealth.thirstValueMultiplier_Check;
        thirstValueMultiplier = tempHealth.thirstValueMultiplier;

        mainHealthValue = tempHealth.mainHealthValue;
        mainHealthValueMultiplier_Check = tempHealth.mainHealthValueMultiplier_Check;
        mainHealthValueMultiplier = tempHealth.mainHealthValueMultiplier;

        dataHasLoaded = true;
    }
    void SaveData()
    {
        health_ToSave.hungerValue = hungerValue;
        health_ToSave.hungerValueMultiplier_Check = hungerValueMultiplier_Check;
        health_ToSave.healthValueMultiplier = healthValueMultiplier;

        health_ToSave.heatResistanceValue = heatResistanceValue;
        health_ToSave.heatResistanceValueMultiplier_Check = heatResistanceValueMultiplier_Check;
        health_ToSave.heatResistanceValueMultiplier = heatResistanceValueMultiplier;

        health_ToSave.thirstValue = thirstValue;
        health_ToSave.thirstValueMultiplier_Check = thirstValueMultiplier_Check;
        health_ToSave.thirstValueMultiplier = thirstValueMultiplier;

        health_ToSave.mainHealthValue = mainHealthValue;
        health_ToSave.mainHealthValueMultiplier_Check = mainHealthValueMultiplier_Check;
        health_ToSave.mainHealthValueMultiplier = mainHealthValueMultiplier;

        DataManager.Instance.health_Store = health_ToSave;
    }


    //--------------------


    void SetHealthValues()
    {
        //Multiplier Check
        #region
        healthValueMultiplier = MultiplierCheck(hungerValueMultiplier_Check);
        heatResistanceValueMultiplier = MultiplierCheck(heatResistanceValueMultiplier_Check);
        thirstValueMultiplier = MultiplierCheck(thirstValueMultiplier_Check);
        #endregion

        //Speed Check
        #region
        hungerValue += (healthSpeed * healthValueMultiplier);
        if (hungerValue <= 0)
            hungerValue = 0;
        else if (hungerValue >= 1)
            hungerValue = 1;

        heatResistanceValue += (healthSpeed * heatResistanceValueMultiplier);
        if (heatResistanceValue <= 0)
            heatResistanceValue = 0;
        else if (heatResistanceValue >= 1)
            heatResistanceValue = 1;

        thirstValue += (healthSpeed * thirstValueMultiplier);
        if (thirstValue <= 0)
            thirstValue = 0;
        else if (thirstValue >= 1)
            thirstValue = 1;
        #endregion

        //Set Main Health Parameter
        #region
        int counter = 0;
        if (hungerValue <= 0)
            counter++;

        if (heatResistanceValue <= 0)
            counter++;

        if (thirstValue <= 0)
            counter++;

        if (counter <= 0)
        {
            mainHealthValue += Mathf.Abs(healthSpeed);
        }
        else
        {
            mainHealthValue += -Mathf.Abs(healthSpeed * counter);
        }
       
        if (mainHealthValue <= 0)
            mainHealthValue = 0;
        else if (mainHealthValue >= 1)
            mainHealthValue = 1;
        #endregion

        //Set Arrow Display
        #region
        for (int i = 0; i < mainHealthValueMultiplier_Image.Count; i++)
        {
            mainHealthValueMultiplier_Image[i].SetActive(false);
        }

        if (counter <= 0 && mainHealthValue >= 1)
        {
            
        }
        else if (counter <= 0)
        {
            mainHealthValueMultiplier_Image[4].SetActive(true);
        }
        else if (counter == 1)
        {
            mainHealthValueMultiplier_Image[2].SetActive(true);
        }
        else if (counter == 2)
        {
            mainHealthValueMultiplier_Image[1].SetActive(true);
            mainHealthValueMultiplier_Image[2].SetActive(true);
        }
        else if (counter >= 3)
        {
            mainHealthValueMultiplier_Image[0].SetActive(true);
            mainHealthValueMultiplier_Image[1].SetActive(true);
            mainHealthValueMultiplier_Image[2].SetActive(true);
        }
        #endregion
    }

    int MultiplierCheck(HealthValueMultiplier multiplier_Check)
    {
        switch (multiplier_Check)
        {
            case HealthValueMultiplier.Down_6:
                return -6;
            case HealthValueMultiplier.Down_5:
                return -5;
            case HealthValueMultiplier.Down_4:
                return -4;
            case HealthValueMultiplier.Down_3:
                return -3;
            case HealthValueMultiplier.Down_2:
                return -2;
            case HealthValueMultiplier.Down_1:
                return -1;
            case HealthValueMultiplier.None:
                return 0;
            case HealthValueMultiplier.Up_1:
                return 1;
            case HealthValueMultiplier.Up_2:
                return 2;
            case HealthValueMultiplier.Up_3:
                return 3;
            case HealthValueMultiplier.Up_4:
                return 4;
            case HealthValueMultiplier.Up_5:
                return 5;
            case HealthValueMultiplier.Up_6:
                return 6;

            default:
                return 0;
        }
    }

    void SetHealthDisplay()
    {
        hunger_Image.fillAmount = hungerValue;
        heatResistance_Image.fillAmount = heatResistanceValue;
        thirst_Image.fillAmount = thirstValue;
        mainHealth_Image.fillAmount = mainHealthValue;

        //Set active arrows
        ArrowSetActive(hungerValueMultiplier_Check, hungerValueMultiplier_Image);
        ArrowSetActive(heatResistanceValueMultiplier_Check, heatResistanceValueMultiplier_Image);
        ArrowSetActive(thirstValueMultiplier_Check, thirstValueMultiplier_Image);
    }
    void ArrowSetActive(HealthValueMultiplier multiplier, List<GameObject> imagesList)
    {
        //Unactivate all arrows
        for (int i = 0; i < imagesList.Count; i++)
        {
            imagesList[i].SetActive(false);
        }

        switch (multiplier)
        {
            case HealthValueMultiplier.Down_6:
                imagesList[0].SetActive(true);
                imagesList[1].SetActive(true);
                imagesList[2].SetActive(true);
                break;
            case HealthValueMultiplier.Down_5:
                imagesList[0].SetActive(true);
                imagesList[1].SetActive(true);
                imagesList[2].SetActive(true);
                break;
            case HealthValueMultiplier.Down_4:
                imagesList[1].SetActive(true);
                imagesList[2].SetActive(true);
                break;
            case HealthValueMultiplier.Down_3:
                imagesList[1].SetActive(true);
                imagesList[2].SetActive(true);
                break;
            case HealthValueMultiplier.Down_2:
                imagesList[2].SetActive(true);
                break;
            case HealthValueMultiplier.Down_1:
                imagesList[2].SetActive(true);
                break;
            case HealthValueMultiplier.None:
                break;
            case HealthValueMultiplier.Up_1:
                imagesList[4].SetActive(true);
                break;
            case HealthValueMultiplier.Up_2:
                imagesList[4].SetActive(true);
                break;
            case HealthValueMultiplier.Up_3:
                imagesList[4].SetActive(true);
                imagesList[5].SetActive(true);
                break;
            case HealthValueMultiplier.Up_4:
                imagesList[4].SetActive(true);
                imagesList[5].SetActive(true);
                break;
            case HealthValueMultiplier.Up_5:
                imagesList[4].SetActive(true);
                imagesList[5].SetActive(true);
                imagesList[6].SetActive(true);
                break;
            case HealthValueMultiplier.Up_6:
                imagesList[4].SetActive(true);
                imagesList[5].SetActive(true);
                imagesList[6].SetActive(true);
                break;

            default:
                break;
        }
    }
}

public enum HealthValueMultiplier
{
    Down_6,
    Down_5,
    Down_4,
    Down_3,
    Down_2,
    Down_1,
    None,
    Up_1,
    Up_2,
    Up_3,
    Up_4,
    Up_5,
    Up_6
}

[Serializable]
public class HealthToSave
{
    public float hungerValue = new float();
    public HealthValueMultiplier hungerValueMultiplier_Check = new HealthValueMultiplier();
    public int healthValueMultiplier = new int();

    public float heatResistanceValue = new float();
    public HealthValueMultiplier heatResistanceValueMultiplier_Check = new HealthValueMultiplier();
    public int heatResistanceValueMultiplier = new int();

    public float thirstValue = new float();
    public HealthValueMultiplier thirstValueMultiplier_Check = new HealthValueMultiplier();
    public int thirstValueMultiplier = new int();

    public float mainHealthValue = new float();
    public HealthValueMultiplier mainHealthValueMultiplier_Check = new HealthValueMultiplier();
    public int mainHealthValueMultiplier = new int();
}