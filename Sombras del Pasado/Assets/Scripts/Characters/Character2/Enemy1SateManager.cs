using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1SateManager : MonoBehaviour
{
    #region State Management system
    Enemy1BaseState currentState;
    public Enemy1MovementState MovementState = new Enemy1MovementState();
    public Enemy1AttackState AttackState = new Enemy1AttackState();
    public Enemy1DeathState DeathState = new Enemy1DeathState();

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

    public void SwitchState(Enemy1BaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }
    #endregion
}
