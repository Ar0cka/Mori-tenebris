using System;
using DefaultNamespace.Enums;
using DefaultNamespace.PlayerStatsOperation.StatSystem;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.TestScripts
{
    public class TestingTakeDamageInPlayer : MonoBehaviour
    {
        [SerializeField] private GameObject objectPlayer;
        [SerializeField] private float cooldownTakeDamage = 2;
        [SerializeField] private int damage = 1;
        private ITakeDamage _takeDamage;

        [Inject]
        public void Constructor(ITakeDamage takeDamage)
        {
            _takeDamage = takeDamage;
        }

        private void Update()
        {
            if (cooldownTakeDamage <= 0)
            {
                Debug.Log($"Current hit point before damage = {_takeDamage.CurrentHitPoint}");
                _takeDamage.TakeDamage(damage, DamageType.Physic);
                cooldownTakeDamage = 2;
                Debug.Log($"current hit point after damage = {_takeDamage.CurrentHitPoint}");
            }
            else
            {
                cooldownTakeDamage -= Time.deltaTime;
            }
        }

        private void OnEnable()
        {
            EventBus.Subscribe<SendDieEvent>(e => PlayerActivatedDie());
        }

        private void PlayerActivatedDie()
        {
            Destroy(objectPlayer);
        }
    }
}