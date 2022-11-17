using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    private Slider _percentage;
    private GameObject _screen;

    void Start()
    {
        _screen = GameObject.Find("Loading");
        _percentage = _screen.GetComponentInChildren<Slider>();
        _screen.SetActive(false);
    }

    /// <summary>
    /// Start loading de scene and the loading screen
    /// </summary>
    /// <param name="scene">Will load the scene based on the String</param>
    public void StartLoading(string scene)
    {
        _screen.SetActive(true);
        StartCoroutine(SceneString(scene));
    }

    /// <summary>
    /// Start loading de scene and the loading screen
    /// </summary>
    /// <param name="scene">Will load the scene based on the Int</param>
    public void StartLoading(int scene, int score)
    {
        _screen.SetActive(true);
        PlayerPrefs.SetInt("TheScore", score);
        StartCoroutine(SceneInt(scene));
    }

    /// <summary>
    /// Start loading the scene
    /// </summary>
    /// <returns>Null</returns>
    IEnumerator SceneString(string scene)
    {
        AsyncOperation Load = SceneManager.LoadSceneAsync(scene);
        
        while (!Load.isDone)
        {
            float Progres = Mathf.Clamp01(Load.progress / 0.9f);
            _percentage.value = Progres;
            yield return null;
        }
    }

    /// <summary>
    /// Start loading the scene
    /// </summary>
    /// <returns>Null</returns>
    IEnumerator SceneInt(int scene)
    {
        AsyncOperation Load = SceneManager.LoadSceneAsync(scene);
        
        while (!Load.isDone)
        {
            float Progres = Mathf.Clamp01(Load.progress / 0.9f);
            _percentage.value = Progres;
            yield return null;
        }
    }
}
