using Actors.NPC.Inventory;
using Actors.NPC.SpecialPanel;
using Actors.Player.Inventory;
using Items;
using Player.Inventory;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace DefaultNamespace.ShopPanel
{
    public class ShopPanel : MonoBehaviour
    {
        [SerializeField] private GameObject shopPanel;
        [SerializeField] private SpecialPanelType specialPanelType;
        [SerializeField] private ItemPanelSystem itemPanel;
        
        [SerializeField] private InventoryPanel inventoryPanel;

        [SerializeField] private Transform leftInventory;
        [SerializeField] private Transform rightInventory;

        [SerializeField] private Button openShopButton;
        
        [Inject] private ItemRouterService _itemRouterService;
        [Inject] private PanelController _panelController;

        private ShopContext _shopContext;

        public void Initialize(NpcInventoryPanel npcInventoryPanel)
        {
            openShopButton.onClick.AddListener(() => SendShopContext(npcInventoryPanel));
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
        public AbstractInventoryLogic PrimaryInventory { get; }
        public AbstractInventoryLogic SecondaryInventory { get; }

        public ShopContext(AbstractInventoryLogic primaryInventory, AbstractInventoryLogic secondaryInventory)
        {
            PrimaryInventory = primaryInventory;
            SecondaryInventory = secondaryInventory;
        }
    }
}