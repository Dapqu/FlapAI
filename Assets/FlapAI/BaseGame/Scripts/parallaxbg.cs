using UnityEngine;

public class Background : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    private float animationSpeed = 0.4f;

    private void Awake() {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update() {   
        // Check if the game state is not GameOver
        if (GameManager.instance.state != GameManager.States.GameOver) {
            // Update the mainTextureOffset of the material to create a scrolling effect
            meshRenderer.material.mainTextureOffset += new Vector2(animationSpeed * Time.deltaTime, 0);
        }
    }
}
