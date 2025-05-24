using System;
using Enemy;
using DefaultNamespace.Zenject;
using Player.Inventory;
using SlotSystem;
using UnityEngine;
using Zenject;

namespace PlayerNameSpace.InventorySystem
{
    [Serializable]
    public class SlotData
    {
        private Slot _slot;
        private SlotView _slotView;
        public SlotData(GameObject slotPrefab, ISpawnProjectObject spawnProjectObject)
        {
            _slot = new Slot();
            _slotView = new SlotView(slotPrefab, spawnProjectObject);
        }

        public void CreateNewItem(ItemData itemData)
        {
            if (itemData == null) return;

            if (_slot.CanCreate())
            {
                _slotView.CreateNewItem(itemData);
                _slot.CreateNewItem(itemData);
            }
        }
        
        public int AddItem(ItemData itemData, int amountItems)
        {
            if (itemData == null || amountItems <= 0) return amountItems;

            int result = amountItems;
            
            if (_slot.CheckItemInSlot(itemData.nameItem))
            {
                result = _slot.AddNewItem(itemData, result);
                
                _slotView.UpdateUI(_slot.CurrentItemCount());
            }
            

            return result;
        }

        public int RemoveItem(ItemData itemData, int amountItems)
        {
            if (itemData == null || amountItems <= 0) return amountItems;

            int result = amountItems;

            if (_slot.CheckItemInSlot(itemData.nameItem))
            {
                result = _slot.RemoveItem(itemData, result);
                
                if (_slot.CurrentItemCount() <= 0)
                {
                    _slotView.ClearSlotView();
                }
                else
                {
                    _slotView.UpdateUI(_slot.CurrentItemCount());
                }
            }

            return result;
        }

        public bool CheckItemInSlot(string nameItem) => _slot.CheckItemInSlot(nameItem);

        public void ChangeItemSettings(GameObject itemPrefab)
        {
            if (itemPrefab == null) return;
            
            var itemSetting = itemPrefab.GetComponent<ItemSettings>();
            
            if (itemSetting == null) return;
            
            _slotView.ChangeItem(itemPrefab, itemSetting);
        }

        public ItemData TakeItemDataFromSlot()
        {
            return _slot.UnEquipData();
        }

        public GameObject TakePrefabFromSlot()
        {
            return _slotView.UnEquipItemObject();
        }

        public bool IsEmpty()
        {
            if (!_slot.IsOccupied && !_slotView.HaveItem())
            {
                return true;
            }

            return false;
        }
    }
}