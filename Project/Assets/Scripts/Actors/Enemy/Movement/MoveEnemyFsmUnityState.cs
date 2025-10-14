using Actors.Enemy.Movement.States;
using FiniteStateMachine;
using ScrObj.EnemyMoveScr;

namespace Actors.Enemy.Movement
{
    public class MoveEnemyFsmUnityState : FsmUnityState<EnemyMoveFsm, MoveEnemyFsmUnityState>
    {
        public MoveEnemyFsmUnityState(EnemyMoveFsm fsm) : base(fsm)
        {
            
        }

        protected void ChooseIdleState(MoveSettings moveSettings)
        {
            if (moveSettings.hasPatrol)
                StateMachine.ChangeState<PatrolMoveState>();
            else
                StateMachine.ChangeState<IdleMoveState>();
        }
    }
}