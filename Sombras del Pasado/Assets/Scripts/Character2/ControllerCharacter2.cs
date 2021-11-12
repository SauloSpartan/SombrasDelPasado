using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ControllerCharacter2 : MonoBehaviour
{
    public NavMeshAgent navEnemy;
    public Transform target;
    private Rigidbody controller;
    private Animator anim;

    [SerializeField] private float followRadius;

    public float health = 100f;

    void Start()
    {
        navEnemy = GetComponent<NavMeshAgent>();
        controller = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        target = PlayerManager.instance.player.transform;
    }


    void Update()
    {
        enemyNav();
    }

    private void enemyNav()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= followRadius)
        {
            navEnemy.SetDestination(target.position);

            if (distance <= navEnemy.stoppingDistance)
            {
                FacePlayer();
            }
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
}
