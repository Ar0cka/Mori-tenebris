using System;
using System.Collections;
using System.Collections.Generic;
using Actors.Enemy.Data.Scripts;
using Actors.Enemy.Monsters.AbstractEnemy;
using Actors.Enemy.Stats.Scripts;
using Enemy;
using Enemy.StatSystems.DamageSystem;
using PlayerNameSpace;
using UnityEngine;
using UnityEngine.Serialization;

namespace Actors.Enemy.AttackSystem.Scripts
{
    public abstract class AttackEnemyAbstract : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] protected Animator animator;
        [Range(0, 1)] [SerializeField] protected float checkAnimationDelay;
        
        protected EnemyDamage DamageSystem;
        protected int MaxComboAttack;
        protected int CurrentCountAttack;
        protected AttackConfig CurrentConfig;
        protected StateController StateController;
        protected Coroutine ExitCorutine;
        
      
        [Min(0)] protected float CooldownAttack = 0;


        protected void InitializeBaseComponents() 
        {
            if (animator == null) animator = GetComponent<Animator>();

            if (animator == null)
            {
#if UNITY_EDITOR
                Debug.LogError(
                    $"animator: {animator}");
#endif
                enabled = false;
            }
        }

        public abstract void AssingBaseAttack();
        
        /// <summary>
        /// Логика назначения атак, с помощью этого скрипта выстраивается очередь и условия атак.
        /// </summary>
        public virtual void Attack(string attackAnimation)
        {
            var currentAnimationPlaying = animator.GetCurrentAnimatorStateInfo(0).IsName(attackAnimation);

            if (!currentAnimationPlaying)
            {
                animator.SetTrigger(attackAnimation);
            }
        }

        protected void ResetCooldownAttack(float cooldown)
        {
            CooldownAttack = cooldown; 
        }

        /// <summary>
        /// Проверка дистации между игроком и монстром
        /// </summary>
        /// <returns></returns>
        protected virtual bool CheckAttackDistance(float attackDistance = 1)
        {
            var playerTransform = GetPlayerPosition.PlayerPosition().position;

            return Vector2.Distance(playerTransform, transform.position) <=
                   attackDistance;
        }

        /// <summary>
        /// Проверка проигрывания анимации
        /// </summary>
        /// <returns></returns>
        protected virtual bool AnimationPlayChecker()
        {
            return animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= checkAnimationDelay;
        }
        
        protected void ExitAction()
        {
            StateController.ChangeStateAttack(false);
            CurrentCountAttack = 0;
            ExitCorutine = null;
        }
    }
}