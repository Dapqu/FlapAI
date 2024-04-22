using UnityEngine;

public class Pipes : MonoBehaviour
{
    // Speed at which the pipes move to the left
    private float speed = 2.4f;

    private void Update() {
        // Check if the game state is not GameOver before moving the pipes
        if (GameManager.instance.state != GameManager.States.GameOver) {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
    }
}