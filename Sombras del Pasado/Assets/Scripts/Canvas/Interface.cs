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
        menu = GameObject.Find("Main Menu");
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
        if (Player.health >= 1)
        {
            ChangeMenu();
        }
        else if (Player.health <= 0)
        {
            GameOver();
        }
    }

    private void ChangeMenu()
    {
        AudioSource[] audio = FindObjectsOfType<AudioSource>();

        if (menuSet == 0 && Input.GetKeyDown(KeyCode.Escape))
        {
            playerInterface.SetActive(false);
            menu.SetActive(true);
            menuSet = menuSet + 1;
            Time.timeScale = 0.0f;

            foreach (AudioSource sound in audio)
            {
                sound.Pause();
            }
        }
        else if (menuSet == 1 && Input.GetKeyDown(KeyCode.Escape))
        {
            playerInterface.SetActive(true);
            menu.SetActive(false);
            menuSet = menuSet - 1;
            Time.timeScale = 1.0f;

            foreach (AudioSource sound in audio)
            {
                sound.Play();
            }
        }
    }

    private void GameOver()
    {
        playerInterface.SetActive(false);
        menu.SetActive(false);
        deathInterface.SetActive(true);
    }
}
