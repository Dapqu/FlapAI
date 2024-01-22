using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] sprites;

    public Rigidbody2D birdRigidbody;

    public float tilt = 5f;

    private int spriteIndex;

    public float gravity = -9.8f;

    public float strength = 5f;

    public float floating_degrees = 0f;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        InvokeRepeating(nameof(AnimateSprite), 0.1f, 0.1f);
    }

    private void Update()
    {
        GameManager.States state = GameManager.instance.State;

        if (Input.GetMouseButtonDown(0)) {

            if (state == GameManager.States.EnterGame)
                GameManager.instance.EnterGame();

            if (state != GameManager.States.GameOver)
                birdRigidbody.velocity = Vector2.up * strength;
        }

        if (state == GameManager.States.EnterGame)
        {
            transform.position = new Vector2(-1.2f, 0f) + (Vector2.up * 0.18f * Mathf.Sin(floating_degrees * Mathf.PI / 180f));
            floating_degrees = (floating_degrees + 400f * Time.deltaTime) % 360f;
        }
        else
        {
            Debug.Log(birdRigidbody.velocity.y);
            float zRotation = transform.eulerAngles.z;
            if (birdRigidbody.velocity.y <= -4) {
            zRotation = (birdRigidbody.velocity.y + 5f) * tilt;
            }
            else {
                zRotation = 25;
            }
            if (zRotation <= -90f) {
                zRotation = -90f;
            }

            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, zRotation);
        }

        if(transform.position.y < -10f) {
            Destroy(gameObject);
        }
    }

    private void AnimateSprite() {
        spriteIndex++;

        if(spriteIndex >= sprites.Length) {
            spriteIndex = 0;
        }

        spriteRenderer.sprite = sprites[spriteIndex];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "obstacle" || other.gameObject.tag == "ground")
        {
            GameManager.instance.GameOver();
        } 
        else if(other.gameObject.tag == "scoring")
        {
            GameManager.instance.IncreaseScore();
        }
    }
}
