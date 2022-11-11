using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Text scoreText;
    public static int score = 0;

    void Update()
    {
        scoreText.text = "000" + score.ToString();
    }
}
