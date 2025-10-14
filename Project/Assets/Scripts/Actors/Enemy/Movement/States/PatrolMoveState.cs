using System;
using ScrObj.EnemyMoveScr;
using UnityEngine;
using UnityEngine.Rendering;

namespace Actors.Enemy.Movement.States
{
    public class PatrolMoveState : MoveEnemyFsmUnityState
    {
        private PatrolSettings _patrolSettings;
        private EnemyMoveFsmRealize _fsmRealize;
        private Rigidbody2D _rb2D;
        
        private int _nodeNumber = 0;
        
        public PatrolMoveState(EnemyMoveFsm fsm, EnemyMoveFsmRealize fsmRealize, Rigidbody2D rigidbody2D,
            PatrolSettings patrolSettings) 
            : base(fsm)
        {
            _patrolSettings = patrolSettings;
            _rb2D = rigidbody2D;
            _fsmRealize = fsmRealize;
        }

        public override void Enter()
        {
            if (_patrolSettings.patrolPoints.Length == 0)
                StateMachine.ChangeState<IdleMoveState>();
            
            _nodeNumber = Math.Clamp(_nodeNumber, 0, _patrolSettings.patrolPoints.Length - 1);
        }

        public override void PhysicsUpdate()
        {
            MoveToNode();
        }

        private void MoveToNode()
        {
            if (_fsmRealize.DetectTarget())
                StateMachine.ChangeState<PursuitPlayer>();
            
            Vector2 targetPosition = _patrolSettings.patrolPoints[_nodeNumber];
            Vector2 currentPosition = _rb2D.position;
            
            Vector2 moveDirection = (targetPosition - currentPosition).normalized;
            _rb2D.MovePosition(currentPosition + moveDirection * _patrolSettings.patrolSpeed * Time.deltaTime);

            if (CheckDistance(targetPosition, currentPosition))
            {
                _nodeNumber++;

                if (_nodeNumber >= _patrolSettings.patrolPoints.Length)
                {
                    _nodeNumber = 0;
                }
            }
        }

        private bool CheckDistance(Vector2 targetPosition, Vector2 currentPosition)
        {
            return Vector2.Distance(targetPosition, currentPosition) <= _patrolSettings.switchNodeDistance;
        }
    }
}