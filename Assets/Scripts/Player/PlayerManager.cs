using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    //Interactable Distance - Distance between player and raycast interactable
    public float InteractableDistance = 3.5f;

    //playerMovementStats_ToSave

    public void LoadData()
    {

    }
    public void SaveData()
    {

    }
    public void SaveData(ref GameData gameData)
    {
        SaveData();

        print("Save_PlayerManager");
    }
}
