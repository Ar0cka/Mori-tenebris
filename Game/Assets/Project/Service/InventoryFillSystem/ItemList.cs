using System;
using System.Collections.Generic;
using Player.Inventory;
using UnityEngine;
using UnityEngine.Serialization;

namespace Service
{
    [CreateAssetMenu(fileName = "itemList", menuName = "Inventory/ItemList", order = 0)]
    public class ItemList : ScriptableObject
    {
        [field: SerializeField] public List<ItemConfig> Items { get; private set; }
    }
    
    [Serializable]
    public class ItemConfig
    {
        public ItemScrObj itemScrObj;
        public int amount;
    }
    
}