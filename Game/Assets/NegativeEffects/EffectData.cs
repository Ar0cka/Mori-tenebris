using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace NegativeEffects
{
    [Serializable]
    public class EffectData
    {
        [field:SerializeField] public int AddStackCount { get; private set; }
        [field:SerializeField] public EffectType EffectType { get; private set; }
        [SerializeField] private int thresholdStack;
        [SerializeField] private int maxStackEffect;
        [SerializeField] private int dumpStackCount;
        public float LastTimeStack { get; private set; }
        public float CurrentStack { get; private set; }

        public void AddStack(int stack)
        {
            CurrentStack += stack;
            LastTimeStack = Time.time;
        }

        public void RemoveStack()
        {
            CurrentStack -= dumpStackCount;
            LastTimeStack = Time.time;
        }
        
        public void ResetStack()
        {
            CurrentStack = 0;
        }
        
        public bool IsReady => CurrentStack >= maxStackEffect;
        public bool СanDump => Time.time - LastTimeStack >= thresholdStack;
    }
    
    public enum EffectType
    {
        Burn,
        Poison,
        Decay,
    }
}