using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    public static GameUI instance { get; private set; }

    private TextMeshProUGUI scoreText;
    private Canvas gameOverScreen;
    private TextMeshProUGUI gameOverScoreText;
    private TextMeshProUGUI gameOverHighScoreText;
    private Button okButton;
    private Canvas tutorialScreen;

    private void Awake() {
        instance = this;

        scoreText = GetComponentInChildren<TextMeshProUGUI>();
        gameOverScreen = transform.Find("GameOverCanvas").GetComponent<Canvas>();
        tutorialScreen = transform.Find("TutorialCanvas").GetComponent<Canvas>();
        okButton = gameOverScreen.transform.Find("Ok").GetComponent<Button>();
        gameOverScoreText = gameOverScreen.transform.Find("ScorePanel/Score").GetComponent<TextMeshProUGUI>();
        gameOverHighScoreText = gameOverScreen.transform.Find("ScorePanel/Best").GetComponent<TextMeshProUGUI>();

        okButton.onClick.AddListener(okPressed);
    }

    // Update the displayed score on the UI
    public void UpdateScore(int score) {
        scoreText.text = score.ToString();
    }

    // Display the game over screen with scores
    public void GameOver() {
        CanvasGroup screen = gameOverScreen.GetComponent<CanvasGroup>();
        SetCanvasGroupProperties(screen, 1f, true);

        CanvasGroup scoreTextGroup = scoreText.GetComponent<CanvasGroup>();
        SetCanvasGroupProperties(scoreTextGroup, 0f, false);

        gameOverScoreText.text = GameManager.instance.Score.ToString();
        gameOverHighScoreText.text = GameManager.instance.highScore.ToString();
    }

    // Transition into the game, showing score and hiding tutorial
    public void EnterGame() {
        CanvasGroup scoreTextGroup = scoreText.GetComponent<CanvasGroup>();
        SetCanvasGroupProperties(scoreTextGroup, 1f, true);

        CanvasGroup tutorialGroup = tutorialScreen.GetComponent<CanvasGroup>();
        SetCanvasGroupProperties(tutorialGroup, 0f, false);
    }

    private void okPressed() {
        GameManager.instance.ReturnToMenu();
    }

    private void SetCanvasGroupProperties(CanvasGroup canvasGroup, float alpha, bool interactable) {
        canvasGroup.alpha = alpha;
        canvasGroup.interactable = interactable;
        canvasGroup.blocksRaycasts = interactable;
    }
}
