using System.Collections.Generic;
using Data;
using DefaultNamespace.Zenject;
using Player.Inventory.InventoryInterface;
using PlayerNameSpace.InventorySystem;
using UnityEngine;
using Zenject;

namespace PlayerNameSpace.Inventory
{
    public class InventoryLogic : IInventoryAdder, IInventorySearch, IInventoryRemove
    {
        private const string INVENTORY_SCR_OBJ_NAME = "Inventory/InvetoryScrObject";
        
        [Inject] private IItemsFactory _itemFactory;
        
        private InventoryScrObj inventoryScrObj;
        
        private GameObject _slotPrefab;
        private Transform _slotParent;
        private int _capacityInventory;
        
        private List<SlotData> slots = new List<SlotData>();
        
        public void Initialize(Transform slotParent)
        {
            #region InitializeInventory

            inventoryScrObj = Resources.Load<InventoryScrObj>(INVENTORY_SCR_OBJ_NAME);

            var inventoryData = inventoryScrObj.InventoryData.Clone();

            _slotPrefab = inventoryData.SlotPrefab;
            _slotParent = slotParent;
            _capacityInventory = inventoryData.CountSlots;

            #endregion
            
            for (int i = 0; i < _capacityInventory; i++)
            {
                var prefabSlot = _itemFactory.Create(_slotPrefab, _slotParent);
                slots.Add(new SlotData(prefabSlot, _itemFactory));
            }
        }

        public void AddItemToInventory(ItemData itemData, int amount)
        {
            if (amount <= 0 || itemData == null) return;

            int remainingAmount = amount;
            
            remainingAmount = AddItem(itemData, remainingAmount);
            
            if (remainingAmount > 0)
            {
                remainingAmount = CreateNewStacks(itemData, remainingAmount);
            }

            if (remainingAmount > 0)
            {
                Debug.Log($"Inventory full. Couldn't add {remainingAmount} of {itemData.nameItem}");
            }
        }

        private int AddItem(ItemData itemData, int amount)
        {
            int remaining = amount;
            
            foreach (var slot in slots)
            {
                if (remaining <= 0) break;
                
                if (slot.CanAddItem(itemData))
                {
                    remaining = slot.AddItem(itemData, remaining);
                }
            }
            
            return remaining;
        }

        private int CreateNewStacks(ItemData itemData, int amount)
        {
            int remaining = amount;
            
            foreach (var slot in slots)
            {
                if (remaining <= 0) break;
                
                if (!slot.IsOccupied)
                {
                    slot.CreateNewItem(itemData);
                    remaining = slot.AddItem(itemData, remaining);
                }
            }
            
            return remaining;
        }

        public void RemoveItem(ItemData itemData, int amount)
        {
            int remaining = amount;

            foreach (var slot in slots)
            {
                if (remaining <= 0) break;

                if (slot.CanRemoveItem(itemData))
                {
                    remaining = slot.RemoveItem(itemData, remaining);
                }
            }

            if (remaining > 0)
            {
                Debug.Log($"Cant find item in inventory. Couldn't remove item {remaining}");
            }
        }
        
        public SlotData FindItemOnInventory(string nameItem)
        {
            List<SlotData> occupiedSlots = slots.FindAll(e => e.IsOccupied && !e.IsFull);

            foreach (var slot in occupiedSlots)
            {
                if (slot.CurrentItemData.nameItem == nameItem)
                {
                    return slot;
                }
            }

            return null;
        }
    }
}