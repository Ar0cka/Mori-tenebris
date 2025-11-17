using Actors.Enemy.AttackSystem.States;
using FiniteStateMachine;

namespace Actors.Enemy.AttackSystem
{
    public class AttackEnemyFsm : FsmUnityBase
    {
        public void Interrupt() => ChangeState<Interrupt>();
        public void Attack() => ChangeState<AttackState>();
        public void Idle() => ChangeState<AttackIdle>();

        private bool _isDie;

        public void OnDie()
        {
            _isDie = true;
        }

        public override void Update()
        {
            if (_isDie)
                return;
            
            base.Update();
        }

        public override void PhysicsUpdate()
        {
            if (_isDie)
                return;
            
            base.PhysicsUpdate();
        }
    }
}