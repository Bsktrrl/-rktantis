using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Singleton<PlayerMovement>
{
    public CharacterController controller;

    public float speed = 12f;
    public float speedMultiplier = 1f;

    public float jumpHeight = 3f;
    public float gravity = -9.81f;

    public float groundDistance = -1.4f;
    public LayerMask groundMask;

    Vector3 velocity;

    public SphereCollider groundCheck;
    public bool isGrounded;


    //--------------------


    private void Start()
    {
        //Set Player to the height it's supposed to be
        MainManager.Instance.playerBody.transform.SetPositionAndRotation(new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), transform.rotation);
    }
    void Update()
    {
        Movement();
    }


    //--------------------


    void Movement()
    {
        //checking if we hit the ground to reset our falling velocity, otherwise we will fall faster the next time
        //isGrounded = Physics.CheckSphere(groundCheck.center, 0.5f, groundMask);

        //if (isGrounded && velocity.y < 0)
        //{
        //    velocity.y = -2f;
        //}

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //right is the red Axis, forward is the blue axis
        Vector3 move = MainManager.Instance.playerBody.transform.right * x + MainManager.Instance.playerBody.transform.forward * z;

        controller.Move(move * speed * speedMultiplier * Time.deltaTime);

        //check if the player is on the ground so he can jump
        //if (Input.GetButtonDown("Jump") && isGrounded)
        //{
        //    //the equation for jumping
        //    velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        //}

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}