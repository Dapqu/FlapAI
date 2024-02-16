using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector3 position { get; private set; }

    private SpriteRenderer spriteRenderer;
    public Rigidbody2D birdRigidbody;
    private Animator animator;
    [SerializeField] private SpriteRenderer bubble;
    private Transform sprite;

    private Boolean hasBubble = false;

    private float tilt = 15f;
    private float strength = 5f;
    private float floatingDegrees = 0f;
    private float alphaIncreaseSpeed = 1f;

    [SerializeField] private ParticleSystem rain;
    private bool isPaused = false;

    private void Awake() {
        // Get references to components in Awake
        sprite = transform.Find("Sprite");
        spriteRenderer = sprite.GetComponent<SpriteRenderer>();
        birdRigidbody = GetComponent<Rigidbody2D>();
        animator = sprite.GetComponent<Animator>();
        bubble = transform.Find("Bubble").GetComponent<SpriteRenderer>();
    }

    public void Jump() {
        birdRigidbody.velocity = Vector2.up * strength;
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

            if (state != GameManager.States.GameOver)
                Jump();
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

        if (!hasBubble) {
            bubble.color = new Color(bubble.color.r, bubble.color.g, bubble.color.b, 0f);
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
        sprite.transform.eulerAngles = new Vector3(sprite.transform.eulerAngles.x, sprite.transform.eulerAngles.y, zRotation);
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
        if (other.CompareTag("obstacle")) {
            if (hasBubble) {
                StartCoroutine(Invinsible());
            }
            else {
                animator.SetBool("Dead", true);
                GameManager.instance.GameOver();
            }
        } 
        else if (other.CompareTag("ground")) {
            animator.SetBool("Dead", true);
            GameManager.instance.GameOver();
        }
        else if(other.CompareTag("scoring")) {
            GameManager.instance.IncreaseScore();
        }
        else if (other.CompareTag("gravityScoring")) {
            GameManager.instance.IncreaseScore();
            StartCoroutine(SwapGravityValuesWithDelay());
        }
        else if (other.CompareTag("bubbleScoring")) {
            GameManager.instance.IncreaseScore();
            // Add a bubble around player, extra life shield
            hasBubble = true;
            StartCoroutine(FadeInBubble());
        }
        else if (other.CompareTag("collectable")) {
            GameManager.instance.IncreaseScore();
            Destroy(other.gameObject);
        }
    }

    IEnumerator FadeInBubble() {
        // Gradually increase the alpha value
        while (bubble.color.a < 0.8f) {
            float newAlpha = Mathf.MoveTowards(bubble.color.a, 1f, alphaIncreaseSpeed * Time.deltaTime);
            bubble.color = new Color(bubble.color.r, bubble.color.g, bubble.color.b, newAlpha);
            yield return null;
        }
    }

    private IEnumerator SwapGravityValuesWithDelay() {
        float oldGravity = birdRigidbody.gravityScale;
        float oldStrength = strength;

        // Set gravityScale and strength to 0 during the delay
        birdRigidbody.gravityScale = 0;
        strength = 0;
        birdRigidbody.velocity = new Vector2(birdRigidbody.velocity.x, 0f);

        TogglePause();

        yield return new WaitForSeconds(0.5f);

        // After the delay, swap gravityScale and strength values
        birdRigidbody.gravityScale = oldGravity * -1;
        strength = oldStrength * -1;
    }

    IEnumerator Invinsible() {        
        // Disable collision with "obstacle" objects
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Obstacle"), true);

        hasBubble = false;
        bubble.color = new Color(bubble.color.r, bubble.color.g, bubble.color.b, 0f);

        // Blink the sprite for 2 seconds
        float blinkDuration = 2f;
        float blinkInterval = 0.2f;

        while (blinkDuration > 0f) {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(blinkInterval);
            blinkDuration -= blinkInterval;
        }

        // Ensure the sprite is visible after blinking
        spriteRenderer.enabled = true;

        // Restore the original collision state
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Obstacle"), false);
    }

    public void TogglePause() {
        if (isPaused) {
            // Resume particle emission
            rain.Play();
        }
        else {
            // Pause particle emission
            rain.Pause();
        }

        isPaused = !isPaused;
    }
}
