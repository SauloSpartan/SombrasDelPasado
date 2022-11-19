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
        _ctx.GeneralCooldown = Random.Range(0.1f, 0.6f); // Takes some time before attacking
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

        if (_ctx.OnAttackRange == false && _ctx.GeneralCooldown <= 0.4f)
        {
            SwitchState(_factory.Escape());
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
        if (_ctx.OnAttackRange == true && _ctx.GeneralCooldown <= 0f)
        {
            _ctx.Animator.SetTrigger("Attack1");

            if (_ctx.TrailSwordOne != null)
            {
                _ctx.TrailSwordOne.SetActive(true);
            }
            if (_ctx.TrailSwordTwo != null) // Not all characters need a second trail, if it's null, nothing happens
            {
                _ctx.TrailSwordTwo.SetActive(true);
            }

            if (_ctx.EnemyType == "Enemy Basic")
            {
                _ctx.GeneralCooldown = 2.0f;
            }
            if (_ctx.EnemyType == "Enemy Heavy")
            {
                _ctx.GeneralCooldown = 3.0f;
            }
            if (_ctx.EnemyType == "Enemy Fast")
            {
                _ctx.GeneralCooldown = 2.0f;
            }
            if (_ctx.EnemyType == "Enemy Boss")
            {
                _ctx.GeneralCooldown = 4.0f;
            }
        }
        else if (_ctx.GeneralCooldown > 0f) // Makes a countdown for cooldown
        {
            _ctx.GeneralCooldown -= Time.deltaTime;
        }

        if (_ctx.GeneralCooldown <= 0.4f) // Before attacking again it rotates to player
        {
            if (_ctx.TrailSwordOne != null)
            {
                _ctx.TrailSwordOne.SetActive(false);
            }
            if (_ctx.TrailSwordTwo != null)
            {
                _ctx.TrailSwordTwo.SetActive(false);
            }

            FacePlayer();
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