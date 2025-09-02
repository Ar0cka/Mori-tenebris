using System;
using Actors.NPC.NpcStateSystem;
using Actors.Player.Inventory;
using ConsoleApp.Runtime;
using DefaultNamespace.ShopPanel;
using EconomicSystem;
using Items;
using Items.Data.Scripts;
using Player.Inventory;
using ScrObj.Economic;
using Zenject;

namespace Project.Service.EconomicService
{
    public class TradeService
    {
        private MoneyService _moneyService;
        private PriceCalculatingService _priceCalculatingService;

        [Inject]
        public TradeService(MoneyService moneyService, PriceCalculatingService priceCalculatingService, ItemRouterService itemRouterService)
        {
            _moneyService = moneyService;
            _priceCalculatingService = priceCalculatingService;
        }

        public bool ItemBuy(AbstractInventoryLogic fromInventoryLogic, ItemInstance itemInstance,
            ShopContext shopContext, EconomicCoefficient coefficient, NpcReputationEnum npcReputation, int amountItem)
        {
            if (itemInstance == null || fromInventoryLogic == null)
            {
                ConsoleLogger.Error("Not find instance from inventory and item");
                return false;
            }

            var buyerWallet = TakeWallet(fromInventoryLogic, shopContext);
            var sellerWallet = TakeWallet(fromInventoryLogic, shopContext);

            if (buyerWallet == null || sellerWallet == null)
            {
                ConsoleLogger.Error("Cant resolve wallets");
                return false;
            }

            var itemPrice = CalculateFinallyItemPrice(itemInstance.itemData.tradeInfo, coefficient, npcReputation, fromInventoryLogic, amountItem);

            if (!CanBuy(buyerWallet, itemPrice))
            {
                ConsoleLogger.Error("Cant buy items");
                return false;
            }

            _moneyService.TransitMoney(buyerWallet, sellerWallet, itemPrice);

            return true;
        }

        private int CalculateFinallyItemPrice(ItemTradeInfo itemTradeInfo, EconomicCoefficient coefficient,
            NpcReputationEnum npcReputation, AbstractInventoryLogic fromInventoryLogic, int amountItem)
        {
            float reputationCoef = coefficient.ReputationCoefficient[npcReputation];
            float itemRarityKoef = coefficient.ItemRarityCoefficient[itemTradeInfo.rarity];
            
            if (reputationCoef == 0) reputationCoef = 1.2f;
            if (itemRarityKoef == 0) itemRarityKoef = 1.2f;
            
            int itemPrice = 0;
            if (fromInventoryLogic.ParentInventory == InventoryObjectType.Player)
            {
                itemPrice = _priceCalculatingService.CalculateSellItemPrice(itemTradeInfo.price, reputationCoef,
                    itemRarityKoef);
            }
            else
            {
                itemPrice = _priceCalculatingService.CalculateItemPrice(itemTradeInfo.price, reputationCoef,
                    itemRarityKoef);
            }

            return itemPrice * amountItem;
        }

        private bool CanBuy(IWallet buyerWallet, int itemPrice)
        {
            return buyerWallet.Balance >= itemPrice;
        }

        private IWallet TakeWallet(AbstractInventoryLogic fromInventoryLogic, ShopContext shopContext)
        {
            return fromInventoryLogic == shopContext.PrimaryInventory
                ? shopContext.SecondaryWallet
                : shopContext.PrimaryWallet;
        }
    }
}