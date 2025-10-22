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
        private RadiusSettings _radiusSettings;

        public LargeRadiusState(EnemyMoveFsm fsm, DataContext<RangeAiMoveFsmRealize, RangeMoveData> dataContext,
            BaseMovementContext baseContext, RadiusService<RadiusSettings> radiusService) : base(fsm, dataContext, baseContext, radiusService)
        {
            _radiusSettings = dataContext.Config.RadiusSettings;
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            
            Debug.Log("Large state");
            
            MaintenanceDistance();
        }

        public void MaintenanceDistance()
        {
            Vector2 targetPosition = GetPosFromState(MoveConfig);

            if (!CheckTargetPosition(targetPosition))
            {
                return;
            }
              
            
            Vector2 direction = VectorMathService.GetForwardVector(targetPosition, Rb2D.position);
            Vector2 featurePosition = Rb2D.position + direction * MoveConfig.MoveSettings.speed * Time.fixedDeltaTime;
            
            float distance = Vector2.Distance(targetPosition, featurePosition);
            var radiusDictionary = MoveConfig.RadiusSettings.RadiusDictionary;

            float distanceWithStopDistance = distance - _radiusSettings.largeStopDistance;
            
            Debug.Log("Distance: " + distanceWithStopDistance + "radius: " + radiusDictionary[AiRadiusEnum.Medium]);
            
            if (distanceWithStopDistance <= radiusDictionary[AiRadiusEnum.Medium])
            {
                direction.x = 0;
            }
            
            BaseMove(direction, MoveConfig.MoveSettings.speed);
        }
    }
}