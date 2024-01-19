using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
    public GameObject TimeDisplay_Parent;

    public TextMeshProUGUI clockText;
    public TextMeshProUGUI dayText;
    public float currentTime = 0f;
    public float secondsPerMinute = 60f;
    public float timeSpeed = 1;
    public int day;


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
        currentTime += timeSpeed * Time.deltaTime;

        //Calculate hours and minutes
        int hours = Mathf.FloorToInt(currentTime / secondsPerMinute / 60f) % 12;
        int minutes = Mathf.FloorToInt(currentTime / secondsPerMinute) % 60;

        //Calculate days
        day = Mathf.FloorToInt(currentTime / 86400);
        dayText.text = "Day: " + day;

        //Make readable string
        string timeString = string.Format("{0:00}:{1:00} {2}", hours == 0 ? 12 : hours, minutes, hours < 12 ? "am" : "pm");


        //Update Timer
        if (clockText != null)
        {
            clockText.text = timeString;
        }
    }
}
