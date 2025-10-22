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

        private RangeMoveData _rangeData;
        
        public SmallRadiusState(EnemyMoveFsm fsm, DataContext<RangeAiMoveFsmRealize, RangeMoveData> dataContext,
            BaseMovementContext baseMovementContext, RadiusService<RadiusSettings> radiusService) : base(fsm,
            dataContext, baseMovementContext, radiusService)
        {
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            
            PursuitPlayer();
        }
        private void PursuitPlayer()
        {
            Vector2 targetPosition = GetPosFromState(MoveConfig);
            
            if (!CheckTargetPosition(targetPosition))
                return;

            Vector2 direction = VectorMathService.GetForwardVector(targetPosition, Rb2D.position);
            
            BaseMove(direction, MoveConfig.MoveSettings.speed);
        }
    }
}