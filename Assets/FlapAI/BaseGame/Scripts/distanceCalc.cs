using UnityEngine;

public class DistanceCalculator : MonoBehaviour
{
    public Transform birdTransform; // Assign your bird's Transform in the inspector

    public int LastXDistance { get; private set; }
    public int LastYDistanceToTopPipe { get; private set; }
    public int LastYDistanceToBottomPipe { get; private set; }

    void Update()
    {
        // Check if the game state is not GameOver
        if (GameManager.instance.state != GameManager.States.GameOver)
        {
            GameObject closestPipe = FindClosestPipe();
            if (closestPipe != null)
            {
                Transform topPipeTransform = closestPipe.transform.Find("SPR_TopPipe");
                Transform bottomPipeTransform = closestPipe.transform.Find("SPR_BottomPipe");

                // Getting the SpriteRenderer components to access the sprite sizes
                SpriteRenderer topPipeSpriteRenderer = topPipeTransform.GetComponent<SpriteRenderer>();
                SpriteRenderer bottomPipeSpriteRenderer = bottomPipeTransform.GetComponent<SpriteRenderer>();

                if (topPipeSpriteRenderer != null && bottomPipeSpriteRenderer != null)
                {
                    float xDistance = Mathf.Abs(birdTransform.position.x - closestPipe.transform.position.x);

                    // Calculate Y distance to the bottom of the top pipe and Y distance from the top of the bottom pipe
                    float yDistanceToTopPipe = birdTransform.position.y - (topPipeTransform.position.y - topPipeSpriteRenderer.bounds.size.y / 2);
                    float yDistanceToBottomPipe = (bottomPipeTransform.position.y + bottomPipeSpriteRenderer.bounds.size.y / 2) - birdTransform.position.y;

                    
                    xDistance = (Mathf.Round(xDistance * 4f) / 4f)*4;
                    yDistanceToTopPipe = (Mathf.Round(yDistanceToTopPipe * 4f) / 4f)*4;
                    yDistanceToBottomPipe = (Mathf.Round(yDistanceToBottomPipe * 4f) / 4f)*4;

                    LastXDistance = (int)xDistance;
                    LastYDistanceToTopPipe = (int)yDistanceToTopPipe;
                    LastYDistanceToBottomPipe = (int)yDistanceToBottomPipe;

                    
                }
            }
        }
    }

    GameObject FindClosestPipe()
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("obstacle");
        GameObject closestPipe = null;
        float closestDistanceX = Mathf.Infinity;

        foreach (GameObject obj in obstacles)
        {
            if (obj.transform.Find("SPR_TopPipe") != null && obj.transform.Find("SPR_BottomPipe") != null)
            {
                float distanceX = Mathf.Abs(obj.transform.position.x - birdTransform.position.x);
                if (distanceX < closestDistanceX)
                {
                    closestDistanceX = distanceX;
                    closestPipe = obj;
                }
            }
        }

        return closestPipe;
    }

    // Method to discretize distances into bins
    public (int, int, int) DiscretizeState(float xDistance, float yDistanceToTop, float yDistanceToBottom)
    {
        int discretizedX = Mathf.FloorToInt(xDistance / 10); // Discretize every 10 pixels for X
        int discretizedYTop = Mathf.FloorToInt(yDistanceToTop / 10); // Discretize every 10 pixels for Y to top pipe
        int discretizedYBottom = Mathf.FloorToInt(yDistanceToBottom / 10); // Discretize every 10 pixels for Y to bottom pipe
        
        return (discretizedX, discretizedYTop, discretizedYBottom);
    }

}
