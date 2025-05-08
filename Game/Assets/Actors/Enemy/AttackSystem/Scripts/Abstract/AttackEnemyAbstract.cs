using System;
using System.Collections.Generic;
using Actors.Enemy.Stats.Scripts;
using Enemy;
using PlayerNameSpace;
using UnityEngine;

namespace Actors.Enemy.AttackSystem.Scripts
{
    public abstract class AttackEnemyAbstract : MonoBehaviour
    {
        [SerializeField] private EnemyData enemyData;
        [SerializeField] protected List<string> baseAnimationList;
        [SerializeField] protected Animator animator;
        [SerializeField] protected Transform playerTransform;


        private float _cooldownAttack;

        private void Awake()
        {
            if (enemyData == null) enemyData = GetComponent<EnemyData>();
            if (animator == null) animator = GetComponent<Animator>();

            if (animator == null || enemyData == null || baseAnimationList == null)
            {
#if UNITY_EDITOR
                Debug.LogError(
                    $"EnemyData: {enemyData} / animator: {animator} / baseAnimationList: {baseAnimationList}");
#endif
                enabled = false;
            }

            ResetCooldownAttack();
        }

        /// <summary>
        /// Базовая updateLogic состоит из проверки на cooldown и вызова метода атаки.
        /// </summary>
        private void Update()
        {
            if (_cooldownAttack >= 0)
            {
                _cooldownAttack -= Time.deltaTime;
                return;
            }

            if (CheckAttackDistance())
            {
                AssingAttack();
                ResetCooldownAttack();
            }
        }

        /// <summary>
        /// Базово вызывает стандартную атаку.
        /// </summary>
        protected virtual void AssingAttack()
        {
            Attack(baseAnimationList);
        }

        /// <summary>
        /// Логика назначения атак, с помощью этого скрипта выстраивается очередь и условия атак.
        /// </summary>
        protected virtual void Attack(List<string> animationList)
        {
            for (int i = 0; i < baseAnimationList.Count; i++)
            {
                var currentAnimationPlaying = animator.GetCurrentAnimatorStateInfo(0).IsName(animationList[i]);

                Debug.Log(currentAnimationPlaying);

                if (!currentAnimationPlaying)
                {
                    animator.SetTrigger(animationList[i]);
                }
                
            }
        }

        /// <summary>
        /// Сброс кулдауна после атаки до первоначального состояния
        /// </summary>
        protected void ResetCooldownAttack()
        {
            _cooldownAttack = enemyData.GetEnemyScrObj().CooldownAttack;
        }

        /// <summary>
        /// Проверка дистации между игроком и монстром
        /// </summary>
        /// <returns></returns>
        protected bool CheckAttackDistance()
        {
            if (playerTransform == null) return false;

            return Vector2.Distance(playerTransform.position, transform.position) >=
                   enemyData.GetEnemyScrObj().AttackDistance;
        }

        /// <summary>
        /// Проверка проигрывания анимации
        /// </summary>
        /// <returns></returns>
        protected bool AnimationPlayChecker()
        {
            return animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f;
        }
    }
}