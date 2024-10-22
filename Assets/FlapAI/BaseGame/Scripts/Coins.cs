using UnityEngine;

public class Coins : MonoBehaviour
{
    // Speed at which the coins move to the left
    private float speed = 2.4f;

    // The left edge beyond which the coins are destroyed
    private float leftEdge = -3.2f;

    private void Update() {
        // Check if the game state is not GameOver before moving the coins
        if (GameManager.instance.state != GameManager.States.GameOver) {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }

        // Check if the coins have moved beyond the leftEdge
        if (transform.position.x < leftEdge) {
            Destroy(gameObject);
        }
    }
}