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


    Score Score;
    ControllerCharacter2 Enemy1;

    void Start()
    {
        buildIndex = SceneManager.GetActiveScene().buildIndex;

        Score = FindObjectOfType<Score>();
        Enemy1 = FindObjectOfType<ControllerCharacter2>();

        Time.timeScale = 1.0f;

        _sliderGlobalVolume.value = PlayerPrefs.GetFloat("volumeGlobal", 0.5f);
        _sliderMusicVolume.value = PlayerPrefs.GetFloat("volumeMusic", -20.0f);
        _sliderSoundVolume.value = PlayerPrefs.GetFloat("volumeSound", -20.0f);
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
        SceneManager.LoadScene(buildIndex);
        Time.timeScale = 1.0f;
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void MenuReturn()
    {
        SceneManager.LoadScene(buildIndex = 0);
        Time.timeScale = 1.0f;
    }

    public void EasyMode()
    {
        Difficulty = 1;
        SceneManager.LoadScene(buildIndex + 1);
        Score.score = 0;
    }

    public void MediumMode()
    {
        Difficulty = 2;
        SceneManager.LoadScene(buildIndex + 1);
        Score.score = 0;
    }

    public void HardMode()
    {
        Difficulty = 3;
        SceneManager.LoadScene(buildIndex + 1);
        Score.score = 0;
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
