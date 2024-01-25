using UnityEngine;

public class Bullets : MonoBehaviour
{   // The left edge beyond which the pipes are destroyed
    private float rightEdge = 3.2f;

    private void Update() {
        // Check if the bullet have moved beyond the rightEdge
        if (transform.position.x > rightEdge) {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("target")) {
            Destroy(other.gameObject);

            // Access the parent GameObject of the collided object
            GameObject parent = other.transform.parent?.gameObject;

            // Check if the parent GameObject has the TargetScript component
            if (parent != null) {
                ClosedPipes targetScript = parent.GetComponent<ClosedPipes>();

                // Check if the script component is found
                if (targetScript != null)
                {
                    // Call a method or modify properties of the TargetScript
                    targetScript.MovePipe();
                }
            }
        }
        else if (other.CompareTag("obstacle")) {
            Destroy(gameObject);
        }
    }
}
