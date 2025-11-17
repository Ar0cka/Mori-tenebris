using Actors.Enemy.Movement.Base;
using Actors.Enemy.Movement.Base.RangeMove.States;
using Actors.Enemy.Movement.Enums;
using Actors.Enemy.Movement.Service;
using ScrObj.EnemyMoveScr;
using UnityEngine;

namespace Actors.Enemy.Movement.RangeMove.States
{
    public class RangeIdleState : RadiusBaseMovement
    {
        protected override AiRadiusEnum StateAiRadius { get; } = AiRadiusEnum.Medium;

        public RangeIdleState(BaseMovementContext<RangeMoveData, EnemyMoveFsm> context,
            RadiusService<RadiusSettings> radiusService) : base(context, radiusService)
        {
        }

        public override void Enter()
        {
            Debug.Log("Entering RangeIdleState");
        }

        public override void PhysicsUpdate()
        {
            IdleCheck();
        }

        private void IdleCheck()
        {
            var dictionary = Ctx.Config.RadiusSettings.RadiusDictionary;

            if (dictionary.TryGetValue(StateAiRadius, out var state))
            {
                if (Ctx.DetectedPlayerService.IdleDetection(state, Ctx.Config.IdleDetectionSettings.fieldOfViewAngle, 
                        Ctx.Config.TargetMask, Ctx.SpriteRenderer, Ctx.Rb2D.position))
                { 
                    Debug.Log($"Detected player");
                    ChangeMoveType(StateAiRadius);
                }
            }
        }
    }
}