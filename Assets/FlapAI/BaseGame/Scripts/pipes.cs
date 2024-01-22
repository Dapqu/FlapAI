using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pipes : MonoBehaviour
{
    private float speed = 2.4f;
    private float leftEdge;

    private void Start()
    {
        leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 1f;
    }

    private void Update()
    {
        if (GameManager.instance.State != GameManager.States.GameOver)
            transform.position += Vector3.left * speed * Time.deltaTime;
    
        if(transform.position.x < leftEdge) {
            Destroy(gameObject);
        }
    }
}
