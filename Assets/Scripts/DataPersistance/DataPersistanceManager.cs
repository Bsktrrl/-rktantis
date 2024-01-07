using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistanceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] string fileName;

    GameData gameData;
    List<IDataPersistance> dataPersistanceObjects;
    FileDataHandler dataHandler;

    public static DataPersistanceManager instance { get; private set; }


    //--------------------


    private void Awake()
    {
        if (instance != null)
        {
            print("Found more than 1 Data Persistance Manager in the scene");
        }

        instance = this;
    }

    private void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistanceObjects = FindAllDataPersistenceObjects();

        LoadGame();
    }


    //--------------------


    public void NewGame()
    {
        this.gameData = new GameData();
    }
    public void LoadGame()
    {
        //Load any saved data from a file using the "data handler"
        this.gameData = dataHandler.Load();

        //If no data can be loaded, initialize to a new data
        if (this.gameData == null)
        {
            print("o data was found. Initializing data to defaults");
            NewGame();
        }

        //Push the loaded data to all other scripts that need it
        foreach (IDataPersistance dataPersistanceObj in dataPersistanceObjects)
        {
            dataPersistanceObj.LoadData(gameData);
        }
    }
    public void SaveGame()
    {
        //Pass the data to other scripts so thay can update it
        foreach (IDataPersistance dataPersistanceObj in dataPersistanceObjects)
        {
            dataPersistanceObj.SaveData(ref gameData);
        }

        //Save that data to a file using the "data handler"
        dataHandler.Save(gameData);
    }


    //--------------------


    private void OnApplicationQuit()
    {
        SaveGame();
    }

    List<IDataPersistance> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistance> dataPersistanceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistance>();

        return new List<IDataPersistance>(dataPersistanceObjects);
    }
}
