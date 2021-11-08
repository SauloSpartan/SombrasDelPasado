using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScore : MonoBehaviour
{
    Score Score;

    void Start()
    {
        Score = FindObjectOfType<Score>();
    }


    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Score.score = Score.score + 1;
    }
}
