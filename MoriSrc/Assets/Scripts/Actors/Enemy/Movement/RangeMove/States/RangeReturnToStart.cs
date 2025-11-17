using UnityEngine;
using Actors.Enemy.Movement.Base;
using Actors.Enemy.Movement.Base.States;
using Actors.Enemy.Movement.Enums;
using ScrObj.EnemyMoveScr;

namespace Actors.Enemy.Movement.RangeMove.States
{
    public class RangeReturnToStart : ReturnToStartPosition
    {
        private RadiusSettings _radiusSettings;
        private const AiRadiusEnum MediumState = AiRadiusEnum.Medium;
        
        public RangeReturnToStart(BaseMovementContext<MoveData, EnemyMoveFsm> context, Vector2 startPosition, RadiusSettings radiusSettings) : 
            base(context, startPosition)
        {
            _radiusSettings = radiusSettings;
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            
            Debug.Log("Range return to start position");
        }

        protected override void IdleState() => ChangeState<RangeIdleState>();
        protected override void PursuitState() => ChangeState<MediumRadiusState>();
        protected override bool CheckPlayer()
        {
            var dictionary = _radiusSettings.RadiusDictionary;

            if (Ctx.DetectedPlayerService.IdleDetection(dictionary[MediumState],
                    Ctx.Config.IdleDetectionSettings.fieldOfViewAngle, Ctx.Config.TargetMask, Ctx.SpriteRenderer,
                    Ctx.Rb2D.position))
            {
                PursuitState();
                return false;
            }

            return true;
        }
    }
}