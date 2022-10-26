using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ControllerCharacter5 : MonoBehaviour
{
    //Navegation
    NavMeshAgent navEnemy;
    Transform target;
    bool followTarget = true;

    //References
    Animator anim;
    BoxCollider sword;
    CapsuleCollider enemyCollider;
    [SerializeField] Material enemyColor;
    Color easyColor;
    Color mediumColor;
    Color hardColor;

    //Attack Range and Health
    [SerializeField] float health;
    public float damage;
    [SerializeField] float followRadius;
    [SerializeField] float attackRadius;
    [HideInInspector] public float attackDistance;
    float attackCoooldown = 0.0f;

    //Animation
    float velocity = 0.0f;
    float acceleration = 5.0f;
    [SerializeField] GameObject trailSword;

    //Audio
    AudioSource audioSource;
    [SerializeField] AudioClip[] stepClips;
    [SerializeField] AudioClip[] attackClips;
    [SerializeField] AudioClip[] deathClips;

    //Health Bar
    [SerializeField] Image healthBar;
    float currentHealth;
    float maxHealth = 100f;
    float lerpSpeed;
    [SerializeField] GameObject interfaceEnemy;
    float healthTimer;

    //Knockback
    [SerializeField] float enemyThrust;
    [SerializeField] float knockTimer;
    Rigidbody rigidbodyEnemy;

    //Other Scripts
    ControllerCharacter1 Player;
    LevelClear InstancedEnemie;
    DMG_Barrel Explosion;

    void Start()
    {
        Player = FindObjectOfType<ControllerCharacter1>();
        Explosion = FindObjectOfType<DMG_Barrel>();

        // Level Clear
        InstancedEnemie = FindObjectOfType<LevelClear>();
        if (InstancedEnemie != null)
        {
            InstancedEnemie.TotalEnemies++;
        }

        navEnemy = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        enemyCollider = GetComponent<CapsuleCollider>();
        rigidbodyEnemy = GetComponent<Rigidbody>();

        sword = GetComponentInChildren<BoxCollider>();

        sword.enabled = false;
        maxHealth = health;
        interfaceEnemy.SetActive(false);
        trailSword.SetActive(false);

        //Optional
        target = PlayerManager.instance.player.transform;

        Difficulty();

        attackDistance = attackRadius;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (health > 0)
        {
            if (followTarget == true)
            {
                FollowPlayer();
            }
            AttackPlayer();
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

    private void FollowPlayer()
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

    private void AttackPlayer()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        if (distance <= attackDistance && attackCoooldown <= 0.0f)
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
        navEnemy.enabled = true;
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
        if (InstancedEnemie != null)
        {
            InstancedEnemie.DeadEnemies++;
        }
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
        if (MainMenu.Difficulty == 1)
        {
            health = 150;
            damage = 10;
            ColorUtility.TryParseHtmlString("#1C7D68", out easyColor);
            enemyColor.color = easyColor;
        }
        else if (MainMenu.Difficulty == 2)
        {
            health = 300;
            damage = 15;
            ColorUtility.TryParseHtmlString("#1C3E7D", out mediumColor);
            enemyColor.color = mediumColor;
        }
        else if (MainMenu.Difficulty == 3)
        {
            health = 450;
            damage = 25;
            ColorUtility.TryParseHtmlString("#731C7D", out hardColor);
            enemyColor.color = hardColor;
        }

        lerpSpeed = 3f * Time.deltaTime;
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, currentHealth / maxHealth, lerpSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player Sword")
        {
            health = health - Player.damage;
            Score.score = Score.score + 600;
            interfaceEnemy.SetActive(true);
            healthTimer = 3.5f;

            attackDistance = 0;
            rigidbodyEnemy.isKinematic = false;
            navEnemy.enabled = false;
            Vector3 difference = rigidbodyEnemy.transform.position - target.transform.position;
            difference = difference.normalized * enemyThrust;
            rigidbodyEnemy.AddForce(difference, ForceMode.Impulse);
            StartCoroutine(KnockBack(rigidbodyEnemy));
        }
        if (other.gameObject.tag == "Barrel")
        {
            health = health - Explosion.damage;
            interfaceEnemy.SetActive(true);
            healthTimer = 3.5f;
        }
    }

    private IEnumerator KnockBack(Rigidbody rigidboyEnemy)
    {
        yield return new WaitForSeconds(knockTimer);
        rigidbodyEnemy.velocity = Vector3.zero;
        rigidbodyEnemy.isKinematic = true;
        navEnemy.enabled = true;
        attackDistance = attackRadius;
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
}
