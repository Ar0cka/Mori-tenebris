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
        
        public PursuitPlayer(EnemyMoveFsm fsm, BaseMovementContext context, 
           AggressiveSettings aggressiveSettings) : base(fsm, context)
        {
            _aggressiveSettings = aggressiveSettings;
        }

        public override void Enter()
        {
            if (!FsmRealize.OnSeePlayer)
                ChooseIdleState(MoveSettings);
            
            base.Enter();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            
            MoveToPlayer();
        }

        private void MoveToPlayer()
        {
            Vector2 targetPosition = FsmRealize.GetTargetPosition();
            
            _stopDistance = Vector2.Distance(Rb2D.position, targetPosition) <= _aggressiveSettings.stopDistance;
            
            if (targetPosition == Vector2.zero)
            {
                 StateMachine.ChangeState<LingerState>();
                 return;
            }
            
            if (!_stopDistance)
            {
                BaseMove(FsmRealize.GetTargetPosition(), MoveSettings.speed);
            }
        }
    }
}