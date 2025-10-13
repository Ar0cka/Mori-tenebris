using System;
using System.Collections.Generic;

namespace FiniteStateMachine
{
    public abstract class FsmAbstract<TFsm, TState>
    where TFsm : FsmAbstract<TFsm, TState>
    where TState : StateAbstract<TFsm, TState>
    {
        private TState _currentState;
        private Dictionary<Type, TState> _states = new();
        
        public TState CurrentState => _currentState;

        public void AddState(TState state)
        {
            _states.Add(state.GetType(), state);
        }

        public void ChangeState<T>() where T : TState
        {
            if (_currentState is not null)
                _currentState.Exit();

            var type = typeof(T);
            if (_states.TryGetValue(type, out var nextState))
            {
                _currentState = nextState;
                _currentState.Enter();
            }
            else
            {
                throw new Exception($"State {type.Name} not found in FSM.");
            }
        }
        
        public void Update()
        {
            _currentState?.Update();
        }

        public void PhysicsUpdate()
        {
            _currentState?.PhysicsUpdate();
        }
    }
}