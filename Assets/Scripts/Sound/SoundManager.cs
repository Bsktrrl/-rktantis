using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public AudioSource audioSource_WorldSound; //World Sound Source
    public AudioSource audioSource_MenuSound; //Menu Sound Source
    public AudioSource audioSource_MenuSound2; //Menu Sound Source
    public AudioSource audioSourceWeatherSound; //Weather Sound Source

    public AudioSource audioSource_Music; //Music Source

    public AudioSource audioSource_VoiceMessages; //Voice Source


    //--------------------


    #region Clips

    #region Player
    [Header("Player")]
    [SerializeField] AudioClip player_Walking_Sand_Clip; //
    [SerializeField] AudioClip player_Walking_Grass_Clip; //
    [SerializeField] AudioClip player_Walking_Water_Clip; //
    [SerializeField] AudioClip player_Walking_Stone_Clip; //
    [SerializeField] AudioClip player_Walking_Wood_Clip; //
    [SerializeField] AudioClip player_Walking_Cryonite_Clip; //

    [SerializeField] AudioClip player_Sprinting_Sand_Clip; //
    [SerializeField] AudioClip player_Sprinting_Grass_Clip; //
    [SerializeField] AudioClip player_Sprinting_Water_Clip; //
    [SerializeField] AudioClip player_Sprinting_Stone_Clip; //
    [SerializeField] AudioClip player_Sprinting_Wood_Clip; //
    [SerializeField] AudioClip player_Sprinting_Cryonite_Clip; //
    #endregion
    #region Tablet
    [Header("Tablet")]
    [SerializeField] AudioClip tablet_OpenTablet_Clip; //
    [SerializeField] AudioClip tablet_CloseTablet_Clip; //
    [SerializeField] AudioClip tablet_ChangeMenu_Clip;
    #endregion
    #region Inventory
    [Header("Inventory")]
    [SerializeField] AudioClip inventory_ItemHover_Clip;

    [SerializeField] AudioClip inventory_PickupItem_Clip;
    [SerializeField] AudioClip inventory_DropItem_Clip;
    [SerializeField] AudioClip inventory_MoveItem_Clip; //
    [SerializeField] AudioClip inventory_ConsumeItem_Clip; //
    [SerializeField] AudioClip inventory_DrinkItem_Clip; //
    [SerializeField] AudioClip inventory_DrinkEmptyItem_Clip; //
    [SerializeField] AudioClip inventory_RefillDrink_Clip; //
    [SerializeField] AudioClip inventory_EquipItem_Clip; //

    [SerializeField] AudioClip inventory_InventoryIsFull_Clip; //
    #endregion
    #region Hotbar
    [Header("Hotbar")]
    [SerializeField] AudioClip hotbar_ChangeSelectedItem_Clip; //
    [SerializeField] AudioClip hotbar_AssignItemToHotbar_Clip; //
    [SerializeField] AudioClip hotbar_RemoveItemFromHotbar_Clip; //
    #endregion
    #region Crafting
    [Header("Crafting")]
    [SerializeField] AudioClip crafting_ChangeCraftingMenu_Clip; //
    [SerializeField] AudioClip crafting_SelectCraftingItem_Clip; //
    [SerializeField] AudioClip crafting_PerformCrafting_Clip;
    [SerializeField] AudioClip crafting_CannotCraft_Clip; //
    #endregion
    #region Skill Tree
    [Header("Skill Tree")]
    [SerializeField] AudioClip skillTree_HoverPerk_Clip;
    [SerializeField] AudioClip skillTree_CompletedPerk_Clip;
    #endregion
    #region EquippedItems
    [Header("Equipped Items")]
    [SerializeField] AudioClip equippedItems_EquippedItemIsBroken_Clip; //
    #endregion
    #region Axe Usage
    [Header("Axe Useage")]
    [SerializeField] AudioClip axeUsage_Hand_Clip; //
    [SerializeField] AudioClip axeUsage_WoodAxe_Clip; //
    [SerializeField] AudioClip axeUsage_StoneAxe_Clip; //
    [SerializeField] AudioClip axeUsage_CryoniteAxe_Clip; //

    [SerializeField] AudioClip axeUsage_TreeFalling_Clip; //
    [SerializeField] AudioClip axeUsage_CactusFalling_Clip; //
    [SerializeField] AudioClip axeUsage_CannotHit_Clip; //
    #endregion
    #region Pickaxe Usage
    [Header("Pickaxe Useage")]
    [SerializeField] AudioClip pickaxeUsage_Hand_Clip; //
    [SerializeField] AudioClip pickaxeUsage_WoodPickaxe_Clip; //
    [SerializeField] AudioClip pickaxeUsage_StonePickaxe_Clip; //
    [SerializeField] AudioClip pickaxeUsage_CryonitePickaxe_Clip; //

    [SerializeField] AudioClip pickaxeUsage_OreIsDestroid_Clip; //
    [SerializeField] AudioClip pickaxeUsage_CannotHit_Clip; //
    #endregion
    #region Sword Usage
    [Header("Sword Usage")]
    [SerializeField] AudioClip swordUsage_Slashing_Clip; //
    #endregion
    #region Building
    [Header("Building")]
    [SerializeField] AudioClip building_Place_Wood;
    [SerializeField] AudioClip building_Place_Stone;
    [SerializeField] AudioClip building_Place_Cryonite;

    [SerializeField] AudioClip building_Remove_Wood;
    [SerializeField] AudioClip building_Remove_Stone;
    [SerializeField] AudioClip building_Remove_Cryonite;

    [SerializeField] AudioClip building_CannotPlaceBlock;
    [SerializeField] AudioClip building_Place_MoveableObject; //
    [SerializeField] AudioClip building_Remove_MoveableObject; //
    #endregion
    #region Chests
    [Header("Chests")]
    [SerializeField] AudioClip chests_OpenSmallChest_Clip;
    [SerializeField] AudioClip chests_CloseSmallChest_Clip;

    [SerializeField] AudioClip chests_OpenMediumChest_Clip; //
    [SerializeField] AudioClip chests_CloseMediumChest_Clip; //

    [SerializeField] AudioClip chests_OpenBigChest_Clip;
    [SerializeField] AudioClip chests_CloseBigChest_Clip;
    #endregion
    #region InteractableObjects
    [Header("InteractableObjects")]
    //Crafting Table
    [SerializeField] AudioClip InteractableObjects_OpenCraftingTable_Clip; //
    [SerializeField] AudioClip InteractableObjects_UngoingCraftingTable_Clip; //
    [SerializeField] AudioClip InteractableObjects_CloseCraftingTable_Clip; //

    //Research Table
    [SerializeField] AudioClip InteractableObjects_OpenResearchTable_Clip; //
    [SerializeField] AudioClip InteractableObjects_UngoingResearchTable_Clip; //
    [SerializeField] AudioClip InteractableObjects_CloseResearchTable_Clip; //
    [SerializeField] AudioClip InteractableObjects_ResearchTable_Researching_Clip; //

    //Skill Tree Table
    [SerializeField] AudioClip InteractableObjects_OpenSkillTreeTable_Clip; //
    [SerializeField] AudioClip InteractableObjects_UngoingSkillTreeTable_Clip; //
    [SerializeField] AudioClip InteractableObjects_CloseSkillTreeTable_Clip; //

    //Crop Plots
    [SerializeField] AudioClip InteractableObjects_OpenCropPlot_Clip; //
    [SerializeField] AudioClip InteractableObjects_UngoingCropPlot_Clip; //
    [SerializeField] AudioClip InteractableObjects_CloseCropPlot_Clip; //

    //Ghost Tank
    [SerializeField] AudioClip InteractableObjects_UngoingGhostTank_Clip; //

    //Extractor
    [SerializeField] AudioClip InteractableObjects_UngoingExtractor_Clip; //

    //Lamp
    [SerializeField] AudioClip InteractableObjects_UngoingLamp_Clip; //

    //Spotlight
    [SerializeField] AudioClip InteractableObjects_UngoingSpotlight_Clip; //
    #endregion
    #region Buffs
    [Header("Buffs")]
    [SerializeField] AudioClip buff_Deactivated; //
    #endregion
    #region Journal Pages
    [Header("Journal Pages")]
    [SerializeField] AudioClip journalPage_GetNewJournalPage; //
    [SerializeField] AudioClip journalPage_SelectingJournalPage; //
    #endregion
    #region Research
    [Header("Research")]
    [SerializeField] AudioClip research_Ongoing; //
    [SerializeField] AudioClip research_Complete; //
    #endregion

    #endregion


    //--------------------


    private void Update()
    {
        audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
        audioSource_MenuSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_MenuSFX();

        audioSource_Music.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_Music();

        audioSourceWeatherSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WeatherSFX();

        audioSource_VoiceMessages.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_Voice();
    }


    //--------------------


    #region Stop Clip from playing
    public void Stop_WorldSound()
    {
        audioSource_WorldSound.Stop();
    }
    public void Stop_MenuSound()
    {
        audioSource_MenuSound.Stop();
    }
    public void Stop_WeatherSound()
    {
        audioSourceWeatherSound.Stop();
    }
    public void Stop_Music()
    {
        audioSource_Music.Stop();
    }
    public void Stop_Voice()
    {
        audioSource_VoiceMessages.Stop();
    }
    #endregion


    //--------------------


    #region Player //Have yet to be implemented
    public void Play_Player_Walking_Sand_Clip()
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = player_Walking_Sand_Clip;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = 1f;
            audioSource_WorldSound.Play();
        }
    }
    public void Play_Player_Walking_Grass_Clip()
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = player_Walking_Grass_Clip;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = 1f;
            audioSource_WorldSound.Play();
        }
    }
    public void Play_Player_Walking_Water_Clip()
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = player_Walking_Water_Clip;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = 1f;
            audioSource_WorldSound.Play();
        }
    }
    public void Play_Player_Walking_Stone_Clip()
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = player_Walking_Stone_Clip;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = 1f;
            audioSource_WorldSound.Play();
        }
    }
    public void Play_Player_Walking_Wood_Clip()
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = player_Walking_Wood_Clip;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = 1f;
            audioSource_WorldSound.Play();
        }
    }
    public void Play_Player_Walking_Cryonite_Clip()
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = player_Walking_Cryonite_Clip;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = 1f;
            audioSource_WorldSound.Play();
        }
    }

    public void Play_Player_Sprinting_Sand_Clip()
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = player_Sprinting_Sand_Clip;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = 1f;
            audioSource_WorldSound.Play();
        }
    }
    public void Play_Player_Sprinting_Grass_Clip()
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = player_Sprinting_Grass_Clip;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = 1f;
            audioSource_WorldSound.Play();
        }
    }
    public void Play_Player_Sprinting_Water_Clip()
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = player_Sprinting_Water_Clip;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = 1f;
            audioSource_WorldSound.Play();
        }
    }
    public void Play_Player_Sprinting_Stone_Clip()
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = player_Sprinting_Stone_Clip;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = 1f;
            audioSource_WorldSound.Play();
        }
    }
    public void Play_Player_Sprinting_Wood_Clip()
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = player_Sprinting_Wood_Clip;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = 1f;
            audioSource_WorldSound.Play();
        }
    }
    public void Play_Player_Sprinting_Cryonite_Clip()
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = player_Sprinting_Cryonite_Clip;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = 1f;
            audioSource_WorldSound.Play();
        }
    }
    #endregion

    #region Tablet
    public void Play_Tablet_OpenTablet_Clip()
    {
        if (audioSource_MenuSound != null)
        {
            audioSource_MenuSound.clip = tablet_OpenTablet_Clip;
            audioSource_MenuSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_MenuSFX();
            audioSource_MenuSound.pitch = 1f;
            audioSource_MenuSound.Play();
        }
    }
    public void Play_Tablet_CloseTablet_Clip()
    {
        if (audioSource_MenuSound != null)
        {
            audioSource_MenuSound.clip = tablet_CloseTablet_Clip;
            audioSource_MenuSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_MenuSFX();
            audioSource_MenuSound.pitch = 1f;
            audioSource_MenuSound.Play();
        }
    }
    public void Play_Tablet_ChangeMenu_Clip()
    {
        if (audioSource_MenuSound != null)
        {
            audioSource_MenuSound.clip = tablet_ChangeMenu_Clip;
            audioSource_MenuSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_MenuSFX();
            audioSource_MenuSound.pitch = 1f;
            audioSource_MenuSound.Play();
        }
    }
    #endregion

    #region Inventory
    public void Play_Inventory_ItemHover_Clip()
    {
        if (audioSource_MenuSound != null)
        {
            audioSource_MenuSound.clip = inventory_ItemHover_Clip;
            audioSource_MenuSound.volume = 0.25f * SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_MenuSFX(); ;
            audioSource_MenuSound.pitch = 20f;
            audioSource_MenuSound.Play();
        }
    }
    public void Play_Inventory_PickupItem_Clip()
    {
        if (audioSource_MenuSound != null)
        {
            audioSource_MenuSound.clip = inventory_PickupItem_Clip;
            audioSource_MenuSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_MenuSFX();
            audioSource_MenuSound.pitch = 1f;
            audioSource_MenuSound.Play();
        }
    }
    public void Play_Inventory_DropItem_Clip()
    {
        if (audioSource_MenuSound != null)
        {
            audioSource_MenuSound.clip = inventory_DropItem_Clip;
            audioSource_MenuSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_MenuSFX();
            audioSource_MenuSound.pitch = 1f;
            audioSource_MenuSound.Play();
        }
    }
    public void Play_Inventory_MoveItem_Clip()
    {
        if (audioSource_MenuSound != null)
        {
            audioSource_MenuSound.clip = inventory_MoveItem_Clip;
            audioSource_MenuSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_MenuSFX();
            audioSource_MenuSound.pitch = 1f;
            audioSource_MenuSound.Play();
        }
    }
    public void Play_Inventory_ConsumeItem_Clip()
    {
        if (audioSource_MenuSound != null)
        {
            audioSource_MenuSound.clip = inventory_ConsumeItem_Clip;
            audioSource_MenuSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_MenuSFX();
            audioSource_MenuSound.pitch = 1f;
            audioSource_MenuSound.Play();
        }
    }
    public void Play_Inventory_DrinkItem_Clip()
    {
        if (audioSource_MenuSound != null)
        {
            audioSource_MenuSound.clip = inventory_DrinkItem_Clip;
            audioSource_MenuSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_MenuSFX();
            audioSource_MenuSound.pitch = 1f;
            audioSource_MenuSound.Play();
        }
    }
    public void Play_Inventory_DrinkEmptyItem_Clip()
    {
        if (audioSource_MenuSound != null)
        {
            audioSource_MenuSound.clip = inventory_DrinkEmptyItem_Clip;
            audioSource_MenuSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_MenuSFX();
            audioSource_MenuSound.pitch = 1f;
            audioSource_MenuSound.Play();
        }
    }
    public void Play_Inventory_RefillDrink_Clip()
    {
        if (audioSource_MenuSound != null)
        {
            audioSource_MenuSound.clip = inventory_RefillDrink_Clip;
            audioSource_MenuSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_MenuSFX();
            audioSource_MenuSound.pitch = 1f;
            audioSource_MenuSound.Play();
        }
    }
    public void Play_Inventory_EquipItem_Clip()
    {
        if (audioSource_MenuSound != null)
        {
            audioSource_MenuSound.clip = inventory_EquipItem_Clip;
            audioSource_MenuSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_MenuSFX();
            audioSource_MenuSound.pitch = 1f;
            audioSource_MenuSound.Play();
        }
    }

    public void Play_Inventory_InventoryIsFull_Clip()
    {
        if (audioSource_MenuSound != null)
        {
            audioSource_MenuSound.clip = inventory_InventoryIsFull_Clip;
            audioSource_MenuSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_MenuSFX();
            audioSource_MenuSound.pitch = 1f;
            audioSource_MenuSound.Play();
        }
    }
    #endregion

    #region Hotbar
    public void Play_Hotbar_ChangeSelectedItem_Clip()
    {
        if (audioSource_MenuSound != null)
        {
            audioSource_MenuSound.clip = hotbar_ChangeSelectedItem_Clip;
            audioSource_MenuSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_MenuSFX();
            audioSource_MenuSound.pitch = 1f;
            audioSource_MenuSound.Play();
        }
    }
    public void Play_Hotbar_AssignItemToHotbar_Clip()
    {
        if (audioSource_MenuSound != null)
        {
            audioSource_MenuSound.clip = hotbar_AssignItemToHotbar_Clip;
            audioSource_MenuSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_MenuSFX();
            audioSource_MenuSound.pitch = 1f;
            audioSource_MenuSound.Play();
        }
    }
    public void Play_Hotbar_RemoveItemFromHotbar_Clip()
    {
        if (audioSource_MenuSound != null)
        {
            audioSource_MenuSound.clip = hotbar_RemoveItemFromHotbar_Clip;
            audioSource_MenuSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_MenuSFX();
            audioSource_MenuSound.pitch = 1f;
            audioSource_MenuSound.Play();
        }
    }
    #endregion

    #region Crafting
    public void Play_Crafting_SelectCraftingItem_Clip()
    {
        if (audioSource_MenuSound != null)
        {
            audioSource_MenuSound.clip = crafting_ChangeCraftingMenu_Clip;
            audioSource_MenuSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_MenuSFX();
            audioSource_MenuSound.pitch = 1f;
            audioSource_MenuSound.Play();
        }
    }
    public void Play_Crafting_ChangeCraftingMenu_Clip()
    {
        if (audioSource_MenuSound != null)
        {
            audioSource_MenuSound.clip = crafting_SelectCraftingItem_Clip;
            audioSource_MenuSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_MenuSFX();
            audioSource_MenuSound.pitch = 1f;
            audioSource_MenuSound.Play();
        }
    }
    public void Play_Crafting_PerformCrafting_Clip()
    {
        if (audioSource_MenuSound != null)
        {
            audioSource_MenuSound.clip = crafting_PerformCrafting_Clip;
            audioSource_MenuSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_MenuSFX();
            audioSource_MenuSound.pitch = 1f;
            audioSource_MenuSound.Play();
        }
    }
    public void Play_Crafting_CannotCraft_Clip()
    {
        if (audioSource_MenuSound != null)
        {
            audioSource_MenuSound.clip = crafting_CannotCraft_Clip;
            audioSource_MenuSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_MenuSFX();
            audioSource_MenuSound.pitch = 1f;
            audioSource_MenuSound.Play();
        }
    }
    #endregion

    #region Skill Tree
    public void Play_SkillTree_HoverPerk_Clip()
    {
        if (audioSource_MenuSound != null)
        {
            audioSource_MenuSound.clip = skillTree_HoverPerk_Clip;
            audioSource_MenuSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_MenuSFX();
            audioSource_MenuSound.pitch = 1f;
            audioSource_MenuSound.Play();
        }
    }
    public void Play_SkillTree_CompletedPerk_Clip()
    {
        if (audioSource_MenuSound != null)
        {
            audioSource_MenuSound.clip = skillTree_CompletedPerk_Clip;
            audioSource_MenuSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_MenuSFX();
            audioSource_MenuSound.pitch = 1f;
            audioSource_MenuSound.Play();
        }
    }
    #endregion

    #region Equipped Item
    public void Play_EquippedItems_EquippedItemIsBroken_Clip()
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = equippedItems_EquippedItemIsBroken_Clip;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = 1f;
            audioSource_WorldSound.Play();
        }
    }
    #endregion

    #region Axe Useage //Have yet to be implemented
    public void Play_AxeUsage_Hand_Clip(float pitch)
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = axeUsage_Hand_Clip;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = pitch;
            audioSource_WorldSound.Play();
        }
    }
    public void Play_AxeUsage_WoodAxe_Clip(float pitch)
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = axeUsage_WoodAxe_Clip;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = pitch;
            audioSource_WorldSound.Play();
        }
    }
    public void Play_AxeUsage_StoneAxe_Clip(float pitch)
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = axeUsage_StoneAxe_Clip;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = pitch;
            audioSource_WorldSound.Play();
        }
    }
    public void Play_AxeUsage_CryoniteAxe_Clip(float pitch)
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = axeUsage_CryoniteAxe_Clip;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = pitch;
            audioSource_WorldSound.Play();
        }
    }

    public void Play_AxeUsage_TreeFalling_Clip()
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = axeUsage_TreeFalling_Clip;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = 1f;
            audioSource_WorldSound.Play();
        }
    }
    public void Play_AxeUsage_CactusFalling_Clip()
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = axeUsage_CactusFalling_Clip;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = 1f;
            audioSource_WorldSound.Play();
        }
    }
    public void Play_AxeUsage_CannotHit_Clip()
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = axeUsage_CannotHit_Clip;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = 1f;
            audioSource_WorldSound.Play();
        }
    }
    #endregion

    #region Pickaxe Useage
    public void Play_PickaxeUsage_Hand_Clip(float pitch)
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = pickaxeUsage_Hand_Clip;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = pitch;
            audioSource_WorldSound.Play();
        }
    }
    public void Play_PickaxeUsage_WoodPickaxe_Clip(float pitch)
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = pickaxeUsage_WoodPickaxe_Clip;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = pitch;
            audioSource_WorldSound.Play();
        }
    }
    public void Play_PickaxeUsage_StonePickaxe_Clip(float pitch)
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = pickaxeUsage_StonePickaxe_Clip;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = pitch;
            audioSource_WorldSound.Play();
        }
    }
    public void Play_PickaxeUsage_CryonitePickaxe_Clip(float pitch)
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = pickaxeUsage_CryonitePickaxe_Clip;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = pitch;
            audioSource_WorldSound.Play();
        }
    }

    public void Play_PickaxeUsage_OreIsDestroid_Clip()
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = pickaxeUsage_OreIsDestroid_Clip;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = 1f;
            audioSource_WorldSound.Play();
        }
    }
    public void Play_PickaxeUsage_CannotHit_Clip()
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = pickaxeUsage_CannotHit_Clip;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = 1f;
            audioSource_WorldSound.Play();
        }
    }
    #endregion

    #region Sword Usage //Have yet to be implemented
    public void Play_SwordUsage_Slashing_Clip()
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = swordUsage_Slashing_Clip;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = 1f;
            audioSource_WorldSound.Play();
        }
    }
    #endregion

    #region Building
    public void Play_Building_Place_Wood_Clip()
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = building_Place_Wood;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = 1f;
            audioSource_WorldSound.Play();
        }
    }
    public void Play_Building_Place_Stone_Clip()
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = building_Place_Stone;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = 0.7f;
            audioSource_WorldSound.Play();
        }
    }
    public void Play_Building_Place_Cryonite_Clip()
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = building_Place_Cryonite;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = 1f;
            audioSource_WorldSound.Play();
        }
    }
    
    public void Play_Building_Remove_Wood_Clip()
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = building_Remove_Wood;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = 1f;
            audioSource_WorldSound.Play();
        }
    }
    public void Play_Building_Remove_Stone_Clip()
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = building_Remove_Stone;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = 0.7f;
            audioSource_WorldSound.Play();
        }
    }
    public void Play_Building_Remove_Cryonite_Clip()
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = building_Remove_Cryonite;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = 1f;
            audioSource_WorldSound.Play();
        }
    }

    public void Play_Building_CannotPlaceBlock_Clip()
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = building_CannotPlaceBlock;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = 0.5f;
            audioSource_WorldSound.Play();
        }
    }
    public void Play_Building_Place_MoveableObject_Clip()
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = building_Place_MoveableObject;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = 1f;
            audioSource_WorldSound.Play();
        }
    }
    public void Play_Building_Remove_MoveableObject_Clip()
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = building_Remove_MoveableObject;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = 1f;
            audioSource_WorldSound.Play();
        }
    }
    #endregion

    #region Chests
    public void Play_Chests_OpenSmallChest_Clip()
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = chests_OpenSmallChest_Clip;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = 1f;
            audioSource_WorldSound.Play();
        }
    }
    public void Play_Chests_CloseSmallChest_Clip()
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = chests_CloseSmallChest_Clip;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX(); ;
            audioSource_WorldSound.pitch = 1f;
            audioSource_WorldSound.Play();
        }
    }

    public void Play_Chests_OpenMediumChest_Clip()
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = chests_OpenMediumChest_Clip;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = 1f;
            audioSource_WorldSound.Play();
        }
    }
    public void Play_Chests_CloseMediumChest_Clip()
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = chests_CloseMediumChest_Clip;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = 1f;
            audioSource_WorldSound.Play();
        }
    }

    public void Play_Chests_OpenBigChest_Clip()
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = chests_OpenBigChest_Clip;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = 1f;
            audioSource_WorldSound.Play();
        }
    }
    public void Play_Chests_CloseBigChest_Clip()
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = chests_CloseBigChest_Clip;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = 1f;
            audioSource_WorldSound.Play();
        }
    }
    #endregion

    #region InteractableObjects //"Ungoing" have yet to be implemented
    #region Crafting Table
    public void Play_InteractableObjects_OpenCraftingTable_Clip()
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = InteractableObjects_OpenCraftingTable_Clip;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = 1f;
            audioSource_WorldSound.Play();
        }
    }
    public void Play_InteractableObjects_UngoingCraftingTable_Clip()
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = InteractableObjects_UngoingCraftingTable_Clip;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = 1f;
            audioSource_WorldSound.Play();
        }
    }
    public void Play_InteractableObjects_CloseCraftingTable_Clip()
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = InteractableObjects_CloseCraftingTable_Clip;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = 1f;
            audioSource_WorldSound.Play();
        }
    }
    #endregion

    #region Research Table
    public void Play_InteractableObjects_OpenResearchTable_Clip()
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = InteractableObjects_OpenResearchTable_Clip;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = 1f;
            audioSource_WorldSound.Play();
        }
    }
    public void Play_InteractableObjects_UngoingResearchTable_Clip()
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = InteractableObjects_UngoingResearchTable_Clip;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = 1f;
            audioSource_WorldSound.Play();
        }
    }
    public void Play_InteractableObjects_CloseResearchTable_Clip()
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = InteractableObjects_CloseResearchTable_Clip;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = 1f;
            audioSource_WorldSound.Play();
        }
    }
    public void Play_ResearchTable_Researching_Clip()
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = InteractableObjects_ResearchTable_Researching_Clip;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = 1f;
            audioSource_WorldSound.Play();
        }
    }
    #endregion

    #region Skill Tree Table
    public void Play_InteractableObjects_OpenSkillTreeTable_Clip()
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = InteractableObjects_OpenSkillTreeTable_Clip;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = 1f;
            audioSource_WorldSound.Play();
        }
    }
    public void Play_InteractableObjects_UngoingSkillTreeTable_Clip()
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = InteractableObjects_UngoingSkillTreeTable_Clip;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = 1f;
            audioSource_WorldSound.Play();
        }
    }
    public void Play_InteractableObjects_CloseSkillTreeTable_Clip()
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = InteractableObjects_CloseSkillTreeTable_Clip;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = 1f;
            audioSource_WorldSound.Play();
        }
    }
    #endregion

    #region Crop Plot
    public void Play_InteractableObjects_OpenCropPlot_Clip()
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = InteractableObjects_OpenCropPlot_Clip;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = 1f;
            audioSource_WorldSound.Play();
        }
    }
    public void Play_InteractableObjects_UngoingCropPlot_Clip()
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = InteractableObjects_UngoingCropPlot_Clip;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = 1f;
            audioSource_WorldSound.Play();
        }
    }
    public void Play_InteractableObjects_CloseCropPlot_Clip()
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = InteractableObjects_CloseCropPlot_Clip;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = 1f;
            audioSource_WorldSound.Play();
        }
    }
    #endregion

    #region Ghost Tank
    public void Play_InteractableObjects_UngoingGhostTank_Clip()
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = InteractableObjects_UngoingGhostTank_Clip;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = 1f;
            audioSource_WorldSound.Play();
        }
    }
    #endregion

    #region Extractor
    public void Play_InteractableObjects_UngoingExtractor_Clip()
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = InteractableObjects_UngoingExtractor_Clip;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = 1f;
            audioSource_WorldSound.Play();
        }
    }
    #endregion

    #region Lamp
    public void Play_InteractableObjects_UngoingLamp_Clip()
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = InteractableObjects_UngoingLamp_Clip;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = 1f;
            audioSource_WorldSound.Play();
        }
    }
    #endregion

    #region Spotlight
    public void Play_InteractableObjects_UngoingSpotlight_Clip()
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = InteractableObjects_UngoingSpotlight_Clip;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = 1f;
            audioSource_WorldSound.Play();
        }
    }
    #endregion|
    #endregion

    #region Buffs
    public void Play_Buff_Deactivated_Clip()
    {
        if (audioSource_WorldSound != null)
        {
            audioSource_WorldSound.clip = buff_Deactivated;
            audioSource_WorldSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_WorldSFX();
            audioSource_WorldSound.pitch = 1f;
            audioSource_WorldSound.Play();
        }
    }
    #endregion

    #region Journal Pages
    public void Play_JournalPage_GetNewJournalPage_Clip()
    {
        if (audioSource_MenuSound != null)
        {
            audioSource_MenuSound.clip = journalPage_GetNewJournalPage;
            audioSource_MenuSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_MenuSFX();
            audioSource_MenuSound.pitch = 1f;
            audioSource_MenuSound.Play();
        }
    }
    public void Play_JournalPage_SelectingJournalPage_Clip()
    {
        if (audioSource_MenuSound != null)
        {
            audioSource_MenuSound.clip = journalPage_SelectingJournalPage;
            audioSource_MenuSound.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_MenuSFX();
            audioSource_MenuSound.pitch = 1f;
            audioSource_MenuSound.Play();
        }
    }
    public void Play_JournalPage_VoiceMessage_Clip(AudioClip clip)
    {
        if (audioSource_VoiceMessages != null)
        {
            audioSource_VoiceMessages.clip = clip;
            audioSource_VoiceMessages.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_Voice();
            audioSource_VoiceMessages.pitch = 1f;
            audioSource_VoiceMessages.Play();
        }
    }
    #endregion

    #region Research
    public void Play_Research_Ongoing_Clip()
    {
        if (audioSource_MenuSound2 != null)
        {
            audioSource_MenuSound2.clip = research_Ongoing;
            audioSource_MenuSound2.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_MenuSFX();
            audioSource_MenuSound2.pitch = 1f;
            audioSource_MenuSound2.Play();
        }
    }
    public void Play_Research_Complete_Clip()
    {
        if (audioSource_MenuSound2 != null)
        {
            audioSource_MenuSound2.clip = research_Complete;
            audioSource_MenuSound2.volume = SettingsManager.Instance.Get_Sound_Master() * SettingsManager.Instance.Get_Sound_MenuSFX();
            audioSource_MenuSound2.pitch = 1f;
            audioSource_MenuSound2.Play();
        }
    }
    #endregion
}
