using Actors.Enemy.Monsters.AbstractEnemy;
using FiniteStateMachine;

namespace Actors.Enemy.AttackSystem.States
{
    public class Interrupt : FsmState
    {
        private AttackEnemyFsm _attackEnemyFsm;
        private StateController _stateController;
        
        public Interrupt(AttackEnemyFsm fsm, StateController stateController)
        {
            _attackEnemyFsm = fsm;
            _stateController = stateController;
        }
        
        
    }
}