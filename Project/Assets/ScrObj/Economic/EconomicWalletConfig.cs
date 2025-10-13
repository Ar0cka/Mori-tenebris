using EconomicSystem;
using UnityEngine;

namespace ScrObj.Economic
{
    [CreateAssetMenu(fileName = "Wallet config", menuName = "Economic/Wallet config")]
    public class EconomicWalletConfig : ScriptableObject
    {
        [SerializeField] private int startBalance;
        
        public Wallet CreateNewWallet() => new Wallet(startBalance);
    }
}