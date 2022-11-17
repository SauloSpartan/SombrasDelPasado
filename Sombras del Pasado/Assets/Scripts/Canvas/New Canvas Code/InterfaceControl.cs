using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Controls the menu and interface when death or paused
public class InterfaceControl : MonoBehaviour
{
    // Variables
    private int _menuState;
    private float _timeScale;

    // Interface variables
    [Header("Interfaces")]
    [SerializeField] private GameObject _playerInterface;
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _deathInterface;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _settingsMenu;
    [SerializeField] private GameObject _controlsMenu;

    // Button variables
    [Header("Buttons")]
    [SerializeField] private Button _pauseButton;
    [SerializeField] private Button _continueButton;

    // Player variables
    private PlayerStateMachine _player;

    void Awake()
    {
        // Finding objects in Scene
        _playerInterface = GameObject.Find("Player Interface");

        _player = FindObjectOfType<PlayerStateMachine>();

        // Making buttons to work with our code
        _pauseButton.onClick.AddListener(ClickMenu);
        _continueButton.onClick.AddListener(ClickContinue);
    }

    void Start()
    {
        _menuState = 1;
        _timeScale = 1f;

        // Deactivating menu and death
        _mainMenu.SetActive(false);
        _deathInterface.SetActive(false);
        _settingsMenu.SetActive(false);
        _controlsMenu.SetActive(false);
    }

    void Update()
    {
        if (_player.Health >= 1)
        {
            ChangeMenu();
        }

        GameOver();
    }

    private void ChangeMenu()
    {
        AudioSource[] audio = FindObjectsOfType<AudioSource>();
        Time.timeScale = _timeScale;

        if (_menuState == 0 || Input.GetKeyDown(KeyCode.Escape) && _menuState == 1)
        {
            _playerInterface.SetActive(false);
            _mainMenu.SetActive(true);
            _pauseMenu.SetActive(true);
            _settingsMenu.SetActive(false);
            _controlsMenu.SetActive(false);

            _menuState = 3;
            _timeScale = 0.0f;

            foreach (AudioSource sound in audio)
            {
                sound.Pause();
            }
        }
        else if (_menuState == 2 || Input.GetKeyDown(KeyCode.Escape) && _menuState == 3)
        {
            _playerInterface.SetActive(true);
            _mainMenu.SetActive(false);

            _menuState = 1;
            _timeScale = 1.0f;

            foreach (AudioSource sound in audio)
            {
                sound.Play();
            }
        }
    }

    private void GameOver()
    {
        if (_player.Health <= 0)
        {
            _playerInterface.SetActive(false);
            _mainMenu.SetActive(false);
            _deathInterface.SetActive(true);
        }
    }

    #region Mouse Listeners
    private void ClickMenu()
    {
        _menuState = 0;
    }

    private void ClickContinue()
    {
        _menuState = 2;
    }
    #endregion
}
