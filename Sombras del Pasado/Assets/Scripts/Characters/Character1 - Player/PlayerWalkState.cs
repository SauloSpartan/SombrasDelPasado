using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactor) : base(currentContext, playerStateFactor)
    {

    }

    public override void EnterState()
    {
        _newContext.Animator.SetFloat("Speed", 1);

        Debug.Log("Hello from Walk");
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

        if (_newContext.MoveDirection == Vector3.zero)
        {
            SwitchState(_factory.Idle());
        }
    }
}
