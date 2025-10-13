using System;
using EventBusNamespace;
using UI.EffectUI;
using UnityEngine;
using UnityEngine.Serialization;

namespace NegativeEffects
{
    [Serializable]
    public class EffectData
    {
        [SerializeField] private int addStackCount;
        [field:SerializeField] public EffectType EffectType { get; private set; }
        [SerializeField] private int thresholdStack;
        [field:SerializeField] public int MaxStackEffect { get; private set; }
        [SerializeField] private int dumpStackCount;
        public float LastTimeStack { get; private set; }
        public float CurrentStack { get; private set; }

        public EffectData(int addStackCount, EffectType effectType, int thresholdStack, int dumpStackCount,
            int maxStackEffect)
        {
            this.addStackCount = addStackCount;
            EffectType = effectType;
            this.thresholdStack = thresholdStack;
            this.dumpStackCount = dumpStackCount;
            MaxStackEffect = maxStackEffect;
        }
        
        public EffectData CloneData() => MemberwiseClone() as EffectData;
        
        public void AddStack()
        {
            CurrentStack += addStackCount;
            LastTimeStack = Time.time;
            
            Debug.Log($"Current stack after add chip: {CurrentStack}");
            
            EventBus.Publish(new SendUpdateEffectEvent(this));
        }

        public void RemoveStack()
        {
            CurrentStack -= dumpStackCount;
            LastTimeStack = Time.time;
            
            Debug.Log($"Stack after dump = {CurrentStack}");
            
            EventBus.Publish(new SendUpdateEffectEvent(this));
            
            if (CurrentStack <= 0)
                EventBus.Publish(new SendDisableEffectEvent(EffectType));
        }
        
        public void ResetStack()
        {
            CurrentStack = 0;
            
            EventBus.Publish(new SendUpdateEffectEvent(this));
            EventBus.Publish(new SendDisableEffectEvent(EffectType));
        }
        
        public bool IsReady => CurrentStack >= MaxStackEffect;
        public bool СanDump => Time.time - LastTimeStack >= thresholdStack;
    }
    
    public enum EffectType
    {
        Burn,
        Poison,
        Decay,
    }
}