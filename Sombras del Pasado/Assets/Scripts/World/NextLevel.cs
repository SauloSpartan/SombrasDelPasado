using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    [SerializeField] private int _buildIndex;

    void Start()
    {
        _buildIndex = SceneManager.GetActiveScene().buildIndex;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(_buildIndex + 1);
        }
    }
}
