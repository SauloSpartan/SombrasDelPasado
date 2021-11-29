using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    public int buildIndex;
    public AudioMixer audioMixer;

    void Start()
    {
        buildIndex = SceneManager.GetActiveScene().buildIndex;
    }

    void Update()
    {
        
    }

    public void NewGame()
    {
        SceneManager.LoadScene(buildIndex + 1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void SetVolume (float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }

    public void SetQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
}
