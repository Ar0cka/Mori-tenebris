using System;
using EventBusNamespace;
using PlayerNameSpace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.PlayerHpBar
{
    public class PlayerHealthBar : MonoBehaviour
    {
        [SerializeField] private Image healthBar;
        [SerializeField] private TextMeshProUGUI healthText;
        private Action<SendUpdateHealthEvent> _healthEvent; 

        public void Init()
        {
            _healthEvent = e => UpdateHpBar(e.CurrentHitPoint, e.MaxHitPoint);
            EventBus.Subscribe(_healthEvent);
        }

        private void UpdateHpBar(int currentHitPoint, int maxHitPoint)
        {
            healthText.text = $"{currentHitPoint}/{maxHitPoint}";
            healthBar.fillAmount = (float)currentHitPoint / maxHitPoint;
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe(_healthEvent);
        }
    }

}


