using UnityEngine;

public class Enemy1MovementState : Enemy1BaseState
{
    private Transform _target;
    private Animator _anim;

    Enemy1OtherParameters enemyParameters;

    public override void EnterState(Enemy1StateManager enemy1)
    {
        enemyParameters = enemy1.GetComponent<Enemy1OtherParameters>();
        enemyParameters.NavEnemy.enabled = true;
        enemyParameters.RigidEnemy.isKinematic = true;

        _target = PlayerManager.instance.player.transform;
        _anim = enemy1.GetComponent<Animator>();
    }

    public override void UpdateState(Enemy1StateManager enemy1)
    {
        //Follow player
        float attackRadius = 1.5f;
        float distance = Vector3.Distance(_target.position, enemy1.transform.position);

        //Look at player
        Vector3 direction = (_target.position - enemy1.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        enemy1.transform.rotation = Quaternion.Slerp(enemy1.transform.rotation, lookRotation, Time.deltaTime * 12.5f);

        //Switching states depending if in attack range
        if (distance > attackRadius)
        {
            enemyParameters.NavEnemy.SetDestination(_target.position);
            _anim.SetFloat("Speed", 1f);
        }
        else
        {
            enemy1.SwitchState(enemy1.AttackState);
        }
    }

    public override void OnTriggerState(Enemy1StateManager enemy1)
    {

    }
}
