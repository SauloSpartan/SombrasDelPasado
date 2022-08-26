using UnityEngine;
using UnityEngine.AI;

public class Enemy1AttackState : Enemy1BaseState
{
    private Transform _target;
    private Animator _anim;

    private float _damage;
    private float _attackCoooldown;

    Enemy1OtherParameters enemyParameters;

    public override void EnterState(Enemy1SateManager enemy1)
    {
        _attackCoooldown = 0.0f;

        enemyParameters = enemy1.GetComponent<Enemy1OtherParameters>();
        _damage = enemyParameters.Damage;

        _target = PlayerManager.instance.player.transform;
        _anim = enemy1.GetComponent<Animator>();
    }

    public override void UpdateState(Enemy1SateManager enemy1)
    {
        if (_attackCoooldown <= 0.0f)
        {
            _anim.SetTrigger("Attack1");
            _attackCoooldown = 2.0f;
        }
        else if (_attackCoooldown > 0.0f)
        {
            _attackCoooldown -= Time.deltaTime;
        }
        if (_attackCoooldown <= 0.4f)
        {
            //Look at player
            Vector3 direction = (_target.position - enemy1.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            enemy1.transform.rotation = Quaternion.Slerp(enemy1.transform.rotation, lookRotation, Time.deltaTime * 12.5f);
        }

        float attackRadius = 1.5f;
        float distance = Vector3.Distance(_target.position, enemy1.transform.position);

        if (distance > attackRadius && _attackCoooldown <= 0.0f)
        {
            enemy1.SwitchState(enemy1.MovementState);
        }
    }

    public override void OnTriggerState(Enemy1SateManager enemy1)
    {
        enemy1.SwitchState(enemy1.DamageState);
    }
}
