using Actors.Enemy.Movement;
using ScrObj.EnemyMoveScr;
using UnityEngine;
using UnityEngine.Rendering;

namespace Actors.Enemy.Movement.States
{
    public class IdleMoveState : MoveEnemyFsmUnityState
    {
        private EnemyMoveFsmRealize _fsmRealize;
        
        public IdleMoveState(EnemyMoveFsm fsm, EnemyMoveFsmRealize moveFsmRealize) : base(fsm)
        {
            _fsmRealize = moveFsmRealize;
        }

        public override void Update()
        {
            bool isDetected = _fsmRealize.DetectTarget();

            if (isDetected)
            {
                StateMachine.ChangeState<PursuitPlayer>();
            }
        }

        public override void Exit()
        {
            _fsmRealize.ChangeViewState(true);
        }
    }
}