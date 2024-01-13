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
        if (_states.TryGetValue(type, out var newState))
        {
            if (newState == _currentState)
            {
                return;
            }
            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter();
        }
    }

    public void Awake()
    {
        _currentState?.Awake();
    }
    
    public void Update()
    {
        _currentState?.Update();
    }

    public void FixedUpdate()
    {
        _currentState?.FixedUpdate();
    }

    public void LateUpdate()
    {
        _currentState?.LateUpdate();
    }
    
    public State GetCurrentState()
    {
        return _currentState;
    }
    
}