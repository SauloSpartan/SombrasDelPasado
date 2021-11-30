using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    public int buildIndex;
    public AudioMixer audioMixer;

    Score Score;

    void Start()
    {
        buildIndex = SceneManager.GetActiveScene().buildIndex;

        Score = FindObjectOfType<Score>();
    }

    void Update()
    {
        
    }

    public void NewGame()
    {
        SceneManager.LoadScene(buildIndex + 1);
        Score.score = 0;
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

    public void SetVolume (float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }

    public void SetMusic(float volume)
    {
        audioMixer.SetFloat("Music", volume);
    }

    public void SetSound(float volume)
    {
        audioMixer.SetFloat("Sound", volume);
    }

    public void SetQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
}
