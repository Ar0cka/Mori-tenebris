using System;
using DefaultNamespace.Enums;
using EventBusNamespace;
using DefaultNamespace.PlayerStatsOperation.StatSystem.ArmourSystem;
using Systems.CalculateDamageSystem;
using UnityEngine;
using Zenject;

namespace PlayerNameSpace
{
    public class Health : IHitPlayer, IRegenerationHealth, IDisposable
    {
        private readonly IGetPlayerStat _getPlayerStat;
        private readonly IUpgradeStat _upgradeStat;
        private readonly Armour _armour;

        public int MaxHitPoint { get; private set; }
        public int CurrentHitPoint { get; private set; }
        
        public bool IsDead { get; private set; }

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
            EventBus.Publish(new SendUpdateHealthEvent(CurrentHitPoint, MaxHitPoint));
            
            CurrentHitPoint = Math.Clamp(CurrentHitPoint, 0, MaxHitPoint);
        }

        public void TakeDamage(int damage, DamageType damageType)
        {
            if (IsDead) return;
            
            int finalDamage = CalculateDamage.CalculateFinalDamageWithResist(damage, _armour.GetArmour(damageType));
            
            CurrentHitPoint -= Mathf.Min(finalDamage, CurrentHitPoint);
            
            EventBus.Publish(new SendUpdateHealthEvent(CurrentHitPoint, MaxHitPoint));
            
            CheckDead();
        }

        public void Regeneration(int countRegeneration)
        {
            if (CurrentHitPoint < MaxHitPoint && !IsDead)
            {
                CurrentHitPoint += Mathf.Clamp(countRegeneration, 0, MaxHitPoint);
                EventBus.Publish(new SendUpdateHealthEvent(CurrentHitPoint, MaxHitPoint));
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
            IsDead = true;
            EventBus.Publish(new SendDieEvent());
        }

        private void UpdateStats()
        {
            PlayerDataStats loadData = _getPlayerStat.GetPlayerDataStats();
            
            MaxHitPoint = _getPlayerStat.GetPlayerDataStaticStats().StartMaxHitPoint + loadData.Vitality * 5;
            CurrentHitPoint = MaxHitPoint;

            Debug.Log($"Current hit point = {CurrentHitPoint}");
        }

        public int GetMaxHealth()
        {
            return MaxHitPoint;
        }
        
        public void Dispose()
        {
            EventBus.Unsubscribe<SendUpdateStatEvent>(e => UpdateStats());
        }
        
    }
}

public class SendDieEvent {}

public class SendUpdateHealthEvent
{
    public int CurrentHitPoint { get; private set; }
    public int MaxHitPoint { get; private set; }

    public SendUpdateHealthEvent(int currentHitPoint, int maxHitPoint)
    {
        CurrentHitPoint = currentHitPoint;
        MaxHitPoint = maxHitPoint;
    }
}