using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

// Controls the score
public class NewScore : MonoBehaviour
{
    [SerializeField] private Text _scoreText;
    public int _score;
    private int _newScore = 0;
    private int _addScore;
    public int ScoreCount;
    private int _oldScoreCount;
    private float _timer;

    void Start()
    {
        _addScore = 10;
        ScoreCount = 0;
        _oldScoreCount = ScoreCount;
        _timer = 0;
        _score = PlayerPrefs.GetInt("TheScore", 0);
        _newScore = PlayerPrefs.GetInt("TheScore", 0);
    }

    void Update()
    {
        _scoreText.text = "000" + _score.ToString(); // Converts Score into text
        ScoreCombo();
    }

    /// <summary>
    /// Function that controls ScoreCombo timer to multiply score per combo.
    /// </summary>
    private void ScoreCombo()
    {
        if (_oldScoreCount < ScoreCount)
        {
            _newScore += _addScore;
            _addScore += (_addScore / 5);
            _oldScoreCount = ScoreCount;
            _timer = 4f; // Max time before reseting score
        }

        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
        }
        if (_timer <= 0)
        {
            ScoreCount = 0;
            _oldScoreCount = ScoreCount;
            _addScore = 10;
        }

        _score = (int)Mathf.Lerp(_score, _newScore, Time.deltaTime * 10); // Smooth transition to new score numbers
    }
}
