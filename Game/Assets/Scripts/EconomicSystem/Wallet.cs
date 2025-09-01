namespace EconomicSystem
{
    public class Wallet : IMoneyDisplay, IWallet
    {
        public int Balance { get; private set; }

        public void AddMoney(int amount)
        {
            Balance += amount;
        }
        
        public void RemoveMoney(int amount)
        {
            Balance -= amount;
        }
        
        public int GetBalance() => Balance;
    }

    public interface IMoneyDisplay
    {
        int GetBalance();
    }
    public interface IWallet
    {
        int Balance { get; }
        void AddMoney(int amount);
        void RemoveMoney(int amount);
    }
}