using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public int buildIndex;
    LoadingScreen loadingScreen;

    private void Awake()
    {
        loadingScreen = FindObjectOfType<LoadingScreen>();
        buildIndex = SceneManager.GetActiveScene().buildIndex;
    }
    void Start()
    {

    }


    void Update()
    {
        
    }

    public void RestartScene()
    {
        loadingScreen.StartLoading(buildIndex, PlayerPrefs.GetInt("score", 0));
    }
}
