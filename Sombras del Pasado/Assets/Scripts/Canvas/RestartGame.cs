using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public int buildIndex;

    void Start()
    {
        buildIndex = SceneManager.GetActiveScene().buildIndex;
    }


    void Update()
    {
        
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(buildIndex);
    }
}
