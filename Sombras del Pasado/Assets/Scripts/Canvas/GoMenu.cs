using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoMenu : MonoBehaviour
{
    private LoadingScreen _loadingScreen;

    private void Awake()
    {
        _loadingScreen = FindObjectOfType<LoadingScreen>();
    }


    public void ReturnMenu()
    {
        _loadingScreen.StartLoading(0, 0);
    }
}
