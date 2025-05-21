using System;
using DefaultNamespace.Enums;
using Enemy;
using PlayerNameSpace.Inventory;
using UnityEngine;


[CreateAssetMenu(fileName = "ItemScrObj", menuName = "Create/NewItem", order = 0)]
public abstract class ItemScrObj : ScriptableObject
{

    public virtual ItemData GetItemData()
    {
        return null;
    }
    
    public virtual void OnEquip(InventoryLogic inventoryLogic)
    {
        
    }

    public virtual void OnUnequip(InventoryLogic inventoryLogic)
    {
        
    }
}

