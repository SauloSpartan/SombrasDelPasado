using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWalkState : EnemyBaseState
{
    public EnemyWalkState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactor) : base(currentContext, enemyStateFactor)
    {

    }

    public override void EnterState()
    {
        Debug.Log("Enemy Walk");
    }

    public override void UpdateState()
    {
        FollowPlayer();
        FacePlayer();
    }

    public override void ExitState()
    {

    }

    public override void CheckSwitchState()
    {

    }

    private void FollowPlayer()
    {
        float distance = Vector3.Distance(_ctx.Target.position, _ctx.transform.position);
        _ctx.NavMesh.enabled = true;
        _ctx.NavMesh.SetDestination(_ctx.Target.position);
    }

    private void FacePlayer()
    {
        Vector3 direction = (_ctx.Target.position - _ctx.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        _ctx.transform.rotation = Quaternion.Slerp(_ctx.transform.rotation, lookRotation, Time.deltaTime * 12.5f);
    }
}