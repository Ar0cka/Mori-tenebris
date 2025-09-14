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

        public CraftSlots(GameObject slotPrefab, IDestroyService destroyService, GameObject previewPrefab, ItemUI itemUI) : base(
            slotPrefab, destroyService, itemUI)
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
            Debug.Log("AddItemToSlot");
            
            if (!IsInit)
            {
                SetItem(new ItemUIContext(itemUI.GetItemInstance(), itemUI.GetCurrentInventory()));
            }
            
            _amount += amount;
            _typeID = itemUI.GetItemInstance().itemData.typeID;
            
            _itemUI.CountUIUpdate(_amount);
            
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
        
        private void ReturnItemToInventory()
        {
            if (_itemUI != null && _amount > 0) 
                _itemUI.GetCurrentInventory().AddItemToInventory(_itemUI.GetItemInstance(), _amount);
        }

        public ItemInstance ItemUseForCraft(int neededCount)
        {
            _amount -= neededCount;
            ItemInstance item = _itemUI.GetItemInstance();

            if (_amount > 0)
            {
                item.amount = _amount;
            }

            Clear();
            _amount = 0;
            
            return item;
        }

        public void Close()
        {
            ReturnItemToInventory();
            _amount = 0;
            Clear();
        }
    }
}