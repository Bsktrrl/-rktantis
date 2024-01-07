using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemCategory", menuName = "ItemCategory", order = 1)]
public class ItemCategory_SO : ScriptableObject
{
    public List<ItemCategory> ItemCategoryList = new List<ItemCategory>();
}

[Serializable]
public class ItemCategory
{
    public ItemCategories categoryName;
    public Sprite categorySprite;

    [Header("SubCategories")]
    public List<ItemSubCategories> subCategoryName = new List<ItemSubCategories>();
}