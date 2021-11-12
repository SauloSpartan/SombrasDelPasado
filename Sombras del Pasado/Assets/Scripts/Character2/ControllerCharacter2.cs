using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ControllerCharacter2 : MonoBehaviour
{
    public NavMeshAgent navEnemy;
    public Transform player;
    private Rigidbody controller;
    private Animator anim;

    void Start()
    {
        controller = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        enemyNav();
    }

    private void enemyNav()
    {
        navEnemy.SetDestination(player.position);
    }
}
