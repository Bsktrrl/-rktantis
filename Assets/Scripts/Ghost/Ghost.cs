using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class Ghost : MonoBehaviour
{
    Animator anim;
    [SerializeField] AudioSource audioSource_Ghost_GhostSounds;

    [Header("General")]
    public GhostStats ghostStats = new GhostStats();

    public InteracteableType interactableType;

    [Header("Stats")]
    public float tempMovementSpeed;
    public float rotationSpeed = 4f;
    public float minDistance = 0.5f;

    [Header("Capture")]
    public bool isTargeted;
    public float capturedRate;
    bool targetedOnce;
    bool targetedOnceOnce;

    [Header("Position")]
    public Vector3 spawnPos;
    public Vector3 targetPoint = Vector3.zero;
    public Vector3 fleeTargetPoint = Vector3.zero;

    [Header("Styles")]
    public GameObject beard;
    public List<GameObject> style1 = new List<GameObject>();
    public List<GameObject> style2 = new List<GameObject>();
    public List<GameObject> style3 = new List<GameObject>();

    [Header("Raycast")]
    [SerializeField] bool isHittingTerrain;
    public LayerMask ghostLayerMask;
    public LayerMask groundLayerMask;

    Ray ray;
    RaycastHit hit;

    float fleeDirectionTimer;
    float fleeDirectionTimerCheck = 2;
    float fleeSpeedTimer;
    float fleeSpeedTimerCheck = 2;
    float fleeTempSpeedMultiplier = 1;
    float fleeDirectionKeepTimer;


    [Header("In GhostTank")]
    float tankAnimationTimer;


    //--------------------


    private void Start()
    {
        anim = GetComponent<Animator>();

        interactableType = InteracteableType.Ghost;

        GhostManager.Instance.ghostMovementSpeed = 2f;
        rotationSpeed = 4f;
        minDistance = 0.5f;

        isTargeted = false;

        SetupGhost();

        StartCoroutine(WaitForNextTankAnimation(tankAnimationTimer));
    }
    private void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            //Check for Despawn distance
            #region
            float distanceFromPlayer = Vector3.Distance(gameObject.transform.position, MainManager.Instance.playerBody.transform.position);
            if (distanceFromPlayer >= GhostManager.Instance.despawnDistance)
            {
                GhostManager.Instance.DespawnGhost(gameObject);
            }
            #endregion

            //Behavior Tree
            SetGhostBehavior();
        }

        CapturedRate();

        SetGhostState();

        UpdateSoundVolume();
        UpdateGhostSounds();

        TerrainRaycast();
    }


    //--------------------


    public void SetupGhost()
    {
        targetPoint = Vector3.zero;
        ghostStats.isTaken = false;

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
            capturedRate += (GhostManager.Instance.capturedRateSpeed * 1.5f) * Time.deltaTime;

            if (capturedRate > 1)
            {
                GhostCaptured();
            }
        }
        else
        {
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
            
        }
    }

    void SetGhostState()
    {
        //In Ghost Tank
        if (ghostStats.isTaken)
        {
            ghostStats.ghostState = GhostStates.Idle;
        }

        //Moving Around
        else if (capturedRate <= 0)
        {
            ghostStats.ghostState = GhostStates.Moving;
            targetedOnce = false;
            targetedOnceOnce = false;
        }

        //Targeted - Fleeing
        else if (capturedRate > 0)
        {
            if (!targetedOnce)
            {
                targetedOnce = true;

                //Play Targeted Sound
                SoundManager.Instance.Play_Ghost_GhostMood_Targeted_Clip(audioSource_Ghost_GhostSounds);

                //Play Targeted Animation
                //...

                print("IDLE");
                ghostStats.ghostState = GhostStates.Idle;

                StartCoroutine(SetGhostFleeing(1));
            }
            else
            {
                if (targetedOnceOnce)
                {
                    StartCoroutine(SetGhostFleeing(0));
                }
            }
        }

        SetOngoingGhostAnimations();
    }
    IEnumerator SetGhostFleeing(float time)
    {
        yield return new WaitForSeconds(time);

        print("FLEEING");
        ghostStats.ghostState = GhostStates.Fleeing;

        SetGhostMovment();
        targetedOnceOnce = true;
    }

    void SetOngoingGhostAnimations()
    {
        switch (ghostStats.ghostState)
        {
            case GhostStates.Idle:
                anim.SetBool("isMoving", false);
                break;
            case GhostStates.Moving:
                anim.SetBool("isMoving", false);
                break;
            case GhostStates.Attacking:
                break;
            case GhostStates.Fleeing:
                anim.SetBool("isMoving", true);
                anim.SetTrigger("Move");
                break;

            default:
                break;
        }
    }
    IEnumerator WaitForNextTankAnimation(float time)
    {
        if (ghostStats.isTaken)
        {
            if (anim.GetInteger("IdleAnimation") != 0)
            {
                anim.SetInteger("IdleAnimation", 0);
            }
            else
            {
                anim.SetInteger("IdleAnimation", Random.Range(0, 8));
            }
        }
        
        yield return new WaitForSeconds(time);

        tankAnimationTimer = Random.Range(25, 80);

        StartCoroutine(WaitForNextTankAnimation(tankAnimationTimer));
    }

    //--------------------

    void SetGhostBehavior()
    {
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
        }
    }

    #region Idle
    void Idle()
    {
        
    }
    #endregion

    #region Movement
    void Move()
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
        if (Vector3.Distance(transform.position, targetPoint) <= minDistance)
        {
            SetGhostMovment();
        }
    }
    void SetGhostMovment()
    {
        //Generate a random target point
        Quaternion randomPosition;

        if (targetPoint == Vector3.zero)
        {
            randomPosition = new Quaternion(
            Random.Range(spawnPos.x - 3f, spawnPos.x + 3f),
            Random.Range(spawnPos.y - 0.5f, spawnPos.y + 0.5f),
            Random.Range(spawnPos.z - 3f, spawnPos.z + 3f),

            Random.Range(GhostManager.Instance.ghostMovementSpeed - 0.5f, GhostManager.Instance.ghostMovementSpeed + 0.5f));
        }
        else
        {
            randomPosition = new Quaternion(
            Random.Range(targetPoint.x - 3f, targetPoint.x + 3f),
            Random.Range(targetPoint.y - 1f, targetPoint.y + 1f),
            Random.Range(targetPoint.z - 3f, targetPoint.z + 3f),

            Random.Range(GhostManager.Instance.ghostMovementSpeed - 0.5f, GhostManager.Instance.ghostMovementSpeed + 0.5f));
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
        //Move the Ghost
        Vector3 ghostDirection = transform.position - MainManager.Instance.playerBody.transform.position;
        Vector3 fleeDirection;

        ////Change Direction
        //fleeDirectionTimer += Time.deltaTime;
        //if (fleeDirectionTimer >= fleeDirectionTimerCheck)
        //{
        //    fleeDirectionTimerCheck = Random.Range(1f, 2f);

        //    float randomAngle = Random.Range(-45, 45);
        //    Quaternion randomRotation = Quaternion.AngleAxis(randomAngle, Vector3.up);
        //    fleeDirection = (randomRotation * ghostDirection).normalized;

        //    fleeDirectionKeepTimer += Time.deltaTime;
        //    if (fleeDirectionKeepTimer >= 3)
        //    {
        //        fleeDirectionKeepTimer = 0;
        //        fleeDirectionTimer = 0;
        //    }
        //}
        //else
        //{
        //    fleeDirection = ghostDirection.normalized;
        //}

        fleeDirection = ghostDirection.normalized;


        //--------------------


        //Change Speed
        fleeSpeedTimer += Time.deltaTime;
        if (fleeSpeedTimer >= fleeSpeedTimerCheck)
        {
            fleeSpeedTimerCheck = Random.Range(1f, 2f);
            fleeTempSpeedMultiplier = Random.Range(0.75f, 2.5f);

            fleeSpeedTimer = 0;
        }

        //Check for height and terrain collision
        float tempPosY;
        if (isHittingTerrain)
        {
            tempPosY = transform.position.y + (Time.deltaTime * 2);
        }
        else
        {
            tempPosY = transform.position.y;
        }

        Vector3 fleePosition = transform.position + fleeDirection * GhostManager.Instance.ghostMovementSpeed * fleeTempSpeedMultiplier * 1.75f * Time.deltaTime;
        fleePosition.y = tempPosY;

        //Move Ghost
        transform.position = fleePosition;

        //Rotate Ghost
        Quaternion targetRotation = Quaternion.LookRotation(fleeDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 4f);
    }
    void SetGhostFleeTargetPosition()
    {
        // Calculate a random angle within the range
        float randomAngle = Random.Range(-45, 45);

        // Convert the angle to radians
        float angleInRadians = randomAngle * Mathf.Deg2Rad;

        // Calculate the forward vector of the player
        Vector3 forward = transform.forward;

        // Calculate the direction vector based on the random angle
        Vector3 direction = Quaternion.AngleAxis(randomAngle, transform.up) * forward;

        // Calculate the random point in front of the player
        Vector3 randomPoint = transform.position + direction * Random.Range(1, 3);

        //Check for height and terrain collision
        float tempPosY;
        if (isHittingTerrain)
        {
            tempPosY = transform.position.y + 0.5f;
        }
        else
        {
            tempPosY = transform.position.y;
        }

        // Set the height of the random point to match the player's height
        randomPoint.y = tempPosY;
        fleeTargetPoint = randomPoint;
    }
    void tempFlee()
    {
        //Move the Ghost
        Vector3 ghostDirection = transform.position - MainManager.Instance.playerBody.transform.position;
        Vector3 fleeDirection;

        //Change Direction
        fleeDirectionTimer += Time.deltaTime;
        if (fleeDirectionTimer >= fleeDirectionTimerCheck)
        {
            fleeDirectionTimerCheck = Random.Range(1f, 2f);

            float randomAngle = Random.Range(-45, 45);
            Quaternion randomRotation = Quaternion.AngleAxis(randomAngle, Vector3.up);
            fleeDirection = (randomRotation * ghostDirection).normalized;

            fleeDirectionKeepTimer += Time.deltaTime;
            if (fleeDirectionKeepTimer >= 3)
            {
                fleeDirectionKeepTimer = 0;
                fleeDirectionTimer = 0;
            }
        }
        else
        {
            fleeDirection = ghostDirection.normalized;
        }


        //--------------------


        //Change Speed
        fleeSpeedTimer += Time.deltaTime;
        if (fleeSpeedTimer >= fleeSpeedTimerCheck)
        {
            fleeSpeedTimerCheck = Random.Range(1f, 2f);
            fleeTempSpeedMultiplier = Random.Range(0.75f, 2.5f);

            fleeSpeedTimer = 0;
        }

        //Check for height and terrain collision
        float tempPosY;
        if (isHittingTerrain)
        {
            tempPosY = transform.position.y + (Time.deltaTime * 2);
        }
        else
        {
            tempPosY = transform.position.y;
        }

        Vector3 fleePosition = transform.position + fleeDirection * GhostManager.Instance.ghostMovementSpeed * fleeTempSpeedMultiplier * 1.75f * Time.deltaTime;
        fleePosition.y = tempPosY;

        //Move Ghost
        transform.position = fleePosition;

        //Rotate Ghost
        Quaternion targetRotation = Quaternion.LookRotation(fleeDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 4f);
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


    //--------------------


    void TerrainRaycast()
    {
        // Create a ray from the center of the cube pointing downwards
        Ray ray = new Ray(transform.position, Vector3.down);

        // Perform the raycast
        if (Physics.Raycast(ray, out hit, 5, groundLayerMask))
        {
            if (Vector3.Distance(hit.point, gameObject.transform.position) <= 0.5f)
            {
                print("Go Upwards");
                targetPoint = new Vector3(targetPoint.x, targetPoint.y + 0.25f, targetPoint.z);
                isHittingTerrain = true;
            }
            else if (Vector3.Distance(hit.point, gameObject.transform.position) >= 3f)
            {
                print("Go Downwards");
                targetPoint = new Vector3(targetPoint.x, targetPoint.y - 0.25f, targetPoint.z);
                isHittingTerrain = true;
            }
            else
            {
                print("Go Nothing");
                isHittingTerrain = false;
            }
        }
        else
        {
            isHittingTerrain = false;
        }
    }
}
