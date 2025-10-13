using System.Collections.Generic;
using Actors.Player.Inventory;
using DefaultNamespace.Zenject;
using Player.Inventory;
using Project.Service;
using Project.Service.Context;
using Project.Service.RendererRealize;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace DefaultNamespace
{
    public class CraftPanel : BasePanel
    {
        [SerializeField] private CraftItemPanel craftItemPanel;
        
        [Header("Craft renderer components")]
        [SerializeField] private InventoryScrObj inventoryScrObj;
        [SerializeField] private Transform playerInventoryPosition;
        
        [Header("Button")]
        [SerializeField] private Button exitButton;
        
        [Inject] private PanelController _panelController;
        [Inject] private ISpawnProjectObject _spawnProjectObject;
        [Inject] private IDestroyService _destroyService;
        [Inject] private IItemUIFactory _itemUIFactory;

        private CraftContext _craftContext;
        private CraftInventoryRenderer _craftInventoryRenderer;
        
        public void InitializeCraftPanel()
        {
            _craftInventoryRenderer = new CraftInventoryRenderer();
            _craftInventoryRenderer.Init(new CraftRendInitContext(inventoryScrObj, playerInventoryPosition), _spawnProjectObject, _destroyService, _itemUIFactory);
            craftItemPanel.InitializeCraftItemPanel(_craftInventoryRenderer);
            
            exitButton.onClick.AddListener(ClosePanel);
        }
        
        public override void OpenPanel(PanelContext panelContext = null)
        {
            craftItemPanel.gameObject.SetActive(false);
            
            inventoryObject.SetActive(true);
            _panelController.UpdatePanel<RecipesConfig>(craftItemPanel);
            
            _craftContext = (CraftContext)panelContext;
            
            if (_craftContext == null) return;
            
            craftItemPanel.SetCraftContext(_craftContext);
            
            RendererItems();
        }

        private void RendererItems()
        {
            var itemList = _craftInventoryRenderer.RedrawItems(_craftContext);

            foreach (var items in itemList)
            {
                items.RemoveListener();
            }
        }

        public void ClosePanel()
        {
            craftItemPanel.UnsetCraftContext();
            gameObject.SetActive(false);
            craftItemPanel.gameObject.SetActive(false);
            craftItemPanel.Close();
        }
    }

    public class CraftContext : PanelContext
    {
        public AbstractInventoryLogic PlayerInventory;

        public CraftContext(AbstractInventoryLogic playerInventory)
        {
            PlayerInventory = playerInventory;
        }
    }
}