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
        [SerializeField] private float minJumpDistance;
        [SerializeField] private float maxJumpDistance;

        private SlimeConfig _slimeConfig;
        private SlimeShield _slimeShield;
        
        private Sequence _jumpSequence;

        public void Initialize(EnemyDamage damageSys, SlimeConfig config, StateController attackStateCtrl,
            Transform playerTrans)
        {
            InitializeComponents(damageSys, attackStateCtrl, playerTrans);

            if (config != null || playerTrans != null)
            {
                _slimeConfig = config; 
                _playerTransform = playerTrans;
            }

            if (_slimeConfig != null){
                _slimeShield = new SlimeShield(_slimeConfig.startShield);
                MaxCountAttack = _slimeConfig.jumpNums;
            }
        }

        public override float BeginAttack()
        {
            Debug.Log($"Begin attack jump");
            
            try
            {
                _damageSystem.DamageUpdate(_currentAttackConfig);

                _jumpSequence?.Kill();

                _stateController.Jump(true, true);

                Vector3 targetPosition = _playerTransform.position;

                _jumpSequence = DOTween.Sequence()
                    .Append(slimeTransform
                        .DOJump(targetPosition, _slimeConfig.jumpForce, _slimeConfig.jumpNums, _slimeConfig.jumpTime)
                        .SetEase(Ease.Linear))
                    .Join(transform.DOScaleY(0.7f, _slimeConfig.jumpTime).SetLoops(2, LoopType.Yoyo)
                        .SetEase(Ease.OutBack));

                return _slimeConfig.jumpTime;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return 0;
            }
        }

        public override float ExecuteHit()
        {
            try
            {
                var hit = Physics2D.OverlapCircle(hitOrigin.position, attackRadius, LayerMask.GetMask("Player"));

                if (hit != null)
                {
                    var playerTakeDamage = hit.gameObject.GetComponentInChildren<PlayerTakeDamage>();
                    playerTakeDamage?.TakeHit(_damageSystem.Damage, _damageSystem.DamageType);
                }

                return _slimeConfig.delayAfterJump;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return 0;
            }
        }

        public override IEnumerator ExitComboCoroutine()
        {
            yield return new WaitForEndOfFrame();
        }
        
        public override bool EndAttack()
        {
            ResetAttackCooldown(_currentAttackConfig.cooldownAttack);
            _stateController.Jump(false, false);
            _slimeShield.DamageAfterJump(_slimeConfig.damageOutJump);
            
            return true;
        }

        public override bool IsTargetInRange() => IsDistanceInRange(minJumpDistance, maxJumpDistance);

        private void OnDisable()
        {
            _jumpSequence?.Kill();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(hitOrigin.position, attackRadius);
        }
#endif
    }
}
