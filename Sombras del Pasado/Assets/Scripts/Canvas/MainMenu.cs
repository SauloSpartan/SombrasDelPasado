using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    public int buildIndex;
    public AudioMixer audioMixer;
    public static int difficulty;

    Score Score;
    ControllerCharacter2 Enemy1;

    void Start()
    {
        buildIndex = SceneManager.GetActiveScene().buildIndex;

        Score = FindObjectOfType<Score>();
        Enemy1 = FindObjectOfType<ControllerCharacter2>();

        Time.timeScale = 1.0f;
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
