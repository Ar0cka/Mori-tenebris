using Actors.Player.Inventory;
using Enemy;
using DefaultNamespace.Zenject;
using Player.Inventory;
using UnityEngine;
using Zenject;

namespace SlotSystem
{
    public class SlotView
    {
        private ISpawnProjectObject _itemFactory;
        
        private GameObject _slotObject;
        private GameObject _itemPrefab;
        private ItemUI _itemUI;
        
        public SlotView(GameObject slotPrefab, ISpawnProjectObject itemFactory)
        {
            _slotObject = slotPrefab;
            _itemFactory = itemFactory;
        }
        
        public void CreateNewItem(ItemInstance itemInstance, AbstractInventoryLogic currentInventory)
        {
            _itemPrefab = _itemFactory.Create(itemInstance.itemData.prefabItemUI, _slotObject.transform);
            _itemUI = _itemPrefab.GetComponent<ItemUI>();
            _itemUI.InitializeItemSettings(itemInstance, currentInventory);
        }
        
        public void UpdateUI(int currentItemAmount)
        {
            _itemUI.UpdateUI(currentItemAmount);
        }

        public void ChangeItem(GameObject itemPrefab, ItemUI itemUI)
        {
            _itemPrefab = itemPrefab;
            _itemUI = itemUI;
            
            _itemPrefab.transform.SetParent(_slotObject.transform);
            _itemPrefab.transform.position = _slotObject.transform.position;
        }
        
        public GameObject UnEquipItemObject()
        {
            var equipItem = _itemPrefab;
            _itemPrefab = null;
            _itemUI = null;
            return equipItem;
        }
        
        public void ClearSlotView()
        {
            _itemPrefab = null;
            _itemUI.DeleteObjectFromSlot();
            _itemUI = null;
        }

        public bool HaveItem()
        {
            return _itemPrefab != null;
        }
    }
}