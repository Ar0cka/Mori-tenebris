using System;
using DefaultNamespace.Enums;
using EventBusNamespace;
using DefaultNamespace.PlayerStatsOperation.StatSystem.ArmourSystem;
using Systems.CalculateDamageSystem;
using UnityEngine;
using Zenject;

namespace PlayerNameSpace
{
    public class Health : ITakeDamage, IRegenerationHealth, IDisposable
    {
        private readonly IGetPlayerStat _getPlayerStat;
        private readonly IUpgradeStat _upgradeStat;
        private readonly Armour _armour;
        
        private int _maxHitPoint;
        public int CurrentHitPoint { get; private set; }

        [Inject]
        public Health(IGetPlayerStat getPlayerStat, Armour armour, IUpgradeStat upgradeStat)
        {
            _armour = armour;
            _getPlayerStat = getPlayerStat;
        }

        public void Initialize()
        {
            EventBus.Subscribe<SendUpdateStatEvent>(e => UpdateStats());
            UpdateStats();
        }

        public void TakeDamage(int damage, DamageType damageType)
        {
            int finalDamage = CalculateDamage.CalculateFinalDamageWithResist(damage, _armour.GetArmour(damageType));
            
            CurrentHitPoint -= finalDamage;
            
            Debug.Log($"Current hit point = {CurrentHitPoint}");
            
            CheckDead();
        }

        public void Regeneration(int countRegeneration)
        {
            if (CurrentHitPoint < _maxHitPoint)
            {
                CurrentHitPoint += Mathf.Clamp(countRegeneration, 0, _maxHitPoint);
            }
        }

        private void CheckDead()
        {
            if (CurrentHitPoint <= 0)
            {
                PlayerDead();
            }
        }
        
        private void PlayerDead()
        {
            EventBus.Publish(new SendDieEvent());
        }

        private void UpdateStats()
        {
            PlayerDataStats loadData = _getPlayerStat.GetPlayerDataStats();
            
            _maxHitPoint = _getPlayerStat.GetPlayerDataStaticStats().StartMaxHitPoint + loadData.Vitality * 5;
            CurrentHitPoint = _maxHitPoint;

            Debug.Log($"Current hit point = {CurrentHitPoint}");
        }

        public int GetMaxHealth()
        {
            return _maxHitPoint;
        }
        
        public void Dispose()
        {
            EventBus.Unsubscribe<SendUpdateStatEvent>(e => UpdateStats());
        }
        
    }
}

public class SendDieEvent {}