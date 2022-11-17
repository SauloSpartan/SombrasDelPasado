using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProvokeState : EnemyBaseState
{
    public EnemyProvokeState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactor) : base(currentContext, enemyStateFactor)
    {

    }

    public override void EnterState()
    {
        _ctx.Animator.SetFloat("Speed", 1f);
        _ctx.NavMesh.isStopped = false;
        _ctx.RandomDesition = Random.Range(1, 3);
        _ctx.GeneralCooldown = 3f;
    }

    public override void UpdateState()
    {
        DesitionState();
        FacePlayer();
        CheckSwitchState();
    }

    public override void ExitState()
    {
        _ctx.FollowTarget = false;
    }

    public override void CheckSwitchState()
    {
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
    /// Function that controls a random direction AI will take.
    /// </summary>
    private void DesitionState()
    {
        if (_ctx.RandomDesition == 1) // For Turning Left
        {
            AroundLeft();
            _ctx.GeneralCooldown -= Time.deltaTime;

            if (_ctx.GeneralCooldown <= 0)
            {
                SwitchState(_factory.Walk());
            }
        }
        else if (_ctx.RandomDesition == 2) // For Turning Right
        {
            AroundRight();
            _ctx.GeneralCooldown -= Time.deltaTime;

            if (_ctx.GeneralCooldown <= 0)
            {
                SwitchState(_factory.Walk());
            }
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
