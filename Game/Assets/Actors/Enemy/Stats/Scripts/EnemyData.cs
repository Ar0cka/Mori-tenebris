using System;
using System.Collections;
using Actors.Enemy.Data.Scripts;
using Actors.Enemy.Monsters.AbstractEnemy;
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
        [SerializeField] private float exitFromHitDelay = 1f;
        
        [Header("TriggersNames")] 
        [SerializeField] private string deathTrigger;
        [SerializeField] private string hitTrigger;

        private int _currentHitPoints;
        private EnemyArmour _enemyArmour;
        private EnemyDamage _enemyDamage;
        private StateController _stateController;
        
        public Transform PlayerPosition { get; private set; }
        
        private Coroutine _exitCoroutine;

        public void Initialize()
        { 
            PlayerPosition = GameObject.FindGameObjectWithTag("Player").transform;
            
            _currentHitPoints = monsterScrObj.GetConfig().hitPoints;
            Debug.Log("current hit points = " + _currentHitPoints);
            _enemyArmour = new EnemyArmour(monsterScrObj.GetConfig());
            _enemyDamage = new EnemyDamage();
            _stateController = new StateController();
        }
        
        #region Health

        public void TakeDamage(int damage, DamageType damageType)
        {
            if (_stateController.IsDeath) return;
            
            int finalDamage = CalculateDamage.CalculateFinalDamageWithResist(damage, _enemyArmour.GetArmour(damageType));
            
            _currentHitPoints -= finalDamage;
            
            EventBus.Publish(new HitEnemyEvent());

            if (!_stateController.IsHit)
            {
                animator.SetTrigger(hitTrigger);
                _stateController.ChangeStateHit(true);
            }

            if (_stateController.IsHit && _exitCoroutine == null)
            {
                _exitCoroutine = StartCoroutine(ExitFromHit());
            }
            
            CheckDie();
        }
        
        private void CheckDie()
        {
            if (_currentHitPoints <= 0)
            {
                _stateController.ChangeStateDeath(true);
                animator.SetTrigger(deathTrigger);
                EventBus.Publish(new SendDieEventEnemy());
                
                Destroy(gameObject, delayForDestroy);
            }
        }

        public StateController GetStateController()
        {
            if (_stateController == null) return new StateController();
            
            return _stateController;
        }

        private IEnumerator ExitFromHit()
        {
            yield return new WaitForSeconds(exitFromHitDelay);
            
            animator.ResetTrigger(hitTrigger);
            _stateController.ChangeStateHit(false);  
            _exitCoroutine = null;
        }
        
        #endregion
        
        #region Getters

        public EnemyDamage GetEnemyDamage() => _enemyDamage;

        public MonsterScrObj GetEnemyScrObj() => monsterScrObj;
        
        public EnemyDamage GetDamageSystem() => _enemyDamage;

        #endregion
    }
}