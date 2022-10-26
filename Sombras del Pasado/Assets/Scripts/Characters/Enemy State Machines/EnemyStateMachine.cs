using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

// This is the CONTEXT and stores the data that the concrete states need to be performed
public class EnemyStateMachine : MonoBehaviour
{
    // Navigation variables
    private NavMeshAgent _navMesh;
    private Transform _target;
    private bool _followTarget = false;
    [SerializeField] float _stopRadius;
    private int _randomDesition;
    [SerializeField] float _escapeRadius;
    [SerializeField] private Transform _escapePosition;

    // Health and Damage variables
    [SerializeField] private float _attackRadius;
    [SerializeField] private float _health;
    private float _baseHealth;
    [SerializeField] private float _damage;
    private float _baseDamage;
    private float _generalCooldown;

    // Power Ups variables
    [SerializeField] private GameObject[] powerUps;
    private int randomPower;
    private int amount = 1;
    private int probabilityPower;

    // Health Bar variables
    [SerializeField] private Image healthBar;
    private float currentHealth;
    private float maxHealth = 100f;
    private float lerpSpeed;
    [SerializeField] private GameObject interfaceEnemy;
    private float healthTimer;

    // Animation variables
    private float velocity = 0.0f;
    private float acceleration = 5.0f;
    private Animator _animator;

    // Reference variables
    [SerializeField] private BoxCollider _swordOne = null;
    [SerializeField] private BoxCollider _swordTwo = null;
    [SerializeField] private GameObject _trailSwordOne = null;
    [SerializeField] private GameObject _trailSwordTwo = null;
    private CapsuleCollider _enemyCollider;
    [SerializeField] private Material _enemyColor;
    private Color _easyColor;
    private Color _mediumColor;
    private Color _hardColor;

    // Audio variables
    private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _stepClips;
    [SerializeField] private AudioClip[] _attackClips;
    [SerializeField] private AudioClip[] _deathClips;

    // Player variables
    PlayerStateMachine _player;

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
    public float EscapeRadius { get { return _escapeRadius; } }
    public Transform EscapePosition { get { return _escapePosition; } }
    public float AttackRadius { get { return _attackRadius; } }
    public float GeneralCooldown { get { return _generalCooldown; } set { _generalCooldown = value; } }
    public Animator Animator { get { return _animator; } }
    public GameObject TrailSwordOne { get { return _trailSwordOne; } set { _trailSwordOne = value; } }
    public GameObject TrailSwordTwo { get { return _trailSwordTwo; } set { _trailSwordTwo = value; } }
    public float Health { get { return _health; } }
    public float Damage { get { return _damage; } }

    // Awake is called earlier than Start
    void Awake()
    {
        _navMesh = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _enemyCollider = GetComponent<CapsuleCollider>();

        maxHealth = _health;
        interfaceEnemy.SetActive(false);

        if (_trailSwordOne != null) // It means that doesn't require and argument
        {
            _trailSwordOne.SetActive(false);
        }
        if (_trailSwordTwo != null)
        {
            _trailSwordTwo.SetActive(false);
        }

        _player = FindObjectOfType<PlayerStateMachine>();

        EnemyVariables();
        Difficulty();
    }

    void Start()
    {
        // Setup state
        _states = new EnemyStateFactory(this); // "(this)" is a EnemyStateMachine instance
        _currentState = _states.Walk();
        _currentState.EnterState();

        // Optional
        _target = PlayerManager.instance.player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        _currentState.UpdateState();
    }

    public void Death()
    {
        _navMesh.isStopped = true;
        _animator.SetTrigger("Death");
        _swordOne.enabled = false;
        _enemyCollider.enabled = false;
        Destroy(gameObject, 4.5f);
    }

    /// <summary>
    /// Function that manages the base variables enemies use depending on tag.
    /// </summary>
    private void EnemyVariables()
    {
        if (gameObject.tag == "Enemy Basic")
        {
            _baseHealth = 100;
            _baseDamage = 4;
            _attackRadius = 1.5f;
        }
        if (gameObject.tag == "Enemy Heavy")
        {
            _baseHealth = 200;
            _baseDamage = 10;
            _attackRadius = 2f;
        }
        if (gameObject.tag == "Enemy Fast")
        {
            _baseHealth = 80;
            _baseDamage = 2;
            _attackRadius = 1.2f;
        }
        if (gameObject.tag == "Enemy Boss")
        {
            _baseHealth = 300;
            _baseDamage = 20;
            _attackRadius = 1.5f;
        }
    }

    /// <summary>
    /// Function that manages the variables used depending on the difficulty.
    /// </summary>
    private void Difficulty()
    {
        if (MainMenu.Difficulty == 1)
        {
            _health = _baseHealth / 2;
            _damage = _baseDamage / 2;
            ColorUtility.TryParseHtmlString("#1C7D68", out _easyColor);
            _enemyColor.color = _easyColor;
        }
        else if (MainMenu.Difficulty == 2)
        {
            _health = _baseHealth;
            _damage = _baseDamage;
            ColorUtility.TryParseHtmlString("#1C3E7D", out _mediumColor);
            _enemyColor.color = _mediumColor;
        }
        else if (MainMenu.Difficulty == 3)
        {
            _health = _baseHealth + (_baseHealth / 2);
            _damage = _baseDamage + (_baseDamage / 2);
            ColorUtility.TryParseHtmlString("#731C7D", out _hardColor);
            _enemyColor.color = _hardColor;
        }
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