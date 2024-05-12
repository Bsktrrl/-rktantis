using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseGameManager : Singleton<PauseGameManager>
{
    public bool gameIsPaused = false;

    public PauseMenuStates pauseMenuStates;

    [Header("Main UI")]
    [SerializeField] GameObject UI_Pause;

    [SerializeField] GameObject UI_Info;
    [SerializeField] GameObject UI_hotbar;
    [SerializeField] GameObject UI_HealthParameters;

    [SerializeField] GameObject saved_Text;

    [SerializeField] GameObject buttons_Panel;

    [Header("Menus")]
    [SerializeField] GameObject menu_Info;
    [SerializeField] GameObject menu_Settings;

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
        pauseMenuStates = PauseMenuStates.None;

        UI_Pause.SetActive(false);
        saved_Text.SetActive(false);

        menu_Info.SetActive(false);
        menu_Settings.SetActive(false);

        infoMenu_Controls.SetActive(true);
    }
    private void Update()
    {
        if (!DataManager.Instance.hasLoaded) { return; }
        if (MainManager.Instance.gameStates == GameStates.GameOver) { return; }

        if (Input.GetKeyDown(KeyCode.Escape) && (MainManager.Instance.menuStates == MenuStates.None || MainManager.Instance.menuStates == MenuStates.PauseMenu))
        {
            switch (pauseMenuStates)
            {
                case PauseMenuStates.None:
                    PauseGame();
                    break;
                case PauseMenuStates.PauseMenu:
                    BackToGame_Button_isPressed();
                    break;
                case PauseMenuStates.SettingsMenu:
                    BackToPauseMenu_Button_isPressed();
                    break;
                case PauseMenuStates.InfoMenu:
                    BackToPauseMenu_Button_isPressed();
                    break;

                default:
                    break;
            }
        }
    }


    //--------------------


    public void PauseGame() //Menu
    {
        SoundManager.Instance.Play_JournalPage_SelectingJournalPage_Clip();

        pauseMenuStates = PauseMenuStates.PauseMenu;
        MainManager.Instance.menuStates = MenuStates.PauseMenu;

        gameIsPaused = true;

        UI_Pause.SetActive(true);
        buttons_Panel.SetActive(true);

        UI_Info.SetActive(false);
        UI_hotbar.SetActive(false);
        UI_HealthParameters.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
    }
    public void PauseGame(bool noMenuOpen) //Not Menu
    {
        gameIsPaused = true;
    }
    public void UnpauseGame()
    {
        pauseMenuStates = PauseMenuStates.None;
        MainManager.Instance.menuStates = MenuStates.None;

        UI_Pause.SetActive(false);

        UI_Info.SetActive(true);
        UI_hotbar.SetActive(true);
        UI_HealthParameters.SetActive(true);

        gameIsPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
    }
    public bool GetPause()
    {
        return gameIsPaused;
    }


    //--------------------


    //Buttons
    #region
    public void BackToGame_Button_isPressed()
    {
        SoundManager.Instance.Play_JournalPage_SelectingJournalPage_Clip();

        pauseMenuStates = PauseMenuStates.None;

        UnpauseGame();

        Cursor.lockState = CursorLockMode.Locked;
    }
    public void Info_Button_isPressed()
    {
        SoundManager.Instance.Play_JournalPage_SelectingJournalPage_Clip();

        pauseMenuStates = PauseMenuStates.InfoMenu;

        buttons_Panel.SetActive(false);
        menu_Info.SetActive(true);
    }
    public void Settings_Button_isPressed()
    {
        SoundManager.Instance.Play_JournalPage_SelectingJournalPage_Clip();

        pauseMenuStates = PauseMenuStates.SettingsMenu;

        buttons_Panel.SetActive(false);
        menu_Settings.SetActive(true);
    }
    public void Save_Button_isPressed()
    {
        SoundManager.Instance.Play_JournalPage_SelectingJournalPage_Clip();

        DataManager.Instance.SaveData(ref DataManager.Instance.gameData);

        StartCoroutine(savedText());
    }
    public void MainMenu_Button_isPressed()
    {
        SoundManager.Instance.Play_JournalPage_SelectingJournalPage_Clip();

        Application.Quit();

        //SceneManager.LoadScene("StartMenu");
    }

    public void BackToPauseMenu_Button_isPressed()
    {
        SoundManager.Instance.Play_JournalPage_SelectingJournalPage_Clip();

        pauseMenuStates = PauseMenuStates.PauseMenu;

        menu_Info.SetActive(false);
        menu_Settings.SetActive(false);

        buttons_Panel.SetActive(true);
    }

    IEnumerator savedText()
    {
        yield return new WaitForSeconds(0.5f);
        saved_Text.SetActive(true);

        yield return new WaitForSeconds(3);
        saved_Text.SetActive(false);
    }
    #endregion

    //InfoButtons
    #region
    public void InfoMenu_Controls_Button_isPressed()
    {
        SoundManager.Instance.Play_JournalPage_SelectingJournalPage_Clip();

        ResetMenus();

        infoMenu_Controls.SetActive(true);
    }
    public void InfoMenu_Health_Button_isPressed()
    {
        SoundManager.Instance.Play_JournalPage_SelectingJournalPage_Clip();

        ResetMenus();

        infoMenu_Health.SetActive(true);
    }
    public void InfoMenu_Thermometer_Button_isPressed()
    {
        SoundManager.Instance.Play_JournalPage_SelectingJournalPage_Clip();

        ResetMenus();

        infoMenu_Thermometer.SetActive(true);
    }
    public void InfoMenu_Weather_Button_isPressed()
    {
        SoundManager.Instance.Play_JournalPage_SelectingJournalPage_Clip();

        ResetMenus();

        infoMenu_Weather.SetActive(true);
    }
    public void InfoMenu_Tables_Button_isPressed()
    {
        SoundManager.Instance.Play_JournalPage_SelectingJournalPage_Clip();

        ResetMenus();

        infoMenu_Tablets.SetActive(true);
    }
    public void InfoMenu_BuildingHammers_Button_isPressed()
    {
        SoundManager.Instance.Play_JournalPage_SelectingJournalPage_Clip();

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

public enum PauseMenuStates
{
    None,

    PauseMenu,

    SettingsMenu,
    InfoMenu
}