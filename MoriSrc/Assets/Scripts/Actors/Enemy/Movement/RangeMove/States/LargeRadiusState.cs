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
        private RangeMoveData _rangeData;
        protected override AiRadiusEnum StateAiRadius { get; } = AiRadiusEnum.Large;

        public LargeRadiusState(EnemyMoveFsm fsm, DataContext<RangeAiMoveFsmRealize, RangeMoveData> dataContext,
            BaseMovementContext baseContext, RadiusService<RadiusSettings> radiusService) : base(fsm, dataContext, baseContext, radiusService)
        {
            _rangeData = dataContext.Config;
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            
            MaintenanceDistance();
        }

        public void MaintenanceDistance()
        {
            Vector2 targetPosition = DetectedPlayer(_rangeData);

            if (CheckTargetPosition(targetPosition, StateAiRadius))
                return;

            
            Vector2 direction = VectorMathService.GetForwardVector(targetPosition, Rb2D.position);
            Vector2 featurePosition = Rb2D.position + direction * MoveConfig.MoveSettings.speed * Time.fixedDeltaTime;
            
            float distance = Vector2.Distance(targetPosition, featurePosition);
            var radiusDictionary = _rangeData.RadiusSettings.RadiusDictionary;

            if (distance <= radiusDictionary[AiRadiusEnum.Medium])
            {
                direction.x = 0;
            }
            
            BaseMove(direction, MoveConfig.MoveSettings.speed);
        }
    }
}