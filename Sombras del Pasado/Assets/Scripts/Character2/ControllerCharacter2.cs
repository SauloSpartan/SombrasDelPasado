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
    public float health = 100f;

    //Animation
    private float velocity;
    [SerializeField] private float acceleration;

    void Start()
    {
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
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= followRadius)
        {
            anim.SetFloat("Speed", 1.0f, 0.1f, Time.deltaTime);
            navEnemy.SetDestination(target.position);

            if (distance <= navEnemy.stoppingDistance)
            {
                FacePlayer();
            }
        }
        if (distance > followRadius)
        {
            anim.SetFloat("Speed", 0.0f, 1.0f, Time.deltaTime);
        }
    }

    private void FacePlayer()
    {
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

    private void Death()
    {
        anim.SetTrigger("Death");
    }
}
