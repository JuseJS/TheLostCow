using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private Image gameOverBackground;
    
    private int score = 0;
    public bool IsGameOver { get; private set; }

    private void Start()
    {
        IsGameOver = false;
        SetupUI();
    }

    private void SetupUI()
    {
        // Configurar el panel de fondo
        if (gameOverBackground != null)
        {
            // Asegurar que el background cubra toda la pantalla
            RectTransform bgRect = gameOverBackground.GetComponent<RectTransform>();
            bgRect.anchorMin = Vector2.zero;
            bgRect.anchorMax = Vector2.one;
            bgRect.sizeDelta = Vector2.zero;
            bgRect.anchoredPosition = Vector2.zero;
            
            // Configurar color negro opaco
            gameOverBackground.color = Color.black;
            gameOverBackground.gameObject.SetActive(false);
        }

        // Configurar el texto de Game Over
        if (gameOverText != null)
        {
            // Asegurar que el texto esté centrado y por encima del fondo
            RectTransform textRect = gameOverText.GetComponent<RectTransform>();
            textRect.anchorMin = new Vector2(0.5f, 0.5f);
            textRect.anchorMax = new Vector2(0.5f, 0.5f);
            textRect.pivot = new Vector2(0.5f, 0.5f);
            textRect.anchoredPosition = Vector2.zero;
            
            // Configurar el estilo del texto
            gameOverText.color = Color.white;
            gameOverText.fontSize = 70;
            gameOverText.alignment = TextAlignmentOptions.Center;
            gameOverText.gameObject.SetActive(false);
        }

        // Configurar el texto de puntuación
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
            
            // Activar y configurar el panel de fondo
            gameOverBackground.gameObject.SetActive(true);
            
            // Configurar y mostrar el texto de Game Over
            gameOverText.text = $"Game Over\nFinal Score: {score}";
            gameOverText.gameObject.SetActive(true);
            
            // Desbloquear el cursor
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void UpdateScoreText()
    {
        scoreText.text = $"Score: {score}";
    }
}