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
        protected override AiRadiusEnum StateAiRadius { get; } = AiRadiusEnum.Medium;

        public MediumRadiusState(BaseMovementContext<RangeMoveData, EnemyMoveFsm> context,
            RadiusService<RadiusSettings> radiusService) : base(context, radiusService)
        {
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            Backstep();
        }

        private void Backstep()
        {
            var targetPosition = GetPosFromState(Ctx.Config);

            if (!CheckTargetPosition(targetPosition))
                return;

            var direction =
                VectorMathService.GetBackstepVector(targetPosition, Ctx.Rb2D.position, Ctx.Config.BackStepSettings);
            
            BaseMove(direction, Ctx.Config.MoveSettings.speed);
        }
    }
}