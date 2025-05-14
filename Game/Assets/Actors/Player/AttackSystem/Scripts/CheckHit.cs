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
        public void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.CompareTag("Enemy") && !_isHit)
            {
                _isHit = true;

                EnemyData enemyData = TakEnemyData(other.gameObject);

                if (enemyData != null)
                {
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
        
        public void OnCollisionExit2D(Collision2D other)
        {
            if (other.collider.CompareTag("Enemy"))
            {
                _isHit = false;
            }
        }
    }
}