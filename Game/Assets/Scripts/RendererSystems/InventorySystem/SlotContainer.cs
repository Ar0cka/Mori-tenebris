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
        
        private readonly Transform _slotParent; 
        private readonly GameObject _slotPrefab;
        public SlotContainer(Transform root, GameObject slotPrefab, int slotCount, ISpawnProjectObject factory, IDestroyService destroyService)
        {
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
                    var item = _factory.Create(items[i].itemData.prefabItemUI);
                    var itemUI = item.GetComponent<ItemUI>();
                    itemUI.InitializeItemSettings(items[i], inventoryFrom);
                    _slots[i].SetItem(itemUI);
                    itemUIList.Add(itemUI);
                }
                else
                {
                    break;
                }
            }
            
            return itemUIList;
        }

        public void CreateNewSlot(IDestroyService destroyService)
        {
            var slotGo = _factory.Create(_slotPrefab, _slotParent);
            var slot = new SlotUI(slotGo, destroyService);
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