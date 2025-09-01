using EconomicSystem;

namespace Project.Service.EconomicService
{
    public class MoneyService
    {
        public void TransitMoney(IWallet targetWallet, IWallet sourceWallet, int amount)
        {
            targetWallet.AddMoney(amount);
            sourceWallet.RemoveMoney(amount);
        }
    }
}