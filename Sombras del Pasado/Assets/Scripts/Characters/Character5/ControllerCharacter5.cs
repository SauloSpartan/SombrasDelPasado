using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ControllerCharacter5 : MonoBehaviour
{
    //Navegation
    public NavMeshAgent navEnemy;
    public Transform target;
    private bool followTarget = true;

    //References
    private Rigidbody controller;
    private Animator anim;
    private CapsuleCollider enemyCollider;

    //Attack Range and Health
    [SerializeField] private float health;
    public float damage;
    [SerializeField] private float followRadius;
    [SerializeField] private float attackRadius;
    private float attackCoooldown = 0.0f;
    private BoxCollider sword;

    //Animation
    private float velocity = 0.0f;
    private float acceleration = 5.0f;

    //Audio
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] stepClips;
    [SerializeField] private AudioClip[] attackClips;
    [SerializeField] private AudioClip[] deathClips;

    //Other Scripts
    ControllerCharacter1 Player;
    Score Score;
    LevelClear InstancedEnemie;

    void Start()
    {
        Player = FindObjectOfType<ControllerCharacter1>();
        Score = FindObjectOfType<Score>();
        InstancedEnemie = FindObjectOfType<LevelClear>();
        InstancedEnemie.TotalEnemies++;

        navEnemy = GetComponent<NavMeshAgent>();
        controller = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        enemyCollider = GetComponent<CapsuleCollider>();

        sword = GetComponentInChildren<BoxCollider>();

        sword.enabled = false;

        //Optional
        target = PlayerManager.instance.player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (health > 0)
        {
            if (followTarget == true)
            {
                MoveEnemy();
            }
            AttackEnemy();
        }
        if (health <= 0)
        {
            Death();
        }
    }

    private void MoveEnemy()
    {
        //Follow player
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= followRadius && velocity < 1.0f)
        {
            velocity += Time.deltaTime * acceleration;
            Walk();
        }
        else if (distance <= followRadius)
        {
            navEnemy.enabled = true;
            navEnemy.SetDestination(target.position);
            if (distance <= navEnemy.stoppingDistance)
            {
                FacePlayer();
            }
        }
        else if (distance > followRadius && velocity > 0.0f)
        {
            navEnemy.enabled = false;
            velocity -= Time.deltaTime * acceleration;
            Idle();
        }
        else if (velocity < 0.0f)
        {
            velocity = 0.0f;
        }
        else if (velocity > 1.0f)
        {
            velocity = 1.0f;
        }
    }

    private void FacePlayer()
    {
        //Rotate to player
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 12.5f);
    }

    //Gizmos are like the colliders, they can not be seen, but they interact with something
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, followRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
    //You can activate gizmos to be seen

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player Sword")
        {
            health = health - Player.damage;
            Score.score = Score.score + 600;
        }
    }

    private void AttackEnemy()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        if (distance <= attackRadius && attackCoooldown <= 0.0f)
        {
            anim.SetTrigger("Attack1");
            attackCoooldown = 2.5f;
        }
        else if (attackCoooldown > 0.0f)
        {
            attackCoooldown -= Time.deltaTime;
            followTarget = false;
        }
        else if (attackCoooldown <= 0.0f)
        {
            attackCoooldown = 0;
        }

        if (attackCoooldown <= 1.0f)
        {
            followTarget = true;
        }
    }

    private void IsAttacking()
    {
        sword.enabled = true;
    }

    private void NotAttacking()
    {
        sword.enabled = false;
    }

    private void Death()
    {
        followTarget = false;
        anim.SetTrigger("Death");
        sword.enabled = false;
        enemyCollider.enabled = false;
        Score.score = Score.score + 4;
        Destroy(gameObject, 4.5f);
    }

    private void OnDestroy()
    {
        InstancedEnemie.DeadEnemies++;
    }

    private void Idle()
    {
        anim.SetFloat("Speed", velocity);
    }

    private void Walk()
    {
        anim.SetFloat("Speed", velocity);
    }

    private void Step_Sound()
    {
        AudioClip clip = StepClip();
        audioSource.PlayOneShot(clip);
    }

    private void Attack_Sound()
    {
        AudioClip clip = AttackClip();
        audioSource.PlayOneShot(clip);
    }

    private void Death_Sound()
    {
        AudioClip clip = DeathClip();
        audioSource.PlayOneShot(clip);
    }

    private AudioClip StepClip()
    {
        return stepClips[UnityEngine.Random.Range(0, stepClips.Length)];
    }

    private AudioClip AttackClip()
    {
        return attackClips[UnityEngine.Random.Range(0, attackClips.Length)];
    }

    private AudioClip DeathClip()
    {
        return deathClips[UnityEngine.Random.Range(0, deathClips.Length)];
    }
}
