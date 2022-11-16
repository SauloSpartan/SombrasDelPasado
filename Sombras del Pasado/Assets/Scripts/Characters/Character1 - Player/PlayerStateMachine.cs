using UnityEngine;

// This is the CONTEXT and stores the data that the concrete states need to be performed
[RequireComponent(typeof(CharacterController), typeof(AudioSource), typeof(Animator))]
public class PlayerStateMachine : MonoBehaviour
{
    // Movement and Rotation variables
    [Header("Movement")]
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _rotationSpeed = 3240;
    private float _moveX;
    private float _moveZ;
    private Vector3 _moveDirection;
    private Vector3 _moveVector;
    private Vector3 _moveRotation;

    // Animation variables
    private Animator _animator;

    // Health and Damage variables
    [Header("Health & Damage")]
    [SerializeField] private float _health = 100f;
    [SerializeField] private float _damage;
    private int _attackCombo = 1;
    private float _maxHealth;
    private bool _canMove = true;
    private int _damagedCount = 0;
    private int _oldDamagedCount;
    private bool _isInvulnerable = false;
    private float _generalTimer;
    private float _timer = 0;

    // Power Ups variables
    private int _defense = 1;
    private int _attack = 1;
    private int _evasion = 0;
    private int _luck;
    private int _powerUp = 0;
    private float _powerTimer;

    // Reference variables
    [Header("Reference")]
    [SerializeField] private GameObject _trailSword;
    [SerializeField] Material[] _mainMaterials = null;
    private CharacterController _charController;
    private BoxCollider _sword;
    private GameObject _powerDefense;
    private GameObject _powerDamage;
    private GameObject _powerVelocity;

    // Audio variables
    [Space]
    [SerializeField] private AudioClip[] _stepClips;
    [SerializeField] private AudioClip[] _attackClips;
    [SerializeField] private AudioClip[] _deathClips;
    private AudioSource _audioSource;

    // Enemy variables
    private EnemyStateMachine _enemy;
    private NewExplosion _barrel;

    // Interface variables
    [Header("Interface")]
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
    public float WalkSpeed { get { return _walkSpeed; } set { _walkSpeed = value; } }
    public float RotationSpeed { get { return _rotationSpeed; } }
    public Vector3 MoveRotation { get { return _moveRotation; } set { _moveRotation = value; } }
    public float Damage { get { return _damage; } set { _damage = value; } }
    public int Attack { get { return _attack; } }
    public int AttackCombo { get { return _attackCombo; } }
    public bool CanMove { get { return _canMove; } set { _canMove = value; } }
    public float Health { get { return _health; } }
    public int DamagedCount { get { return _damagedCount; } set { _damagedCount = value; } }
    public bool IsInvulnerable { get { return _isInvulnerable; } set { _isInvulnerable = value; } }
    public float GeneralTimer { get { return _generalTimer; } set { _generalTimer = value; } }
    public Material[] MainMaterials { get { return _mainMaterials;} set { _mainMaterials = value; } }
    public CharacterController CharController { get { return _charController; } set { _charController = value; } }

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
        _oldDamagedCount = _damagedCount;
        NoEmitionMaterial();
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
        PowerUp();
        DamagedCooldown();
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

    /// <summary>
    /// Function that controls power ups viusals and effects.
    /// </summary>
    private void PowerUp()
    {
        if (_powerTimer > 0.0f && _powerUp == 1) // Defense power up
        {
            _defense = 2;
            _powerDefense.SetActive(true);
            _powerTimer -= Time.deltaTime;
        }
        if (_powerTimer > 0.0f && _powerUp == 2) // Attack power up
        {
            _attack = 2;
            _powerDamage.SetActive(true);
            _powerTimer -= Time.deltaTime;
        }
        if (_powerTimer > 0.0f && _powerUp == 3) // Evasion power up
        {
            _evasion = 1;
            _walkSpeed = 4;
            _powerVelocity.SetActive(true);
            _powerTimer -= Time.deltaTime;
        }
        if (_powerTimer <= 0.0f) // No power up
        {
            _defense = 1;
            _attack = 1;
            _walkSpeed = 3;
            _evasion = 0;

            _powerDefense.SetActive(false);
            _powerDamage.SetActive(false);
            _powerVelocity.SetActive(false);
        }
    }

    /// <summary>
    /// Function that controls the cooldown of the damaged state.
    /// </summary>
    private void DamagedCooldown()
    {
        if (_oldDamagedCount < _damagedCount)
        {
            _timer = 3.0f;
            _oldDamagedCount = _damagedCount;
        }

        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
        }
        else if (_timer <= 0)
        {
            _damagedCount = 0;
            _oldDamagedCount = _damagedCount;
        }
    }

    /// <summary>
    /// Function that disables emition at the beggining of the scene
    /// </summary>
    private void NoEmitionMaterial()
    {
        foreach (Material materials in _mainMaterials)
        {
            materials.DisableKeyword("_EMISSION");
        }
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
        _attackCombo = 4;
        _canMove = false;
    }

    private void ComboEnd()
    {
        _trailSword.SetActive(false);
        _attackCombo = 1;
        _animator.SetInteger("AttackCombo", 0);
        _canMove = true;
    }

    private void Heavy4()
    {
        _trailSword.SetActive(true);
        _attackCombo = 4;
        _canMove = false;
    }

    private void Heavy5()
    {
        _trailSword.SetActive(true);
        _attackCombo = 5;
        _canMove = false;
    }

    private void Heavy6()
    {
        _trailSword.SetActive(true);
        _canMove = false;
    }
    #endregion

    #region Damage Events
    private void Damaged()
    {
        _trailSword.SetActive(false);
        _canMove = false;
    }

    private void DamagedEnd()
    {
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
        // For Enemy Swords
        if (other.tag == "Enemy1 Sword" || other.tag == "Enemy2 Sword" || other.tag == "Enemy3 Dagger" || other.tag == "Enemy4 Sword")
        {
            _luck = Random.Range(0, 4);
            _enemy = other.GetComponentInParent<EnemyStateMachine>(); // Gets the unique copy of that script

            if (_luck == _evasion && _evasion == 1)
            {
                _health -= _enemy.Damage - _enemy.Damage;
            }
            else if (_isInvulnerable == true)
            {
                _health -= _enemy.Damage - _enemy.Damage;
            }
            else
            {
                _health -= _enemy.Damage / _defense;
                StartCoroutine(_camera.CameraShake(0.1f, 1f));
                _currentState = _states.Damage();
                _currentState.EnterState();
            }
        }

        // For Barrel Explsion
        if (other.tag == "Barrel")
        {
            _luck = Random.Range(0, 4);
            _barrel = other.GetComponentInParent<NewExplosion>();
            
            if (_luck == _evasion && _evasion == 1)
            {
                _health -= _barrel.Damage - _barrel.Damage;
            }
            else if (_isInvulnerable == true)
            {
                _health -= _barrel.Damage - _barrel.Damage;
            }
            else
            {
                _health -= _barrel.Damage / _defense;
                _currentState = _states.Damage();
                _currentState.EnterState();
            }
        }

        // For Power Ups
        float timer = 30;
        if (other.tag == "PowerUp Defense")
        {
            _powerTimer = timer;
            _powerUp = 1;
        }
        if (other.tag == "PowerUp Attack")
        {
            _powerTimer = timer;
            _powerUp = 2;
        }
        if (other.tag == "PowerUp Velocity")
        {
            _powerTimer = timer;
            _powerUp = 3;
        }
    }
}
