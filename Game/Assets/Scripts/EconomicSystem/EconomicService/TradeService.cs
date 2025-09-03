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

            var (buyerWallet, sellerWallet) = TakeWallet(fromInventoryLogic, shopContext);

            if (buyerWallet == null || sellerWallet == null)
            {
                ConsoleLogger.Error("Cant resolve wallets");
                return false;
            }
            
            ConsoleLogger.Info($"BuyerWallet: {buyerWallet.Balance}, SellerWallet: {sellerWallet.Balance}");

            var itemPrice = CalculateFinallyItemPrice(itemInstance.itemData.tradeInfo, coefficient, npcReputation, fromInventoryLogic, amountItem);
            ConsoleLogger.Info($"Item price {itemInstance.itemData.nameItem} = {itemPrice}");

            if (!CanBuy(buyerWallet, itemPrice))
            {
                ConsoleLogger.Error("Cant buy items");
                return false;
            }

            _moneyService.TransitMoney(sellerWallet, buyerWallet, itemPrice);

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
                
                ConsoleLogger.Info($"Player sell item price {itemPrice}");
            }
            else
            {
                itemPrice = _priceCalculatingService.CalculateItemPrice(itemTradeInfo.price, reputationCoef,
                    itemRarityKoef);
                ConsoleLogger.Info($"Npc sell item price {itemPrice}");
            }

            return itemPrice * amountItem;
        }

        private bool CanBuy(IWallet buyerWallet, int itemPrice)
        {
            ConsoleLogger.Info($"Can buy {buyerWallet.Balance >= itemPrice} where itemPrice is {itemPrice} and balance is {buyerWallet.Balance}");
            return buyerWallet.Balance >= itemPrice;
        }

        private (IWallet buyerWallet, IWallet sellerWallet) TakeWallet(AbstractInventoryLogic fromInventoryLogic, ShopContext shopContext)
        {
            if (fromInventoryLogic == shopContext.PrimaryInventory) return (shopContext.SecondaryWallet, shopContext.PrimaryWallet);
            
            if (fromInventoryLogic == shopContext.SecondaryInventory) return (shopContext.PrimaryWallet, shopContext.SecondaryWallet);
            
            return (null, null);
        }
    }
}