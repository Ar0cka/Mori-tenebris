using System;
using System.Collections.Generic;
using NegativeEffects;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using EventBus = EventBusNamespace.EventBus;
using Image = UnityEngine.UI.Image;

namespace UI.EffectUI
{
    public class EffectUIController : MonoBehaviour
    {
        [SerializeField] private List<GameObject> uiDataList = new();
        [SerializeField] private Transform spawnParent;

        private Dictionary<EffectType, EffectUIScript> _effectUIDictionary = new();

        private Action<SendUpdateEffectEvent> _updateEffectUI;
        private Action<SendResetEffectEvent> _resetEffectUI;
        private Action<SendDisableEffectEvent> _disableEffectUI;

        public void Init()
        {
            var uiList = GetListWithUI();
            
            foreach (var uiData in uiList)
            {
                var effectData = uiData.UiData;
                
                _effectUIDictionary.TryAdd(effectData.effectType, uiData);
            }

            SubscribeToEvents();
        }

        private List<EffectUIScript> GetListWithUI()
        {
            List<GameObject> list = uiDataList.Count == 0 ? DownloadUI() : uiDataList;
            List<EffectUIScript> returnList = new List<EffectUIScript>();

            foreach (var data in list)
            {
                var obj = Instantiate(data, spawnParent);
                EffectUIScript effect = obj.GetComponent<EffectUIScript>();

                if (effect == null)
                {
                    Destroy(obj);
                    continue;
                }

                obj.SetActive(false);
                returnList.Add(effect);
            }
            
            return returnList;
        }

        private List<GameObject> DownloadUI()
        {
            // TODO: Реализовать Addressables или Resources.Load
            return null;
        }

        private void SubscribeToEvents()
        {
            _updateEffectUI = e => UpdateEffect(e.Effect);
            _resetEffectUI = _ => DisableAllEffectUI();
            _disableEffectUI = e => DisableEffectUI(e.EffectType);

            EventBus.Subscribe(_updateEffectUI);
            EventBus.Subscribe(_resetEffectUI);
            EventBus.Subscribe(_disableEffectUI);
        }

        private void UpdateEffect(EffectData effectData)
        {
            if (_effectUIDictionary.TryGetValue(effectData.EffectType, out var effectUIScript))
            {
                effectUIScript?.UpdateEffect(effectData);
            }
        }

        private void DisableEffectUI(EffectType type)
        {
            if (_effectUIDictionary.TryGetValue(type, out var uiData))
                uiData.SetActive(false);
        }

        private void DisableAllEffectUI()
        {
            foreach (var ui in _effectUIDictionary.Values)
                ui.SetActive(false);
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe(_updateEffectUI);
            EventBus.Unsubscribe(_resetEffectUI);
            EventBus.Unsubscribe(_disableEffectUI);
        }
    }
}

[Serializable]
public class EffectUIData
{
    public EffectType effectType;
    public GameObject effectPrefab;
    public Image effectBar;
}

#region Events

public class SendUpdateEffectEvent
{
    public EffectData Effect { get; private set; }

    public SendUpdateEffectEvent(EffectData effect)
    {
        Effect = effect;
    }
}

public class SendResetEffectEvent
{
}

public class SendDisableEffectEvent
{
    public EffectType EffectType { get; private set; }

    public SendDisableEffectEvent(EffectType effectType)
    {
        EffectType = effectType;
    }
}

#endregion