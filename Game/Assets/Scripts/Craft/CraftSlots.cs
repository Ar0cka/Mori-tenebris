using Actors.Player.Inventory;
using DefaultNamespace.ShopPanel;
using Player.Inventory;
using Project.Service;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class CraftSlots : SlotUI
    {
        private GameObject _previewPrefab;
        private int _amount;
        private int _typeID;

        public CraftSlots(GameObject slotPrefab, IDestroyService destroyService, GameObject previewPrefab) : base(
            slotPrefab, destroyService)
        {
            _previewPrefab = previewPrefab;
        }

        public void ActivitySlot(bool activeStatus, Sprite previewSprite, int typeID)
        {
            _typeID = typeID;
            
            _slotPrefab.SetActive(activeStatus);
            
            Image previewImage = _previewPrefab.GetComponent<Image>();
            previewImage.sprite = previewSprite;
            
            _previewPrefab.SetActive(activeStatus); 
        }

        public void AddItemToSlot(ItemUI itemUI, int amount)
        {
            _amount += amount;
            SetItem(itemUI);
            
            _itemUI.UpdateUI(_amount);
            
            if (_amount > 1)
                itemUI.gameObject.SetActive(false);
        }
        
        public bool CheckSlotID(int typeID)
        {
            return _typeID == typeID;
        }

        public bool CheckSlotID(int typeID, int neededCount)
        {
            return typeID == _typeID && _amount >= neededCount;
        }
        
        public void ReturnItemToInventory()
        {
            _itemUI.GetCurrentInventory().AddItemToInventory(_itemUI.GetItemInstance(), _amount);
        }

        public void Close()
        {
            ReturnItemToInventory();
            _itemUI = null;
            _previewPrefab = null;
        }
    }
}