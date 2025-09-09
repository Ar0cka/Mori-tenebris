using System;
using System.Collections.Generic;
using Enemy;
using UnityEngine;

public class RecipesConfig : ScriptableObject
{
    [field:SerializeField] public GameObject RecipePrefab { get; private set; }
    
    [SerializeField] private ItemScrObj resultItem;
    [SerializeField] private List<RecipeData> recipes = new List<RecipeData>();
    
    public List<RecipeData> Recipes => recipes;
    public ItemData GetResultItemData() => resultItem.GetItemData();
}

[Serializable]
public class RecipeData
{
    [SerializeField] private ItemScrObj itemScrObj;
    
    [field:SerializeField] public int CountForCraft { get; private set; }

    public ItemData GetItemData() => itemScrObj.GetItemData();
    
}