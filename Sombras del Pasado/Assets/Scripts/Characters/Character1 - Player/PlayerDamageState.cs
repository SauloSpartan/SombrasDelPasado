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
        _ctx.DamagedCount++;
        _ctx.GeneralTimer = 0.5f;
        _ctx.StartCoroutine(Invulnerability());
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

    private IEnumerator Invulnerability()
    {
        if (_ctx.DamagedCount >= 3)
        {
            _ctx.IsInvulnerable = true;

            for (int i = 0; i < 6; i++)
            {
                _ctx.MainMaterial.EnableKeyword("_EMISSION");
                yield return new WaitForSecondsRealtime(_ctx.GeneralTimer);

                _ctx.MainMaterial.DisableKeyword("_EMISSION");
                yield return new WaitForSecondsRealtime(_ctx.GeneralTimer);
            }

            _ctx.DamagedCount = 0;
            _ctx.IsInvulnerable = false;
        }
    }
}