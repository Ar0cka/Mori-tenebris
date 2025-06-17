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
    public class SlimeJumpAttack : AttackEnemyAbstract
    {
        [SerializeField] private Transform slimeTransform;
        [SerializeField] private float offsetJump;

        private SlimeConfig _slimeConfig;
        private SlimeShield _slimeShield;

        private Coroutine _jumpCoroutine;
        private Sequence _jumpSequence;

        public void Initialize(EnemyDamage damageSystem,
            AttackConfig attackConfig, SlimeConfig slimeConfig, StateController stateController,
            Transform playerTransform)
        {
            InitializeBaseComponents(damageSystem, stateController);

            if (slimeConfig != null || attackConfig != null || playerTransform != null)
            {
                _slimeConfig = slimeConfig;
                CurrentAttackConfig = attackConfig;
                PlayerTransform = playerTransform;
            }

            if (_slimeConfig != null)
                _slimeShield = new SlimeShield(_slimeConfig.startShield);
        }

        public override void TryAttack()
        {
            if (StateController.CanAttack() && _jumpCoroutine == null && CooldownAttack <= 0)
            {
                Debug.Log("Jump");
                _jumpCoroutine = StartCoroutine(Attack());
            }
        }

        private IEnumerator Attack()
        {
            if (!StartAttack()) yield break;

            yield return new WaitForSeconds(_slimeConfig.jumpTime);

            if (!Hit()) yield break;

            yield return new WaitForSeconds(_slimeConfig.delayAfterJump);

            EndAttack();
        }

        protected virtual bool StartAttack()
        {
            try
            {
                DamageSystem.DamageUpdate(CurrentAttackConfig);

                _jumpSequence?.Kill();

                StateController.Jump(true, true);

                Vector3 targetPosition = PlayerTransform.position;

                _jumpSequence = DOTween.Sequence()
                    .Append(slimeTransform
                        .DOJump(targetPosition, _slimeConfig.jumpForce, _slimeConfig.jumpNums, _slimeConfig.jumpTime)
                        .SetEase(Ease.Linear))
                    .Join(transform.DOScaleY(0.7f, _slimeConfig.jumpTime).SetLoops(2, LoopType.Yoyo)
                        .SetEase(Ease.OutBack));

                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        private bool Hit()
        {
            try
            {
                var hit = Physics2D.OverlapCircle(hitPosition.position, radiusAttack, LayerMask.GetMask("Player"));

                if (hit != null)
                {
                    var playerTakeHit = hit.gameObject.GetComponentInChildren<PlayerTakeDamage>();
                    playerTakeHit.TakeHit(DamageSystem.Damage, DamageSystem.DamageType);
                }

                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        private void EndAttack()
        {
            ResetCooldownAttack(CurrentAttackConfig.cooldownAttack);
            StateController.Jump(false, false);
            _slimeShield.DamageAfterJump(_slimeConfig.damageOutJump);
            _jumpCoroutine = null;
        }

        private void OnDisable()
        {
            _jumpSequence.Kill();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(hitPosition.position, radiusAttack);
        }
#endif
    }
}