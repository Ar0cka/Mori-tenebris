using Actors.Enemy.Movement.Base;
using ScrObj.EnemyMoveScr;
using UnityEngine;
using UnityEngine.Rendering;

namespace Actors.Enemy.Movement.Base.States
{
    public class IdleMoveState : MoveEnemyFsmUnityState
    {
        protected MoveData MoveData;
        
        public IdleMoveState(EnemyMoveFsm fsm, MoveData moveData, BaseMovementContext context) : base(fsm, context)
        {
            MoveData = moveData;
        }

        public override void Update()
        {
            bool isDetected = IdleDetected(MoveData);

            if (isDetected)
            {
                StateMachine.ChangeState<PursuitPlayer>();
            }
        }
    }
}