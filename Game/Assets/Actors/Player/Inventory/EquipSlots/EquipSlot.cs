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
        
        public ItemInstance ItemData { get; private set; }
        
        public EquipSlot(EquipItemType typeItem)
        {
            IsEquipped = false;
            TypeItem = typeItem;
        }
        
        public ItemInstance EquipItem(ItemInstance itemInstance)
        {
            if (itemInstance == null) return null;
            
            #region SendUpdateEvents

            if (itemInstance.itemData is WeaponData weaponData)
            {
                EquipWeapon(weaponData);
            }

            if (itemInstance.itemData is ArmourData armourData)
            {
                EquipArmour(armourData);
            }

            #endregion
            
            if (ItemData == null)
            {
                IsEquipped = true;
                ItemData = itemInstance;
            }
            else
            {
                return ChangeItemInSlot(itemInstance);
            }
            
            return null;
        }

        private void EquipWeapon(WeaponData weaponData)
        {
            EventBus.Publish(new SendEquipWeaponEvent(weaponData.damage, weaponData.damageType, weaponData.weaponConfig));
        }

        private void EquipArmour(ArmourData armourData)
        {
            EventBus.Publish(new SendEquipArmourEvent(armourData.physicArmour, armourData.magicArmour));
        }
        
        public ItemInstance UnEquipItem(ItemInstance itemInstance)
        {
            if (ItemData == null || itemInstance == null) return null;
            
            var currentItem = ItemData;
            
            if (ItemData?.itemID == itemInstance.itemID)
            {
                IsEquipped = false;
                ItemData = null;
            }
            
            return currentItem;
            }

        public ItemInstance ChangeItemInSlot(ItemInstance itemData)
        {
            var returnItem = ItemData;
            
            ItemData = itemData;
            
            return returnItem;
        }
    }
}