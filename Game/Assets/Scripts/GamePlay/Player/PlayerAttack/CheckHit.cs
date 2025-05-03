using System;
using EventBusNamespace;
using PlayerNameSpace;
using UnityEngine;
using Zenject;

namespace Player.PlayerAttack
{
    public class CheckHit : MonoBehaviour
    {
        [Inject] private DamageSystem damageSystem;
        
        
        private bool _isHit;
        public void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.CompareTag("Enemy") && !_isHit)
            {
                _isHit = true;
                EventBus.Publish(new SendHitEnemyEvent(damageSystem.Damage));
            }
        }

        public void OnCollisionExit2D(Collision2D other)
        {
            if (other.collider.CompareTag("Enemy"))
            {
                _isHit = false;
            }
        }
    }
}