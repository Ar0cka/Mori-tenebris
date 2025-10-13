# Finite State Machine — Generic Architecture (CRTP)

This document explains how the generic-based FSM (Finite State Machine) is structured using mutually recursive generic types — a common design known as the Curiously Recurring Template Pattern (CRTP).  
The goal of this pattern is to achieve strong type-safety between the FSM and its states without infinite recursion or runtime casting.

## Concept Overview

Normally, you want each State to know which FSM it belongs to, and each FSM to know which type of states it manages.  
However, this mutual relationship can lead to recursive generic definitions that never terminate.  

```C#
public abstract class FsmAbstract<TState>
    where TState : StateAbstract<FsmAbstract<TState>>
```


> This structure is invalid because:
>
> - `FsmAbstract` depends on `StateAbstract`  
> - `StateAbstract` depends on `FsmAbstract`  
> - …and the compiler ends up chasing this relationship forever.

To solve this, we use two generic parameters and cross-reference them in their constraints.
This way, the recursion is stopped at the first level and type relationships remain strict.

---


**Generic Parameters Explained**
| Symbol   | Defined In                      | Description                                                                   |
| :------- | :------------------------------ | :---------------------------------------------------------------------------- |
| `TFsm`   | `FsmAbstract` & `StateAbstract` | Represents the concrete FSM type (for example, `PlayerFsm`).                  |
| `TState` | `FsmAbstract` & `StateAbstract` | Represents the base state type used by this FSM (for example, `PlayerState`). |

## Why Two Generics?

1. `FsmAbstract<TFsm, TState>` defines what states it can manage.  
2. `StateAbstract<TFsm, TState>` defines which FSM it belongs to.  
3. Their constraints reference each other:  
   - `TFsm : FsmAbstract<TFsm, TState>`  
   - `TState : StateAbstract<TFsm, TState>`

This mirrored design ensures the compiler enforces:

- Any state must belong to a compatible FSM.  
- Any FSM only works with states tied to itself.

No runtime checks or reflection are needed, since both sides know each other’s structure at compile time.

---

## How to Use
1. **Define Base FSM and State**  
   Base classes contain shared logic and lifecycle methods (`Enter`, `Update`, `Exit`, etc.).
```C#
public abstract class StateAbstract<TFsm, TState> 
        where TState : StateAbstract<TFsm, TState>
        where TFsm : FsmAbstract<TFsm, TState>
    {
        protected TFsm StateMachine;
        
        public StateAbstract(TFsm fsm)
        {
            StateMachine = fsm;
        }
        
        public virtual void Enter() { }
        
        public virtual void Update() { }
        
        public virtual void PhysicsUpdate() { }
        
        public virtual void Exit() { }
    }

  -----------------------------------------------
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

```
Finite State Machine — Generic Architecture (continued)

2. **Create Concrete Types**  
   - FSM class inherits from `FsmAbstract<TFsm, TState>`.  
   - Base state class inherits from `StateAbstract<TFsm, TState>`.  
   - Each specific state (`Idle`, `Move`, `Attack`, etc.) derives from that base state.

# Benefits of this Design

## ✅ Type-safe FSM–State relationship
Each state can only work with its corresponding FSM.  
You can’t accidentally assign a `PlayerState` to an `EnemyFsm`.

## ✅ No runtime casts or reflection
The compiler already knows the exact types, so no type conversions are needed.

## ✅ Extremely flexible and extensible
You can reuse the base classes for multiple entities — e.g. `PlayerFsm`, `EnemyFsm`, `BossFsm`, each with their own state families.

## ✅ Prevents infinite recursion
By using two generics and cross-referencing them, the compiler resolves all dependencies at compile time instead of nesting them infinitely.

---

## 💡 Typical Usage Example

1. Create a `PlayerFsm` derived from `FsmAbstract<PlayerFsm, PlayerState>`.
2. Create several concrete states (`Idle`, `Move`, `Attack`, etc.) inheriting from `PlayerState`.
3. Switch between states by calling `ChangeState(new IdleState(this))`.

The compiler ensures that:

- Only `PlayerState` instances can be used with `PlayerFsm`.
- All states correctly reference their parent FSM type.

---

## 🧭 Summary

| Concept | Purpose |
|---------|---------|
| CRTP pattern | Binds derived and base classes via generics for type safety. |
| `TFsm` | The concrete FSM type that manages a family of states. |
| `TState` | The base state type for that FSM. |
| Cross-referenced generics | Prevent recursion while preserving full type awareness. |

