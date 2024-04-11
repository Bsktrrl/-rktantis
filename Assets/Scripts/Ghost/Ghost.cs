using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    Animator anim;
    AudioSource audioSource_Ghost_GhostSounds;

    [Header("General")]
    public GhostStats ghostStats = new GhostStats();

    public InteracteableType interactableType;

    [Header("Stats")]
    public float movementSpeed = 2f;
    public float tempMovementSpeed;
    public float rotationSpeed = 4f;
    public float minDistance = 0.5f;

    [Header("Capture")]
    public bool isTargeted;
    public float capturedRate;

    [Header("Position")]
    public Vector3 spawnPos;
    public Vector3 targetPoint = Vector3.zero;

    [Header("Styles")]
    public GameObject beard;
    public List<GameObject> style1 = new List<GameObject>();
    public List<GameObject> style2 = new List<GameObject>();
    public List<GameObject> style3 = new List<GameObject>();


    //--------------------


    private void Start()
    {
        anim = GetComponent<Animator>();
        audioSource_Ghost_GhostSounds = GetComponent<AudioSource>();

        interactableType = InteracteableType.Ghost;

        movementSpeed = 2f;
        rotationSpeed = 4f;
        minDistance = 0.5f;

        spawnPos = transform.position;
        isTargeted = false;

        //SetGhostMovment();

        SetupGhost();
    }
    private void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            anim.SetFloat("MovementSpeed", movementSpeed);

            //Check for Despawn distance
            #region
            float distanceFromPlayer = Vector3.Distance(gameObject.transform.position, MainManager.Instance.playerBody.transform.position);
            if (distanceFromPlayer >= GhostManager.Instance.despawnDistance)
            {
                GhostManager.Instance.DespawnGhost(gameObject);
            }
            #endregion

            //Behavior Tree
            #region
            switch (ghostStats.ghostState)
            {
                //Get the action from the selected State
                case GhostStates.Idle:
                    Idle();
                    break;
                case GhostStates.Moving:
                    Move();
                    break;
                case GhostStates.Attacking:
                    Attack();
                    break;
                case GhostStates.Fleeing:
                    Flee();
                    break;
                case GhostStates.Captured:
                    Captured();
                    break;
            }
            #endregion
        }

        CapturedRate();

        UpdateSoundVolume();
        UpdateGhostSounds();
    }


    //--------------------


    public void SetupGhost()
    {
        SetGhostMovment();

        SetGhostAppearance();

        isTargeted = false;
        capturedRate = 0;
    }
    void SetGhostAppearance()
    {
        //Reset
        for (int i = 0; i < style1.Count; i++)
            style1[i].SetActive(false);
        for (int i = 0; i < style2.Count; i++)
            style2[i].SetActive(false);
        for (int i = 0; i < style3.Count; i++)
            style3[i].SetActive(false);

        //Set Beard
        if (ghostStats.isBeard)
        {
            beard.SetActive(true);
        }
        else
        {
            beard.SetActive(false);
        }

        //Set Style
        switch (ghostStats.ghostAppearance)
        {
            case GhostAppearance.Type1:
                for (int i = 0; i < style1.Count; i++)
                    style1[i].SetActive(true);
                break;
            case GhostAppearance.Type2:
                for (int i = 0; i < style2.Count; i++)
                    style2[i].SetActive(true);
                break;
            case GhostAppearance.Type3:
                for (int i = 0; i < style3.Count; i++)
                    style3[i].SetActive(true);
                break;

            default:
                break;
        }
    }

    void CapturedRate()
    {
        isTargeted = false;

        if (GhostManager.Instance.targetGhostObject == gameObject)
        {
            if (GhostManager.Instance.hasTarget)
            {
                isTargeted = true;
            }
        }
        

        if (isTargeted)
        {
            ghostStats.ghostState = GhostStates.Fleeing;

            //print("capturedRate - UP: " + capturedRate);
            capturedRate += (GhostManager.Instance.capturedRateSpeed * 1.5f) * Time.deltaTime;

            if (capturedRate > 1)
            {
                GhostCaptured();
            }
        }
        else
        {
            ghostStats.ghostState = GhostStates.Moving;

            if (capturedRate >= -0.1)
            {
                //print("capturedRate - DOWN: " + capturedRate);
                capturedRate -= GhostManager.Instance.capturedRateSpeed * Time.deltaTime;

                if (capturedRate < 0)
                {
                    capturedRate = 0;
                }
            }
        }
    }
    
    void UpdateSoundVolume()
    {
        audioSource_Ghost_GhostSounds.volume = SoundManager.Instance.sound_Creatures;
    }
    void UpdateGhostSounds()
    {
        if (!isTargeted)
        {
            //Play Roaming Sounds with a silent timer in between
            //...

            //SoundManager.Instance.Play_Ghost_GhostMood_Happy_Clip(audioSource_Ghost_GhostSounds);
        }
        else
        {
            if (capturedRate <= 0)
            {
                SoundManager.Instance.Play_Ghost_GhostStartsGettingTarget_Clip(audioSource_Ghost_GhostSounds);
            }
        }
    }


    //--------------------


    #region Idle
    void Idle()
    {

    }
    #endregion

    #region Movement
    void Move()
    {
        MoveTowardsTarget();
    }
    void MoveTowardsTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, tempMovementSpeed * Time.deltaTime);

        //Rotate towards the targetPoint
        Vector3 direction = (targetPoint - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        //Check if ghost is close to the targetPoint
        if (Vector3.Distance(transform.position, targetPoint) < minDistance)
        {
            SetGhostMovment();
        }
    }
    void SetGhostMovment()
    {
        // Generate random target points and add them to the list
        Quaternion randomPosition;

        if (targetPoint == Vector3.zero)
        {
            randomPosition = new Quaternion(
            Random.Range(spawnPos.x - 3f, spawnPos.x + 3f),
            Random.Range(spawnPos.y - 1f, spawnPos.y + 1f),
            Random.Range(spawnPos.z - 3f, spawnPos.z + 3f),

            Random.Range(movementSpeed - 0.5f, movementSpeed + 0.5f));
        }
        else
        {
            randomPosition = new Quaternion(
            Random.Range(targetPoint.x - 3f, targetPoint.x + 3f),
            Random.Range(targetPoint.y - 1f, targetPoint.y + 1f),
            Random.Range(targetPoint.z - 3f, targetPoint.z + 3f),

            Random.Range(movementSpeed - 0.5f, movementSpeed + 0.5f));
        }

        targetPoint = new Vector3(randomPosition.x, randomPosition.y, randomPosition.z);
        tempMovementSpeed = randomPosition.w;
    }
    #endregion

    #region Attack
    void Attack()
    {

    }
    #endregion

    #region Flee
    void Flee()
    {

    }
    #endregion

    #region Captured
    void Captured()
    {

    }
    #endregion


    //--------------------


    void GhostCaptured()
    {
        print("Ghost Captured");

        SoundManager.Instance.Play_Ghost_CaptureGhost_Clip();

        GhostManager.Instance.AddGhostToCapturer(ghostStats);

        GhostManager.Instance.DespawnGhost(gameObject);
    }
}
