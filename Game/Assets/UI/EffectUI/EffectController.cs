using System;
using NegativeEffects;
using UnityEngine;
using Image = UnityEngine.UI.Image;

namespace UI.EffectUI
{
    public class EffectController : MonoBehaviour
    {
        private Action<EffectType> _updatePoisonUI;
        
        #region PoisonUI

        [SerializeField] private GameObject poisonPrefab;
        [SerializeField] private Image poisonBar;
        
        
        #endregion
        
    }

    public class SendUpdateEffectEvent
    {
        public EffectType EffectType { get; private set; }
        public int MaxCount { get; private set; }
        public int CurrentCount { get; private set; }
        
        public SendUpdateEffectEvent(EffectType effectType, int maxCount, int currentCount)
        {
            EffectType = effectType;
            MaxCount = maxCount;
            CurrentCount = currentCount;
        }
    }
}