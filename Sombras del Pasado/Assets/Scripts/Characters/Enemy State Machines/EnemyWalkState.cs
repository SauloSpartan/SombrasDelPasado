using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalkState : EnemyBaseState
{
    public EnemyWalkState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactor) : base(currentContext, enemyStateFactor)
    {

    }

    public override void EnterState()
    {
        _ctx.Animator.SetFloat("Speed", 1f);
        _ctx.NavMesh.isStopped = false;
    }

    public override void UpdateState()
    {
        FollowPlayer();
        FacePlayer();
        CheckSwitchState();
    }

    public override void ExitState()
    {

    }

    public override void CheckSwitchState()
    {
        float distance = Vector3.Distance(_ctx.Target.position, _ctx.transform.position);

        if (distance <= _ctx.StopRadius && _ctx.FollowTarget == false)
        {
            SwitchState(_factory.Idle());
        }
        if (distance <= _ctx.AttackRadius)
        {
            SwitchState(_factory.Attack());
        }
        if (_ctx.Health <= 0)
        {
            SwitchState(_factory.Death());
        }
    }

    /// <summary>
    /// Function that controls following the player.
    /// </summary>
    private void FollowPlayer()
    {
        _ctx.NavMesh.SetDestination(_ctx.Target.position);
    }

    /// <summary>
    /// Function that controls rotating to player.
    /// </summary>
    private void FacePlayer()
    {
        Vector3 direction = (_ctx.Target.position - _ctx.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        _ctx.transform.rotation = Quaternion.Slerp(_ctx.transform.rotation, lookRotation, Time.deltaTime * 12.5f);
    }
}