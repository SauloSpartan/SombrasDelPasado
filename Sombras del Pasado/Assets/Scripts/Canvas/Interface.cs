using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interface : MonoBehaviour
{
    //Variables
    private int menuSet;
    private float _timeScale;
    private GameObject playerInterface;
    private GameObject menu;
    private GameObject deathInterface;
    [SerializeField] private Button menuButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _settingsMenu;
    [SerializeField] private GameObject _controlsMenu;

    ControllerCharacter1 Player;

    void Start()
    {
        //Finding objects in Scene
        playerInterface = GameObject.Find("Player Interface");
        menu = GameObject.Find("Main Menu");
        deathInterface = GameObject.Find("Death Interface");
        Player = FindObjectOfType<ControllerCharacter1>();

        menuButton.onClick.AddListener(ClickMenu);
        continueButton.onClick.AddListener(ClickContinue);

        //Deactivating menu and death
        menu.SetActive(false);
        deathInterface.SetActive(false);

        //It is necessary to first have them active to make them inactive,
        //if not they can not be referenced

        _timeScale = 1.0f;
        menuSet = 0;
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
        Time.timeScale = _timeScale;
    }

    private void ClickMenu()
    {
        menuSet = 3;
    }

    private void ClickContinue()
    {
        menuSet = 4;
    }

    private void ChangeMenu()
    {
        AudioSource[] audio = FindObjectsOfType<AudioSource>();

        if (menuSet == 0 && Input.GetKeyDown(KeyCode.Escape) || menuSet == 3)
        {
            playerInterface.SetActive(false);
            menu.SetActive(true);
            menuSet = 1;
            _timeScale = 0.0f;
            _pauseMenu.SetActive(true);
            _settingsMenu.SetActive(false);
            _controlsMenu.SetActive(false);

            foreach (AudioSource sound in audio)
            {
                sound.Pause();
            }
        }
        else if (menuSet == 1 && Input.GetKeyDown(KeyCode.Escape) || menuSet == 4)
        {
            playerInterface.SetActive(true);
            menu.SetActive(false);
            menuSet = 0;
            _timeScale = 1.0f;

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
