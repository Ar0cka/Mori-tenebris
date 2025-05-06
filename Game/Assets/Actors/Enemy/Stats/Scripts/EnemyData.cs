using System;
using Actors.Enemy.EnemyData.Scripts;
using Actors.Enemy.Stats.Events;
using Actors.Enemy.Stats.StatSystems.Armour;
using Actors.Enemy.Stats.StatSystems.DamageSystem;
using DefaultNamespace.Enums;
using EventBusNamespace;
using Systems.CalculateDamageSystem;
using UnityEngine;

namespace Actors.Enemy.Stats
{
    public class EnemyData : MonoBehaviour
    {
        [SerializeField] private EnemyScrObj enemyScrObj;

        private int _currentHitPoints;
        private EnemyArmour _enemyArmour;
        private EnemyDamage _enemyDamage;

        private void Start()
        {
            _currentHitPoints = enemyScrObj.HitPoints;
            _enemyArmour = new EnemyArmour(enemyScrObj);
            _enemyDamage = new EnemyDamage(enemyScrObj.Damage);
        }

        #region Health

        public void TakeDamage(int damage, DamageType damageType)
        {
            int finalDamage = CalculateDamage.CalculateFinalDamageWithResist(damage, _enemyArmour.GetArmour(damageType));
            
            _currentHitPoints -= finalDamage;
            
            EventBus.Publish(new HitEnemyEvent());
            
            CheckDie();
        }
        
        private void CheckDie()
        {
            if (_currentHitPoints <= 0)
            {
                EventBus.Publish(new SendDieEventEnemy());
            }
        }

        #endregion
        
        #region Getters

        public EnemyDamage GetEnemyDamage() => _enemyDamage;

        #endregion
    }
}