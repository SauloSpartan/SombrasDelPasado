using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    public PlayerAttackState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactor) : base(currentContext, playerStateFactor)
    {

    }

    public override void EnterState()
    {
        _ctx.Animator.SetInteger("AttackCombo", 1);
        _ctx.Damage = 3 * _ctx.Attack;
        _ctx.CanMove = false;
        _ctx.MoveDirection = Vector3.zero;
    }

    public override void UpdateState()
    {
        Attack();
        StopInvulnerability();
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

    /// <summary>
    /// Function that controls the attack and combos.
    /// </summary>
    private void Attack()
    {
        if (_ctx.AttackCombo == 1 && Input.GetKeyDown(KeyCode.J))
        {
            _ctx.Animator.SetInteger("AttackCombo", 1);
            _ctx.Damage = 3 * _ctx.Attack;
        }
        if ((_ctx.AttackCombo == 2 || _ctx.AttackCombo == 4) && Input.GetKeyDown(KeyCode.J))
        {
            _ctx.Animator.SetInteger("AttackCombo", 2);
            _ctx.Damage = 3 * _ctx.Attack;
        }
        if ((_ctx.AttackCombo == 3 || _ctx.AttackCombo == 5) && Input.GetKeyDown(KeyCode.J))
        {
            _ctx.Animator.SetInteger("AttackCombo", 3);
            _ctx.Damage = 5 * _ctx.Attack;
        }

        // Attacks with K
        if (_ctx.AttackCombo == 2 && Input.GetKeyDown(KeyCode.K))
        {
            _ctx.Animator.SetInteger("AttackCombo", 4);
            _ctx.Damage = 7 * _ctx.Attack;
        }
        if (_ctx.AttackCombo == 3 && Input.GetKeyDown(KeyCode.K))
        {
            _ctx.Animator.SetInteger("AttackCombo", 5);
            _ctx.Damage = 7 * _ctx.Attack;
        }
        if (_ctx.AttackCombo == 4 && Input.GetKeyDown(KeyCode.K))
        {
            _ctx.Animator.SetInteger("AttackCombo", 6);
            _ctx.Damage = 10 * _ctx.Attack;
        }
    }

    private void StopInvulnerability()
    {
        _ctx.GeneralTimer = 0;
        _ctx.IsInvulnerable = false;
    }
}
