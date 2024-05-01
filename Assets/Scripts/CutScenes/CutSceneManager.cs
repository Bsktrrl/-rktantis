using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneManager : Singleton<CutSceneManager>
{
    public CutScenes cutScenes = new CutScenes();


    //--------------------


    public void LoadData()
    {
        cutScenes = DataManager.Instance.cutScenes_Store;
    }
    public void SaveData()
    {
        DataManager.Instance.cutScenes_Store = cutScenes;
    }


    //--------------------


    public void StartGhostCrystal_Scene_isOver()
    {
        print("Save CutSceneState");

        cutScenes.start_GhostCrystal_Scene = true;
        SaveData();
    }
}

[Serializable]
public class CutScenes
{
    public bool start_GhostCrystal_Scene;
}