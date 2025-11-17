using Actors.Enemy.Movement.Service;
using Actors.Enemy.Movement.Base.States;
using Actors.Enemy.Movement.Enums;
using Actors.Enemy.Movement.RangeMove;
using Actors.Enemy.Movement.RangeMove.States;
using ScrObj.EnemyMoveScr;
using UnityEngine;

namespace Actors.Enemy.Movement.Base.RangeMove.States
{
    public abstract class RadiusBaseMovement : BaseMovementState<EnemyMoveFsm, RangeMoveData>
    {
        protected RadiusService<RadiusSettings> RadiusService;
        protected abstract AiRadiusEnum StateAiRadius { get; }

        protected RadiusBaseMovement(BaseMovementContext<RangeMoveData, EnemyMoveFsm> context,
            RadiusService<RadiusSettings> radiusService) : base(context)
        {
            RadiusService = radiusService;
        }

        protected virtual bool CheckTargetPosition(Vector2 targetPosition)
        {
            var radiusType = RadiusService.CheckCirclePosition(targetPosition, Ctx.Rb2D.position);

            if (targetPosition == Vector2.zero)
            {
                ChangeState<RangeReturnToStart>();
                Debug.Log("Return to start pos");
                return false;
            }

            if (radiusType != StateAiRadius)
            {
                ChangeMoveType(radiusType);
                Debug.Log("not needed type, change");
                return false;
            }

            return true;
        }
        
        protected void ChangeMoveType(AiRadiusEnum status)
        {
            switch (status)
            {
                case AiRadiusEnum.Large:
                    ChangeState<LargeRadiusState>();
                    Debug.Log("Change on large");
                    break;
                case AiRadiusEnum.Medium:
                    ChangeState<MediumRadiusState>();
                    Debug.Log("Change on medium");
                    break;
                case AiRadiusEnum.Small:
                    ChangeState<SmallRadiusState>();
                    Debug.Log("Change on small");
                    break;
                default:
                    ChangeState<RangeReturnToStart>();
                    break;
            }
        }

        protected override void BaseMove(Vector2 targetPos, float speed)
        {
            CurrentVelocity = targetPos * speed * Time.deltaTime;
            Ctx.Rb2D.MovePosition(Ctx.Rb2D.position + CurrentVelocity);
            SetSpriteSide();
        }

        protected Vector2 GetPosFromState(RangeMoveData data)
        {
            var radiusDictionary = data.RadiusSettings.RadiusDictionary;

            if (radiusDictionary != null && radiusDictionary.ContainsKey(StateAiRadius))
            {
                return DetectedPlayer(radiusDictionary[StateAiRadius], data.TargetMask);
            }
            
            return Vector2.zero;
        }
    }
}