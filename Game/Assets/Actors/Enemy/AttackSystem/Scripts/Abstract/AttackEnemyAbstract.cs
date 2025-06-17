using System;
using System.Collections;
using System.Collections.Generic;
using Actors.Enemy.Data.Scripts;
using Actors.Enemy.Monsters.AbstractEnemy;
using Actors.Enemy.Stats.Scripts;
using Enemy;
using Enemy.StatSystems.DamageSystem;
using NegativeEffects;
using PlayerNameSpace;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Actors.Enemy.AttackSystem.Scripts
{
    public abstract class AttackEnemyAbstract : MonoBehaviour
    {
        [Header("Components")] 
        [SerializeField] protected Animator animator;
        [SerializeField] protected Transform hitPosition;
        [SerializeField] protected float radiusAttack;

        [Header("Settings")] 
        [SerializeField] protected string attackName;
        [SerializeField] protected float exitDelay;
        [SerializeField] protected float comboWindow;
        

        protected EnemyDamage DamageSystem;
        protected int MaxComboAttack;
        protected int CurrentCountAttack;
        protected AttackConfig CurrentAttackConfig;
        protected StateController StateController;
        protected Coroutine ExitCorutine;
        protected Transform PlayerTransform;

        public bool IsCooldown => CooldownAttack > 0;

        [Min(0)] protected float CooldownAttack = 0;

        public string AttackName => attackName;

        protected void InitializeBaseComponents(EnemyDamage damageSystem, StateController stateController)
        {
            if (animator == null) animator = GetComponent<Animator>();

            if (damageSystem != null || stateController != null)
            {
                DamageSystem = damageSystem;
                StateController = stateController;
            }

            if (animator == null)
            {
#if UNITY_EDITOR
                Debug.LogError(
                    $"animator: {animator}");
#endif
                enabled = false;
            }
        }

        protected virtual void Update()
        {
            if (CooldownAttack > 0)
            {
                CooldownAttack -= Time.deltaTime;
            }
        }

        public abstract void TryAttack();

        /// <summary>
        /// Логика назначения атак, с помощью этого скрипта выстраивается очередь и условия атак.
        /// </summary>
        public virtual void StartAnimation(AnimAttackSettings currentAttackConfig)
        {
            var stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            if (!stateInfo.IsName(currentAttackConfig.nameTrigger))
                animator.SetTrigger(currentAttackConfig.nameTrigger);
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
            return Vector2.Distance(PlayerTransform.position, transform.position) <=
                   attackDistance;
        }

        protected void ExitAction()
        {
            StateController.ChangeStateAttack(false);
            CurrentCountAttack = 0;
            ExitCorutine = null;
        }

        protected Collider2D HitCollider(Vector2 position, float radius)
        {
            var hit = Physics2D.OverlapCircle(position, radius);

            if (hit != null)
            {
                return hit;
            }

            return null;
        }
    }
}