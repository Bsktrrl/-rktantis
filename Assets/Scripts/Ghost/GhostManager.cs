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

    [SerializeField] Vector2 spawnPosition = new Vector2 (20, 25);
    public float despawnDistance = 35;

    List<GameObject> ghostPool = new List<GameObject>();


    [Header("Ghost Capturer")]
    public GhostCapturerStats ghostCapturerStats;

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
    }
    public void SaveData()
    {
        DataManager.Instance.ghostCapturerStats_Store = ghostCapturerStats;
    }


    //--------------------


    public void AddGhostToCapturer(int slot, GhostStats ghostStats)
    {
        ghostCapturerStats.activeGhostCapturerSlotList[slot] = true;
        ghostCapturerStats.ghostCapturedStats[slot] = ghostStats;
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
                ghostSpawnAmount = 6;
                break;
            case WeatherType.Sunny:
                ghostSpawnAmount = 3;
                break;
            case WeatherType.Windy:
                ghostSpawnAmount = 0;
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
                obj.GetComponent<Ghost>().ghostState = GhostStates.Moving;

                //Set Beard State
                if (UnityEngine.Random.value > 0.5f)
                {
                    obj.GetComponent<Ghost>().isBeard = false;
                }
                else
                {
                    obj.GetComponent<Ghost>().isBeard = true;
                }

                //Set Style State
                int randomStyle = UnityEngine.Random.Range(0, 2 + 1);
                if (randomStyle == 0)
                {
                    obj.GetComponent<Ghost>().ghostAppearance = GhostAppearance.Type1;
                }
                else if (randomStyle == 1)
                {
                    obj.GetComponent<Ghost>().ghostAppearance = GhostAppearance.Type2;
                }
                else if (randomStyle == 2)
                {
                    obj.GetComponent<Ghost>().ghostAppearance = GhostAppearance.Type3;
                }

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

        //newObj.SetActive(true);
        ghostPool.Add(newObj);
        ghostPool[ghostPool.Count - 1].GetComponent<InvisibleObject>().transparencyValue = 1;
        ghostPool[ghostPool.Count - 1].GetComponent<InvisibleObject>().UpdateRenderList();
        ghostPool[ghostPool.Count - 1].GetComponent<Ghost>().ghostState = GhostStates.Moving;
        ghostPool[ghostPool.Count - 1].GetComponent<Ghost>().SetupGhost();

        ghostPool[ghostPool.Count - 1].SetActive(true);
        #endregion
    }
    Vector3 GetSpawnPosition()
    {
        //Get a random direction vector
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere.normalized;

        //Get a random distance within a range
        float randomDistanceAwayFromPlayer = UnityEngine.Random.Range(spawnPosition.x, spawnPosition.y);
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