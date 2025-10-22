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
        
        public RangeReturnToStart(EnemyMoveFsm fsm, DataContext<EnemyMoveFsmRealize, MoveData> dataContext,
            BaseMovementContext context, Vector2 startPosition, RadiusSettings radiusSettings) : 
            base(fsm, dataContext, context, startPosition)
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

            if (DetectedPlayerService.IdleDetection(dictionary[MediumState],
                    MoveConfig.IdleDetectionSettings.fieldOfViewAngle, MoveConfig.TargetMask, SpriteRenderer,
                    Rb2D.position))
            {
                PursuitState();
                return false;
            }

            return true;
        }
    }
}