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
        
        private ITakeDamage takeDamage;

        public override bool ApplyEffect<TStatProvider>(TStatProvider statProvider)
        {
            if (statProvider is IPoisonStats poisonStats)
            {
                takeDamage = poisonStats.Damage;

                if (takeDamage == null) return false;
                
                CurrentDuration = duration;
                IsActive = true;
                return true;
            }

            return false;
        }

        public override void UpdateEffect()
        {
            CurrentDuration = duration;
            Debug.Log("Update Effect = " + CurrentDuration);
        }

        public override void Tick(float dt)
        {
            _tick += dt;
            CurrentDuration -= dt;

            if (_tick >= tickInterval && IsActive)
            {
                takeDamage.TakeHit(CalculateDamage(takeDamage.GetCurrentHitPoint()), damageType);
                _tick = 0;
            }
            
            Debug.Log("Tick. Current duration = " + CurrentDuration);
        }

        public override void RemoveEffect(EffectData effectData)
        {
            IsActive = false;
            CurrentDuration = 0;
            effectData.ResetStack();
        }
        private int CalculateDamage(int currentHealth = 0)
        {
            var damage = Mathf.FloorToInt((currentHealth * PercentageOfLife) + damageInSeconds);

            if (damage == 0) return 1;
            
            return damage;
        }
    }
    
}