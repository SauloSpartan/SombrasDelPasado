using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    // Movement variables
    [SerializeField] private float WalkSpeed;
    [SerializeField] private float RotationSpeed;

    // Animation variables
    private float velocity = 0.0f;
    [SerializeField] private float acceleration;
    [SerializeField] private GameObject trailSword;
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

    // 3D Direction & Gravity variables
    private Vector3 _moveDirection;
    private Vector3 moveVector;
    private Vector3 moveRotation;

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
    public Vector3 MoveDirection { get { return _moveDirection; } set { _moveDirection = value; } }

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
        trailSword.SetActive(false);

        // Setup state
        _states = new PlayerStateFactory(this); // "(this)" is a PlayerStateMachine instance
        _currentState = _states.Idle();
        _currentState.EnterState();
    }

    void Update()
    {
        Rotation();
        Gravity();
        _currentState.UpdateState();
        _charController.Move(_moveDirection * Time.deltaTime);
    }

    private void Rotation()
    {
        //Direction it rotates
        float rotateX = Input.GetAxis("Horizontal");

        //Apply varibles of rotation
        moveRotation = new Vector3(rotateX, 0, 0);
        moveRotation.Normalize();

        transform.Translate(moveRotation * WalkSpeed * Time.deltaTime, Space.World);

        //If character is moving it rotates
        if (moveRotation != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveRotation, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, RotationSpeed * Time.deltaTime);
        }
    }

    private void Gravity()
    {
        moveVector = Vector3.zero;

        //Check if character is grounded
        if (_charController.isGrounded == false)
        {
            //Add our gravity Vector
            moveVector += Physics.gravity;
        }

        //Apply our move Vector , remeber to multiply by Time.delta
        _charController.Move(moveVector * Time.deltaTime);
    }
}
