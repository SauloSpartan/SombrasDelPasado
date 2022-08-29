using UnityEngine;

public class Enemy1DeathState : Enemy1BaseState
{
    private Animator _anim;
    private CapsuleCollider _enemyCollider;

    Enemy1OtherParameters enemyParameters;

    public override void EnterState(Enemy1SateManager enemy1)
    {
        //Set the Death animation
        _anim = enemy1.GetComponent<Animator>();
        _anim.SetTrigger("Death");

        enemyParameters = enemy1.GetComponent<Enemy1OtherParameters>();
        enemyParameters.NavEnemy.enabled = true;
        enemyParameters.RigidEnemy.isKinematic = true;

        _enemyCollider = enemy1.GetComponent<CapsuleCollider>();
        _enemyCollider.enabled = false;

        //The number in PowerUpSpawn(30%) represents probability to spawn
        enemyParameters.PowerUpSpawn(20);
    }

    public override void UpdateState(Enemy1SateManager enemy1)
    {
        enemyParameters.DeathDestroy();
    }

    public override void OnTriggerState(Enemy1SateManager enemy1)
    {

    }
}
