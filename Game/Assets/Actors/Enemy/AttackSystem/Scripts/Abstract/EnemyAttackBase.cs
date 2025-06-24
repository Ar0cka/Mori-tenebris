using System;
using System.Collections;
using System.Collections.Generic;
using Actors.Enemy.Data.Scripts;
using Actors.Enemy.Monsters.AbstractEnemy;
using Enemy.StatSystems.DamageSystem;
using NegativeEffects;
using PlayerNameSpace;
using UnityEngine;

namespace Actors.Enemy.AttackSystem.Scripts
{
    public abstract class EnemyAttackBase : MonoBehaviour
    {
        #region Fields

        [Header("Components")] 
        [SerializeField] protected Animator animator;
        [SerializeField] protected SpriteRenderer spriteRenderer;
        [SerializeField] protected Transform hitOrigin;
        [SerializeField] protected float attackRadius;

        [Header("Settings")] 
        [SerializeField] protected string attackName;
        [SerializeField] protected float attackExitDelay;
        [SerializeField] protected float comboInputWindow;
        [SerializeField] protected List<string> hittableTags;

        protected EnemyDamage _damageSystem;
        protected int _maxComboCount;
        protected int _currentComboCount;
        protected AttackConfig _currentAttackConfig;
        protected StateController _stateController;
        protected Coroutine _exitCoroutine;
        protected Transform _playerTransform;

        public bool IsOnCooldown => attackCooldown > 0;

        [Min(0)] protected float attackCooldown = 0;

        public string AttackName => attackName;

        #endregion
        
        #region Initialization

        protected void InitializeComponents(EnemyDamage damageSys, StateController stateCtrl)
        {
            if (animator == null) animator = GetComponent<Animator>();

            if (damageSys != null) _damageSystem = damageSys;
            if (stateCtrl != null) _stateController = stateCtrl;

            if (animator == null)
            {
#if UNITY_EDITOR
                Debug.LogError($"Animator component missing on {gameObject.name}");
#endif
                enabled = false;
            }
        }
        
        #endregion

        #region Attack Logic

        protected bool ApplyDamage(ITakeDamage target)
        {
            if (target == null || _currentAttackConfig == null) return false;
            
            target.TakeHit(_currentAttackConfig.damage, _currentAttackConfig.damageType);
            
            return true;
        }

        protected bool ApplyEffect(ITakeDamage target, EffectScrObj effect)
        {
            if (target == null || effect == null) return false;
            
            target.AddEffect(effect);
            return true;
        }

        protected bool ApplyDamageWithEffect(ITakeDamage target, EffectScrObj effect)
        {
            if (target == null || effect == null) return false;

            var damageApplied = ApplyDamage(target);
            var effectApplied = ApplyEffect(target, effect);

            return damageApplied && effectApplied;
        }
        
        protected void ResetAttackCooldown(float cooldown)
        {
            attackCooldown = cooldown;
        }

        protected virtual IEnumerator PerformAttack(float delayBeforeHit, float delayAfterHit, AnimAttackSettings attackSettings)
        {
            if (delayAfterHit == 0 || delayBeforeHit == 0)
            {
                delayBeforeHit = 1;
                delayAfterHit = 1;
            }
            
            if (!BeginAttack(attackSettings)) yield break;
            
            yield return new WaitForSeconds(delayBeforeHit);

            if (!ExecuteHit()) yield break;

            yield return new WaitForSeconds(delayAfterHit);
            
            EndAttack();
        }
        
        protected void ExitAttack()
        {
            _stateController.ChangeStateAttack(false);
            _currentComboCount = 0;
            _exitCoroutine = null;
        }

        #endregion
        
        #region Abstract Methods
        
        public abstract void TryAttack();

        protected abstract bool BeginAttack(AnimAttackSettings animAttack);

        protected abstract bool ExecuteHit();
        protected abstract void EndAttack();
        
        #endregion

        #region Unity Callbacks

        protected virtual void Update()
        {
            if (attackCooldown > 0)
            {
                attackCooldown -= Time.deltaTime;
            }
        }
        
        public virtual bool PlayAttackAnimation(AnimAttackSettings attackSettings, float x)
        {
            try
            {
                var stateInfo = animator.GetCurrentAnimatorStateInfo(0);

                if (!stateInfo.IsName(attackSettings.nameTrigger))
                {
                    animator.SetFloat("X", x);
                    animator.SetTrigger(attackSettings.nameTrigger);
                }

                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                return false;
            }
        }
        
        protected virtual bool IsPlayerInRange(float range = 1f)
        {
            if (_playerTransform == null) return false;

            return Vector2.Distance(_playerTransform.position, transform.position) <= range;
        }

        #endregion
    }
}
