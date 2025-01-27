using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private Image gameOverBackground; // Panel que servirá de fondo
    
    [Header("Game Over Settings")]
    [SerializeField] private Color backgroundColor = new Color(0f, 0f, 0f, 0.8f); // Negro semi-transparente

    private int score = 0;
    public bool IsGameOver { get; private set; }

    private void Start()
    {
        IsGameOver = false;
        
        // Configurar el panel de fondo
        if (gameOverBackground != null)
        {
            gameOverBackground.color = backgroundColor;
            gameOverBackground.gameObject.SetActive(false);
        }
        
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
            
            // Mostrar el panel de fondo y el texto
            if (gameOverBackground != null)
            {
                gameOverBackground.gameObject.SetActive(true);
            }
            
            gameOverText.gameObject.SetActive(true);
            gameOverText.text = $"Game Over\nFinal Score: {score}";
            
            // Desbloquear el cursor
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void UpdateScoreText()
    {
        scoreText.text = $"Score: {score}";
    }
}