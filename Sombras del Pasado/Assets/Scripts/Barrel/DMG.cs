using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DMG : MonoBehaviour
{
    // Start is called before the first frame update
    public int damage;
    ControllerCharacter1 Player;

    void Start()
    {
        Player = FindObjectOfType<ControllerCharacter1>();
    }

    void Update()
    {

    }

    //Es un trigger "Stay" porque el jugador ya se encuentra dentro del trigger
    private void OnTriggerStay(Collider other)
    {
        Player.health = Player.health - damage;
    }
}
