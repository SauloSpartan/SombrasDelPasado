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
        _ctx.RandomDesition = Random.Range(1, 3);
    }

    public override void UpdateState()
    {
        DesitionState();
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
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, 0));
        _ctx.transform.rotation = Quaternion.Slerp(_ctx.transform.rotation, lookRotation, Time.deltaTime * 12.5f);
    }

    private void DesitionState()
    {
        float distance = Vector3.Distance(_ctx.Target.position, _ctx.transform.position);
        if (distance <= _ctx.AttackRadius)
        {
            if (_ctx.RandomDesition == 1) // For Turning Left
            {
                AroundLeft();
            }
            else if (_ctx.RandomDesition == 2) // For Turning Right
            {
                AroundRight();
            }
        }
        else
        {
            FollowPlayer();
        }
    }

    /// <summary>
    /// Function that controls rotation around player on left side.
    /// </summary>
    private void AroundLeft()
    {
        Vector3 offsetPlayer = _ctx.Target.transform.position - _ctx.NavMesh.transform.position;
        Vector3 rotateDirection = Vector3.Cross(offsetPlayer, Vector3.up);
        _ctx.NavMesh.SetDestination(_ctx.NavMesh.transform.position + rotateDirection); // Controls rotate direction
    }

    /// <summary>
    /// Function that controls rotation around player on right side.
    /// </summary>
    private void AroundRight()
    {
        Vector3 offsetPlayer = _ctx.Target.transform.position - _ctx.NavMesh.transform.position;
        Vector3 rotateDirection = Vector3.Cross(offsetPlayer, Vector3.up);
        _ctx.NavMesh.SetDestination(_ctx.NavMesh.transform.position - rotateDirection); // Controls rotate direction
    }
}