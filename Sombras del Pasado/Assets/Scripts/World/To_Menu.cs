using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class To_Menu : MonoBehaviour
{
    LoadingScreen loadingScreen;
    private float fast = 5.0f;

    private void Awake()
    {
        loadingScreen = FindObjectOfType<LoadingScreen>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape"))
            loadingScreen.StartLoading("Menu");

        if (Input.GetKeyDown(KeyCode.Space))
            Time.timeScale = fast;

        if (Input.GetKeyUp(KeyCode.Space))
            Time.timeScale = 1.0f;

        if (Input.GetKeyDown(KeyCode.W))
            Time.timeScale = fast;

        if (Input.GetKeyUp(KeyCode.W))
            Time.timeScale = 1.0f;

        if (Input.GetKeyDown(KeyCode.A))
            Time.timeScale = fast;

        if (Input.GetKeyUp(KeyCode.A))
            Time.timeScale = 1.0f;

        if (Input.GetKeyDown(KeyCode.S))
            Time.timeScale = fast;

        if (Input.GetKeyUp(KeyCode.S))
            Time.timeScale = 1.0f;

        if (Input.GetKeyDown(KeyCode.D))
            Time.timeScale = fast;

        if (Input.GetKeyUp(KeyCode.D))
            Time.timeScale = 1.0f;

        if (Input.GetKeyDown(KeyCode.J))
            Time.timeScale = fast;

        if (Input.GetKeyUp(KeyCode.J))
            Time.timeScale = 1.0f;

        if (Input.GetKeyDown(KeyCode.K))
            Time.timeScale = fast;

        if (Input.GetKeyUp(KeyCode.K))
            Time.timeScale = 1.0f;
    }
}
