using System;
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
    public class CraftInventory
    {
        private List<CraftSlots> _slots = new();
        private Transform _slotParent;
        
        private ISpawnProjectObject  _spawnProjectObject;
        private IDestroyService _destroyService;
        
        private Dictionary<int, CraftSlots> _craftSlots = new();
        
        [Inject]
        public CraftInventory(InventoryScrObj inventoryScrObj, Transform slotParent, GameObject previewPrefab,
            ISpawnProjectObject spawnProjectObject, IDestroyService destroyService, IItemUIFactory itemUIFactory)
        {
            _spawnProjectObject = spawnProjectObject;
            _destroyService = destroyService;
            
            _slotParent = slotParent;
            InitializeSlots(inventoryScrObj,  previewPrefab, itemUIFactory);
        }

        private void InitializeSlots(InventoryScrObj inventoryScrObj,  GameObject previewPrefab, IItemUIFactory itemUIFactory)
        {
            for (int i = 0; i < inventoryScrObj.InventoryData.CountSlots; i++)
            {
                var slotPrefab = _spawnProjectObject.Create(inventoryScrObj.InventoryData.SlotPrefab, _slotParent);
                var viewPrefab = _spawnProjectObject.Create(previewPrefab, slotPrefab.transform);
                
                slotPrefab.SetActive(false);
                viewPrefab.SetActive(false);
                
                _slots.Add(new CraftSlots(slotPrefab, _destroyService, viewPrefab, itemUIFactory.CreateItemUI()));
            }
        }

        public void OpenCraft(List<RecipeData> recipes)
        {
            for (int i = 0; i < recipes.Count; i++)
            {
                _slots[i].ActivitySlot(true, recipes[i].GetItemData().iconItem, recipes[i].GetItemData().typeID);
                _craftSlots[recipes[i].GetItemData().typeID] = _slots[i];
                ConsoleLogger.Info($"Craft slot type = {recipes[i].GetItemData().typeID}");
            }
        }

        public void AddItemOnCraftSlot(ItemUI itemUI, int amount = 1)
        {
            Debug.Log($"Item adding on craft slot with amount: {amount}");
            Debug.Log($"Item have on player inventory {itemUI.GetCurrentInventory().FindItem(itemUI.GetItemInstance())}");
            
            if (_craftSlots.TryGetValue(itemUI.GetItemInstance().itemData.typeID, out var slot))
            {
                var inventory = itemUI.GetCurrentInventory();
                inventory.RemoveItem(itemUI.GetItemInstance(), amount);
                slot.AddItemToSlot(itemUI, amount);
            }
            else
            {
                ConsoleLogger.Error("Craft slot type doesn't exist");
                return;
            }
        }

        public bool CheckCanCraft(RecipesConfig recipes)
        {
            foreach (var item in recipes.Recipes)
            {
                if (_craftSlots.TryGetValue(item.GetItemData().typeID, out var slot))
                {
                    if (!slot.CheckSlotID(item.GetItemData().typeID, item.CountForCraft))
                    {
                        ConsoleLogger.Error($"Not component {item.GetItemData().typeID}");
                        return false;
                    }
                }
            }
            
            ConsoleLogger.Info("CanCraft");
            return true;
        }

        public void CraftItem(RecipesConfig recipes, CraftContext craftContext)
        {
            List<ItemInstance> items = new List<ItemInstance>();
            
            foreach (var recipeData in recipes.Recipes)
            {
                if (_craftSlots.TryGetValue(recipeData.GetItemData().typeID, out var slot))
                {
                    var item = slot.ItemUseForCraft(recipeData.CountForCraft);
                    
                    if (item != null) 
                        items.Add(item);
                }
            }

            foreach (var item in items)
            {
                craftContext.PlayerInventory.AddItemToInventory(item, item.amount);
            }
            
            craftContext.PlayerInventory.AddItemToInventory(new ItemInstance(recipes.GetResultItemData()), 1);                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                
        }
        
        public void CraftPanelClose()
        {
            _craftSlots.Clear();
            
            foreach (var slot in _slots)
            {
                slot.Close();
            }
        }
    }
}