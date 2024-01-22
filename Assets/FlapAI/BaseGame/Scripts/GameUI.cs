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

    private void Awake()
    {
        instance = this;
    }

    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }

    public void GameOver()
    {
        CanvasGroup scrn = gameOverScreen.GetComponent<CanvasGroup>();
        scrn.alpha = 1f;
        scrn.interactable = true;
        scrn.blocksRaycasts = true;

        gameOverScoreText.text = GameManager.instance.Score.ToString();
        gameOverHighScoreText.text = GameManager.instance.Highscore.ToString();
    }
}
