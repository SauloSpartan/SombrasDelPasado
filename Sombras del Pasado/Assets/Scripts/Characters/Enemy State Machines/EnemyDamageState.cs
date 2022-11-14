using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageState : EnemyBaseState
{
    public EnemyDamageState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactor) : base(currentContext, enemyStateFactor)
    {

    }

    public override void EnterState()
    {
        _ctx.Animator.SetTrigger("Damage");
        _ctx.NavMesh.isStopped = true;
        _ctx.CanMove = false;

        if (_ctx.TrailSwordOne != null)
        {
            _ctx.TrailSwordOne.SetActive(false);
        }
        if (_ctx.TrailSwordTwo != null)
        {
            _ctx.TrailSwordTwo.SetActive(false);
        }
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
        if (_ctx.CanMove == true) // CanMove is controlled in enemy events
        {
            SwitchState(_factory.Walk());
        }
        if (_ctx.Health <= 0)
        {
            SwitchState(_factory.Death());
        }
    }
}