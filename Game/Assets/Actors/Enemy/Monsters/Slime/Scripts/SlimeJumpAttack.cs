using System;
using System.Collections;
using Actors.Enemy.AttackSystem.Scripts;
using Actors.Enemy.Data.Scripts;
using Actors.Enemy.Monsters.AbstractEnemy;
using Actors.Enemy.Monsters.Slime.Data.Scripts;
using DefaultNamespace.Enums;
using DG.Tweening;
using Enemy.StatSystems.DamageSystem;
using PlayerNameSpace;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Actors.Enemy.Monsters.Slime
{
    public class SlimeJumpAttack : EnemyAttackBase
    {
        [SerializeField] private Transform slimeTransform;
        [SerializeField] private float jumpOffset;

        private SlimeConfig slimeConfig;
        private SlimeShield slimeShield;

        private Coroutine jumpCoroutine;
        private Sequence jumpSequence;

        public void Initialize(EnemyDamage damageSys,
            AttackConfig attackConfig, SlimeConfig config, StateController stateCtrl,
            Transform playerTrans)
        {
            InitializeComponents(damageSys, stateCtrl);

            if (config != null || attackConfig != null || playerTrans != null)
            {
                slimeConfig = config;
                _currentAttackConfig = attackConfig;
                _playerTransform = playerTrans;
            }

            if (slimeConfig != null)
                slimeShield = new SlimeShield(slimeConfig.startShield);
        }

        public override void TryAttack()
        {
            if (_stateController.CanAttack() && jumpCoroutine == null && attackCooldown <= 0)
            {
                Debug.Log("Jump");
                jumpCoroutine = StartCoroutine(PerformAttack(slimeConfig.jumpTime, slimeConfig.delayAfterJump, null));
            }
        }

        protected override bool BeginAttack(AnimAttackSettings attackSettings)
        {
            try
            {
                _damageSystem.DamageUpdate(_currentAttackConfig);

                jumpSequence?.Kill();

                _stateController.Jump(true, true);

                Vector3 targetPosition = _playerTransform.position;

                jumpSequence = DOTween.Sequence()
                    .Append(slimeTransform
                        .DOJump(targetPosition, slimeConfig.jumpForce, slimeConfig.jumpNums, slimeConfig.jumpTime)
                        .SetEase(Ease.Linear))
                    .Join(transform.DOScaleY(0.7f, slimeConfig.jumpTime).SetLoops(2, LoopType.Yoyo)
                        .SetEase(Ease.OutBack));

                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        protected override bool ExecuteHit()
        {
            try
            {
                var hit = Physics2D.OverlapCircle(hitOrigin.position, attackRadius, LayerMask.GetMask("Player"));

                if (hit != null)
                {
                    var playerTakeDamage = hit.gameObject.GetComponentInChildren<PlayerTakeDamage>();
                    playerTakeDamage?.TakeHit(_damageSystem.Damage, _damageSystem.DamageType);
                }

                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        protected override void EndAttack()
        {
            ResetAttackCooldown(_currentAttackConfig.cooldownAttack);
            _stateController.Jump(false, false);
            slimeShield.DamageAfterJump(slimeConfig.damageOutJump);
            jumpCoroutine = null;
        }

        private void OnDisable()
        {
            jumpSequence?.Kill();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(hitOrigin.position, attackRadius);
        }
#endif
    }
}
