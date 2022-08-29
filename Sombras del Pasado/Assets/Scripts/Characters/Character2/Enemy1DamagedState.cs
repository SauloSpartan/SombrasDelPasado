using UnityEngine;

public class Enemy1DamagedState : Enemy1BaseState
{
    private Transform _target;

    Enemy1OtherParameters enemyParameters;

    public override void EnterState(Enemy1SateManager enemy1)
    {
        _target = PlayerManager.instance.player.transform;
        enemyParameters = enemy1.GetComponent<Enemy1OtherParameters>();
        enemyParameters.NavEnemy.enabled = false;
        enemyParameters.RigidEnemy.isKinematic = false;

        enemyParameters.DamageRecieve();
    }

    public override void UpdateState(Enemy1SateManager enemy1)
    {
        if (enemyParameters.Health <= 0)
        {
            enemy1.SwitchState(enemy1.DeathState);
        }
        else
        {
            enemy1.SwitchState(enemy1.AttackState);
        }
    }

    public override void OnTriggerState(Enemy1SateManager enemy1)
    {

    }
}
