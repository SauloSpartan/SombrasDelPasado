using System.Collections;
using System.Transactions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewNextLevel : MonoBehaviour
{
    private int _buildIndex;
    private LoadingScreen _loadingScreen;
    private NewScore _theScore;

    private void Awake()
    {
        _loadingScreen = FindObjectOfType<LoadingScreen>();
        _buildIndex = SceneManager.GetActiveScene().buildIndex;
        _theScore = FindObjectOfType<NewScore>();
    }

    public IEnumerator NextLevel()
    {
        yield return new WaitForSecondsRealtime(3);
        PlayerPrefs.SetInt("Saved", SceneManager.GetActiveScene().buildIndex);
        _loadingScreen.StartLoading(_buildIndex + 1, _theScore._score);
    }
}
