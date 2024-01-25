using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector3 position { get; private set; }

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D birdRigidbody;
    private Animator animator;

    private float tilt = 15f;
    private float strength = 5f;
    private float floatingDegrees = 0f;

    private void Awake() {
        // Get references to components in Awake
        spriteRenderer = GetComponent<SpriteRenderer>();
        birdRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update() {
        GameManager.States state = GameManager.instance.state;
        position = transform.position;

        if (animator.GetBool("Dead") && position.y <= -2.6) {
            transform.position = new Vector3(-1.2f, -2.6f, 0);
        }

        // Handle user input
        if (Input.GetMouseButtonDown(0)) {
            // Check game state and perform actions accordingly
            // Start Game
            if (state == GameManager.States.EnterGame)
                GameManager.instance.EnterGame();

            // Flap upwards
            if (state != GameManager.States.GameOver)
                birdRigidbody.velocity = Vector2.up * strength;
        }

        // Handle player movement and rotation
        if (state == GameManager.States.EnterGame) {
            // Apply floating effect during EnterGame state
            ApplyFloatingEffect();
        }
        else {
            // Adjust bird rotation based on velocity
            if (position.y > -2.5) {
                AdjustBirdRotation();
            }
        }

        // Check if player is below a certain y-position and destroy if necessary
        CheckDestroyCondition();
    }

    private void ApplyFloatingEffect() {
        // Apply a sinusoidal floating effect during EnterGame state
        transform.position = new Vector2(-1.2f, 0f) + (Vector2.up * 0.18f * Mathf.Sin(floatingDegrees * Mathf.PI / 180f));
        floatingDegrees = (floatingDegrees + 400f * Time.deltaTime) % 360f;
    }

    private void AdjustBirdRotation() {
        // Adjust bird rotation based on velocity and add a lower limit
        float zRotation = Mathf.Clamp((birdRigidbody.velocity.y + 5f) * tilt, -90f, 25f);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, zRotation);
    }

    private void CheckDestroyCondition() {
        // Destroy the player object if it goes below a certain y-position
        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // Handle collisions with obstacles, ground, and scoring objects
        if (other.CompareTag("obstacle") || other.CompareTag("ground")) {
            animator.SetBool("Dead", true);
            GameManager.instance.GameOver();
        } 
        else if(other.CompareTag("scoring")) {
            GameManager.instance.IncreaseScore();
        }
        else if (other.CompareTag("collectable")) {
            GameManager.instance.IncreaseScore();
            Destroy(other.gameObject);
        }
    }
}
