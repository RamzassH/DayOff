using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    
    private State _currentState = null;

    StateMachine(State startState) {
        _currentState = startState;
        _currentState.Enter();
    }

    public void ChangeState(State newState) { 
        _currentState.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    public State CurrentState { get 
        { return _currentState; } 
    }
    
}
