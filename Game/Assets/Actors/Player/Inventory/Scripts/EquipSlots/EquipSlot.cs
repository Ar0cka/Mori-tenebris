using Actors.Player.Inventory.Enums;
using Enemy;
using EventBusNamespace;
using Items.EquipArmour.Data;
using Player.Inventory;
using UnityEngine;


namespace Actors.Player.Inventory.Scripts.EquipSlots
{
    public class EquipSlot
    {
        public bool IsEquipped { get; private set;}
        public EquipItemType TypeItem { get; private set;}
        
        public ItemData ItemData { get; private set; }
        
        public EquipSlot(EquipItemType typeItem)
        {
            IsEquipped = false;
            TypeItem = typeItem;
        }
        
        public ItemData EquipItem(ItemData itemData)
        {
            #region SendUpdateEvents

            if (itemData is WeaponData weaponData)
            {
                EquipWeapon(weaponData);
            }

            if (itemData is ArmourData armourData)
            {
                EquipArmour(armourData);
            }

            #endregion
            
            if (ItemData == null)
            {
                IsEquipped = true;
                ItemData = itemData;
            }
            else
            {
                return ChangeItemInSlot(itemData);
            }
            
            return null;
        }

        private void EquipWeapon(WeaponData weaponData)
        {
            EventBus.Publish(new SendEquipWeaponEvent(weaponData.damage, weaponData.damageType));
        }

        private void EquipArmour(ArmourData armourData)
        {
            EventBus.Publish(new SendEquipArmourEvent(armourData.physicArmour, armourData.magicArmour));
        }
        
        public ItemData UnEquipItem(ItemData itemData)
        {
            if (ItemData == null || itemData == null) return null;
            
            var currentItem = ItemData;
            
            if (ItemData.nameItem == itemData.nameItem)
            {
                IsEquipped = false;
                ItemData = null;
            }
            
            return currentItem;
        }

        public ItemData ChangeItemInSlot(ItemData itemData)
        {
            var returnItem = ItemData;
            
            ItemData = itemData;
            
            return returnItem;
        }
    }
}