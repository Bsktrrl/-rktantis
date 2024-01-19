using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
    public GameObject TimeDisplay_Parent;

    public TextMeshProUGUI clockText;
    public TextMeshProUGUI dayText;
    float secondsPerMinute = 60f;
    public float currentTime;
    public float timeSpeed = 1;
    public int day;
    int loops;
    bool timestamp;


    //--------------------


    private void Start()
    {
        TimeDisplay_Parent.SetActive(true);
    }
    private void Update()
    {
        RunClock();

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


    //--------------------


    void RunClock()
    {
        //Add time
        if (currentTime >= (float.MaxValue - 10000))
        {
            currentTime = 0;
            loops = day;
        }
        else
        {
            currentTime += timeSpeed * Time.deltaTime;
        }

        //Calculate hours and minutes
        int hours = Mathf.FloorToInt(currentTime / secondsPerMinute / 60f) % 12;
        int minutes = Mathf.FloorToInt(currentTime / secondsPerMinute) % 60;

        //Calculate days
        day = Mathf.FloorToInt(currentTime / 86400) + loops;
        dayText.text = "Day: " + day;

        //Make readable string
        //string timeString = string.Format("{0:D2}:{1:D2} {2}", hours == -1 ? 11 : hours, minutes/*, hours <= 11 ? "am" : "pm"*/);
        string timeString = "";
        if (hours < 10)
            timeString += "0" + hours + ":";
        else
            timeString += hours + ":";
        if (minutes < 10)
            timeString += "0" + minutes;
        else
            timeString += minutes;

        //Update Timer
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
}
