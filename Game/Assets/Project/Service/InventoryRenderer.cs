using System.Collections.Generic;
using Actors.Player.Inventory;
using DefaultNamespace.ShopPanel;
using DefaultNamespace.Zenject;
using Player.Inventory;
using UnityEngine;
using Zenject;

namespace Project.Service
{
    public class SlotContainer
    {
        private readonly List<ShopSlot> _slots = new();
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

        public void Render(AbstractInventoryLogic inventoryFrom)
        {
            ClearSlots();

            List<ItemInstance> items = inventoryFrom.GetAllItems();
            
            for (int i = 0; i < _slots.Count; i++)
            {
                if (i < items.Count)
                {
                    var item = _factory.Create(items[i].itemData.prefabItemUI);
                    var itemUI = item.GetComponent<ItemUI>();
                    itemUI.InitializeItemSettings(items[i], inventoryFrom);
                    _slots[i].SetItem(itemUI);
                }
                else
                {
                    break;
                }
            }
        }

        public void CreateNewSlot(IDestroyService destroyService)
        {
            var slotGo = _factory.Create(_slotPrefab, _slotParent);
            var slot = new ShopSlot(slotGo, destroyService);
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
    
    public class InventoryRenderer
    {
        private SlotContainer _leftPanel;
        private SlotContainer _rightPanel;

        public void Init(InventoryRendererInitContext ctx, ISpawnProjectObject factory, IDestroyService destroyService)
        {
            _leftPanel = new SlotContainer(ctx.LeftInventory, ctx.ShopInventoryConfig.InventoryData.SlotPrefab, ctx.ShopInventoryConfig.InventoryData.CountSlots, factory, destroyService);
            _rightPanel = new SlotContainer(ctx.RightInventory, ctx.ShopInventoryConfig.InventoryData.SlotPrefab, ctx.ShopInventoryConfig.InventoryData.CountSlots, factory, destroyService);
        }

        public void Redraw(ShopContext ctx)
        {
            _leftPanel.Render(ctx.PrimaryInventory);
            _rightPanel.Render(ctx.SecondaryInventory);
        }
    }

    public class InventoryRendererInitContext
    {
        public readonly InventoryScrObj ShopInventoryConfig;
        public readonly Transform LeftInventory;
        public readonly Transform RightInventory;

        public InventoryRendererInitContext(InventoryScrObj shopInventoryConfig, Transform leftInventory, Transform rightInventory)
        {
            ShopInventoryConfig = shopInventoryConfig;
            LeftInventory = leftInventory;
            RightInventory = rightInventory;
        }
    }
}