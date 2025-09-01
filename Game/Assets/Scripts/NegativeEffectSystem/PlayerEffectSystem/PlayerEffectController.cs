using System;
using System.Collections.Generic;
using EventBusNamespace;
using PlayerNameSpace;
using UI.EffectUI;
using UnityEngine;
using UnityEngine.Serialization;

namespace NegativeEffects
{
    public class PlayerEffectController : MonoBehaviour
    {
        [SerializeField] private PlayerGeterStats getStats;
        
        private Dictionary<EffectType, IEffectData> _waitedEffects = new();
        private Dictionary<EffectType, IEffectData> _activeEffects = new();

        private void Awake()
        {
            _waitedEffects.Clear();
            _activeEffects.Clear();
        }

        private void FixedUpdate()
        {
            CheckWaitedEffects();
            
            TickSystem();
            
            RemoveEffect();
            
            DumpStack();
        }

        private void CheckWaitedEffects()
        {
            if (_waitedEffects.Count > 0)
            {
                List<EffectType> removeEffect = new List<EffectType>();
                
                foreach (var effect in _waitedEffects)
                {
                    var effectDataStatus = effect.Value.EffectData();

                    if (!effectDataStatus.IsReady) continue;

                    _activeEffects.Add(effect.Key, effect.Value);
                    effect.Value.ActionEffect().ApplyEffect(getStats.GetStat<IEffectStat>(effect.Key));
                    removeEffect.Add(effect.Key);
                }

                foreach (var remove in removeEffect)
                {
                    _waitedEffects.Remove(remove);
                }
            }
        }

        private void DumpStack()
        {
            List<EffectType> removeEffect = new List<EffectType>();
            
            foreach (var effect in _waitedEffects)
            {
                var effectData = effect.Value.EffectData();

                if (effectData.СanDump)
                {
                    effectData.RemoveStack();
                }
                
                if (effectData.CurrentStack <= 0)
                    removeEffect.Add(effect.Key);
            }

            foreach (var remove in removeEffect)
            {
                _waitedEffects.Remove(remove);
            }
        }

        private void TickSystem()
        {
            foreach (var effect in _activeEffects)
            {
                var action = effect.Value.ActionEffect();
                
                if (!action.IsEffecting) continue;
                
                action.Tick(Time.fixedDeltaTime);
            }
        }
        
        public void AddEffect(EffectScrObj effect)
        {
            var effectData = effect.EffectData();

            if (_activeEffects.ContainsKey(effectData.EffectType))
            {
                _activeEffects[effectData.EffectType].ActionEffect().UpdateEffect();
                return;
            }
            
            if (_waitedEffects.ContainsKey(effectData.EffectType))
            {
                _waitedEffects[effectData.EffectType].EffectData().AddStack();
                return;
            }

            _waitedEffects.Add(effectData.EffectType, new EffectClone(effect));
            _waitedEffects[effectData.EffectType].EffectData().AddStack();
        }

        private void RemoveEffect()
        {
            if (_activeEffects.Count == 0) return;
            
            List<EffectType> removeEffects = new List<EffectType>();
            
            foreach (var effects in _activeEffects)
            {
                var action = effects.Value.ActionEffect();

                if (action.IsEffecting) continue;
                
                action.RemoveEffect(effects.Value.EffectData());
                removeEffects.Add(effects.Key);
            }

            foreach (var remove in removeEffects)
            {
                _activeEffects.Remove(remove);
            }
        }

        public void DiSpellAllEffects()
        {
            foreach (var effect in _activeEffects.Values)
            {
                effect.ActionEffect().RemoveEffect(effect.EffectData());
            }
            
            _activeEffects.Clear();
        }
    }

    public class EffectClone : IEffectData
    {
        private readonly EffectData _effectData;
        private readonly AbstractActionEffect _effectAction;
        
        public EffectClone(EffectScrObj effect)
        {
            _effectData = effect.EffectData().CloneData();
            _effectAction = effect.ActionEffect();
        }

        public EffectData EffectData() => _effectData;
        public AbstractActionEffect ActionEffect() => _effectAction;
    }
    
    public interface IEffectData
    {
        EffectData EffectData();
        AbstractActionEffect ActionEffect();
    }
}