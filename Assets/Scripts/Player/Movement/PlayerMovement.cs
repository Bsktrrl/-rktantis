using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Singleton<PlayerMovement>
{
    public CharacterController controller;

    public float movementSpeed = 4f;
    public float movementSpeedVarianceByMovement = 1f;
    public float movementSpeedVarianceByWeather = 1f;

    public float gravity = -30f;

    Vector3 velocity;

    bool movementAppearance_isHappening;
    float FOV_Smoother = 0;
    float crouch_Kneel = -0.8f;
    float crouch_Up = -0.5f;


    //--------------------


    private void Start()
    {
        //Set Player to the height it's supposed to be
        MainManager.Instance.playerBody.transform.SetPositionAndRotation(new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), transform.rotation);

        gravity = -30f;
    }
    void Update()
    {
        Movement();
        UpdateHealthValues();

        if (movementAppearance_isHappening)
        {
            MovementAppearance();
        }
    }


    //--------------------


    void Movement()
    {
        #region Movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //Check if button is not pressed
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
        {
            x = 0;
        }
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            z = 0;
        }

        //right is the red Axis, forward is the blue axis
        Vector3 move = MainManager.Instance.playerBody.transform.right * x + MainManager.Instance.playerBody.transform.forward * z;

        controller.Move(move * movementSpeed * movementSpeedVarianceByWeather * movementSpeedVarianceByMovement * PlayerManager.Instance.movementSpeedMultiplier_SkillTree * Time.deltaTime);

        //check if the player is on the ground so he can jump
        if (Input.GetKeyDown(KeyCode.Space) && GetComponent<DistanceAboveGround>().isGrounded)
        {
            //the equation for jumping
            velocity.y = Mathf.Sqrt(PlayerManager.Instance.jumpHeight * -2f * gravity);
        }

        if (x == 0 && z == 0)
        {
            velocity.x = 0;
            velocity.y += gravity * Time.deltaTime;
            velocity.z = 0;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);
        #endregion

        #region Set different MovementStates
        //If no menus are open
        if (MainManager.Instance.menuStates == MenuStates.None)
        {
            //Jumping
            if (!GetComponent<DistanceAboveGround>().isGrounded)
            {
                if (PlayerManager.Instance.movementStates != MovementStates.Jumping)
                {
                    print("Jumping");
                    PlayerManager.Instance.oldMovementStates = PlayerManager.Instance.movementStates;

                    if (PlayerManager.Instance.oldMovementStates == MovementStates.Crouching)
                    {
                        MainManager.Instance.playerBody.transform.localPosition = new Vector3(
                                                                                     MainManager.Instance.playerBody.transform.localPosition.x,
                                                                                     crouch_Up,
                                                                                     MainManager.Instance.playerBody.transform.localPosition.z);
                    }

                    //MainManager.Instance.player.GetComponent<CharacterController>().height = 1.1f;

                    //MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_0.transform.localPosition = new Vector3(0, -1, 0);
                    //MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_1.transform.localPosition = new Vector3(0, -1, 0);
                    //MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_2.transform.localPosition = new Vector3(0, -1, 0);
                    //MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_3.transform.localPosition = new Vector3(0, -1, 0);
                    //MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_4.transform.localPosition = new Vector3(0, -1, 0);

                    //MainManager.Instance.playerBody.GetComponent<CapsuleCollider>().height = 1.75f;
                    //MainManager.Instance.playerBody.GetComponent<CapsuleCollider>().center = new Vector3(0f, 0f, 0f);

                    movementAppearance_isHappening = true;
                }

                PlayerManager.Instance.movementStates = MovementStates.Jumping;
            }

            //Running
            else if (((x != 0 && z != 0) || x != 0 || z != 0) && Input.GetKey(KeyCode.LeftShift))
            {
                if (PlayerManager.Instance.movementStates != MovementStates.Running)
                {
                    print("Running");
                    PlayerManager.Instance.oldMovementStates = PlayerManager.Instance.movementStates;

                    if (PlayerManager.Instance.oldMovementStates == MovementStates.Crouching)
                    {
                        MainManager.Instance.playerBody.transform.localPosition = new Vector3(
                                                                                     MainManager.Instance.playerBody.transform.localPosition.x, 
                                                                                     crouch_Up,
                                                                                     MainManager.Instance.playerBody.transform.localPosition.z);
                    }

                    MainManager.Instance.player.GetComponent<CharacterController>().height = 1.1f;

                    MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_0.transform.localPosition = new Vector3(0, -1, 0);
                    MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_1.transform.localPosition = new Vector3(0, -1, 0);
                    MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_2.transform.localPosition = new Vector3(0, -1, 0);
                    MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_3.transform.localPosition = new Vector3(0, -1, 0);
                    MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_4.transform.localPosition = new Vector3(0, -1, 0);

                    MainManager.Instance.playerBody.GetComponent<CapsuleCollider>().height = 1.75f;
                    MainManager.Instance.playerBody.GetComponent<CapsuleCollider>().center = new Vector3(0f, 0f, 0f);

                    movementAppearance_isHappening = true;
                }

                PlayerManager.Instance.movementStates = MovementStates.Running;
            }

            //Crouching
            else if (Input.GetKey(KeyCode.LeftControl) || (((x != 0 && z != 0) || x != 0 || z != 0) && Input.GetKey(KeyCode.LeftControl)))
            {
                if (PlayerManager.Instance.movementStates != MovementStates.Crouching)
                {
                    print("Crouching");
                    PlayerManager.Instance.oldMovementStates = PlayerManager.Instance.movementStates;

                    MainManager.Instance.playerBody.transform.localPosition = new Vector3(
                                                                                     MainManager.Instance.playerBody.transform.localPosition.x,
                                                                                     crouch_Kneel,
                                                                                     MainManager.Instance.playerBody.transform.localPosition.z);

                    MainManager.Instance.player.GetComponent<CharacterController>().height = 0.25f;

                    MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_0.transform.localPosition = new Vector3(0, 0.6f, 0);
                    MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_1.transform.localPosition = new Vector3(0, 0.6f, 0);
                    MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_2.transform.localPosition = new Vector3(0, 0.6f, 0);
                    MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_3.transform.localPosition = new Vector3(0, 0.6f, 0);
                    MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_4.transform.localPosition = new Vector3(0, 0.6f, 0);

                    MainManager.Instance.playerBody.GetComponent<CapsuleCollider>().height = 1.2f;
                    MainManager.Instance.playerBody.GetComponent<CapsuleCollider>().center = new Vector3(0f, 0.5f, 0f);

                    movementAppearance_isHappening = true;
                }

                PlayerManager.Instance.movementStates = MovementStates.Crouching;
            }

            //Walking
            else if ((x != 0 && z != 0) || x != 0 || z != 0)
            {
                if (PlayerManager.Instance.movementStates != MovementStates.Walking)
                {
                    print("Walking");
                    PlayerManager.Instance.oldMovementStates = PlayerManager.Instance.movementStates;

                    if (PlayerManager.Instance.oldMovementStates == MovementStates.Crouching)
                    {
                        MainManager.Instance.playerBody.transform.localPosition = new Vector3(
                                                                                     MainManager.Instance.playerBody.transform.localPosition.x,
                                                                                     crouch_Up,
                                                                                     MainManager.Instance.playerBody.transform.localPosition.z);
                    }

                    movementAppearance_isHappening = true;
                }

                MainManager.Instance.player.GetComponent<CharacterController>().height = 1.1f;

                MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_0.transform.localPosition = new Vector3(0, -1, 0);
                MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_1.transform.localPosition = new Vector3(0, -1, 0);
                MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_2.transform.localPosition = new Vector3(0, -1, 0);
                MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_3.transform.localPosition = new Vector3(0, -1, 0);
                MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_4.transform.localPosition = new Vector3(0, -1, 0);

                MainManager.Instance.playerBody.GetComponent<CapsuleCollider>().height = 1.75f;
                MainManager.Instance.playerBody.GetComponent<CapsuleCollider>().center = new Vector3(0f, 0f, 0f);

                PlayerManager.Instance.movementStates = MovementStates.Walking;
            }

            //Standing
            else
            {
                if (PlayerManager.Instance.movementStates != MovementStates.Standing)
                {
                    print("Standing");
                    PlayerManager.Instance.oldMovementStates = PlayerManager.Instance.movementStates;

                    if (PlayerManager.Instance.oldMovementStates == MovementStates.Crouching)
                    {
                        MainManager.Instance.playerBody.transform.localPosition = new Vector3(
                                                                                     MainManager.Instance.playerBody.transform.localPosition.x,
                                                                                     crouch_Up,
                                                                                     MainManager.Instance.playerBody.transform.localPosition.z);
                    }

                    MainManager.Instance.player.GetComponent<CharacterController>().height = 1.1f;

                    MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_0.transform.localPosition = new Vector3(0, -1, 0);
                    MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_1.transform.localPosition = new Vector3(0, -1, 0);
                    MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_2.transform.localPosition = new Vector3(0, -1, 0);
                    MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_3.transform.localPosition = new Vector3(0, -1, 0);
                    MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_4.transform.localPosition = new Vector3(0, -1, 0);

                    MainManager.Instance.playerBody.GetComponent<CapsuleCollider>().height = 1.75f;
                    MainManager.Instance.playerBody.GetComponent<CapsuleCollider>().center = new Vector3(0f, 0f, 0f);

                    movementAppearance_isHappening = true;
                }

                PlayerManager.Instance.movementStates = MovementStates.Standing;
            }
        }

        //If a menu is open
        else
        {
            print("Tablet");
            PlayerManager.Instance.oldMovementStates = PlayerManager.Instance.movementStates;

            MainManager.Instance.playerBody.transform.localPosition = new Vector3(
                                                                         MainManager.Instance.playerBody.transform.localPosition.x,
                                                                         crouch_Up,
                                                                         MainManager.Instance.playerBody.transform.localPosition.z);

            MainManager.Instance.player.GetComponent<CharacterController>().height = 1.1f;

            MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_0.transform.localPosition = new Vector3(0, -1, 0);
            MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_1.transform.localPosition = new Vector3(0, -1, 0);
            MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_2.transform.localPosition = new Vector3(0, -1, 0);
            MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_3.transform.localPosition = new Vector3(0, -1, 0);
            MainManager.Instance.player.GetComponent<DistanceAboveGround>().point_4.transform.localPosition = new Vector3(0, -1, 0);

            MainManager.Instance.playerBody.GetComponent<CapsuleCollider>().height = 1.75f;
            MainManager.Instance.playerBody.GetComponent<CapsuleCollider>().center = new Vector3(0f, 0f, 0f);

            movementAppearance_isHappening = true;

            PlayerManager.Instance.movementStates = MovementStates.Standing;
        }
        
        #endregion
    }

    void UpdateHealthValues()
    {
        switch (PlayerManager.Instance.movementStates)
        {
            case MovementStates.Standing:
                PlayerManager.Instance.hungerTemp = 0;
                PlayerManager.Instance.thirstTemp = 0;
                break;
            case MovementStates.Walking:
                PlayerManager.Instance.hungerTemp = -1;
                PlayerManager.Instance.thirstTemp = -1;
                break;
            case MovementStates.Running:
                PlayerManager.Instance.hungerTemp = -3;
                PlayerManager.Instance.thirstTemp = -2;
                break;
            case MovementStates.Crouching:
                PlayerManager.Instance.hungerTemp = 1;
                PlayerManager.Instance.thirstTemp = 1;
                break;
            case MovementStates.Jumping:
                PlayerManager.Instance.hungerTemp = -6;
                PlayerManager.Instance.thirstTemp = -3;
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

                //MainManager.Instance.player.GetComponent<CharacterController>().height = 1.1f;
                //MainManager.Instance.player.GetComponent<CharacterController>().center = new Vector3(0f, -0.85f, 0f);

                //MainManager.Instance.playerBody.GetComponent<CapsuleCollider>().height = 1.75f;
                //MainManager.Instance.playerBody.GetComponent<CapsuleCollider>().center = new Vector3(0f, 0f, 0f);

                SettingsManager.Instance.ChangeFOV(FOV_Smoother);
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

                //MainManager.Instance.player.GetComponent<CharacterController>().height = 1.1f;
                //MainManager.Instance.player.GetComponent<CharacterController>().center = new Vector3(0f, -0.85f, 0f);

                //MainManager.Instance.playerBody.GetComponent<CapsuleCollider>().height = 1.75f;
                //MainManager.Instance.playerBody.GetComponent<CapsuleCollider>().center = new Vector3(0f, 0f, 0f);

                SettingsManager.Instance.ChangeFOV(FOV_Smoother);
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

                //MainManager.Instance.player.GetComponent<CharacterController>().height = 1.1f;
                //MainManager.Instance.player.GetComponent<CharacterController>().center = new Vector3(0f, -0.85f, 0f);

                //MainManager.Instance.playerBody.GetComponent<CapsuleCollider>().height = 1.75f;
                //MainManager.Instance.playerBody.GetComponent<CapsuleCollider>().center = new Vector3(0f, 0f, 0f);

                SettingsManager.Instance.ChangeFOV(FOV_Smoother);
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

                //MainManager.Instance.player.GetComponent<CharacterController>().height = 0.5f;
                //MainManager.Instance.player.GetComponent<CharacterController>().center = new Vector3(0f, -0.4f, 0f);

                //MainManager.Instance.playerBody.GetComponent<CapsuleCollider>().height = 1.2f;
                //MainManager.Instance.playerBody.GetComponent<CapsuleCollider>().center = new Vector3(0f, 0.5f, 0f);

                SettingsManager.Instance.ChangeFOV(FOV_Smoother);
                movementSpeedVarianceByMovement = 0.5f;
                break;
            case MovementStates.Jumping:
                break;

            default:
                break;
        }
    }
}