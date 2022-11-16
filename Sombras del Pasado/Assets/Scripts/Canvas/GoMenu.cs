using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoMenu : MonoBehaviour
{
    LoadingScreen loadingScreen;

    private void Awake()
    {
        loadingScreen = FindObjectOfType<LoadingScreen>();
    }


    public void ReturnMenu()
    {
        loadingScreen.StartLoading("Menu");
    }
}
