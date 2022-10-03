using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    #region State Management System
    PlayerBaseState currentState;
    public PlayerIdleState IdleState = new PlayerIdleState();
    public PlayerMovementState MovementState = new PlayerMovementState();
    public PlayerAttackState AttackState = new PlayerAttackState();
    public PlayerDeathState DeathState = new PlayerDeathState();
    public PlayerDamagedState DamageState = new PlayerDamagedState();

    void Start()
    {
        currentState = IdleState;
        //"(this)" is a reference to the player context
        currentState.EnterState(this);
    }

    void Update()
    {
        currentState.UpdateState(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy1 Sword")
        {
            currentState.OnTriggerState(this);
        }
        if (other.gameObject.tag == "Enemy2 Sword")
        {
            currentState.OnTriggerState(this);
        }
        if (other.gameObject.tag == "Enemy3 Dagger")
        {
            currentState.OnTriggerState(this);
        }
        if (other.gameObject.tag == "Enemy4 Sword")
        {
            currentState.OnTriggerState(this);
        }
        if (other.gameObject.tag == "Barrel")
        {
            currentState.OnTriggerState(this);
        }
        if (other.gameObject.tag == "Spikes")
        {
            currentState.OnTriggerState(this);
        }
    }

    public void SwitchState(PlayerBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }
    #endregion
}
