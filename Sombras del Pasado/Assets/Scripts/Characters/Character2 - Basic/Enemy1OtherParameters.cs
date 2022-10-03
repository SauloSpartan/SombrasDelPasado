using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody), typeof(Collider), typeof(NavMeshAgent))]
[RequireComponent(typeof(AudioSource))]
public class Enemy1OtherParameters : MonoBehaviour
{
    //Audio variables
    private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _stepClips;
    [SerializeField] private AudioClip[] _attackClips;
    [SerializeField] private AudioClip[] _deathClips;

    //Material variables
    [SerializeField] private Material _enemyColor;
    private Color _easyColor;
    private Color _mediumColor;
    private Color _hardColor;

    BoxCollider _sword;

    public float Health;
    public float Damage;

    [SerializeField] private GameObject[] _powerUps;

    [HideInInspector] public NavMeshAgent NavEnemy;
    [HideInInspector] public Rigidbody RigidEnemy;
    private Transform _target;
    private float _enemyThrust = 3;

    ControllerCharacter1 _player;

    private void Start()
    {
        Difficulty();

        _audioSource = GetComponent<AudioSource>();

        _sword = GetComponentInChildren<BoxCollider>();
        _sword.enabled = false;

        NavEnemy = GetComponent<NavMeshAgent>();
        RigidEnemy = GetComponent<Rigidbody>();
        _target = PlayerManager.instance.player.transform;

        _player = FindObjectOfType<ControllerCharacter1>();
    }

    #region Start Methods
    private void Difficulty()
    {
        if (MainMenu.difficulty == 1)
        {
            Health = 50;
            Damage = 2;
            ColorUtility.TryParseHtmlString("#1C7D68", out _easyColor);
            _enemyColor.color = _easyColor;
        }
        else if (MainMenu.difficulty == 2)
        {
            Health = 100;
            Damage = 4;
            ColorUtility.TryParseHtmlString("#1C3E7D", out _mediumColor);
            _enemyColor.color = _mediumColor;
        }
        else if (MainMenu.difficulty == 3)
        {
            Health = 150;
            Damage = 6;
            ColorUtility.TryParseHtmlString("#731C7D", out _hardColor);
            _enemyColor.color = _hardColor;
        }
    }
    #endregion

    #region Damage Recieve Method
    public void DamageRecieve()
    {
        Health = Health - _player.damage;

        Vector3 difference = RigidEnemy.transform.position - _target.transform.position;
        difference = difference.normalized * _enemyThrust;
        RigidEnemy.AddForce(difference, ForceMode.Impulse);
    }
    #endregion

    #region Death Methods
    public void DeathDestroy()
    {
        if (Health <= 0)
        {
            Destroy(gameObject, 4.5f);
        }
    }

    public void PowerUpSpawn(int spawnProbability)
    {
        int randomPower;
        int probabilityPower;

        Vector3 enemyPosition = (transform.position);
        Vector3 powerPosition = new Vector3(enemyPosition.x, enemyPosition.y + 0.7f, enemyPosition.z);
        probabilityPower = Random.Range(0, 100);
        randomPower = Random.Range(0, _powerUps.Length);

        if (probabilityPower <= spawnProbability)
        {
            Instantiate(_powerUps[randomPower], powerPosition, transform.rotation);
        }
    }
    #endregion

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
}
