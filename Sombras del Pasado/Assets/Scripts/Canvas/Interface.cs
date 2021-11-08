using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interface : MonoBehaviour
{
    //Variables
    private int menuSet = 0;
    private GameObject playerInterface;
    private GameObject menu;
    private GameObject deathInterface;

    ControllerCharacter1 Player;

    void Start()
    {
        //Finding objects in Scene
        playerInterface = GameObject.Find("Player Interface");
        menu = GameObject.Find("Menu");
        deathInterface = GameObject.Find("Death Interface");
        Player = FindObjectOfType<ControllerCharacter1>();

        //Deactivating menu and death
        menu.SetActive(false);
        deathInterface.SetActive(false);

        //It is necessary to first have them active to make them inactive,
        //if not they can not be referenced
    }

    void Update()
    {
        ChangeMenu();
    }

    private void ChangeMenu()
    {
        //If the player is alive he can open and close the menu
        if (Player.health >= 1)
        {
            if (menuSet == 0 && Input.GetKeyDown(KeyCode.Escape))
            {
                playerInterface.SetActive(false);
                menu.SetActive(true);
                menuSet = menuSet + 1;
            }
            else if (menuSet == 1 && Input.GetKeyDown(KeyCode.Escape))
            {
                playerInterface.SetActive(true);
                menu.SetActive(false);
                menuSet = menuSet - 1;
            }
        }
        //If the player dies the death interface shows up
        else if (Player.health <= 0)
        {
            playerInterface.SetActive(false);
            menu.SetActive(false);
            deathInterface.SetActive(true);
        }
    }
}
