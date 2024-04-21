using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [Header("Stats")]
    public float InteractableDistance = 3.5f;
    public float movementSpeedMultiplier_SkillTree = 1f;

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


    //--------------------


    private void Update()
    {
        SaveData();
    }


    //--------------------


    public void LoadData()
    {
        if (DataManager.Instance.playerStats_Store.playerPos == Vector3.zero && DataManager.Instance.playerStats_Store.playerRot == Quaternion.identity
            && DataManager.Instance.playerStats_Store.InteractableDistance == 0
            && DataManager.Instance.playerStats_Store.movementSpeedMultiplier_SkillTree == 0
            && DataManager.Instance.playerStats_Store.jumpHeight == 0)
        {
            //Set Player Start Position - New Game
            MainManager.Instance.player.transform.SetPositionAndRotation(new Vector3(-26.5f, 29.9f, -45.1f), Quaternion.identity); //Change to Playtest

            //Also set StartDeathPos to catch if the player dies before building Floors
            UpdatePlayerDyingPos(MainManager.Instance.player.transform);
        }
        else
        {
            //Get loaded data
            playerStats = DataManager.Instance.playerStats_Store;

            //Set Player Position
            MainManager.Instance.player.transform.SetPositionAndRotation(playerStats.playerPos, playerStats.playerRot);

            if (playerStats.playerGameOverPos == Vector3.zero)
            {
                UpdatePlayerDyingPos(MainManager.Instance.player.transform);
            }

            movementSpeedMultiplier_SkillTree = playerStats.movementSpeedMultiplier_SkillTree;

            jumpHeight = playerStats.jumpHeight;
        }

        SaveData();
    }
    public void SaveData()
    {
        playerStats.playerPos = MainManager.Instance.player.transform.position;
        playerStats.playerRot = MainManager.Instance.player.transform.rotation;

        playerStats.movementSpeedMultiplier_SkillTree = movementSpeedMultiplier_SkillTree;
        playerStats.jumpHeight = jumpHeight;

        DataManager.Instance.playerStats_Store = playerStats;
    }
    public void SaveData(ref GameData gameData)
    {
        SaveData();

        print("Save_PlayerManager");
    }


    //--------------------


    public void TransferPlayerToDyingPos() //Upon GameOver
    {
        PlayerMovement.Instance.controller.Move(playerStats.playerGameOverPos - MainManager.Instance.player.transform.position);

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