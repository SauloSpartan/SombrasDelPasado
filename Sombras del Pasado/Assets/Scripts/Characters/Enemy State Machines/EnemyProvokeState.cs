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
    }

    public override void UpdateState()
    {
        AroundPlayer();
        FacePlayer();
    }

    public override void ExitState()
    {

    }

    public override void CheckSwitchState()
    {

    }

    private void AroundPlayer()
    {
        Vector3 offsetPlayer = _ctx.Target.transform.position - _ctx.NavMesh.transform.position;
        Vector3 rotateDirection = Vector3.Cross(offsetPlayer, Vector3.up);
        _ctx.NavMesh.SetDestination(_ctx.NavMesh.transform.position + rotateDirection);
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
