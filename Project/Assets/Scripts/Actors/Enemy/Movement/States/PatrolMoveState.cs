using System;
using ScrObj.EnemyMoveScr;
using UnityEngine;
using UnityEngine.Rendering;

namespace Actors.Enemy.Movement.States
{
    public class PatrolMoveState : BaseMovementState
    {
        private PatrolSettings _patrolSettings;
        
        private int _nodeNumber = 0;
        
        public PatrolMoveState(EnemyMoveFsm fsm, BaseMovementContext context,
            PatrolSettings patrolSettings) 
            : base(fsm, context)
        {
            _patrolSettings = patrolSettings;
        }

        public override void Enter()
        {
            if (_patrolSettings.patrolPoints.Length == 0)
                StateMachine.ChangeState<IdleMoveState>();
            
            base.Enter();   
            
            _nodeNumber = Math.Clamp(_nodeNumber, 0, _patrolSettings.patrolPoints.Length - 1);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            MoveToNode();
        }

        private void MoveToNode()
        {
            if (FsmRealize.DetectTarget())
                StateMachine.ChangeState<PursuitPlayer>();
            
            Vector2 targetPosition = _patrolSettings.patrolPoints[_nodeNumber];
            
            BaseMove(targetPosition, _patrolSettings.patrolSpeed);

            if (CheckDistance(targetPosition, Rb2D.position, _patrolSettings.switchNodeDistance))
            {
                _nodeNumber++;

                if (_nodeNumber >= _patrolSettings.patrolPoints.Length)
                {
                    _nodeNumber = 0;
                }
            }
        }
    }
}