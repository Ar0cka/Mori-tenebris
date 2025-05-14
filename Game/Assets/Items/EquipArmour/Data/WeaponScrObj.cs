using System;
using DefaultNamespace.Enums;
using Enemy;
using UnityEngine;

namespace Items.EquipArmour.Data
{
    public class WeaponScrObj : ScriptableObject
    {
        [SerializeField] private WeaponData weaponData;
        public WeaponData WeaponData => weaponData;
    }

    [Serializable]
    public class WeaponData : ItemData
    {
        public int damage;
        public DamageType damageType;
    }
}