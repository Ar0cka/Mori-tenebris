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
        private ItemSettings _itemSettings;

        public SlotView(GameObject slotPrefab, ISpawnProjectObject itemFactory)
        {
            _slotObject = slotPrefab;
            _itemFactory = itemFactory;
        }
        
        public void CreateNewItem(ItemData itemData)
        {
            _itemPrefab = _itemFactory.Create(itemData.prefabItem, _slotObject.transform);
            _itemSettings = _itemPrefab.GetComponent<ItemSettings>();
            Debug.Log(_itemSettings);
        }
        
        public void UpdateUI(int currentItemAmount)
        {
            _itemSettings.UpdateUI(currentItemAmount);
        }

        public void ChangeItem(GameObject itemPrefab, ItemSettings itemSettings)
        {
            _itemPrefab = itemPrefab;
            _itemSettings = itemSettings;
            
            _itemPrefab.transform.SetParent(_slotObject.transform);
            _itemPrefab.transform.position = _slotObject.transform.position;
        }
        
        public GameObject UnEquipItemObject()
        {
            var equipItem = _itemPrefab;
            _itemPrefab = null;
            _itemSettings = null;
            return equipItem;
        }
        
        public void ClearSlotView()
        {
            _itemPrefab = null;
            _itemSettings.DeleteObjectFromSlot();
            _itemSettings = null;
        }

        public bool HaveItem()
        {
            return _itemPrefab != null;
        }
    }
}