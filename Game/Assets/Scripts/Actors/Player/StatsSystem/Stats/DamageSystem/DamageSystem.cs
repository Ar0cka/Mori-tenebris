using System;
using System.Collections.Generic;
using DefaultNamespace.Enums;
using EventBusNamespace;
using Player.Inventory;
using UnityEngine;
using Zenject;

namespace PlayerNameSpace
{
    public class DamageSystem: IDisposable, IDamageSystem
    {
        private IGetPlayerStat _getPlayerStat;
        
        public int Damage { get; private set; }
        public DamageType DamageType { get; private set; }

        private PlayerStaticData PlayerStaticData => _getPlayerStat.GetPlayerDataStaticStats();
        private PlayerDataStats PlayerData => _getPlayerStat.GetPlayerDataStats();

        public void Initialize(IGetPlayerStat getPlayerStat)
        {
            _getPlayerStat = getPlayerStat;
            
            EventBus.Subscribe<SendUpdateStatEvent>(e => CalculateDamage());
            EventBus.Subscribe<SendEquipWeaponEvent>(e => CalculateDamageWithWeapon(e.WeaponDamage));
            
            CalculateDamage();
        }

        private void CalculateDamage()
        {
            Damage = PlayerStaticData.BaseDamage + Mathf.FloorToInt(PlayerData.Strength * 1.5f); // + inventory.weaponSlot.damage
        }

        private void CalculateDamageWithWeapon(int weaponDamage)
        {
            CalculateDamage();
            Damage += weaponDamage;
        }

        public void Dispose()
        {
            EventBus.Unsubscribe<SendUpdateStatEvent>(e => CalculateDamage());
            EventBus.Unsubscribe<SendEquipWeaponEvent>(e => CalculateDamage());
        }
    }

    public interface IDamageSystem
    {
        public int Damage { get; }
        public DamageType DamageType { get; }
    }
}