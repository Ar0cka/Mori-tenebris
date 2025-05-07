using Enemy;
using Enemy.ItemTypeData;
using Items;
using Player.Inventory.InventoryInterface;
using PlayerNameSpace;
using Zenject;

namespace Player.Inventory
{
    public class PotionAction : ItemAction
    {
        [Inject] private IRegenerationHealth _regenerationHealth;
        [Inject] private IInventoryRemove _inventoryRemove;
        
        public override void ActionItem(ItemScrObj itemScr)
        {
            base.ActionItem(itemScr);

            if (itemScr is PotionScr potion)  
            {
                _regenerationHealth.Regeneration(potion.Potion.amount);
                _inventoryRemove.RemoveItem(itemScr.ItemData, 1);
            }
        }
    }
}