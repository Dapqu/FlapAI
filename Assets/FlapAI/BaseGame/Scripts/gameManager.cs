using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
   public static GameManager instance { get; private set; }

   public enum States {
      EnterGame,
      ActiveGame,
      GameOver,
      MenuScreen
   }
   
   public States state { get; private set; }

   public int highScore { get; private set; }

   private int score;
   public int Score {
      get { return score; }
      private set {
         score = value;
         if (GameUI.instance != null)
            GameUI.instance.UpdateScore(score);
      }
   }

   public void StartEastMode() {
      state = States.EnterGame;
      SceneManager.LoadScene("FlapAI/BaseGame/Scenes/mainGame");
      Score = 0;
   }

   public void StartHardMode() {
      state = States.EnterGame;
      SceneManager.LoadScene("FlapAI/BaseGame/Scenes/hardGame");
      Score = 0;
   }

   public void StartAiEastMode() {
      state = States.EnterGame;
      SceneManager.LoadScene("FlapAI/BaseGame/Scenes/AiMode");
      Score = 0;
   }

   public void EnterGame() {
      state = States.ActiveGame;
      GameUI.instance.EnterGame();
   }

   public void GameOver()  {
      state = States.GameOver;
      if (score > highScore) {
         highScore = score;
      }
      GameUI.instance.GameOver();
   }

   public void ReturnToMenu() {
      state = States.MenuScreen;
      SceneManager.LoadScene("FlapAI/BaseGame/Scenes/StartScene");
   }

   public void IncreaseScore() {
      Debug.Log("Increase Score");
      Score++;
   }

   void Awake() {
      // Ensure only one instance of GameManager exists
      if (instance == null) {
         instance = this;
         DontDestroyOnLoad(gameObject);
      } 
      else if (instance != this) {
         Destroy(gameObject);
      }
   }
}
