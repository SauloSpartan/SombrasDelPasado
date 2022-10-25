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
        FacePlayer();
        CheckSwitchState();
    }

    public override void ExitState()
    {

    }

    public override void CheckSwitchState()
    {
        float distance = Vector3.Distance(_ctx.Target.position, _ctx.transform.position);

        if (distance <= _ctx.AttackRadius)
        {
            SwitchState(_factory.Attack());
        }
    }

    /// <summary>
    /// Coroutine that controls a random desition that AI will make.
    /// </summary>
    /// <returns> Returns a timer if Idle() or null if not.</returns>
    private IEnumerator DesitionState()
    {
        // Random.Range(x, secondNumber) secondNumber is not gona be used, just the one before it
        _ctx.RandomDesition = Random.Range(1, 5); // 1. Idle, 2. Walk, 3. Provoke, 4. Escape

        if (_ctx.RandomDesition == 1) // For Idle
        {
            _ctx.FollowTarget = false;
            _ctx.Animator.SetFloat("Speed", 0f);
            _ctx.NavMesh.isStopped = true;
            yield return new WaitForSecondsRealtime(2f);

            SwitchState(_factory.Idle());
        }
        else if (_ctx.RandomDesition == 2) // For Walk
        {
            _ctx.FollowTarget = true;
            yield return null;

            SwitchState(_factory.Walk());
        }
        else if (_ctx.RandomDesition == 3) // For Provoke
        {
            yield return null;

            SwitchState(_factory.Provoke());
        }
        else if (_ctx.RandomDesition == 4) // For Escape
        {
            yield return null;

            SwitchState(_factory.Escape());
        }
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