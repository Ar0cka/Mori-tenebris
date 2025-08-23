using Actors.NPC.Inventory;
using Actors.NPC.SpecialPanel;
using Actors.Player.Inventory;
using DefaultNamespace.Zenject;
using Items;
using Player.Inventory;
using Project.Service;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace DefaultNamespace.ShopPanel
{
    public class ShopPanel : MonoBehaviour
    {
        [Header("Shop panel settings")]
        [SerializeField] private GameObject shopPanel;
        [SerializeField] private SpecialPanelType specialPanelType;
        [SerializeField] private ItemPanelSystem itemPanel;
        [SerializeField] private Button openShopButton;
        
        [Header("Test player inventory")]
        [SerializeField] private InventoryPanel inventoryPanel;

        [Header("Settings for inventory in shop")]
        [SerializeField] private Transform leftInventory;
        [SerializeField] private Transform rightInventory;
        [SerializeField] private InventoryScrObj shopInventoryConfig;
        
        [Inject] private ItemRouterService _itemRouterService;
        [Inject] private PanelController _panelController;
        [Inject] private ISpawnProjectObject _itemFactory;
        [Inject] private IDestroyService _destroyService;

        private ShopContext _shopContext;
        private InventoryRenderer _inventoryRenderer;

        public void Initialize(NpcInventoryPanel npcInventoryPanel)
        {
            openShopButton.onClick.AddListener(() => SendShopContext(npcInventoryPanel));

            _inventoryRenderer = new InventoryRenderer();
            _inventoryRenderer.Init(new InventoryRendererInitContext(shopInventoryConfig, leftInventory, rightInventory), _itemFactory, _destroyService);
        }

        private void SendShopContext(NpcInventoryPanel npcInventoryPanel)
        {
            OpenShopPanel(new ShopContext(npcInventoryPanel.GetInventoryLogic(), inventoryPanel.GetInventoryLogic()));
        }
        
        private void OpenShopPanel(ShopContext shopContext)
        {
            if (shopPanel.activeInHierarchy)
                CloseShopPanel();
            
            _shopContext = shopContext;
            
            _panelController.UpdatePanel(itemPanel);
            shopPanel.SetActive(true); 
            
            _inventoryRenderer.Redraw(shopContext);
        }

        public void RouteItem(AbstractInventoryLogic inventoryFrom, ItemInstance item, int amountItems)
        {
            AbstractInventoryLogic targetInventory = TakeTargetInventory(inventoryFrom);

            if (targetInventory == null)
            {
#if UNITY_EDITOR
                Debug.LogWarning("Not find target inventory");
#endif
                return;
            }
            
            _itemRouterService.TransitItem(inventoryFrom, targetInventory, item, amountItems);
            _inventoryRenderer.Redraw(_shopContext);
        }
        
        public void CloseShopPanel()
        { 
            gameObject.SetActive(false);
        }

        private AbstractInventoryLogic TakeTargetInventory(AbstractInventoryLogic inventoryFrom)
        {
            if (_shopContext.PrimaryInventory == inventoryFrom) return _shopContext.SecondaryInventory;
            
            if (_shopContext.SecondaryInventory == inventoryFrom) return _shopContext.PrimaryInventory;
            
            return null;
        }
    }

    public class ShopContext
    {
        public AbstractInventoryLogic PrimaryInventory;
        public AbstractInventoryLogic SecondaryInventory;

        public ShopContext(AbstractInventoryLogic primaryInventory, AbstractInventoryLogic secondaryInventory)
        {
            PrimaryInventory = primaryInventory;
            SecondaryInventory = secondaryInventory;
        }
    }
}