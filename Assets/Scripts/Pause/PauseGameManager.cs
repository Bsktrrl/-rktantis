using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseGameManager : Singleton<PauseGameManager>
{
    public bool gameIsPaused = false;

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


    //--------------------


    private void Start()
    {
        PlayerButtonManager.pauseMenu_isPressed += PauseGame;

        UI_Pause.SetActive(false);
        saved_Text.SetActive(false);
    }
    private void Update()
    {
        if (!DataManager.Instance.hasLoaded) { return; }
        if (MainManager.Instance.gameStates == GameStates.GameOver) { return; }

        if (gameIsPaused)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                BackToGame_Button_isPressed();
            }
        }
    }


    //--------------------


    public void PauseGame()
    {
        gameIsPaused = true;

        UI_Pause.SetActive(true);

        UI_Info.SetActive(false);
        UI_hotbar.SetActive(false);
        UI_HealthParameters.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
    }
    public void UnpauseGame()
    {
        print("UnpauseGame");

        gameIsPaused = false;

        UI_Pause.SetActive(false);

        UI_Info.SetActive(true);
        UI_hotbar.SetActive(true);
        UI_HealthParameters.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;

    }
    public bool GetPause()
    {
        return gameIsPaused;
    }


    //--------------------


    //Buttons
    #region
    public void BackToPauseMenu_Button_isPressed()
    {
        menu_Info.SetActive(false);
        menu_Settings.SetActive(false);

        buttons_Panel.SetActive(true);
    }

    public void BackToGame_Button_isPressed()
    {
        UnpauseGame();
    }
    public void Info_Button_isPressed()
    {
        buttons_Panel.SetActive(false);
        menu_Info.SetActive(true);
    }
    public void Settings_Button_isPressed()
    {
        buttons_Panel.SetActive(false);
        menu_Settings.SetActive(true);
    }
    public void Save_Button_isPressed()
    {
        DataManager.Instance.SaveData(ref DataManager.Instance.gameData);

        StartCoroutine(savedText());
    }
    public void MainMenu_Button_isPressed()
    {
        Application.Quit(); //Temporary
    }

    IEnumerator savedText()
    {
        yield return new WaitForSeconds(0.5f);
        saved_Text.SetActive(true);

        yield return new WaitForSeconds(3);
        saved_Text.SetActive(false);
    }
    #endregion
}
