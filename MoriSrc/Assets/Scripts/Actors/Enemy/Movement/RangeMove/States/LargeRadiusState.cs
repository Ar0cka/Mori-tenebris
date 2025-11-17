using System.Collections.Generic;
using Actors.Enemy.Movement.Service;
using Actors.Enemy.Movement.Base.States;
using Actors.Enemy.Movement.Enums;
using Actors.Enemy.Movement.RangeMove;
using ScrObj.EnemyMoveScr;
using Service;
using UnityEngine;

namespace Actors.Enemy.Movement.Base.RangeMove.States
{
    public class LargeRadiusState : RadiusBaseMovement
    {
        protected override AiRadiusEnum StateAiRadius { get; } = AiRadiusEnum.Large;

        public LargeRadiusState(BaseMovementContext<RangeMoveData, EnemyMoveFsm> context,
            RadiusService<RadiusSettings> radiusService) : base(context, radiusService)
        {
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            
            Debug.Log("Large state");
            
            MaintenanceDistance();
        }

        public void MaintenanceDistance()
        {
            Vector2 targetPosition = GetPosFromState(Ctx.Config);

            if (!CheckTargetPosition(targetPosition))
            {
                return;
            }
              
            
            Vector2 direction = VectorMathService.GetForwardVector(targetPosition, Ctx.Rb2D.position);
            Vector2 featurePosition = Ctx.Rb2D.position + direction * Ctx.Config.MoveSettings.speed * Time.fixedDeltaTime;
            
            float distance = Vector2.Distance(targetPosition, featurePosition);
            var radiusDictionary = Ctx.Config.RadiusSettings.RadiusDictionary;

            float distanceWithStopDistance = distance - Ctx.Config.RadiusSettings.largeStopDistance;
            
            Debug.Log("Distance: " + distanceWithStopDistance + "radius: " + radiusDictionary[AiRadiusEnum.Medium]);
            
            if (distanceWithStopDistance <= radiusDictionary[AiRadiusEnum.Medium])
            {
                direction.x = 0;
            }
            
            BaseMove(direction, Ctx.Config.MoveSettings.speed);
        }
    }
}