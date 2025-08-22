using Actors.Player.Inventory;
using Actors.Player.Inventory.EquipSlots;
using Items;

namespace Player.Inventory
{
    public class ItemActionContext
    {
        public AbstractInventoryLogic InventoryLogic { get; private set; }
        public PlayerEquipSystem PlayerEquipSystem { get; private set; }
        public ItemRouterService ItemRouterService { get; private set; }

        public ItemActionContext(AbstractInventoryLogic inventoryLogic, PlayerEquipSystem playerEquipSystem, ItemRouterService itemRouterService)
        {
            InventoryLogic = inventoryLogic;
            PlayerEquipSystem = playerEquipSystem;
            ItemRouterService = itemRouterService;
        }
    }
}