using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Actors.Player.Inventory.Enums;
using Actors.Player.Inventory.Scripts.EquipSlots;
using DefaultNamespace.Enums;
using Enemy;
using DefaultNamespace.Zenject;
using Player.Inventory;
using Player.Inventory.InventoryInterface;
using PlayerNameSpace.InventorySystem;
using SlotSystem;
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
        private Dictionary<EquipItemType, EquipSlotData> _equipSlot = new Dictionary<EquipItemType, EquipSlotData>();

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

            var equipSlotType = Enum.GetValues(typeof(EquipItemType));

            for (int i = 0; i < equipSlots.Count; i++)
            {
                if (equipSlotType.Length <= i) break;

                EquipItemType itemType = (EquipItemType)equipSlotType.GetValue(i);

                _equipSlot.Add(itemType, new EquipSlotData(itemType, equipSlots[i]));
            }

            #endregion
        }

        public void AddItemToInventory(ItemInstance itemInstance, int amount)
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

        private int AddItem(ItemInstance itemInstance, int amount)
        {
            int remaining = amount;

            foreach (var slot in slots)
            {
                if (remaining <= 0) break;

                remaining = slot.AddItem(itemInstance, remaining);
            }

            return remaining;
        }
        
        private int CreateNewStacks(ItemInstance itemInstance, int amount)
        {
            int remaining = amount;

            foreach (var slot in slots)
            {
                if (remaining <= 0) break;

                slot.CreateNewItem(itemInstance);
                remaining = slot.AddItem(itemInstance, remaining);
            }

            return remaining;
        }

        public void RemoveItem(ItemInstance itemInstance, int amount)
        {
            int remaining = amount;

            foreach (var slot in slots)
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

        public void SelectEquipAction(EquipItemType equipItemType, ItemInstance itemInstance)
        {
            if (_equipSlot.TryGetValue(equipItemType, out EquipSlotData equipSlotData))
            {
                EquipItem(itemInstance, equipSlotData);
            }
        }

        private void EquipItem(ItemInstance itemInstance, EquipSlotData equipSlotData)
        {
            foreach (var slot in slots)
            {
                if (slot.CheckItemInSlot(itemInstance.itemID))
                {
                    var currentItemData = slot.TakeItemDataFromSlot();
                    var currentItemObject = slot.TakePrefabFromSlot();
                    
                    var item = equipSlotData.EquipItem(currentItemData);
                    var prefab = equipSlotData.EquipItemPrefab(currentItemObject);

                    if (item == null)
                    {
                        break;
                    }
                    
                    ChangeItemInSlot(slot, item, prefab);
                    break;
                }
            }
        }

        private void ChangeItemInSlot(SlotData slotData, ItemInstance itemInstance, GameObject itemPrefab)
        {
            slotData.RegistrateData(itemInstance);
            slotData.ChangeItemSettings(itemPrefab);
            slotData.AddItem(itemInstance, 1);
        }

        public void UnEquipItem(EquipItemType equipItemType, ItemInstance itemData)
        {
            if (!HaveSlot()) return;
            
            if (_equipSlot.TryGetValue(equipItemType, out EquipSlotData equipSlotData))
            {
                var item = equipSlotData.UnEquipItem(itemData);
                var prefab = equipSlotData.UnEquipItemObject();

                if (item != null && prefab != null)
                {
                    foreach (var slot in slots)
                    {
                        if (slot.IsEmpty())
                        {
                            ChangeItemInSlot(slot, item, prefab);
                            break;
                        }
                    }
                }
            }
        }

        private bool HaveSlot()
        {
            foreach (var slot in slots)
            {
                if (slot.IsEmpty())
                {
                    return true;
                }
            }

            return false;
        }
    }
}