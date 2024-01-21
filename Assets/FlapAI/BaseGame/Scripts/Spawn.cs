using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject prefab;
    public float spawnRate = 1f;
    public float minHeight = -1f;
    public float maxHeight = 1f;

    private void OnEnable() {
        InvokeRepeating(nameof(Spn),spawnRate,spawnRate);
    }

    private void OnDisable() {
        CancelInvoke(nameof(Spn));
    }

    private void Spn() {
        GameObject pipes = Instantiate(prefab,transform.position, Quaternion.identity);
        pipes.transform.position += Vector3.up * Random.Range(minHeight,maxHeight);
    }
}
