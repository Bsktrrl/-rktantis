using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeatherManager : Singleton<WeatherManager>
{
    public Light sun_Light;

    [SerializeField] GameObject temperatureDisplay_Parent;
    [SerializeField] TextMeshProUGUI temperatureDisplay;
    [SerializeField] TextMeshProUGUI playerTemperatureDisplay;
    public float temperature;
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
    public float wood_Cover = 5;
    public float stone_Cover = 10;
    public float iron_Cover = 15;

    RaycastHit hit;


    //--------------------


    private void Update()
    {
        SetSunRotation();

        CheckIfPlayerIsInTheCoverageOfBuildingBlock();
        SetTemperature();
        SetPlayerTemperature(coverValue);
        SetTemperatureDisplay();
    }


    //--------------------


    void SetSunRotation()
    {
        float tempValue = (TimeManager.Instance.GetTime() / 240) - 90;

        sun_Light.transform.SetLocalPositionAndRotation(transform.position, Quaternion.Euler(tempValue, 0, 0));
    }

    void SetTemperature()
    {
        temperature = 50;

        //Check if there is night
        if (TimeManager.Instance.GetTime() <= TimeManager.Instance.GetHour(6))
        {
            temperature = minTemperature;
            return;
        }
        else if (TimeManager.Instance.GetTime() >= TimeManager.Instance.GetHour(18))
        {
            temperature = minTemperature;
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

            temperature = Mathf.FloorToInt(maxTemperature + calculatingTemperature);
        }
    }
    void SetPlayerTemperature(float coverValue)
    {
        //Set Temperature for BuildingBlock Coverage (doesn't affect temperature under idealTemperature)
        #region
        float temperatureResistance = coverValue;
        float awayFromIdealtemperature = Mathf.Abs(temperature - idealTemperature);

        if (temperature >= idealTemperature)
        {
            if (temperatureResistance < awayFromIdealtemperature)
            {
                playerTemperature = temperature - temperatureResistance;
            }
            else if (temperatureResistance >= awayFromIdealtemperature)
            {
                playerTemperature = idealTemperature;
            }
        }
        else
        {
            playerTemperature = temperature;
        }
        //else if (temperature < idealTemperature)
        //{
        //    if (temperatureResistance < awayFromIdealtemperature)
        //    {
        //        playerTemperature = temperature + temperatureResistance;
        //    }
        //    else if (temperatureResistance >= awayFromIdealtemperature)
        //    {
        //        playerTemperature = idealTemperature;
        //    }
        //}
        #endregion
    }
    void SetTemperatureDisplay()
    {
        temperatureDisplay.text = temperature + "°C";
        playerTemperatureDisplay.text = playerTemperature + "°C";
    }


    //--------------------


    void CheckIfPlayerIsInTheCoverageOfBuildingBlock()
    {
        //Check if there is daylight
        if (TimeManager.Instance.timeOfDay == TimeOfDay.Night) { return; }


        //-----


        float maxRange = 100;

        Vector3 sunDirection = sun_Light.transform.forward;

        if (Physics.Raycast(MainManager.Instance.player.transform.position, -sunDirection, out hit, maxRange))
        {
            if (hit.transform.CompareTag("BuildingBlock"))
            {
                if (hit.transform.gameObject.GetComponent<BuildingBlock>())
                {
                    //If standing in a shadow from a Wood Block
                    if (hit.transform.gameObject.GetComponent<BuildingBlock>().buidingBlock_Parent.GetComponent<BuildingBlock_Parent>().buildingMaterial == BuildingMaterial.Wood)
                    {
                        print("Stands behind a Wood Block");

                        coverValue = wood_Cover;
                    }

                    //If standing in a shadow from a Stone Block
                    else if (hit.transform.gameObject.GetComponent<BuildingBlock>().buidingBlock_Parent.GetComponent<BuildingBlock_Parent>().buildingMaterial == BuildingMaterial.Stone)
                    {
                        print("Stands behind a Stone Block");

                        coverValue = stone_Cover;
                    }

                    //If standing in a shadow from a Iron Block
                    else if (hit.transform.gameObject.GetComponent<BuildingBlock>().buidingBlock_Parent.GetComponent<BuildingBlock_Parent>().buildingMaterial == BuildingMaterial.Iron)
                    {
                        print("Stands behind a Iron Block");

                        coverValue = iron_Cover;
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
