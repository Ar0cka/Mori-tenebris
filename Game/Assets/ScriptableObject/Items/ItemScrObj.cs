using System;
using DefaultNamespace.Enums;
using Enemy;
using Items.Data.Scripts;
using PlayerNameSpace.Inventory;
using UnityEngine;


[CreateAssetMenu(fileName = "ItemScrObj", menuName = "Create/NewItem", order = 0)]
public abstract class ItemScrObj : ScriptableObject
{
    [SerializeField] private GameObject spawnPrefab;
    [SerializeField] private ItemTradeInfo itemTradeInfo;
    
    public GameObject SpawnPrefab => spawnPrefab;
    
    public virtual ItemData GetItemData()
    {
        return null;
    }
}

