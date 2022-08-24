using UnityEngine;
using UnityEngine.AI;


public class Enemy1MovementState : Enemy1BaseState
{
    private NavMeshAgent _navEnemy;
    private Transform _target;
    private float _attackRadius = 1.5f;

    public override void EnterState(Enemy1SateManager enemy1)
    {
        _navEnemy = enemy1.GetComponent<NavMeshAgent>();
        _target = PlayerManager.instance.player.transform;
    }

    public override void UpdateState(Enemy1SateManager enemy1)
    {
        float distance = Vector3.Distance(_target.position, enemy1.transform.position);
        if (distance > _attackRadius)
        {
            _navEnemy.SetDestination(_target.position);
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
