using Actors.Enemy.Movement.Base;
using Actors.Enemy.Movement.Enums;
using Actors.Enemy.Movement.Base.RangeMove.States;
using Actors.Enemy.Movement.Base.Service;
using Actors.Enemy.Movement.Service;
using ScrObj.EnemyMoveScr;
using Service;
using UnityEngine;

namespace Actors.Enemy.Movement.RangeMove.States
{
    public class MediumRadiusState : RadiusBaseMovement
    {
        private readonly RangeMoveData _rangeMoveData;

        protected override AiRadiusEnum StateAiRadius { get; } = AiRadiusEnum.Medium;

        public MediumRadiusState(EnemyMoveFsm fsm, 
            DataContext<RangeAiMoveFsmRealize, RangeMoveData> dataContext, 
            BaseMovementContext baseMovementContext, RadiusService<RadiusSettings> radiusService) : base(fsm, dataContext, baseMovementContext, radiusService)
        {
            _rangeMoveData = dataContext.Config;
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            Backstep();
        }

        private void Backstep()
        {
            var targetPosition = DetectedPlayer(_rangeMoveData);

            if (!CheckTargetPosition(targetPosition, StateAiRadius))
                return;

            var direction =
                VectorMathService.GetBackstepVector(targetPosition, Rb2D.position, _rangeMoveData.BackStepSettings);
            
            BaseMove(direction, _rangeMoveData.MoveSettings.speed);
        }
    }
}