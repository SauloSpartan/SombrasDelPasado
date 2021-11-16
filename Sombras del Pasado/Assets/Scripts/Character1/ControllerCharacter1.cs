using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerCharacter1 : MonoBehaviour
{
    //Movement
    private float moveSpeed;
    public float walkSpeed;
    public float rotationSpeed;

    //Animation
    private float velocity = 0.0f;
    [SerializeField]  private float acceleration;

    //Health and Damage
    public float health = 100f;
    public int damage;
    [SerializeField] private float attackCoooldown = 0.0f;

    //3D Direction & Gravity
    private Vector3 moveDirection;
    private Vector3 moveVector;
    private Vector3 moveRotation;

    //References
    private CharacterController controller;
    private Animator anim;
    private BoxCollider sword;

    //Audio
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] stepClips;
    [SerializeField] private AudioClip[] attackClips;
    [SerializeField] private AudioClip[] deathClips;

    ControllerCharacter2 Enemy1;
    DMG_Barrel Explosion;
    DMG_Spike Spiked;


    void Start()
    {
        Enemy1 = FindObjectOfType<ControllerCharacter2>();
        Explosion = FindObjectOfType<DMG_Barrel>();
        Spiked = FindObjectOfType<DMG_Spike>();

        //Getting the references
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        sword = GameObject.Find("Espada Allard").GetComponent<BoxCollider>();

        sword.enabled = false;
    }

    void Update()
    {
        //Always updating
        if (health > 0)
        {
            Move();
            Rotate();
            Attack();
        }
        if (health <= 0)
        {
            Death();
        }

        Gravity();
    }

    private void Move()
    {
        //Keys it gets to move
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        //Directions it can move and speed
        moveDirection = new Vector3(moveX, 0, moveZ);

        if (moveDirection == Vector3.zero && velocity > 0.0f)
        {
            velocity -= Time.deltaTime * acceleration;
            Idle();
        }
        else if (moveDirection != Vector3.zero && velocity < 1.0f)
        {
            velocity += Time.deltaTime * acceleration;
            Walk();
        }
        else if(velocity < 0.0f)
        {
            velocity = 0.0f;
        }
        else if (velocity > 1.0f)
        {
            velocity = 1.0f;
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
        anim.SetFloat("Speed", velocity);
    }       

    private void Walk()
    {
        anim.SetFloat("Speed", velocity);
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
        if (Input.GetKeyDown(KeyCode.J) && attackCoooldown <= 0.5f)
        {
            anim.SetTrigger("Attack1");
            attackCoooldown = 2.0f;
        }
        else if (Input.GetKeyDown(KeyCode.J) && attackCoooldown > 0.5f && attackCoooldown <= 2.0f)
        {
            anim.SetTrigger("Attack2");
            attackCoooldown = 4.0f;
        }
        else if (Input.GetKeyDown(KeyCode.K) && attackCoooldown > 2.0f && attackCoooldown <= 4.0f)
        {
            anim.SetTrigger("Attack3");
            attackCoooldown = 0.0f;
        }
        else if (Input.GetKeyDown(KeyCode.J) && attackCoooldown > 2.0f && attackCoooldown <= 4.0f)
        {
            attackCoooldown = 0.0f;
        }
        else if (attackCoooldown > 0.0f)
        {
            attackCoooldown -= Time.deltaTime;
        }
        else if (attackCoooldown <= 0.0f)
        {
            attackCoooldown = 0;
        }
    }

    private void IsAttacking()
    {
        sword.enabled = true;
    }

    private void NotAttacking()
    {
        sword.enabled = false;
    }

    private void Death()
    {
        anim.SetTrigger("Death");
    }


    //Audio functions
    private void Step_Sound()
    {
        AudioClip clip = StepClip();
        audioSource.PlayOneShot(clip);
    }

    private void Attack_Sound()
    {
        AudioClip clip = AttackClip();
        audioSource.PlayOneShot(clip);
    }

    private void Death_Sound()
    {
        AudioClip clip = DeathClip();
        audioSource.PlayOneShot(clip);
    }

    private AudioClip StepClip()
    {
        return stepClips[UnityEngine.Random.Range(0, stepClips.Length)];
    }

    private AudioClip AttackClip()
    {
        return attackClips[UnityEngine.Random.Range(0, attackClips.Length)];
    }

    private AudioClip DeathClip()
    {
        return deathClips[UnityEngine.Random.Range(0, deathClips.Length)];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy1 Sword")
        {
            health = health - Enemy1.damage;
        }
        if (other.gameObject.tag == "Barrel")
        {
            health = health - Explosion.damage;
        }
        if (other.gameObject.tag == "Spikes")
        {
            health = health - Spiked.damage;
        }
    }
}
