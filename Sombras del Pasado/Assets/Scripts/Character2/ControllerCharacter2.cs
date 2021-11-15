using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ControllerCharacter2 : MonoBehaviour
{
    //Navegation
    public NavMeshAgent navEnemy;
    public Transform target;

    //References
    private Rigidbody controller;
    private Animator anim;

    //Attack Range and Health
    [SerializeField] private float followRadius;
    [SerializeField] private float health;

    //Animation
    private float velocity = 0.0f;
    [SerializeField] private float acceleration;

    //Other Scripts
    ControllerCharacter1 Player;

    void Start()
    {
        Player = FindObjectOfType<ControllerCharacter1>();

        navEnemy = GetComponent<NavMeshAgent>();
        controller = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        //Optional
        target = PlayerManager.instance.player.transform;
    }


    void Update()
    {
        if (health > 0)
        {
            MoveEnemy();
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
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5.0f);
    }

    //Gizmos are like the colliders, they can not be seen, but they interact with something
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, followRadius);
    }
    //You can activate gizmos to be seen

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player Sword")
        {
            health = health - Player.damage;
        }
    }

    private void Death()
    {
        anim.SetTrigger("Death");
    }

    private void Idle()
    {
        anim.SetFloat("Speed", velocity);
    }

    private void Walk()
    {
        anim.SetFloat("Speed", velocity);
    }
}
