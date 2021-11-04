using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerCharacter1 : MonoBehaviour
{
    //Variables
    private float moveSpeed;
    private float walkSpeed;

    //3D Direction & Gravity
    private Vector3 moveDirection;
    private Vector3 moveVector;

    //Reference
    private CharacterController controller;
    private Animator anim;
    void Start()
    {
        //Getting the character controller reference
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        //Always updating movement
        Move();
        Gravity();
    }

    private void Move()
    {
        //Keys it gets to move
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        //Directions it can move and speed
        moveDirection = new Vector3(moveX, 0, moveZ);

        if(moveDirection == Vector3.zero)
        {
            Idle();
        }
        else
        {
            Walk();
        }

        moveDirection *= walkSpeed;
        walkSpeed = 5;

        controller.Move(moveDirection * Time.deltaTime);
    }

    private void Idle()
    {
        anim.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
    }       

    private void Walk()
    {
        anim.SetFloat("Speed", 1, 0.1f, Time.deltaTime);
    }

    private void Gravity()
    {
        moveVector = Vector3.zero;

        //Check if character is grounded
        if (controller.isGrounded == false)
        {
            //Add our gravity Vector
            moveVector += Physics.gravity;
        }

        //Apply our move Vector , remeber to multiply by Time.delta
        controller.Move(moveVector * Time.deltaTime);
    }
}
