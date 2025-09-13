using System.Collections.Generic;
using Actors.Player.Inventory;
using DefaultNamespace.Zenject;
using Player.Inventory;
using Project.Service;
using Project.Service.Context;
using Project.Service.RendererRealize;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class CraftPanel : BasePanel
    {
        [SerializeField] private CraftItemPanel craftItemPanel;
        
        [Header("Craft renderer components")]
        [SerializeField] private InventoryScrObj inventoryScrObj;
        [SerializeField] private Transform playerInventoryPosition;
        
        [Inject] private PanelController _panelController;
        [Inject] private ISpawnProjectObject _spawnProjectObject;
        [Inject] private IDestroyService _destroyService;

        private CraftContext _craftContext;
        private CraftInventoryRenderer _craftInventoryRenderer;
        
        public void InitializeCraftPanel()
        {
            craftItemPanel.InitializeCraftItemPanel();
            _craftInventoryRenderer = new CraftInventoryRenderer();
            _craftInventoryRenderer.Init(new CraftRendInitContext(inventoryScrObj, playerInventoryPosition), _spawnProjectObject, _destroyService);
        }
        
        public override void OpenPanel(PanelContext panelContext = null)
        {
            inventoryObject.SetActive(true);
            _panelController.UpdatePanel<RecipesConfig>(craftItemPanel);
            
            _craftContext = (CraftContext)panelContext;
            
            if (_craftContext == null) return;
            
            RendererItems();
        }

        private void RendererItems()
        {
            var itemList = _craftInventoryRenderer.RedrawItems(_craftContext);

            foreach (var items in itemList)
            {
                craftItemPanel.RegisterNewListenerOnItem(items);
            }
        }

        public void ClosePanel()
        {
            craftItemPanel.Close();
            gameObject.SetActive(false);
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