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
    [SerializeField] private float _damage;
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

    // Knockback variables
    [SerializeField] private float enemyThrust;
    [SerializeField] private float knockTimer;
    private Rigidbody rigidbodyEnemy;

    // Animation variables
    private float velocity = 0.0f;
    private float acceleration = 5.0f;
    private Animator _animator;
    [SerializeField] private GameObject _trailSword;

    // Reference variables
    private BoxCollider _sword;
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
    public GameObject TrailSword { get { return _trailSword; } set { _trailSword = value; } }
    public float Health { get { return _health; } }
    public float Damage { get { return _damage; } }

    // Awake is called earlier than Start
    void Awake()
    {
        _navMesh = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _enemyCollider = GetComponent<CapsuleCollider>();
        rigidbodyEnemy = GetComponent<Rigidbody>();

        _sword = GetComponentInChildren<BoxCollider>();

        maxHealth = _health;
        interfaceEnemy.SetActive(false);
        _trailSword.SetActive(false);

        _player = FindObjectOfType<PlayerStateMachine>();

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
        _sword.enabled = false;
        _enemyCollider.enabled = false;
        Destroy(gameObject, 4.5f);
    }

    private void Difficulty()
    {
        if (MainMenu.difficulty == 1)
        {
            _health = 50;
            _damage = 2;
            ColorUtility.TryParseHtmlString("#1C7D68", out _easyColor);
            _enemyColor.color = _easyColor;
        }
        else if (MainMenu.difficulty == 2)
        {
            _health = 100;
            _damage = 4;
            ColorUtility.TryParseHtmlString("#1C3E7D", out _mediumColor);
            _enemyColor.color = _mediumColor;
        }
        else if (MainMenu.difficulty == 3)
        {
            _health = 150;
            _damage = 6;
            ColorUtility.TryParseHtmlString("#731C7D", out _hardColor);
            _enemyColor.color = _hardColor;
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