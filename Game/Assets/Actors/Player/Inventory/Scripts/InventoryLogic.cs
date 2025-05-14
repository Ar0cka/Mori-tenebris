using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Actors.Player.Inventory.Enums;
using Actors.Player.Inventory.Scripts.EquipSlots;
using Enemy;
using DefaultNamespace.Zenject;
using Player.Inventory.InventoryInterface;
using PlayerNameSpace.InventorySystem;
using Systems.DataLoader.Scripts;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace PlayerNameSpace.Inventory
{
    public class InventoryLogic : IInventoryAdder, IInventoryRemove
    {
        [Inject] private ISpawnProjectObject _itemFactory;

        private InventoryScrObj _inventoryScrObj;

        private GameObject _slotPrefab;
        private Transform _slotParent;
        private int _capacityInventory;

        private List<SlotData> slots = new List<SlotData>();
        private Dictionary<EquipSlotType, EquipSlotData> _equipSlot = new Dictionary<EquipSlotType, EquipSlotData>();

        public void Initialize(Transform slotParent, InventoryScrObj inventoryScrObj, List<GameObject> equipSlots)
        {
            #region InitializeInventory

            _inventoryScrObj = inventoryScrObj;

            var inventoryData = _inventoryScrObj.InventoryData.Clone();

            _slotPrefab = inventoryData.SlotPrefab;
            _slotParent = slotParent;
            _capacityInventory = inventoryData.CountSlots;

            #endregion

            for (int i = 0; i < _capacityInventory; i++)
            {
                var prefabSlot = _itemFactory.Create(_slotPrefab, _slotParent);
                slots.Add(new SlotData(prefabSlot, _itemFactory));
            }

            #region CreateEquipSlots

            var equipSlotType = Enum.GetValues(typeof(EquipSlotType));
            
            for (int i = 0; i < equipSlots.Count; i++)
            {
                if (equipSlotType.Length <= i) break;
                
                EquipSlotType slotType =(EquipSlotType)equipSlotType.GetValue(i);
                
                _equipSlot.Add(slotType, null);
            }

            foreach (var slotKey in _equipSlot.Keys)
            {
                _equipSlot[slotKey] = new EquipSlotData(slotKey);
            }

            #endregion
        }

        public void AddItemToInventory(ItemData itemData, int amount)
        {
            if (amount <= 0 || itemData == null)
            {
                Debug.LogError("item data = " + itemData + " amount add " + amount);
                return;
            }

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
                
                remaining = slot.AddItem(itemData, remaining);
            }

            return remaining;
        }

        private int CreateNewStacks(ItemData itemData, int amount)
        {
            int remaining = amount;

            foreach (var slot in slots)
            {
                if (remaining <= 0) break;

                slot.CreateNewItem(itemData);
                remaining = slot.AddItem(itemData, remaining);
            }

            return remaining;
        }

        public void RemoveItem(ItemData itemData, int amount)
        {
            int remaining = amount;

            foreach (var slot in slots)
            {
                if (remaining <= 0) break;

                remaining = slot.RemoveItem(itemData, remaining);
            }

            if (remaining > 0)
            {
                Debug.Log($"Cant find item in inventory. Couldn't remove item {remaining}");
            }
        }

        public void SelectEquipAction(EquipSlotType equipSlotType, ItemData itemData)
        {
            if (_equipSlot.TryGetValue(equipSlotType, out EquipSlotData equipSlotData))
            {
                if (equipSlotData.IsEquipped())
                {
                    ChangeItemInSlot(equipSlotType, itemData);
                }
                else
                {
                    EquipItem(equipSlotType, itemData);
                }
            }
        }
        
        private void EquipItem(EquipSlotType equipSlotType, ItemData itemData)
        {
            foreach (var slot in slots) //Заменить на другой алгоритм поиска
            {
                if (slot.CheckItemInSlot(itemData.nameItem))
                {
                    var currentItemData = slot.UnEquipItemData();
                    var currentItemObject = slot.UnEquipGameObject();
                    
                    if (_equipSlot.TryGetValue(equipSlotType, out EquipSlotData equipSlotData))
                    {
                        equipSlotData.EquipItem(currentItemObject, currentItemData);
                    }
                }
            }
        }

        private void ChangeItemInSlot(EquipSlotType equipSlotType, ItemData itemData)
        {
            
        }
    }
}