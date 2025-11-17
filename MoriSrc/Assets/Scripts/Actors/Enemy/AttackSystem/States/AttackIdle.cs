using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Actors.Enemy.Movement.Base.Service;
using Actors.Enemy.Stats.Scripts.TakeDamageSystem;
using FiniteStateMachine;

namespace Actors.Enemy.AttackSystem.States
{
    public class AttackIdle : FsmState, IObserver<HealthStates>
    {
        private AttackEnemyFsm _fsm;
        private List<IEnemyAttack> _attacks;
        private bool _interrupted;
        
        public AttackIdle(AttackEnemyFsm fsm, List<IEnemyAttack> attacks)
        {
            _fsm = fsm;
            _attacks = attacks;
        }

        public override void Enter()
        {
            _interrupted = false;
        }
        public override void Update()
        {
            if (_interrupted)
                return;
            
            if (TryHit())
            {
                _fsm.Attack();
            }
        }
        #region Observer

        public void OnNext(HealthStates value)
        {
            switch (value)
            {
                case HealthStates.Damage:
                    Interrupt();
                    break;
                case HealthStates.Died:
                    _fsm.OnDie();
                    break;
            }
        }
        public void OnCompleted()
        {
            
        }
        public void OnError(Exception error)
        {
            throw error;
        }

        #endregion
        private void Interrupt()
        {
            _interrupted = true;
            _fsm.Interrupt();
        }

        private bool TryHit()
        {
            foreach (var attack in _attacks)
            {
                if (attack.CurrentCooldown <= 0 && attack.CheckRadius())
                    return true;
            }

            return false;
        }
    }
}