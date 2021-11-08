using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerCharacter1 : MonoBehaviour
{
    //Variables
    private float moveSpeed;
    public float walkSpeed;
    public float rotationSpeed;
    private float attack;
    public float health = 100f;

    //3D Direction & Gravity
    private Vector3 moveDirection;
    private Vector3 moveVector;
    private Vector3 moveRotation;

    //References
    private CharacterController controller;
    private Animator anim;

    void Start()
    {
        //Getting the references
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        attack = 0;
    }

    void Update()
    {
        //Always updating
        Move();
        Rotate();
        Gravity();
        Death();

        if (Input.GetKeyDown(KeyCode.J))
        {
            Attack();
        }
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

        controller.Move(moveDirection * Time.deltaTime);
    }

    private void Rotate()
    {
        //Direction it rotates
        float rotateX = Input.GetAxis("Horizontal");

        //Apply varibles of rotation
        moveRotation = new Vector3(rotateX, 0, 0);
        moveRotation.Normalize();

        transform.Translate(moveRotation * moveSpeed * Time.deltaTime, Space.World);

        //If character is moving it rotates
        if (moveRotation != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveRotation, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
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

    private void Attack()
    {
        anim.SetTrigger("Attack1");
    }

    private void Death()
    {
        if (health == 0)
        {
            anim.SetTrigger("Death");
            GetComponent<ControllerCharacter1>().enabled = false;
        }
    }
}
