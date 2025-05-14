using System;
using Actors.Enemy.Stats.Scripts;
using Enemy.StatSystems.DamageSystem;
using PlayerNameSpace;
using UnityEngine;
using Zenject;

namespace Actors.Enemy.AttackSystem.Scripts
{
    public class CheckEnemyHit : MonoBehaviour
    {
        [SerializeField] private EnemyData enemyData;
        [SerializeField] private string hitTag;

        private EnemyDamage _enemyDamage;
        private bool _hit;

        private void Awake()
        {
            _enemyDamage = enemyData.GetEnemyDamage();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag(hitTag))
            {
                _hit = true;
                TakeDamage playerTakeDamage = GetPlayerTakeDamage(other.gameObject);
                playerTakeDamage.TakeHit(_enemyDamage.Damage, _enemyDamage.DamageType);
            }
        }

        private TakeDamage GetPlayerTakeDamage(GameObject player)
        {
            if (player != null)
            {
                return player.GetComponent<TakeDamage>();
            }       
            return null;
        }
        
        private void OnCollisionExit2D(Collision2D other)
        {
            if (_hit)
            {
                _hit = false;
            }
        }
    }
}