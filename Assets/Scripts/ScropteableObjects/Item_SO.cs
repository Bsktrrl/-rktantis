using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

[CreateAssetMenu(fileName = "Item", menuName = "Item", order = 1)]
public class Item_SO : ScriptableObject
{
    public List<Item> itemList = new List<Item>();
}

[Serializable]
public class Item
{
    [Header("General")]
    public Items itemName;
    public ItemCategories categoryName;
    public ItemSubCategories subCategoryName;
    public Vector2 itemSize;
    [TextArea (5, 10)] public string itemDescription;

    [Header("Sprites")]
    public Sprite hotbarSprite;
    public List<Sprite> itemSpriteList = new List<Sprite>();
    public List<Sprite> itemSelected_SpriteList = new List<Sprite>();

    [Header("Stats")]
    public bool isActive = true;
    public bool isConsumeable;
    public bool isEquipableInHand;
    public bool isEquipableClothes;

    [Header("HealthStats")]
    public int mainHealthHeal;
    public int heatresistanceHealthHeal;
    public int hungerHealthHeal;
    public int thirstHealthHeal;

    public int heatColdRegulator;
    public int heatColdRegulatorDuration;

    [Header("durability_Max")]
    public int durability_Max;

    [Header("OrePower")]
    public int orePower;

    [Header("Prefabs")]
    public GameObject worldObjectPrefab;
    public GameObject equippedPrefab;

    [Header("Animations")]
    public Animation idleAnimation;
    public Animation actionAnimation;

    [Header("Crafting")]
    public bool isCrafteable;
    public List<CraftingRequirements> craftingRequirements = new List<CraftingRequirements>();
}

[Serializable]
public class CraftingRequirements
{
    public Items itemName;
    public int amount;
}