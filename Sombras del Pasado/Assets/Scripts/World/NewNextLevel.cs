using UnityEngine;
using UnityEngine.SceneManagement;

public class NewNextLevel : MonoBehaviour
{
    private int _buildIndex;

    void Start()
    {
        _buildIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(_buildIndex + 1);
    }
}
