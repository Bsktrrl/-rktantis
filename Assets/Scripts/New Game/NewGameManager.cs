using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameManager : Singleton<NewGameManager>
{
    public bool isNewGame;

    private void Awake()
    {
        isNewGame = true;
    }
}
