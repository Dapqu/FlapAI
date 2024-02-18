using UnityEngine;
using System.Collections.Generic;

public class Spawn : MonoBehaviour
{
    // Reference to the prefab to be spawned
    [SerializeField] private GameObject pipePrefabNormal;

    [Header("Hardmode")]
    [SerializeField] private System.Boolean spawn;
    [SerializeField] private GameObject pipePrefabGravity;
    [SerializeField] private GameObject pipePrefabBubble;
    [SerializeField] private GameObject coinPrefab;

    private float spawnRate = 1.5f;

    private float minHeight = -1f;
    private float maxHeight = 3f;

    private float minCoinHeight = -2f;
    private float maxCoinHeight = 4f;

    // The left edge beyond which the pipes are destroyed
    private float leftEdge = -3.2f;

    //Queue to store pipes
    Queue<GameObject> pipes = new Queue<GameObject>();
    GameObject hanging_pipe = null;


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
            GameObject pipe;

            // 0 to 1
            float randomValue = Random.value;

            // If spawn is true and the random value is less than or equal to 0.1 (10% chance)
            if (spawn && randomValue <= 0.1f) {
                pipe = Instantiate(pipePrefabBubble, transform.position, Quaternion.identity);
            }
            // If spawn is true and the random value is greater than 0.1 but less than or equal to 0.3 (20% chance)
            else if (spawn && randomValue > 0.1f && randomValue <= 0.3f) {
                pipe = Instantiate(pipePrefabGravity, transform.position, Quaternion.identity);
            }
            else {
                pipe = Instantiate(pipePrefabNormal, transform.position, Quaternion.identity);
            }

            pipe.transform.position += Vector3.up * Random.Range(minHeight, maxHeight);

            if (spawn) {
                GameObject coins = Instantiate(coinPrefab, new Vector3(transform.position.x + 1.8f, transform.position.y, transform.position.z), Quaternion.identity);
                coins.transform.position += Vector3.up * Random.Range(minCoinHeight, maxCoinHeight);
            }
            pipes.Enqueue(pipe);

        }
    }

    private void Update() {
        if (pipes.Count > 0 && pipes.Peek().transform.position.x < -1.3f) {
            hanging_pipe = pipes.Peek();
            pipes.Dequeue();
        }
        if (hanging_pipe != null && hanging_pipe.transform.position.x < leftEdge) {
            Destroy(hanging_pipe);
            hanging_pipe = null;
        }
    }

    public Vector3 GetClosestPipePos() {
        if (pipes.Count > 0)
            return pipes.Peek().transform.position;
        else
            return transform.position;
    }

    public void Reset() {
        foreach (GameObject obj in pipes) {
            Destroy(obj);
        }
        if (hanging_pipe != null) {
            Destroy(hanging_pipe);
            hanging_pipe = null;
        }
        pipes.Clear();
    }
}