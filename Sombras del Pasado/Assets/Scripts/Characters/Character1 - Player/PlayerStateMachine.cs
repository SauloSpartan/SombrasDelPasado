using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is the CONTEXT and stores the data concrete states need to be performed
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
    [SerializeField] private float health = 100f;
    [SerializeField] private int damage;
    [SerializeField] private int defense = 1;
    private int attack = 1;
    private int luck;
    private int evasion = 0;
    private int attackCombo = 1;
    private float powerTimer;
    private int powerUp = 0;

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
    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public Animator Animator { get { return _animator; } set { _animator = value; } }
    public float MoveX { get { return _moveX; } set { _moveX = value; } }
    public float MoveZ { get { return _moveZ; } set { _moveZ = value; } }
    public Vector3 MoveDirection { get { return _moveDirection; } set { _moveDirection = value; } }
    public CharacterController CharController { get { return _charController; } }
    public float WalkSpeed { get { return _walkSpeed; } set { _walkSpeed = value; } }
    public float RotationSpeed { get { return _rotationSpeed; } }
    public Vector3 MoveRotation { get { return _moveRotation; } set { _moveRotation = value; } }

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

        // Setup state
        _states = new PlayerStateFactory(this); // "(this)" is a PlayerStateMachine instance
        _currentState = _states.Idle();
        _currentState.EnterState();
    }

    void Update()
    {
        Gravity();
        _currentState.UpdateState();
        _charController.Move(_moveDirection * _walkSpeed * Time.deltaTime);
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
}
