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
    public static int difficulty;

    [SerializeField] private Slider sliderGlobalVolume;
    [SerializeField] private Slider sliderMusicVolume;
    [SerializeField] private Slider sliderSoundVolume;
    [SerializeField] private Dropdown dropdownQuality;


    Score Score;
    ControllerCharacter2 Enemy1;

    void Start()
    {
        buildIndex = SceneManager.GetActiveScene().buildIndex;

        Score = FindObjectOfType<Score>();
        Enemy1 = FindObjectOfType<ControllerCharacter2>();

        Time.timeScale = 1.0f;

        sliderGlobalVolume.value = PlayerPrefs.GetFloat("volumeGlobal", 0.5f);
        sliderMusicVolume.value = PlayerPrefs.GetFloat("volumeMusic", -20.0f);
        sliderSoundVolume.value = PlayerPrefs.GetFloat("volumeSound", -20.0f);
        AudioListener.volume = sliderGlobalVolume.value;
        audioMixer.SetFloat("Music", sliderMusicVolume.value);
        audioMixer.SetFloat("Sound", sliderSoundVolume.value);

        dropdownQuality.value = PlayerPrefs.GetInt("quality", 3);
    }

    void Update()
    {
        
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
        difficulty = 1;
        SceneManager.LoadScene(buildIndex + 1);
        Score.score = 0;
    }

    public void MediumMode()
    {
        difficulty = 2;
        SceneManager.LoadScene(buildIndex + 1);
        Score.score = 0;
    }

    public void HardMode()
    {
        difficulty = 3;
        SceneManager.LoadScene(buildIndex + 1);
        Score.score = 0;
    }

    public void SetGlobalVolume (float volume)
    {
        PlayerPrefs.SetFloat("volumeGlobal", volume);
        AudioListener.volume = sliderGlobalVolume.value;
    }

    public void SetMusic(float volume)
    {
        PlayerPrefs.SetFloat("volumeMusic", volume);
        audioMixer.SetFloat("Music", sliderMusicVolume.value);
    }

    public void SetSound(float volume)
    {
        PlayerPrefs.SetFloat("volumeSound", volume);
        audioMixer.SetFloat("Sound", sliderSoundVolume.value);
    }

    public void SetQuality ()
    {
        QualitySettings.SetQualityLevel(dropdownQuality.value);
        PlayerPrefs.SetInt("quality", dropdownQuality.value);
    }
    
}
