using Data;
using Data.ItemTypeData;
using Items;
using Player.Inventory.InventoryInterface;
using PlayerNameSpace;
using Zenject;

namespace Player.Inventory
{
    public class PotionAction : ItemAction
    {
        [Inject] private IRegenerationHealth _regenerationHealth;
        
        public override void ActionItem(ItemScrObj itemScr, SlotData slotData)
        {
            base.ActionItem(itemScr, slotData);

            if (itemScr is PotionScr potion)  
            {
                _regenerationHealth.Regeneration(potion.Potion.amount);
                slotData.RemoveItemInSlot(itemScr.ItemData, 1);
            }
        }
    }
}