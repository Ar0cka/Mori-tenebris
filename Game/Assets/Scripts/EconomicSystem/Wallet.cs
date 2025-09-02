using DefaultNamespace.PlayerStatsOperation.SaveSystem;

namespace EconomicSystem
{
    public class Wallet : IWallet
    {
        public int Balance { get; private set; }

        public Wallet(int balance)
        {
            Balance = balance;
        }
        
        public void AddMoney(int amount)
        {
            Balance += amount;
        }
        
        public void RemoveMoney(int amount)
        {
            Balance -= amount;
        }
    }
    
    public interface IWallet
    {
        int Balance { get; }
        void AddMoney(int amount);
        void RemoveMoney(int amount);
    }
}