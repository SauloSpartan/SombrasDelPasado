using UnityEngine;
using UnityEngine.AI;


public class Enemy1MovementState : Enemy1BaseState
{
    private NavMeshAgent _navEnemy;
    private Transform _target;
    private Animator _anim;

    public override void EnterState(Enemy1SateManager enemy1)
    {
        _navEnemy = enemy1.GetComponent<NavMeshAgent>();
        _target = PlayerManager.instance.player.transform;
        _anim = enemy1.GetComponent<Animator>();
    }

    public override void UpdateState(Enemy1SateManager enemy1)
    {
        float attackRadius = 1.5f;
        float distance = Vector3.Distance(_target.position, enemy1.transform.position);

        if (distance > attackRadius)
        {
            _navEnemy.SetDestination(_target.position);
            _anim.SetFloat("Speed", 1f);
        }
        else
        {
            enemy1.SwitchState(enemy1.AttackState);
        }
    }

    public override void OnCollisionState(Enemy1SateManager enemy1)
    {

    }
}
