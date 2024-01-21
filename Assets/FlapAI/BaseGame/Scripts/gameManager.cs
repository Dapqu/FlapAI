using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance { get; private set; }

    [SerializeField] private Player player;
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject gameOver;

    private int score;
    public int Score => score;

   public void GameOver() 
   {
        Debug.Log("Game Over");
   }

   public void IncreaseScore()
   {
    score++;
   }

}
