using Actors.Enemy.Movement;
using ScrObj.EnemyMoveScr;
using UnityEngine;
using UnityEngine.Rendering;

namespace Actors.Enemy.Movement.States
{
    public class IdleMoveState : MoveEnemyFsmUnityState
    {
        public IdleMoveState(EnemyMoveFsm fsm, BaseMovementContext context) : base(fsm, context)
        {
            
        }

        public override void Update()
        {
            bool isDetected = IdleDetected();

            if (isDetected)
            {
                StateMachine.ChangeState<PursuitPlayer>();
            }
        }
    }
}