using Actors.Player.Inventory;
using Player.Inventory;
using PlayerNameSpace.Inventory;
using UnityEngine;

namespace PlayerContextProviders
{
    public class PlayerDialogContextProvider : MonoBehaviour, IPlayerContextProviders<PlayerDialogContext>
    {
        private InventoryPanel _inventoryPanel;

        public void Initialize(InventoryPanel inventoryPanel)
        {
            _inventoryPanel = inventoryPanel;
        }
        
        public PlayerDialogContext GetPlayerContext()
        {
            return new PlayerDialogContext(_inventoryPanel);
        }
    }

    public class PlayerDialogContext
    {
        public InventoryPanel InventoryPanel { get; private set; }

        public PlayerDialogContext(InventoryPanel inventoryPanel)
        {
            InventoryPanel = inventoryPanel;
        }
    }
}