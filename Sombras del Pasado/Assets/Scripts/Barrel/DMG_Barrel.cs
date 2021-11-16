using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DMG_Barrel : MonoBehaviour
{
    // Start is called before the first frame update
    public int damage;
    //ControllerCharacter1 Player;
        
    void Start()
    {
        //Player = FindObjectOfType<ControllerCharacter1>();
    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        //Player.health = Player.health - damage;
    }
}
