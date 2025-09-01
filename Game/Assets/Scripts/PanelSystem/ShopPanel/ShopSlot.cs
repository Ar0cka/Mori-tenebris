using Player.Inventory;
using Project.Service;
using UnityEngine;

namespace DefaultNamespace.ShopPanel
{
    public class ShopSlot
    {
        private GameObject _slotPrefab;
        private ItemUI _itemUI;

        private IDestroyService _destroyService;
        
        public ShopSlot(GameObject slotPrefab, IDestroyService destroyService)
        {
            _slotPrefab = slotPrefab;
            _destroyService = destroyService;
        }

        public void SetItem(ItemUI itemUI)
        {
            _itemUI = itemUI;
            _itemUI.transform.SetParent(_slotPrefab.transform);
            _itemUI.transform.position = _slotPrefab.transform.position;
            _itemUI.transform.localScale = new Vector3(1, 1, 1);
            _itemUI.UpdateUI();
        }

        public void Clear()
        {
            if (_itemUI == null) return;
            
            _destroyService.DestroyItem(_itemUI.gameObject);
            _itemUI = null;
        }
    }
}