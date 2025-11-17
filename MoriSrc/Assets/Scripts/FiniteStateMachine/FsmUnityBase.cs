using System;
using System.Collections.Generic;

namespace FiniteStateMachine
{
    public abstract class FsmUnityBase
    {
        private FsmState _currentState;
        private Dictionary<Type, FsmState> _states = new();
        
        public FsmState CurrentState => _currentState;

        public void AddState(FsmState state)
        {
            _states.Add(state.GetType(), state);
        }

        public void ChangeState<T>() where T : FsmState
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
        
        public virtual void Update()
        {
            _currentState?.Update();
        }

        public virtual void PhysicsUpdate()
        {
            _currentState?.PhysicsUpdate();
        }
    }
}