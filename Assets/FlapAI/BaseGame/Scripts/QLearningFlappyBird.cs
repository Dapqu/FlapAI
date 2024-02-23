using UnityEngine;
using System.Collections.Generic;
using System;
public class QLearningFlappyBird : MonoBehaviour
{
    // Q-table to store Q-values for state-action pairs
    private Dictionary<State, Dictionary<Action, float>> qTable = new Dictionary<State, Dictionary<Action, float>>();

    private DistanceCalculator distanceCalculator;

    private Player player = FindObjectOfType<Player>();


    // State representation
    private class State
    {
        public float xDistance;
        public float distanceToBottomOfTopPipe;
        public float distanceToTopOfBottomPipe;
    }

    // Actions
    private enum Action { Flap, DoNothing }

    // rewards
    private float alive = 1f;
    private float dead = -50f;
    private float point = 10f;


    void Start() 
    {
        distanceCalculator = GetComponent<DistanceCalculator>();
    }
    void Update()
    {
        GameManager.States st = GameManager.instance.state;
        // Calculate state features
        State currentState = CalculateState();

        // Choose action based on epsilon-greedy policy
        Action chosenAction = ChooseAction(currentState);

        // Execute action
        ExecuteAction(chosenAction);

        // Update Q-values
        UpdateQValues(currentState, chosenAction);
    }

    // Calculate state features
    private State CalculateState()
    {
        // Your existing code to calculate state features
        float nearestX = distanceCalculator.NearestXDistance;
        float ToBottomOfTopPipe = distanceCalculator.DistanceToBottomOfTopPipe;
        float ToTopOfBottomPipe = distanceCalculator.DistanceToTopOfBottomPipe;
        
        return new State
        {
            xDistance = nearestX,
            distanceToBottomOfTopPipe = ToBottomOfTopPipe,
            distanceToTopOfBottomPipe = ToTopOfBottomPipe
        };
    }

    // Choose action based on epsilon-greedy policy
    private Action ChooseAction(State state)
    {
        float epsilon = 0.1f; // Exploration rate
        if (UnityEngine.Random.value < epsilon)
        {
            // Explore: choose a random action
            return (UnityEngine.Random.value < 0.5) ? Action.Flap : Action.DoNothing;
        }
        else
        {
            // Exploit: choose the best action based on current Q-values
            if (!qTable.ContainsKey(state))
            {
                qTable[state] = new Dictionary<Action, float> { { Action.Flap, 0f }, { Action.DoNothing, 0f } };
            }

            return qTable[state][Action.Flap] > qTable[state][Action.DoNothing] ? Action.Flap : Action.DoNothing;
        }
    }

    // Execute action
    private void ExecuteAction(Action action)
    {
        if(action == Action.Flap)
        {
            player.Flap();
        }
    }

    // Update Q-values
    private void UpdateQValues(State state, Action action)
    {
        float learningRate = 0.1f;
        float discountFactor = 0.9f; // Discount rate for future rewards
    
        // Initialize Q-values for unseen state-action pairs
        if (!qTable.ContainsKey(state))
        {
            qTable[state] = new Dictionary<Action, float> { { Action.Flap, 0f }, { Action.DoNothing, 0f } };
        }
        float reward = GetReward();
        
        State nextState = CalculateState();
        float maxFutureReward = 0;
        if (qTable.ContainsKey(nextState))
        {
            maxFutureReward = Math.Max(qTable[nextState][Action.Flap], qTable[nextState][Action.DoNothing]);
        }
        
        // Q-learning update rule
        float currentQValue = qTable[state][action];
        float newQValue = currentQValue + learningRate * (reward + discountFactor * maxFutureReward - currentQValue);
        qTable[state][action] = newQValue;

    }

   private float GetReward()
    {
        
        if (GameManager.instance.hitObstacle)
        {
            Debug.Log($"reward was: {dead}");
            GameManager.instance.ResetFlags(); // Reset after reading the state
            return dead;
        }
        else if (GameManager.instance.scored)
        {
            Debug.Log($"reward was: {point}");
            GameManager.instance.ResetFlags(); // Reset after reading the state
            return point;
        }
        Debug.Log($"reward was: {alive}");
        GameManager.instance.ResetFlags();
        // Optionally, reset the scored flag here if scoring doesn't indicate game state change
        return alive; // Default alive reward
    }

}
