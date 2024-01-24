using UnityEngine;

public class Spawn : MonoBehaviour
{
    // Reference to the prefab to be spawned
    [SerializeField] private GameObject prefab;

    private float spawnRate = 1.5f;

    private float minHeight = -1.2f;
    private float maxHeight = 3.2f;

    private void OnEnable() {
        InvokeRepeating(nameof(Spn), spawnRate, spawnRate);
    }

    private void OnDisable() {
        CancelInvoke(nameof(Spn));
    }

    private void Spn() {
        // Check if the game state is ActiveGame
        if (GameManager.instance.state == GameManager.States.ActiveGame) {
            // Instantiate the prefab at the spawn point with a random height
            GameObject pipes = Instantiate(prefab, transform.position, Quaternion.identity);
            pipes.transform.position += Vector3.up * Random.Range(minHeight, maxHeight);
        }
    }
}