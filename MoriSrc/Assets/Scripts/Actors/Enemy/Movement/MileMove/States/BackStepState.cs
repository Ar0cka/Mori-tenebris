using ScrObj.EnemyMoveScr;
using UnityEngine;

namespace Actors.Enemy.Movement.Base.States
{
    public class BackStepState : BaseMovementState<EnemyMoveFsm, MoveData>
    {
        private Vector2 _currentTarget;
        
        public BackStepState(BaseMovementContext<MoveData, EnemyMoveFsm> context) : base(context)
        {
        }

        public override void PhysicsUpdate()
        {
            Backstep();
        }

        protected virtual void Backstep()
        {
            Vector2 objectPosition = DetectedPlayer(Ctx.Config.AggressiveSettings.detectionRadius, Ctx.Config.TargetMask);
            
            if (Vector2.Distance(Ctx.Rb2D.position, objectPosition) <= Ctx.Config.BackStepSettings.maxDistance)
            {
                Ctx.Fsm.ChangeState<MileIdle>(); //В будущем сменить на Idle атаки либо иные виды, например регенирация
            }
            
            if (_currentTarget == Vector2.zero || Vector2.Distance(Ctx.Rb2D.position, _currentTarget) <= Ctx.Config.BackStepSettings.distanceForChangeVector)
            {
                if (objectPosition == Vector2.zero)
                {
                    ChooseIdleState<MileIdle, PatrolMoveState>(Ctx.Config.MoveSettings);
                    return;
                }
                
                _currentTarget = Ctx.Rb2D.position - objectPosition;
                
                Vector2 offset = Ctx.Config.BackStepSettings.backStepOffset;
                
                _currentTarget += new Vector2(Random.Range(-offset.x, offset.x), Random.Range(-offset.y, offset.y));
                _currentTarget = _currentTarget.normalized;
            }

            if (!Ctx.DetectedPlayerService.CheckTargetPositionOnObstacle(_currentTarget))
            {
                Ctx.Fsm.ChangeState<PathfinderMove>(); //В будущем заменить на BackstepPathfinderMove после добавления
                return;
            }
            
            BaseMove(_currentTarget, Ctx.Config.BackStepSettings.backstepSpeed);
        }
    }
}