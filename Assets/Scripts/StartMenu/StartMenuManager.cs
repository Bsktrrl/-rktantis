using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class StartMenuManager : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] GameObject menu_Main;
    [SerializeField] GameObject menu_Info;

    [SerializeField] GameObject continue_Button;

    [SerializeField] GameObject logo;

    [Header("InfoMenus")]
    [SerializeField] GameObject infoMenu_Controls;
    [SerializeField] GameObject infoMenu_Health;
    [SerializeField] GameObject infoMenu_Thermometer;
    [SerializeField] GameObject infoMenu_Weather;
    [SerializeField] GameObject infoMenu_Tablets;
    [SerializeField] GameObject infoMenu_BuildingHammers;


    //--------------------


    private void Start()
    {
        string fullPath = Path.Combine(Application.persistentDataPath, "data.game");

        menu_Info.SetActive(false);

        if (File.Exists(fullPath))
        {
            continue_Button.SetActive(true);
        }
        else
        {
            continue_Button.SetActive(false);
        }
    }


    //--------------------


    public void Continue_Button_isPressed()
    {
        StartMenuSounds.Instance.Play_Inventory_ButtonPressed_Clip();

        //DataPersistanceManager.instance.LoadGame();
        SceneManager.LoadScene("Landscape");
    }
    public void NewGame_Button_isPressed()
    {
        StartMenuSounds.Instance.Play_Inventory_ButtonPressed_Clip();

        string fullPath = Path.Combine(Application.persistentDataPath, "data.game");

        if (File.Exists(fullPath))
        {
            Debug.Log("File exists at path: " + fullPath);
            File.Delete(fullPath);
            Debug.Log("File deleted at path: " + fullPath);
        }
        else
        {
            Debug.Log("File does not exist at path: " + fullPath);
        }

        //DataPersistanceManager.instance.NewGame();
        SceneManager.LoadScene("Landscape");
    }
    public void Info_Button_isPressed()
    {
        StartMenuSounds.Instance.Play_Inventory_ButtonPressed_Clip();

        menu_Main.SetActive(false);
        menu_Info.SetActive(true);
        logo.SetActive(false);
    }
    public void Exit_Button_isPressed()
    {
        StartMenuSounds.Instance.Play_Inventory_ButtonPressed_Clip();

        Application.Quit();
    }

    public void Back_Button_isPressed()
    {
        StartMenuSounds.Instance.Play_Inventory_ButtonPressed_Clip();

        menu_Main.SetActive(true);
        menu_Info.SetActive(false);
        logo.SetActive(true);
    }


    //InfoButtons
    #region
    public void InfoMenu_Controls_Button_isPressed()
    {
        StartMenuSounds.Instance.Play_Inventory_ButtonPressed_Clip();

        ResetMenus();

        infoMenu_Controls.SetActive(true);
    }
    public void InfoMenu_Health_Button_isPressed()
    {
        StartMenuSounds.Instance.Play_Inventory_ButtonPressed_Clip();

        ResetMenus();

        infoMenu_Health.SetActive(true);
    }
    public void InfoMenu_Thermometer_Button_isPressed()
    {
        StartMenuSounds.Instance.Play_Inventory_ButtonPressed_Clip();

        ResetMenus();

        infoMenu_Thermometer.SetActive(true);
    }
    public void InfoMenu_Weather_Button_isPressed()
    {
        StartMenuSounds.Instance.Play_Inventory_ButtonPressed_Clip();

        ResetMenus();

        infoMenu_Weather.SetActive(true);
    }
    public void InfoMenu_Tables_Button_isPressed()
    {
        StartMenuSounds.Instance.Play_Inventory_ButtonPressed_Clip();

        ResetMenus();

        infoMenu_Tablets.SetActive(true);
    }
    public void InfoMenu_BuildingHammers_Button_isPressed()
    {
        StartMenuSounds.Instance.Play_Inventory_ButtonPressed_Clip();

        ResetMenus();

        infoMenu_BuildingHammers.SetActive(true);
    }

    void ResetMenus()
    {
        infoMenu_Controls.SetActive(false);
        infoMenu_Health.SetActive(false);
        infoMenu_Thermometer.SetActive(false);
        infoMenu_Weather.SetActive(false);
        infoMenu_Tablets.SetActive(false);
        infoMenu_BuildingHammers.SetActive(false);
    }
    #endregion
}
