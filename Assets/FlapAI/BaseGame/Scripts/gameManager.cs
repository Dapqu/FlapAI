using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour
{
   public static GameManager instance { get; private set; }

   private string savePath = "/GameData.json";

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

   public void LoadScore() {
      string json = File.ReadAllText(Application.dataPath + savePath);
      GameData data = JsonUtility.FromJson<GameData>(json);
      highScore = data.highScore;
   }

   public void SaveScore() {
      GameData data;
      // if data already exists, load it to modify score, otherwise create new
      if (File.Exists(Application.dataPath + savePath))
      {
         string oldJson = File.ReadAllText(Application.dataPath + savePath);
         data = JsonUtility.FromJson<GameData>(oldJson);
      }
      else
         data = new GameData();

      data.highScore = highScore;
      string newJson = JsonUtility.ToJson(data, true);
      File.WriteAllText(Application.dataPath + savePath, newJson);
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

   public void StartAiEasyMode() {
      state = States.EnterGame;
      SceneManager.LoadScene("FlapAI/BaseGame/Scenes/AiMode");
      Score = 0;
   }

   public void StartAiHardMode() {
      state = States.EnterGame;
      SceneManager.LoadScene("FlapAI/BaseGame/Scenes/AiHardMode");
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
         SaveScore();
      }
      GameUI.instance.GameOver();
   }

   public void ReturnToMenu() {
      state = States.MenuScreen;
      SceneManager.LoadScene("FlapAI/BaseGame/Scenes/StartScene");
   }

   public void IncreaseScore() {
      Score++;
   }

   public void ResetScore() {
      Score = 0;
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
      //Load high score
      LoadScore();
   }
}
