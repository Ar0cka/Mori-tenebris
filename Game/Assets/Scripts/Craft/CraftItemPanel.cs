using System;
using System.Collections.Generic;
using ConsoleApp.Runtime;
using Enemy;
using JetBrains.Annotations;
using Player.Inventory;
using Project.Service.RendererRealize;
using Service;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace DefaultNamespace
{
    public class CraftItemPanel : ItemPanelSystem, IPanelOpen<RecipesConfig>
    {
        [Header("Data")]
        [SerializeField] private InventoryScrObj craftSettings;

        [Header("Components")] 
        [SerializeField] private Transform slotParent;
        [SerializeField] private GameObject previewPrefab;
        
        [Header("Craft UI")] 
        [SerializeField] private Image itemIcon;
        [SerializeField] private Button craftButton;
        
        [Inject] private ZenjectClassFactory _classFactory;
        
        private CraftInventory _craftInventory;
        private CraftContext _craftContext;
        private CraftInventoryRenderer _craftInventoryRenderer;
        
        private RecipesConfig _config;

        public void InitializeCraftItemPanel(CraftInventoryRenderer craftRenderer)
        {
            Debug.Assert(slotParent != null, "slotParent is null");
            _craftInventory = _classFactory.Create<CraftInventory>(craftSettings, slotParent, previewPrefab);
            _craftInventoryRenderer = craftRenderer;
        }
        
        public void Open(RecipesConfig itemRecipesConfig)
        {
            _config = itemRecipesConfig;
            
            panelObject.SetActive(true); 
            _craftInventory.OpenCraft(itemRecipesConfig.Recipes);
            
            ItemData itemData = itemRecipesConfig.GetResultItemData();
            itemIcon.sprite = itemData.iconItem;
            itemNameText.text = itemData.nameItem;
            itemDescriptionText.text = itemData.description;
            
            craftButton.onClick.AddListener(PanelAction);
            closeButton.onClick.AddListener(Close);
            closeButton.onClick.AddListener(CloseDraw);
            
            DrawItems();
        }

        public void SetCraftContext(CraftContext craftContext)
        {
            if (craftContext != null)
                _craftContext = craftContext;
            else
                ConsoleLogger.Error("craftContext is null");
        }

        public void UnsetCraftContext()
        {
            _craftContext = null;
        }
        
        protected override void PanelAction()
        {
            if (_craftInventory.CheckCanCraft(_config))
            {
                _craftInventory.CraftItem(_config, _craftContext);
                DrawItems();
            }
            else
            {
                ConsoleLogger.Error("Craft failed"); //Отображение панели с ошибкой
            }
        }

        #region DrawRegion

        private void CloseDraw()
        {
            var list = _craftInventoryRenderer.RedrawItems(_craftContext);
            
            foreach (var item in list)
            {
                item.RemoveListener();
            }
        }
        
        private void DrawItems()
        {
            var list = _craftInventoryRenderer.RedrawItems(_craftContext);
            
            foreach (var item in list)
            {
                item.CustomListener(() =>
                {
                    _craftInventory.AddItemOnCraftSlot(item, 1);
                    return null;
                });
            }
        }

        #endregion
        
        public override void Close()
        {
            _craftInventory.CraftPanelClose();
            craftButton.onClick.RemoveAllListeners();
            base.Close();
        }
    }
}