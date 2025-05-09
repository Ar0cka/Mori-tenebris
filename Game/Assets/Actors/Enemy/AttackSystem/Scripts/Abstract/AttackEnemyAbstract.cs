using System;
using System.Collections.Generic;
using Actors.Enemy.Stats.Scripts;
using Enemy;
using PlayerNameSpace;
using UnityEngine;

namespace Actors.Enemy.AttackSystem.Scripts
{
    public class AttackEnemyAbstract : MonoBehaviour
    {
        [SerializeField] private EnemyData enemyData;
        [SerializeField] protected Animator animator;
        [SerializeField] protected Transform playerTransform;


        private float _cooldownAttack = 0;

        private void Awake()
        {
            if (enemyData == null) enemyData = GetComponent<EnemyData>();
            if (animator == null) animator = GetComponent<Animator>();

            if (animator == null || enemyData == null)
            {
#if UNITY_EDITOR
                Debug.LogError(
                    $"EnemyData: {enemyData} / animator: {animator}");
#endif
                enabled = false;
            }
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
        /// Обработка атак
        /// </summary>
        public virtual void AssingAttack()
        {
            
        }

        /// <summary>
        /// Логика назначения атак, с помощью этого скрипта выстраивается очередь и условия атак.
        /// </summary>
        protected void Attack(List<string> animationList)
        {
            for (int i = 0; i < animationList.Count; i++)
            {
                if (!CheckAttackDistance())
                {
                    foreach (var animation in animationList)
                    {
                        animator.ResetTrigger(animation);
                    }
                    break;
                }
                
                var currentAnimationPlaying = animator.GetCurrentAnimatorStateInfo(0).IsName(animationList[i]);

                if (!currentAnimationPlaying)
                {
                    animator.SetTrigger(animationList[i]);
                }
            }
        }
        
        private void ResetCooldownAttack()
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

            return Vector2.Distance(playerTransform.position, transform.position) <=
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