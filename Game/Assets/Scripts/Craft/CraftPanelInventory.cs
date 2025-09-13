using System.Collections.Generic;
using ConsoleApp.Runtime;
using DefaultNamespace.ShopPanel;
using DefaultNamespace.Zenject;
using Player.Inventory;
using Project.Service;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class CraftPanelInventory
    {
        private List<CraftSlots> _slots = new();
        private Transform _slotParent;
        
        private ISpawnProjectObject  _spawnProjectObject;
        private IDestroyService _destroyService;
        
        private Dictionary<int, CraftSlots> _craftSlots = new();
        
        [Inject]
        public CraftPanelInventory(InventoryScrObj inventoryScrObj, Transform slotParent, GameObject previewPrefab, ISpawnProjectObject spawnProjectObject, IDestroyService destroyService)
        {
            _spawnProjectObject = spawnProjectObject;
            _destroyService = destroyService;
            
            _slotParent = slotParent;
            InitializeSlots(inventoryScrObj,  previewPrefab);
        }

        private void InitializeSlots(InventoryScrObj inventoryScrObj,  GameObject previewPrefab)
        {
            for (int i = 0; i < inventoryScrObj.InventoryData.CountSlots; i++)
            {
                var slotPrefab = _spawnProjectObject.Create(inventoryScrObj.InventoryData.SlotPrefab, _slotParent);
                var viewPrefab = _spawnProjectObject.Create(previewPrefab, slotPrefab.transform);
                
                slotPrefab.SetActive(false);
                viewPrefab.SetActive(false);
                
                _slots.Add(new CraftSlots(slotPrefab, _destroyService, viewPrefab));
            }
        }

        public void OpenCraft(List<RecipeData> recipes)
        {
            for (int i = 0; i < recipes.Count; i++)
            {
                _slots[i].ActivitySlot(true, recipes[i].GetItemData().iconItem, recipes[i].GetItemData().typeID);
                _craftSlots[recipes[i].GetItemData().typeID] = _slots[i];
            }
        }

        public void AddItemOnCraftSlot(ItemUI itemUI, int amount = 1)
        {
            var inventory = itemUI.GetCurrentInventory();
            inventory.RemoveItem(itemUI.GetItemInstance(), amount);

            if (_craftSlots.TryGetValue(itemUI.GetItemInstance().itemData.typeID, out var slot))
            {
                slot.AddItemToSlot(itemUI, amount);
            }
        }

        public void CraftItem(RecipesConfig recipes)
        {
            foreach (var item in recipes.Recipes)
            {
                if (_craftSlots.TryGetValue(item.GetItemData().typeID, out var slot))
                {
                    if (!slot.CheckSlotID(item.GetItemData().typeID, item.CountForCraft))
                    {
                        ConsoleLogger.Info("Not all components are crafted");
                    }
                }
            }
            
            ConsoleLogger.Info("Successfully crafted");
        }

        public void CraftPanelClose()
        {
            foreach (var slot in _slots)
            {
                slot.Close();
            }
        }
    }

    public class CraftSlotData
    {
        public int TypeID;
        public int Amount;
        public ItemUI ItemUI;

        public CraftSlotData(int typeID, int amount, ItemUI itemUI)
        {
            this.TypeID = typeID;
            this.Amount = amount;
            this.ItemUI = itemUI;
        }
    }
}