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

    public float hunger_Speed = 2e-05f;
    public float hunger_SpeedMultiplier_ByWeather = 1f;

    public float heatResistance_Speed = 2e-05f;
    public float heatResistance_SpeedMultiplier_ByWeather = 1f;

    public float thirst_Speed = 2e-05f;
    public float thirst_SpeedMultiplier_ByWeather = 1f;

    public float mainHealth_Speed = 8e-06f;
    public float mainHealth_SpeedMultiplier_ByWeather = 1f;


    [Header("Colors")]
    [SerializeField] Color fullColor;
    [SerializeField] Color progressColor;
    [SerializeField] Color emptyColor;

    [Header("Hunger Parameter")]
    [SerializeField] Image hunger_Image;
    public Image hungerIcon_Image;
    public float hungerValue = 1;
    [SerializeField] List<GameObject> hungerValueMultiplier_Image = new List<GameObject>();
    public HealthValueMultiplier hungerValueMultiplier_Check = HealthValueMultiplier.None;
    public int healthValueMultiplier;

    [Header("HeatResistance Parameter")]
    [SerializeField] Image heatResistance_Image;
    public Image heatResistanceIcon_Image;
    public float heatResistanceValue = 1;
    [SerializeField] List<GameObject> heatResistanceValueMultiplier_Image = new List<GameObject>();
    public HealthValueMultiplier heatResistanceValueMultiplier_Check = HealthValueMultiplier.None;
    public int heatResistanceValueMultiplier;

    [Header("Thirst Parameter")]
    [SerializeField] Image thirst_Image;
    public Image thirstIcon_Image;
    public float thirstValue = 1;
    [SerializeField] List<GameObject> thirstValueMultiplier_Image = new List<GameObject>();
    public HealthValueMultiplier thirstValueMultiplier_Check = HealthValueMultiplier.None;
    public int thirstValueMultiplier;

    [Header("MainHealth Parameter")]
    [SerializeField] Image mainHealth_Image;
    public Image mainHealthIcon_Image;
    public float mainHealthValue = 1;
    [SerializeField] List<GameObject> mainHealthValueMultiplier_Image = new List<GameObject>();
    public HealthValueMultiplier mainHealthValueMultiplier_Check = HealthValueMultiplier.None;
    public int mainHealthValueMultiplier;

    public int mainHealthCounter = 1;
    #endregion
    

    //--------------------


    private void Start()
    {
        health_Parent.SetActive(true);

        //Set HealthParameters
        hunger_Speed = 2e-05f;
        heatResistance_Speed = 2e-05f;
        thirst_Speed = 2e-05f;
        mainHealth_Speed = 8e-06f;

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
    }
    private void Update()
    {
        if (!DataManager.Instance.hasLoaded) { return; }
        if (PauseGameManager.Instance.GetPause()) { return; }
        if (MainManager.Instance.gameStates == GameStates.GameOver) { return; }

        SetHealthValues();
        SetHealthDisplay(hunger_Image, hungerValueMultiplier_Image,
                         heatResistance_Image, heatResistanceValueMultiplier_Image,
                         thirst_Image, thirstValueMultiplier_Image,
                         mainHealth_Image);

        SetPlayerHeatResistance();

        SaveData();
    }


    //--------------------


    public void LoadData()
    {
        HealthToSave tempHealth = DataManager.Instance.health_Store;

        //print("1. tempHealth: Thirst:" + tempHealth.thirstValue + " | Hunger" + tempHealth.hungerValue + " | Resist" + tempHealth.heatResistanceValue + " | Main" + tempHealth.mainHealthValue);
        //print("2. DataManager.Instance.health_Store: Thirst:" + DataManager.Instance.health_Store.thirstValue + " | Hunger" + DataManager.Instance.health_Store.hungerValue + " | Resist" + DataManager.Instance.health_Store.heatResistanceValue + " | Main" + DataManager.Instance.health_Store.mainHealthValue);

        if (tempHealth.thirstValue <= 0
            && tempHealth.hungerValue <= 0 
            && tempHealth.heatResistanceValue <= 0 
            && tempHealth.mainHealthValue <= 0)
        {
            //print("3. Reset");

            hungerValue = 1;
            hungerValueMultiplier_Check = HealthValueMultiplier.Down_1;
            healthValueMultiplier = -1;

            heatResistanceValue = 1;
            heatResistanceValueMultiplier_Check = HealthValueMultiplier.Down_1;
            heatResistanceValueMultiplier = -1;

            thirstValue = 1;
            thirstValueMultiplier_Check = HealthValueMultiplier.Down_1;
            thirstValueMultiplier = -1;

            mainHealthValue = 1;
            mainHealthValueMultiplier_Check = HealthValueMultiplier.None;
            mainHealthValueMultiplier = 5;

            SaveData();
        }
        else
        {
            //print("4. Not Reset");

            hungerValue = tempHealth.hungerValue;
            hungerValueMultiplier_Check = HealthValueMultiplier.Down_1;
            healthValueMultiplier = tempHealth.healthValueMultiplier;

            heatResistanceValue = tempHealth.heatResistanceValue;
            heatResistanceValueMultiplier_Check = tempHealth.heatResistanceValueMultiplier_Check;
            heatResistanceValueMultiplier = tempHealth.heatResistanceValueMultiplier;

            thirstValue = tempHealth.thirstValue;
            thirstValueMultiplier_Check = HealthValueMultiplier.Down_1;
            thirstValueMultiplier = tempHealth.thirstValueMultiplier;

            mainHealthValue = tempHealth.mainHealthValue;
            mainHealthValueMultiplier_Check = HealthValueMultiplier.None;
            mainHealthValueMultiplier = tempHealth.mainHealthValueMultiplier;
        }
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
        healthValueMultiplier = MultiplierCheck((int)hungerValueMultiplier_Check + PlayerManager.Instance.hungerTemp);
        heatResistanceValueMultiplier = MultiplierCheck((int)heatResistanceValueMultiplier_Check + PlayerManager.Instance.heatresistanceTemp);
        thirstValueMultiplier = MultiplierCheck((int)thirstValueMultiplier_Check + PlayerManager.Instance.thirstTemp);
        #endregion

        //Speed Check
        #region
        hungerValue += (hunger_Speed * hunger_SpeedMultiplier_ByWeather * healthValueMultiplier);
        if (hungerValue <= 0)
        {
            hungerValue = 0;

            hungerIcon_Image.color = emptyColor;
        }
        else if (hungerValue >= 1)
        {
            hungerValue = 1;

            hungerIcon_Image.color = fullColor;
        }
        else
        {
            hungerIcon_Image.color = progressColor;
        }

        heatResistanceValue += (heatResistance_Speed * heatResistance_SpeedMultiplier_ByWeather * heatResistanceValueMultiplier);
        if (heatResistanceValue <= 0)
        {
            heatResistanceValue = 0;

            heatResistanceIcon_Image.color = emptyColor;
        }
        else if (heatResistanceValue >= 1)
        {
            heatResistanceValue = 1;

            heatResistanceIcon_Image.color = fullColor;
        }
        else
        {
            heatResistanceIcon_Image.color = progressColor;
        }

        thirstValue += (thirst_Speed * thirst_SpeedMultiplier_ByWeather * thirstValueMultiplier);
        if (thirstValue <= 0)
        {
            thirstValue = 0;

            thirstIcon_Image.color = emptyColor;
        }
        else if (thirstValue >= 1)
        {
            thirstValue = 1;

            thirstIcon_Image.color = fullColor;
        }
        else
        {
            thirstIcon_Image.color = progressColor;
        }
        #endregion

        //Set Main Health Parameter
        #region
        mainHealthCounter = 0;

        if (hungerValue <= 0)
            mainHealthCounter++;

        if (heatResistanceValue <= 0)
            mainHealthCounter++;

        if (thirstValue <= 0)
            mainHealthCounter++;

        if (mainHealthCounter <= 0)
        {
            mainHealthValue += Mathf.Abs(mainHealth_Speed);
        }
        else
        {
            mainHealthValue += -Mathf.Abs(mainHealth_Speed * mainHealth_SpeedMultiplier_ByWeather * mainHealthValueMultiplier * mainHealthCounter);
        }
       
        if (mainHealthValue <= 0.12f)
        {
            mainHealthValue = 0;

            mainHealthIcon_Image.color = emptyColor;

            //Set GameOver
            if (MainManager.Instance.gameStates != GameStates.GameOver)
            {
                GameOverManager.Instance.TriggerGameOver();
            }
        }
        else if (mainHealthValue >= 1)
        {
            mainHealthValue = 1;

            mainHealthIcon_Image.color = fullColor;
        }
        else
        {
            mainHealthIcon_Image.color = emptyColor;
        }
        #endregion

        //Set Main Health Arrow Display
        SetMainHealthArrowDisplay(mainHealthValueMultiplier_Image);
    }
    public void ResetPlayerHealthValues()
    {
        hungerValue = 1;
        heatResistanceValue = 1;
        thirstValue = 1;

        mainHealthValue = 1;
    }

    int MultiplierCheck(int multiplier_Check)
    {
        if (multiplier_Check <= 0)
        {
            return -6;
        }
        else if (multiplier_Check == 1)
        {
            return -5;
        }
        else if (multiplier_Check == 2)
        {
            return -4;
        }
        else if (multiplier_Check == 3)
        {
            return -3;
        }
        else if (multiplier_Check == 4)
        {
            return -2;
        }
        else if (multiplier_Check == 5)
        {
            return -1;
        }
        else if (multiplier_Check == 6)
        {
            return 0;
        }
        else if (multiplier_Check == 7)
        {
            return 1;
        }
        else if (multiplier_Check == 8)
        {
            return 2;
        }
        else if (multiplier_Check == 9)
        {
            return 3;
        }
        else if (multiplier_Check == 10)
        {
            return 4;
        }
        else if (multiplier_Check == 11)
        {
            return 5;
        }
        else if (multiplier_Check >= 12)
        {
            return 6;
        }
        else
        {
            return 0;
        }

        //switch (multiplier_Check)
        //{
        //    case HealthValueMultiplier.Down_6:
                
        //    case HealthValueMultiplier.Down_5:
        //        return -5;
        //    case HealthValueMultiplier.Down_4:
        //        return -4;
        //    case HealthValueMultiplier.Down_3:
        //        return -3;
        //    case HealthValueMultiplier.Down_2:
        //        return -2;
        //    case HealthValueMultiplier.Down_1:
        //        return -1;
        //    case HealthValueMultiplier.None:
        //        return 0;
        //    case HealthValueMultiplier.Up_1:
        //        return 1;
        //    case HealthValueMultiplier.Up_2:
        //        return 2;
        //    case HealthValueMultiplier.Up_3:
        //        return 3;
        //    case HealthValueMultiplier.Up_4:
        //        return 4;
        //    case HealthValueMultiplier.Up_5:
        //        return 5;
        //    case HealthValueMultiplier.Up_6:
        //        return 6;

        //    default:
        //        return 0;
        //}
    }
    HealthValueMultiplier SetCheckValue(int value)
    {
        if (value <= -6)
        {
            return HealthValueMultiplier.Down_6;
        }
        else if (value == -5)
        {
            return HealthValueMultiplier.Down_5;
        }
        else if (value == -4)
        {
            return HealthValueMultiplier.Down_4;
        }
        else if (value == -3)
        {
            return HealthValueMultiplier.Down_3;
        }
        else if (value == -2)
        {
            return HealthValueMultiplier.Down_2;
        }
        else if (value == -1)
        {
            return HealthValueMultiplier.Down_1;
        }
        else if (value == 0)
        {
            return HealthValueMultiplier.None;
        }
        else if (value == 1)
        {
            return HealthValueMultiplier.Up_1;
        }
        else if (value == 2)
        {
            return HealthValueMultiplier.Up_2;
        }
        else if (value == 3)
        {
            return HealthValueMultiplier.Up_3;
        }
        else if (value == 4)
        {
            return HealthValueMultiplier.Up_4;
        }
        else if (value == 5)
        {
            return HealthValueMultiplier.Up_5;
        }
        else if (value >= 6)
        {
            return HealthValueMultiplier.Up_6;
        }
        else
        {
            return HealthValueMultiplier.None;
        }
    }

    public void SetMainHealthArrowDisplay(List<GameObject> mainHealthValueMultiplier_Image)
    {
        #region Main Health Parameter
        for (int i = 0; i < mainHealthValueMultiplier_Image.Count; i++)
        {
            mainHealthValueMultiplier_Image[i].SetActive(false);
        }

        if (mainHealthCounter <= 0 && mainHealthValue >= 1)
        {
            mainHealthValueMultiplier_Image[3].SetActive(true);
        }
        else if (mainHealthCounter <= 0)
        {
            mainHealthValueMultiplier_Image[4].SetActive(true);
        }
        else if (mainHealthCounter == 1)
        {
            mainHealthValueMultiplier_Image[2].SetActive(true);
        }
        else if (mainHealthCounter == 2)
        {
            mainHealthValueMultiplier_Image[1].SetActive(true);
            mainHealthValueMultiplier_Image[2].SetActive(true);
        }
        else if (mainHealthCounter >= 3)
        {
            mainHealthValueMultiplier_Image[0].SetActive(true);
            mainHealthValueMultiplier_Image[1].SetActive(true);
            mainHealthValueMultiplier_Image[2].SetActive(true);
        }
        #endregion
    }

    public void SetHealthDisplay(Image hunger_Image, List<GameObject> hungerValueMultiplier_Image,
                                 Image heatResistance_Image, List<GameObject> heatResistanceValueMultiplier_Image,
                                 Image thirst_Image, List<GameObject> thirstValueMultiplier_Image,
                                 Image mainHealth_Image)
    {
        //Set Values
        hunger_Image.fillAmount = hungerValue;
        heatResistance_Image.fillAmount = heatResistanceValue;
        thirst_Image.fillAmount = thirstValue;
        mainHealth_Image.fillAmount = mainHealthValue;

        //Set active arrows
        ArrowSetActive(healthValueMultiplier, hungerValueMultiplier_Image, hungerValue);
        ArrowSetActive(heatResistanceValueMultiplier, heatResistanceValueMultiplier_Image, heatResistanceValue);
        ArrowSetActive(thirstValueMultiplier, thirstValueMultiplier_Image, thirstValue);
    }
    void ArrowSetActive(int multiplier, List<GameObject> imagesList, float value)
    {
        //Unactivate all arrows
        for (int i = 0; i < imagesList.Count; i++)
        {
            imagesList[i].SetActive(false);
        }

        if (value >= 1)
        {
            imagesList[3].SetActive(true);
        }
        else
        {
            if (multiplier <= -6)
            {
                imagesList[0].SetActive(true);
                imagesList[1].SetActive(true);
                imagesList[2].SetActive(true);
            }
            else if (multiplier == -5)
            {
                imagesList[0].SetActive(true);
                imagesList[1].SetActive(true);
                imagesList[2].SetActive(true);
            }
            else if (multiplier == -4)
            {
                imagesList[1].SetActive(true);
                imagesList[2].SetActive(true);
            }
            else if (multiplier == -3)
            {
                imagesList[1].SetActive(true);
                imagesList[2].SetActive(true);
            }
            else if (multiplier == -2)
            {
                imagesList[2].SetActive(true);
            }
            else if (multiplier == -1)
            {
                imagesList[2].SetActive(true);
            }
            else if (multiplier == 0)
            {
            }
            else if (multiplier == 1)
            {
                imagesList[4].SetActive(true);
            }
            else if (multiplier == 2)
            {
                imagesList[4].SetActive(true);
            }
            else if (multiplier == 3)
            {
                imagesList[4].SetActive(true);
                imagesList[5].SetActive(true);
            }
            else if (multiplier == 4)
            {
                imagesList[4].SetActive(true);
                imagesList[5].SetActive(true);
            }
            else if (multiplier == 5)
            {
                imagesList[4].SetActive(true);
                imagesList[5].SetActive(true);
                imagesList[6].SetActive(true);
            }
            else if (multiplier >= 6)
            {
                imagesList[4].SetActive(true);
                imagesList[5].SetActive(true);
                imagesList[6].SetActive(true);
            }


            //switch (multiplier)
            //{
            //    case -6:
            //        break;
            //    case -5:
            //        break;
            //    case HealthValueMultiplier.Down_4:
            //        break;
            //    case HealthValueMultiplier.Down_3:
            //        break;
            //    case HealthValueMultiplier.Down_2:
            //        break;
            //    case HealthValueMultiplier.Down_1:
            //        break;
            //    case HealthValueMultiplier.None:
            //        break;
            //    case HealthValueMultiplier.Up_1:
            //        break;
            //    case HealthValueMultiplier.Up_2:
            //        break;
            //    case HealthValueMultiplier.Up_3:
            //        break;
            //    case HealthValueMultiplier.Up_4:
            //        break;
            //    case HealthValueMultiplier.Up_5:
            //        break;
            //    case HealthValueMultiplier.Up_6:
            //        break;

            //    default:
            //        break;
            //}
        }
    }


    //--------------------


    void SetPlayerHeatResistance()
    {
        switch (WeatherManager.Instance.playerTemperature)
        {
            //High Temperature (26 - 50)
            case >= 50:
                heatResistanceValueMultiplier_Check = HealthValueMultiplier.Down_6;
                break;
            case >= 45:
                heatResistanceValueMultiplier_Check = HealthValueMultiplier.Down_5;
                break;
            case >= 40:
                heatResistanceValueMultiplier_Check = HealthValueMultiplier.Down_4;
                break;
            case >= 35:
                heatResistanceValueMultiplier_Check = HealthValueMultiplier.Down_3;
                break;
            case >= 30:
                heatResistanceValueMultiplier_Check = HealthValueMultiplier.Down_2;
                break;
            case > 25:
                heatResistanceValueMultiplier_Check = HealthValueMultiplier.Down_1;
                break;

            //None (25)
            case 25:
                heatResistanceValueMultiplier_Check = HealthValueMultiplier.None;
                break;

            //Up (16 - 24)
            case 24:
                heatResistanceValueMultiplier_Check = HealthValueMultiplier.Up_1;
                break;
            case 23:
                heatResistanceValueMultiplier_Check = HealthValueMultiplier.Up_2;
                break;
            case 22:
                heatResistanceValueMultiplier_Check = HealthValueMultiplier.Up_3;
                break;
            case 21:
                heatResistanceValueMultiplier_Check = HealthValueMultiplier.Up_4;
                break;
            case 20:
                heatResistanceValueMultiplier_Check = HealthValueMultiplier.Up_5;
                break;
            case 19:
                heatResistanceValueMultiplier_Check = HealthValueMultiplier.Up_4;
                break;
            case 18:
                heatResistanceValueMultiplier_Check = HealthValueMultiplier.Up_3;
                break;
            case 17:
                heatResistanceValueMultiplier_Check = HealthValueMultiplier.Up_2;
                break;
            case 16:
                heatResistanceValueMultiplier_Check = HealthValueMultiplier.Up_1;
                break;

            //None(15)
            case 15:
                heatResistanceValueMultiplier_Check = HealthValueMultiplier.None;
                break;

            //Low Temperature (-15 - 14)
            case >= 10:
                heatResistanceValueMultiplier_Check = HealthValueMultiplier.Down_1;
                break;
            case >= 5:
                heatResistanceValueMultiplier_Check = HealthValueMultiplier.Down_2;
                break;
            case >= 0:
                heatResistanceValueMultiplier_Check = HealthValueMultiplier.Down_3;
                break;
            case >= -5:
                heatResistanceValueMultiplier_Check = HealthValueMultiplier.Down_4;
                break;
            case >= -10:
                heatResistanceValueMultiplier_Check = HealthValueMultiplier.Down_5;
                break;
            case < -10:
                heatResistanceValueMultiplier_Check = HealthValueMultiplier.Down_6;
                break;

            default:
                break;
        }
    }


    //--------------------


    public void SetPlayerHealthSpeed()
    {
        hunger_Speed = 2e-05f * (1 + (PerkManager.Instance.perkValues.healthResistance_Increase_Percentage / 100));
        heatResistance_Speed = 2e-05f * (1 + (PerkManager.Instance.perkValues.healthResistance_Increase_Percentage / 100));
        thirst_Speed = 2e-05f * (1 + (PerkManager.Instance.perkValues.healthResistance_Increase_Percentage / 100));
        mainHealth_Speed = 8e-06f * (1 + (PerkManager.Instance.perkValues.healthResistance_Increase_Percentage / 100));
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