using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    public EnemyAttackState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactor) : base(currentContext, enemyStateFactor)
    {

    }

    public override void EnterState()
    {
        _ctx.Animator.SetFloat("Speed", 1f);
        _ctx.NavMesh.isStopped = true;
        _ctx.GeneralCooldown = 0.5f;
    }

    public override void UpdateState()
    {
        AttackPlayer();
        CheckSwitchState();
    }

    public override void ExitState()
    {
        _ctx.FollowTarget = false;
    }

    public override void CheckSwitchState()
    {
        float distance = Vector3.Distance(_ctx.Target.position, _ctx.transform.position);

        if (distance > _ctx.AttackRadius && _ctx.GeneralCooldown <= 0.0f)
        {
            SwitchState(_factory.Walk());
        }
        if (_ctx.Health <= 0)
        {
            SwitchState(_factory.Death());
        }
    }

    /// <summary>
    /// Function that controls attack animation and cooldown.
    /// </summary>
    private void AttackPlayer()
    {
        float distance = Vector3.Distance(_ctx.Target.position, _ctx.transform.position);

        if (distance <= _ctx.AttackRadius && _ctx.GeneralCooldown <= 0.0f)
        {
            _ctx.TrailSword.SetActive(true);
            _ctx.Animator.SetTrigger("Attack1");
            _ctx.GeneralCooldown = 2.0f;
        }
        else if (_ctx.GeneralCooldown > 0.0f)
        {
            _ctx.GeneralCooldown -= Time.deltaTime;
        }

        if (_ctx.GeneralCooldown <= 0.4f)
        {
            _ctx.TrailSword.SetActive(false);
            FacePlayer();
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