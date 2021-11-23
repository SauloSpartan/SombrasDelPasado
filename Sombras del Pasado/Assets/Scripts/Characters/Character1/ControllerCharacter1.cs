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
    public int defense;
    private int attack = 1;
    private float attackCombo1 = 0.0f;
    private float attackCombo2 = 0.0f;
    private float powerTimer;
    private int powerUp = 0;

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
    ControllerCharacter3 Enemy2;
    ControllerCharacter4 Enemy3;
    ControllerCharacter5 Boss;
    DMG_Barrel Explosion;
    DMG_Spike Spiked;


    void Start()
    {
        Enemy1 = FindObjectOfType<ControllerCharacter2>();
        Enemy2 = FindObjectOfType<ControllerCharacter3>();
        Enemy3 = FindObjectOfType<ControllerCharacter4>();
        Boss = FindObjectOfType<ControllerCharacter5>();
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
        if (health > 100)
        {
            health = 100;
        }

        Gravity();
        PowerUp();
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
        if (Input.GetKeyDown(KeyCode.J) && attackCombo1 <= 0.0f && attackCombo2 <= 0.0f)
        {
            anim.SetTrigger("Attack1");
            attackCombo1 = 0.4f;
            attackCombo2 = 0.0f;
            damage = 20 * attack;
        }
        else if (Input.GetKeyDown(KeyCode.J) && attackCombo1 <= 0.4f && attackCombo1 > 0.0f)
        {
            anim.SetTrigger("Attack2");
            attackCombo1 = 0.0f;
            attackCombo2 = 0.5f;
            damage = 30 * attack;
        }
        else if (Input.GetKeyDown(KeyCode.J) && attackCombo2 <= 0.5f && attackCombo2 > 0.0f)
        {
            anim.SetTrigger("Attack3");
            attackCombo1 = 0.0f;
            attackCombo2 = 0.0f;
            damage = 50 * attack;
        }

        if (attackCombo1 > 0.0f)
        {
            attackCombo1 -= Time.deltaTime;
        }
        if (attackCombo2 > 0.0f)
        {
            attackCombo2 -= Time.deltaTime;
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

    private void PowerUp()
    {
        if (powerTimer > 0.0f && powerUp == 1)
        {
            defense = 2;
            powerTimer -= Time.deltaTime;
            Destroy(GameObject.Find("PowerUp Defense"));
        }
        if (powerTimer > 0.0f && powerUp == 2)
        {
            attack = 2;
            powerTimer -= Time.deltaTime;
            Destroy(GameObject.Find("PowerUp Damage"));
        }
        if (powerTimer > 0.0f && powerUp == 3)
        {
            walkSpeed = 4;
            powerTimer -= Time.deltaTime;
            Destroy(GameObject.Find("PowerUp Velocity"));
        }
        if (powerTimer <= 0.0f)
        {
            defense = 0;
            attack = 1;
            walkSpeed = 3;
        }
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
            health = health - (Enemy1.damage - defense);
        }
        if (other.gameObject.tag == "Enemy2 Sword")
        {
            health = health - (Enemy2.damage - defense);
        }
        if (other.gameObject.tag == "Enemy3 Dagger")
        {
            health = health - (Enemy3.damage - defense);
        }
        if (other.gameObject.tag == "Enemy4 Sword")
        {
            health = health - (Boss.damage - defense);
        }
        if (other.gameObject.tag == "Barrel")
        {
            health = health - (Explosion.damage - defense);
        }
        if (other.gameObject.tag == "Spikes")
        {
            health = health - (Spiked.damage - defense);
        }

        if (other.gameObject.tag == "PowerUp Defense")
        {
            powerTimer = 15.0f;
            powerUp = 1;
        }
        if (other.gameObject.tag == "PowerUp Attack")
        {
            powerTimer = 15.0f;
            powerUp = 2;
        }
        if (other.gameObject.tag == "PowerUp Velocity")
        {
            powerTimer = 15.0f;
            powerUp = 3;
        }
    }
}