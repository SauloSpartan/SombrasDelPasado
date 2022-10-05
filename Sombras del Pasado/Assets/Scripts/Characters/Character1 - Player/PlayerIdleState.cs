using UnityEngine;

public class PlayerIdleState : PlayerBaseState
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
        float _moveX = Input.GetAxis("Horizontal");
        float _moveZ = Input.GetAxis("Vertical");
        playerParameters.MoveDirection = new Vector3(_moveX, 0, _moveZ);

        if (playerParameters.MoveDirection == Vector3.zero)
        {
            _anim.SetFloat("Speed", 0);
        }
        else if (playerParameters.MoveDirection != Vector3.zero) //Switch state if detect movement
        {
            player.SwitchState(player.MovementState);
        }
    }

    public override void OnTriggerState(PlayerStateManager player)
    {

    }
}
