using System.Collections;
using UnityEngine;
using UnityEngine.AI;

// This is the CONTEXT and stores the data that the concrete states need to be performed
public class EnemyStateMachine : MonoBehaviour
{
    // Navigation variables
    [Header("Navigation")]
    [SerializeField] float _stopRadius;
    [SerializeField] float _escapeRadius;
    [SerializeField] private Transform _escapePosition;
    private NavMeshAgent _navMesh;
    private Transform _target;
    private bool _followTarget = false;
    private int _randomDesition;
    private float _initialSpeed;

    // Health and Damage variables
    [Header("Health & Damage")]
    [SerializeField] private float _attackRadius;
    [SerializeField] private float _health;
    [SerializeField] private float _damage;
    private float _generalCooldown;
    private float _maxHealth;
    private float _baseHealth;
    private float _baseDamage;
    private bool _canMove = true;
    private string _enemyType;

    // Animation variables
    private Animator _animator;

    // Reference variables
    [Header("Reference")]
    [SerializeField] private BoxCollider _swordOne = null;
    [SerializeField] private BoxCollider _swordTwo = null;
    [SerializeField] private GameObject _trailSwordOne = null;
    [SerializeField] private GameObject _trailSwordTwo = null;
    [SerializeField] private Material _enemyColor;
    private CapsuleCollider _enemyCollider;
    private Color _easyColor;
    private Color _mediumColor;
    private Color _hardColor;

    // Power Ups variables
    [Space]
    [SerializeField] private GameObject[] _powerUps;
    private int _powerAmount = 1;

    // Audio variables
    [Space]
    [SerializeField] private AudioClip[] _stepClips;
    [SerializeField] private AudioClip[] _attackClips;
    [SerializeField] private AudioClip[] _deathClips;
    private AudioSource _audioSource;

    // Player variables
    private PlayerStateMachine _player;
    private NewScore _score;

    // Interface variables
    [Header("Interface")]
    [SerializeField] private GameObject _interfaceEnemy;
    [SerializeField] private HealthControl _healthBar;
    [SerializeField] private GameObject _enemyObject;
    private int _hierarchyIndex;

    // Spawn variables
    private SpawnEnemy _spawnEnemy;
    private NewNextLevel _nextLevel;

    // State variables
    private EnemyBaseState _currentState;
    private EnemyStateFactory _states;

    // Getters and Setters
    /// <value> Reference to BaseState Script. </value>
    public EnemyBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public NavMeshAgent NavMesh { get { return _navMesh; } set { _navMesh = value; } }
    public Transform Target { get { return _target; } }
    public bool FollowTarget { get { return _followTarget; } set { _followTarget = value; } }
    public float StopRadius { get { return _stopRadius; } }
    public int RandomDesition { get { return _randomDesition; } set { _randomDesition = value; } }
    public float InitialSpeed { get { return _initialSpeed; } }
    public float EscapeRadius { get { return _escapeRadius; } }
    public Transform EscapePosition { get { return _escapePosition; } }
    public float AttackRadius { get { return _attackRadius; } }
    public float GeneralCooldown { get { return _generalCooldown; } set { _generalCooldown = value; } }
    public Animator Animator { get { return _animator; } }
    public GameObject TrailSwordOne { get { return _trailSwordOne; } set { _trailSwordOne = value; } }
    public GameObject TrailSwordTwo { get { return _trailSwordTwo; } set { _trailSwordTwo = value; } }
    public float Health { get { return _health; } }
    public float Damage { get { return _damage; } }
    public bool CanMove { get { return _canMove; } set { _canMove = value; } }
    public string EnemyType { get { return _enemyType; } }

    // Awake is called earlier than Start
    void Awake()
    {
        _navMesh = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _enemyCollider = GetComponent<CapsuleCollider>();

        _player = FindObjectOfType<PlayerStateMachine>();
        _score = FindObjectOfType<NewScore>();
        _nextLevel = FindObjectOfType<NewNextLevel>();

        // Initial variables
        _interfaceEnemy.SetActive(false);

        if (_trailSwordOne != null) // It means that doesn't require and argument
        {
            _trailSwordOne.SetActive(false);
        }
        if (_trailSwordTwo != null)
        {
            _trailSwordTwo.SetActive(false);
        }

        EnemyVariables();
        Difficulty();

        _maxHealth = _health;
        _initialSpeed = _navMesh.speed;
    }

    void Start()
    {
        // Setup state
        _states = new EnemyStateFactory(this); // "(this)" is a EnemyStateMachine instance
        _currentState = _states.Walk();
        _currentState.EnterState();

        // Optional
        _target = PlayerManager.instance.player.transform;
        _spawnEnemy = GetComponentInParent<SpawnEnemy>();

        if (_swordOne != null)
        {
            _swordOne.enabled = false;
        }
        if (_swordTwo != null)
        {
            _swordTwo.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        _currentState.UpdateState();

        // Functions and code needed
        if (_healthBar != null) // Takes out dependance, verifying that reference is not null
        {
            _healthBar.HealthBarControl(_health, _maxHealth);
        }
    }

    /// <summary>
    /// Function that disables variables when death.
    /// </summary>
    public void Death()
    {
        _navMesh.isStopped = true;
        _animator.SetTrigger("Death");
        _swordOne.enabled = false;
        _enemyCollider.enabled = false;
        PowerUp();
        _spawnEnemy.EnemyDeath++;
        Destroy(this.gameObject, 4.5f);
    }

    /// <summary>
    /// Function that manages the base variables enemies use depending on tag.
    /// </summary>
    private void EnemyVariables()
    {
        if (gameObject.tag == "Enemy Basic")
        {
            _baseHealth = 50;
            _baseDamage = 4;
            _attackRadius = 1.5f;
            _enemyType = "Enemy Basic";
        }
        if (gameObject.tag == "Enemy Heavy")
        {
            _baseHealth = 100;
            _baseDamage = 10;
            _attackRadius = 2f;
            _enemyType = "Enemy Heavy";
        }
        if (gameObject.tag == "Enemy Fast")
        {
            _baseHealth = 50;
            _baseDamage = 6;
            _attackRadius = 1.2f;
            _enemyType = "Enemy Fast";
        }
        if (gameObject.tag == "Enemy Boss")
        {
            _baseHealth = 350;
            _baseDamage = 15;
            _attackRadius = 1.5f;
            _enemyType = "Enemy Boss";
        }
    }

    /// <summary>
    /// Function that manages the variables used depending on the difficulty.
    /// </summary>
    private void Difficulty()
    {
        if (MainMenu.Difficulty == 1) // Easy
        {
            _health = _baseHealth / 2;
            _damage = _baseDamage / 2;
            ColorUtility.TryParseHtmlString("#1C7D68", out _easyColor);
            _enemyColor.color = _easyColor;
        }
        else if (MainMenu.Difficulty == 2) // Normal
        {
            _health = _baseHealth;
            _damage = _baseDamage;
            ColorUtility.TryParseHtmlString("#1C3E7D", out _mediumColor);
            _enemyColor.color = _mediumColor;
        }
        else if (MainMenu.Difficulty == 3) // Hard
        {
            _health = _baseHealth * 2;
            _damage = _baseDamage * 2;
            ColorUtility.TryParseHtmlString("#731C7D", out _hardColor);
            _enemyColor.color = _hardColor;
        }
    }

    /// <summary>
    /// Function that controls power up spawn when enemies die.
    /// </summary>
    private void PowerUp()
    {
        if (_powerAmount == 1)
        {
            Vector3 enemyPosition = (transform.position);
            Vector3 powerPosition = new Vector3(enemyPosition.x, enemyPosition.y + 0.7f, enemyPosition.z);
            _powerAmount = 0;
            int probabilityPower = Random.Range(0, 100);
            int randomPower = Random.Range(0, _powerUps.Length);

            if (probabilityPower <= 10)
            {
                Instantiate(_powerUps[randomPower], powerPosition, transform.rotation);
            }
        }
    }

    /// <summary>
    /// Coroutine that controls enemy health bar showing up.
    /// </summary>
    /// <returns> Returns the time before hiding again.</returns>
    private IEnumerator HealthTimer()
    {
        _interfaceEnemy.SetActive(true);
        _hierarchyIndex = 2;
        _enemyObject.transform.SetSiblingIndex(_hierarchyIndex);
        yield return new WaitForSecondsRealtime(2f);
        _interfaceEnemy.SetActive(false);
    }

    #region Attack Events
    private void IsAttacking()
    {
        if (_swordOne != null)
        {
            _swordOne.enabled = true;
        }
        if (_swordTwo != null)
        {
            _swordTwo.enabled = true;
        }
    }

    private void NotAttacking()
    {
        if (_swordOne != null)
        {
            _swordOne.enabled = false;
        }
        if (_swordTwo != null)
        {
            _swordTwo.enabled = false;
        }
    }
    #endregion

    #region Damage Events
    private void Damaged()
    {
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
        if (other.gameObject.tag == "Player Sword")
        {
            _health -= _player.Damage;
            StartCoroutine(HealthTimer());
            _score.ScoreCount++;
            _currentState = _states.Damage();
            _currentState.EnterState();
        }
    }

    private void OnDestroy()
    {
        if (gameObject.tag == "Enemy Boss")
        {
            _nextLevel.NextLevel();
        }
    }

    // Gizmos are like the colliders, they can not be seen, but they interact with something
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _stopRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _escapeRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRadius);
    }
}