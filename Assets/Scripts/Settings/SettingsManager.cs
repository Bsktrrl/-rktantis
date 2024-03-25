using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : Singleton<SettingsManager>
{
    [Header("Interactable Variables")]
    [SerializeField] SettingsValues settingsValues;

    [Header("UI Elements")]
    [SerializeField] Slider UI_Sound_Master;
    [SerializeField] TextMeshProUGUI UI_Sound_Master_Text;
    [Space(5)]
    [SerializeField] Slider UI_Sound_WorldSFX;
    [SerializeField] TextMeshProUGUI UI_Sound_WorldSFX_Text;
    [Space(5)]
    [SerializeField] Slider UI_Sound_MenuSFX;
    [SerializeField] TextMeshProUGUI UI_Sound_MenuSFX_Text;
    [Space(5)]
    [SerializeField] Slider UI_Sound_Music;
    [SerializeField] TextMeshProUGUI UI_Sound_Music_Text;
    [Space(5)]
    [SerializeField] Slider UI_Sound_Voice;
    [SerializeField] TextMeshProUGUI UI_Sound_Voice_Text;
    [Space(5)]
    [SerializeField] Slider UI_Sound_Weather;
    [SerializeField] TextMeshProUGUI UI_Sound_Weather_Text;
    [Space(5)]

    [SerializeField] Slider UI_Camera_FOV;
    [SerializeField] TextMeshProUGUI UI_Camera_FOV_Text;
    [Space(5)]
    [SerializeField] Slider UI_Camera_MouseSensitivity;
    [SerializeField] TextMeshProUGUI UI_Camera_MouseSensitivity_Text;

    bool firstSetup;
    bool setupIsFinished;


    //--------------------


    private void Start()
    {
        if (DataManager.Instance.settingsValues_Store.sound_Master <= 0
            && DataManager.Instance.settingsValues_Store.sound_WorldSFX <= 0
            && DataManager.Instance.settingsValues_Store.sound_MenuSFX <= 0
            && DataManager.Instance.settingsValues_Store.sound_Music <= 0
            && DataManager.Instance.settingsValues_Store.sound_Voice <= 0
            && DataManager.Instance.settingsValues_Store.sound_Weather <= 0

            && DataManager.Instance.settingsValues_Store.camera_FOV <= 0
            && DataManager.Instance.settingsValues_Store.camera_MouseSensitivity <= 0)
        {
            print("1000000. DataManager.Instance.settingsValues_Store == null");

            //Setup Basic values
            settingsValues.sound_Master = 1f;
            settingsValues.sound_WorldSFX = 1f;
            settingsValues.sound_MenuSFX = 1f;
            settingsValues.sound_Music = 1f;
            settingsValues.sound_Voice = 1f;
            settingsValues.sound_Weather = 1f;

            settingsValues.camera_FOV = 0.3f;
            settingsValues.camera_MouseSensitivity = 0.2f;

            //Setup all Slider
            UI_Sound_Master.value = 1f;
            UI_Sound_WorldSFX.value = 1f;
            UI_Sound_MenuSFX.value = 1f;
            UI_Sound_Music.value = 1f;
            UI_Sound_Voice.value = 1f;
            UI_Sound_Weather.value = 1f;

            UI_Camera_FOV.value = 0.3f;
            UI_Camera_MouseSensitivity.value = 0.2f;

            firstSetup = true;
            setupIsFinished = true;

            SaveData();
        }
    }


    //--------------------


    public void LoadData()
    {
        if (firstSetup) { return; }

        //Get Values from File
        settingsValues = DataManager.Instance.settingsValues_Store;

        //Setup all Slider
        UI_Sound_Master.value = settingsValues.sound_Master;
        UI_Sound_WorldSFX.value = settingsValues.sound_WorldSFX;
        UI_Sound_MenuSFX.value = settingsValues.sound_MenuSFX;
        UI_Sound_Music.value = settingsValues.sound_Music;
        UI_Sound_Voice.value = settingsValues.sound_Voice;
        UI_Sound_Weather.value = settingsValues.sound_Weather;

        UI_Camera_FOV.value = settingsValues.camera_FOV;
        UI_Camera_MouseSensitivity.value = settingsValues.camera_MouseSensitivity;

        SaveData();

        ChangeTextDisplay_Percent();

        setupIsFinished = true;
    }
    public void SaveData()
    {
        //Save all Values to File
        DataManager.Instance.settingsValues_Store = settingsValues;
    }
    public void SaveData(ref GameData gameData)
    {
        SaveData();

        print("Save_Settings");
    }


    //--------------------


    #region Getters - for SoundManager

    //Get Sound
    #region
    public float Get_Sound_Master()
    {
        return settingsValues.sound_Master;
    }
    public float Get_Sound_WorldSFX()
    {
        return settingsValues.sound_WorldSFX;
    }
    public float Get_Sound_MenuSFX()
    {
        return settingsValues.sound_MenuSFX;
    }
    public float Get_Sound_Music()
    {
        return settingsValues.sound_Music;
    }
    public float Get_Sound_Voice()
    {
        return settingsValues.sound_Voice;
    }
    public float Get_Sound_WeatherSFX()
    {
        return settingsValues.sound_Weather;
    }
    #endregion

    //Get Camera
    #region
    public float Get_Camera_FOV()
    {
        return settingsValues.camera_FOV;
    }
    public float Get_Camera_MouseSensitivity()
    {
        return settingsValues.camera_MouseSensitivity;
    }
    #endregion

    #endregion


    //--------------------


    #region Buttons / Interactions - Change Variables

    //Change slider values
    public void Slider_IsChanged()
    {
        if (!setupIsFinished) { return; }

        ChangeValues();

        ChangeComponents();

        ChangeTextDisplay_Percent();
    }

    #endregion


    //--------------------


    void ChangeValues()
    {
        SoundManager.Instance.Play_Inventory_ItemHover_Clip();

        //Sound
        settingsValues.sound_Master = UI_Sound_Master.value;
        settingsValues.sound_WorldSFX = UI_Sound_WorldSFX.value;
        settingsValues.sound_MenuSFX = UI_Sound_MenuSFX.value;
        settingsValues.sound_Music = UI_Sound_Music.value;
        settingsValues.sound_Voice = UI_Sound_Voice.value;
        settingsValues.sound_Weather = UI_Sound_Weather.value;

        //Camera
        settingsValues.camera_FOV = UI_Camera_FOV.value;
        settingsValues.camera_MouseSensitivity = UI_Camera_MouseSensitivity.value;

        SaveData();
    }
    void ChangeComponents()
    {
        MainManager.Instance.mainCamera.fieldOfView = (settingsValues.camera_FOV * 50) + 50;
        MouseMovement.Instance.mouseSensitivity = (settingsValues.camera_MouseSensitivity * 200) + 50;
    }
    void ChangeTextDisplay_Percent()
    {
        UI_Sound_Master_Text.text = (settingsValues.sound_Master * 100).ToString("F0") + "%";

        UI_Sound_WorldSFX_Text.text = (settingsValues.sound_WorldSFX * 100).ToString("F0") + "%";
        UI_Sound_MenuSFX_Text.text = (settingsValues.sound_MenuSFX * 100).ToString("F0") + "%";
        UI_Sound_Music_Text.text = (settingsValues.sound_Music * 100).ToString("F0") + "%";
        UI_Sound_Voice_Text.text = (settingsValues.sound_Voice * 100).ToString("F0") + "%";
        UI_Sound_Weather_Text.text = (settingsValues.sound_Weather * 100).ToString("F0") + "%";

        UI_Camera_FOV_Text.text = ((settingsValues.camera_FOV * 50) + 50).ToString("F0");
        UI_Camera_MouseSensitivity_Text.text = ((settingsValues.camera_MouseSensitivity * 200) + 50).ToString("F0");
    }
}

[Serializable]
public class SettingsValues
{
    //Sound
    public float sound_Master;
    public float sound_WorldSFX;
    public float sound_MenuSFX;
    public float sound_Music;
    public float sound_Voice;
    public float sound_Weather;

    //Camera
    public float camera_FOV;
    public float camera_MouseSensitivity;
}