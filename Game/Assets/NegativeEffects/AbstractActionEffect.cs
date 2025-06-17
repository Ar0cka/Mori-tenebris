using System.Collections.Generic;
using PlayerNameSpace;
using UnityEngine;

namespace NegativeEffects
{
    public abstract class AbstractActionEffect
    {
        protected float CurrentDuration;

        protected bool IsActive;
        
        public bool IsEffecting => CurrentDuration > 0;
        public abstract bool ApplyEffect<T>(T stat);
        public abstract void UpdateEffect();
        public abstract void Tick(float dt);
        public abstract void RemoveEffect(EffectData effectData);
    }

    public enum EffectingStats
    {
        Health,
        Stamina,
        Damage,
    }
}