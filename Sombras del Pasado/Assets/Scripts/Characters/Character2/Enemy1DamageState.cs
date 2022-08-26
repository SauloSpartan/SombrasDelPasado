using UnityEngine;

public class Enemy1DamageState : Enemy1BaseState
{
    Enemy1OtherParameters enemyParameters;
    private Transform _target;

    public override void EnterState(Enemy1SateManager enemy1)
    {
        _target = PlayerManager.instance.player.transform;
        enemyParameters = enemy1.GetComponent<Enemy1OtherParameters>();
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
