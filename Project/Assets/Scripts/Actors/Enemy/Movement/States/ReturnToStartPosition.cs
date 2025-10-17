using ScrObj.EnemyMoveScr;
using UnityEngine;

namespace Actors.Enemy.Movement.States
{
    public class ReturnToStartPosition : BaseMovementState
    {
        private readonly Vector2 _startPosition;

        private bool _hasArrived = false;
        private float _distance = 0.5f;
        
        public ReturnToStartPosition(EnemyMoveFsm fsm, BaseMovementContext context, Vector2 startPosition) : base(fsm, context)
        {
            _startPosition = startPosition;
        }

        public override void Enter()
        {
            base.Enter();
            _hasArrived = false;
            Debug.Log("Entering ReturnToStartPosition");
        }

        public override void PhysicsUpdate()
        {
            if (_hasArrived) return;
            
            base.PhysicsUpdate();
            
            if (FsmRealize.DetectTarget())
                StateMachine.ChangeState<PursuitPlayer>();
            
            BaseMove(_startPosition, MoveSettings.speed);

            // Проверка дистации от начальной точки
            if (CheckDistance(_startPosition, Rb2D.position, _distance))
            {
                _hasArrived = true; ;
                StateMachine.ChangeState<IdleMoveState>();
            }
        }

        public override void Exit()
        {
            base.Exit();
            _hasArrived = false;
        }
    }
}