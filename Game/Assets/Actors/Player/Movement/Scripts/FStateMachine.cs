using System;
using System.Collections.Generic;

public class FStateMachine
{
    public State CurrentState { get; private set; }
    private Dictionary<Type, State> _states = new Dictionary<Type, State>();
    
    public void AddNewState(State state)
    {
        _states.Add(state.GetType(), state);
    }

    public void ChangeState<T>() where T : State
    {
        var type = typeof(T);

        if (CurrentState?.GetType() == type) return;

        if (_states.TryGetValue(type, out var state))
        {
            CurrentState?.Exit();
            CurrentState = state;
            state.Enter();
        }
    }

    public void Update()
    {
        CurrentState.UpdateLogic();
    }

    public void FixedUpdate()
    {
        CurrentState.PhysicUpdate();
    }
}