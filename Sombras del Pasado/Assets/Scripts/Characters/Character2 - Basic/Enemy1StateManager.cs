using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1StateManager : MonoBehaviour
{
    #region State Management System
    Enemy1BaseState currentState;
    public Enemy1MovementState MovementState = new Enemy1MovementState();
    public Enemy1AttackState AttackState = new Enemy1AttackState();
    public Enemy1DeathState DeathState = new Enemy1DeathState();
    public Enemy1DamagedState DamageState = new Enemy1DamagedState();

    void Start()
    {
        currentState = MovementState;
        //"this" is a reference to the Enemy1 context
        currentState.EnterState(this);
    }

    void Update()
    {
        currentState.UpdateState(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        currentState.OnTriggerState(this, other);
    }

    public void SwitchState(Enemy1BaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }
    #endregion
}
