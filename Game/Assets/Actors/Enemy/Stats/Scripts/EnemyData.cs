using System;
using System.Collections;
using Actors.Enemy.Data.Scripts;
using Actors.Enemy.Monsters.Slime.Data.Scripts;
using DefaultNamespace.Enums;
using Enemy.Events;
using Enemy.StatSystems.Armour;
using Enemy.StatSystems.DamageSystem;
using EventBusNamespace;
using PlayerNameSpace;
using Systems.CalculateDamageSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace Actors.Enemy.Stats.Scripts
{
    public class EnemyData : MonoBehaviour
    {
        [SerializeField] private MonsterScrObj monsterScrObj;
        [SerializeField] private Animator animator;
        [SerializeField] private float delayForDestroy = 2f;

        private int _currentHitPoints;
        private EnemyArmour _enemyArmour;
        private EnemyDamage _enemyDamage;

        private bool _isDie;
        public Transform PlayerPosition { get; private set; }

        public void Initialize()
        { 
            PlayerPosition = GameObject.FindGameObjectWithTag("Player").transform;
            
            _currentHitPoints = monsterScrObj.GetConfig().hitPoints;
            Debug.Log("current hit points = " + _currentHitPoints);
            _enemyArmour = new EnemyArmour(monsterScrObj.GetConfig());
            _enemyDamage = new EnemyDamage();
        }
        
        #region Health

        public void TakeDamage(int damage, DamageType damageType)
        {
            if (_isDie) return;
            
            int finalDamage = CalculateDamage.CalculateFinalDamageWithResist(damage, _enemyArmour.GetArmour(damageType));
            
            _currentHitPoints -= finalDamage;
            
            Debug.Log(_currentHitPoints);
            
            EventBus.Publish(new HitEnemyEvent());
            
            CheckDie();
        }
        
        private void CheckDie()
        {
            if (_currentHitPoints <= 0)
            {
                _isDie = true;
                animator.SetTrigger("Dead");
                EventBus.Publish(new SendDieEventEnemy());
                
                Destroy(gameObject, delayForDestroy);
            }
        }

        #endregion
        
        #region Getters

        public EnemyDamage GetEnemyDamage() => _enemyDamage;

        public MonsterScrObj GetEnemyScrObj() => monsterScrObj;
        
        public EnemyDamage GetDamageSystem() => _enemyDamage;

        #endregion
    }
}