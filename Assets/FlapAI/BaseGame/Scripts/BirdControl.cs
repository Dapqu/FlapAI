using System;
using System.Collections;
using UnityEngine;

public class BirdControl : MonoBehaviour
{
    // Properties
    public Vector3 position { get; private set; }

    // Components
    private SpriteRenderer spriteRenderer;
    public Rigidbody2D birdRigidbody;
    private Animator animator;
    [SerializeField] private SpriteRenderer bubble;
    private Transform sprite;

    // States
    private Boolean hasBubble = false;
    private Boolean isShrunk = false;
    private Boolean isPaused = false;
    private Boolean crashed = false;
    private Boolean dead = false;

    // Parameters
    private Vector3 ogScale;
    private float tilt = 15f;
    private float strength = 5f;
    private float floatingDegrees = 0f;
    private float alphaIncreaseSpeed = 1f;

    // References
    [SerializeField] private ParticleSystem rain;
    [SerializeField] private AudioClip flapSoundClip;
    [SerializeField] private AudioClip scoreSoundClip;
    [SerializeField] private AudioClip crashSoundClip;
    [SerializeField] private AudioClip dieSoundClip;

    private void Awake() {
        // Get references to components in Awake
        sprite = transform.Find("Sprite");
        spriteRenderer = sprite.GetComponent<SpriteRenderer>();
        birdRigidbody = GetComponent<Rigidbody2D>();
        animator = sprite.GetComponent<Animator>();
        bubble = transform.Find("Bubble").GetComponent<SpriteRenderer>();
        ogScale = transform.localScale;
    }

    // Jump method
    public void Jump() {
        birdRigidbody.velocity = Vector2.up * strength;
        SoundFXManager.Instance.PlaySoundFXClip(flapSoundClip, transform, 1f);
    }

    private void Update() {
        // Current game state
        GameManager.States state = GameManager.instance.state;
        position = transform.position;

        // Reset position on death
        if (animator.GetBool("Dead") && position.y <= -2.6) {
            transform.position = new Vector3(-1.2f, -2.6f, 0);
        }

        // Handle user input
        if (Input.GetMouseButtonDown(0)) {
            // Check game state and perform actions accordingly
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

        // Handle bubble visibility
        if (!hasBubble) {
            bubble.color = new Color(bubble.color.r, bubble.color.g, bubble.color.b, 0f);
        }

        // Check if player is below a certain y-position and destroy if necessary
        CheckDestroyCondition();
    }

    // Apply floating effect
    private void ApplyFloatingEffect() {
        transform.position = new Vector2(-1.2f, 0f) + (Vector2.up * 0.18f * Mathf.Sin(floatingDegrees * Mathf.PI / 180f));
        floatingDegrees = (floatingDegrees + 400f * Time.deltaTime) % 360f;
    }

    // Adjust bird rotation
    private void AdjustBirdRotation() {
        float zRotation = Mathf.Clamp((birdRigidbody.velocity.y + 5f) * tilt, -90f, 25f);
        sprite.transform.eulerAngles = new Vector3(sprite.transform.eulerAngles.x, sprite.transform.eulerAngles.y, zRotation);
    }

    // Check destroy condition
    private void CheckDestroyCondition() {
        if (transform.position.y < -10f) {
            Destroy(gameObject);
        }
    }

    // Handle collisions
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("obstacle")) {
            if (hasBubble) {
                StartCoroutine(Invinsible());
            }
            else {
                if (!crashed) {
                    SoundFXManager.Instance.PlaySoundFXClip(crashSoundClip, transform, 1f);
                    crashed = true;
                }
                animator.SetBool("Dead", true);
                GameManager.instance.GameOver();
            }
        } 
        else if (other.CompareTag("ground")) {
            if (!dead) {
                SoundFXManager.Instance.PlaySoundFXClip(dieSoundClip, transform, 1f);
                dead = true;
            }
            animator.SetBool("Dead", true);
            GameManager.instance.GameOver();
            GameUI.instance.GameOver();
        }
        else if(other.CompareTag("scoring")) {
            SoundFXManager.Instance.PlaySoundFXClip(scoreSoundClip, transform, 1f);
            GameManager.instance.IncreaseScore();
        }
        else if (other.CompareTag("gravityScoring")) {
            GameManager.instance.IncreaseScore();
            StartCoroutine(SwapGravityValuesWithDelay());
        }
        else if (other.CompareTag("bubbleScoring")) {
            GameManager.instance.IncreaseScore();
            hasBubble = true;
            StartCoroutine(FadeInBubble());
        }
        else if (other.CompareTag("shrinkScoring")) {
            GameManager.instance.IncreaseScore();
            Shrink();
        }
        else if (other.CompareTag("collectable")) {
            GameManager.instance.IncreaseScore();
            Destroy(other.gameObject);
        }
    }

    // Shrink player
    private void Shrink() {
        if (!isShrunk) {
            StartCoroutine(ShrinkAndRestore());
        }
    }

    // Coroutine for shrinking and restoring player size
    private IEnumerator ShrinkAndRestore()
    {
        isShrunk = true;
        transform.localScale = ogScale * 0.5f;
        yield return new WaitForSeconds(10);
        transform.localScale = ogScale;
        isShrunk = false;
    }

    // Fade in bubble
    private IEnumerator FadeInBubble() {
        while (bubble.color.a < 0.8f) {
            float newAlpha = Mathf.MoveTowards(bubble.color.a, 1f, alphaIncreaseSpeed * Time.deltaTime);
            bubble.color = new Color(bubble.color.r, bubble.color.g, bubble.color.b, newAlpha);
            yield return null;
        }
    }

    // Swap gravity values with delay
    private IEnumerator SwapGravityValuesWithDelay() {
        float oldGravity = birdRigidbody.gravityScale;
        float oldStrength = strength;

        birdRigidbody.gravityScale = 0;
        strength = 0;
        birdRigidbody.velocity = new Vector2(birdRigidbody.velocity.x, 0f);

        TogglePause();

        yield return new WaitForSeconds(0.5f);

        birdRigidbody.gravityScale = oldGravity * -1;
        strength = oldStrength * -1;
    }

    // Make player invincible temporarily
    private IEnumerator Invinsible() {        
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Obstacle"), true);

        hasBubble = false;
        bubble.color = new Color(bubble.color.r, bubble.color.g, bubble.color.b, 0f);

        float blinkDuration = 2f;
        float blinkInterval = 0.2f;

        while (blinkDuration > 0f) {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(blinkInterval);
            blinkDuration -= blinkInterval;
        }

        spriteRenderer.enabled = true;

        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Obstacle"), false);
    }

    // Toggle pause state
    public void TogglePause() {
        if (isPaused) {
            rain.Play();
        }
        else {
            rain.Pause();
        }

        isPaused = !isPaused;
    }
}