using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostManager : Singleton<GhostManager>
{
    [Header("Ghost Lists")]
    //public List<GhostStats> capturedGhostList = new List<GhostStats>();

    [Header("Ghost Object Pool")]
    public int ghostSpawnAmount = 1;
    public float ghostMovementSpeed = 2f;

    [SerializeField] GameObject ghostPoolParent;
    [SerializeField] GameObject ghostPrefab;

    public Vector2 spawnPosition = new Vector2 (2, 3);
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
    }


    //--------------------


    public void LoadData()
    {
        ghostCapturerStats = DataManager.Instance.ghostCapturerStats_Store;

        //Make sure there is 8 slots int the list
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

                if (ghostCapturerStats.ghostCapturedStats.Count == 8)
                {
                    break;
                }
            }
        }

        if (ghostCapturerStats.slotsActivated == 0)
        {
            ghostCapturerStats.slotsActivated = 1;
        }

        SaveData();
    }
    public void SaveData()
    {
        DataManager.Instance.ghostCapturerStats_Store = ghostCapturerStats;
    }


    //--------------------


    public int SetGhostSpawnAmount()
    {
        switch (WeatherManager.Instance.weatherTypeDayList[0])
        {
            case WeatherType.Cold:
                ghostSpawnAmount = 1;
                break;
            case WeatherType.Cloudy:
                ghostSpawnAmount = 1; //(6)
                break;
            case WeatherType.Sunny:
                ghostSpawnAmount = 1; //(3)
                break;
            case WeatherType.Windy:
                ghostSpawnAmount = 1; //(0)
                break;

            default:
                break;
        }

        return ghostSpawnAmount;
    }


    //--------------------


    public void AddGhostToCapturer(GhostStats ghostStats)
    {
        for (int i = 0; i < ghostCapturerStats.slotsActivated; i++)
        {
            if (!ghostCapturerStats.ghostCapturedStats[i].isTaken)
            {
                ghostCapturerStats.ghostCapturedStats[i].isTaken = true;
                ghostCapturerStats.ghostCapturedStats[i].ghostState = GhostStates.Idle;

                ghostStats.isTaken = true;
                ghostStats.ghostState = GhostStates.Idle;

                //ghostCapturerStats.ghostCapturedStats[i] = ghostStats;

                break;
            }
        }

        //Stop GhostCapturer from being Active
        if (HotbarManager.Instance.selectedItem == Items.GhostCapturer)
        {
            if (HotbarManager.Instance.equippedItem.GetComponent<GhostCapturer>())
            {
                HotbarManager.Instance.equippedItem.GetComponent<GhostCapturer>().UpdateGhostCapturer();
                HotbarManager.Instance.equippedItem.GetComponent<GhostCapturer>().StopCapturing();
            }
        }
       
        SaveData();
    }
    public void RemoveGhostFromCapturer()
    {
        for (int i = ghostCapturerStats.ghostCapturedStats.Count - 1; i >= 0; i--)
        {
            if (ghostCapturerStats.ghostCapturedStats[i].isTaken)
            {
                ghostCapturerStats.ghostCapturedStats[i].isTaken = false;
                ghostCapturerStats.ghostCapturedStats[i].ghostState = GhostStates.Idle;
                ghostCapturerStats.ghostCapturedStats[i].ghostAppearance = GhostAppearance.None;
                ghostCapturerStats.ghostCapturedStats[i].isBeard = false;
                ghostCapturerStats.ghostCapturedStats[i].ghostElement = GhostElement.None;
                ghostCapturerStats.ghostCapturedStats[i].elementFuel_Amount = 0;

                break;
            }
        }

        //Stop GhostCapturer from being Active
        if (HotbarManager.Instance.selectedItem == Items.GhostCapturer)
        {
            if (HotbarManager.Instance.equippedItem.GetComponent<GhostCapturer>())
            {
                HotbarManager.Instance.equippedItem.GetComponent<GhostCapturer>().UpdateGhostCapturer();
                HotbarManager.Instance.equippedItem.GetComponent<GhostCapturer>().StopCapturing();
            }
        }

        SaveData();
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
                obj.GetComponent<Ghost>().spawnPos = obj.transform.position;
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
        ghostPool[ghostPool.Count - 1].GetComponent<Ghost>().spawnPos = ghostPool[ghostPool.Count - 1].transform.position;
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
    public Vector3 GetSpawnPosition()
    {
        //Get a random direction vector
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere.normalized;

        //Get a random distance within a range
        float randomDistanceAwayFromPlayer = UnityEngine.Random.Range(spawnPosition.x, spawnPosition.y);
        Vector3 tempSpawnPosition = MainManager.Instance.playerBody.transform.position + randomDirection * randomDistanceAwayFromPlayer;
        
        float randomDistance_Y = UnityEngine.Random.Range(MainManager.Instance.playerBody.transform.position.y - 0.5f, MainManager.Instance.playerBody.transform.position.y + 1);
        tempSpawnPosition.y = randomDistance_Y;

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


    //--------------------


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
    public bool isTaken;
    public GhostStates ghostState;
    public GhostAppearance ghostAppearance;
    public bool isBeard;

    public GhostElement ghostElement;
    public float elementFuel_Amount;
}

[Serializable]
public class GhostCapturerStats
{
    public int slotsActivated;
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
    Fleeing,

    Attacking,

    Tank
}
public enum GhostAppearance
{
    None,

    Type1,
    Type2,
    Type3
}