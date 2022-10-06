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
        Debug.Log("Hello from Idle");
    }

    public override void UpdateState()
    {

    }

    public override void ExitState()
    {

    }

    public override void CheckSwitchState()
    {

    }
}
