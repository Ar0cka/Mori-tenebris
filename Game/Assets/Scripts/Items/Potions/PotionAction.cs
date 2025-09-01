using Enemy;
using Enemy.ItemTypeData;
using Items;
using Player.Inventory;
using Player.Inventory.InventoryInterface;
using PlayerNameSpace;
using UnityEngine;
using Zenject;

namespace Items.Potions.Scripts
{
    public class PotionAction : ItemAction
    {
        [Inject] private IRegenerationHealth _regenerationHealth;

        public int ItemUsedForOneUse { get; private set; } = 1;

        public override void Action(ItemInstance itemInstance, ItemActionContext context)
        {
            if (itemInstance.itemData is Potion potion)  
            {
                _regenerationHealth.Regeneration(potion.healthAmount);
                context.ItemRouterService.RemoveItem(context.InventoryLogic, itemInstance, ItemUsedForOneUse);
            }
        }
    }
}