using UnityEngine;

public class Spawn : MonoBehaviour
{
    // Reference to the prefab to be spawned
    [SerializeField] private GameObject pipePrefabNormal;

    [Header("Hardmode")]
    [SerializeField] private System.Boolean spawn;
    [SerializeField] private GameObject pipePrefabGravity;
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
        if (GameManager.instance.state == GameManager.States.ActiveGame) {
            GameObject pipes;

            // Generate a random number between 0 and 1
            float randomValue = Random.value;

            // If spawn is true and the random value is less than or equal to 0.2 (20% chance)
            if (spawn && randomValue <= 0.2f) {
                pipes = Instantiate(pipePrefabGravity, transform.position, Quaternion.identity);
            }
            else {
                pipes = Instantiate(pipePrefabNormal, transform.position, Quaternion.identity);
            }

            pipes.transform.position += Vector3.up * Random.Range(minHeight, maxHeight);

            if (spawn) {
                GameObject coins = Instantiate(coinPrefab, new Vector3(transform.position.x + 1.8f, transform.position.y, transform.position.z), Quaternion.identity);
                coins.transform.position += Vector3.up * Random.Range(minCoinHeight, maxCoinHeight);
            }
        }
    }
}