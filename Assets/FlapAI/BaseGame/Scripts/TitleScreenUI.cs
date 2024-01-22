using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenUI : MonoBehaviour
{

    [SerializeField] GameObject Title;
    RectTransform TitleRect;
    Vector2 titleStartPosition;
    float titleYPosDegs = 0;

    [SerializeField] Button Easy;
    [SerializeField] Button Hard; //not used yet


    private void Awake()
    {
        Easy.onClick.AddListener(EasyClicked);
    }

    void Start()
    {
        TitleRect = Title.GetComponent<RectTransform>();
        titleStartPosition = TitleRect.anchoredPosition;
    }

    void Update()
    {
        TitleRect.anchoredPosition = titleStartPosition + (Vector2.up * 12 * Mathf.Sin(titleYPosDegs * Mathf.PI / 180f));
        titleYPosDegs = (titleYPosDegs + 120 * Time.deltaTime) % 360;
    }

    void EasyClicked()
    {
        Debug.Log("Enter game");
        GameManager.instance.StartGame();
    }
}
