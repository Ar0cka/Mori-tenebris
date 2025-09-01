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
        
        private float _subtractionStaminaOnRun;
        
        public SprintRun(FStateMachine fsm, StateMachineRealize stateMachineRealize, PlayerScrObj playerScr, Rigidbody2D _rb2D, ISubtractionStamina subtractionStamina, 
            SpriteRenderer spriteRenderer, MovementColliderOffset movementColliderOffset, CapsuleCollider2D capsuleCollider, Animator animator) :
            base(fsm, stateMachineRealize, playerScr, _rb2D, spriteRenderer, movementColliderOffset, capsuleCollider, animator)
        {
            _playerScrObj = playerScr;
            _subtractionStamina = subtractionStamina;
            rb2D = _rb2D;

            _subtractionStaminaOnRun = _playerScrObj.StaticPlayerStats.RunSubtraction;
        }

        public override void Enter()
        {
            base.Enter();
            _staminaCount = 0;
        }

        public override void UpdateLogic()
        {
            if (Input.GetKeyUp(KeyCode.LeftShift) || _subtractionStamina.CurrentStamina < _subtractionStaminaOnRun)
            {
                FStateMachine.ChangeState<MovementState>();
            }
        }

        public override void PhysicUpdate()
        {
            Move(_playerScrObj.StaticPlayerStats.SpritSpeed);
            ChangeSpriteSide(GetInput().normalized);
            SubstractionStamina();
        }

        private void SubstractionStamina()
        {
            try
            {
                _staminaCount += _subtractionStaminaOnRun;
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