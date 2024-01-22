using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenUI : MonoBehaviour
{

    [SerializeField] GameObject Title;
    [SerializeField] GameObject Bird;
    [SerializeField] GameObject AiTitle;

    RectTransform TitleRect;
    RectTransform BirdRect;
    RectTransform AiTitleRect;

    Vector2 titleStartPosition;
    Vector2 birdStartPosition;
    Vector2 aiTitleStartPosition;

    float titleYPosDegs = 0;

    float bounceSpeed = 400f;
    float bounceRange = 18f;

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

        BirdRect = Bird.GetComponent<RectTransform>();
        birdStartPosition = BirdRect.anchoredPosition;

        AiTitleRect = AiTitle.GetComponent<RectTransform>();
        aiTitleStartPosition = AiTitleRect.anchoredPosition;
    }

    void Update()
    {
        TitleRect.anchoredPosition = titleStartPosition + (Vector2.up * bounceRange * Mathf.Sin(titleYPosDegs * Mathf.PI / 180f));
        BirdRect.anchoredPosition = birdStartPosition + (Vector2.up * bounceRange * Mathf.Sin(titleYPosDegs * Mathf.PI / 180f));
        AiTitleRect.anchoredPosition = aiTitleStartPosition + (Vector2.up * bounceRange * Mathf.Sin(titleYPosDegs * Mathf.PI / 180f));
        titleYPosDegs = (titleYPosDegs + bounceSpeed * Time.deltaTime) % 360;
    }

    void EasyClicked()
    {
        Debug.Log("Enter game");
        GameManager.instance.StartGame();
    }
}
