using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageState : PlayerBaseState
{
    public PlayerDamageState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactor) : base(currentContext, playerStateFactor)
    {

    }

    public override void EnterState()
    {
        _ctx.Animator.SetTrigger("Damage");
        _ctx.CanMove = false;
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
        if (_ctx.CanMove == true) // CanMove is controlled in player events
        {
            SwitchState(_factory.Idle());
        }
        if (_ctx.Health <= 0)
        {
            SwitchState(_factory.Death());
        }
    }
}