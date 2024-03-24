using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public AudioSource audioSource1; //Main Sound Source
    public AudioSource audioSource2;
    public AudioSource audioSource3;
    public AudioSource audioSource4;
    public AudioSource audioSource_VoiceMessages;

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
    [SerializeField] AudioClip axeUsage_Tree_Clip; //
    [SerializeField] AudioClip axeUsage_Cactus_Clip; //
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

    #endregion


    //--------------------


    #region Player //Have yet to be implemented
    public void Play_Player_Walking_Sand_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = player_Walking_Sand_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    public void Play_Player_Walking_Grass_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = player_Walking_Grass_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    public void Play_Player_Walking_Water_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = player_Walking_Water_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    public void Play_Player_Walking_Stone_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = player_Walking_Stone_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    public void Play_Player_Walking_Wood_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = player_Walking_Wood_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    public void Play_Player_Walking_Cryonite_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = player_Walking_Cryonite_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }

    public void Play_Player_Sprinting_Sand_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = player_Sprinting_Sand_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    public void Play_Player_Sprinting_Grass_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = player_Sprinting_Grass_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    public void Play_Player_Sprinting_Water_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = player_Sprinting_Water_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    public void Play_Player_Sprinting_Stone_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = player_Sprinting_Stone_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    public void Play_Player_Sprinting_Wood_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = player_Sprinting_Wood_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    public void Play_Player_Sprinting_Cryonite_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = player_Sprinting_Cryonite_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    #endregion

    #region Tablet
    public void Play_Tablet_OpenTablet_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = tablet_OpenTablet_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    public void Play_Tablet_CloseTablet_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = tablet_CloseTablet_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    public void Play_Tablet_ChangeMenu_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = tablet_ChangeMenu_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    #endregion

    #region Inventory
    public void Play_Inventory_ItemHover_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = inventory_ItemHover_Clip;
            audioSource1.pitch = 20f;
            audioSource1.volume = 0.25f;
            audioSource1.Play();
        }
    }
    public void Play_Inventory_PickupItem_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = inventory_PickupItem_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    public void Play_Inventory_DropItem_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = inventory_DropItem_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    public void Play_Inventory_MoveItem_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = inventory_MoveItem_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    public void Play_Inventory_ConsumeItem_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = inventory_ConsumeItem_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    public void Play_Inventory_DrinkItem_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = inventory_DrinkItem_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    public void Play_Inventory_DrinkEmptyItem_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = inventory_DrinkEmptyItem_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    public void Play_Inventory_RefillDrink_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = inventory_RefillDrink_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    public void Play_Inventory_EquipItem_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = inventory_EquipItem_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }

    public void Play_Inventory_InventoryIsFull_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = inventory_InventoryIsFull_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    #endregion

    #region Hotbar
    public void Play_Hotbar_ChangeSelectedItem_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = hotbar_ChangeSelectedItem_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    public void Play_Hotbar_AssignItemToHotbar_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = hotbar_AssignItemToHotbar_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    public void Play_Hotbar_RemoveItemFromHotbar_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = hotbar_RemoveItemFromHotbar_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    #endregion

    #region Crafting
    public void Play_Crafting_SelectCraftingItem_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = crafting_ChangeCraftingMenu_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    public void Play_Crafting_ChangeCraftingMenu_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = crafting_SelectCraftingItem_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    public void Play_Crafting_PerformCrafting_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = crafting_PerformCrafting_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    public void Play_Crafting_CannotCraft_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = crafting_CannotCraft_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    #endregion

    #region Skill Tree
    public void Play_SkillTree_HoverPerk_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = skillTree_HoverPerk_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    public void Play_SkillTree_CompletedPerk_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = skillTree_CompletedPerk_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    #endregion

    #region Equipped Item
    public void Play_EquippedItems_EquippedItemIsBroken_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = equippedItems_EquippedItemIsBroken_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    #endregion

    #region Axe Useage //Have yet to be implemented
    public void Play_AxeUsage_Tree_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = axeUsage_Tree_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    public void Play_AxeUsage_Cactus_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = axeUsage_Cactus_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    #endregion

    #region Pickaxe Useage
    public void Play_PickaxeUsage_Hand_Clip(float pitch)
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = pickaxeUsage_Hand_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = pitch;
            audioSource1.Play();
        }
    }
    public void Play_PickaxeUsage_WoodPickaxe_Clip(float pitch)
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = pickaxeUsage_WoodPickaxe_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = pitch;
            audioSource1.Play();
        }
    }
    public void Play_PickaxeUsage_StonePickaxe_Clip(float pitch)
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = pickaxeUsage_StonePickaxe_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = pitch;
            audioSource1.Play();
        }
    }
    public void Play_PickaxeUsage_CryonitePickaxe_Clip(float pitch)
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = pickaxeUsage_CryonitePickaxe_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = pitch;
            audioSource1.Play();
        }
    }

    public void Play_PickaxeUsage_OreIsDestroid_Clip()
    {
        if (audioSource2 != null)
        {
            audioSource2.clip = pickaxeUsage_OreIsDestroid_Clip;
            audioSource2.volume = 1f;
            audioSource2.pitch = 1f;
            audioSource2.Play();
        }
    }
    public void Play_PickaxeUsage_CannotHit_Clip()
    {
        if (audioSource2 != null)
        {
            audioSource2.clip = pickaxeUsage_CannotHit_Clip;
            audioSource2.volume = 1f;
            audioSource2.pitch = 1f;
            audioSource2.Play();
        }
    }
    #endregion

    #region Sword Usage //Have yet to be implemented
    public void Play_SwordUsage_Slashing_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = swordUsage_Slashing_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    #endregion

    #region Building
    public void Play_Building_Place_Wood_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = building_Place_Wood;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    public void Play_Building_Place_Stone_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = building_Place_Stone;
            audioSource1.volume = 1f;
            audioSource1.pitch = 0.7f;
            audioSource1.Play();
        }
    }
    public void Play_Building_Place_Cryonite_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = building_Place_Cryonite;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    
    public void Play_Building_Remove_Wood_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = building_Remove_Wood;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    public void Play_Building_Remove_Stone_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = building_Remove_Stone;
            audioSource1.volume = 1f;
            audioSource1.pitch = 0.7f;
            audioSource1.Play();
        }
    }
    public void Play_Building_Remove_Cryonite_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = building_Remove_Cryonite;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }

    public void Play_Building_CannotPlaceBlock_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = building_CannotPlaceBlock;
            audioSource1.volume = 1f;
            audioSource1.pitch = 0.5f;
            audioSource1.Play();
        }
    }
    public void Play_Building_Place_MoveableObject_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = building_Place_MoveableObject;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    public void Play_Building_Remove_MoveableObject_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = building_Remove_MoveableObject;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    #endregion

    #region Chests
    public void Play_Chests_OpenSmallChest_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = chests_OpenSmallChest_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    public void Play_Chests_CloseSmallChest_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = chests_CloseSmallChest_Clip;
            audioSource1.volume = 0.7f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }

    public void Play_Chests_OpenMediumChest_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = chests_OpenMediumChest_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    public void Play_Chests_CloseMediumChest_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = chests_CloseMediumChest_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }

    public void Play_Chests_OpenBigChest_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = chests_OpenBigChest_Clip;
            audioSource1.volume = 0.5f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    public void Play_Chests_CloseBigChest_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = chests_CloseBigChest_Clip;
            audioSource1.volume = 0.5f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    #endregion

    #region InteractableObjects //"Ungoing" have yet to be implemented
    public void Play_InteractableObjects_OpenCraftingTable_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = InteractableObjects_OpenCraftingTable_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    public void Play_InteractableObjects_UngoingCraftingTable_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = InteractableObjects_UngoingCraftingTable_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    public void Play_InteractableObjects_CloseCraftingTable_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = InteractableObjects_CloseCraftingTable_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }

    public void Play_InteractableObjects_OpenSkillTreeTable_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = InteractableObjects_OpenSkillTreeTable_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    public void Play_InteractableObjects_UngoingSkillTreeTable_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = InteractableObjects_UngoingSkillTreeTable_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    public void Play_InteractableObjects_CloseSkillTreeTable_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = InteractableObjects_CloseSkillTreeTable_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }

    public void Play_InteractableObjects_OpenCropPlot_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = InteractableObjects_OpenCropPlot_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    public void Play_InteractableObjects_UngoingCropPlot_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = InteractableObjects_UngoingCropPlot_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    public void Play_InteractableObjects_CloseCropPlot_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = InteractableObjects_CloseCropPlot_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }

    public void Play_InteractableObjects_UngoingGhostTank_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = InteractableObjects_UngoingGhostTank_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }

    public void Play_InteractableObjects_UngoingExtractor_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = InteractableObjects_UngoingExtractor_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }

    public void Play_InteractableObjects_UngoingLamp_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = InteractableObjects_UngoingLamp_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }

    public void Play_InteractableObjects_UngoingSpotlight_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = InteractableObjects_UngoingSpotlight_Clip;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    #endregion

    #region Buffs
    public void Play_Buff_Deactivated_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = buff_Deactivated;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    #endregion

    #region Journal Pages
    public void Play_JournalPage_GetNewJournalPage_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = journalPage_GetNewJournalPage;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    public void Play_JournalPage_SelectingJournalPage_Clip()
    {
        if (audioSource1 != null)
        {
            audioSource1.clip = journalPage_SelectingJournalPage;
            audioSource1.volume = 1f;
            audioSource1.pitch = 1f;
            audioSource1.Play();
        }
    }
    #endregion
}
