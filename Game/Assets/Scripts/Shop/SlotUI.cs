using Player.Inventory;
using Project.Service;
using UnityEngine;

namespace DefaultNamespace.ShopPanel
{
    public class SlotUI
    {
        protected GameObject _slotPrefab;
        protected ItemUI _itemUI;

        protected IDestroyService _destroyService;
        
        public SlotUI(GameObject slotPrefab, IDestroyService destroyService)
        {
            _slotPrefab = slotPrefab;
            _destroyService = destroyService;
        }

        public virtual void SetItem(ItemUI itemUI)
        {
            if (_itemUI == null) 
                _itemUI = itemUI;
            
            itemUI.transform.SetParent(_slotPrefab.transform);
            itemUI.transform.position = _slotPrefab.transform.position;
            itemUI.transform.localScale = new Vector3(1, 1, 1);
            _itemUI.UpdateUI(_itemUI.GetItemInstance().amount);
        }

        public void Clear()
        {
            if (_itemUI == null) return;
            
            _destroyService.DestroyItem(_itemUI.gameObject);
            _itemUI = null;
        }
    }
}