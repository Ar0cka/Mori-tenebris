using Actors.Player.Inventory;
using EconomicSystem;
using Player.Inventory;
using PlayerNameSpace.Inventory;
using UnityEngine;

namespace PlayerContextProviders
{
    public class PlayerDialogContextProvider : MonoBehaviour, IPlayerContextProviders<PlayerDialogContext>
    {
        private InventoryPanel _inventoryPanel;
        private WalletRealize _wallet;

        public void Initialize(InventoryPanel inventoryPanel, WalletRealize wallet)
        {
            _inventoryPanel = inventoryPanel;
            _wallet = wallet;
        }
        
        public PlayerDialogContext GetPlayerContext()
        {
            return new PlayerDialogContext(_inventoryPanel, _wallet.Wallet);
        }
    }

    public class PlayerDialogContext
    {
        public InventoryPanel InventoryPanel { get; private set; }
        public IWallet Wallet { get; private set; }

        public PlayerDialogContext(InventoryPanel inventoryPanel, IWallet wallet)
        {
            InventoryPanel = inventoryPanel;
            Wallet = wallet;
        }
    }
}