using System;
using Actors.Enemy.Stats.Scripts;
using PlayerNameSpace;
using UnityEngine;
using Zenject;

namespace Player.PlayerAttack
{
    public class CheckHit : MonoBehaviour
    {
        [Inject] private DamageSystem damageSystem;
        
        
        private bool _isHit;

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Enemy") && !_isHit)
            {
                _isHit = true;
                
                EnemyData enemyData = TakEnemyData(other.gameObject);

                if (enemyData != null)
                {
                    Debug.Log("Hit enemy " + damageSystem.Damage);
                    enemyData.TakeDamage(damageSystem.Damage, damageSystem.DamageType);
                }

            }
        }

        private EnemyData TakEnemyData(GameObject enemy)
        {
            if (enemy != null)
            {
                return enemy.GetComponent<EnemyData>();
            }

            return null;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                _isHit = false;
            }
        }
    }
}