using UnityEngine;

public class Enemy1DeathState : Enemy1BaseState
{
    private Animator _anim;
    private CapsuleCollider _enemyCollider;
    Enemy1OtherParameters enemyParameters;

    public override void EnterState(Enemy1SateManager enemy1)
    {
        _anim = enemy1.GetComponent<Animator>();
        _anim.SetTrigger("Death");
        _enemyCollider = enemy1.GetComponent<CapsuleCollider>();
        _enemyCollider.enabled = false;
        enemyParameters = enemy1.GetComponent<Enemy1OtherParameters>();
    }

    public override void UpdateState(Enemy1SateManager enemy1)
    {
        enemyParameters.DeathDestroy();
    }

    public override void OnTriggerState(Enemy1SateManager enemy1)
    {

    }
}
