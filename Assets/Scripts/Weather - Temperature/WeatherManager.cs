using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("TemperatureDisplay")]
    public bool termostatDisplay_isUpgraded = true;
    public GameObject termostatDisplay_Parent;
    public Image termostat_Image;
    public Image pointer_Image;
    [SerializeField] Color idealColor;
    [SerializeField] Color hotColor;
    [SerializeField] Color coldColor;

    [Header("WeatherDisplay")]
    public List<WeatherType> weatherTypeDayList = new List<WeatherType>();

    public WeatherType weatherType = WeatherType.Sunny;
    public GameObject weatherDisplay_Parent;
    public bool weatherImageDisplay_Day1_isUpgraded = true;
    public bool weatherImageDisplay_Day2_isUpgraded = true;
    public bool weatherImageDisplay_Day3_isUpgraded = true;
    public bool weatherImageDisplay_Day4_isUpgraded = true;
    public bool weatherImageDisplay_Day5_isUpgraded = true;
    public GameObject weatherImageDisplay_Day1_Parent;
    public GameObject weatherImageDisplay_Day2_Parent;
    public GameObject weatherImageDisplay_Day3_Parent;
    public GameObject weatherImageDisplay_Day4_Parent;
    public GameObject weatherImageDisplay_Day5_Parent;
    public Image weatherImage_Day1;
    public Image weatherImage_Day2;
    public Image weatherImage_Day3;
    public Image weatherImage_Day4;
    public Image weatherImage_Day5;
    [SerializeField] Sprite weatherImage_Sunny;
    [SerializeField] Sprite weatherImage_Cloudy;
    [SerializeField] Sprite weatherImage_Windy;
    [SerializeField] Sprite weatherImage_Cold;

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

        SetTermostatDisplay();
    }


    //--------------------


    public void LoadData()
    {
        weatherTypeDayList = DataManager.Instance.weatherTypeDayList_Store;

        //At NewGame, make a list of 20 WeatherTypes, to prevent the list of getting emptied
        if (weatherTypeDayList.Count <= 0)
        {
            CalculateLastWeather(20);

            SetWeather();
        }

        //If not NewGame, set the parameters given by the list
        else
        {
            //Set the stats for today's weather

            //Display the weather for today and the next 4 days
            SetWeatherDisplay();

            SaveData();
        }
    }
    public void SaveData()
    {
        DataManager.Instance.weatherTypeDayList_Store = weatherTypeDayList;
    }
    public void SaveData(ref GameData gameData)
    {
        SaveData();

        print("Save_Weather");
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


    void SetTermostatDisplay()
    {
        if (termostatDisplay_isUpgraded)
        {
            //Set Pointer
            if (playerTemperature < 20)
            {
                pointer_Image.gameObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(0, 0, ((20 - playerTemperature) * 3)));
            }
            else if (playerTemperature > 20)
            {
                pointer_Image.gameObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(0, 0, ((20 - playerTemperature) * 3)));
            }
            else
            {
                pointer_Image.gameObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(0, 0, ((20 - 20) * 3)));
            }

            //Set Termostat Color
            if (playerTemperature < 15)
            {
                termostat_Image.color = coldColor;
            }
            else if (playerTemperature <= 25)
            {
                termostat_Image.color = idealColor;
            }
            else
            {
                termostat_Image.color = hotColor;
            }
        }
    }

    public void SetWeather()
    {
        if (TimeManager.Instance.newDay_Weather)
        {
            if (termostatDisplay_Parent.activeInHierarchy)
            {
                //Add new Weather Type
                CalculateLastWeather(1);

                //Remove yesterday's Weather Type
                weatherTypeDayList.RemoveAt(0);


                //-----


                //Set the stats for today's weather
                SetWeatherStats_Today();

                //Display the weather for today and the next 4 days
                SetWeatherDisplay();

                SaveData();
            }
        }
    }
    public void CalculateLastWeather(int daysForward)
    {
        int enumSize = Enum.GetValues(typeof(WeatherType)).Length;

        for (int i = 0; i < daysForward; i++)
        {
            weatherTypeDayList.Add((WeatherType)UnityEngine.Random.Range(0, enumSize));
        }
    }
    void SetWeatherStats_Today()
    {
        PlayerMovement.Instance.movementSpeedVarianceByWeather = 1f;

        HealthManager.Instance.hunger_SpeedMultiplier_ByWeather = 1f;
        HealthManager.Instance.heatResistance_SpeedMultiplier_ByWeather = 1f;
        HealthManager.Instance.thirst_SpeedMultiplier_ByWeather = 1f;
        HealthManager.Instance.mainHealth_SpeedMultiplier_ByWeather = 1f;

        //Sunny
        if (weatherTypeDayList[0] == WeatherType.Sunny)
        {
            //Set Weather Type
            weatherType = WeatherType.Sunny;

            //Set Min/Max temperatures
            minTemperature = 10;
            maxTemperature = 50;

            //Set Thirsty faster
            HealthManager.Instance.hunger_SpeedMultiplier_ByWeather = 1.25f;
        }

        //Cloudy
        else if (weatherTypeDayList[0] == WeatherType.Cloudy)
        {
            //Set Weather Type
            weatherType = WeatherType.Cloudy;

            //Set Min/Max temperatures
            minTemperature = 0;
            maxTemperature = 30;

            //Nothing
        }

        //Windy
        else if (weatherTypeDayList[0] == WeatherType.Windy)
        {
            //Set Weather Type
            weatherType = WeatherType.Windy;

            //Set Min/Max temperatures
            minTemperature = -10;
            maxTemperature = 40;

            //Walking slower
            PlayerMovement.Instance.movementSpeedVarianceByWeather = 0.75f;
        }

        //Cold
        else if (weatherTypeDayList[0] == WeatherType.Cold)
        {
            //Set Weather Type
            weatherType = WeatherType.Cold;

            //Set Min/Max temperatures
            minTemperature = -10;
            maxTemperature = 10;

            //Hungry faster
            HealthManager.Instance.hunger_SpeedMultiplier_ByWeather = 1.25f;
        }
    }
    void SetWeatherDisplay()
    {
        if (weatherTypeDayList.Count > 0 && weatherImageDisplay_Day1_isUpgraded)
        {
            weatherImage_Day1.sprite = GetWeatherImage(weatherTypeDayList[0]);
            weatherImageDisplay_Day1_Parent.SetActive(true);
        }
        else
        {
            weatherImageDisplay_Day1_Parent.SetActive(false);
        }

        if (weatherTypeDayList.Count > 1 && weatherImageDisplay_Day2_isUpgraded)
        {
            weatherImage_Day2.sprite = GetWeatherImage(weatherTypeDayList[1]);
            weatherImageDisplay_Day2_Parent.SetActive(true);
        }
        else
        {
            weatherImageDisplay_Day2_Parent.SetActive(false);
        }

        if (weatherTypeDayList.Count > 2 && weatherImageDisplay_Day3_isUpgraded)
        {
            weatherImage_Day3.sprite = GetWeatherImage(weatherTypeDayList[2]);
            weatherImageDisplay_Day3_Parent.SetActive(true);
        }
        else
        {
            weatherImageDisplay_Day3_Parent.SetActive(false);
        }

        if (weatherTypeDayList.Count > 3 && weatherImageDisplay_Day4_isUpgraded)
        {
            weatherImage_Day4.sprite = GetWeatherImage(weatherTypeDayList[3]);
            weatherImageDisplay_Day4_Parent.SetActive(true);
        }
        else
        {
            weatherImageDisplay_Day4_Parent.SetActive(false);
        }

        if (weatherTypeDayList.Count > 4 && weatherImageDisplay_Day5_isUpgraded)
        {
            weatherImage_Day5.sprite = GetWeatherImage(weatherTypeDayList[4]);
            weatherImageDisplay_Day5_Parent.SetActive(true);
        }
        else
        {
            weatherImageDisplay_Day5_Parent.SetActive(false);
        }
    }
    Sprite GetWeatherImage(WeatherType weatherType)
    {
        if (weatherType == WeatherType.Sunny)
            return weatherImage_Sunny;

        else if (weatherType == WeatherType.Cloudy)
            return weatherImage_Cloudy;

        else if (weatherType == WeatherType.Windy)
            return weatherImage_Windy;

        else if (weatherType == WeatherType.Cold)
            return weatherImage_Cold;

        return null;
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

public enum WeatherType
{
    Sunny,
    Cloudy,
    Windy,
    Cold
}