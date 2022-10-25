using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    public EnemyIdleState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactor) : base(currentContext, enemyStateFactor)
    {

    }

    public override void EnterState()
    {
        _ctx.StartCoroutine(DesitionState());
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

    }

    private IEnumerator DesitionState()
    {
        // Random.Range(x, secondNumber) secondNumber is not gona be used, just the one before it
        int randomDesition = Random.Range(1, 4); // 1. Idle, 2. Walk, 3. Provoke

        if (randomDesition == 1) // For Idle
        {
            _ctx.Animator.SetFloat("Speed", 0f);
            _ctx.NavMesh.isStopped = true;
            yield return new WaitForSecondsRealtime(2f);

            SwitchState(_factory.Idle());
        }
        else if (randomDesition == 2) // For Walk
        {
            _ctx.FollowTarget = true;
            yield return null;

            SwitchState(_factory.Walk());
        }
        else if (randomDesition == 3) // For Provoke
        {
            yield return null;

            SwitchState(_factory.Provoke());
        }
    }
}