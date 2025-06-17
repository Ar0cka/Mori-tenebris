using System;
using System.Collections.Generic;
using PlayerNameSpace;
using UnityEngine;
using UnityEngine.Serialization;

namespace NegativeEffects
{
    public class PlayerEffectController : MonoBehaviour
    {
        [SerializeField] private PlayerGeterStats getStats;
        
        private Dictionary<EffectType, IEffectData> _waitedEffects = new();
        private Dictionary<EffectType, IEffectData> _activeEffects = new();
        
        private void Update()
        {
            CheckWaitedEffects();
            
            TickSystem();
            
            RemoveEffect();
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

        private void TickSystem()
        {
            foreach (var effect in _activeEffects)
            {
                var action = effect.Value.ActionEffect();
                
                if (!action.IsEffecting) continue;
                
                action.Tick(Time.deltaTime);
            }
        }
        
        public void AddEffect(EffectScrObj effect)
        {
            var effectData = effect.EffectData();

            if (_activeEffects.ContainsKey(effectData.effectType))
            {
                _activeEffects[effectData.effectType].ActionEffect().UpdateEffect();
                return;
            }
            
            if (_waitedEffects.ContainsKey(effectData.effectType))
            {
                _waitedEffects[effectData.effectType].EffectData().AddStack(effectData.addStack);
                return;
            }

            _waitedEffects.Add(effectData.effectType, effect);
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
                _activeEffects.Remove(effect.EffectData().effectType);
            }
            
            if (_activeEffects.Count > 0) _activeEffects.Clear();
        }
    }
   
}