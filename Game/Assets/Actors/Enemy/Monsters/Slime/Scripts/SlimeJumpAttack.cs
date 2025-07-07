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
        [SerializeField] private AttackConfig attackConfig;
        [SerializeField] private float jumpOffset;

        private SlimeConfig slimeConfig;
        private SlimeShield slimeShield;
        
        private Sequence jumpSequence;

        public void Initialize(EnemyDamage damageSys, SlimeConfig config, StateController attackStateCtrl,
            Transform playerTrans)
        {
            InitializeComponents(damageSys, attackStateCtrl);

            if (config != null || playerTrans != null)
            {
                slimeConfig = config; 
                _playerTransform = playerTrans;
            }

            if (slimeConfig != null)
                slimeShield = new SlimeShield(slimeConfig.startShield);
        }

        public override bool BeginAttack(AnimAttackSettings attackSettings)
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

        public override bool ExecuteHit()
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

        public override void EndAttack()
        {
            ResetAttackCooldown(_currentAttackConfig.cooldownAttack);
            _stateController.Jump(false, false);
            slimeShield.DamageAfterJump(slimeConfig.damageOutJump);
        }

        public override bool IsTargetInRange()
        {
            return true;
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
