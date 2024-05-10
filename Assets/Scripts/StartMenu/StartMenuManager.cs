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


    //--------------------


    private void Start()
    {
        string fullPath = Path.Combine(Application.persistentDataPath, "data.game");

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
    }
}
