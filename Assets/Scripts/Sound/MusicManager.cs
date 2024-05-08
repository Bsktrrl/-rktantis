using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [Header("Counters")]
    [SerializeField] float music_HomeBase_Counter = 0;
    [SerializeField] float music_Seclusion_Counter = 0;
    [SerializeField] float music_MysteriousSand_Counter = 0;

    [Header("Parameters")]
    [SerializeField] float music_Seclusion_FailureBuildup;
    [SerializeField] float timeSinceLastMusicPlayed;

    [Header("Time to reach before playing music")]
    [SerializeField] int timeForCountersToReach = 600;


    //--------------------


    private void Start()
    {
        timeForCountersToReach = 600;
    }
    private void Update()
    {
        //Play Music
        if (SoundManager.Instance.audioSource__Music_HomeBase.isPlaying
            || SoundManager.Instance.audioSource__Music_Seclusion.isPlaying
            || SoundManager.Instance.audioSource__Music_MysteriousSand.isPlaying)
        {
            timeSinceLastMusicPlayed = 0;
        }

        //Is not playing music
        else
        {
            RunMusicCounters();
        }
    }


    //--------------------


    void RunMusicCounters()
    {
        //Add time to counters
        #region
        timeSinceLastMusicPlayed += Time.deltaTime;
        #endregion

        //Check if Counters are reset
        #region

        //Music - HomeBase
        #region
        if (DistanceAboveGround.Instance.GroundTagLookingAt == "Ground_Wood"
            || DistanceAboveGround.Instance.GroundTagLookingAt == "Ground_Stone"
            || DistanceAboveGround.Instance.GroundTagLookingAt == "GroundCryonite")
        {
            music_HomeBase_Counter += Time.deltaTime;
        }
        #endregion

        //Music Seclusion
        #region
        if (DistanceAboveGround.Instance.GroundTagLookingAt == "Ground_Sand")
        {
            music_Seclusion_Counter += Time.deltaTime;

            music_Seclusion_FailureBuildup = 0;
        }
        else
        {
            music_Seclusion_FailureBuildup += Time.deltaTime;

            if (music_Seclusion_FailureBuildup > 5)
            {
                music_Seclusion_Counter = 0;
            }
        }
        #endregion

        //Music MysterySand
        #region
        if (timeSinceLastMusicPlayed > timeForCountersToReach && DistanceAboveGround.Instance.GroundTagLookingAt == "Ground_Ruin")
        {
            music_MysteriousSand_Counter = timeForCountersToReach;
        }
        #endregion

        #endregion

        //Check if timer is reached
        #region
        if (music_HomeBase_Counter >= timeForCountersToReach)
        {
            if (CheckIfMusicCanBePlayed())
            {
                print("1. Music - HomeBase is Playing");
                Play_HomeBase();
            }
            else
            {
                music_HomeBase_Counter = 0;
            }
        }
        else if (music_Seclusion_Counter >= timeForCountersToReach / 2)
        {
            if (CheckIfMusicCanBePlayed())
            {
                print("2. Music - Seclusion is Playing");

                Play_Seclusion();
            }
            else
            {
                music_Seclusion_Counter = 0;
            }
        }
        else if (music_MysteriousSand_Counter >= timeForCountersToReach)
        {
            if (CheckIfMusicCanBePlayed())
            {
                print("3. Music - MysteriousSand is Playing");

                Play_MysteriousSand();
            }
            else
            {
                music_MysteriousSand_Counter = 0;
            }
        }
        #endregion
    }


    //--------------------


    void Play_HomeBase()
    {
        if (!SoundManager.Instance.audioSource__Music_HomeBase.isPlaying)
        {
            SoundManager.Instance.Play_Music_HomeBase_Clip();

            ResetCounters();
        }
    }
    void Play_Seclusion()
    {
        if (!SoundManager.Instance.audioSource__Music_Seclusion.isPlaying)
        {
            SoundManager.Instance.Play_Music_Seclusion_Clip();

            ResetCounters();
        }
    }
    void Play_MysteriousSand()
    {
        if (!SoundManager.Instance.audioSource__Music_MysteriousSand.isPlaying)
        {
            SoundManager.Instance.Play_Music_MysteriousSand_Clip();

            ResetCounters();
        }
    }


    //--------------------


    bool CheckIfMusicCanBePlayed()
    {
        if (SoundManager.Instance.audioSource__Music_HomeBase.isPlaying
            || SoundManager.Instance.audioSource__Music_Seclusion.isPlaying
            || SoundManager.Instance.audioSource__Music_MysteriousSand.isPlaying)
        {
            return false;
        }

        return true;
    }
    void ResetCounters()
    {
        music_HomeBase_Counter = 0;
        music_Seclusion_Counter = 0;
        music_MysteriousSand_Counter = 0;
    }
}
