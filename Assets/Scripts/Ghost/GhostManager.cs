using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostManager : Singleton<GhostManager>
{
    [Header("Ghost Lists")]
    List<GhostStats> capturedGhostList = new List<GhostStats>();

    [Header("Ghost Object Pool")]
    public int ghostSpawnAmount = 1;

    [SerializeField] GameObject ghostPoolParent;
    [SerializeField] GameObject ghostPrefab;

    [SerializeField] Vector2 spawnPosition = new Vector2 (2, 3);
    public float despawnDistance = 8;

    List<GameObject> ghostPool = new List<GameObject>();

    [Header("Ghost Capturer")]
    public bool hasTarget;
    public GameObject targetGhostObject;
    public GhostCapturerStats ghostCapturerStats;
    public float leafRotationSpeed = 250;
    public float capturedRateSpeed = 0.1f;

    public Sprite ghostImage_Water;
    public Sprite ghostImage_Fire;
    public Sprite ghostImage_Earth;
    public Sprite ghostImage_Wind;
    public Sprite ghostImage_Electric;

    [Header("Materials")]
    public Material material_Empty;
    public Material material_Water;


    //--------------------


    void Start()
    {
        //Populate the ghostObjectPool
        for (int i = 0; i < ghostSpawnAmount; i++)
        {
            SpawnGhost();
        }

        leafRotationSpeed = 250;
        capturedRateSpeed = 0.1f;
    }


    //--------------------


    public void LoadData()
    {
        ghostCapturerStats = DataManager.Instance.ghostCapturerStats_Store;

        //Make sure there is 8 slots int the list
        if (ghostCapturerStats.activeGhostCapturerSlotList.Count != 8)
        {
            for (int i = 0; i < 8; i++)
            {
                ghostCapturerStats.activeGhostCapturerSlotList.Add(false);

                if (ghostCapturerStats.activeGhostCapturerSlotList.Count == 8)
                {
                    SaveData();

                    break;
                }
            }
        }
        if (ghostCapturerStats.ghostCapturedStats.Count != 8)
        {
            for (int i = 0; i < 8; i++)
            {
                GhostStats tempGhostStats = new GhostStats();

                tempGhostStats.ghostState = GhostStates.Idle;
                tempGhostStats.ghostAppearance = GhostAppearance.Type1;
                tempGhostStats.isBeard = false;
                tempGhostStats.ghostElement = GhostElement.None;
                tempGhostStats.elementFuel_Amount = 0;

                ghostCapturerStats.ghostCapturedStats.Add(tempGhostStats);

                if (ghostCapturerStats.activeGhostCapturerSlotList.Count == 8)
                {
                    SaveData();

                    break;
                }
            }
        }
    }
    public void SaveData()
    {
        DataManager.Instance.ghostCapturerStats_Store = ghostCapturerStats;
    }


    //--------------------


    public void AddGhostToCapturer(GhostStats ghostStats)
    {
        for (int i = 0; i < ghostCapturerStats.activeGhostCapturerSlotList.Count; i++)
        {
            if (!ghostCapturerStats.activeGhostCapturerSlotList[i])
            {
                ghostStats.ghostState = GhostStates.Idle;

                ghostCapturerStats.activeGhostCapturerSlotList[i] = true;
                ghostCapturerStats.ghostCapturedStats[i] = ghostStats;

                break;
            }
        }

        SaveData();
    }
    public void PlaceGhostInTank(int slot)
    {

    }
    public void RemoveGhostFromCapturer(int slot)
    {

    }


    //--------------------


    public GhostStats SetupGhost(GhostElement _ghostElement, float _elementFuel_Amount, GhostStates _ghostState)
    {
        GhostStats tempGhostStats = new GhostStats();

        tempGhostStats.ghostElement = _ghostElement;
        tempGhostStats.elementFuel_Amount = _elementFuel_Amount;
        tempGhostStats.ghostState = _ghostState;

        return tempGhostStats;
    }


    //--------------------


    public int SetGhostSpawnAmount()
    {
        switch (WeatherManager.Instance.weatherTypeDayList[0])
        {
            case WeatherType.Cold:
                ghostSpawnAmount = 10;
                break;
            case WeatherType.Cloudy:
                ghostSpawnAmount = 10; //(6)
                break;
            case WeatherType.Sunny:
                ghostSpawnAmount = 10; //(3)
                break;
            case WeatherType.Windy:
                ghostSpawnAmount = 10; //(0)
                break;

            default:
                break;
        }

        return ghostSpawnAmount;
    }


    //-------------------- Object Pooling of Ghosts


    public void SpawnGhost()
    {
        if (CountActiveGhosts() >= ghostSpawnAmount)
        {
            return;
        }

        //If GhostPool isn't used up
        #region
        foreach (GameObject obj in ghostPool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.transform.position = GetSpawnPosition();
                obj.GetComponent<InvisibleObject>().transparencyValue = 1;
                obj.GetComponent<InvisibleObject>().UpdateRenderList();
                obj.GetComponent<Ghost>().ghostStats.ghostState = GhostStates.Moving;
                obj.GetComponent<Ghost>().ghostStats.ghostElement = GhostElement.Water;
                obj.GetComponent<Ghost>().ghostStats.elementFuel_Amount = 100;

                SetRandomStyle(obj);

                obj.GetComponent<Ghost>().SetupGhost();
                obj.SetActive(true);

                return;
            }
        }
        #endregion

        //If GhostPool needs expansion
        #region
        //Spawn Ghost at new spawn position
        GameObject newObj = Instantiate(ghostPrefab, GetSpawnPosition(), Quaternion.identity);
        newObj.transform.parent = ghostPoolParent.transform;

        ghostPool.Add(newObj);
        ghostPool[ghostPool.Count - 1].GetComponent<InvisibleObject>().transparencyValue = 1;
        ghostPool[ghostPool.Count - 1].GetComponent<InvisibleObject>().UpdateRenderList();
        ghostPool[ghostPool.Count - 1].GetComponent<Ghost>().ghostStats.ghostState = GhostStates.Moving;
        ghostPool[ghostPool.Count - 1].GetComponent<Ghost>().ghostStats.ghostElement = GhostElement.Water;
        ghostPool[ghostPool.Count - 1].GetComponent<Ghost>().ghostStats.elementFuel_Amount = 100;

        SetRandomStyle(ghostPool[ghostPool.Count - 1]);

        ghostPool[ghostPool.Count - 1].GetComponent<Ghost>().SetupGhost();
        ghostPool[ghostPool.Count - 1].SetActive(true);
        #endregion
    }
    void SetRandomStyle(GameObject obj)
    {
        //Set Beard State
        if (UnityEngine.Random.value > 0.5f)
        {
            obj.GetComponent<Ghost>().ghostStats.isBeard = false;
        }
        else
        {
            obj.GetComponent<Ghost>().ghostStats.isBeard = true;
        }

        //Set Style State
        int randomStyle = UnityEngine.Random.Range(0, 2 + 1);
        if (randomStyle == 0)
        {
            obj.GetComponent<Ghost>().ghostStats.ghostAppearance = GhostAppearance.Type1;
        }
        else if (randomStyle == 1)
        {
            obj.GetComponent<Ghost>().ghostStats.ghostAppearance = GhostAppearance.Type2;
        }
        else if (randomStyle == 2)
        {
            obj.GetComponent<Ghost>().ghostStats.ghostAppearance = GhostAppearance.Type3;
        }

    }
    Vector3 GetSpawnPosition()
    {
        //Get a random direction vector
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere.normalized;

        //Get a random distance within a range
        float randomDistanceAwayFromPlayer = UnityEngine.Random.Range(MainManager.Instance.playerBody.transform.position.x + spawnPosition.x, MainManager.Instance.playerBody.transform.position.y + spawnPosition.y);
        float randomDistance_Y = UnityEngine.Random.Range(MainManager.Instance.playerBody.transform.position.y - 0.5f, MainManager.Instance.playerBody.transform.position.y + 1);

        Vector3 tempSpawnPosition = MainManager.Instance.playerBody.transform.position + randomDirection * randomDistanceAwayFromPlayer;
        tempSpawnPosition.y = MainManager.Instance.playerBody.transform.position.y + randomDistance_Y;

        return tempSpawnPosition;
    }
    int CountActiveGhosts()
    {
        int count = 0;
        foreach (GameObject obj in ghostPool)
        {
            if (obj.activeSelf)
            {
                count++;
            }
        }

        return count;
    }

    public void DespawnGhost(GameObject obj)
    {
        obj.GetComponent<InvisibleObject>().transparencyValue = 1;
        obj.GetComponent<InvisibleObject>().UpdateRenderList();
        obj.SetActive(false);

        //Spawn New Ghost to keep up with the set amount
        SpawnGhost();
    }
}

[Serializable]
public class GhostStats
{
    public GhostStates ghostState;
    public GhostAppearance ghostAppearance;
    public bool isBeard;

    public GhostElement ghostElement;
    public float elementFuel_Amount;
}

[Serializable]
public class GhostCapturerStats
{
    public List<bool> activeGhostCapturerSlotList = new List<bool>();

    public List<GhostStats> ghostCapturedStats;
}

public enum GhostElement
{
    None,

    Water,
    Fire,
    Stone,
    Wind,
    Poison,
    Power
}

public enum GhostStates
{
    Idle,

    Moving,

    Attacking,
    Fleeing,

    Captured
}
public enum GhostAppearance
{
    Type1,
    Type2,
    Type3
}