using UnityEngine;

public class PlayerMovement : Singleton<PlayerMovement>
{
    public CharacterController controller;

    public float movementSpeed = 4f;
    public float movementSpeedVarianceByMovement = 1f;
    public float movementSpeedVarianceByWeather = 1f;

    public float gravity = -9.81f * 2;
    public float gravityResistance = 2f;

    Vector3 velocity;

    bool movementAppearance_isHappening;
    float FOV_Smoother = 0;
    //float crouch_Kneel = -0.75f;
    float crouch_Up = -0.5f;

    float movement_X;
    float movement_Z;


    [Header("RaycastHit Distance")]
    [SerializeField] GameObject pointUp_0;
    [SerializeField] GameObject pointUp_1;
    [SerializeField] GameObject pointUp_2;
    [SerializeField] GameObject pointUp_3;
    [SerializeField] GameObject pointUp_4;
    [SerializeField] float raycastDistance = 0.2f;
    RaycastHit hit;

    [Header("Fall Damage2")]
    [SerializeField] bool jumping;
    [SerializeField] Vector3 jumpDistance_Start;
    [SerializeField] Vector3 jumpDistance_End;
    [SerializeField] float totalJumpDistance = 0;
    [SerializeField] float safeJumpDistance = 10f;
    [SerializeField] float fallDamage = 0;

    public float normalRaycastPos = -1.4f;
    public float crouchRaycastPos = -1.2f;


    //--------------------


    private void Start()
    {
        //Set Player to the height it's supposed to be
        MainManager.Instance.playerBody.transform.SetPositionAndRotation(new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), transform.rotation);

        gravity = -9.81f * 2;
        gravityResistance = 2f;

        jumpDistance_Start = Vector3.zero;
        jumpDistance_End = Vector3.zero;
    }
    void Update()
    {
        if (MainManager.Instance.gameStates == GameStates.GameOver)  { return; }

        //Perform the movement
        Movement();

        //Check if not crouching while stuck under something
        if (!HeadIsHittingSomethingWhileCrouching())
        {
            //Set States
            SetMovementStates(movement_X, movement_Z);

            //Update Health Parameters
            UpdateHealthValues();

            //FOV smoothness
            if (movementAppearance_isHappening)
            {
                MovementAppearance();
            }
        }
        else
        {
            PlayerManager.Instance.movementStates = MovementStates.Crouching;
        }

        if (MainManager.Instance.menuStates == MenuStates.None)
        {
            CheckJumpTimer();
        }
    }


    //--------------------


    void Movement()
    {
        movement_X = Input.GetAxis("Horizontal");
        movement_Z = Input.GetAxis("Vertical");

        //Check if button is not pressed
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
        {
            movement_X = 0;
        }
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            movement_Z = 0;
        }

        //right is the red Axis, forward is the blue axis
        Vector3 move = MainManager.Instance.playerBody.transform.right * movement_X + MainManager.Instance.playerBody.transform.forward * movement_Z;

        controller.Move(move * movementSpeed * movementSpeedVarianceByWeather * movementSpeedVarianceByMovement * PlayerManager.Instance.movementSpeedMultiplier_SkillTree * Time.deltaTime);

        //check if the player is on the ground so he can jump
        if (Input.GetKeyDown(KeyCode.Space) && GetComponent<DistanceAboveGround>().isGrounded)
        {
            //the equation for jumping
            velocity.y = Mathf.Sqrt(PlayerManager.Instance.jumpHeight * -gravityResistance * gravity);
        }

        if (movement_X == 0 && movement_Z == 0)
        {
            velocity.x = 0;
            velocity.z = 0;
        }

        if (GetComponent<DistanceAboveGround>().isGrounded && velocity.y < 0)
        {
            velocity.y = -2;
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    void SetMovementStates(float x, float z)
    {
        // If no menus are open
        if (MainManager.Instance.menuStates == MenuStates.None)
        {
            //Jumping
            if (!GetComponent<DistanceAboveGround>().isGrounded)
            {
                if (PlayerManager.Instance.movementStates != MovementStates.Jumping)
                {
                    PlayerManager.Instance.oldMovementStates = PlayerManager.Instance.movementStates;

                    if (PlayerManager.Instance.oldMovementStates == MovementStates.Crouching)
                    {
                        //MainManager.Instance.playerBody.transform.localPosition = new Vector3(
                        //                                                             MainManager.Instance.playerBody.transform.localPosition.x,
                        //                                                             crouch_Up,
                        //                                                             MainManager.Instance.playerBody.transform.localPosition.z);
                    }

                    //MainManager.Instance.player.GetComponent<CharacterController>().height = 1.1f;

                    //MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_0.transform.localPosition = new Vector3(0, normalRaycastPos, 0);
                    //MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_1.transform.localPosition = new Vector3(0, normalRaycastPos, 0.5f);
                    //MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_2.transform.localPosition = new Vector3(0, normalRaycastPos, -0.5f);
                    //MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_3.transform.localPosition = new Vector3(0.5f, normalRaycastPos, 0);
                    //MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_4.transform.localPosition = new Vector3(-0.5f, normalRaycastPos, 0);

                    //MainManager.Instance.playerBody.GetComponent<CapsuleCollider>().height = 1.75f;
                    //MainManager.Instance.playerBody.GetComponent<CapsuleCollider>().center = new Vector3(0f, 0f, 0f);

                    movementAppearance_isHappening = true;
                    jumpDistance_Start = MainManager.Instance.playerBody.transform.position;
                    jumping = true;
                }

                PlayerManager.Instance.movementStates = MovementStates.Jumping;
            }

            //Running
            else if (((x != 0 && z != 0) || x != 0 || z != 0) && Input.GetKey(KeyCode.LeftShift))
            {
                if (PlayerManager.Instance.movementStates != MovementStates.Running)
                {
                    PlayerManager.Instance.oldMovementStates = PlayerManager.Instance.movementStates;

                    if (PlayerManager.Instance.oldMovementStates == MovementStates.Crouching)
                    {
                        //MainManager.Instance.playerBody.transform.localPosition = new Vector3(
                        //                                                             MainManager.Instance.playerBody.transform.localPosition.x,
                        //                                                             crouch_Up,
                        //                                                             MainManager.Instance.playerBody.transform.localPosition.z);
                    }

                    MainManager.Instance.player.GetComponent<CharacterController>().height = 1.1f;

                    MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_0.transform.localPosition = new Vector3(0, normalRaycastPos, 0);
                    MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_1.transform.localPosition = new Vector3(0, normalRaycastPos, 0.5f);
                    MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_2.transform.localPosition = new Vector3(0, normalRaycastPos, -0.5f);
                    MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_3.transform.localPosition = new Vector3(0.5f, normalRaycastPos, 0);
                    MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_4.transform.localPosition = new Vector3(-0.5f, normalRaycastPos, 0);

                    MainManager.Instance.playerBody.GetComponent<CapsuleCollider>().height = 1.75f;
                    MainManager.Instance.playerBody.GetComponent<CapsuleCollider>().center = new Vector3(0f, 0f, 0f);

                    movementAppearance_isHappening = true;
                    jumping = false;

                    //Check if a jump is landed
                    jumpDistance_End = MainManager.Instance.playerBody.transform.position;
                }

                PlayerManager.Instance.movementStates = MovementStates.Running;
            }

            //Crouching
            else if (Input.GetKey(KeyCode.LeftControl) || (((x != 0 && z != 0) || x != 0 || z != 0) && Input.GetKey(KeyCode.LeftControl)))
            {
                if (PlayerManager.Instance.movementStates != MovementStates.Crouching)
                {
                    PlayerManager.Instance.oldMovementStates = PlayerManager.Instance.movementStates;

                    //MainManager.Instance.playerBody.transform.localPosition = new Vector3(
                    //                                                                 MainManager.Instance.playerBody.transform.localPosition.x,
                    //                                                                 /*crouch_Kneel*/crouch_Up,
                    //                                                                 MainManager.Instance.playerBody.transform.localPosition.z);

                    MainManager.Instance.player.GetComponent<CharacterController>().height = 0.25f;

                    MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_0.transform.localPosition = new Vector3(0, crouchRaycastPos, 0);
                    MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_1.transform.localPosition = new Vector3(0, crouchRaycastPos, 0.5f);
                    MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_2.transform.localPosition = new Vector3(0, crouchRaycastPos, -0.5f);
                    MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_3.transform.localPosition = new Vector3(0.5f, crouchRaycastPos, 0);
                    MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_4.transform.localPosition = new Vector3(-0.5f, crouchRaycastPos, 0);

                    MainManager.Instance.playerBody.GetComponent<CapsuleCollider>().height = 1.2f;
                    MainManager.Instance.playerBody.GetComponent<CapsuleCollider>().center = new Vector3(0f, 0.5f, 0f);

                    movementAppearance_isHappening = true;
                    jumping = false;

                    //Check if a jump is landed
                    jumpDistance_End = MainManager.Instance.playerBody.transform.position;
                }

                PlayerManager.Instance.movementStates = MovementStates.Crouching;
            }

            //Walking
            else if ((x != 0 && z != 0) || x != 0 || z != 0)
            {
                if (PlayerManager.Instance.movementStates != MovementStates.Walking)
                {
                    PlayerManager.Instance.oldMovementStates = PlayerManager.Instance.movementStates;

                    if (PlayerManager.Instance.oldMovementStates == MovementStates.Crouching)
                    {
                        //MainManager.Instance.playerBody.transform.localPosition = new Vector3(
                        //                                                             MainManager.Instance.playerBody.transform.localPosition.x,
                        //                                                             crouch_Up,
                        //                                                             MainManager.Instance.playerBody.transform.localPosition.z);
                    }

                    MainManager.Instance.player.GetComponent<CharacterController>().height = 1.1f;

                    MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_0.transform.localPosition = new Vector3(0, normalRaycastPos, 0);
                    MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_1.transform.localPosition = new Vector3(0, normalRaycastPos, 0.5f);
                    MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_2.transform.localPosition = new Vector3(0, normalRaycastPos, -0.5f);
                    MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_3.transform.localPosition = new Vector3(0.5f, normalRaycastPos, 0);
                    MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_4.transform.localPosition = new Vector3(-0.5f, normalRaycastPos, 0);

                    MainManager.Instance.playerBody.GetComponent<CapsuleCollider>().height = 1.75f;
                    MainManager.Instance.playerBody.GetComponent<CapsuleCollider>().center = new Vector3(0f, 0f, 0f);

                    movementAppearance_isHappening = true;
                    jumping = false;

                    //Check if a jump is landed
                    jumpDistance_End = MainManager.Instance.playerBody.transform.position;
                }

                PlayerManager.Instance.movementStates = MovementStates.Walking;
            }

            //Standing
            else
            {
                if (PlayerManager.Instance.movementStates != MovementStates.Standing)
                {
                    PlayerManager.Instance.oldMovementStates = PlayerManager.Instance.movementStates;

                    if (PlayerManager.Instance.oldMovementStates == MovementStates.Crouching)
                    {
                        //MainManager.Instance.playerBody.transform.localPosition = new Vector3(
                        //                                                             MainManager.Instance.playerBody.transform.localPosition.x,
                        //                                                             crouch_Up,
                        //                                                             MainManager.Instance.playerBody.transform.localPosition.z);
                    }

                    MainManager.Instance.player.GetComponent<CharacterController>().height = 1.1f;

                    MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_0.transform.localPosition = new Vector3(0, normalRaycastPos, 0);
                    MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_1.transform.localPosition = new Vector3(0, normalRaycastPos, 0.5f);
                    MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_2.transform.localPosition = new Vector3(0, normalRaycastPos, -0.5f);
                    MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_3.transform.localPosition = new Vector3(0.5f, normalRaycastPos, 0);
                    MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_4.transform.localPosition = new Vector3(-0.5f, normalRaycastPos, 0);

                    MainManager.Instance.playerBody.GetComponent<CapsuleCollider>().height = 1.75f;
                    MainManager.Instance.playerBody.GetComponent<CapsuleCollider>().center = new Vector3(0f, 0f, 0f);

                    movementAppearance_isHappening = true;
                    jumping = false;

                    //Check if a jump is landed
                    jumpDistance_End = MainManager.Instance.playerBody.transform.position;
                }

                PlayerManager.Instance.movementStates = MovementStates.Standing;
            }
        }

        //If a menu is open
        else
        {
            PlayerManager.Instance.oldMovementStates = PlayerManager.Instance.movementStates;

            MainManager.Instance.playerBody.transform.localPosition = new Vector3(
                                                                         MainManager.Instance.playerBody.transform.localPosition.x,
                                                                         crouch_Up,
                                                                         MainManager.Instance.playerBody.transform.localPosition.z);

            MainManager.Instance.player.GetComponent<CharacterController>().height = 1.1f;

            //MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_0.transform.localPosition = new Vector3(0, -1, 0);
            //MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_1.transform.localPosition = new Vector3(0, -1, 0.5f);
            //MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_2.transform.localPosition = new Vector3(0, -1, -0.5f);
            //MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_3.transform.localPosition = new Vector3(0.5f, -1, 0);
            //MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_4.transform.localPosition = new Vector3(-0.5f, -1, 0);

            MainManager.Instance.playerBody.GetComponent<CapsuleCollider>().height = 1.75f;
            MainManager.Instance.playerBody.GetComponent<CapsuleCollider>().center = new Vector3(0f, 0f, 0f);

            movementAppearance_isHappening = true;
            jumping = false;

            //Check if a jump is landed
            jumpDistance_End = MainManager.Instance.playerBody.transform.position;

            PlayerManager.Instance.movementStates = MovementStates.Standing;
        }
    }

    void UpdateHealthValues()
    {
        switch (PlayerManager.Instance.movementStates)
        {
            case MovementStates.Standing:
                PlayerManager.Instance.hungerTemp = 0;
                PlayerManager.Instance.thirstTemp = 0;
                PlayerManager.Instance.heatresistanceTemp = 0;
                break;
            case MovementStates.Walking:
                PlayerManager.Instance.hungerTemp = -1;
                PlayerManager.Instance.thirstTemp = -1;
                PlayerManager.Instance.heatresistanceTemp = 0;
                break;
            case MovementStates.Running:
                PlayerManager.Instance.hungerTemp = -3;
                PlayerManager.Instance.thirstTemp = -2;
                PlayerManager.Instance.heatresistanceTemp = -2;
                break;
            case MovementStates.Crouching:
                PlayerManager.Instance.hungerTemp = 1;
                PlayerManager.Instance.thirstTemp = 1;
                PlayerManager.Instance.heatresistanceTemp = 0;
                break;
            case MovementStates.Jumping:
                PlayerManager.Instance.hungerTemp = -6;
                PlayerManager.Instance.thirstTemp = -3;
                PlayerManager.Instance.heatresistanceTemp = -1;
                break;

            default:
                break;
        }
    }

    void MovementAppearance()
    {
        PlayerManager.Instance.FOV_Addon = 0;

        float timeMultiplier_Running = 60;
        float timeMultiplier_Crouching = 30;
        float timeMultiplier_Reset = 30;

        switch (PlayerManager.Instance.movementStates)
        {
            case MovementStates.Standing:
                PlayerManager.Instance.FOV_Addon = PlayerManager.Instance.FOV_Standing;

                if (FOV_Smoother >= PlayerManager.Instance.FOV_Standing + 0.2f)
                {
                    FOV_Smoother -= Time.deltaTime * timeMultiplier_Reset;
                }
                else if(FOV_Smoother <= PlayerManager.Instance.FOV_Standing - 0.2f)
                {
                    FOV_Smoother += Time.deltaTime * timeMultiplier_Reset;
                }
                else
                {
                    FOV_Smoother = PlayerManager.Instance.FOV_Standing;
                    movementAppearance_isHappening = false;
                }

                //SettingsManager.Instance.ChangeFOV(FOV_Smoother);
                movementSpeedVarianceByMovement = 1;
                break;
            case MovementStates.Walking:
                PlayerManager.Instance.FOV_Addon = PlayerManager.Instance.FOV_Walking;

                if (FOV_Smoother >= PlayerManager.Instance.FOV_Walking + 0.2f)
                {
                    FOV_Smoother -= Time.deltaTime * timeMultiplier_Reset;
                }
                else if (FOV_Smoother <= PlayerManager.Instance.FOV_Walking - 0.2f)
                {
                    FOV_Smoother += Time.deltaTime * timeMultiplier_Reset;
                }
                else
                {
                    FOV_Smoother = PlayerManager.Instance.FOV_Walking;
                    movementAppearance_isHappening = false;
                }

                //SettingsManager.Instance.ChangeFOV(FOV_Smoother);
                movementSpeedVarianceByMovement = 1;
                break;
            case MovementStates.Running:
                PlayerManager.Instance.FOV_Addon = PlayerManager.Instance.FOV_Running;

                if (FOV_Smoother >= PlayerManager.Instance.FOV_Running + 0.2f)
                {
                    FOV_Smoother -= Time.deltaTime * timeMultiplier_Running;
                }
                else if (FOV_Smoother <= PlayerManager.Instance.FOV_Running - 0.2f)
                {
                    FOV_Smoother += Time.deltaTime * timeMultiplier_Running;
                }
                else
                {
                    FOV_Smoother = PlayerManager.Instance.FOV_Running;
                    movementAppearance_isHappening = false;
                }

                //SettingsManager.Instance.ChangeFOV(FOV_Smoother);
                movementSpeedVarianceByMovement = 2;
                break;
            case MovementStates.Crouching:
                PlayerManager.Instance.FOV_Addon = PlayerManager.Instance.FOV_Crouching;

                if (FOV_Smoother >= PlayerManager.Instance.FOV_Crouching + 0.2f)
                {
                    FOV_Smoother -= Time.deltaTime * timeMultiplier_Crouching;
                }
                else if (FOV_Smoother <= PlayerManager.Instance.FOV_Crouching - 0.2f)
                {
                    FOV_Smoother += Time.deltaTime * timeMultiplier_Crouching;
                }
                else
                {
                    FOV_Smoother = PlayerManager.Instance.FOV_Crouching;
                    movementAppearance_isHappening = false;
                }

                //SettingsManager.Instance.ChangeFOV(FOV_Smoother);
                movementSpeedVarianceByMovement = 0.5f;
                break;
            case MovementStates.Jumping:
                break;

            default:
                break;
        }
    }

    bool HeadIsHittingSomethingWhileCrouching()
    {
        if (PlayerManager.Instance.movementStates != MovementStates.Crouching)
        {
            PlayerManager.Instance.isHittingHeadRaycast = false;

            return false;
        }
        else
        {
            if (Raycast(pointUp_0.transform.position))
            {
                PlayerManager.Instance.isHittingHeadRaycast = true;
                return true;
            }
            else if (Raycast(pointUp_1.transform.position))
            {
                PlayerManager.Instance.isHittingHeadRaycast = true;
                return true;
            }
            else if (Raycast(pointUp_2.transform.position))
            {
                PlayerManager.Instance.isHittingHeadRaycast = true;
                return true;
            }
            else if (Raycast(pointUp_3.transform.position))
            {
                PlayerManager.Instance.isHittingHeadRaycast = true;
                return true;
            }
            else if (Raycast(pointUp_4.transform.position))
            {
                PlayerManager.Instance.isHittingHeadRaycast = true;
                return true;
            }
            else
            {
                PlayerManager.Instance.isHittingHeadRaycast = false;
                return false;
            }
        }
    }
    bool Raycast(Vector3 pos)
    {
        if (Physics.Raycast(pos, Vector3.up, out hit, raycastDistance))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void CheckJumpTimer()
    {
        if (!jumping)
        {
            jumpDistance_End = MainManager.Instance.playerBody.transform.position;

            if (jumpDistance_Start != Vector3.zero && jumpDistance_End != Vector3.zero)
            {
                totalJumpDistance = Vector3.Distance(jumpDistance_Start, jumpDistance_End);

                if (totalJumpDistance >= safeJumpDistance)
                {
                    SoundManager.Instance.Play_Player_FallDamage_Clip();

                    fallDamage = totalJumpDistance - safeJumpDistance;

                    float damageTaken = fallDamage / 20;
                    HealthManager.Instance.mainHealthValue -= damageTaken;

                    print("1. Height: " + totalJumpDistance + " | Damage: " + damageTaken);
                }

                //Reset jumpDistance
                jumpDistance_Start = Vector3.zero;
                jumpDistance_End = Vector3.zero;
            }
        }
    }
}