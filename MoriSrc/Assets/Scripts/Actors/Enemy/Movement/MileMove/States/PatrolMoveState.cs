using System;
using ScrObj.EnemyMoveScr;
using UnityEngine;
using UnityEngine.Rendering;

namespace Actors.Enemy.Movement.Base.States
{
    public class PatrolMoveState : BaseMovementState<EnemyMoveFsm, MoveData>
    {
        private PatrolSettings _patrolSettings;
        
        private int _nodeNumber = 0;
        
        public PatrolMoveState(BaseMovementContext<MoveData, EnemyMoveFsm> context) 
            : base(context)
        {
            _patrolSettings = Ctx.Config.PatrolSettings;
        }

        public override void Enter()
        {
            if (_patrolSettings.patrolPoints.Count == 0)
                Ctx.Fsm.ChangeState<MileIdle>();
            
            base.Enter();   
            
            _nodeNumber = Math.Clamp(_nodeNumber, 0, _patrolSettings.patrolPoints.Count - 1);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            MoveToNode();
        }

        private void MoveToNode()
        {
            if (IdleDetected(Ctx.Config))
                Ctx.Fsm.ChangeState<PursuitPlayer>();
            
            Vector2 targetPosition = _patrolSettings.patrolPoints[_nodeNumber];
            
            BaseMove(targetPosition, _patrolSettings.patrolSpeed);

            if (CheckDistance(targetPosition, Ctx.Rb2D.position, _patrolSettings.switchNodeDistance))
            {
                _nodeNumber++;

                if (_nodeNumber >= _patrolSettings.patrolPoints.Count)
                {
                    _nodeNumber = 0;
                }
            }
        }
    }
}