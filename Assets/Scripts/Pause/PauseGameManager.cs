using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGameManager : Singleton<PauseGameManager>
{
    bool gameIsPaused = false;


    //--------------------


    public void PauseGame()
    {
        gameIsPaused = true;
    }
    public void UnpauseGame()
    {
        gameIsPaused = false;
    }
    public bool GetPause()
    {
        return gameIsPaused;
    }
}
