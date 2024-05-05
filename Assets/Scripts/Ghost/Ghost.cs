using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    #region Variables
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

    [Header("Snapping_Raycast")]
    [SerializeField] int terrainDirection_Down;
    [SerializeField] int terrainDirection_Forward;
    public LayerMask ghostLayerMask;
    public LayerMask groundLayerMask;

    Ray ray;
    RaycastHit hit;

    float fleeRightDirectionTimer;
    float fleeRightDirectionTimerCheck = 2;

    float fleeUpDirectionTimer;
    float fleeUpDirectionTimerCheck = 2;

    float fleeSpeedTimer;
    float fleeSpeedTimerCheck = 2;
    float fleeTempSpeedMultiplier = 1;
    float fleeSpeedObstacleMultiplier = 1;

    float fleeDirectionKeepTimer;


    [Header("In GhostTank")]
    float tankAnimationTimer;
    #endregion


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
        if (ghostStats.ghostState == GhostStates.Tank) { return; }

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

        fleeRightDirectionTimer = 0;
        fleeUpDirectionTimer = 0;
        fleeTempSpeedMultiplier = 1;
    }
    public void SetGhostAppearance()
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
            if (capturedRate > 0 && ghostStats.ghostState != GhostStates.Idle)
            {
                capturedRate -= (GhostManager.Instance.capturedRateSpeed / 2) * Time.deltaTime;
            }

            if (capturedRate < 0)
            {
                capturedRate = 0;
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

            fleeRightDirectionTimer = 0;
            fleeUpDirectionTimer = 0;
            fleeTempSpeedMultiplier = 1;

            targetedOnce = false;
            targetedOnceOnce = false;
        }

        //Targeted - Fleeing
        else if (capturedRate > 0 && ghostStats.ghostState != GhostStates.Idle)
        {
            if (!targetedOnce)
            {
                targetedOnce = true;

                //Play Targeted Sound
                SoundManager.Instance.Play_Ghost_GhostMood_Targeted_Clip(audioSource_Ghost_GhostSounds);

                //Play Targeted Animation
                //...

                ghostStats.ghostState = GhostStates.Idle;

                StartCoroutine(SetGhostFleeing(0.5f));
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
                anim.SetTrigger("Move");
                break;
            case GhostStates.Moving:
                anim.SetBool("isMoving", false);
                break;
            case GhostStates.Attacking:
                anim.SetBool("isMoving", false);
                break;
            case GhostStates.Fleeing:
                anim.SetBool("isMoving", true);
                break;

            default:
                break;
        }
    }
    IEnumerator WaitForNextTankAnimation(float time)
    {
        if (ghostStats.ghostState == GhostStates.Tank && gameObject.activeInHierarchy)
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

        tankAnimationTimer = Random.Range(5, 5); //25, 80

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
            case GhostStates.Tank:
                Tank();
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
        //Get the movementDirection of the player
        Vector3 ghostDirection = transform.position - MainManager.Instance.playerBody.transform.position;

        //Get the direction for the Ghost to flee in
        Vector3 fleeDirection = ghostDirection.normalized;


        //--------------------


        //Change Right-Vector to random
        fleeRightDirectionTimerCheck = Random.Range(-10f + PerkManager.Instance.perkValues.ghostMovementReducer_Right, 10f - PerkManager.Instance.perkValues.ghostMovementReducer_Right);
        fleeRightDirectionTimer += fleeRightDirectionTimerCheck * Time.deltaTime;

        if (fleeRightDirectionTimer != 0)
        {
            if (terrainDirection_Forward == 1)
            {
                fleeRightDirectionTimer = Random.Range(0, 2) == 0 ? -0.5f : 0.5f;
            }

            fleeDirection.x = fleeRightDirectionTimer;
        }


        //Change Up-vector to random
        fleeUpDirectionTimerCheck = Random.Range(-0.75f + PerkManager.Instance.perkValues.ghostMovementReducer_Up, 0.75f - PerkManager.Instance.perkValues.ghostMovementReducer_Up);

        //Check for height and terrain collision
        if (terrainDirection_Down == 1 || terrainDirection_Forward == 1)
        {
            fleeUpDirectionTimer = 0.35f;
        }
        if (terrainDirection_Down == 2 || terrainDirection_Forward == 2)
        {
            fleeUpDirectionTimer = -0.35f;
        }
        else
        {
            fleeUpDirectionTimer += fleeUpDirectionTimerCheck * Time.deltaTime;
        }

        fleeDirection.y = fleeUpDirectionTimer;


        //Change Speed to random
        fleeSpeedTimerCheck = Random.Range(-0.75f + PerkManager.Instance.perkValues.ghostMovementReducer_Speed, 0.75f - PerkManager.Instance.perkValues.ghostMovementReducer_Speed);
        fleeTempSpeedMultiplier += fleeSpeedTimerCheck * Time.deltaTime;

        if (fleeTempSpeedMultiplier <= 0.1f)
            fleeTempSpeedMultiplier = 0.1f;
        if (fleeTempSpeedMultiplier >= 1.5f)
            fleeTempSpeedMultiplier = 1.5f;


        //--------------------


        Vector3 fleePosition = transform.position + fleeDirection * GhostManager.Instance.ghostMovementSpeed * fleeTempSpeedMultiplier * fleeSpeedObstacleMultiplier * 1.25f * Time.deltaTime;

        //Move Ghost
        transform.position = fleePosition;

        //Rotate Ghost
        Quaternion targetRotation = Quaternion.LookRotation(fleeDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 4f);
    }
    #endregion

    #region Tank
    void Tank()
    {
        if (gameObject.GetComponent<InvisibleObject>())
        {
            gameObject.GetComponent<InvisibleObject>().isInTank = true;
            gameObject.GetComponent<InvisibleObject>().transparencyValue = 0;
        }
    }
    #endregion


    //--------------------


    void GhostCaptured()
    {
        SoundManager.Instance.Play_Ghost_CaptureGhost_Clip();

        GhostManager.Instance.AddGhostToCapturer(ghostStats);

        GhostManager.Instance.DespawnGhost(gameObject);
    }


    //--------------------


    void TerrainRaycast()
    {
        //Raycast Down
        #region
        ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, out hit, 5, groundLayerMask))
        {
            if (Vector3.Distance(hit.point, gameObject.transform.position) <= 0.5f)
            {
                targetPoint = new Vector3(targetPoint.x, targetPoint.y + 0.25f, targetPoint.z);
                terrainDirection_Down = 1;
            }
            else if (Vector3.Distance(hit.point, gameObject.transform.position) >= 2.5f)
            {
                targetPoint = new Vector3(targetPoint.x, targetPoint.y - 0.25f, targetPoint.z);
                terrainDirection_Down = 2;
            }
            else
            {
                terrainDirection_Down = 0;
            }
        }
        else
        {
            terrainDirection_Down = 0;
        }
        #endregion

        //Raycast Forward
        #region
        ray = new Ray(transform.position, Vector3.forward);

        if (Physics.Raycast(ray, out hit, 5, groundLayerMask))
        {
            if (Vector3.Distance(hit.point, gameObject.transform.position) <= 4)
            {
                targetPoint = new Vector3(targetPoint.x, targetPoint.y + 0.25f, targetPoint.z);
                terrainDirection_Forward = 1;
            }
            else
            {
                terrainDirection_Forward = 0;
                fleeSpeedObstacleMultiplier = 1;
            }
        }
        else
        {
            terrainDirection_Forward = 0;
            fleeSpeedObstacleMultiplier = 1;
        }
        #endregion
    }
}
