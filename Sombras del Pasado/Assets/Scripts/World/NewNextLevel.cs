using System.Collections;
using System.Transactions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewNextLevel : MonoBehaviour
{
    private int _buildIndex;

    void Start()
    {
        _buildIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public IEnumerator NextLevel()
    {
        yield return new WaitForSecondsRealtime(3);
        SceneManager.LoadScene(_buildIndex + 1);
    }
}
