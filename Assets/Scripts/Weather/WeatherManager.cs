using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeatherManager : Singleton<WeatherManager>
{
    public Light sun_Light;

    [SerializeField] GameObject temperatureDisplay_Parent;
    [SerializeField] TextMeshProUGUI temperatureDisplay;
    public float temperature;
    public float playerTemperature;

    public int maxTemperature = 50;
    public int minTemperature = -10;

    [Header("Temperature Resistances")]
    public float equippment;
    public float skillTree;
    public float waterCooler;


    //--------------------


    private void Update()
    {
        SetSunRotation();

        SetTemperature();
        SetPlayerTemperature(equippment, skillTree, waterCooler);
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
    void SetPlayerTemperature(float equippment, float skillTree, float waterCooler)
    {
        playerTemperature = temperature;
        playerTemperature += equippment;
        playerTemperature += skillTree;
        playerTemperature += waterCooler;
    }
    void SetTemperatureDisplay()
    {
        temperatureDisplay.text = temperature + "°C";
    }
}
