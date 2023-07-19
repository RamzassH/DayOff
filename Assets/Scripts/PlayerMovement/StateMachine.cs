using System.Collections.Generic;
using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class StateMachine
{
    private Dictionary<Type, State> _states = new Dictionary<Type, State>();

    private State _currentState { get; set; }


    public void AddState(State state)
    {
        _states.Add(state.GetType(), state);
    }

    public void SetState<T>() where T : State
    {
        var type = typeof(T);

        if (_currentState != null && _currentState.GetType() == type)
        {
            return;
        }

        if (_states.TryGetValue(type, out var newState))
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter();
        }
    }

    public void Update()
    {
        _currentState?.Update();
    }

    public void FixedUpdate()
    {
        _currentState?.FixedUpdate();
    }

    public State GetCurrentState()
    {
        return _currentState;
    }
}