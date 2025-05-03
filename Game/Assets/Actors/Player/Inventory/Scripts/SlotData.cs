using System;
using Data;
using DefaultNamespace.Zenject;
using Player.Inventory;
using UnityEngine;
using Zenject;

namespace PlayerNameSpace.InventorySystem
{
    public class SlotData
    {
        private IItemsFactory _itemFactory;
        public bool IsFull { get; private set; }
        public bool IsOccupied { get; private set; }
        
        public ItemData CurrentItemData { get; private set; }
        
        private int _currentItemCount;
       
        private GameObject _slotObject;
        private GameObject _itemPrefab;
        private ItemSettings _itemSettings;

        public SlotData(GameObject prefab, IItemsFactory itemFactory)
        {
            _itemFactory = itemFactory;
            IsFull = false;
            IsOccupied = false;
            _currentItemCount = 0;
            
            _slotObject = prefab;
            _itemPrefab = null;
            _itemSettings = null;    
        }

        public void CreateNewItem(ItemData itemData)
        {
            if (IsOccupied || IsFull) return;

            CurrentItemData = itemData;

            Debug.Log("Creating new item " + itemData.prefabItem.name);
            
            Debug.Log(_itemFactory);
            
            _itemPrefab = _itemFactory.Create(itemData.prefabItem, _slotObject.transform);
            _itemSettings = _itemPrefab.GetComponent<ItemSettings>();
            
            IsOccupied = true;
        }
        
        public int AddItem(ItemData itemData, int amountItems)
        {
            if (!IsFull)
            {
                int space = itemData.maxStackInSlot - _currentItemCount;
                int itemAddCount = Mathf.Min(amountItems, space);
                
                _currentItemCount += itemAddCount;
                IsFull = CurrentItemData.maxStackInSlot == _currentItemCount;
                
                _itemSettings?.UpdateUI(_currentItemCount);
                
                return amountItems - itemAddCount;
            }
            
            return amountItems;
        }

        public int RemoveItem(ItemData itemData, int amountItems)
        {
            if (itemData == null) return amountItems;

            int itemRemove = Mathf.Min(amountItems, _currentItemCount);

            _currentItemCount -= itemRemove;
            
            _itemSettings.UpdateUI(_currentItemCount);

            if (_currentItemCount <= 0)
            {
                ClearSlot();
            }
            
            return amountItems - itemRemove;
        }
        
        public bool CanAddItem(ItemData itemData)
        {
            return IsOccupied && !IsFull && CurrentItemData.nameItem == itemData.nameItem && _currentItemCount < itemData.maxStackInSlot;
        }

        public bool CanRemoveItem(ItemData itemData)
        {
            return CurrentItemData.nameItem == itemData.nameItem && _currentItemCount > 0;
        }

        private void ClearSlot()
        {
            IsFull = false;
            IsOccupied = false;
        
            _currentItemCount = 0;
            
            CurrentItemData = null;
            _itemPrefab = null;
            
            _itemSettings.DeleteObjectFromSlot();
            _itemSettings = null;
        }
    }
}