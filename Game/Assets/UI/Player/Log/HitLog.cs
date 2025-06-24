using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using EventBus = EventBusNamespace.EventBus;

namespace UI.Player.Log
{
    public class HitLog : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI hitLogText;
        [SerializeField] private GameObject logPrefab;
        [SerializeField] private Transform logPosition;

        [SerializeField] private float lifeTime;
        
        private Action<SendUpdateHealthEvent> _healthCallback;

        public void Initialize()
        {
            _healthCallback = e => ShowHitLog(e.Damage);
            transform.position = logPosition.position;
            EventBus.Subscribe(_healthCallback);
        }

        private void ShowHitLog(int finallyDamage)
        {
            if (!ValidateComponents() || finallyDamage == 0) return;
            
            if (!logPrefab.activeInHierarchy) 
                logPrefab.SetActive(true);
            
            hitLogText.text = finallyDamage.ToString();
            
            StartAnim();
        }

        private void StartAnim()
        {
            Vector2 up = transform.position + Vector3.up;
            
            transform.DOMove(up, lifeTime);
            
            StartCoroutine(EndAnim());
        }

        private IEnumerator EndAnim()
        {
            yield return new WaitForSeconds(lifeTime);
            
            logPrefab.SetActive(false);
            transform.position = logPosition.position;
        }
        
        private bool ValidateComponents()
        {
            return logPrefab != null && logPosition != null;
        }
    }
}