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
    private bool _followTarget = true;

    // Animation variables
    private float velocity = 0.0f;
    private float acceleration = 5.0f;
    [SerializeField] private GameObject trailSword;

    // Health and Damage variables
    [SerializeField] private float health;
    public float damage;
    [SerializeField] private float attackRadius;
    [HideInInspector] public float attackDistance;
    private float attackCoooldown = 0.0f;

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

    // Reference variables
    private Animator anim;
    private BoxCollider sword;
    private CapsuleCollider enemyCollider;
    [SerializeField] private Material enemyColor;
    private Color easyColor;
    private Color mediumColor;
    private Color hardColor;

    // Audio variables
    private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _stepClips;
    [SerializeField] private AudioClip[] _attackClips;
    [SerializeField] private AudioClip[] _deathClips;

    // State variables
    private EnemyBaseState _currentState;
    private EnemyStateFactory _states;

    // Getters and Setters
    /// <value> Reference to BaseState Script. </value>
    public EnemyBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public NavMeshAgent NavMesh { get { return _navMesh; } set { _navMesh = value; } }
    public Transform Target { get { return _target; } }

    // Awake is called earlier than Start
    void Awake()
    {
        _navMesh = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        enemyCollider = GetComponent<CapsuleCollider>();
        rigidbodyEnemy = GetComponent<Rigidbody>();

        sword = GetComponentInChildren<BoxCollider>();

        maxHealth = health;
        interfaceEnemy.SetActive(false);
        trailSword.SetActive(false);

        Difficulty();
    }

    void Start()
    {
        // Setup state
        _states = new EnemyStateFactory(this); // "(this)" is a PlayerStateMachine instance
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

    private void Difficulty()
    {
        if (MainMenu.difficulty == 1)
        {
            health = 50;
            damage = 2;
            ColorUtility.TryParseHtmlString("#1C7D68", out easyColor);
            enemyColor.color = easyColor;
        }
        else if (MainMenu.difficulty == 2)
        {
            health = 100;
            damage = 4;
            ColorUtility.TryParseHtmlString("#1C3E7D", out mediumColor);
            enemyColor.color = mediumColor;
        }
        else if (MainMenu.difficulty == 3)
        {
            health = 150;
            damage = 6;
            ColorUtility.TryParseHtmlString("#731C7D", out hardColor);
            enemyColor.color = hardColor;
        }
    }

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
}