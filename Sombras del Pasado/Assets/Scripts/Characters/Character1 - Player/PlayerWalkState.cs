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
        _newContext.Animator.SetFloat("Speed", 1);

        Debug.Log("Hello from Walk");
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }

    public override void ExitState()
    {

    }

    public override void CheckSwitchState()
    {
        Movement();
        Rotation();

        if (_newContext.MoveDirection == Vector3.zero)
        {
            SwitchState(_factory.Idle());
        }
    }

    private void Movement()
    {
        _newContext.MoveX = Input.GetAxis("Horizontal");
        _newContext.MoveZ = Input.GetAxis("Vertical");

        _newContext.MoveDirection = new Vector3(_newContext.MoveX, 0, _newContext.MoveZ);
    }

    private void Rotation()
    {
        _newContext.MoveX = Input.GetAxis("Horizontal");

        //Apply varibles of rotation
        _newContext.MoveRotation = new Vector3(_newContext.MoveX, 0, 0);
        _newContext.MoveRotation.Normalize();

        _newContext.transform.Translate(_newContext.MoveRotation * _newContext.WalkSpeed * Time.deltaTime, Space.World);

        //If character is moving it rotates
        if (_newContext.MoveRotation != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(_newContext.MoveRotation, Vector3.up);

            _newContext.transform.rotation = Quaternion.RotateTowards(_newContext.transform.rotation, toRotation, _newContext.RotationSpeed * Time.deltaTime);
        }
    }
}
