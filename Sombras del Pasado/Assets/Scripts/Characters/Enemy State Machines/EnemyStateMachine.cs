using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is the CONTEXT and stores the data that the concrete states need to be performed
public class EnemyStateMachine : MonoBehaviour
{
    // State variables
    private EnemyBaseState _currentState;
    private EnemyStateFactory _states;

    // Getters and Setters
    /// <value> Reference to BaseState Script. </value>
    public EnemyBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }

    void Start()
    {
        // Setup state
        _states = new EnemyStateFactory(this); // "(this)" is a PlayerStateMachine instance
        _currentState = _states.Idle();
        _currentState.EnterState();
    }

    // Update is called once per frame
    void Update()
    {
        _currentState.UpdateState();
    }
}