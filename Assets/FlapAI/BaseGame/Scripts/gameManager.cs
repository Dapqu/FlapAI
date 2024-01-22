using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
   public static GameManager instance { get; private set; }

   public enum States
   {
      MenuScreen,
      EnterGame,
      ActiveGame,
      GameOver
   }
   public States State;

   private int highscore;
   public int Highscore => highscore;

   private int score;
   public int Score
   {
      get { return score; }
      set
      {
         score = value;
         if (GameUI.instance != null)
            GameUI.instance.UpdateScore(score);
      }
   }

   //Exit main menu, go into game
   public void StartGame()
   {
      State = States.EnterGame;
      SceneManager.LoadScene("FlapAI/BaseGame/Scenes/mainGame");
      Score = 0;
   }

   //after clicking once, pipes start to spawn
   public void EnterGame()
   {
      State = States.ActiveGame;
      GameUI.instance.EnterGame();
   }

   public void GameOver() 
   {
      State = States.GameOver;
      if (score > highscore)
      {
         highscore = score;
      }
      GameUI.instance.GameOver();
   }

   public void ReturnToMenu()
   {
      State = States.MenuScreen;
      SceneManager.LoadScene("FlapAI/BaseGame/Scenes/StartScene");
   }

   public void IncreaseScore()
   {
      Debug.Log("Increase Score");
      Score++;
   }

   void Awake()
   {
      if (instance == null)
      {
         instance = this;
         DontDestroyOnLoad(gameObject);
      } 
      else if (instance != this)
      {
         Destroy(gameObject);
      }
   }
}
