using UnityEngine;

public class DistanceCalculator : MonoBehaviour
{
    private GameObject[] pipes; // Array to hold all pipe instances

    public float NearestXDistance { get; private set; }
    public float DistanceToBottomOfTopPipe { get; private set; }
    public float DistanceToTopOfBottomPipe { get; private set; }

    void Update()
    {
        // Find all game objects tagged as "Pipe" (make sure your pipes are tagged accordingly)
        pipes = GameObject.FindGameObjectsWithTag("Pipe");

        float nearestXDistance = float.MaxValue;
        GameObject nearestPipeSet = null;

        // Iterate through all pipes to find the nearest set that is in front of the player
        foreach (GameObject pipe in pipes)
        {

            if (pipe.transform.position.x > transform.position.x) // Pipe is in front of the bird
            {
                BoxCollider2D collider = pipe.GetComponent<BoxCollider2D>();
                if (collider != null)
                {
                    // Calculate the left edge's X position
                    float leftEdgeX = pipe.transform.position.x - collider.size.x * 0.5f * Mathf.Abs(pipe.transform.localScale.x);

                    // Now measure distance from the bird to the left edge instead of the center
                    float distanceX = leftEdgeX - transform.position.x;
                    distanceX = RoundToNearestIncrement(distanceX, 0.1f);
                    if (distanceX < nearestXDistance)
                    {
                        nearestXDistance = distanceX;
                        
                        if (nearestXDistance < 0)
                        {
                            nearestXDistance = 0;
                        }

                        nearestPipeSet = pipe;
                    }
                }
            }
        }

        // If a nearest pipe set is found
        if (nearestPipeSet != null)
        {
            // Assuming your prefab structure allows you to easily find the top and bottom pipes
            Transform topPipe = nearestPipeSet.transform.Find("SPR_TopPipe"); // Adjust these names based on your prefab
            Transform bottomPipe = nearestPipeSet.transform.Find("SPR_BottomPipe");

            // Calculate Y distances
            BoxCollider2D topPipeCollider = topPipe.GetComponent<BoxCollider2D>();
            BoxCollider2D bottomPipeCollider = bottomPipe.GetComponent<BoxCollider2D>();

            // Calculate distance from the bottom of the top pipe to the player
            float bottomOfTopPipeY = topPipe.position.y - topPipeCollider.size.y * 0.5f * topPipe.localScale.y;
            float distanceToBottomOfTopPipe = RoundToNearestIncrement(transform.position.y - bottomOfTopPipeY, 0.1f);

            // Calculate distance from the top of the bottom pipe to the player
            float topOfBottomPipeY = bottomPipe.position.y + bottomPipeCollider.size.y * 0.5f * bottomPipe.localScale.y;
            float distanceToTopOfBottomPipe = RoundToNearestIncrement(topOfBottomPipeY - transform.position.y, 0.1f);

            // Output the distances (for debug purposes or further use)
            if(GameManager.instance.state != GameManager.States.GameOver) 
            {
                Debug.Log($"Nearest X Distance to Left Edge: {nearestXDistance}");
                Debug.Log($"Distance to Bottom of Top Pipe: {distanceToBottomOfTopPipe}");
                Debug.Log($"Distance to Top of Bottom Pipe: {distanceToTopOfBottomPipe}");
            }
            
        }
        else
        {
            NearestXDistance = float.MaxValue;
            DistanceToBottomOfTopPipe = float.MaxValue;
            DistanceToTopOfBottomPipe = float.MaxValue;
        }
    }

    // Function to round a value to the nearest multiple of an increment
    private float RoundToNearestIncrement(float value, float increment)
    {
        return Mathf.Round(value / increment) * increment;
    }
}
