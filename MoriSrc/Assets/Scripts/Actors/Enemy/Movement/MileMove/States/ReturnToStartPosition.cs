using ScrObj.EnemyMoveScr;
using UnityEngine;

namespace Actors.Enemy.Movement.Base.States
{
    public class ReturnToStartPosition : BaseMovementState<EnemyMoveFsm, MoveData>
    {
        private readonly Vector2 _startPosition;

        private bool _hasArrived = false;
        private float _distance = 0.5f;
        
        public ReturnToStartPosition(BaseMovementContext<MoveData, EnemyMoveFsm> context, Vector2 startPosition) : base(context)
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
            
            BaseMove(_startPosition, Ctx.Config.MoveSettings.speed);

            // Проверка дистации от начальной точки
            if (CheckDistance(_startPosition, Ctx.Rb2D.position, _distance))
            {
                IdleState();
            }
        }

        protected virtual void IdleState() => ChangeState<MileIdle>();
        protected virtual void PursuitState() => ChangeState<PursuitPlayer>();
        protected virtual bool CheckPlayer()
        {
            if (IdleDetected(Ctx.Config))
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