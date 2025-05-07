using System;
using DefaultNamespace.Enums;
using Enemy;
using UnityEngine;


[CreateAssetMenu(fileName = "ItemScrObj", menuName = "Create/NewItem", order = 0)]
public class ItemScrObj : ScriptableObject
{
    [SerializeField] private ItemData _itemData;
    public ItemData ItemData => _itemData;
}

