using System;
using DefaultNamespace.Enums;
using Items.Data.Scripts;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemy
{
    [Serializable]
    public class ItemData
    {
        public int typeID;
        public string nameItem;
        public string description;
        
        public int maxStackInSlot;
        public ItemTypes itemTypes;
        
        public Sprite iconItem;
        public GameObject prefabItemUI;

        public ItemTradeInfo tradeInfo;
        
        public object Clone() => this.MemberwiseClone();
    }
}