using UnityEngine;

public class Spawn : MonoBehaviour
{
    // Reference to the prefab to be spawned
    [SerializeField] private GameObject pipePrefabNormal;

    [Header("Hardmode")]
    [SerializeField] private System.Boolean spawn;
    [SerializeField] private GameObject pipePrefabGravity;
    [SerializeField] private GameObject pipePrefabBubble;
    [SerializeField] private GameObject pipePrefabShrink;
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

            // 0 to 1
            float randomValue = Random.value;

            // If spawn is true and the random value is less than or equal to 0.1 (10% chance)
            if (spawn && randomValue <= 0.1f) {
                pipes = Instantiate(pipePrefabBubble, transform.position, Quaternion.identity);
            }
            // If spawn is true and the random value is greater than 0.1 but less than or equal to 0.3 (20% chance)
            else if (spawn && randomValue > 0.1f && randomValue <= 0.3f) {
                pipes = Instantiate(pipePrefabGravity, transform.position, Quaternion.identity);
            }
            else if (spawn && randomValue > 0.3f && randomValue <= 0.4f) {
                pipes = Instantiate(pipePrefabShrink, transform.position, Quaternion.identity);
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