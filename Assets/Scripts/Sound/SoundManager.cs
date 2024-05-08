using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    #region Variables
    public float sound_Master;

    public float sound_World;
    public float sound_Menu;
    public float sound_Creatures;
    public float sound_Music;
    public float sound_Weather;
    public float sound_Voice;
    #endregion

    #region Sound Categories
    [Header("Everywhere")]
    public AudioSource audioSource__Music_HomeBase; //Music Source
    public AudioSource audioSource__Music_MysteriousSand; //Music Source
    public AudioSource audioSource__Music_Seclusion; //Music Source

    public AudioSource audioSource_WeatherSound_Cloudy; //Weather Sound Source
    public AudioSource audioSource_WeatherSound_Cold; //Weather Sound Source
    public AudioSource audioSource_WeatherSound_Warm; //Weather Sound Source
    public AudioSource audioSource_WeatherSound_Windy; //Weather Sound Source
    public AudioSource audioSource_WeatherSound_Night; //Weather Sound Source

    [Header("Player Movement")]
    public AudioSource audioSource_PlayerMovement_WalkingRunning; //
    public AudioSource audioSource_PlayerMovement_WalkingRunning_Water; //
    public AudioSource audioSource_PlayerMovement_FallDamage; //

    [Header("Tablet")]
    public AudioSource audioSource_Tablet_OpenCloseTablet; //
    public AudioSource audioSource_Tablet_ChangeTabetMenu; //

    [Header("Inventory")]
    public AudioSource audioSource_Inventory_ItemHover; //
    public AudioSource audioSource_Inventory_ItemManouver; //

    [Header("Hotbar")]
    public AudioSource audioSource_Hotbar_ChangeHotbarSlot; //
    public AudioSource audioSource_Hotbar_AddToHotbar; //
    public AudioSource audioSource_Hotbar_RemoveFromHotbar; //

    [Header("Crafting")]
    public AudioSource audioSource_Crafting_ChangeCraftingMenu; //
    public AudioSource audioSource_Crafting_SelectCraftingItem; //
    public AudioSource audioSource_Crafting_PerformCrafting; //
    public AudioSource audioSource_Crafting_CannotCraftError; //

    [Header("SkillTree")]
    public AudioSource audioSource_SkillTree_PerkHover; //
    public AudioSource audioSource_SkillTree_ChangeSkillTreeMenu; //
    public AudioSource audioSource_CompletePerk; //
    public AudioSource audioSource_CannotUpgrade; //

    [Header("Equipped Items")]
    public AudioSource audioSource_EquippedItems_BreakEquipment; //

    [Header("Axe Usage")]
    public AudioSource audioSource_AxeUsage_Cutting; //
    public AudioSource audioSource_AxeUsage_TreeFall; //
    public AudioSource audioSource_AxeUsage_TreeHitGround; //
    public AudioSource audioSource_CannotHitCuttable; //

    [Header("Pickaxe Usage")]
    public AudioSource audioSource_PickaxeUsage_Mining; //
    public AudioSource audioSource_PickaxeUsage_OreDestroyd; //
    public AudioSource audioSource_PickaxeUsage_CannotHitMineable; //

    [Header("Sword Usage")]
    public AudioSource audioSource_SwordUsage_Slash; //
    public AudioSource audioSource_EnemyHit; //

    [Header("Building")]
    public AudioSource audioSource_Building_Placement; //
    public AudioSource audioSource_Building_Remove; //
    public AudioSource audioSource_CannotPlace; //

    [Header("Chest")]
    public AudioSource audioSource_Chest_Open; //
    public AudioSource audioSource_Chest_Close; //

    [Header("InteractableObjects")]
    public AudioSource audioSource_InteractableObjects_Open; //
    public AudioSource audioSource_InteractableObjects_Ongoing; //
    public AudioSource audioSource_InteractableObjects_Close; //

    [Header("ResearchTable")]
    public AudioSource audioSource_ResearchTable_Research; //
    public AudioSource audioSource_ResearchTable_ResearchComplete; //
    public AudioSource audioSource_ResearchTable_NewItemAvailable; //

    [Header("Buffs")]
    public AudioSource audioSource_Buffs_AddBuffEffect; //
    public AudioSource audioSource_Buffs_RemoveBuffEffect; //

    [Header("Journal")]
    public AudioSource audioSource_Journal_GetNewJournalPage; //
    public AudioSource audioSource_Journal_SelectingJournalPage; //
    public AudioSource audioSource_Journal_VoiceMessage; //Voice Source

    [Header("GhostFight")]
    public AudioSource audioSource_Ghost_GhostCapturerSFX; //
    public AudioSource audioSource_Ghost_WorldGhostSFX; //

    [Header("GameOver")]
    public AudioSource audioSource_GameOver_Screen; //

    [Header("Arídea Gate")]
    public AudioSource audioSource_ArídeaGate_Rotate; //

    #endregion


    //--------------------


    #region Clips

    [Space(50)]

    #region Music
    [Header("Music")]
    [SerializeField] AudioClip music_HomeBase_Clip; //
    [SerializeField] AudioClip music_MysteriousSand_Clip; //
    [SerializeField] AudioClip music_Seclusion_Clip; //
    #endregion
    #region Weather
    [Header("Weather")]
    [SerializeField] AudioClip weather_Cloudy_Clip; //
    [SerializeField] AudioClip weather_Cold_Clip; //
    [SerializeField] AudioClip weather_Hot_Clip; //
    [SerializeField] AudioClip weather_Windy_Clip; //
    [SerializeField] AudioClip weather_Night_Clip; //
    #endregion

    #region Player
    [Header("Player")]
    [SerializeField] AudioClip player_Walking_Sand_Clip; //
    [SerializeField] AudioClip player_Walking_Ruin_Clip; //
    [SerializeField] AudioClip player_Walking_Water_Clip; //
    [SerializeField] AudioClip player_Walking_Stone_Clip; //
    [SerializeField] AudioClip player_Walking_Wood_Clip; //
    [SerializeField] AudioClip player_Walking_Cryonite_Clip; //

    [SerializeField] AudioClip player_FallDamage_Clip; //
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
    [SerializeField] AudioClip skillTree_ChangeMenu_Clip;
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
    [SerializeField] AudioClip axeUsage_TreeHitGround_Clip; //
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
    [SerializeField] AudioClip swordUsage_EnemyHit_Clip; //
    #endregion
    #region Building
    [Header("Building")]
    [SerializeField] AudioClip building_Place_Wood_Clip;
    [SerializeField] AudioClip building_Place_Stone_Clip;
    [SerializeField] AudioClip building_Place_Cryonite;

    [SerializeField] AudioClip building_Remove_Wood_Clip;
    [SerializeField] AudioClip building_Remove_Stone_Clip;
    [SerializeField] AudioClip building_Remove_Cryonite_Clip;

    [SerializeField] AudioClip building_CannotPlaceBlock_Clip;
    [SerializeField] AudioClip building_Place_MoveableObject_Clip; //
    [SerializeField] AudioClip building_Remove_MoveableObject_Clip; //

    [SerializeField] AudioClip building_WoodDoor_Open_Clip; //
    [SerializeField] AudioClip building_WoodDoor_Close_Clip; //
    [SerializeField] AudioClip building_StoneDoor_Open_Clip; //
    [SerializeField] AudioClip building_StoneDoor_Close_Clip; //
    [SerializeField] AudioClip building_CryoniteDoor_Open_Clip; //
    [SerializeField] AudioClip building_CryoniteDoor_Close_Clip; //
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
    [SerializeField] AudioClip InteractableObjects_ResearchTable_ResearchComplete_Clip; //
    [SerializeField] AudioClip InteractableObjects_ResearchTable_NewItemAvailable_Clip; //

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
    [SerializeField] AudioClip buff_Activated_Clip; //
    [SerializeField] AudioClip buff_Deactivated_Clip; //
    #endregion
    #region Journal Pages
    [Header("Journal Pages")]
    [SerializeField] AudioClip journalPage_GetNewJournalPage_Clip; //
    [SerializeField] AudioClip journalPage_SelectingJournalPage_Clip; //
    #endregion
    #region Research
    #endregion
    #region Ghost
    [Header("Ghost")]
    [SerializeField] AudioClip ghost_TargetGhost_Clip; //

    [SerializeField] AudioClip ghost_GhostMood_Targeted_Clip; //
    [SerializeField] AudioClip ghost_GhostMood_Happy_Clip; //
    [SerializeField] AudioClip ghost_GhostMood_Sad_Clip; //
    [SerializeField] AudioClip ghost_GhostMood_Moderate_Clip; //
    [SerializeField] AudioClip ghost_GhostMood_Thinking_Clip; //
    [SerializeField] AudioClip ghost_GhostMood_Enjoying_Clip; //
    [SerializeField] AudioClip ghost_GhostMood_Whispering_Clip; //

    [SerializeField] AudioClip ghost_GhostMood_Captured_Clip; //

    [SerializeField] AudioClip ghost_GhostTank_AddedToGhostTank_Clip; //
    [SerializeField] AudioClip ghost_GhostTank_RemovedFromGhostTank_Clip; //

    [SerializeField] AudioClip ghost_GhostAnimation_Spin1_Clip; //
    [SerializeField] AudioClip ghost_GhostAnimation_Spin2_Clip; //
    [SerializeField] AudioClip ghost_GhostAnimation_Spin3_Clip; //
    [SerializeField] AudioClip ghost_GhostAnimation_FakeDeath_Clip; //
    [SerializeField] AudioClip ghost_GhostAnimation_Sneeze1_Clip; //
    [SerializeField] AudioClip ghost_GhostAnimation_Sneeze2_Clip; //
    [SerializeField] AudioClip ghost_GhostAnimation_Wave_Clip; //
    #endregion
    #region GameOver
    [Header("GameOver")]
    [SerializeField] AudioClip gameOver_GameOverSound_Clip; //
    #endregion
    #region Arídea Gate
    [Header("Arídea Gate")]
    [SerializeField] AudioClip gameOver_ArídeaGate_KeyPlacement_Clip; //
    [SerializeField] AudioClip gameOver_ArídeaGate_Rotate_Clip; //
    [SerializeField] AudioClip gameOver_ArídeaGate_InPlace_Clip; //
    #endregion

    #endregion


    //--------------------


    public void UpdateSounds()
    {
        //Get Volumes
        #region
        sound_Master = SettingsManager.Instance.Get_Sound_Master();

        sound_World = SettingsManager.Instance.Get_Sound_WorldSFX() * sound_Master;
        sound_Menu = SettingsManager.Instance.Get_Sound_MenuSFX() * sound_Master;
        sound_Creatures = SettingsManager.Instance.Get_Sound_CreaturesSFX() * sound_Master;
        sound_Music = SettingsManager.Instance.Get_Sound_Music() * sound_Master * 0.5f;
        sound_Weather = SettingsManager.Instance.Get_Sound_WeatherSFX() * sound_Master * 0.6f;
        sound_Voice = SettingsManager.Instance.Get_Sound_Voice() * sound_Master;
        #endregion

        //Add Volume to the audioSources
        #region 
        //Everywhere
        audioSource__Music_HomeBase.volume = sound_Music;
        audioSource__Music_MysteriousSand.volume = sound_Music;
        audioSource__Music_Seclusion.volume = sound_Music;

        audioSource_WeatherSound_Cloudy.volume = sound_Weather;
        audioSource_WeatherSound_Cold.volume = sound_Weather;
        audioSource_WeatherSound_Warm.volume = sound_Weather;
        audioSource_WeatherSound_Windy.volume = sound_Weather;
        audioSource_WeatherSound_Night.volume = sound_Weather;

        //Player Movement
        audioSource_PlayerMovement_WalkingRunning.volume = sound_World;
        audioSource_PlayerMovement_WalkingRunning_Water.volume = sound_World;
        audioSource_PlayerMovement_FallDamage.volume = sound_World;

        //Tablet
        audioSource_Tablet_OpenCloseTablet.volume = sound_Menu;
        audioSource_Tablet_ChangeTabetMenu.volume = sound_Menu;

        //Inventory
        audioSource_Inventory_ItemHover.volume = sound_Menu;
        audioSource_Inventory_ItemManouver.volume = sound_Menu;

        //Hotbar
        audioSource_Hotbar_ChangeHotbarSlot.volume = sound_Menu;
        audioSource_Hotbar_AddToHotbar.volume = sound_Menu;
        audioSource_Hotbar_RemoveFromHotbar.volume = sound_Menu;

        //Crafting
        audioSource_Crafting_ChangeCraftingMenu.volume = sound_Menu;
        audioSource_Crafting_SelectCraftingItem.volume = sound_Menu;
        audioSource_Crafting_PerformCrafting.volume = sound_Menu;
        audioSource_Crafting_CannotCraftError.volume = sound_Menu;

        //SkillTree
        audioSource_SkillTree_PerkHover.volume = sound_Menu;
        audioSource_SkillTree_ChangeSkillTreeMenu.volume = sound_Menu;
        audioSource_CompletePerk.volume = sound_Menu;
        audioSource_CannotUpgrade.volume = sound_Menu;

        //Equipped Item
        audioSource_EquippedItems_BreakEquipment.volume = sound_World;

        //Axe Usage
        audioSource_AxeUsage_Cutting.volume = sound_World;
        audioSource_AxeUsage_TreeFall.volume = sound_World;
        audioSource_AxeUsage_TreeHitGround.volume = sound_World;
        audioSource_CannotHitCuttable.volume = sound_World;

        //Pickxe Usage
        audioSource_PickaxeUsage_Mining.volume = sound_World;
        audioSource_PickaxeUsage_OreDestroyd.volume = sound_World;
        audioSource_PickaxeUsage_CannotHitMineable.volume = sound_World;

        //SwordUsage
        audioSource_SwordUsage_Slash.volume = sound_World;
        audioSource_EnemyHit.volume = sound_World;

        //Building
        audioSource_Building_Placement.volume = sound_World;
        audioSource_Building_Remove.volume = sound_World;
        audioSource_CannotPlace.volume = sound_World;

        //Chest
        audioSource_Chest_Open.volume = sound_World;
        audioSource_Chest_Close.volume = sound_World;

        //InteractableObjects
        audioSource_InteractableObjects_Open.volume = sound_World;
        audioSource_InteractableObjects_Ongoing.volume = sound_World;
        audioSource_InteractableObjects_Close.volume = sound_World;

        //Research
        audioSource_ResearchTable_Research.volume = sound_Menu;
        audioSource_ResearchTable_ResearchComplete.volume = sound_Menu;
        audioSource_ResearchTable_NewItemAvailable.volume = sound_Menu;

        //Buff
        audioSource_Buffs_AddBuffEffect.volume = sound_World;
        audioSource_Buffs_RemoveBuffEffect.volume = sound_World;

        //Journal
        audioSource_Journal_GetNewJournalPage.volume = sound_Menu;
        audioSource_Journal_SelectingJournalPage.volume = sound_Menu;
        audioSource_Journal_VoiceMessage.volume = sound_Voice;

        //GhostFight
        audioSource_Ghost_GhostCapturerSFX.volume = sound_World; //
        audioSource_Ghost_WorldGhostSFX.volume = sound_World; //

        //GameOver
        audioSource_GameOver_Screen.volume = sound_Menu;

        //Arídea Gate
        audioSource_ArídeaGate_Rotate.volume = sound_World;

        #endregion
    }


    //--------------------


    #region Music
    public void Play_Music_HomeBase_Clip()
    {
        if (audioSource__Music_HomeBase != null)
        {
            //audioSource__Music_HomeBase.clip = music_HomeBase_Clip;
            audioSource__Music_HomeBase.pitch = 1f;
            audioSource__Music_HomeBase.Play();
        }
    }
    public void Play_Music_MysteriousSand_Clip()
    {
        if (audioSource__Music_MysteriousSand != null)
        {
            //audioSource__Music_MysteriousSand.clip = music_MysteriousSand_Clip;
            audioSource__Music_MysteriousSand.pitch = 1f;
            audioSource__Music_MysteriousSand.Play();
        }
    }
    public void Play_Music_Seclusion_Clip()
    {
        if (audioSource__Music_Seclusion != null)
        {
            //audioSource__Music_Seclusion.clip = music_Seclusion_Clip;
            audioSource__Music_Seclusion.pitch = 1f;
            audioSource__Music_Seclusion.Play();
        }
    }
    #endregion

    #region Weather
    public void Play_Weather_Cloudy_Clip()
    {
        if (audioSource_WeatherSound_Cloudy != null)
        {
            audioSource_WeatherSound_Cloudy.clip = weather_Cloudy_Clip;
            audioSource_WeatherSound_Cloudy.pitch = 1f;
            audioSource_WeatherSound_Cloudy.Play();
        }
    }
    public void Stop_Weather_Cloudy_Clip()
    {
        if (audioSource_WeatherSound_Cloudy != null)
        {
            audioSource_WeatherSound_Cloudy.Stop();
        }
    }
    public void Play_Weather_Cold_Clip()
    {
        if (audioSource_WeatherSound_Cold != null)
        {
            audioSource_WeatherSound_Cold.clip = weather_Cold_Clip;
            audioSource_WeatherSound_Cold.pitch = 1f;
            audioSource_WeatherSound_Cold.Play();
        }
    }
    public void Stop_Weather_Cold_Clip()
    {
        if (audioSource_WeatherSound_Cold != null)
        {
            audioSource_WeatherSound_Cold.Stop();
        }
    }
    public void Play_Weather_Warm_Clip()
    {
        if (audioSource_WeatherSound_Warm != null)
        {
            audioSource_WeatherSound_Warm.clip = weather_Hot_Clip;
            audioSource_WeatherSound_Warm.pitch = 1f;
            audioSource_WeatherSound_Warm.Play();
        }
    }
    public void Stop_Weather_Warm_Clip()
    {
        if (audioSource_WeatherSound_Warm != null)
        {
            audioSource_WeatherSound_Warm.Stop();
        }
    }
    public void Play_Weather_Windy_Clip()
    {
        if (audioSource_WeatherSound_Windy != null)
        {
            audioSource_WeatherSound_Windy.clip = weather_Windy_Clip;
            audioSource_WeatherSound_Windy.pitch = 1f;
            audioSource_WeatherSound_Windy.Play();
        }
    }
    public void Stop_Weather_Windy_Clip()
    {
        if (audioSource_WeatherSound_Windy != null)
        {
            audioSource_WeatherSound_Windy.Stop();
        }
    }
    public void Play_Weather_Night_Clip()
    {
        if (audioSource_WeatherSound_Night != null)
        {
            audioSource_WeatherSound_Night.clip = weather_Night_Clip;
            audioSource_WeatherSound_Night.pitch = 1f;
            audioSource_WeatherSound_Night.Play();
        }
    }
    public void Stop_Weather_Night_Clip()
    {
        if (audioSource_WeatherSound_Night != null)
        {
            audioSource_WeatherSound_Night.Stop();
        }
    }
    #endregion

    #region Player //Have yet to be implemented
    public void Play_Player_Walking_Water_Clip()
    {
        if (audioSource_PlayerMovement_WalkingRunning_Water != null)
        {
            audioSource_PlayerMovement_WalkingRunning_Water.clip = player_Walking_Water_Clip;
            audioSource_PlayerMovement_WalkingRunning_Water.pitch = 1f;
            audioSource_PlayerMovement_WalkingRunning_Water.Play();
        }
    }
    public void Stop_Player_Walking_Water_Clip()
    {
        if (audioSource_PlayerMovement_WalkingRunning_Water != null)
        {
            audioSource_PlayerMovement_WalkingRunning_Water.Stop();
        }
    }

    public void Play_Player_Walking_Sand_Clip()
    {
        if (audioSource_PlayerMovement_WalkingRunning != null)
        {
            audioSource_PlayerMovement_WalkingRunning.clip = player_Walking_Sand_Clip;
            audioSource_PlayerMovement_WalkingRunning.pitch = Random.Range(0.8f, 1.2f); //1f
            audioSource_PlayerMovement_WalkingRunning.Play();
        }
    }
    public void Play_Player_Walking_Ruin_Clip()
    {
        if (audioSource_PlayerMovement_WalkingRunning != null)
        {
            audioSource_PlayerMovement_WalkingRunning.clip = player_Walking_Ruin_Clip;
            audioSource_PlayerMovement_WalkingRunning.pitch = Random.Range(1.3f, 1.7f); //1.5f
            audioSource_PlayerMovement_WalkingRunning.Play();
        }
    }
    public void Play_Player_Walking_Stone_Clip()
    {
        if (audioSource_PlayerMovement_WalkingRunning != null)
        {
            audioSource_PlayerMovement_WalkingRunning.clip = player_Walking_Stone_Clip;
            audioSource_PlayerMovement_WalkingRunning.pitch = Random.Range(0.8f, 1.2f); //1f
            audioSource_PlayerMovement_WalkingRunning.Play();
        }
    }
    public void Play_Player_Walking_Wood_Clip()
    {
        if (audioSource_PlayerMovement_WalkingRunning != null)
        {
            audioSource_PlayerMovement_WalkingRunning.clip = player_Walking_Wood_Clip;
            audioSource_PlayerMovement_WalkingRunning.pitch = Random.Range(1.8f, 2.2f); //2f
            audioSource_PlayerMovement_WalkingRunning.Play();
        }
    }
    public void Play_Player_Walking_Cryonite_Clip()
    {
        if (audioSource_PlayerMovement_WalkingRunning != null)
        {
            audioSource_PlayerMovement_WalkingRunning.clip = player_Walking_Cryonite_Clip;
            audioSource_PlayerMovement_WalkingRunning.pitch = Random.Range(0.5f, 0.7f); //0.6f
            audioSource_PlayerMovement_WalkingRunning.Play();
        }
    }
    public void Play_Player_FallDamage_Clip()
    {
        if (audioSource_PlayerMovement_FallDamage != null)
        {
            audioSource_PlayerMovement_FallDamage.clip = player_FallDamage_Clip;
            audioSource_PlayerMovement_FallDamage.pitch = Random.Range(0.8f, 1.2f); //1f
            audioSource_PlayerMovement_FallDamage.Play();
        }
    }
    public void Stop_Player_Walking_Clip()
    {
        if (audioSource_PlayerMovement_WalkingRunning != null)
        {
            audioSource_PlayerMovement_WalkingRunning.Stop();
        }
    }
    #endregion

    #region Tablet
    public void Play_Tablet_OpenTablet_Clip()
    {
        if (audioSource_Tablet_OpenCloseTablet != null)
        {
            audioSource_Tablet_OpenCloseTablet.clip = tablet_OpenTablet_Clip;
            audioSource_Tablet_OpenCloseTablet.pitch = 1f;
            audioSource_Tablet_OpenCloseTablet.Play();
        }
    }
    public void Play_Tablet_CloseTablet_Clip()
    {
        if (audioSource_Tablet_OpenCloseTablet != null)
        {
            audioSource_Tablet_OpenCloseTablet.clip = tablet_CloseTablet_Clip;
            audioSource_Tablet_OpenCloseTablet.pitch = 1f;
            audioSource_Tablet_OpenCloseTablet.Play();
        }
    }
    public void Play_Tablet_ChangeMenu_Clip()
    {
        if (audioSource_Tablet_ChangeTabetMenu != null)
        {
            audioSource_Tablet_ChangeTabetMenu.clip = tablet_ChangeMenu_Clip;
            audioSource_Tablet_ChangeTabetMenu.pitch = 1f;
            audioSource_Tablet_ChangeTabetMenu.Play();
        }
    }
    #endregion

    #region Inventory
    public void Play_Inventory_ItemHover_Clip()
    {
        if (audioSource_Inventory_ItemHover != null)
        {
            audioSource_Inventory_ItemHover.clip = inventory_ItemHover_Clip;
            audioSource_Inventory_ItemHover.pitch = 20f;
            audioSource_Inventory_ItemHover.Play();
        }
    }
    public void Play_Inventory_PickupItem_Clip()
    {
        if (audioSource_Inventory_ItemManouver != null)
        {
            audioSource_Inventory_ItemManouver.clip = inventory_PickupItem_Clip;
            audioSource_Inventory_ItemManouver.pitch = 1f;
            audioSource_Inventory_ItemManouver.Play();
        }
    }
    public void Play_Inventory_DropItem_Clip()
    {
        if (audioSource_Inventory_ItemManouver != null)
        {
            audioSource_Inventory_ItemManouver.clip = inventory_DropItem_Clip;
            audioSource_Inventory_ItemManouver.pitch = 1f;
            audioSource_Inventory_ItemManouver.Play();
        }
    }
    public void Play_Inventory_MoveItem_Clip()
    {
        if (audioSource_Inventory_ItemManouver != null)
        {
            audioSource_Inventory_ItemManouver.clip = inventory_MoveItem_Clip;
            audioSource_Inventory_ItemManouver.pitch = 1f;
            audioSource_Inventory_ItemManouver.Play();
        }
    }
    public void Play_Inventory_ConsumeItem_Clip()
    {
        if (audioSource_Inventory_ItemManouver != null)
        {
            audioSource_Inventory_ItemManouver.clip = inventory_ConsumeItem_Clip;
            audioSource_Inventory_ItemManouver.pitch = 1f;
            audioSource_Inventory_ItemManouver.Play();
        }
    }
    public void Play_Inventory_DrinkItem_Clip()
    {
        if (audioSource_Inventory_ItemManouver != null)
        {
            audioSource_Inventory_ItemManouver.clip = inventory_DrinkItem_Clip;
            audioSource_Inventory_ItemManouver.pitch = 1f;
            audioSource_Inventory_ItemManouver.Play();
        }
    }
    public void Play_Inventory_DrinkEmptyItem_Clip()
    {
        if (audioSource_Inventory_ItemManouver != null)
        {
            audioSource_Inventory_ItemManouver.clip = inventory_DrinkEmptyItem_Clip;
            audioSource_Inventory_ItemManouver.pitch = 1f;
            audioSource_Inventory_ItemManouver.Play();
        }
    }
    public void Play_Inventory_RefillDrink_Clip()
    {
        if (audioSource_Inventory_ItemManouver != null)
        {
            audioSource_Inventory_ItemManouver.clip = inventory_RefillDrink_Clip;
            audioSource_Inventory_ItemManouver.pitch = 1f;
            audioSource_Inventory_ItemManouver.Play();
        }
    }
    public void Play_Inventory_EquipItem_Clip()
    {
        if (audioSource_Inventory_ItemManouver != null)
        {
            audioSource_Inventory_ItemManouver.clip = inventory_EquipItem_Clip;
            audioSource_Inventory_ItemManouver.pitch = 1f;
            audioSource_Inventory_ItemManouver.Play();
        }
    }

    public void Play_Inventory_InventoryIsFull_Clip()
    {
        if (audioSource_Inventory_ItemManouver != null)
        {
            audioSource_Inventory_ItemManouver.clip = inventory_InventoryIsFull_Clip;
            audioSource_Inventory_ItemManouver.pitch = 1f;
            audioSource_Inventory_ItemManouver.Play();
        }
    }
    #endregion

    #region Hotbar
    public void Play_Hotbar_ChangeSelectedItem_Clip()
    {
        if (audioSource_Hotbar_ChangeHotbarSlot != null)
        {
            audioSource_Hotbar_ChangeHotbarSlot.clip = hotbar_ChangeSelectedItem_Clip;
            audioSource_Hotbar_ChangeHotbarSlot.pitch = 20f;
            audioSource_Hotbar_ChangeHotbarSlot.Play();
        }
    }
    public void Play_Hotbar_AssignItemToHotbar_Clip()
    {
        if (audioSource_Hotbar_AddToHotbar != null)
        {
            audioSource_Hotbar_AddToHotbar.clip = hotbar_AssignItemToHotbar_Clip;
            audioSource_Hotbar_AddToHotbar.pitch = 1f;
            audioSource_Hotbar_AddToHotbar.Play();
        }
    }
    public void Play_Hotbar_RemoveItemFromHotbar_Clip()
    {
        if (audioSource_Hotbar_RemoveFromHotbar != null)
        {
            audioSource_Hotbar_RemoveFromHotbar.clip = hotbar_RemoveItemFromHotbar_Clip;
            audioSource_Hotbar_RemoveFromHotbar.pitch = 1f;
            audioSource_Hotbar_RemoveFromHotbar.Play();
        }
    }
    #endregion

    #region Crafting
    public void Play_Crafting_ChangeCraftingMenu_Clip()
    {
        if (audioSource_Crafting_ChangeCraftingMenu != null)
        {
            audioSource_Crafting_ChangeCraftingMenu.clip = crafting_SelectCraftingItem_Clip;
            audioSource_Crafting_ChangeCraftingMenu.pitch = 1f;
            audioSource_Crafting_ChangeCraftingMenu.Play();
        }
    }
    public void Play_Crafting_SelectCraftingItem_Clip()
    {
        if (audioSource_Crafting_SelectCraftingItem != null)
        {
            audioSource_Crafting_SelectCraftingItem.clip = crafting_ChangeCraftingMenu_Clip;
            audioSource_Crafting_SelectCraftingItem.pitch = 1f;
            audioSource_Crafting_SelectCraftingItem.Play();
        }
    }
    public void Play_Crafting_PerformCrafting_Clip()
    {
        if (audioSource_Crafting_PerformCrafting != null)
        {
            audioSource_Crafting_PerformCrafting.clip = crafting_PerformCrafting_Clip;
            audioSource_Crafting_PerformCrafting.pitch = 1f;
            audioSource_Crafting_PerformCrafting.Play();
        }
    }
    public void Play_Crafting_CannotCraft_Clip()
    {
        if (audioSource_Crafting_CannotCraftError != null)
        {
            audioSource_Crafting_CannotCraftError.clip = crafting_CannotCraft_Clip;
            audioSource_Crafting_CannotCraftError.pitch = 1f;
            audioSource_Crafting_CannotCraftError.Play();
        }
    }
    #endregion

    #region Skill Tree
    public void Play_SkillTree_HoverPerk_Clip()
    {
        if (audioSource_SkillTree_PerkHover != null)
        {
            audioSource_SkillTree_PerkHover.clip = skillTree_HoverPerk_Clip;
            audioSource_SkillTree_PerkHover.pitch = 20f;
            audioSource_SkillTree_PerkHover.Play();
        }
    }
    public void Play_SkillTree_ChangeMenu_Clip()
    {
        if (audioSource_SkillTree_ChangeSkillTreeMenu != null)
        {
            audioSource_SkillTree_ChangeSkillTreeMenu.clip = skillTree_ChangeMenu_Clip;
            audioSource_SkillTree_ChangeSkillTreeMenu.pitch = 1f;
            audioSource_SkillTree_ChangeSkillTreeMenu.Play();
        }
    }
    public void Play_SkillTree_CompletedPerk_Clip()
    {
        if (audioSource_CompletePerk != null)
        {
            audioSource_CompletePerk.clip = skillTree_CompletedPerk_Clip;
            audioSource_CompletePerk.pitch = 1f;
            audioSource_CompletePerk.Play();
        }
    }
    #endregion

    #region Equipped Item
    public void Play_EquippedItems_EquippedItemIsBroken_Clip()
    {
        if (audioSource_EquippedItems_BreakEquipment != null)
        {
            audioSource_EquippedItems_BreakEquipment.clip = equippedItems_EquippedItemIsBroken_Clip;
            audioSource_EquippedItems_BreakEquipment.pitch = 1f;
            audioSource_EquippedItems_BreakEquipment.Play();
        }
    }
    #endregion

    #region Axe Useage
    public void Play_AxeUsage_Hand_Clip(float pitch)
    {
        if (audioSource_AxeUsage_Cutting != null)
        {
            audioSource_AxeUsage_Cutting.clip = axeUsage_Hand_Clip;
            audioSource_AxeUsage_Cutting.pitch = pitch;
            audioSource_AxeUsage_Cutting.Play();
        }
    }
    public void Play_AxeUsage_WoodAxe_Clip(float pitch)
    {
        if (audioSource_AxeUsage_Cutting != null)
        {
            audioSource_AxeUsage_Cutting.clip = axeUsage_WoodAxe_Clip;
            audioSource_AxeUsage_Cutting.pitch = pitch;
            audioSource_AxeUsage_Cutting.Play();
        }
    }
    public void Play_AxeUsage_StoneAxe_Clip(float pitch)
    {
        if (audioSource_AxeUsage_Cutting != null)
        {
            audioSource_AxeUsage_Cutting.clip = axeUsage_StoneAxe_Clip;
            audioSource_AxeUsage_Cutting.pitch = pitch;
            audioSource_AxeUsage_Cutting.Play();
        }
    }
    public void Play_AxeUsage_CryoniteAxe_Clip(float pitch)
    {
        if (audioSource_AxeUsage_Cutting != null)
        {
            audioSource_AxeUsage_Cutting.clip = axeUsage_CryoniteAxe_Clip;
            audioSource_AxeUsage_Cutting.pitch = pitch;
            audioSource_AxeUsage_Cutting.Play();
        }
    }

    public void Play_AxeUsage_TreeFalling_Clip()
    {
        if (audioSource_AxeUsage_TreeFall != null)
        {
            audioSource_AxeUsage_TreeFall.clip = axeUsage_TreeFalling_Clip;
            audioSource_AxeUsage_TreeFall.pitch = 1f;
            audioSource_AxeUsage_TreeFall.Play();
        }
    }
    public void Play_AxeUsage_TreeHitGround_Clip()
    {
        if (audioSource_AxeUsage_TreeHitGround != null)
        {
            audioSource_AxeUsage_TreeHitGround.clip = axeUsage_TreeHitGround_Clip;
            audioSource_AxeUsage_TreeHitGround.pitch = 1f;
            audioSource_AxeUsage_TreeHitGround.Play();
        }
    }
    public void Play_AxeUsage_CannotHit_Clip()
    {
        if (audioSource_CannotHitCuttable != null)
        {
            audioSource_CannotHitCuttable.clip = axeUsage_CannotHit_Clip;
            audioSource_CannotHitCuttable.pitch = 1f;
            audioSource_CannotHitCuttable.Play();
        }
    }
    #endregion

    #region Pickaxe Useage
    public void Play_PickaxeUsage_Hand_Clip(float pitch)
    {
        if (audioSource_PickaxeUsage_Mining != null)
        {
            audioSource_PickaxeUsage_Mining.clip = pickaxeUsage_Hand_Clip;
            audioSource_PickaxeUsage_Mining.pitch = pitch;
            audioSource_PickaxeUsage_Mining.Play();
        }
    }
    public void Play_PickaxeUsage_WoodPickaxe_Clip(float pitch)
    {
        if (audioSource_PickaxeUsage_Mining != null)
        {
            audioSource_PickaxeUsage_Mining.clip = pickaxeUsage_WoodPickaxe_Clip;
            audioSource_PickaxeUsage_Mining.pitch = pitch;
            audioSource_PickaxeUsage_Mining.Play();
        }
    }
    public void Play_PickaxeUsage_StonePickaxe_Clip(float pitch)
    {
        if (audioSource_PickaxeUsage_Mining != null)
        {
            audioSource_PickaxeUsage_Mining.clip = pickaxeUsage_StonePickaxe_Clip;
            audioSource_PickaxeUsage_Mining.pitch = pitch;
            audioSource_PickaxeUsage_Mining.Play();
        }
    }
    public void Play_PickaxeUsage_CryonitePickaxe_Clip(float pitch)
    {
        if (audioSource_PickaxeUsage_Mining != null)
        {
            audioSource_PickaxeUsage_Mining.clip = pickaxeUsage_CryonitePickaxe_Clip;
            audioSource_PickaxeUsage_Mining.pitch = pitch;
            audioSource_PickaxeUsage_Mining.Play();
        }
    }

    public void Play_PickaxeUsage_OreIsDestroyd_Clip()
    {
        if (audioSource_PickaxeUsage_OreDestroyd != null)
        {
            audioSource_PickaxeUsage_OreDestroyd.clip = pickaxeUsage_OreIsDestroid_Clip;
            audioSource_PickaxeUsage_OreDestroyd.pitch = 1f;
            audioSource_PickaxeUsage_OreDestroyd.Play();
        }
    }
    public void Play_PickaxeUsage_CannotHit_Clip()
    {
        if (audioSource_PickaxeUsage_CannotHitMineable != null)
        {
            audioSource_PickaxeUsage_CannotHitMineable.clip = pickaxeUsage_CannotHit_Clip;
            audioSource_PickaxeUsage_CannotHitMineable.pitch = 1f;
            audioSource_PickaxeUsage_CannotHitMineable.Play();
        }
    }
    #endregion

    #region Sword Usage //Have yet to be implemented
    public void Play_SwordUsage_Slashing_Clip()
    {
        if (audioSource_SwordUsage_Slash != null)
        {
            audioSource_SwordUsage_Slash.clip = swordUsage_Slashing_Clip;
            audioSource_SwordUsage_Slash.pitch = 1f;
            audioSource_SwordUsage_Slash.Play();
        }
    }
    public void Play_SwordUsage_EnemyHit_Clip()
    {
        if (audioSource_EnemyHit != null)
        {
            audioSource_EnemyHit.clip = swordUsage_EnemyHit_Clip;
            audioSource_EnemyHit.pitch = 1f;
            audioSource_EnemyHit.Play();
        }
    }
    #endregion

    #region Building
    public void Play_Building_Place_Wood_Clip()
    {
        if (audioSource_Building_Placement != null)
        {
            audioSource_Building_Placement.clip = building_Place_Wood_Clip;
            audioSource_Building_Placement.pitch = 1f;
            audioSource_Building_Placement.Play();
        }
    }
    public void Play_Building_Place_Stone_Clip()
    {
        if (audioSource_Building_Placement != null)
        {
            audioSource_Building_Placement.clip = building_Place_Stone_Clip;
            audioSource_Building_Placement.pitch = 0.9f;
            audioSource_Building_Placement.Play();
        }
    }
    public void Play_Building_Place_Cryonite_Clip()
    {
        if (audioSource_Building_Placement != null)
        {
            audioSource_Building_Placement.clip = building_Place_Cryonite;
            audioSource_Building_Placement.pitch = 1f;
            audioSource_Building_Placement.Play();
        }
    }
    
    public void Play_Building_Remove_Wood_Clip()
    {
        if (audioSource_Building_Remove != null)
        {
            audioSource_Building_Remove.clip = building_Remove_Wood_Clip;
            audioSource_Building_Remove.pitch = 1f;
            audioSource_Building_Remove.Play();
        }
    }
    public void Play_Building_Remove_Stone_Clip()
    {
        if (audioSource_Building_Remove != null)
        {
            audioSource_Building_Remove.clip = building_Remove_Stone_Clip;
            audioSource_Building_Remove.pitch = 0.9f;
            audioSource_Building_Remove.Play();
        }
    }
    public void Play_Building_Remove_Cryonite_Clip()
    {
        if (audioSource_Building_Remove != null)
        {
            audioSource_Building_Remove.clip = building_Remove_Cryonite_Clip;
            audioSource_Building_Remove.pitch = 1f;
            audioSource_Building_Remove.Play();
        }
    }

    public void Play_Building_CannotPlaceBlock_Clip()
    {
        if (audioSource_CannotPlace != null)
        {
            audioSource_CannotPlace.clip = building_CannotPlaceBlock_Clip;
            audioSource_CannotPlace.pitch = 1f;
            audioSource_CannotPlace.Play();
        }
    }
    public void Play_Building_Place_MoveableObject_Clip()
    {
        if (audioSource_Building_Placement != null)
        {
            audioSource_Building_Placement.clip = building_Place_MoveableObject_Clip;
            audioSource_Building_Placement.pitch = 1f;
            audioSource_Building_Placement.Play();
        }
    }
    public void Play_Building_Remove_MoveableObject_Clip()
    {
        if (audioSource_Building_Remove != null)
        {
            audioSource_Building_Remove.clip = building_Remove_MoveableObject_Clip;
            audioSource_Building_Remove.pitch = 1f;
            audioSource_Building_Remove.Play();
        }
    }

    public void Play_Building_WoodDoor_Open_Clip(AudioSource objSource)
    {
        if (objSource != null)
        {
            objSource.clip = building_WoodDoor_Open_Clip;
            objSource.pitch = 1f;
            objSource.Play();
        }
    }
    public void Play_Building_WoodDoor_Close_Clip(AudioSource objSource)
    {
        if (objSource != null)
        {
            objSource.clip = building_WoodDoor_Close_Clip;
            objSource.pitch = 1f;
            objSource.Play();
        }
    }
    public void Play_Building_StoneDoor_Open_Clip(AudioSource objSource)
    {
        if (objSource != null)
        {
            objSource.clip = building_StoneDoor_Open_Clip;
            objSource.pitch = 1f;
            objSource.Play();
        }
    }
    public void Play_Building_StoneDoor_Close_Clip(AudioSource objSource)
    {
        if (objSource != null)
        {
            objSource.clip = building_StoneDoor_Close_Clip;
            objSource.pitch = 1f;
            objSource.Play();
        }
    }
    public void Play_Building_CryoniteDoor_Open_Clip(AudioSource objSource)
    {
        if (objSource != null)
        {
            objSource.clip = building_CryoniteDoor_Open_Clip;
            objSource.pitch = 1f;
            objSource.Play();
        }
    }
    public void Play_Building_CryoniteDoor_Close_Clip(AudioSource objSource)
    {
        if (objSource != null)
        {
            objSource.clip = building_CryoniteDoor_Close_Clip;
            objSource.pitch = 1f;
            objSource.Play();
        }
    }
    #endregion

    #region Chests
    public void Play_Chests_OpenSmallChest_Clip()
    {
        if (audioSource_Chest_Open != null)
        {
            audioSource_Chest_Open.clip = chests_OpenSmallChest_Clip;
            audioSource_Chest_Open.pitch = 1f;
            audioSource_Chest_Open.Play();
        }
    }
    public void Play_Chests_CloseSmallChest_Clip()
    {
        if (audioSource_Chest_Close != null)
        {
            audioSource_Chest_Close.clip = chests_CloseSmallChest_Clip;
            audioSource_Chest_Close.pitch = 1f;
            audioSource_Chest_Close.Play();
        }
    }

    public void Play_Chests_OpenMediumChest_Clip()
    {
        if (audioSource_Chest_Open != null)
        {
            audioSource_Chest_Open.clip = chests_OpenMediumChest_Clip;
            audioSource_Chest_Open.pitch = 1f;
            audioSource_Chest_Open.Play();
        }
    }
    public void Play_Chests_CloseMediumChest_Clip()
    {
        if (audioSource_Chest_Close != null)
        {
            audioSource_Chest_Close.clip = chests_CloseMediumChest_Clip;
            audioSource_Chest_Close.pitch = 1f;
            audioSource_Chest_Close.Play();
        }
    }

    public void Play_Chests_OpenBigChest_Clip()
    {
        if (audioSource_Chest_Open != null)
        {
            audioSource_Chest_Open.clip = chests_OpenBigChest_Clip;
            audioSource_Chest_Open.pitch = 0.75f;
            audioSource_Chest_Open.Play();
        }
    }
    public void Play_Chests_CloseBigChest_Clip()
    {
        if (audioSource_Chest_Close != null)
        {
            audioSource_Chest_Close.clip = chests_CloseBigChest_Clip;
            audioSource_Chest_Close.pitch = 0.75f;
            audioSource_Chest_Close.Play();
        }
    }
    #endregion

    #region InteractableObjects //"Ungoing" have yet to be implemented
    #region Crafting Table
    public void Play_InteractableObjects_OpenCraftingTable_Clip()
    {
        if (audioSource_InteractableObjects_Open != null)
        {
            audioSource_InteractableObjects_Open.clip = InteractableObjects_OpenCraftingTable_Clip;
            audioSource_InteractableObjects_Open.pitch = 1f;
            audioSource_InteractableObjects_Open.Play();
        }
    }
    public void Play_InteractableObjects_UngoingCraftingTable_Clip()
    {
        if (audioSource_InteractableObjects_Ongoing != null)
        {
            audioSource_InteractableObjects_Ongoing.clip = InteractableObjects_UngoingCraftingTable_Clip;
            audioSource_InteractableObjects_Ongoing.pitch = 1f;
            audioSource_InteractableObjects_Ongoing.Play();
        }
    }
    public void Play_InteractableObjects_CloseCraftingTable_Clip()
    {
        if (audioSource_InteractableObjects_Close != null)
        {
            audioSource_InteractableObjects_Close.clip = InteractableObjects_CloseCraftingTable_Clip;
            audioSource_InteractableObjects_Close.pitch = 1f;
            audioSource_InteractableObjects_Close.Play();
        }
    }
    #endregion

    #region Research Table
    public void Play_InteractableObjects_OpenResearchTable_Clip()
    {
        if (audioSource_InteractableObjects_Open != null)
        {
            audioSource_InteractableObjects_Open.clip = InteractableObjects_OpenResearchTable_Clip;
            audioSource_InteractableObjects_Open.pitch = 1f;
            audioSource_InteractableObjects_Open.Play();
        }
    }
    public void Play_InteractableObjects_UngoingResearchTable_Clip()
    {
        if (audioSource_InteractableObjects_Ongoing != null)
        {
            audioSource_InteractableObjects_Ongoing.clip = InteractableObjects_UngoingResearchTable_Clip;
            audioSource_InteractableObjects_Ongoing.pitch = 1f;
            audioSource_InteractableObjects_Ongoing.Play();
        }
    }
    public void Play_InteractableObjects_CloseResearchTable_Clip()
    {
        if (audioSource_InteractableObjects_Close != null)
        {
            audioSource_InteractableObjects_Close.clip = InteractableObjects_CloseResearchTable_Clip;
            audioSource_InteractableObjects_Close.pitch = 1f;
            audioSource_InteractableObjects_Close.Play();
        }
    }
    public void Play_ResearchTable_Researching_Clip()
    {
        if (audioSource_ResearchTable_Research != null)
        {
            audioSource_ResearchTable_Research.clip = InteractableObjects_ResearchTable_Researching_Clip;
            audioSource_ResearchTable_Research.pitch = 1f;
            audioSource_ResearchTable_Research.Play();
        }
    }
    public void Stop_ResearchTable_Researching_Clip()
    {
        if (audioSource_ResearchTable_Research != null)
        {
            audioSource_ResearchTable_Research.Stop();
        }
    }
    public void Play_Research_Complete_Clip()
    {
        if (audioSource_ResearchTable_ResearchComplete != null)
        {
            audioSource_ResearchTable_ResearchComplete.clip = InteractableObjects_ResearchTable_ResearchComplete_Clip;
            audioSource_ResearchTable_ResearchComplete.pitch = 1f;
            audioSource_ResearchTable_ResearchComplete.Play();
        }
    }
    public void Play_Research_NewItemAvailable_Clip()
    {
        if (audioSource_ResearchTable_NewItemAvailable != null)
        {
            audioSource_ResearchTable_NewItemAvailable.clip = InteractableObjects_ResearchTable_NewItemAvailable_Clip;
            audioSource_ResearchTable_NewItemAvailable.pitch = 1f;
            audioSource_ResearchTable_NewItemAvailable.Play();
        }
    }
    #endregion

    #region Skill Tree Table
    public void Play_InteractableObjects_OpenSkillTreeTable_Clip()
    {
        if (audioSource_InteractableObjects_Open != null)
        {
            audioSource_InteractableObjects_Open.clip = InteractableObjects_OpenSkillTreeTable_Clip;
            audioSource_InteractableObjects_Open.pitch = 1f;
            audioSource_InteractableObjects_Open.Play();
        }
    }
    public void Play_InteractableObjects_UngoingSkillTreeTable_Clip()
    {
        if (audioSource_InteractableObjects_Ongoing != null)
        {
            audioSource_InteractableObjects_Ongoing.clip = InteractableObjects_UngoingSkillTreeTable_Clip;
            audioSource_InteractableObjects_Ongoing.pitch = 1f;
            audioSource_InteractableObjects_Ongoing.Play();
        }
    }
    public void Play_InteractableObjects_CloseSkillTreeTable_Clip()
    {
        if (audioSource_InteractableObjects_Close != null)
        {
            audioSource_InteractableObjects_Close.clip = InteractableObjects_CloseSkillTreeTable_Clip;
            audioSource_InteractableObjects_Close.pitch = 1f;
            audioSource_InteractableObjects_Close.Play();
        }
    }
    #endregion

    #region Crop Plot
    public void Play_InteractableObjects_OpenCropPlot_Clip()
    {
        if (audioSource_InteractableObjects_Open != null)
        {
            audioSource_InteractableObjects_Open.clip = InteractableObjects_OpenCropPlot_Clip;
            audioSource_InteractableObjects_Open.pitch = 1f;
            audioSource_InteractableObjects_Open.Play();
        }
    }
    public void Play_InteractableObjects_UngoingCropPlot_Clip()
    {
        if (audioSource_InteractableObjects_Ongoing != null)
        {
            audioSource_InteractableObjects_Ongoing.clip = InteractableObjects_UngoingCropPlot_Clip;
            audioSource_InteractableObjects_Ongoing.pitch = 1f;
            audioSource_InteractableObjects_Ongoing.Play();
        }
    }
    public void Play_InteractableObjects_CloseCropPlot_Clip()
    {
        if (audioSource_InteractableObjects_Close != null)
        {
            audioSource_InteractableObjects_Close.clip = InteractableObjects_CloseCropPlot_Clip;
            audioSource_InteractableObjects_Close.pitch = 1f;
            audioSource_InteractableObjects_Close.Play();
        }
    }
    #endregion

    #region Ghost Tank
    public void Play_InteractableObjects_UngoingGhostTank_Clip()
    {
        if (audioSource_InteractableObjects_Ongoing != null)
        {
            audioSource_InteractableObjects_Ongoing.clip = InteractableObjects_UngoingGhostTank_Clip;
            audioSource_InteractableObjects_Ongoing.pitch = 1f;
            audioSource_InteractableObjects_Ongoing.Play();
        }
    }
    #endregion

    #region Extractor
    public void Play_InteractableObjects_UngoingExtractor_Clip()
    {
        if (audioSource_InteractableObjects_Ongoing != null)
        {
            audioSource_InteractableObjects_Ongoing.clip = InteractableObjects_UngoingExtractor_Clip;
            audioSource_InteractableObjects_Ongoing.pitch = 1f;
            audioSource_InteractableObjects_Ongoing.Play();
        }
    }
    #endregion

    #region Lamp
    public void Play_InteractableObjects_UngoingLamp_Clip()
    {
        if (audioSource_InteractableObjects_Ongoing != null)
        {
            audioSource_InteractableObjects_Ongoing.clip = InteractableObjects_UngoingLamp_Clip;
            audioSource_InteractableObjects_Ongoing.pitch = 1f;
            audioSource_InteractableObjects_Ongoing.Play();
        }
    }
    #endregion

    #region Spotlight
    public void Play_InteractableObjects_UngoingSpotlight_Clip()
    {
        if (audioSource_InteractableObjects_Ongoing != null)
        {
            audioSource_InteractableObjects_Ongoing.clip = InteractableObjects_UngoingSpotlight_Clip;
            audioSource_InteractableObjects_Ongoing.pitch = 1f;
            audioSource_InteractableObjects_Ongoing.Play();
        }
    }
    #endregion|
    #endregion

    #region Buffs
    public void Play_Buff_Activated_Clip()
    {
        if (audioSource_Buffs_AddBuffEffect != null)
        {
            audioSource_Buffs_AddBuffEffect.clip = buff_Activated_Clip;
            audioSource_Buffs_AddBuffEffect.pitch = 1f;
            audioSource_Buffs_AddBuffEffect.Play();
        }
    }
    public void Play_Buff_Deactivated_Clip()
    {
        if (audioSource_Buffs_RemoveBuffEffect != null)
        {
            audioSource_Buffs_RemoveBuffEffect.clip = buff_Deactivated_Clip;
            audioSource_Buffs_RemoveBuffEffect.pitch = 1f;
            audioSource_Buffs_RemoveBuffEffect.Play();
        }
    }
    #endregion

    #region Journal Pages
    public void Play_JournalPage_GetNewJournalPage_Clip()
    {
        if (audioSource_Journal_GetNewJournalPage != null)
        {
            audioSource_Journal_GetNewJournalPage.clip = journalPage_GetNewJournalPage_Clip;
            audioSource_Journal_GetNewJournalPage.pitch = 1f;
            audioSource_Journal_GetNewJournalPage.Play();
        }
    }
    public void Play_JournalPage_SelectingJournalPage_Clip()
    {
        if (audioSource_Journal_SelectingJournalPage != null)
        {
            audioSource_Journal_SelectingJournalPage.clip = journalPage_SelectingJournalPage_Clip;
            audioSource_Journal_SelectingJournalPage.pitch = 1f;
            audioSource_Journal_SelectingJournalPage.Play();
        }
    }
    public void Play_JournalPage_VoiceMessage_Clip(AudioClip clip)
    {
        if (audioSource_Journal_VoiceMessage != null)
        {
            audioSource_Journal_VoiceMessage.clip = clip;
            audioSource_Journal_VoiceMessage.pitch = 1f;
            audioSource_Journal_VoiceMessage.Play();
        }
    }
    #endregion

    #region Ghosts
    //Is Targeted Sound Effects
    public void Play_Ghost_TargetGhost_Clip()
    {
        if (audioSource_Ghost_GhostCapturerSFX != null)
        {
            audioSource_Ghost_GhostCapturerSFX.clip = ghost_TargetGhost_Clip;
            audioSource_Ghost_GhostCapturerSFX.pitch = 1f;
            audioSource_Ghost_GhostCapturerSFX.loop = true;
            audioSource_Ghost_GhostCapturerSFX.Play();
        }
    }
    public void Stop_Ghost_TargetGhost_Clip()
    {
        if (audioSource_Ghost_GhostCapturerSFX != null)
        {
            audioSource_Ghost_GhostCapturerSFX.loop = false;
            audioSource_Ghost_GhostCapturerSFX.Stop();
        }
    }

    //In Roaming Sounds
    public void Play_Ghost_GhostMood_Targeted_Clip(AudioSource objSource)
    {
        if (objSource != null)
        {
            objSource.clip = ghost_GhostMood_Targeted_Clip;
            objSource.pitch = 1f;
            objSource.Play();
        }
    }
    public void Play_Ghost_GhostMood_Happy_Clip(AudioSource objSource)
    {
        if (objSource != null)
        {
            objSource.clip = ghost_GhostMood_Happy_Clip;
            objSource.pitch = 1f;
            objSource.Play();
        }
    }
    public void Play_Ghost_GhostMood_Sad_Clip(AudioSource objSource)
    {
        if (objSource != null)
        {
            objSource.clip = ghost_GhostMood_Sad_Clip;
            objSource.pitch = 1f;
            objSource.Play();
        }
    }
    public void Play_Ghost_GhostMood_Moderate_Clip(AudioSource objSource)
    {
        if (objSource != null)
        {
            objSource.clip = ghost_GhostMood_Moderate_Clip;
            objSource.pitch = 1f;
            objSource.Play();
        }
    }
    public void Play_Ghost_GhostMood_Thinking_Clip(AudioSource objSource)
    {
        if (objSource != null)
        {
            objSource.clip = ghost_GhostMood_Thinking_Clip;
            objSource.pitch = 1f;
            objSource.Play();
        }
    }
    public void Play_Ghost_GhostMood_Enjoying_Clip(AudioSource objSource)
    {
        if (objSource != null)
        {
            objSource.clip = ghost_GhostMood_Enjoying_Clip;
            objSource.pitch = 1f;
            objSource.Play();
        }
    }
    public void Play_Ghost_GhostMood_Whispering_Clip(AudioSource objSource)
    {
        if (objSource != null)
        {
            objSource.clip = ghost_GhostMood_Whispering_Clip;
            objSource.pitch = 1f;
            objSource.Play();
        }
    }

    //Captured
    public void Play_Ghost_CaptureGhost_Clip()
    {
        if (audioSource_Ghost_WorldGhostSFX != null)
        {
            audioSource_Ghost_WorldGhostSFX.clip = ghost_GhostMood_Captured_Clip;
            audioSource_Ghost_WorldGhostSFX.pitch = 1f;
            audioSource_Ghost_WorldGhostSFX.Play();
        }
    }
    public void Play_GhostTank_AddedToGhostTank_Clip()
    {
        if (audioSource_Ghost_WorldGhostSFX != null)
        {
            audioSource_Ghost_WorldGhostSFX.clip = ghost_GhostTank_AddedToGhostTank_Clip;
            audioSource_Ghost_WorldGhostSFX.pitch = 1f;
            audioSource_Ghost_WorldGhostSFX.Play();
        }
    }
    public void Play_GhostTank_RemovedFromGhostTank_Clip()
    {
        if (audioSource_Ghost_WorldGhostSFX != null)
        {
            audioSource_Ghost_WorldGhostSFX.clip = ghost_GhostTank_RemovedFromGhostTank_Clip;
            audioSource_Ghost_WorldGhostSFX.pitch = 1f;
            audioSource_Ghost_WorldGhostSFX.Play();
        }
    }

    //In GhostTank
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
    public void Play_GhostAnimation_FakeDeath_Clip(AudioSource objSource)
    {
        if (objSource != null)
        {
            objSource.clip = ghost_GhostAnimation_FakeDeath_Clip;
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
    public void Play_GhostAnimation_Wave_Clip(AudioSource objSource)
    {
        if (objSource != null)
        {
            objSource.clip = ghost_GhostAnimation_Wave_Clip;
            objSource.pitch = 1f;
            objSource.Play();
        }
    }
    #endregion

    #region GameOver
    public void Play_GameOver_Clip()
    {
        if (audioSource_GameOver_Screen != null)
        {
            audioSource_GameOver_Screen.clip = gameOver_GameOverSound_Clip;
            audioSource_GameOver_Screen.pitch = 1f;
            audioSource_GameOver_Screen.Play();
        }
    }
    public void Stop_GameOver_Clip()
    {
        if (audioSource_GameOver_Screen != null)
        {
            audioSource_GameOver_Screen.Stop();
        }
    }
    #endregion

    #region Arídea Gate
    public void Play_ArídeaGate_KeyPlacement_Clip()
    {
        if (audioSource_ArídeaGate_Rotate != null)
        {
            audioSource_ArídeaGate_Rotate.clip = gameOver_ArídeaGate_KeyPlacement_Clip;
            audioSource_ArídeaGate_Rotate.pitch = 0.25f;
            audioSource_ArídeaGate_Rotate.Play();
        }
    }
    public void Play_ArídeaGate_Rotate_Clip()
    {
        if (audioSource_ArídeaGate_Rotate != null)
        {
            audioSource_ArídeaGate_Rotate.clip = gameOver_ArídeaGate_Rotate_Clip;
            audioSource_ArídeaGate_Rotate.pitch = 1f;
            audioSource_ArídeaGate_Rotate.Play();
        }
    }
    public void Play_ArídeaGate_InPlace_Clip()
    {
        if (audioSource_ArídeaGate_Rotate != null)
        {
            audioSource_ArídeaGate_Rotate.clip = gameOver_ArídeaGate_InPlace_Clip;
            audioSource_ArídeaGate_Rotate.pitch = 1f;
            audioSource_ArídeaGate_Rotate.Play();
        }
    }
    #endregion
}
