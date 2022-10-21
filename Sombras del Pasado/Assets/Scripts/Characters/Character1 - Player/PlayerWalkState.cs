using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactor) : base(currentContext, playerStateFactor)
    {

    }

    public override void EnterState()
    {
        _ctx.Animator.SetFloat("Speed", 1);

        Debug.Log("Hello from Walk");
    }

    public override void UpdateState()
    {
        Movement();
        Rotation();
        CheckSwitchState();
    }

    public override void ExitState()
    {
        _ctx.MoveDirection = Vector3.zero;
    }

    public override void CheckSwitchState()
    {
        if (_ctx.MoveDirection == Vector3.zero)
        {
            SwitchState(_factory.Idle());
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            SwitchState(_factory.Attack());
        }
        if (_ctx.Health <= 0)
        {
            SwitchState(_factory.Death());
        }
    }

    /// <summary>
    /// Function that controls character movement.
    /// </summary>
    private void Movement()
    {
        _ctx.MoveX = Input.GetAxis("Horizontal");
        _ctx.MoveZ = Input.GetAxis("Vertical");

        _ctx.MoveDirection = new Vector3(_ctx.MoveX, 0, _ctx.MoveZ);
    }

    /// <summary>
    /// Function that controls character rotation.
    /// </summary>
    private void Rotation()
    {
        _ctx.MoveX = Input.GetAxis("Horizontal");

        //Apply varibles of rotation
        _ctx.MoveRotation = new Vector3(_ctx.MoveX, 0, 0);
        _ctx.MoveRotation.Normalize();

        _ctx.transform.Translate(_ctx.MoveRotation * _ctx.WalkSpeed * Time.deltaTime, Space.World);

        //If character is moving it rotates
        if (_ctx.MoveRotation != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(_ctx.MoveRotation, Vector3.up);

            _ctx.transform.rotation = Quaternion.RotateTowards(_ctx.transform.rotation, toRotation, _ctx.RotationSpeed * Time.deltaTime);
        }
    }
}