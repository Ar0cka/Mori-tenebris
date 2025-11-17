using Actors.Enemy.Movement.Base;
using Actors.Enemy.Movement.Base.RangeMove.States;
using Actors.Enemy.Movement.Enums;
using Actors.Enemy.Movement.Service;
using ScrObj.EnemyMoveScr;
using Service;
using UnityEngine;

namespace Actors.Enemy.Movement.RangeMove.States
{
    public class SmallRadiusState : RadiusBaseMovement
    {
        protected override AiRadiusEnum StateAiRadius { get; } =  AiRadiusEnum.Small;
        
        public SmallRadiusState(BaseMovementContext<RangeMoveData, EnemyMoveFsm> context,
            RadiusService<RadiusSettings> radiusService) : base(context, radiusService)
        {
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            
            PursuitPlayer();
        }
        private void PursuitPlayer()
        {
            Vector2 targetPosition = GetPosFromState(Ctx.Config);
            
            if (!CheckTargetPosition(targetPosition) || CheckDistance(targetPosition, Ctx.Rb2D.position, Ctx.Config.AggressiveSettings.stopDistance))
                return;

            Vector2 direction = VectorMathService.GetForwardVector(targetPosition, Ctx.Rb2D.position);
            
            BaseMove(direction, Ctx.Config.MoveSettings.speed);
        }
    }
}