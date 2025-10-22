using ScrObj.EnemyMoveScr;
using UnityEngine;

namespace Actors.Enemy.Movement.Base.States
{
    public class ReturnToStartPosition : BaseMovementState<EnemyMoveFsmRealize, MoveData>
    {
        private readonly Vector2 _startPosition;

        private bool _hasArrived = false;
        private float _distance = 0.5f;
        
        public ReturnToStartPosition(EnemyMoveFsm fsm, DataContext<EnemyMoveFsmRealize, MoveData> dataContext,
            BaseMovementContext context, Vector2 startPosition) : base(fsm, dataContext, context)
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

            if (!CheckPlayer())
                return;
            
            BaseMove(_startPosition, MoveConfig.MoveSettings.speed);

            // Проверка дистации от начальной точки
            if (CheckDistance(_startPosition, Rb2D.position, _distance))
            {
                IdleState();
            }
        }

        protected virtual void IdleState() => ChangeState<IdleMoveState>();
        protected virtual void PursuitState() => ChangeState<PursuitPlayer>();

        protected virtual bool CheckPlayer()
        {
            if (IdleDetected(MoveConfig))
            {
                PursuitState();
                return false;
            }

            return true;
        }
        
        public override void Exit()
        {
            base.Exit();
            _hasArrived = false;
        }
    }
}