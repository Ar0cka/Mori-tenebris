using Actors.Enemy.Data.Scripts;
using HitChecker;
using NegativeEffects;
using PlayerNameSpace;
using UnityEngine;

namespace Actors.Enemy.AttackStateMachine.State
{
    public class HitState : IAttackState
    {
        private AttackConfig _attackConfig;
        private EffectScrObj _effect;
        private Transform _hitPos;
        public float StateDelay { get; private set; }
        private float _radius;

        private float _currentDt;
        
        public HitState(AttackConfig attackConfig, EffectScrObj effectScrObj, Transform hitPos, float stateDelay, float radius)
        {
            _attackConfig = attackConfig;
            _hitPos = hitPos;
            stateDelay = stateDelay;
            _radius = radius;
            _effect = effectScrObj;
        }
        
        public bool Apply()
        {
            if (!ValidateComponents()) return false;
            _currentDt = StateDelay;
            return true;
        }
        
        public bool Action()
        {
            GameObject hitPrefab = HitSystem.CircleHit(_hitPos.position, _radius, "Player");
            
            if (hitPrefab == null) return false;
            
            ITakeDamage takeDamage = hitPrefab.GetComponentInChildren<ITakeDamage>();
            
            if (takeDamage == null) return false;
            
            takeDamage.TakeHit(_attackConfig.damage, _attackConfig.damageType);
            
            if (_effect == null) return true;
            
            takeDamage.AddEffect(_effect);
            
            return true;
        }
        
        public bool EndAction(float dt)
        {
            _currentDt -= dt;
            
            if (_currentDt <= 0) return true;
            
            return false;
        }

        private bool ValidateComponents()
        {
            if (_attackConfig == null || _hitPos == null)
                return false;
            
            return true;
        }
    }
}