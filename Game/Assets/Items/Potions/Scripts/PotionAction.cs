using Enemy;
using Enemy.ItemTypeData;
using Items;
using Player.Inventory.InventoryInterface;
using PlayerNameSpace;
using UnityEngine;
using Zenject;

namespace Player.Inventory
{
    public class PotionAction : ItemAction
    {
        [Inject] private IRegenerationHealth _regenerationHealth;
        [Inject] private IInventoryRemove _inventoryRemove;
        
        public override void ActionItem(ItemInstance itemInstance)
        {
            Debug.Log("Potion Action");
            
            if (itemInstance.itemData is Potion potion)  
            {
                _regenerationHealth.Regeneration(potion.amount);
                _inventoryRemove.RemoveItem(itemInstance, 1);
            }
        }
    }
}