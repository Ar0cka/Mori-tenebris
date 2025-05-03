using System;
using System.Collections.Generic;
using EventBusNamespace;
using Player.Inventory;
using UnityEngine;
using Zenject;

namespace PlayerNameSpace
{
    public class DamageSystem: IDisposable
    {
        [Inject] private IGetPlayerStat _getPlayerStat;
        
        public int Damage { get; private set; }

        private PlayerStaticData PlayerStaticData => _getPlayerStat.GetPlayerDataStaticStats();
        private PlayerDataStats PlayerData => _getPlayerStat.GetPlayerDataStats();

        public void Initialize()
        {
            EventBus.Subscribe<SendUpdateStatEvent>(e => CalculateDamage());
            EventBus.Subscribe<SendEquipWeaponEvent>(e => CalculateDamage());
            
            CalculateDamage();
        }
        
        private void CalculateDamage()
        {
            Damage = PlayerStaticData.BaseDamage + Mathf.FloorToInt(PlayerData.Strength * 1.5f); // + inventory.weaponSlot.damage
        }

        public void Dispose()
        {
            EventBus.Unsubscribe<SendUpdateStatEvent>(e => CalculateDamage());
            EventBus.Unsubscribe<SendEquipWeaponEvent>(e => CalculateDamage());
        }
    }
}