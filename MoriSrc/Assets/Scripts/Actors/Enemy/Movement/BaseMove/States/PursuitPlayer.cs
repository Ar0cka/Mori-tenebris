using Actors.Enemy.Movement;
using Actors.Enemy.Movement.States;
using ScrObj.EnemyMoveScr;
using UnityEngine;

namespace Actors.Enemy.Movement.States
{
    public class PursuitPlayer : BaseMovementState
    {
        private readonly AggressiveSettings _aggressiveSettings;
        private bool _stopDistance;
        
        public PursuitPlayer(EnemyMoveFsm fsm, BaseMovementContext context) : base(fsm, context)
        {
            _aggressiveSettings = MoveData.AggressiveSettings;
        }

        public override void Enter()
        {
            if (!Realize.OnSeePlayer)
                ChooseIdleState(MoveData.MoveSettings);
            
            base.Enter();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            
            MoveToPlayer();
        }

        private void MoveToPlayer()
        {
            Vector2 targetPosition = DetectedPlayer();
            
            _stopDistance = Vector2.Distance(Rb2D.position, targetPosition) <= _aggressiveSettings.stopDistance;
            
            if (targetPosition == Vector2.zero)
            {
                 StateMachine.ChangeState<LingerState>();
                 return;
            }
            
            if (!_stopDistance)
            {
                BaseMove(targetPosition, MoveData.MoveSettings.speed);
            }
        }
    }
}