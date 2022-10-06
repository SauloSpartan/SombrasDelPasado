using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    // Movement variables
    public float WalkSpeed;
    public float RotationSpeed;

    // Animation variables
    private float velocity = 0.0f;
    [SerializeField] private float acceleration;
    [SerializeField] private GameObject trailSword;

    // Health and Damage variables
    public float health = 100f;
    public int damage;
    public int defense = 1;
    private int attack = 1;
    private int luck;
    private int evasion = 0;
    private int attackCombo = 1;
    private float powerTimer;
    private int powerUp = 0;

    // 3D Direction & Gravity variables
    private Vector3 moveDirection;
    private Vector3 moveVector;
    private Vector3 moveRotation;

    // References variables
    private CharacterController _charController;
    private Animator anim;
    private BoxCollider sword;
    private GameObject powerDefense;
    private GameObject powerDamage;
    private GameObject powerVelocity;

    // Audio variables
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] stepClips;
    [SerializeField] private AudioClip[] attackClips;
    [SerializeField] private AudioClip[] deathClips;

    // Scripts variables
    private ControllerCharacter2 Enemy1;
    private ControllerCharacter3 Enemy2;
    private ControllerCharacter4 Enemy3;
    private ControllerCharacter5 Boss;
    private DMG_Barrel Explosion;
    private DMG_Spike Spiked;

    // State variables
    private PlayerBaseState _currentState;
    private PlayerStateFactory _states;

    // Getters and Setters
    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }

    void Awake()
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

        // Getting the references
        _charController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
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
        _charController.Move(moveDirection * Time.deltaTime);
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
}
