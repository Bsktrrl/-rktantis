using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    Animator anim;

    [Header("General")]
    public GhostStates ghostState;
    public GhostAppearance ghostAppearance;
    public bool isBeard;

    [Header("Stats")]
    public float movementSpeed = 2f;
    public float tempMovementSpeed;
    public float rotationSpeed = 4f;
    public float minDistance = 0.5f;

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
        movementSpeed = 2f;
        rotationSpeed = 4f;
        minDistance = 0.5f;

        anim = GetComponent<Animator>();
        spawnPos = transform.position;

        SetGhostMovment();
    }
    private void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            anim.SetFloat("MovementSpeed", movementSpeed);

            //Check for Despawn distance
            float distanceFromPlayer = Vector3.Distance(gameObject.transform.position, MainManager.Instance.playerBody.transform.position);
            if (distanceFromPlayer >= GhostManager.Instance.despawnDistance)
            {
                GhostManager.Instance.DespawnGhost(gameObject);
            }

            //Behavior Tree
            switch (ghostState)
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
        }
    }


    //--------------------


    public void SetupGhost()
    {
        SetGhostMovment();

        SetGhostAppearance();
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
        if (isBeard)
        {
            beard.SetActive(true);
        }
        else
        {
            beard.SetActive(false);
        }

        //Set Style
        switch (ghostAppearance)
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
}
