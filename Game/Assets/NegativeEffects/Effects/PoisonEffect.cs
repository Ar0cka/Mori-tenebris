using System;
using System.Collections.Generic;
using DefaultNamespace.Enums;
using PlayerNameSpace;
using UnityEngine;
using UnityEngine.Serialization;

namespace NegativeEffects
{
    [Serializable]
    public class PoisonEffect : AbstractActionEffect
    {
        [SerializeField] private int tickInterval;
        [SerializeField] private float damageInSeconds;
        [SerializeField] private float duration;
        [SerializeField] private DamageType damageType;
        
        private const float PercentageOfLife = 0.02f;

        public float Duration => CurrentDuration;

        private float _damageCount;

        private float _tick;
        
        private IHitPlayer _damage;
        private Health _health;

        public override bool ApplyEffect<TStatProvider>(TStatProvider statProvider)
        {
            if (statProvider is IPoisonStats poisonStats)
            {
                _health = poisonStats.Health;
                _damage = poisonStats.Damage;
                
                if (_health == null || _damage == null) return false;
                
                CurrentDuration = duration;
                IsActive = true;
                return true;
            }

            return false;
        }

        public override void UpdateEffect()
        {
            CurrentDuration = duration;
        }

        public override void Tick(float dt)
        {
            _tick += dt;
            CurrentDuration -= dt;

            if (_tick >= tickInterval && IsActive)
            {
                _damage.TakeDamage(CalculateDamage(_health.CurrentHitPoint), damageType);
                _tick = 0;
            }
        }

        public override void RemoveEffect(EffectData effectData)
        {
            IsActive = false;
            CurrentDuration = 0;
            effectData.ResetStack();
        }
        private int CalculateDamage(int currentHealth = 0)
        {
            return Mathf.FloorToInt((currentHealth * PercentageOfLife) + damageInSeconds);
        }
    }
    
}