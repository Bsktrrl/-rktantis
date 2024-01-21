using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public AudioSource audioSource;

    //Menu
    [Header("Menu")]
    [SerializeField] AudioClip menu_SelectPanel_Clip;

    //Inventory
    [Header("Inventory")]
    [SerializeField] AudioClip menu_Select_Clip;
    [SerializeField] AudioClip menu_DropItem_Clip;
    [SerializeField] AudioClip menu_SortInventory_Clip;

    [SerializeField] AudioClip menu_AddItemToInevntory_Clip;
    [SerializeField] AudioClip menu_RemoveItemFromInevntory_Clip;
    [SerializeField] AudioClip menu_InventoryIsFull_Clip;

    //Crafting
    [Header("Crafting")]
    [SerializeField] AudioClip menu_ChangeCraftingScreen_Clip;
    [SerializeField] AudioClip menu_Crafting_Clip;
    [SerializeField] AudioClip menu_CannotCraft_Clip;

    //Building
    [Header("Building")]
    [SerializeField] AudioClip wood_Placed;
    [SerializeField] AudioClip stone_Placed;
    [SerializeField] AudioClip iron_Placed;
    [SerializeField] AudioClip wood_Removed;
    [SerializeField] AudioClip stone_Removed;
    [SerializeField] AudioClip iron_Removed;
    [SerializeField] AudioClip buildingBlock_CannotPlaceBlock;
    [SerializeField] AudioClip moveableObject_placed;


    //--------------------
    
    
    //Menu
    public void PlaySelectPanel_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = menu_SelectPanel_Clip;
            audioSource.volume = 0.50f;
            audioSource.Play();
        }
    }

    //inventory
    public void PlaySelect_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = menu_Select_Clip;
            audioSource.volume = 0.50f;
            audioSource.Play();
        }
    }
    public void PlayChangeCraftingScreen_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = menu_ChangeCraftingScreen_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void PlayDropItem_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = menu_DropItem_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void Playmenu_SortInventory_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = menu_SortInventory_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void Playmenu_AddItemToInevntory_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = menu_AddItemToInevntory_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void Playmenu_RemoveItemFromInevntory_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = menu_RemoveItemFromInevntory_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void Playmenu_InventoryIsFull_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = menu_InventoryIsFull_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }

    //Crafting
    public void Playmenu_Crafting_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = menu_Crafting_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void Playmenu_CanntoCraft_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = menu_CannotCraft_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }

    //Building
    public void PlayWood_Placed_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = wood_Placed;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void PlayStone_Placed_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = stone_Placed;
            audioSource.pitch = 0.7f;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void PlayIron_Placed_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = iron_Placed;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void PlayWood_Remove_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = wood_Removed;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void PlayStone_Remove_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = stone_Removed;
            audioSource.pitch = 0.7f;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void PlayIron_Remove_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = iron_Removed;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void PlaybuildingBlock_CannotPlaceBlock()
    {
        if (audioSource != null)
        {
            audioSource.clip = buildingBlock_CannotPlaceBlock;
            audioSource.pitch = 0.5f;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void PlayMoveableObject_Placed()
    {
        if (audioSource != null)
        {
            audioSource.clip = moveableObject_placed;
            audioSource.pitch = 1f;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    

}
