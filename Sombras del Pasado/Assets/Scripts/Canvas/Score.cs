using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Text scoreText;

    public int score = 0;

    void Start()
    {

    }

    void Update()
    {
        scoreText.text = "000" + score.ToString();
    }
}
