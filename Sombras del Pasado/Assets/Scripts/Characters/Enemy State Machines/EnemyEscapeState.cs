using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEscapeState : EnemyBaseState
{
    public EnemyEscapeState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactor) : base(currentContext, enemyStateFactor)
    {

    }

    public override void EnterState()
    {
        _ctx.Animator.SetFloat("Speed", 1f);
        _ctx.NavMesh.isStopped = false;
        _ctx.GeneralCooldown = 1.5f;
        _ctx.NavMesh.speed *= 0.75f;
    }

    public override void UpdateState()
    {
        SpacingPlayer();
        FacePlayer();
        CheckSwitchState();
    }

    public override void ExitState()
    {
        _ctx.FollowTarget = false;
        _ctx.NavMesh.speed = _ctx.InitialSpeed;
    }

    public override void CheckSwitchState()
    {
        float distance = Vector3.Distance(_ctx.Target.position, _ctx.transform.position);

        if (distance >= _ctx.EscapeRadius || _ctx.GeneralCooldown <= 0f)
        {
            SwitchState(_factory.Walk());
        }
        if (_ctx.OnAttackRange == true)
        {
            SwitchState(_factory.Attack());
        }
        if (_ctx.Health <= 0)
        {
            SwitchState(_factory.Death());
        }
    }

    /// <summary>
    /// Function that makes the enemy get away from player.
    /// </summary>
    private void SpacingPlayer()
    {
        _ctx.NavMesh.SetDestination(_ctx.EscapePosition.position);

        if (_ctx.GeneralCooldown > 0f)
        {
            _ctx.GeneralCooldown -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Function that controls rotating to player.
    /// </summary>
    private void FacePlayer()
    {
        Vector3 direction = (_ctx.Target.position - _ctx.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, 0));
        _ctx.transform.rotation = Quaternion.Slerp(_ctx.transform.rotation, lookRotation, Time.deltaTime * 12.5f);
    }
}
