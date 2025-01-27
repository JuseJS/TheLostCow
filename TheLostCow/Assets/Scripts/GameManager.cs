using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text gameOverText;

    private int score = 0;
    public bool IsGameOver { get; private set; }

    private void Start()
    {
        IsGameOver = false;
        gameOverText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(true);
        UpdateScoreText();
    }

    public void AddPoint()
    {
        if (!IsGameOver)
        {
            score++;
            UpdateScoreText();
        }
    }

    public void GameOver()
    {
        if (!IsGameOver)
        {
            IsGameOver = true;
            gameOverText.gameObject.SetActive(true);
            gameOverText.text = $"Game Over\nFinal Score: {score}";
        }
    }

    private void UpdateScoreText()
    {
        scoreText.text = $"Score: {score}";
    }
}