using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections.Generic;


public class GameManager : MonoBehaviour
{
   public static GameManager instance { get; private set; }

   private string savePath = "/GameData.json";
   private string qtableFilePath = "/QTable.json";

   public enum States {
      EnterGame,
      ActiveGame,
      GameOver,
      MenuScreen
   }

   public States state { get; private set; }

   public int highScore { get; private set; }

   private int score;

   private int iteration = 0;

   private Dictionary<(int, int, int, int), float> qTableDict = new Dictionary<(int, int, int, int), float>();
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
      iteration = data.iteration;
   }

   public Dictionary<(int, int, int, int), float> LoadQTable()
   {
      Dictionary<(int, int, int, int), float> qTableDict = new Dictionary<(int, int, int, int), float>();

      if (File.Exists(Application.dataPath + qtableFilePath))
      {
         string json = File.ReadAllText(Application.dataPath + qtableFilePath);
         QTable qTable = JsonUtility.FromJson<QTable>(json);

         foreach (QTableEntry entry in qTable.entries)
         {
               (int, int, int, int) key = (entry.distX, entry.distYTop, entry.distYBottom, entry.action);
               qTableDict[key] = entry.value;
         }
      }
      else
      {
         Debug.LogWarning("QTable file not found, initializing new QTable.");
      }

      return qTableDict;
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

   public void UpdateIterationCount() {
      GameData data;
      // if data already exists, load it to modify score, otherwise create new
      if (File.Exists(Application.dataPath + savePath))
      {
         string oldJson = File.ReadAllText(Application.dataPath + savePath);
         data = JsonUtility.FromJson<GameData>(oldJson);
      }
      else
         data = new GameData();

      data.iteration = ++iteration;
      string newJson = JsonUtility.ToJson(data, true);
      File.WriteAllText(Application.dataPath + savePath, newJson);
   }

   public void SaveQTable(Dictionary<(int, int, int, int), float> qTableDict)
   {
      QTable qTable = new QTable();
      foreach (var item in qTableDict)
      {
         (int distX, int distYTop, int distYBottom, int action) = item.Key;
         float value = item.Value;
         qTable.entries.Add(new QTableEntry(distX, distYTop, distYBottom, action, value));
      }

      string json = JsonUtility.ToJson(qTable, true);
      File.WriteAllText(Application.dataPath + qtableFilePath, json);
   }

   public void UpdateQValue(int distX, int distYTop, int distYBottom, int action, float newValue)
   {
      qTableDict[(distX, distYTop, distYBottom, action)] = newValue;
   }

   public float GetQValue(int distX, int distYTop, int distYBottom, int action)
   {
      return qTableDict.TryGetValue((distX, distYTop, distYBottom, action), out float value) ? value : 0f;
   }

   public void PrintQTable()
   {
      foreach (var item in qTableDict)
      {
         (int distX, int distYTop, int distYBottom, int action) = item.Key;
         float qValue = item.Value;
         Debug.Log($"State (X: {distX}, YTop: {distYTop}, YBottom: {distYBottom}), Action: {action} => Q-Value: {qValue}");
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

   public void StartAiEasyMode() {
      state = States.EnterGame;
      SceneManager.LoadScene("FlapAI/BaseGame/Scenes/AiMode");
      Score = 0;

      var qTableDict = LoadQTable();
   }

   public void StartAiHardMode() {
      state = States.EnterGame;
      SceneManager.LoadScene("FlapAI/BaseGame/Scenes/easyqlearning");
      Score = 0;
   }

   public void EnterGame() {
      state = States.ActiveGame;
      GameUI.instance.EnterGame();
   }

   public void GameOver()  {
      state = States.GameOver;
      UpdateIterationCount();
      if (score > highScore) {
         highScore = score;
         SaveScore();
      }

      SaveQTable(qTableDict);

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

      qTableDict = LoadQTable();
      PrintQTable();
   }
}
