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
    public float equippment;
    public float skillTree;
    public float waterCooler;

    [Header("Cover Values")]
    public float wood_Cover = 10;
    public float stone_Cover = 20;
    public float iron_Cover = 30;

    RaycastHit hit;


    //--------------------


    private void Start()
    {
        sun_Light.transform.SetPositionAndRotation(new Vector3(270, 0, 0), transform.rotation);
    }
    private void Update()
    {
        SetSunRotation();

        CheckIfPlayerIsInTheCoverageOfBuildingBlock();
        SetTemperature();
        SetPlayerTemperature(coverValue);
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
    void SetPlayerTemperature(float coverValue)
    {
        //Set Temperature for BuildingBlock Coverage (doesn't affect temperature under idealTemperature)
        float temperatureResistance = coverValue;
        float awayFromIdealtemperature = Mathf.Abs(currentWorldTemperature - idealTemperature);

        //BuildingBlock Cover Calculation (perform this first to get the playerTemperature. Add other buffs later
        #region
        if (currentWorldTemperature >= idealTemperature)
        {
            if ((currentWorldTemperature + temperatureResistance) >= idealTemperature)
            {
                playerTemperature = currentWorldTemperature + temperatureResistance;
            }
            else
            {
                playerTemperature = idealTemperature;
            }
        }
        else
        {
            if ((currentWorldTemperature + temperatureResistance) <= idealTemperature)
            {
                playerTemperature = currentWorldTemperature + temperatureResistance;
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
            RaycastCoverage(Vector3.up, maxRange, false);
        }

        //Raycast the sun if temperature is over or equal to ideal
        else
        {
            //It's hot
            RaycastCoverage(-sunDirection, maxRange, true);
        }
    }
    void RaycastCoverage(Vector3 raycastDirection, float maxRange, bool isWarm)
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
}
