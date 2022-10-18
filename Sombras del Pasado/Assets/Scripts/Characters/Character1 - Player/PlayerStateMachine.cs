using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is the CONTEXT and stores the data that the concrete states need to be performed
public class PlayerStateMachine : MonoBehaviour
{
    // Movement and Rotation variables
    private float _moveX;
    private float _moveZ;
    [SerializeField] private float _walkSpeed;
    private float _rotationSpeed = 1080;
    private Vector3 _moveDirection;
    private Vector3 _moveVector;
    private Vector3 _moveRotation;

    // Animation variables
    private float velocity = 0.0f;
    [SerializeField] private float _acceleration;
    [SerializeField] private GameObject _trailSword;
    private Animator _animator;

    // Health and Damage variables
    [SerializeField] private float _health = 100f;
    [SerializeField] private int _damage;
    [SerializeField] private int defense = 1;
    private int _attack = 1;
    private int luck;
    private int evasion = 0;
    private int _attackCombo = 1;
    private float powerTimer;
    private int powerUp = 0;
    private bool _canMove = true;

    // References variables
    private CharacterController _charController;
    private BoxCollider sword;
    private GameObject powerDefense;
    private GameObject powerDamage;
    private GameObject powerVelocity;

    // Audio variables
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] stepClips;
    [SerializeField] private AudioClip[] attackClips;
    [SerializeField] private AudioClip[] deathClips;

    // State variables
    private PlayerBaseState _currentState;
    private PlayerStateFactory _states;

    // Getters and Setters
    /// <value> Reference to BaseState Script. </value>
    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public Animator Animator { get { return _animator; } set { _animator = value; } }
    /// <value> Float for movement in X axis. </value>
    public float MoveX { get { return _moveX; } set { _moveX = value; } }
    /// <value> Float for movement in Z axis. </value>
    public float MoveZ { get { return _moveZ; } set { _moveZ = value; } }
    public Vector3 MoveDirection { get { return _moveDirection; } set { _moveDirection = value; } }
    public CharacterController CharController { get { return _charController; } }
    public float WalkSpeed { get { return _walkSpeed; } set { _walkSpeed = value; } }
    public float RotationSpeed { get { return _rotationSpeed; } }
    public Vector3 MoveRotation { get { return _moveRotation; } set { _moveRotation = value; } }
    public int Damage { get { return _damage; } set { _damage = value; } }
    public int Attack { get { return _attack; } set { _attack = value; } }
    public int AttackCombo { get { return _attackCombo; } }
    public bool CanMove { get { return _canMove; } set { _canMove = value; } }
    public float Health { get { return _health; } }

    // Awake is called earlier than Start
    void Awake()
    {
        powerDefense = GameObject.Find("Power Defense");
        powerDamage = GameObject.Find("Power Damage");
        powerVelocity = GameObject.Find("Power Velocity");

        // Getting the references
        _charController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        sword = GameObject.Find("Espada Allard").GetComponent<BoxCollider>();

        sword.enabled = false;
        powerDefense.SetActive(false);
        powerDamage.SetActive(false);
        powerVelocity.SetActive(false);
        _trailSword.SetActive(false);
    }

    void Start()
    {
        // Setup state
        _states = new PlayerStateFactory(this); // "(this)" is a PlayerStateMachine instance
        _currentState = _states.Idle();
        _currentState.EnterState();
    }

    void Update()
    {
        Gravity();

        _currentState.UpdateState();

        // It is outside "WalkState" because glitches if its inside
        _charController.Move(_moveDirection * _walkSpeed * Time.deltaTime); // It moves the character
    }


    /// <summary>
    /// Function that adds Gravity to the player.
    /// </summary>
    private void Gravity()
    {
        _moveVector = Vector3.zero;

        if (_charController.isGrounded == false) // Check if character is grounded
        {
            _moveVector += Physics.gravity; // Add our gravity Vector
        }

        _charController.Move(_moveVector * Time.deltaTime); // Apply our move Vector
    }

    #region Attack Events
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
        _trailSword.SetActive(true);
        _attackCombo = 2;
        _canMove = false;
    }
    private void Combo2()
    {
        _trailSword.SetActive(true);
        _attackCombo = 3;
        _canMove = false;
    }

    private void Combo3()
    {
        _trailSword.SetActive(true);
        _canMove = false;
    }

    private void ComboEnd()
    {
        _trailSword.SetActive(false);
        _attackCombo = 1;
        _animator.SetInteger("AttackCombo", 0);
        _canMove = true;
    }
    #endregion

    #region Audio Events
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
    #endregion
}
