using UnityEngine;

public class ClosedPipes : MonoBehaviour
{
    // Speed at which the pipes move to the left
    private float speed = 2.4f;

    // The left edge beyond which the pipes are destroyed
    private float leftEdge = -3.2f;

    private void Update() {
        // Check if the game state is not GameOver before moving the pipes
        if (GameManager.instance.state != GameManager.States.GameOver) {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }

        // Check if the pipes have moved beyond the leftEdge
        if (transform.position.x < leftEdge) {
            Destroy(gameObject);
        }
    }

    public void MovePipe() {
        // Get the BottomPipe's transform
        Transform bottomPipe = transform.GetChild(0);
        // Move the BottomPipe down by 0.8
        bottomPipe.position += Vector3.down * 0.8f;

        // Get the TopPipe's transform
        Transform topPipe = transform.GetChild(1);
        // Move the TopPipe up by 0.8
        topPipe.position += Vector3.up * 0.8f;
        }
}