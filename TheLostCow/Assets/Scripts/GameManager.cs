using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private Image gameOverBackground;
    [SerializeField] private Button restartButton;
    [SerializeField] private TMP_Text restartButtonText; // Referencia al texto del botón

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
            RectTransform bgRect = gameOverBackground.GetComponent<RectTransform>();
            bgRect.anchorMin = Vector2.zero;
            bgRect.anchorMax = Vector2.one;
            bgRect.sizeDelta = Vector2.zero;
            bgRect.anchoredPosition = Vector2.zero;

            gameOverBackground.color = Color.black;
            gameOverBackground.gameObject.SetActive(false);
        }

        // Configurar el texto de Game Over
        if (gameOverText != null)
        {
            RectTransform textRect = gameOverText.GetComponent<RectTransform>();

            // Mantener anclajes en el centro
            textRect.anchorMin = new Vector2(0.5f, 0.5f);
            textRect.anchorMax = new Vector2(0.5f, 0.5f);
            textRect.pivot = new Vector2(0.5f, 0.5f);

            // Ajustar la posición anclada (solo movimiento vertical, X = 0 para centrado perfecto)
            textRect.anchoredPosition = new Vector2(0, 50);

            // Ajustar el tamaño para que sea simétrico
            textRect.sizeDelta = new Vector2(1000, 400);

            // Configuración del texto
            gameOverText.color = Color.white;
            gameOverText.fontSize = 70;
            gameOverText.alignment = TextAlignmentOptions.Center;
            gameOverText.horizontalAlignment = HorizontalAlignmentOptions.Center;
            gameOverText.verticalAlignment = VerticalAlignmentOptions.Middle;

            // Asegurarse que el rectángulo del texto esté ajustado al contenido
            gameOverText.enableAutoSizing = true;
            gameOverText.enableWordWrapping = true;

            // Establecer márgenes uniformes
            gameOverText.margin = new Vector4(0, 0, 0, 0);

            gameOverText.gameObject.SetActive(false);
        }

        // Configurar el botón de restart
        if (restartButton != null)
        {
            // Configurar posición y tamaño del botón
            RectTransform buttonRect = restartButton.GetComponent<RectTransform>();
            buttonRect.anchorMin = new Vector2(0.5f, 0.5f);
            buttonRect.anchorMax = new Vector2(0.5f, 0.5f);
            buttonRect.pivot = new Vector2(0.5f, 0.5f);
            buttonRect.anchoredPosition = new Vector2(0, -200);
            buttonRect.sizeDelta = new Vector2(200, 60);

            // Configurar color del botón
            ColorBlock colors = restartButton.colors;
            colors.normalColor = new Color(0.25f, 0.25f, 0.25f);
            colors.highlightedColor = new Color(0.3f, 0.3f, 0.3f);
            colors.pressedColor = new Color(0.2f, 0.2f, 0.2f);
            restartButton.colors = colors;

            // Configurar el texto del botón
            if (restartButtonText != null)
            {
                restartButtonText.text = "Restart";
                restartButtonText.color = new Color(0.8f, 0.8f, 0.8f);
                restartButtonText.fontSize = 30;
                restartButtonText.alignment = TextAlignmentOptions.Center;
            }

            restartButton.onClick.AddListener(RestartGame);
            restartButton.gameObject.SetActive(false);
        }

        // Configurar el texto de puntuación
        if (scoreText != null)
        {
            RectTransform scoreRect = scoreText.GetComponent<RectTransform>();

            // Anclar a la esquina superior izquierda
            scoreRect.anchorMin = new Vector2(0, 1);
            scoreRect.anchorMax = new Vector2(0, 1);
            scoreRect.pivot = new Vector2(0, 1);

            // Añadir margen (20 píxeles desde la izquierda y desde arriba)
            scoreRect.anchoredPosition = new Vector2(20, -20);

            // Establecer un tamaño fijo para el área del texto
            scoreRect.sizeDelta = new Vector2(200, 50);

            // Configurar el estilo del texto
            scoreText.alignment = TextAlignmentOptions.Left;
            scoreText.fontSize = 36;

            scoreText.gameObject.SetActive(true);
            UpdateScoreText();
        }
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

            gameOverBackground.gameObject.SetActive(true);

            gameOverText.text = "GAME OVER\n\nFINAL SCORE: " + score;
            gameOverText.gameObject.SetActive(true);

            restartButton.gameObject.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void UpdateScoreText()
    {
        scoreText.text = $"Score: {score}";
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}