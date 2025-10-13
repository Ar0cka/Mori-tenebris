using System;
using System.Threading.Tasks;
using Actors.Player.AttackSystem;
using DG.Tweening;
using PlayerNameSpace;
using UnityEngine;

namespace StateMachin.States
{
    public class Roll : State
    {
        protected Rigidbody2D rb2D;
        protected SpriteRenderer _spriteRenderer;
        protected CapsuleCollider2D _capsuleCollider;
        protected Animator _animator;

        private PlayerStaticData _playerStaticData;
        private ISubtractionStamina _subtractionStamina;

        private bool _isRoll;
        private bool _isEndEnterToState;

        private Sequence _rollSequence;
        private Coroutine _rollCoroutine;

        public Roll(FStateMachine fsm, StateMachineRealize stateMachineRealize, PlayerScrObj playerScr,
            Rigidbody2D _rb2D,
            SpriteRenderer spriteRenderer, CapsuleCollider2D capsuleCollider,
            Animator animator, ISubtractionStamina subtractionStamina) :
            base(fsm, stateMachineRealize, playerScr)
        {
            _playerScrObj = playerScr;
            rb2D = _rb2D;
            _spriteRenderer = spriteRenderer;
            _capsuleCollider = capsuleCollider;
            _animator = animator;
            
            _subtractionStamina = subtractionStamina;

            _playerStaticData = _playerScrObj.StaticPlayerStats;
        }

        public override void Enter()
        {
            if (_subtractionStamina.CurrentStamina < _playerStaticData.CostRoll)
            {
                FStateMachine.ChangeState<IdleState>();
                return;
            }
            
            _capsuleCollider.enabled = false;

            base.Enter();

            RollingCharacter();

            _isRoll = true;
            _isEndEnterToState = true;
            PlayerStates.UpdateRolling(_isRoll);
        }

        public override void UpdateLogic()
        {
            if (!_isEndEnterToState) return;

            if (!CheckSequence() && !_isRoll)
            {
                PlayerStates.UpdateRolling(_isRoll);
                _stateMachineRealize.RollCooldownReset(_playerStaticData.RollCooldown);
                _capsuleCollider.enabled = true;
                FStateMachine.ChangeState<IdleState>();
            }
        }

        public override void SetAnimation()
        {
            base.SetAnimation();
            _animator.SetTrigger("Roll");
        }

        private void RollingCharacter()
        {
            _rollSequence?.Kill();
            
            Vector3 direction = GetInput().normalized;

            Vector2 flipX = _spriteRenderer.flipX ? Vector2.left : Vector2.right;
            
            _subtractionStamina.SubtractionStamina(_playerStaticData.CostRoll);
                
            Vector3 targetPos = direction != Vector3.zero
                ? rb2D.transform.position + direction * _playerStaticData.RollDistance
                : rb2D.transform.position + (Vector3)(flipX * _playerStaticData.RollDistance);

            _rollSequence = DOTween.Sequence(rb2D.transform.DOMove(targetPos, _playerStaticData.RollDuration)
                    .SetEase(Ease.Linear))
                .AppendInterval(_playerStaticData.RollDuration)
                .OnComplete(() =>
                {
                    _isRoll = false;
                });
        }

        private bool CheckSequence()
        {
            return _rollSequence != null && _rollSequence.IsActive() && _rollSequence.IsPlaying();
        }
    }
}