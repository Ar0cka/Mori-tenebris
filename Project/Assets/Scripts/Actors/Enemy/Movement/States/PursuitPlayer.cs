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
        
        public PursuitPlayer(EnemyMoveFsm fsm, Rigidbody2D rigidbody2D,
           EnemyMoveFsmRealize fsmRealize , MoveSettings moveData) : base(fsm)
        {
            _rigidbody = rigidbody2D;
            _fsmRealize = fsmRealize;
            _moveSettings = moveData;
        }

        public override void Enter()
        {
            if (!_fsmRealize.OnSeePlayer)
                StateMachine.ChangeState<IdleMoveState>();
        }

        public override void PhysicsUpdate()
        {
            MoveToPlayer();
        }

        private void MoveToPlayer()
        {
            Vector2 targetPosition = _fsmRealize.GetTargetPosition();
            
            if (targetPosition == Vector2.zero)
                StateMachine.ChangeState<ReturnToStartPosition>();
            
            Vector2 direction = (_fsmRealize.GetTargetPosition() - _rigidbody.position).normalized;
            Vector2 moveDirection = direction * _moveSettings.speed * Time.deltaTime;
            _rigidbody.MovePosition(_rigidbody.position + moveDirection);
        }

        public override void Exit()
        {
            _fsmRealize.ChangeViewState(false);
        }
    }
}