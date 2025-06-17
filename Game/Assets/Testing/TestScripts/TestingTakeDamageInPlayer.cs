using DefaultNamespace.Enums;
using DefaultNamespace.PlayerStatsOperation.StatSystem;
using EventBusNamespace;
using PlayerNameSpace;
using UnityEngine;
using Zenject;

namespace TestingNameSpace
{
    public class TestingTakeDamageInPlayer : MonoBehaviour
    {
        [SerializeField] private GameObject objectPlayer;
        [SerializeField] private float cooldownTakeDamage = 2;
        [SerializeField] private int damage = 1;
        private IHitPlayer _hitPlayer;

        [Inject]
        public void Constructor(IHitPlayer hitPlayer)
        {
            _hitPlayer = hitPlayer;
        }

        private void Update()
        {
            if (cooldownTakeDamage <= 0)
            {
                Debug.Log($"Current hit point before damage = {_hitPlayer.CurrentHitPoint}");
                _hitPlayer.TakeDamage(damage, DamageType.Physic);
                cooldownTakeDamage = 2;
                Debug.Log($"current hit point after damage = {_hitPlayer.CurrentHitPoint}");
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