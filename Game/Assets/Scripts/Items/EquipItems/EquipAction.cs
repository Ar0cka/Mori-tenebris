using Player.Inventory;

namespace Items.EquipArmour
{
    public class EquipAction : ItemAction
    {
        public override void Action(ItemInstance itemInstance, ItemActionContext context)
        {
            context.ItemRouterService.EquipItem(context.InventoryLogic, context.PlayerEquipSystem, itemInstance);
        }
    }
}