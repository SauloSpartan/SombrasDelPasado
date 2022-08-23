using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerCharacter1 : MonoBehaviour
{
    //Movement
    private float _moveSpeed;
    public float _walkSpeed;
    public float _rotationSpeed;

    //Animation
    private float _animVelocity = 0.0f;
    [SerializeField] private float _animAcceleration;
    [SerializeField] private GameObject _particTrailSword; 

    //Health and Damage
    public float _health = 100f;
    public int _damage;
    public int _defense = 1;
    private int _attack = 1;
    private int _luck;
    private int _evasion = 0;
    private int _attackCombo = 1;
    private float _powerTimer;
    private int _powerUp = 0;

    //3D Direction & Gravity
    private Vector3 _moveDirection;
    private Vector3 _moveVector;
    private Vector3 _moveRotation;

    //References
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
    private CharacterController _charController;
    private ControllerCharacter2 _enemy1;
    private ControllerCharacter3 _enemy2;
    private ControllerCharacter4 _enemy3;
    private ControllerCharacter5 _boss;
    private DMG_Barrel _explosion;
    private DMG_Spike _spiked;


    void Start()
    {
        _enemy1 = FindObjectOfType<ControllerCharacter2>();
        _enemy2 = FindObjectOfType<ControllerCharacter3>();
        _enemy3 = FindObjectOfType<ControllerCharacter4>();
        _boss = FindObjectOfType<ControllerCharacter5>();
        _explosion = FindObjectOfType<DMG_Barrel>();
        _spiked = FindObjectOfType<DMG_Spike>();

        powerDefense = GameObject.Find("Power Defense");
        powerDamage = GameObject.Find("Power Damage");
        powerVelocity = GameObject.Find("Power Velocity");

        //Getting the references
        _charController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        sword = GameObject.Find("Espada Allard").GetComponent<BoxCollider>();

        sword.enabled = false;
        powerDefense.SetActive(false);
        powerDamage.SetActive(false);
        powerVelocity.SetActive(false);
        _particTrailSword.SetActive(false);
    }

    void Update()
    {
        //Always updating
        if (_health > 0)
        {
            Movement();
            Rotation();
            Attack();
        }
        if (_health <= 0)
        {
            Death();
        }
        if (_health > 100)
        {
            _health = 100;
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
        _moveDirection = new Vector3(moveX, 0, moveZ);

        if (_moveDirection == Vector3.zero && _animVelocity > 0.0f)
        {
            _animVelocity -= Time.deltaTime * _animAcceleration;
            Idle();
        }
        else if (_moveDirection != Vector3.zero && _animVelocity < 1.0f)
        {
            _animVelocity += Time.deltaTime * _animAcceleration;
            Walk();
        }
        else if(_animVelocity < 0.0f)
        {
            _animVelocity = 0.0f;
        }
        else if (_animVelocity > 1.0f)
        {
            _animVelocity = 1.0f;
        }

        _moveDirection *= _walkSpeed;

        _charController.Move(_moveDirection * Time.deltaTime);
    }

    private void Rotation()
    {
        //Direction it rotates
        float rotateX = Input.GetAxis("Horizontal");

        //Apply varibles of rotation
        _moveRotation = new Vector3(rotateX, 0, 0);
        _moveRotation.Normalize();

        transform.Translate(_moveRotation * _moveSpeed * Time.deltaTime, Space.World);

        //If character is moving it rotates
        if (_moveRotation != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(_moveRotation, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, _rotationSpeed * Time.deltaTime);
        }
    }

    private void Idle()
    {
        anim.SetFloat("Speed", _animVelocity);
    }       

    private void Walk()
    {
        anim.SetFloat("Speed", _animVelocity);
    }

    private void Gravity()
    {
        _moveVector = Vector3.zero;

        //Check if character is grounded
        if (_charController.isGrounded == false)
        {
            //Add our gravity Vector
            _moveVector += Physics.gravity;
        }

        //Apply our move Vector , remeber to multiply by Time.delta
        _charController.Move(_moveVector * Time.deltaTime);
    }

    private void Attack()
    {
        if (_attackCombo == 1 && Input.GetKeyDown(KeyCode.J))
        {
            anim.SetInteger("AttackCombo", 1);
            _damage = 20 * _attack;
        }
        if (_attackCombo == 2 && Input.GetKeyDown(KeyCode.J))
        {
            anim.SetInteger("AttackCombo", 2);
            _damage = 30 * _attack;
        }
        if (_attackCombo == 3 && Input.GetKeyDown(KeyCode.J))
        {
            anim.SetInteger("AttackCombo", 3);
            _damage = 50 * _attack;
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
        _particTrailSword.SetActive(true);
        _attackCombo = 2;
    }
    private void Combo2()
    {
        _particTrailSword.SetActive(true);
        _attackCombo = 3;
    }

    private void ComboEnd()
    {
        _attackCombo = 1;
        anim.SetInteger("AttackCombo", 0);
        _particTrailSword.SetActive(false);
    }

    private void Death()
    {
        anim.SetTrigger("Death");
    }

    private void PowerUp()
    {
        _luck = Random.Range(0, 4);

        if (_powerTimer > 0.0f && _powerUp == 1)
        {
            _defense = 2;
            powerDefense.SetActive(true);
            _powerTimer -= Time.deltaTime;
        }
        if (_powerTimer > 0.0f && _powerUp == 2)
        {
            _attack = 2;
            powerDamage.SetActive(true);
            _powerTimer -= Time.deltaTime;
        }
        if (_powerTimer > 0.0f && _powerUp == 3)
        {
            _walkSpeed = 4;
            _evasion = 1;
            powerVelocity.SetActive(true);
            _powerTimer -= Time.deltaTime;
        }
        if (_powerTimer <= 0.0f)
        {
            _defense = 1;
            _attack = 1;
            _walkSpeed = 3;
            _evasion = 0;

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
        if (_luck == _evasion && _evasion == 1)
        {
            if (other.gameObject.tag == "Enemy1 Sword")
                _health = _health - (_enemy1.damage - _enemy1.damage);

            if (other.gameObject.tag == "Enemy2 Sword")
                _health = _health - (_enemy2.damage - _enemy2.damage);

            if (other.gameObject.tag == "Enemy3 Dagger")
                _health = _health - (_enemy3.damage - _enemy3.damage);

            if (other.gameObject.tag == "Enemy4 Sword")
                _health = _health - (_boss.damage - _boss.damage);

            if (other.gameObject.tag == "Barrel")
                _health = _health - (_explosion.damage - _explosion.damage);

            if (other.gameObject.tag == "Spikes")
                _health = _health - (_spiked.damage - _spiked.damage);
        }
        else
        {
            if (other.gameObject.tag == "Enemy1 Sword")
                _health = _health - (_enemy1.damage / _defense);

            if (other.gameObject.tag == "Enemy2 Sword")
                _health = _health - (_enemy2.damage / _defense);

            if (other.gameObject.tag == "Enemy3 Dagger")
                _health = _health - (_enemy3.damage / _defense);

            if (other.gameObject.tag == "Enemy4 Sword")
                _health = _health - (_boss.damage / _defense);

            if (other.gameObject.tag == "Barrel")
                _health = _health - (_explosion.damage / _defense);

            if (other.gameObject.tag == "Spikes")
                _health = _health - (_spiked.damage / _defense);

        }

        if (other.gameObject.tag == "PowerUp Defense")
        {
            _powerTimer = 20.0f;
            _powerUp = 1;
        }
        if (other.gameObject.tag == "PowerUp Attack")
        {
            _powerTimer = 20.0f;
            _powerUp = 2;
        }
        if (other.gameObject.tag == "PowerUp Velocity")
        {
            _powerTimer = 20.0f;
            _powerUp = 3;
        }
    }
}
