using UnityEngine;

public class Spawn : MonoBehaviour
{
    // Reference to the prefab to be spawned
    [SerializeField] private GameObject pipePrefab;

    [Header("Coins")]
    [SerializeField] private System.Boolean spawn;
    [SerializeField] private GameObject coinPrefab;

    private float spawnRate = 1.5f;

    private float minHeight = -1f;
    private float maxHeight = 3f;

    private float minCoinHeight = -2f;
    private float maxCoinHeight = 4f;

    private void OnEnable() {
        if (spawn) {
            spawnRate = 2f;
        }
        InvokeRepeating(nameof(Spn), spawnRate, spawnRate);
    }

    private void OnDisable() {
        CancelInvoke(nameof(Spn));
    }

    private void Spn() {
        // Check if the game state is ActiveGame
        if (GameManager.instance.state == GameManager.States.ActiveGame) {
            // Instantiate the prefab at the spawn point with a random height
            GameObject pipes = Instantiate(pipePrefab, transform.position, Quaternion.identity);
            pipes.transform.position += Vector3.up * Random.Range(minHeight, maxHeight);

            if (spawn) {
                GameObject coins = Instantiate(coinPrefab, new Vector3(transform.position.x + 1.8f, transform.position.y, transform.position.z), Quaternion.identity);
                coins.transform.position += Vector3.up * Random.Range(minCoinHeight, maxCoinHeight);
            }
        }
    }
}