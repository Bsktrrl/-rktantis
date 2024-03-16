using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.LightAnchor;

public class WeatherManager : Singleton<WeatherManager>
{
    public Light sun_Light;

    public GameObject temperatureDisplay_Parent;
    [SerializeField] TextMeshProUGUI temperatureDisplay;
    [SerializeField] TextMeshProUGUI playerTemperatureDisplay;
    public float currentWorldTemperature;
    public float playerTemperature;

    public int maxTemperature = 50;
    public int minTemperature = -10;
    public int idealTemperature = 20;

    [Header("Temperature Resistances")]
    public float coverValue;
    public float temperatureFruit;
    public float equippment;
    public float skillTree;
    public float waterCooler;

    [Header("Cover Values")]
    public float wood_Cover = 10;
    public float stone_Cover = 20;
    public float iron_Cover = 30;

    [Header("temperatureFruit")]
    public List<TemperatureFruit> temperatureFruitList = new List<TemperatureFruit>();

    RaycastHit hit;


    //--------------------


    private void Start()
    {
        sun_Light.transform.SetPositionAndRotation(new Vector3(270, 0, 0), transform.rotation);
    }
    private void Update()
    {
        SetSunRotation();
        if (temperatureFruitList.Count > 0)
        {
            SetTemperatureFromFruit(temperatureFruitList);
        }
        
        CheckIfPlayerIsInTheCoverageOfBuildingBlock();
        SetTemperature();
        SetPlayerTemperature(coverValue, temperatureFruit);
        SetTemperatureDisplay(temperatureDisplay, playerTemperatureDisplay);
    }


    //--------------------


    void SetSunRotation()
    {
        float tempValue = (TimeManager.Instance.GetTime() / 240) - 90;

        sun_Light.transform.SetLocalPositionAndRotation(transform.position, Quaternion.Euler(tempValue, 0, 0));
    }

    void SetTemperature()
    {
        currentWorldTemperature = 50;

        //Check if there is night
        if (TimeManager.Instance.GetTime() <= TimeManager.Instance.GetHour(6))
        {
            currentWorldTemperature = minTemperature;
            return;
        }
        else if (TimeManager.Instance.GetTime() >= TimeManager.Instance.GetHour(18))
        {
            currentWorldTemperature = minTemperature;
            return;
        }

        //Check temperature during the day
        if (TimeManager.Instance.GetTime() > TimeManager.Instance.GetHour(6)
            && TimeManager.Instance.GetTime() < TimeManager.Instance.GetHour(18))
        {
            float timeSpan = TimeManager.Instance.GetHour(6) - TimeManager.Instance.GetHour(12);
            float timeAwayFromMidthday = Mathf.Abs(TimeManager.Instance.GetTime() - TimeManager.Instance.GetHour(12));
            float maxTemperatureRange = maxTemperature - minTemperature;

            float timeSlize = timeSpan / maxTemperatureRange;

            float calculatingTemperature = timeAwayFromMidthday / timeSlize;

            currentWorldTemperature = Mathf.FloorToInt(maxTemperature + calculatingTemperature);
        }
    }
    void SetPlayerTemperature(float coverValue, float temperatureFruit)
    {
        //Set Temperature for BuildingBlock Coverage (doesn't affect temperature under idealTemperature)
        float coverResistance = coverValue;

        //BuildingBlock Cover Calculation (perform this first to get the playerTemperature. Add other buffs later
        #region
        if (currentWorldTemperature >= idealTemperature)
        {
            if ((currentWorldTemperature + coverResistance) >= idealTemperature)
            {
                playerTemperature = currentWorldTemperature + coverResistance;
            }
            else
            {
                playerTemperature = idealTemperature;
            }
        }
        else
        {
            if ((currentWorldTemperature + coverResistance) <= idealTemperature)
            {
                playerTemperature = currentWorldTemperature + coverResistance;
            }
            else
            {
                playerTemperature = idealTemperature;
            }
        }
        #endregion


        //Add Heat/Freeze Fruit influence to the playerTemperature
        #region
        float fruitResistance = temperatureFruit;

        if (playerTemperature >= idealTemperature)
        {
            if ((playerTemperature + fruitResistance) >= idealTemperature)
            {
                if (fruitResistance <= 0)
                {
                    playerTemperature = currentWorldTemperature + fruitResistance;
                }
            }
            else
            {
                playerTemperature = idealTemperature;
            }
        }
        else
        {
            if ((playerTemperature + fruitResistance) <= idealTemperature)
            {
                if (fruitResistance >= 0)
                {
                    playerTemperature = currentWorldTemperature + fruitResistance;
                }
            }
            else
            {
                playerTemperature = idealTemperature;
            }
        }
        #endregion

        //Add further variables to the playerTemperature
        #region

        #endregion
    }
    public void SetTemperatureDisplay(TextMeshProUGUI temperatureDisplay, TextMeshProUGUI playerTemperatureDisplay)
    {
        temperatureDisplay.text = currentWorldTemperature + "°C";
        playerTemperatureDisplay.text = playerTemperature + "°C";
    }


    //--------------------


    void CheckIfPlayerIsInTheCoverageOfBuildingBlock()
    {
        float maxRange = 100;

        Vector3 sunDirection = sun_Light.transform.forward;

        //Raycast UP if temperature is under ideal
        if (currentWorldTemperature < idealTemperature)
        {
            //it's warm
            SetCoverValue(Vector3.up, maxRange, false);
        }

        //Raycast the sun if temperature is over or equal to ideal
        else
        {
            //It's hot
            SetCoverValue(-sunDirection, maxRange, true);
        }
    }
    void SetCoverValue(Vector3 raycastDirection, float maxRange, bool isWarm)
    {
        if (Physics.Raycast(MainManager.Instance.player.transform.position, raycastDirection, out hit, maxRange))
        {
            if (hit.transform.CompareTag("BuildingBlock"))
            {
                if (hit.transform.gameObject.GetComponent<BuildingBlock>())
                {
                    //If standing in a shadow from a Wood Block
                    if (hit.transform.gameObject.GetComponent<BuildingBlock>().buidingBlock_Parent.GetComponent<BuildingBlock_Parent>().buildingMaterial == BuildingMaterial.Wood)
                    {
                        if (isWarm)
                        {
                            coverValue = -wood_Cover;
                        }
                        else
                        {
                            coverValue = wood_Cover;
                        }
                    }

                    //If standing in a shadow from a Stone Block
                    else if (hit.transform.gameObject.GetComponent<BuildingBlock>().buidingBlock_Parent.GetComponent<BuildingBlock_Parent>().buildingMaterial == BuildingMaterial.Stone)
                    {
                        if (isWarm)
                        {
                            coverValue = -stone_Cover;
                        }
                        else
                        {
                            coverValue = stone_Cover;
                        }
                    }

                    //If standing in a shadow from a Iron Block
                    else if (hit.transform.gameObject.GetComponent<BuildingBlock>().buidingBlock_Parent.GetComponent<BuildingBlock_Parent>().buildingMaterial == BuildingMaterial.Iron)
                    {
                        if (isWarm)
                        {
                            coverValue = -iron_Cover;
                        }
                        else
                        {
                            coverValue = iron_Cover;
                        }
                    }
                    else
                    {
                        coverValue = 0;
                    }
                }
                else
                {
                    coverValue = 0;
                }
            }
            else
            {
                coverValue = 0;
            }
        }
        else
        {
            coverValue = 0;
        }
    }
    
    void SetTemperatureFromFruit(List<TemperatureFruit> temperatureFruitList)
    {
        int totalTemperature = 0;

        //Add All temperatures together
        for (int i = 0; i < temperatureFruitList.Count; i++)
        {
            totalTemperature += temperatureFruitList[i].value;
        }

        temperatureFruit = totalTemperature;


        //-----


        //Update the timer of each temperature buff
        for (int i = 0; i < temperatureFruitList.Count; i++)
        {
            temperatureFruitList[i].duration -= Time.deltaTime;
        }

        //Check if any Element should be removed
        for (int i = temperatureFruitList.Count - 1; i >= 0; i--)
        {
            if (temperatureFruitList[i].duration <= 0)
            {
                temperatureFruitList.RemoveAt(i);

                SoundManager.Instance.Play_Buff_Deactivated_Clip();
            }
        }

        //Reset temperatureFruit if there isn't any active fruit buffs
        if (temperatureFruitList.Count <= 0)
        {
            temperatureFruit = 0;
        }
    }
}

[Serializable]
public class TemperatureFruit
{
    public int value;
    public float duration;
}