using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuSounds : Singleton<StartMenuSounds>
{
    [Header("AudioSource")]
    public AudioSource audioSource_Weather_Wind; //Starts on Awake
    public AudioSource audioSource_Weather_Night; //Starts on Awake
    public AudioSource audioSource_Menu_ItemHover;

    [Header("Weather Clip")]
    [SerializeField] AudioClip menu_ButtonHover_Clip;
    [SerializeField] AudioClip menu_ButtonPressed_Clip;
    [SerializeField] AudioClip ghost_Weather_Wind_Clip; 
    [SerializeField] AudioClip ghost_Weather_Night_Clip;

    [Header("Ghost Clip")]
    [SerializeField] AudioClip ghost_GhostAnimation_Spin1_Clip; 
    [SerializeField] AudioClip ghost_GhostAnimation_Spin2_Clip;
    [SerializeField] AudioClip ghost_GhostAnimation_Spin3_Clip; 
    [SerializeField] AudioClip ghost_GhostAnimation_Sneeze1_Clip; 
    [SerializeField] AudioClip ghost_GhostAnimation_Sneeze2_Clip;
    [SerializeField] AudioClip ghost_GhostAnimation_FakeDeath_Clip; 
    [SerializeField] AudioClip ghost_GhostAnimation_Wave_Clip;


    //--------------------


    public void Play_Inventory_ButtonHover_Clip()
    {
        if (audioSource_Menu_ItemHover != null)
        {
            audioSource_Menu_ItemHover.clip = menu_ButtonHover_Clip;
            audioSource_Menu_ItemHover.pitch = 20f;
            audioSource_Menu_ItemHover.Play();
        }
    }
    public void Play_Inventory_ButtonPressed_Clip()
    {
        if (audioSource_Menu_ItemHover != null)
        {
            audioSource_Menu_ItemHover.clip = menu_ButtonPressed_Clip;
            audioSource_Menu_ItemHover.pitch = 1f;
            audioSource_Menu_ItemHover.Play();
        }
    }

    public void Play_GhostAnimation_Spin1_Clip(AudioSource objSource)
    {
        if (objSource != null)
        {
            objSource.clip = ghost_GhostAnimation_Spin1_Clip;
            objSource.pitch = 1f;
            objSource.Play();
        }
    }
    public void Play_GhostAnimation_Spin2_Clip(AudioSource objSource)
    {
        if (objSource != null)
        {
            objSource.clip = ghost_GhostAnimation_Spin2_Clip;
            objSource.pitch = 1f;
            objSource.Play();
        }
    }
    public void Play_GhostAnimation_Spin3_Clip(AudioSource objSource)
    {
        if (objSource != null)
        {
            objSource.clip = ghost_GhostAnimation_Spin3_Clip;
            objSource.pitch = 1f;
            objSource.Play();
        }
    }
    public void Play_GhostAnimation_Sneeze1_Clip(AudioSource objSource)
    {
        if (objSource != null)
        {
            objSource.clip = ghost_GhostAnimation_Sneeze1_Clip;
            objSource.pitch = 1f;
            objSource.Play();
        }
    }
    public void Play_GhostAnimation_Sneeze2_Clip(AudioSource objSource)
    {
        if (objSource != null)
        {
            objSource.clip = ghost_GhostAnimation_Sneeze2_Clip;
            objSource.pitch = 1f;
            objSource.Play();
        }
    }
    public void Play_GhostAnimation_FakeDeath_Clip(AudioSource objSource)
    {
        if (objSource != null)
        {
            objSource.clip = ghost_GhostAnimation_FakeDeath_Clip;
            objSource.pitch = 1f;
            objSource.Play();
        }
    }
    public void Play_GhostAnimation_Wave_Clip(AudioSource objSource)
    {
        if (objSource != null)
        {
            objSource.clip = ghost_GhostAnimation_Wave_Clip;
            objSource.pitch = 1f;
            objSource.Play();
        }
    }
}
