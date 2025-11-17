using ScrObj.EnemyMoveScr;
using UnityEngine;

namespace Actors.Enemy.Movement.Base.States
{
    public class PursuitPlayer : BaseMovementState<EnemyMoveFsm, MoveData>
    {
        private bool _stopDistance;
        
        public PursuitPlayer(BaseMovementContext<MoveData, EnemyMoveFsm> context) : base(context)
        {
        }

        public override void Enter()
        {
            if (DetectedPlayer(Ctx.Config.AggressiveSettings.detectionRadius, Ctx.Config.TargetMask) == Vector2.zero)
                ChooseIdleState<MileIdle, PatrolMoveState>(Ctx.Config.MoveSettings);
            
            base.Enter();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            
            MoveToPlayer();
        }

        private void MoveToPlayer()
        {
            Vector2 targetPosition = DetectedPlayer(Ctx.Config.AggressiveSettings.detectionRadius, Ctx.Config.TargetMask);
            
            _stopDistance = Vector2.Distance(Ctx.Rb2D.position, targetPosition) <= Ctx.Config.AggressiveSettings.stopDistance;
            
            if (targetPosition == Vector2.zero)
            {
                 Ctx.Fsm.ChangeState<LingerState>();
                 return;
            }
            
            if (!_stopDistance)
            {
                BaseMove(targetPosition, Ctx.Config.MoveSettings.speed);
            }
        }
    }
}