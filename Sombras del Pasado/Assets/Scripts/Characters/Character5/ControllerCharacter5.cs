using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ControllerCharacter5 : MonoBehaviour
{
    //Navegation
    public NavMeshAgent navEnemy;
    [HideInInspector] public Transform target;
    private bool followTarget = true;

    //References
    private Animator anim;
    private BoxCollider sword;
    private CapsuleCollider enemyCollider;
    [SerializeField] private Material enemyColor;
    private Color easyColor;
    private Color mediumColor;
    private Color hardColor;

    //Attack Range and Health
    [SerializeField] private float health;
    public float damage;
    [SerializeField] private float followRadius;
    [SerializeField] private float attackRadius;
    private float attackCoooldown = 0.0f;

    //Animation
    private float velocity = 0.0f;
    private float acceleration = 5.0f;
    [SerializeField] private GameObject trailSword;

    //Audio
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] stepClips;
    [SerializeField] private AudioClip[] attackClips;
    [SerializeField] private AudioClip[] deathClips;

    //Health Bar
    [SerializeField] private Image healthBar;
    private float currentHealth;
    private float maxHealth = 100f;
    private float lerpSpeed;
    [SerializeField] private GameObject interfaceEnemy;
    private float healthTimer;

    //Other Scripts
    ControllerCharacter1 Player;
    LevelClear InstancedEnemie;
    DMG_Barrel Explosion;

    void Start()
    {
        Player = FindObjectOfType<ControllerCharacter1>();
        Explosion = FindObjectOfType<DMG_Barrel>();
        InstancedEnemie = FindObjectOfType<LevelClear>();
        InstancedEnemie.TotalEnemies++;

        navEnemy = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        enemyCollider = GetComponent<CapsuleCollider>();

        sword = GetComponentInChildren<BoxCollider>();

        sword.enabled = false;
        maxHealth = health;
        interfaceEnemy.SetActive(false);
        trailSword.SetActive(false);

        //Optional
        target = PlayerManager.instance.player.transform;

        Difficulty();
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
        HealthControl();

        lerpSpeed = 3f * Time.deltaTime;

        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, currentHealth / maxHealth, lerpSpeed);

        ColorChanger();
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

    private void AttackEnemy()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        if (distance <= attackRadius && attackCoooldown <= 0.0f)
        {
            anim.SetTrigger("Attack1");
            attackCoooldown = 4.0f;
            trailSword.SetActive(true);
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

        if (attackCoooldown <= 2.0f)
        {
            followTarget = true;
        }
        if (attackCoooldown <= 0.5f)
        {
            trailSword.SetActive(false);
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
        interfaceEnemy.SetActive(false);
        Destroy(gameObject, 4.5f);
    }

    private void HealthControl()
    {
        currentHealth = health;
        healthBar.fillAmount = currentHealth / maxHealth;
        healthTimer -= Time.deltaTime;
        if (healthTimer <= 0)
            interfaceEnemy.SetActive(false);
    }

    private void ColorChanger()
    {
        Color healthColor = Color.Lerp(Color.red, Color.green, (currentHealth / maxHealth));

        healthBar.color = healthColor;
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

    private void Difficulty()
    {
        if (MainMenu.difficulty == 1)
        {
            health = 150;
            damage = 10;
            ColorUtility.TryParseHtmlString("#1C7D68", out easyColor);
            enemyColor.color = easyColor;
        }
        else if (MainMenu.difficulty == 2)
        {
            health = 300;
            damage = 15;
            ColorUtility.TryParseHtmlString("#1C3E7D", out mediumColor);
            enemyColor.color = mediumColor;
        }
        else if (MainMenu.difficulty == 3)
        {
            health = 450;
            damage = 25;
            ColorUtility.TryParseHtmlString("#731C7D", out hardColor);
            enemyColor.color = hardColor;
        }
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
            interfaceEnemy.SetActive(true);
            healthTimer = 3.5f;
        }
        if (other.gameObject.tag == "Barrel")
        {
            health = health - Explosion.damage;
            interfaceEnemy.SetActive(true);
            healthTimer = 3.5f;
        }
    }
}
