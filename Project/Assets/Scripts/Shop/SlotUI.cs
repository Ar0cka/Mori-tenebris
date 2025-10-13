using Actors.Player.Inventory;
using ConsoleApp.Runtime;
using Player.Inventory;
using Project.Service;
using UnityEngine;

namespace DefaultNamespace.ShopPanel
{
    public class SlotUI
    {
        protected GameObject _slotPrefab;
        protected ItemUI _itemUI;

        protected bool IsInit = false;
        
        public SlotUI(GameObject slotPrefab, IDestroyService destroyService, ItemUI itemUI)
        {
            _slotPrefab = slotPrefab;
            
            _itemUI = itemUI;
            
            _itemUI.transform.SetParent(slotPrefab.transform);
            _itemUI.transform.position = _slotPrefab.transform.position;
            _itemUI.transform.localScale = new Vector3(1, 1, 1);
            
            _itemUI.gameObject.SetActive(false);
        }

        public ItemUI SetItem(ItemUIContext itemUIContext)
        {
            if (_itemUI == null)
            {
                ConsoleLogger.Error("Not item ui on slot UI");
                return null;
            }
            
            Clear();
            
            _itemUI.InitializeItemSettings(itemUIContext.ItemInstance, itemUIContext.InventoryLogic);
            _itemUI.gameObject.SetActive(true);
            _itemUI.CountUIUpdate(_itemUI.GetItemInstance().amount);
            
            IsInit = true;
            
            return _itemUI;
        }

        public void Clear()
        {
            _itemUI.ClearItem();
            _itemUI.gameObject.SetActive(false);
            IsInit = false;
        }
    }

    public struct ItemUIContext
    {
        public ItemInstance ItemInstance;
        public AbstractInventoryLogic InventoryLogic;

        public ItemUIContext(ItemInstance itemInstance, AbstractInventoryLogic inventoryLogic)
        {
            ItemInstance = itemInstance;
            InventoryLogic = inventoryLogic;
        }
    }
}