using FiniteStateMachine;

namespace Actors.Enemy.Movement
{
    public class MoveEnemyFsmUnityState : FsmUnityState<EnemyMoveFsm, MoveEnemyFsmUnityState>
    {
        public MoveEnemyFsmUnityState(EnemyMoveFsm fsm) : base(fsm)
        {
            
        }
    }
}