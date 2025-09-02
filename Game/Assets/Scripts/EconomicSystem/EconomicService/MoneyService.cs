using Actors.NPC.NpcStateSystem;
using Actors.Player.Inventory;
using ConsoleApp.Runtime;
using DefaultNamespace.ShopPanel;
using EconomicSystem;
using Items.Data.Scripts;
using Player.Inventory;
using ScrObj.Economic;
using Zenject;

namespace Project.Service.EconomicService
{
    public class MoneyService
    {
        private PriceCalculatingService _priceCalculatingService;
        
        [Inject]
        public MoneyService(PriceCalculatingService priceCalculatingService)
        {
            _priceCalculatingService = priceCalculatingService;
        }
        
        public void TransitMoney(IWallet targetWallet, IWallet sourceWallet, int amount)
        {
            targetWallet.AddMoney(amount);
            sourceWallet.RemoveMoney(amount);
        }
        
        public void AddMoney(IWallet targetWallet, int amount)
        {
            targetWallet.AddMoney(amount);
        }

        public void RemoveMoney(IWallet targetWallet, int amount)
        {
            targetWallet.RemoveMoney(amount);
        }
    }
}