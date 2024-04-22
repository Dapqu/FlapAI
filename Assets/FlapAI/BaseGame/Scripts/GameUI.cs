using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    public static GameUI instance { get; private set; }

    // UI elements
    private TextMeshProUGUI scoreText;
    private Canvas gameOverScreen;
    private Canvas nameEntryCanvas;
    private Canvas leaderboardCanvas;
    private TextMeshProUGUI gameOverScoreText;
    private TextMeshProUGUI gameOverHighScoreText;
    private Button okButton;
    private Button shareButton;
    private Button saveButton;
    private Button doneButton;
    private Canvas tutorialScreen;
    private Image gameOverMedal;

    [SerializeField] private Sprite[] medals;
    [SerializeField] private int[] scoreThresholds;
    [SerializeField] private TMP_Text nameEntered;

    private void Awake()
    {
        instance = this;

        // Initialize UI components
        scoreText = GetComponentInChildren<TextMeshProUGUI>();
        gameOverScreen = transform.Find("GameOverCanvas").GetComponent<Canvas>();
        nameEntryCanvas = transform.Find("NameEntryCanvas").GetComponent<Canvas>();
        leaderboardCanvas = transform.Find("LeaderBoardCanvas").GetComponent<Canvas>();
        tutorialScreen = transform.Find("TutorialCanvas").GetComponent<Canvas>();
        okButton = gameOverScreen.transform.Find("Ok").GetComponent<Button>();
        shareButton = gameOverScreen.transform.Find("Share").GetComponent<Button>();
        saveButton = nameEntryCanvas.transform.Find("Save").GetComponent<Button>();
        doneButton = leaderboardCanvas.transform.Find("Done").GetComponent<Button>();
        gameOverScoreText = gameOverScreen.transform.Find("ScorePanel/Score").GetComponent<TextMeshProUGUI>();
        gameOverHighScoreText = gameOverScreen.transform.Find("ScorePanel/Best").GetComponent<TextMeshProUGUI>();
        gameOverMedal = gameOverScreen.transform.Find("ScorePanel/Medal").GetComponent<Image>();

        // Add event listeners to buttons
        okButton.onClick.AddListener(okPressed);
        shareButton.onClick.AddListener(sharePressed);
        saveButton.onClick.AddListener(savePressed);
        doneButton.onClick.AddListener(okPressed);
    }

    // Update the displayed score on the UI
    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }

    // Display the game over screen with scores
    public void GameOver()
    {
        CanvasGroup nameEntryScreen = nameEntryCanvas.GetComponent<CanvasGroup>();
        CanvasGroup leaderboardScreen = leaderboardCanvas.GetComponent<CanvasGroup>();

        // Check if name entry and leaderboard screens are not active
        if (nameEntryScreen.alpha == 0 && leaderboardScreen.alpha == 0)
        {
            CanvasGroup screen = gameOverScreen.GetComponent<CanvasGroup>();
            SetCanvasGroupProperties(screen, 1f, true);

            CanvasGroup scoreTextGroup = scoreText.GetComponent<CanvasGroup>();
            SetCanvasGroupProperties(scoreTextGroup, 0f, false);

            // Display scores
            gameOverHighScoreText.text = GameManager.instance.highScore.ToString();
            int score = GameManager.instance.Score;
            gameOverScoreText.text = score.ToString();

            // Check if score reaches medal threshold
            if (score >= scoreThresholds[0])
            {
                CanvasGroup medalGroup = gameOverMedal.GetComponent<CanvasGroup>();
                SetCanvasGroupProperties(medalGroup, 1f, true);
                int spriteIndex = 0;
                for (int i = 0; i < scoreThresholds.Length; i++)
                {
                    if (score >= scoreThresholds[i])
                        spriteIndex = i;
                }
                gameOverMedal.GetComponent<Image>().sprite = medals[spriteIndex];
            }
        }
    }

    // Transition into the game, showing score and hiding tutorial
    public void EnterGame()
    {
        CanvasGroup scoreTextGroup = scoreText.GetComponent<CanvasGroup>();
        SetCanvasGroupProperties(scoreTextGroup, 1f, true);

        CanvasGroup tutorialGroup = tutorialScreen.GetComponent<CanvasGroup>();
        SetCanvasGroupProperties(tutorialGroup, 0f, false);
    }

    // Event handler for 'Ok' button click
    private void okPressed()
    {
        GameManager.instance.ReturnToMenu();
    }

    // Event handler for 'Share' button click
    private void sharePressed()
    {
        NameEntry();
    }

    // Event handler for 'Save' button click
    private void savePressed()
    {
        SaveToLeaderboard();
    }

    // Save the player's score and name to the leaderboard
    private void SaveToLeaderboard()
    {
        CanvasGroup oldScreen = nameEntryCanvas.GetComponent<CanvasGroup>();
        SetCanvasGroupProperties(oldScreen, 0f, false);

        CanvasGroup newScreen = leaderboardCanvas.GetComponent<CanvasGroup>();
        SetCanvasGroupProperties(newScreen, 1f, true);

        HighScoreTable.instance.AddHighScoreEntry(GameManager.instance.Score, nameEntered.text);
        HighScoreTable.instance.spawnScoreRecords();
    }

    // Transition from game over screen to name entry screen
    private void NameEntry()
    {
        CanvasGroup oldScreen = gameOverScreen.GetComponent<CanvasGroup>();
        SetCanvasGroupProperties(oldScreen, 0f, false);

        CanvasGroup newScreen = nameEntryCanvas.GetComponent<CanvasGroup>();
        SetCanvasGroupProperties(newScreen, 1f, true);
    }

    // Set properties of a CanvasGroup
    private void SetCanvasGroupProperties(CanvasGroup canvasGroup, float alpha, bool interactable)
    {
        canvasGroup.alpha = alpha;
        canvasGroup.interactable = interactable;
        canvasGroup.blocksRaycasts = interactable;
    }
}