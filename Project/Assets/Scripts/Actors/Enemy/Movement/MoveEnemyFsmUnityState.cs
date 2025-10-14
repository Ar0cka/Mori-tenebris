using FiniteStateMachine;

namespace Actors.Enemy.Movement.MovementFsm
{
    public class MoveEnemyFsmUnityState : FsmUnityState<EnemyMoveFsm, MoveEnemyFsmUnityState>
    {
        public MoveEnemyFsmUnityState(EnemyMoveFsm fsm) : base(fsm)
        {
            
        }
    }
}