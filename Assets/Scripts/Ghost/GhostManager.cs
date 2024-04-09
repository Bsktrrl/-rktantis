using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostManager : Singleton<GhostManager>
{
    [Header("Ghost Lists")]
    List<GhostStats> capturedGhostList = new List<GhostStats>();
    List<GameObject> worldGhostList = new List<GameObject>();

    [Header("Ghost Object Pool")]
    [SerializeField] float ghostSpawnTime = 1;
    float tempSpawnTime = 0;

    [SerializeField] GameObject ghostPoolParent;
    [SerializeField] GameObject ghostPrefab; // The prefab to pool
    [SerializeField] int poolStartSize = 20; // Initial size of the object pool
    [SerializeField] Vector2 spawnPosition = new Vector2 (3, 5);
    public float despawnDistance = 25;

    List<GameObject> ghostPool = new List<GameObject>(); // List to store pooled objects


    //--------------------


    void Start()
    {
        //Populate the ghostObjectPool
        for (int i = 0; i < poolStartSize; i++)
        {
            SpawnGhost();
        }
    }

    private void Update()
    {
        tempSpawnTime += Time.deltaTime;

        if (tempSpawnTime >= ghostSpawnTime)
        {
            tempSpawnTime = 0;

            SpawnGhost();
        }
    }


    //--------------------


    public GhostStats SetupGhost(GhostElement _ghostElement, float _elementFuel_Amount, bool _isInTank)
    {
        GhostStats tempGhostStats = new GhostStats();

        tempGhostStats.ghostElement = _ghostElement;
        tempGhostStats.elementFuel_Amount = _elementFuel_Amount;
        tempGhostStats.isInTank = _isInTank;

        return tempGhostStats;
    }


    //-------------------- Object Pooling of Ghosts


    public void SpawnGhost()
    {
        //If GhostPool isn't used up
        foreach (GameObject obj in ghostPool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.transform.position = GetSpawnPosition();
                obj.GetComponent<InvisibleObject>().transparencyValue = 1;
                obj.GetComponent<InvisibleObject>().UpdateRenderList();
                obj.SetActive(true);

                break;
            }
        }

        #region If GhostPool needs expansion
        //Spawn Ghost at new spawn position
        GameObject newObj = Instantiate(ghostPrefab, GetSpawnPosition(), Quaternion.identity);
        newObj.transform.parent = ghostPoolParent.transform;

        //newObj.SetActive(true);
        ghostPool.Add(newObj);
        ghostPool[ghostPool.Count - 1].GetComponent<InvisibleObject>().transparencyValue = 1;
        ghostPool[ghostPool.Count - 1].GetComponent<InvisibleObject>().UpdateRenderList();
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

    public void ReturnGhostToPool(GameObject obj)
    {
        obj.GetComponent<InvisibleObject>().transparencyValue = 1;
        obj.GetComponent<InvisibleObject>().UpdateRenderList();
        obj.SetActive(false);
    }
}

[Serializable]
public class GhostStats
{
    public GhostElement ghostElement;
    public float elementFuel_Amount;
    public bool isInTank;
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