using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ControllerCharacter4 : MonoBehaviour
{
    //Navegation
    public NavMeshAgent navEnemy;
    public Transform target;
    private bool followTarget = true;

    //References
    private Animator anim;
    private CapsuleCollider enemyCollider;
    public BoxCollider daggerRight;
    public BoxCollider daggerLeft;
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
    [SerializeField] private GameObject trailDaggerR;
    [SerializeField] private GameObject trailDaggerL;

    //Audio
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] stepClips;
    [SerializeField] private AudioClip[] attackClips;
    [SerializeField] private AudioClip[] deathClips;

    //Power Ups
    [SerializeField] private GameObject[] powerUps;
    private int randomPower;
    private int amount = 1;
    private int probabilityPower;

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

        daggerRight.enabled = false;
        daggerLeft.enabled = false;
        maxHealth = health;
        interfaceEnemy.SetActive(false);
        trailDaggerR.SetActive(false);
        trailDaggerL.SetActive(false);

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
            trailDaggerR.SetActive(true);
            trailDaggerL.SetActive(true);
            anim.SetTrigger("Attack1");
            attackCoooldown = 2.0f;
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
        if (attackCoooldown <= 0.3f)
        {
            trailDaggerR.SetActive(false);
            trailDaggerL.SetActive(false);
        }
    }

    private void IsAttacking()
    {
        daggerRight.enabled = true;
        daggerLeft.enabled = true;
    }

    private void NotAttacking()
    {
        daggerRight.enabled = false;
        daggerLeft.enabled = false;
    }

    private void Death()
    {
        followTarget = false;
        anim.SetTrigger("Death");
        daggerRight.enabled = false;
        daggerLeft.enabled = false;
        enemyCollider.enabled = false;
        Score.score = Score.score + 3;
        PowerUp();
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
            health = 40;
            damage = 1;
            ColorUtility.TryParseHtmlString("#1C7D68", out easyColor);
            enemyColor.color = easyColor;
        }
        else if (MainMenu.difficulty == 2)
        {
            health = 80;
            damage = 2;
            ColorUtility.TryParseHtmlString("#1C3E7D", out mediumColor);
            enemyColor.color = mediumColor;
        }
        else if (MainMenu.difficulty == 3)
        {
            health = 120;
            damage = 3;
            ColorUtility.TryParseHtmlString("#731C7D", out hardColor);
            enemyColor.color = hardColor;
        }
    }

    private void PowerUp()
    {
        if (amount == 1)
        {
            Vector3 enemyPosition = (transform.position);
            Vector3 powerPosition = new Vector3(enemyPosition.x, enemyPosition.y + 0.7f, enemyPosition.z);
            amount = 0;
            probabilityPower = Random.Range(0, 100);
            randomPower = Random.Range(0, powerUps.Length);

            if (probabilityPower <= 12 || probabilityPower >= 88)
                Instantiate(powerUps[randomPower], powerPosition, transform.rotation);
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
            Score.score = Score.score + 450;
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
