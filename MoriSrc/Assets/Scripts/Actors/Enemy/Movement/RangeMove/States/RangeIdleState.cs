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

        public RangeIdleState(EnemyMoveFsm moveFsm, DataContext<RangeAiMoveFsmRealize, RangeMoveData> dataContext, 
            BaseMovementContext componentContext, RadiusService<RadiusSettings> radiusService) : base(moveFsm, dataContext, componentContext, radiusService)
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
            var dictionary = MoveConfig.RadiusSettings.RadiusDictionary;

            if (dictionary == null || !dictionary.ContainsKey(StateAiRadius))
                return;
            
            if (DetectedPlayerService.IdleDetection(dictionary[StateAiRadius], MoveConfig.IdleDetectionSettings.fieldOfViewAngle, 
                    MoveConfig.TargetMask, SpriteRenderer, Rb2D.position))
            { 
                Debug.Log($"Detected player");
                ChangeMoveType(StateAiRadius);
            }
        }
    }
}