using Actors.Enemy.Movement;
using Actors.Enemy.Movement.States;
using ScrObj.EnemyMoveScr;
using UnityEngine;

namespace Actors.Enemy.Movement.States
{
    public class PursuitPlayer : BaseMovementState<EnemyMoveFsmRealize, MoveData>
    {
        private readonly AggressiveSettings _aggressiveSettings;
        private bool _stopDistance;
        
        public PursuitPlayer(EnemyMoveFsm fsm, DataContext<EnemyMoveFsmRealize, 
            MoveData> dataContext, BaseMovementContext context) : base(fsm, dataContext, context)
        {
            _aggressiveSettings = MoveConfig.AggressiveSettings;
        }

        public override void Enter()
        {
            if (DetectedPlayer(MoveConfig) == Vector2.zero)
                ChooseIdleState(MoveConfig.MoveSettings);
            
            base.Enter();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            
            MoveToPlayer();
        }

        private void MoveToPlayer()
        {
            Vector2 targetPosition = DetectedPlayer(MoveConfig);
            
            _stopDistance = Vector2.Distance(Rb2D.position, targetPosition) <= _aggressiveSettings.stopDistance;
            
            if (targetPosition == Vector2.zero)
            {
                 StateMachine.ChangeState<LingerState>();
                 return;
            }
            
            if (!_stopDistance)
            {
                BaseMove(targetPosition, MoveConfig.MoveSettings.speed);
            }
        }
    }
}