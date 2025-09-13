using System;
using Actors.NPC.DialogSystem;
using Actors.NPC.Inventory;
using Actors.NPC.NpcStateSystem;
using Actors.NPC.SpecialPanel;
using Actors.Player.Inventory;
using ConsoleApp.Runtime;
using DefaultNamespace.Zenject;
using EconomicSystem;
using Items;
using Items.Data.Scripts;
using Player.Inventory;
using Project.Service;
using Project.Service.Context;
using Project.Service.EconomicService;
using ScrObj.Economic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Zenject;

namespace DefaultNamespace.ShopPanel
{
    public class ShopPanel : BasePanel
    {
        [Header("Shop panel settings")]
        [SerializeField] private GameObject shopPanel;
        [SerializeField] private SpecialPanelType specialPanelType;
        [SerializeField] private ItemPanelSystem itemPanel;

        [Header("Settings for inventory in shop")]
        [SerializeField] private Transform leftInventory;
        [SerializeField] private Transform rightInventory;
        [SerializeField] private InventoryScrObj shopInventoryConfig;
        [SerializeField] private TextMeshProUGUI npcBalance;
        [SerializeField] private TextMeshProUGUI targetBalance;
        
        [Header("Npc wallet")]
        [SerializeField] private WalletRealize walletRealize;
        [SerializeField] private EconomicCoefficient coefficient;
        
        [Header("Npc settings")]
        [SerializeField] private NpcController npcController;
        
        [Inject] private ItemRouterService _itemRouterService;
        [Inject] private PanelController _panelController;
        [Inject] private ISpawnProjectObject _itemFactory;
        [Inject] private IDestroyService _destroyService;
        [Inject] private TradeService _tradeService;

        private ShopContext _shopContext;
        private ShopInventoryRenderer _shopInventoryRenderer;
        
        private NpcInventoryPanel _npcInventoryPanel;
        private DialogFSM _dialogFsm;

        public void Initialize(NpcInventoryPanel npcInventoryPanel, DialogFSM dialogFsm)
        {
            _npcInventoryPanel = npcInventoryPanel;
            
            _shopInventoryRenderer = new ShopInventoryRenderer();
            _shopInventoryRenderer.Init(new InventoryRendererInitContext(shopInventoryConfig, leftInventory, rightInventory), _itemFactory, _destroyService);
            
            _dialogFsm = dialogFsm;
            
            shopPanel.SetActive(false);
            itemPanel.gameObject.SetActive(false);
        }

        public void SendShopContext(InventoryPanel playerInventoryPanel, IWallet targetWallet)
        {
            OpenShopPanel(new ShopContext(_npcInventoryPanel.GetInventoryLogic(), walletRealize.Wallet, 
                playerInventoryPanel.GetInventoryLogic(), targetWallet));
        }

        private void OpenShopPanel(ShopContext shopContext)
        {
            if (shopPanel.activeInHierarchy)
                CloseShopPanel();
            
            _shopContext = shopContext;
            
            UpdateBalance(shopContext.PrimaryWallet, shopContext.SecondaryWallet);
            
            _panelController.UpdatePanel(itemPanel);
            shopPanel.SetActive(true); 
            
            _shopInventoryRenderer.Redraw(shopContext);
        }

        public void ItemRouter(AbstractInventoryLogic inventoryFrom, ItemInstance item, int amountItems)
        {
            AbstractInventoryLogic targetInventory = TakeTargetInventory(inventoryFrom);

            if (targetInventory == null)
            {
#if UNITY_EDITOR
                Debug.LogWarning("Not find target inventory");
#endif
                return;
            }

            if (targetInventory.HaveFreeSlot())
            {
                if (_tradeService.ItemBuy(inventoryFrom, item, _shopContext, coefficient,
                        npcController.GetNpcRepSystem().GetCurrentNpcReputationState(), amountItems))
                {
                    _itemRouterService.TransitItem(inventoryFrom, targetInventory, item, amountItems);
                    _shopInventoryRenderer.Redraw(_shopContext);
                    UpdateBalance(_shopContext.PrimaryWallet, _shopContext.SecondaryWallet);
                }
            }
            else
            {
                ConsoleLogger.Error("Not have slots");
            }
        }

        private void UpdateBalance(IWallet primaryWallet, IWallet secondaryWallet)
        {
            if (npcBalance == null || targetBalance == null || primaryWallet == null || secondaryWallet == null)
            {
                ConsoleLogger.Error("Not find components for update");
                return;
            }
            
            npcBalance.text = primaryWallet.Balance.ToString();
            targetBalance.text = secondaryWallet.Balance.ToString();
        }
        
        public void CloseShopPanel()
        { 
            _dialogFsm.OnClosePanel?.Invoke();
            _shopContext = null;
            gameObject.SetActive(false);
        }

        private AbstractInventoryLogic TakeTargetInventory(AbstractInventoryLogic inventoryFrom)
        {
            if (_shopContext.PrimaryInventory == inventoryFrom) return _shopContext.SecondaryInventory;
            
            if (_shopContext.SecondaryInventory == inventoryFrom) return _shopContext.PrimaryInventory;
            
            return null;
        }
    }

    public class ShopContext : PanelContext
    {
        public AbstractInventoryLogic PrimaryInventory;
        public AbstractInventoryLogic SecondaryInventory;
        public IWallet PrimaryWallet;
        public IWallet SecondaryWallet;

        public ShopContext(AbstractInventoryLogic primaryInventory, IWallet primaryWallet, AbstractInventoryLogic secondaryInventory, IWallet secondaryWallet)
        {
            PrimaryInventory = primaryInventory;
            SecondaryInventory = secondaryInventory;
            PrimaryWallet = primaryWallet;
            SecondaryWallet = secondaryWallet;
        }
    }
}