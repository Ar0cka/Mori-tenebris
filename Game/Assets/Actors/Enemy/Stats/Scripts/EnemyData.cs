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
        [Header("Components")]
        [SerializeField] private MonsterScrObj monsterScrObj;
        [SerializeField] private Animator animator;
      
        [Header("Delays")]
        [SerializeField] private float delayForDestroy = 2f;
        [SerializeField] private float exitFromHitDelay = 1f;

        [Header("TriggersNames")] 
        [SerializeField] private string deathTrigger;
        [SerializeField] private string hitTrigger;

        #region param

        private int _currentHitPoints;
        private EnemyArmour _enemyArmour;
        private EnemyDamage _enemyDamage;
        private StateController _stateController;
        private EnemyConfig _enemyConfig;

        public Transform PlayerPosition { get; private set; }

        private Coroutine _exitCoroutine;
        private float _cooldown;
        private int _countHit;

        #endregion

        public void Initialize()
        {
            PlayerPosition = GameObject.FindGameObjectWithTag("Player").transform;

            _enemyConfig = monsterScrObj.GetConfig();
            _currentHitPoints = _enemyConfig.hitPoints;
            Debug.Log("current hit points = " + _currentHitPoints);
            _enemyArmour = new EnemyArmour(monsterScrObj.GetConfig());
            _enemyDamage = new EnemyDamage();
            _stateController = new StateController();
        }

        #region Health

        private void Update()
        {
            if (_countHit >= _enemyConfig.maxCountHit)
            {
                _cooldown = _enemyConfig.cooldownHit;
                _countHit = 0;
            }

            if (_cooldown > 0)
            {
                _cooldown -= Time.deltaTime;
            }
        }

        public void TakeDamage(int damage, DamageType damageType)
        {
            if (_stateController.IsDeath) return;

            int finalDamage =
                CalculateDamage.CalculateFinalDamageWithResist(damage, _enemyArmour.GetArmour(damageType));

            _currentHitPoints -= finalDamage;

            EventBus.Publish(new HitEnemyEvent());

            HitTrigger();

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

        protected virtual void HitTrigger()
        {
            if (_stateController.CanHit() && _cooldown <= 0)
            {
                _countHit++;
                animator.SetTrigger(hitTrigger);
                _stateController.ChangeStateHit(true);
            }
            
            if (_stateController.IsHit && _exitCoroutine == null)
            {
                _exitCoroutine = StartCoroutine(ExitFromHit());
            }
        }

        #region Getters

        public EnemyDamage GetEnemyDamage() => _enemyDamage;

        public MonsterScrObj GetEnemyScrObj() => monsterScrObj;

        public EnemyDamage GetDamageSystem() => _enemyDamage;

        #endregion
    }
}