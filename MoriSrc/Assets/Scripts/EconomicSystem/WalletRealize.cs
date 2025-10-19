using DefaultNamespace.PlayerStatsOperation.SaveSystem;
using ScrObj.Economic;
using UnityEngine;
using Zenject;

namespace EconomicSystem
{
    public class WalletRealize : MonoBehaviour
    {
        [SerializeField] private EconomicWalletConfig config;

        [Inject] private ISaveAndLoad _saveAndLoad;
        
        public Wallet Wallet { get; private set; }

        public void Initialize()
        {
            Wallet = config.CreateNewWallet();
        }
    }
}