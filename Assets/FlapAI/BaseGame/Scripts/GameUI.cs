using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    public static GameUI instance;

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] Canvas gameOverScreen;
    [SerializeField] TextMeshProUGUI gameOverScoreText;
    [SerializeField] TextMeshProUGUI gameOverHighScoreText;
    [SerializeField] Button okButton;

    private void Awake()
    {
        instance = this;
        okButton.onClick.AddListener(okPressed);
    }

    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }

    public void GameOver()
    {
        CanvasGroup screen = gameOverScreen.GetComponent<CanvasGroup>();
        screen.alpha = 1f;
        screen.interactable = true;
        screen.blocksRaycasts = true;

        CanvasGroup scoreTextGroup = scoreText.GetComponent<CanvasGroup>();
        scoreTextGroup.alpha = 0f;

        gameOverScoreText.text = GameManager.instance.Score.ToString();
        gameOverHighScoreText.text = GameManager.instance.Highscore.ToString();
    }

    public void okPressed()
    {
        GameManager.instance.ReturnToMenu();
    }
}
