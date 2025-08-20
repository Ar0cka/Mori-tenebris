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
    public class PotionAction : ItemAction, ICollectable
    {
        [Inject] private IRegenerationHealth _regenerationHealth;

        public int ItemUsedForOneUse { get; private set; } = 1;

        public override int Action(ItemInstance itemInstance)
        {
            Debug.Log("Potion Action");
            
            if (itemInstance.itemData is Potion potion)  
            {
                _regenerationHealth.Regeneration(potion.healthAmount);
            }

            return ItemUsedForOneUse;
        }
    }

    public interface ICollectable
    {
        public int ItemUsedForOneUse { get; }
    }
}