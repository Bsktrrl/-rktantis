using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public AudioSource audioSource;

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
    [SerializeField] AudioClip pickaxeUsage_Tungsten_Clip; //
    [SerializeField] AudioClip pickaxeUsage_Stone_Clip; //
    [SerializeField] AudioClip pickaxeUsage_Cryonite_Clip; //
    [SerializeField] AudioClip pickaxeUsage_Gold_Clip; //
    [SerializeField] AudioClip pickaxeUsage_Magnetite_Clip; //
    [SerializeField] AudioClip pickaxeUsage_Viridian_Clip; //
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

    #endregion


    //--------------------


    #region Player //Have yet to be implemented
    public void Play_Player_Walking_Sand_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = player_Walking_Sand_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void Play_Player_Walking_Grass_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = player_Walking_Grass_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void Play_Player_Walking_Water_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = player_Walking_Water_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void Play_Player_Walking_Stone_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = player_Walking_Stone_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void Play_Player_Walking_Wood_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = player_Walking_Wood_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void Play_Player_Walking_Cryonite_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = player_Walking_Cryonite_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }

    public void Play_Player_Sprinting_Sand_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = player_Sprinting_Sand_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void Play_Player_Sprinting_Grass_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = player_Sprinting_Grass_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void Play_Player_Sprinting_Water_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = player_Sprinting_Water_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void Play_Player_Sprinting_Stone_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = player_Sprinting_Stone_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void Play_Player_Sprinting_Wood_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = player_Sprinting_Wood_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void Play_Player_Sprinting_Cryonite_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = player_Sprinting_Cryonite_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    #endregion

    #region Tablet
    public void Play_Tablet_OpenTablet_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = tablet_OpenTablet_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void Play_Tablet_CloseTablet_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = tablet_CloseTablet_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void Play_Tablet_ChangeMenu_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = tablet_ChangeMenu_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    #endregion

    #region Inventory
    public void Play_Inventory_ItemHover_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = inventory_ItemHover_Clip;
            audioSource.pitch = 20f;
            audioSource.volume = 0.25f;
            audioSource.Play();
        }
    }
    public void Play_Inventory_PickupItem_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = inventory_PickupItem_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void Play_Inventory_DropItem_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = inventory_DropItem_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void Play_Inventory_MoveItem_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = inventory_MoveItem_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void Play_Inventory_ConsumeItem_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = inventory_ConsumeItem_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void Play_Inventory_EquipItem_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = inventory_EquipItem_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }

    public void Play_Inventory_InventoryIsFull_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = inventory_InventoryIsFull_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    #endregion

    #region Hotbar
    public void Play_Hotbar_ChangeSelectedItem_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = hotbar_ChangeSelectedItem_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void Play_Hotbar_AssignItemToHotbar_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = hotbar_AssignItemToHotbar_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void Play_Hotbar_RemoveItemFromHotbar_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = hotbar_RemoveItemFromHotbar_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    #endregion

    #region Crafting
    public void Play_Crafting_SelectCraftingItem_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = crafting_ChangeCraftingMenu_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void Play_Crafting_ChangeCraftingMenu_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = crafting_SelectCraftingItem_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void Play_Crafting_PerformCrafting_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = crafting_PerformCrafting_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void Play_Crafting_CannotCraft_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = crafting_CannotCraft_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    #endregion

    #region Skill Tree
    public void Play_SkillTree_HoverPerk_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = skillTree_HoverPerk_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void Play_SkillTree_CompletedPerk_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = skillTree_CompletedPerk_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    #endregion

    #region Equipped Item
    public void Play_EquippedItems_EquippedItemIsBroken_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = equippedItems_EquippedItemIsBroken_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    #endregion

    #region Axe Useage //Have yet to be implemented
    public void Play_AxeUsage_Tree_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = axeUsage_Tree_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void Play_AxeUsage_Cactus_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = axeUsage_Cactus_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    #endregion

    #region Pickaxe Useage //Have yet to be implemented
    public void Play_PickaxeUsage_Tungsten_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = pickaxeUsage_Tungsten_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void Play_PickaxeUsage_Stone_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = pickaxeUsage_Stone_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void Play_PickaxeUsage_Cryonite_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = pickaxeUsage_Cryonite_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void Play_PickaxeUsage_Gold_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = pickaxeUsage_Gold_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void Play_PickaxeUsage_Magnetite_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = pickaxeUsage_Magnetite_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void Play_PickaxeUsage_Viridian_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = pickaxeUsage_Viridian_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    #endregion

    #region Sword Usage //Have yet to be implemented
    public void Play_SwordUsage_Slashing_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = swordUsage_Slashing_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    #endregion

    #region Building
    public void Play_Building_Place_Wood_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = building_Place_Wood;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void Play_Building_Place_Stone_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = building_Place_Stone;
            audioSource.pitch = 0.7f;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void Play_Building_Place_Cryonite_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = building_Place_Cryonite;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    
    public void Play_Building_Remove_Wood_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = building_Remove_Wood;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void Play_Building_Remove_Stone_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = building_Remove_Stone;
            audioSource.pitch = 0.7f;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void Play_Building_Remove_Cryonite_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = building_Remove_Cryonite;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }

    public void Play_Building_CannotPlaceBlock_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = building_CannotPlaceBlock;
            audioSource.pitch = 0.5f;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void Play_Building_Place_MoveableObject_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = building_Place_MoveableObject;
            audioSource.pitch = 1f;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void Play_Building_Remove_MoveableObject_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = building_Remove_MoveableObject;
            audioSource.pitch = 1f;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    #endregion

    #region Chests
    public void Play_Chests_OpenSmallChest_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = chests_OpenSmallChest_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void Play_Chests_CloseSmallChest_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = chests_CloseSmallChest_Clip;
            audioSource.volume = 0.7f;
            audioSource.Play();
        }
    }

    public void Play_Chests_OpenMediumChest_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = chests_OpenMediumChest_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void Play_Chests_CloseMediumChest_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = chests_CloseMediumChest_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }

    public void Play_Chests_OpenBigChest_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = chests_OpenBigChest_Clip;
            audioSource.volume = 0.5f;
            audioSource.Play();
        }
    }
    public void Play_Chests_CloseBigChest_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = chests_CloseBigChest_Clip;
            audioSource.volume = 0.5f;
            audioSource.Play();
        }
    }
    #endregion

    #region InteractableObjects //"Ungoing" have yet to be implemented
    public void Play_InteractableObjects_OpenCraftingTable_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = InteractableObjects_OpenCraftingTable_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void Play_InteractableObjects_UngoingCraftingTable_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = InteractableObjects_UngoingCraftingTable_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void Play_InteractableObjects_CloseCraftingTable_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = InteractableObjects_CloseCraftingTable_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }

    public void Play_InteractableObjects_OpenSkillTreeTable_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = InteractableObjects_OpenSkillTreeTable_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void Play_InteractableObjects_UngoingSkillTreeTable_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = InteractableObjects_UngoingSkillTreeTable_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void Play_InteractableObjects_CloseSkillTreeTable_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = InteractableObjects_CloseSkillTreeTable_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }

    public void Play_InteractableObjects_OpenCropPlot_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = InteractableObjects_OpenCropPlot_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void Play_InteractableObjects_UngoingCropPlot_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = InteractableObjects_UngoingCropPlot_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    public void Play_InteractableObjects_CloseCropPlot_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = InteractableObjects_CloseCropPlot_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }

    public void Play_InteractableObjects_UngoingGhostTank_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = InteractableObjects_UngoingGhostTank_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }

    public void Play_InteractableObjects_UngoingExtractor_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = InteractableObjects_UngoingExtractor_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }

    public void Play_InteractableObjects_UngoingLamp_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = InteractableObjects_UngoingLamp_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }

    public void Play_InteractableObjects_UngoingSpotlight_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = InteractableObjects_UngoingSpotlight_Clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    #endregion

    #region Buffs
    public void Play_Buff_Deactivated_Clip()
    {
        if (audioSource != null)
        {
            audioSource.clip = buff_Deactivated;
            audioSource.volume = 1f;
            audioSource.Play();
        }
    }
    #endregion
}
