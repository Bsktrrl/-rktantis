using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : Singleton<WeatherManager>
{
    public Light sun_Light;


    //--------------------


    private void Update()
    {
        SetSunRotation();
    }


    //--------------------


    void SetSunRotation()
    {
        float tempValue = (TimeManager.Instance.GetTime() / 240) - 90;

        sun_Light.transform.SetLocalPositionAndRotation(transform.position, Quaternion.Euler(tempValue, 0, 0));
    }
}
