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
        _ctx.Animator.SetFloat("Speed", 0);
        _ctx.MoveDirection = Vector3.zero;
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
        _ctx.MoveX = Input.GetAxis("Horizontal");
        _ctx.MoveZ = Input.GetAxis("Vertical");

        _ctx.MoveDirection = new Vector3(_ctx.MoveX, 0, _ctx.MoveZ);

        if (_ctx.MoveDirection != Vector3.zero)
        {
            SwitchState(_factory.Walk());
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            SwitchState(_factory.Attack());
        }
        if (_ctx.Health <= 0)
        {
            SwitchState(_factory.Death());
        }
    }
}
