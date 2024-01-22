using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class background : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    public float animationSpeed = 0.1f;

    private void Awake() 
    {
        meshRenderer = GetComponent <MeshRenderer>();
    }

    private void Update() 
    {   
        if (GameManager.instance.State != GameManager.States.GameOver)
        {
            meshRenderer.material.mainTextureOffset += new Vector2(animationSpeed * Time.deltaTime, 0);
        }
    }
}
