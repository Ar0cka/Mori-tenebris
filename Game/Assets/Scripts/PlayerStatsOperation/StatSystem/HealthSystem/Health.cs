using System;
using DefaultNamespace.Enums;
using DefaultNamespace.PlayerStatsOperation.IPlayerData;
using DefaultNamespace.PlayerStatsOperation.StatSystem.ArmourSystem;
using DefaultNamespace.PlayerStatsOperation.StatUpgrade;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.PlayerStatsOperation.StatSystem
{
    public class Health : ITakeDamage, IRegeneration, IDisposable
    {
        private readonly IGetPlayerStat _getPlayerStat;
        private readonly IUpgradeStat _upgradeStat;
        private readonly Armour _armour;
        
        private int _maxHitPoint;
        public int CurrentHitPoint { get; private set; }

        public event Action dieEvent;

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
            float multiplyDamage = 0;

            switch (damageType)
            {
                case DamageType.Physic:
                    multiplyDamage = damage / (damage + _armour.PhysicArmour);
                    break;
                case DamageType.Magic:
                    multiplyDamage = damage / (damage + _armour.MagicArmour);
                    break;
                default:
                    return;
            }

            int finallyDamage = (int)(damage * multiplyDamage);


            CurrentHitPoint -= Mathf.Clamp(finallyDamage, 0, CurrentHitPoint);


            if (CurrentHitPoint <= 0)
            {
                PlayerDead();
            }
        }

        public void Regeneration(int countRegeneration)
        {
            if (CurrentHitPoint < _maxHitPoint)
            {
                CurrentHitPoint += Mathf.Clamp(countRegeneration, 0, _maxHitPoint);
            }
        }

        private void PlayerDead()
        {
            EventBus.Publish(new SendDieEvent());
        }

        private void UpdateStats()
        {
            PlayerDataStats loadData = _getPlayerStat.GetPlayerDataStats();
            
            _maxHitPoint = loadData.startMaxHitPoint + loadData.strength * 5;
            CurrentHitPoint = _maxHitPoint;

            Debug.Log($"Current hit point = {CurrentHitPoint}");
        }

        public void Dispose()
        {
            EventBus.Unsubscribe<SendDieEvent>(e => UpdateStats());
        }
    }
}

public class SendDieEvent {}