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
        public EquipItemType TypeItem { get; private set;}

        private ItemInstance _itemInstance;
        
        public EquipSlot(EquipItemType typeItem)
        {
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
            
            if (_itemInstance == null)
            {
                if (itemInstance.itemData is IEquipable equipable)
                {
                    _itemInstance = itemInstance;
                    equipable.ChangeEquipStatus(true);
                }
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
            if (_itemInstance == null || itemInstance == null) return null;
            
            var currentItem = _itemInstance;
            
            if (_itemInstance?.itemID == itemInstance.itemID)
            {
                if (itemInstance.itemData is IEquipable equipable)
                {
                    equipable.ChangeEquipStatus(false);
                    _itemInstance = null;
                }
            }
            
            return currentItem;
            }

        public ItemInstance ChangeItemInSlot(ItemInstance itemData)
        {
            var returnItem = _itemInstance;
            
            _itemInstance = itemData;
            
            return returnItem;
        }
    }
}