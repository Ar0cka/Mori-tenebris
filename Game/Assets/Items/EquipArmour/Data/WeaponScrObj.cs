using System;
using DefaultNamespace.Enums;
using Enemy;
using PlayerNameSpace.Inventory;
using UnityEngine;

namespace Items.EquipArmour.Data
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "EquipItem/Weapon")]
    public class WeaponScrObj : ItemScrObj
    {
        [SerializeField] private WeaponData weaponData;
        public WeaponData WeaponData => weaponData;

        public override void OnEquip(InventoryLogic inventoryLogic)
        {
            inventoryLogic.SelectEquipAction(weaponData.equipItemType, weaponData);
        }

        public override void OnUnequip(InventoryLogic inventoryLogic)
        {
            inventoryLogic.UnEquipItem(weaponData.equipItemType, weaponData);
        }

        public override ItemData GetItemData()
        {
            return weaponData;
        }
    }

    [Serializable]
    public class WeaponData : EquipData
    {
        public int damage;
        public DamageType damageType;
    }
}