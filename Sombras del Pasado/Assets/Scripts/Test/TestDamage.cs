using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDamage : MonoBehaviour
{
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
        Player.health = Player.health - 20;
    }
}
