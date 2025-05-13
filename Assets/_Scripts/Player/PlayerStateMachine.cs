using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState currentState { get; private set; }
    public PlayerState previousState { get; private set; } // Store the actual state object
    public string previousStateName { get; private set; } // Store the name if needed\
    public string currentStateName { get { return currentState?.stateName; } } // Store the name if needed

    public void Initialize(PlayerState startingState)
    {
        currentState = startingState;
        currentState.Enter();
    }

    public void ChangeState(PlayerState newState)
    {
        // Store previous state before changing
        previousState = currentState;
        previousStateName = currentState?.GetType().Name;
        
        // Exit current state
        currentState.Exit();
        
        // Change to new state
        currentState = newState;
        currentState.Enter();
    }
}

