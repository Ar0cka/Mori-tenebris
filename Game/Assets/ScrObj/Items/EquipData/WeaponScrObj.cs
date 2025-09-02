using System;
using Actors.Player.AttackSystem.Data;
using DefaultNamespace.Enums;
using Enemy;
using Player.Inventory;
using PlayerNameSpace.Inventory;
using UnityEngine;

namespace Items.EquipArmour.Data
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "EquipItem/Weapon")]
    public class WeaponScrObj : ItemScrObj
    {
        [SerializeField] private WeaponData weaponData;
        public WeaponData WeaponData => weaponData;

        public override ItemData GetItemData()
        {
            return (ItemData)weaponData.Clone();
        }
    }

    [Serializable]
    public class WeaponData : EquipData
    {
        public WeaponAttackSettings weaponConfig;
        public int damage;
        public DamageType damageType;
    }
}