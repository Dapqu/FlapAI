using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   public static GameManager instance { get; private set; }


   private int highscore;
   public int Highscore => highscore;

   private int score;
   public int Score
   {
      get { return score; }
      set
      {
         score = value;
         GameUI.instance.UpdateScore(score);
      }
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

   public void StartGame()
   {
      Score = 0;
   }

   public void GameOver() 
   {
      Debug.Log("Game Over");
      if (score > highscore)
      {
         highscore = score;
      }
      GameUI.instance.GameOver();
   }

   public void IncreaseScore()
   {
      Debug.Log("Increase Score");
      Score++;
   }

}
