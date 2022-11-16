using System.Collections;
using System.Transactions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewNextLevel : MonoBehaviour
{
    private int _buildIndex;
    LoadingScreen loadingScreen;

    private void Awake()
    {
        loadingScreen = FindObjectOfType<LoadingScreen>();
        _buildIndex = SceneManager.GetActiveScene().buildIndex;
    }

    void Start()
    {

    }

    public IEnumerator NextLevel()
    {
        yield return new WaitForSecondsRealtime(3);
        loadingScreen.StartLoading(_buildIndex + 1);
    }
}
