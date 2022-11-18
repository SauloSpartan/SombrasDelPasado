using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public int buildIndex;
    public AudioMixer audioMixer;
    public static int Difficulty;

    [SerializeField] private Slider _sliderGlobalVolume;
    [SerializeField] private Slider _sliderMusicVolume;
    [SerializeField] private Slider _sliderSoundVolume;
    [SerializeField] private Dropdown _dropdownQuality;


    private NewScore _theScore;
    private LoadingScreen _loadingScreen;

    private void Awake()
    {
        _loadingScreen = FindObjectOfType<LoadingScreen>();
    }

    void Start()
    {
        buildIndex = SceneManager.GetActiveScene().buildIndex;

        _theScore = FindObjectOfType<NewScore>();

        Time.timeScale = 1.0f;

        _sliderGlobalVolume.value = PlayerPrefs.GetFloat("volumeGlobal", 0.8f);
        _sliderMusicVolume.value = PlayerPrefs.GetFloat("volumeMusic", 0f);
        _sliderSoundVolume.value = PlayerPrefs.GetFloat("volumeSound", 0f);
        AudioListener.volume = _sliderGlobalVolume.value;
        audioMixer.SetFloat("Music", _sliderMusicVolume.value);
        audioMixer.SetFloat("Sound", _sliderSoundVolume.value);

        _dropdownQuality.value = PlayerPrefs.GetInt("quality", 3);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        _loadingScreen.StartLoading(buildIndex, PlayerPrefs.GetInt("TheScore", 0));
        Time.timeScale = 1.0f;
    }

    public void Credits()
    {
        _loadingScreen.StartLoading("Credits");
    }

    public void MenuReturn()
    {
        _loadingScreen.StartLoading(0, PlayerPrefs.GetInt("TheScore", 0));
        PlayerPrefs.SetInt("Saved", SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1.0f;
    }

    public void EasyMode()
    {
        Difficulty = 1;
        _loadingScreen.StartLoading(1, 0);
        PlayerPrefs.SetInt("Saved", 0);
    }

    public void MediumMode()
    {
        Difficulty = 2;
        _loadingScreen.StartLoading(1, 0);
        PlayerPrefs.SetInt("Saved", 0);
    }

    public void HardMode()
    {
        Difficulty = 3;
        _loadingScreen.StartLoading(1, 0);
        PlayerPrefs.SetInt("Saved", 0);
    }

    public void Continue()
    {
        if (PlayerPrefs.GetInt("Saved", 0) == 0)
            return;

        _loadingScreen.StartLoading(PlayerPrefs.GetInt("Saved", 0), PlayerPrefs.GetInt("TheScore", 0));
    }

    public void SetGlobalVolume (float volume)
    {
        PlayerPrefs.SetFloat("volumeGlobal", volume);
        AudioListener.volume = _sliderGlobalVolume.value;
    }

    public void SetMusic(float volume)
    {
        PlayerPrefs.SetFloat("volumeMusic", volume);
        audioMixer.SetFloat("Music", _sliderMusicVolume.value);
    }

    public void SetSound(float volume)
    {
        PlayerPrefs.SetFloat("volumeSound", volume);
        audioMixer.SetFloat("Sound", _sliderSoundVolume.value);
    }

    public void SetQuality ()
    {
        QualitySettings.SetQualityLevel(_dropdownQuality.value);
        PlayerPrefs.SetInt("quality", _dropdownQuality.value);
    }
    
}
