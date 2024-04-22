using System.Collections.Generic;
using UnityEngine;

public class FlappyAIAgent : MonoBehaviour
{
    public DistanceCalculator distanceCalculator;

    private Rigidbody2D birdRigidbody;
    public float strength = 5f;
    public float decisionInterval = 0.3f; // Time in seconds between decisions
    private float timeSinceLastDecision = 0f; // Time elapsed since the last decision

    private Dictionary<(int, int, int, int), float> qTable = new Dictionary<(int, int, int, int), float>();
    private float learningRate = 0.6f;
    private float discountFactor = 0.9f;
    private float explorationRate = 0.5f;

    private bool scored = false;
    private bool hitPipe = false;

    private void Awake()
    {
        birdRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (GameManager.instance.state == GameManager.States.EnterGame) {
            GameManager.instance.EnterGame();
        }
    }

    private void Update()
    {
        Time.timeScale = 5;

        if (GameManager.instance.state == GameManager.States.ActiveGame)
        {
            // Update the time since the last decision
            timeSinceLastDecision += Time.deltaTime;

            // Check if it's time to make a new decision
            if (timeSinceLastDecision >= decisionInterval)
            {
                TakeActionBasedOnCurrentState();
                // Reset the timer
                timeSinceLastDecision = 0f;
            }
        }
    }

    private void TakeActionBasedOnCurrentState()
    {
        var currentState = GetCurrentState();
        bool shouldFlap = DecideAction(currentState);

        // Execute action
        if (shouldFlap)
        {
            birdRigidbody.velocity = Vector2.up * strength;
        }

        float reward = GetImmediateReward();
        var nextState = GetCurrentState();

        UpdateQValue(currentState, shouldFlap ? 1 : 0, reward, nextState);

        if(hitPipe)
        {
            Restart();
        }

        scored = false;
        
        hitPipe = false;
    }

    private (int distX, int distYTop, int distYBottom) GetCurrentState()
    {
        // Access distances from DistanceCalculator and discretize them
        return (
            distanceCalculator.LastXDistance,
            distanceCalculator.LastYDistanceToTopPipe,
            distanceCalculator.LastYDistanceToBottomPipe);
    }

    private bool DecideAction((int distX, int distYTop, int distYBottom) state)
    {
            // Exploration vs Exploitation
        if (Random.value < explorationRate)
        {
            // Exploration: choose an action randomly
            return Random.value < 0.5f;
        }
        else
        {
            // Exploitation: choose the best action based on the current Q-values
            float flapValue = GameManager.instance.GetQValue(state.distX, state.distYTop, state.distYBottom, 1); // Assuming 1 represents 'flap'
            float noFlapValue = GameManager.instance.GetQValue(state.distX, state.distYTop, state.distYBottom, 0); // Assuming 0 represents 'do not flap'

            // If flapping has a higher or equal Q-value, choose to flap, otherwise do not
            bool shouldFlap = flapValue >= noFlapValue;
            Debug.Log($"Flap Q-value: {flapValue}, No Flap Q-value: {noFlapValue}, Action chosen: {(shouldFlap ? "Flap" : "No Flap")}");

            return flapValue >= noFlapValue;
        }
    }

    private void UpdateQValue((int distX, int distYTop, int distYBottom) state, int action, float reward, (int distX, int distYTop, int distYBottom) nextState)
    {
        float currentQValue = GameManager.instance.GetQValue(state.distX, state.distYTop, state.distYBottom, action);
        Debug.Log($"current q value is: {currentQValue}");
        Debug.Log($"reward is: {reward}");
        // Calculate the max Q-value for the next state across all possible actions
        float nextMaxQ = Mathf.Max(
            GameManager.instance.GetQValue(state.distX, state.distYTop, state.distYBottom, 0),
            GameManager.instance.GetQValue(state.distX, state.distYTop, state.distYBottom, 1)
        );

        // Apply the Q-learning formula to update the Q-value
        float newQValue = currentQValue + learningRate * (reward + discountFactor * nextMaxQ - currentQValue);
        Debug.Log($"New q value is: {newQValue}");
        // Update the Q-table in GameManager with the new Q-value
        GameManager.instance.UpdateQValue(state.distX, state.distYTop, state.distYBottom, action, newQValue);
    }

    private float GetImmediateReward()
    {
        
        if(scored)
        {
            return 20f;
        }

        if(hitPipe)
        {
            return -100f;
        }
        
        return 1f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if we've collided with a scoring object
        if (other.CompareTag("scoring"))
        {
            scored = true;
            GameManager.instance.IncreaseScore();
        }
        else if(other.CompareTag("obstacle") || other.CompareTag("ground"))
        {
            hitPipe = true;
        }
    }

    private void Restart() 
    {
        GameManager.instance.GameOver();
        GameManager.instance.StartAiHardMode();
    }
}
