using System;
using NegativeEffects;
using UnityEngine;

namespace UI.EffectUI
{
    public class EffectUIScript : MonoBehaviour
    {
        [field: SerializeField] public EffectUIData UiData { get; private set; }

        private void Awake()
        {
            UiData.effectPrefab.SetActive(false);
        }

        public void SetActive(bool active)
        {
            UiData.effectPrefab.SetActive(active);
        }

        public void UpdateEffect(EffectData data)
        {
            if (data == null || !IsValid(UiData)) return;
            
            UiData.effectPrefab.SetActive(true);
            
            UiData.effectBar.fillAmount = data.CurrentStack / data.MaxStackEffect;
        }
        
        private bool IsValid(EffectUIData data) =>
            data.effectPrefab != null && data.effectBar != null;
    }
}