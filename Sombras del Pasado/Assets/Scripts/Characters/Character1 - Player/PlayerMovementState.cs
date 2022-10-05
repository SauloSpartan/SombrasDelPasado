using UnityEngine;

public class PlayerMovementState : PlayerBaseState
{
    private Animator _anim;

    PlayerOtherParameters playerParameters;

    public override void EnterState(PlayerStateManager player)
    {
        _anim = player.GetComponent<Animator>();
        playerParameters = player.GetComponent<PlayerOtherParameters>();
    }

    public override void UpdateState(PlayerStateManager player)
    {
        playerParameters.MovePlayer();

        if (playerParameters.MoveDirection != Vector3.zero)
        {
            _anim.SetFloat("Speed", 1f);
        }
        else if (playerParameters.MoveDirection == Vector3.zero)
        {
            player.SwitchState(player.IdleState);
        }
    }

    public override void OnTriggerState(PlayerStateManager player)
    {

    }
}
