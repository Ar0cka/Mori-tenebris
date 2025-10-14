using Actors.Enemy.Movement;
using Actors.Enemy.Movement.States;
using ScrObj.EnemyMoveScr;
using UnityEngine;

namespace Actors.Enemy.Movement.States
{
    public class PursuitPlayer : MoveEnemyFsmUnityState
    {
        private readonly Rigidbody2D _rigidbody;
        private readonly EnemyMoveFsmRealize _fsmRealize;
        private MoveSettings _moveSettings;
        private AggressiveSettings _aggressiveSettings;

        private bool _stopDistance;
        
        public PursuitPlayer(EnemyMoveFsm fsm, Rigidbody2D rigidbody2D,
           EnemyMoveFsmRealize fsmRealize , MoveSettings moveData, AggressiveSettings aggressiveSettings) : base(fsm)
        {
            _rigidbody = rigidbody2D;
            _fsmRealize = fsmRealize;
            _moveSettings = moveData;
            _aggressiveSettings = aggressiveSettings;
        }

        public override void Enter()
        {
            if (!_fsmRealize.OnSeePlayer)
                ChooseIdleState(_moveSettings);
        }

        public override void PhysicsUpdate()
        {
            MoveToPlayer();
        }

        private void MoveToPlayer()
        {
            Vector2 targetPosition = _fsmRealize.GetTargetPosition();

            _stopDistance = Vector2.Distance(_rigidbody.position, targetPosition) <= _aggressiveSettings.stopDistance;

            if (targetPosition == Vector2.zero)
                StateMachine.ChangeState<ReturnToStartPosition>();
            
            if (!_stopDistance)
            {
                Vector2 direction = (_fsmRealize.GetTargetPosition() - _rigidbody.position).normalized;
                Vector2 moveDirection = direction * _moveSettings.speed * Time.deltaTime;
                _rigidbody.MovePosition(_rigidbody.position + moveDirection);
            }
        }

        public override void Exit()
        {
            _fsmRealize.ChangeViewState(false);
        }
    }
}