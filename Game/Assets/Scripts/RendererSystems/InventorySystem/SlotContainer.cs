using System.Collections.Generic;
using Actors.Player.Inventory;
using DefaultNamespace.ShopPanel;
using DefaultNamespace.Zenject;
using Player.Inventory;
using UnityEngine;

namespace Project.Service
{
    public class SlotContainer
    {
        private readonly List<SlotUI> _slots = new();
        private readonly ISpawnProjectObject _factory;
        private readonly IItemUIFactory _itemUIFactory;
        
        private readonly Transform _slotParent; 
        private readonly GameObject _slotPrefab;
        public SlotContainer(Transform root, GameObject slotPrefab, int slotCount, ISpawnProjectObject factory,
            IDestroyService destroyService, IItemUIFactory itemUIFactory)
        {
            _itemUIFactory = itemUIFactory;
            _factory = factory;
            _slotPrefab = slotPrefab;
            _slotParent = root;
            
            for (int i = 0; i < slotCount; i++)
            {
                CreateNewSlot(destroyService);
            }
        }

        public List<ItemUI> Render(AbstractInventoryLogic inventoryFrom)
        {
            ClearSlots();

            List<ItemInstance> items = inventoryFrom.GetAllItems();
            List<ItemUI> itemUIList = new List<ItemUI>();
            
            for (int i = 0; i < _slots.Count; i++)
            {
                if (i < items.Count)
                {
                    Debug.Log($"Items count = {items.Count} and current index = {i}");
                    
                    var item = _slots[i].SetItem(new ItemUIContext(items[i], inventoryFrom));
                    
                    if (item != null)
                        itemUIList.Add(item);
                }
                else
                {
                    _slots[i].Clear();
                    break;
                }
            }
            
            return itemUIList;
        }

        public void CreateNewSlot(IDestroyService destroyService)
        {
            var slotGo = _factory.Create(_slotPrefab, _slotParent);
            var slot = new SlotUI(slotGo, destroyService, _itemUIFactory.CreateItemUI());
            _slots.Add(slot);
        }
        
        public void ClearSlots()
        {
            foreach (var slot in _slots)
            {
                slot.Clear();
            }
        }
    }
}