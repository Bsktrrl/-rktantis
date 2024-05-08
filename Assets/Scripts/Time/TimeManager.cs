using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
    public GameObject TimeDisplay_Parent;

    public TimeOfDay timeOfDay;

    public TextMeshProUGUI clockText;
    public TextMeshProUGUI dayText;
    float secondsPerMinute = 60f;
    public float currentTime;
    public float timeSpeed = 1;
    public int day;
    int loops;
    bool timestamp;
    public bool newDay_Weather;


    //--------------------


    private void Start()
    {
        //TimeDisplay_Parent.SetActive(true);
    }
    private void Update()
    {
        RunClock();
        SetTimeOfDay();

        SaveData();
    }


    //--------------------


    public void LoadData()
    {
        currentTime = DataManager.Instance.currentTime_Store;
        day = DataManager.Instance.day_Store;
    }
    void SaveData()
    {
        DataManager.Instance.currentTime_Store = currentTime;
        DataManager.Instance.day_Store = day;
    }
    public void SaveData(ref GameData gameData)
    {
        SaveData();

        print("Save_Time");
    }


    //--------------------


    void RunClock()
    {
        //Add time
        Addtime();

        //Update Timer
        SetTimerDisplay(clockText, dayText);
    }
    void Addtime()
    {
        if (currentTime >= (float.MaxValue - 10000))
        {
            currentTime = 0;
            loops = day;
        }
        else
        {
            if (timeOfDay == TimeOfDay.Night)
            {
                currentTime += timeSpeed * Time.deltaTime * 1.75f;
            }
            else
            {
                currentTime += timeSpeed * Time.deltaTime;
            }
        }
    }
    public void SetTimerDisplay(TextMeshProUGUI clockText, TextMeshProUGUI dayText)
    {
        //Calculate hours and minutes
        int hours = Mathf.FloorToInt(currentTime / secondsPerMinute / 60f) % 12;
        int minutes = Mathf.FloorToInt(currentTime / secondsPerMinute) % 60;

        //Set New Weather
        if (hours <= 0 && minutes <= 0 && !newDay_Weather)
        {
            if (!newDay_Weather)
            {
                WeatherManager.Instance.SetWeather();
            }

            newDay_Weather = true;
        }
        else if (hours <= 0 && minutes >= 1 && newDay_Weather)
        {
            newDay_Weather = false;
        }

        //Calculate days
        day = Mathf.FloorToInt(currentTime / 86400) + loops;
        dayText.text = "Day: " + day;

        string timeString = "";
        if (hours < 10)
            timeString += "0" + hours + ":";
        else
            timeString += hours + ":";
        if (minutes < 10)
            timeString += "0" + minutes;
        else
            timeString += minutes;

        if (clockText != null)
        {
            clockText.text = timeString;

            float tempStamp = currentTime;
            tempStamp -= (day * 86400);
            if (tempStamp <= 86400 / 2)
            {
                timestamp = true;
            }
            else
            {
                timestamp = false;
            }

            if (timestamp)
            {
                clockText.text += " am";
            }
            else
            {
                clockText.text += " pm";
            }
        }
    }

    void SetTimeOfDay()
    {
        if (GetTime() <= GetHour(5)) //Night
        {
            timeOfDay = TimeOfDay.Night;

            if (!SoundManager.Instance.audioSource_WeatherSound_Night.isPlaying)
            {
                SoundManager.Instance.Play_Weather_Night_Clip();
            }
        }
        else if (GetTime() <= GetHour(7))
        {
            timeOfDay = TimeOfDay.Morning;
            SoundManager.Instance.Stop_Weather_Night_Clip();
        }
        else if (GetTime() <= GetHour(17))
        {
            timeOfDay = TimeOfDay.Day;
        }
        else if (GetTime() <= GetHour(19))
        {
            timeOfDay = TimeOfDay.Evening;

            if (!SoundManager.Instance.audioSource_WeatherSound_Night.isPlaying)
            {
                SoundManager.Instance.Play_Weather_Night_Clip();
            }
        }
        else
        {
            timeOfDay = TimeOfDay.Night;

            if (!SoundManager.Instance.audioSource_WeatherSound_Night.isPlaying)
            {
                SoundManager.Instance.Play_Weather_Night_Clip();
            }
        }
    }
    

    public float GetTime()
    {
        if (currentTime >= 86400)
        {
            float tempTime = currentTime;

            return tempTime -= (86400 * day);
        }
        else
        {
            return currentTime;
        }
    }
    public int GetHour(int hour)
    {
        return 3600 * hour;
    }
}

public enum TimeOfDay
{
    Morning,
    Day,
    Evening,
    Night
}