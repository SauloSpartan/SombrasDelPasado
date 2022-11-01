using UnityEngine;

// This is the CONTEXT and stores the data that the concrete states need to be performed
public class PlayerStateMachine : MonoBehaviour
{
    // Movement and Rotation variables
    private float _moveX;
    private float _moveZ;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _rotationSpeed = 1080;
    private Vector3 _moveDirection;
    private Vector3 _moveVector;
    private Vector3 _moveRotation;

    // Animation variables
    private Animator _animator;

    // Health and Damage variables
    [SerializeField] private float _health = 100f;
    private float _maxHealth;
    [SerializeField] private int _damage;
    private int _defense = 1;
    private int _attack = 1;
    private int luck;
    private int evasion = 0;
    private int _attackCombo = 1;
    private float powerTimer;
    private int powerUp = 0;
    private bool _canMove = true;

    // Reference variables
    [SerializeField] private GameObject _trailSword;
    private CharacterController _charController;
    private BoxCollider _sword;
    private GameObject _powerDefense;
    private GameObject _powerDamage;
    private GameObject _powerVelocity;

    // Audio variables
    private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _stepClips;
    [SerializeField] private AudioClip[] _attackClips;
    [SerializeField] private AudioClip[] _deathClips;

    // Enemy variables
    private EnemyStateMachine _enemy;

    // Interface variables
    [SerializeField] private HealthControl _healthBar;

    // Camera variables
    private CameraControl _camera;

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
        _powerDefense = GameObject.Find("Power Defense");
        _powerDamage = GameObject.Find("Power Damage");
        _powerVelocity = GameObject.Find("Power Velocity");

        // Getting the references
        _charController = GetComponent<CharacterController>();
        _camera = FindObjectOfType<CameraControl>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _sword = GameObject.Find("Espada Allard").GetComponent<BoxCollider>();

        // Deactivating visual or other stuff
        _sword.enabled = false;
        _powerDefense.SetActive(false);
        _powerDamage.SetActive(false);
        _powerVelocity.SetActive(false);
        _trailSword.SetActive(false);

        // Initial variables
        _maxHealth = _health;
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
        // Setup state
        _currentState.UpdateState();

        // Functions and code needed
        Gravity();
        _charController.Move(_moveDirection * _walkSpeed * Time.deltaTime); // It moves the character
        if (_healthBar != null) // Takes out dependance, verifying that reference is not null
        {
            _healthBar.HealthBarControl(_health, _maxHealth);
        }
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
        _sword.enabled = true;
    }

    private void NotAttacking()
    {
        _sword.enabled = false;
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
        _audioSource.PlayOneShot(clip);
    }

    private void Attack_Sound()
    {
        AudioClip clip = AttackClip();
        _audioSource.PlayOneShot(clip);
    }

    private void Death_Sound()
    {
        AudioClip clip = DeathClip();
        _audioSource.PlayOneShot(clip);
    }

    private AudioClip StepClip()
    {
        return _stepClips[UnityEngine.Random.Range(0, _stepClips.Length)];
    }

    private AudioClip AttackClip()
    {
        return _attackClips[UnityEngine.Random.Range(0, _attackClips.Length)];
    }

    private AudioClip DeathClip()
    {
        return _deathClips[UnityEngine.Random.Range(0, _deathClips.Length)];
    }
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy1 Sword" || other.tag == "Enemy2 Sword" || other.tag == "Enemy3 Dagger" || other.tag == "Enemy4 Sword")
        {
            _enemy = other.GetComponentInParent<EnemyStateMachine>(); // Gets the unique copy of that script
            _health -= _enemy.Damage / _defense;
            StartCoroutine(_camera.CameraShake(0.1f, 1f));
        }
    }
}
