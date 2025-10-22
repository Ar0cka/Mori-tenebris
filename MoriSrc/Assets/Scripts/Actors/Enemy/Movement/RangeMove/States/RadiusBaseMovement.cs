using Actors.Enemy.Movement.Service;
using Actors.Enemy.Movement.Base.States;
using Actors.Enemy.Movement.Enums;
using Actors.Enemy.Movement.RangeMove;
using Actors.Enemy.Movement.RangeMove.States;
using ScrObj.EnemyMoveScr;
using UnityEngine;

namespace Actors.Enemy.Movement.Base.RangeMove.States
{
    public abstract class RadiusBaseMovement : BaseMovementState<RangeAiMoveFsmRealize, RangeMoveData>
    {
        protected RadiusService<RadiusSettings> RadiusService;
        protected abstract AiRadiusEnum StateAiRadius { get; }

        public RadiusBaseMovement(EnemyMoveFsm fsm, DataContext<RangeAiMoveFsmRealize,
                RangeMoveData> dataContext, BaseMovementContext baseMovementContext,
            RadiusService<RadiusSettings> radiusService) : base(fsm, dataContext,
            baseMovementContext)
        {
            RadiusService = radiusService;
        }

        protected virtual bool CheckTargetPosition(Vector2 targetPosition, AiRadiusEnum stateRadius)
        {
            var radiusType = RadiusService.CheckCirclePosition(targetPosition, Rb2D.position);

            if (targetPosition == Vector2.zero)
            {
                ChooseIdleState(MoveConfig.MoveSettings);
                return false;
            }

            if (radiusType != StateAiRadius)
            {
                ChangeMoveType(radiusType);
                return false;
            }

            return true;
        }
        
        protected void ChangeMoveType(AiRadiusEnum status)
        {
            switch (status)
            {
                case AiRadiusEnum.Large:
                    StateMachine.ChangeState<LargeRadiusState>();
                    break;
                case AiRadiusEnum.Medium:
                    StateMachine.ChangeState<MediumRadiusState>();
                    break;
                case AiRadiusEnum.Small:
                    StateMachine.ChangeState<PursuitPlayer>();
                    break;
                default:
                    StateMachine.ChangeState<IdleMoveState>();
                    break;
            }
        }

        protected override void BaseMove(Vector2 targetPos, float speed)
        {
            CurrentVelocity = targetPos * speed * Time.deltaTime;
            Rb2D.MovePosition(Rb2D.position + CurrentVelocity);
            SetSpriteSide();
        }
    }
}