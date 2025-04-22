using System;
using DefaultNamespace.Enums;
using DefaultNamespace.Events;
using DefaultNamespace.PlayerStatsOperation.IPlayerData;
using DefaultNamespace.PlayerStatsOperation.StatSystem.ArmourSystem;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.PlayerStatsOperation.StatSystem
{
    public class Health : ITakeDamage, IRegeneration, IGetSubscribeInDieEvent
    {
        private readonly IGetPlayerStat _getPlayerStat;
        private readonly Armour _armour;
        private readonly string _statName;
        
        private int _maxHitPoint;
        public int CurrentHitPoint { get; private set; }

        public event Action dieEvent;

        [Inject]
        public Health(IGetPlayerStat getPlayerStat, Armour armour, string statName)
        {
            _armour = armour;
            _getPlayerStat = getPlayerStat;
            _statName = statName;
        }

        public void Initialize()
        {
            Debug.Log($"{_getPlayerStat} {_armour} {_statName}");
            
            _maxHitPoint = (int)_getPlayerStat.TryGetStat(_statName).currentCountStats;
            CurrentHitPoint = _maxHitPoint;
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
            dieEvent?.Invoke();
        }
        
        public void SubscribeInDieEvent(Action action) => dieEvent += action;
        public void UnsubscribeFromDieEvent(Action action) => dieEvent -= action;
    }
}