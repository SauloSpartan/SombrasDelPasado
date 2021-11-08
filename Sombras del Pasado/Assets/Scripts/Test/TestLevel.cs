using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TestLevel : MonoBehaviour
{
    public Text levelText;
    public int buildIndex;

    void Start()
    {
        buildIndex = SceneManager.GetActiveScene().buildIndex;
        Debug.Log(SceneManager.GetActiveScene().buildIndex + 1);
        levelText.text = "Level: " + buildIndex.ToString("0");
    }

    void Update()
    {
        
    }

    public void levelUpdate()
    {
        Debug.Log("Triggered");
        levelText.text = "Level: " + buildIndex.ToString("0");
        Debug.Log(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
