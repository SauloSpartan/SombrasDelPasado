using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDamage : MonoBehaviour
{
    public int damage;
    ControllerCharacter1 Player;

    void Start()
    {
        Player = FindObjectOfType<ControllerCharacter1>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player.health = Player.health - damage;
        }
    }
}
