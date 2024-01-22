using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] sprites;

    public float tilt = 5f;

    private int spriteIndex;
    private Vector3 direction;

    public float gravity = -9.8f;

    public float strength = 5f;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        InvokeRepeating(nameof(AnimateSprite), 0.15f, 0.15f);
    }

    private void Update()
    {
        GameManager.States state = GameManager.instance.State;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {

            if (state == GameManager.States.EnterGame)
                GameManager.instance.EnterGame();

            if (state != GameManager.States.GameOver)
                direction = Vector3.up * strength;
        }

        direction.y += gravity * Time.deltaTime;
        transform.position += direction * Time.deltaTime;

        Vector3 rotation = transform.eulerAngles;
        rotation.z = direction.y * tilt;
        transform.eulerAngles = rotation;
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
            direction.x = 0f;
        } 
        else if(other.gameObject.tag == "scoring")
        {
            GameManager.instance.IncreaseScore();
        }
    }
}
