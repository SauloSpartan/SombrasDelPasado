using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactor) : base (currentContext, playerStateFactor)
    {

    }

    public override void EnterState()
    {
        _newContext.Animator.SetFloat("Speed", 0);

        Debug.Log("Hello from Idle");
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }

    public override void ExitState()
    {

    }

    public override void CheckSwitchState()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        _newContext.MoveDirection = new Vector3(moveX, 0, moveZ);

        if (_newContext.MoveDirection != Vector3.zero)
        {
            SwitchState(_factory.Walk());
        }
    }
}
