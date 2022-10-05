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
    [SerializeField] private float acceleration;
    [SerializeField] private GameObject trailSword; 

    //Health and Damage
    public float health = 100f;
    public int damage;
    public int defense = 1;
    private int attack = 1;
    private int luck;
    private int evasion = 0;
    private int attackCombo = 1;
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
    private GameObject powerDefense;
    private GameObject powerDamage;
    private GameObject powerVelocity;

    //Audio
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] stepClips;
    [SerializeField] private AudioClip[] attackClips;
    [SerializeField] private AudioClip[] deathClips;

    //Scripts
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

        powerDefense = GameObject.Find("Power Defense");
        powerDamage = GameObject.Find("Power Damage");
        powerVelocity = GameObject.Find("Power Velocity");

        //Getting the references
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        sword = GameObject.Find("Espada Allard").GetComponent<BoxCollider>();

        sword.enabled = false;
        powerDefense.SetActive(false);
        powerDamage.SetActive(false);
        powerVelocity.SetActive(false);
        trailSword.SetActive(false);
    }

    void Update()
    {
        //Always updating
        if (health > 0)
        {
            Movement();
            Rotation();
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

    private void Movement()
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

    private void Rotation()
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
        if (attackCombo == 1 && Input.GetKeyDown(KeyCode.J))
        {
            anim.SetInteger("AttackCombo", 1);
            damage = 20 * attack;
        }
        if (attackCombo == 2 && Input.GetKeyDown(KeyCode.J))
        {
            anim.SetInteger("AttackCombo", 2);
            damage = 30 * attack;
        }
        if (attackCombo == 3 && Input.GetKeyDown(KeyCode.J))
        {
            anim.SetInteger("AttackCombo", 3);
            damage = 50 * attack;
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

    private void ComboStart()
    {
        trailSword.SetActive(true);
        attackCombo = 2;
    }
    private void Combo2()
    {
        trailSword.SetActive(true);
        attackCombo = 3;
    }

    private void ComboEnd()
    {
        attackCombo = 1;
        anim.SetInteger("AttackCombo", 0);
        trailSword.SetActive(false);
    }

    private void Death()
    {
        anim.SetTrigger("Death");
    }

    private void PowerUp()
    {
        luck = Random.Range(0, 4);

        if (powerTimer > 0.0f && powerUp == 1)
        {
            defense = 2;
            powerDefense.SetActive(true);
            powerTimer -= Time.deltaTime;
        }
        if (powerTimer > 0.0f && powerUp == 2)
        {
            attack = 2;
            powerDamage.SetActive(true);
            powerTimer -= Time.deltaTime;
        }
        if (powerTimer > 0.0f && powerUp == 3)
        {
            walkSpeed = 4;
            evasion = 1;
            powerVelocity.SetActive(true);
            powerTimer -= Time.deltaTime;
        }
        if (powerTimer <= 0.0f)
        {
            defense = 1;
            attack = 1;
            walkSpeed = 3;
            evasion = 0;

            powerDefense.SetActive(false);
            powerDamage.SetActive(false);
            powerVelocity.SetActive(false);
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
        if (luck == evasion && evasion == 1)
        {
            if (other.gameObject.tag == "Enemy1 Sword")
                health = health - (Enemy1.damage - Enemy1.damage);

            if (other.gameObject.tag == "Enemy2 Sword")
                health = health - (Enemy2.damage - Enemy2.damage);

            if (other.gameObject.tag == "Enemy3 Dagger")
                health = health - (Enemy3.damage - Enemy3.damage);

            if (other.gameObject.tag == "Enemy4 Sword")
                health = health - (Boss.damage - Boss.damage);

            if (other.gameObject.tag == "Barrel")
                health = health - (Explosion.damage - Explosion.damage);

            if (other.gameObject.tag == "Spikes")
                health = health - (Spiked.damage - Spiked.damage);
        }
        else
        {
            if (other.gameObject.tag == "Enemy1 Sword")
                health = health - (Enemy1.damage / defense);

            if (other.gameObject.tag == "Enemy2 Sword")
                health = health - (Enemy2.damage / defense);

            if (other.gameObject.tag == "Enemy3 Dagger")
                health = health - (Enemy3.damage / defense);

            if (other.gameObject.tag == "Enemy4 Sword")
                health = health - (Boss.damage / defense);

            if (other.gameObject.tag == "Barrel")
                health = health - (Explosion.damage / defense);

            if (other.gameObject.tag == "Spikes")
                health = health - (Spiked.damage / defense);

        }

        if (other.gameObject.tag == "PowerUp Defense")
        {
            powerTimer = 20.0f;
            powerUp = 1;
        }
        if (other.gameObject.tag == "PowerUp Attack")
        {
            powerTimer = 20.0f;
            powerUp = 2;
        }
        if (other.gameObject.tag == "PowerUp Velocity")
        {
            powerTimer = 20.0f;
            powerUp = 3;
        }
    }
}
