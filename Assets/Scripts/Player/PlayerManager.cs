using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : Singleton<PlayerManager>
{
    [Header("Stats")]
    public float InteractableDistance = 3.5f;

    public float jumpHeight = 1.5f;

    public PlayerStats playerStats;

    [Header("Movement States")]
    public MovementStates oldMovementStates;
    public MovementStates movementStates;

    [Header("FOV values")]
    public float FOV_Addon;
    public float FOV_Standing;
    public float FOV_Walking;
    public float FOV_Running;
    public float FOV_Crouching;

    [Header("HealthParameterMovementVariables")]
    public int hungerTemp = 0;
    public int heatresistanceTemp = 0;
    public int thirstTemp = 0;

    public bool isHittingHeadRaycast = true;

    [Header("Player Movement Sounds")]
    public Vector3 player_PreviousPosition;
    public float movementSoundTimer;
    public bool hasStartedToMove = true;
    public bool isJumping;


    //--------------------


    private void Update()
    {
        if (!DataManager.Instance.hasLoaded) { return; }

        SaveData();

        PlayerWalkingSounds();
    }


    //--------------------


    public void LoadData()
    {
        //Get loaded data
        playerStats = DataManager.Instance.playerStats_Store;

        playerStats.playerPos = DataManager.Instance.playerStats_Store.playerPos;
        playerStats.playerRot = DataManager.Instance.playerStats_Store.playerRot;

        playerStats.playerGameOverPos = DataManager.Instance.playerStats_Store.playerGameOverPos;
        playerStats.playerGameOverRot = DataManager.Instance.playerStats_Store.playerGameOverRot;


        if (playerStats.playerGameOverPos == Vector3.zero)
        {
            //Set Player Start Position - New Game
            if (SceneManager.GetActiveScene().name == "Adrian")
            {
                MainManager.Instance.player.transform.SetPositionAndRotation(new Vector3(-12f, 2f, 6f), Quaternion.identity); //Main Scene
            }
            else if (SceneManager.GetActiveScene().name == "Landscape")
            {
                MainManager.Instance.player.transform.SetPositionAndRotation(new Vector3(-26.5f, 30f, -45.1f), Quaternion.identity); //Main Scene
            }
            
            //MainManager.Instance.player.transform.SetPositionAndRotation(new Vector3(17f, 2f, 25f), Quaternion.identity); //Adrian Secene

            //Also set StartDeathPos to catch if the player dies before building Floors
            UpdatePlayerDyingPos(MainManager.Instance.player.transform);
        }
        else
        {
            //Set Player Position
            MainManager.Instance.player.transform.SetPositionAndRotation(playerStats.playerPos, playerStats.playerRot);

            //if (playerStats.playerGameOverPos == Vector3.zero)
            //{
            //    print("3. Start - " + playerStats.playerGameOverPos);
            //    UpdatePlayerDyingPos(MainManager.Instance.player.transform);
            //}

            jumpHeight = playerStats.jumpHeight;
        }

        SaveData();
    }
    public void SaveData()
    {
        playerStats.playerPos = MainManager.Instance.player.transform.position;
        playerStats.playerRot = MainManager.Instance.player.transform.rotation;

        playerStats.jumpHeight = jumpHeight;

        DataManager.Instance.playerStats_Store = playerStats;
    }
    public void SaveData(ref GameData gameData)
    {
        SaveData();

        print("Save_PlayerManager");
    }


    //--------------------


    void PlayerWalkingSounds()
    {
        Vector3 currentPosition = transform.position;

        //If in the air
        if (DistanceAboveGround.Instance.GroundLookingAt == null)
        {
            StopWaterMovementSound();

            hasStartedToMove = false;
            isJumping = true;
        }

        //If landing
        else if (DistanceAboveGround.Instance.GroundLookingAt && isJumping)
        {
            isJumping = false;
            StartMovementSound();
        }

        //If on the ground
        else if (currentPosition.x != player_PreviousPosition.x && currentPosition.z != player_PreviousPosition.z)
        {
            //Moving outside of water
            if (PlayerMovement.Instance.movementSpeedVarianceByWater >= 1)
            {
                StopWaterMovementSound();
            }

            if (!hasStartedToMove)
            {
                hasStartedToMove = true;

                StartMovementSound();
                movementSoundTimer = 0;
            }
            
            else if (movementStates == MovementStates.Walking)
            {
                movementSoundTimer += Time.deltaTime;

                if (movementSoundTimer > 0.65f)
                {
                    movementSoundTimer = 0;

                    StartMovementSound();
                }
            }
            else if (movementStates == MovementStates.Crouching)
            {
                movementSoundTimer += Time.deltaTime;

                if (movementSoundTimer > 0.9f)
                {
                    movementSoundTimer = 0;

                    StartMovementSound();
                }
            }
            else if (movementStates == MovementStates.Running)
            {
                movementSoundTimer += Time.deltaTime;

                if (movementSoundTimer > 0.4f)
                {
                    movementSoundTimer = 0;

                    StartMovementSound();
                }
            }
            else
            {
                movementSoundTimer = 0;
                StopWaterMovementSound();
            }
        }

        else
        {
            StopWaterMovementSound();

            hasStartedToMove = false;
        }

        player_PreviousPosition = MainManager.Instance.player.transform.position;
    }
    public void StartMovementSound()
    {
        if (!SoundManager.Instance.audioSource_PlayerMovement_WalkingRunning.isPlaying)
        {
            //Water
            if (PlayerMovement.Instance.movementSpeedVarianceByWater < 1)
            {
                if (!SoundManager.Instance.audioSource_PlayerMovement_WalkingRunning_Water.isPlaying)
                {
                    SoundManager.Instance.Play_Player_Walking_Water_Clip();
                }
            }

            //Other
            else if (DistanceAboveGround.Instance.GroundTagLookingAt == "Ground_Sand")
            {
                SoundManager.Instance.Play_Player_Walking_Sand_Clip();
            }
            else if (DistanceAboveGround.Instance.GroundTagLookingAt == "Ground_Ruin")
            {
                SoundManager.Instance.Play_Player_Walking_Ruin_Clip();
            }

            else if (DistanceAboveGround.Instance.GroundTagLookingAt == "Ground_Wood")
            {
                SoundManager.Instance.Play_Player_Walking_Wood_Clip();
            }
            else if (DistanceAboveGround.Instance.GroundTagLookingAt == "Ground_Stone")
            {
                SoundManager.Instance.Play_Player_Walking_Stone_Clip();
            }
            else if (DistanceAboveGround.Instance.GroundTagLookingAt == "GroundCryonite")
            {
                SoundManager.Instance.Play_Player_Walking_Cryonite_Clip();
            }
        }
    }
    void StopWaterMovementSound()
    {
        SoundManager.Instance.Stop_Player_Walking_Water_Clip();
    }


    //--------------------


    public void TransferPlayerToDyingPos() //Upon GameOver
    {
        PlayerMovement.Instance.Teleport(playerStats.playerGameOverPos);

        //Vector3 targetPosition = playerStats.playerGameOverPos;

        //// Teleport the object to the target position
        //MainManager.Instance.player.transform.position = targetPosition;

        //MainManager.Instance.player.transform.SetPositionAndRotation(playerStats.playerGameOverPos, Quaternion.identity);
        //PlayerMovement.Instance.controller.Move(playerStats.playerGameOverPos - MainManager.Instance.player.transform.position);

        SaveData();
    }
    public void UpdatePlayerDyingPos(Transform obj) //When entering a new BuildingBlockFloor
    {
        playerStats.playerGameOverPos = obj.position;
        playerStats.playerGameOverRot = MainManager.Instance.playerBody.transform.rotation;

        SaveData();
    }
}

[Serializable]
public class PlayerStats
{
    public Vector3 playerPos;
    public Quaternion playerRot;

    public Vector3 playerGameOverPos;
    public Quaternion playerGameOverRot;

    public float jumpHeight;

    public float InteractableDistance;
    public float movementSpeedMultiplier_SkillTree;
}

public enum MovementStates
{
    Standing,

    Walking,
    Running,
    Crouching,
    Jumping
}