using System.Collections.Generic;
using DefaultNamespace.Zenject;
using Player.Inventory;
using Player.Inventory.InventoryInterface;
using PlayerNameSpace.InventorySystem;
using UnityEngine;

namespace Actors.Player.Inventory
{
    public abstract class AbstractInventoryLogic : IInventoryAdder, IInventoryRemove
    {
        protected ISpawnProjectObject ItemFactory;
        
        protected InventoryScrObj InventoryScrObj;
        protected GameObject SlotPrefab;
        protected Transform SlotParent;
        protected int CapacityInventory;
        
        protected List<SlotData> Slots = new List<SlotData>();
        
        public abstract void Initialize(Transform slotParent, InventoryScrObj inventoryScrObj);
        
        protected virtual void BaseInit(Transform slotParent, InventoryScrObj inventoryScrObj)
        {
            #region InitializeInventory

            InventoryScrObj = inventoryScrObj;

            var inventoryData = InventoryScrObj.InventoryData.Clone();

            SlotPrefab = inventoryData.SlotPrefab;
            SlotParent = slotParent;
            CapacityInventory = inventoryData.CountSlots;

            #endregion

            for (int i = 0; i < CapacityInventory; i++)
            {
                var prefabSlot = ItemFactory.Create(SlotPrefab, SlotParent);
                Slots.Add(new SlotData(prefabSlot, ItemFactory));
            }
        }
        
        public virtual void AddItemToInventory(ItemInstance itemInstance, int amount)
        {
            if (amount <= 0 || itemInstance == null)
            {
                Debug.LogError("item data = " + itemInstance + " amount add " + amount);
                return;
            }

            int remainingAmount = amount;

            remainingAmount = AddItem(itemInstance, remainingAmount);

            if (remainingAmount > 0)
            {
                remainingAmount = CreateNewStacks(itemInstance, remainingAmount);
            }

            if (remainingAmount > 0)
            {
                Debug.Log($"Inventory full. Couldn't add {remainingAmount} of {itemInstance.itemData.nameItem}");
            }
        }
        
        protected virtual int AddItem(ItemInstance itemInstance, int amount)
        {
            int remaining = amount;

            foreach (var slot in Slots)
            {
                if (remaining <= 0) break;

                remaining = slot.AddItem(itemInstance, remaining);
            }

            return remaining;
        }
        
        protected virtual int CreateNewStacks(ItemInstance itemInstance, int amount)
        {
            int remaining = amount;

            foreach (var slot in Slots)
            {
                if (remaining <= 0) break;

                slot.CreateNewItem(itemInstance);
                remaining = slot.AddItem(itemInstance, remaining);
            }

            return remaining;
        }

        public virtual void RemoveItem(ItemInstance itemInstance, int amount)
        {
            int remaining = amount;

            foreach (var slot in Slots)
            {
                if (remaining <= 0)
                {
                    Debug.Log("Break");
                    break;
                }

                remaining = slot.RemoveItem(itemInstance, remaining);
            }

            if (remaining > 0)
            {
                Debug.Log($"Cant find item in inventory. Couldn't remove item {remaining}");
            }
        }
    }
}