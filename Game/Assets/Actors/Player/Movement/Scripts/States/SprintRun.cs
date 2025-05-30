using System;
using Actors.Player.Movement.Scripts;
using PlayerNameSpace;
using UnityEngine;
using Zenject;

namespace StateMachin.States
{
    public class SprintRun : MovementState
    {
        private ISubtractionStamina _subtractionStamina;
        private float _staminaCount;
        private const float StaminaCheckThreshold = 1f;
        
        public SprintRun(FStateMachine fsm, StateMachineRealize stateMachineRealize, PlayerScrObj playerScr, Rigidbody2D _rb2D, ISubtractionStamina subtractionStamina, 
            SpriteRenderer spriteRenderer, MovementOffsetScr movementOffsetScr, CapsuleCollider2D capsuleCollider) :
            base(fsm, stateMachineRealize, playerScr, _rb2D, spriteRenderer, movementOffsetScr, capsuleCollider)
        {
            _playerScr = playerScr;
            _subtractionStamina = subtractionStamina;
            rb2D = _rb2D;
        }

        public override void Enter()
        {
            base.Enter();
            _staminaCount = 0;
        }

        public override void UpdateLogic()
        {
            if (InputVector().sqrMagnitude == 0)
            {
                FStateMachine.ChangeState<IdleState>();
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                FStateMachine.ChangeState<MovementState>();
            }
        }

        public override void PhysicUpdate()
        {
            Move(_playerScr.StaticPlayerStats.SpritSpeed);
            ChangeSpriteSide(InputVector().normalized);
            SubstractionStamina();
        }

        private void SubstractionStamina()
        {
            try
            {
                _staminaCount += _subtractionStamina.SubstractionCount;
                if (_staminaCount >= StaminaCheckThreshold)
                {
                    int staminaSubstraction = Mathf.FloorToInt(_staminaCount);
                    _subtractionStamina.SubtractionStamina(staminaSubstraction);
                    _staminaCount -= staminaSubstraction;
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}