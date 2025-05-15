using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    public EnemyState currentState { get; private set; }
    public string currentStateName { get { return currentState?.stateName; } } // Store the name if needed

    public void Initialize(EnemyState _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(EnemyState _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }

}
