using System;
using UnityEngine;

namespace NegativeEffects
{
    [Serializable]
    public class EffectData
    {
        public int addStack;
        public StatusEffectType Status => IsReady ? StatusEffectType.Active : StatusEffectType.Pending;
        public EffectType effectType;
        public int threshold;
        public int maxStackEffect;
        public float LastTimeStack { get; private set; }
        public float CurrentStack { get; private set; }

        public void AddStack(int stack)
        {
            CurrentStack += stack;
            LastTimeStack = Time.time;
        }

        public void ResetStack()
        {
            CurrentStack = 0;
        }
        
        public bool IsReady => CurrentStack >= maxStackEffect;
        public bool СanDump => Time.time - LastTimeStack >= threshold;
    }
    
    public enum EffectType
    {
        Burn,
        Poison,
        Decay,
    }

    public enum StatusEffectType
    {
        Active,
        Pending
    }
}