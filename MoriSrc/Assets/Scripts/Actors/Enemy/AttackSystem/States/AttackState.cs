using System.Collections.Generic;
using FiniteStateMachine;

namespace Actors.Enemy.AttackSystem.States
{
    public class AttackState : FsmState
    {
        private FsmUnityBase _fsm;
        private List<IEnemyAttack> _enemyAttacks;
        
        public AttackState(FsmUnityBase fsm, List<IEnemyAttack> attacks)
        {
            _enemyAttacks = attacks;
        }

        public override void Enter() //Выбираем доступную атаку, если таковых нет, переходим в Idle.
        {
            base.Enter();
        }

        public override void Update() //Проверяем на Interrupt и ждем окончание атаки
        {
            base.Update();
        }

        public override void Exit() //Выходим из состояния атаки в Idle
        {
            base.Exit();
        }
    }
}