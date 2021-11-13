using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordDamage : MonoBehaviour
{
    ControllerCharacter1 Player;
    ControllerCharacter2 Enemy1;

    void Start()
    {
        Player = FindObjectOfType<ControllerCharacter1>();
        Enemy1 = FindObjectOfType<ControllerCharacter2>();
    }


    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Enemy1.health = Enemy1.health - Player.damage;
        }
    }
}
