using System;
using System.Collections.Generic;
using Actors.Player.Movement.Scripts;
using StateMachin.States;

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

    public void UpdateStateData<T>(MovementColliderOffset movementColliderOffset) where T : MovementState
    {
        var type = typeof(T);
        if (CurrentState?.GetType() == type)
        {
            if (CurrentState is MovementState movementState)
            {
                movementState.UpdateMovementOffset(movementColliderOffset);
            }
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