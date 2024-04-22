using System;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenUI : MonoBehaviour
{
    RectTransform TitleRect;
    RectTransform BirdRect;
    RectTransform AiTitleRect;

    Vector2 titleStartPosition;
    Vector2 birdStartPosition;
    Vector2 aiTitleStartPosition;

    float titleYPosDegs = 0;

    float bounceSpeed = 400f;
    float bounceRange = 18f;

    Button Easy;
    Button Hard;
    
    Button AiEasy;
    Button AiHard;


    private void Awake() {
        TitleRect = transform.Find("Title").GetComponent<RectTransform>();
        BirdRect = transform.Find("Bird").GetComponent<RectTransform>();
        AiTitleRect = transform.Find("AiTitle").GetComponent<RectTransform>();

        Easy = transform.Find("Easy").GetComponent<Button>();
        Hard = transform.Find("Hard").GetComponent<Button>();
        AiEasy = transform.Find("AiEasy").GetComponent<Button>();
        AiHard = transform.Find("AiHard").GetComponent<Button>();

        Easy.onClick.AddListener(EasyClicked);
        Hard.onClick.AddListener(HardClicked);
        AiEasy.onClick.AddListener(AiEasyClicked);
        AiHard.onClick.AddListener(AiHardClicked);
    }

    void Start() {
        titleStartPosition = TitleRect.anchoredPosition;
        birdStartPosition = BirdRect.anchoredPosition;
        aiTitleStartPosition = AiTitleRect.anchoredPosition;
    }

    void Update() {
        TitleRect.anchoredPosition = titleStartPosition + (Vector2.up * bounceRange * Mathf.Sin(titleYPosDegs * Mathf.PI / 180f));
        BirdRect.anchoredPosition = birdStartPosition + (Vector2.up * bounceRange * Mathf.Sin(titleYPosDegs * Mathf.PI / 180f));
        AiTitleRect.anchoredPosition = aiTitleStartPosition + (Vector2.up * bounceRange * Mathf.Sin(titleYPosDegs * Mathf.PI / 180f));
        titleYPosDegs = (titleYPosDegs + bounceSpeed * Time.deltaTime) % 360;
    }

    void EasyClicked() {
        Debug.Log("Enter Easy game");
        GameManager.instance.StartEastMode();
    }

    void HardClicked() {
        Debug.Log("Enter Hard game");
        GameManager.instance.StartHardMode();
    }

    void AiEasyClicked() {
        Debug.Log("Enter AI Easy game");
        GameManager.instance.StartAiEasyMode();
    }

    private void AiHardClicked() {
        Debug.Log("Enter AI Hard game");
        GameManager.instance.StartAiHardMode();
    }
}