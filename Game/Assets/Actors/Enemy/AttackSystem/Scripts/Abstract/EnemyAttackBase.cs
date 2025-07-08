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
        [SerializeField] protected AttackScrConf attackConfig;

        [Header("Settings")] 
        [SerializeField] protected string attackName;
        [SerializeField] protected float attackExitDelay;
        [Min(0)][field:SerializeField] public float comboInputWindow;
        [SerializeField] protected List<string> hittableTags;

        protected int MaxCountAttack;
        protected int CurrentCountAttack;

        protected EnemyDamage _damageSystem;
        protected AttackConfig _currentAttackConfig;
        protected StateController _stateController;
        protected Coroutine _exitCoroutine;
        protected Transform _playerTransform;

        public bool IsOnCooldown => attackCooldown > 0;

        [Min(0)] protected float attackCooldown = 0;

        public string AttackName => attackName;

        #endregion
        
        #region Initialization

        protected void InitializeComponents(EnemyDamage damageSys, StateController stateCtrl, Transform playerTransform)
        {
            if (animator == null) animator = GetComponent<Animator>();

            if (damageSys != null) _damageSystem = damageSys;
            if (stateCtrl != null) _stateController = stateCtrl;
            if (playerTransform == null) _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

            if (animator == null && attackConfig == null)
            {
#if UNITY_EDITOR
                Debug.LogError($"Animator component missing on {gameObject.name}");
#endif
                enabled = false;
                return;
            }

            _currentAttackConfig = attackConfig.GetAttackConfig();
            _playerTransform = playerTransform;
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
        
        public abstract IEnumerator ExitComboCoroutine();

        #endregion
        
        #region Abstract Methods
        public abstract float BeginAttack();
        public abstract float ExecuteHit();
        public abstract bool EndAttack();
        public abstract bool IsTargetInRange();
        
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
        
        public int GetWeight()
        {
            return 0;
        }

        public AttackConfig GetAttackConfig() => _currentAttackConfig;

        #endregion

        #region DistanceDefaultChecers
        protected bool IsDistanceLess(float distance)
        {
            return GetPlayerDistance() <= distance;
        }
        protected bool IsDistanceInRange(float distanceMin, float distanceMax)
        {
            float dist = GetPlayerDistance();
            return dist > distanceMin && dist <= distanceMax;
        }
        protected float GetPlayerDistance()
        {
            return Vector2.Distance(transform.position, _playerTransform.position);
        }

        #endregion
        
        #region ExitFromCombo
        
        protected void ExitAttack()
        {
            _stateController.ChangeStateAttack(false);
            _exitCoroutine = null;
        }

        protected IEnumerator ExitFromComboCoroutine()
        {
            if (_exitCoroutine != null) yield break;

            CurrentCountAttack = 0;
            
            ResetAttackCooldown(_currentAttackConfig.cooldownAttack);
        
            yield return new WaitForSeconds(attackExitDelay);
        
            foreach (var attackEntry in _currentAttackConfig.animAttackSettings)
            {
                animator.ResetTrigger(attackEntry.nameTrigger);
            }
        
            ExitAttack();
        }
        
        #endregion
    }
}
