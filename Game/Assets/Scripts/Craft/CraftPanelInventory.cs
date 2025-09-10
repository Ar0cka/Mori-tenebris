using System.Collections.Generic;
using DefaultNamespace.ShopPanel;
using DefaultNamespace.Zenject;
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
            if (inventoryScrObj == null)
                Debug.LogError("InventoryScrObj is null");
            if (previewPrefab == null)
                Debug.LogError("preview is null");
            if (_slotParent == null)
                Debug.LogError("_slotParent is null");
            if (_spawnProjectObject == null)
                Debug.LogError("_spawnProjectObject is null");
            if (_destroyService == null)
                Debug.LogError("_destroyService is null");
            
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
                _slots[i].ActivitySlot(true, recipes[i].GetItemData().iconItem);
            }
        }
    }
}