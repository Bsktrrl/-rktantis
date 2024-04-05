using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Singleton<PlayerMovement>
{
    public CharacterController controller;

    public float movementSpeed = 4f;
    public float movementSpeedMultiplier = 1f;

    public float jumpHeight = 2f;
    public float gravity = -30f;

    Vector3 velocity;

    PlayerMovementStats playerMovementStats_ToSave;


    //--------------------


    private void Start()
    {
        //Set Player to the height it's supposed to be
        MainManager.Instance.playerBody.transform.SetPositionAndRotation(new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), transform.rotation);

        gravity = -30f;
        jumpHeight = 2f;
    }
    void Update()
    {
        Movement();
    }


    //--------------------


    public void LoadData()
    {

    }
    public void SaveData()
    {

    }


    //--------------------


    void Movement()
    {
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

        controller.Move(move * movementSpeed * movementSpeedMultiplier * Time.deltaTime);

        //check if the player is on the ground so he can jump
        if (Input.GetKeyDown(KeyCode.Space) && GetComponent<DistanceAboveGround>().isGrounded)
        {
            //the equation for jumping
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
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
    }
}

public class PlayerMovementStats
{
    public float movementSpeed;
    public float movementSpeedMultiplier;
}