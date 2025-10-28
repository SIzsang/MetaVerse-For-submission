using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    static GameManager gameManager; // ΩÃ±€≈Ê
    public static GameManager Instance { get { return gameManager; } }

    UIManager uiManager;
    public UIManager UIManager { get { return uiManager; } }

    BgLooper bgLooper;
    public BgLooper BgLooper { get { return bgLooper; } }

    private int currentScore = 0;

    private void Awake()
    {
        gameManager = this;
        uiManager = FindObjectOfType<UIManager>();
        bgLooper = FindObjectOfType<BgLooper>();
    }
    void Start()
    {
        uiManager.UpdateScore(0);
        GameStart();
    }
    public void GameStart()
    {
        uiManager.GameStartUI();
        Time.timeScale = 0f;
    }
    public void GameOver() // GameOverUI ∫∏ø©¡÷±‚
    {
        uiManager.GameOverUI();
        bgLooper.Reset();
        Time.timeScale = 0f;
    }

    public void AddScore(int score)
    {
        currentScore += score;
        uiManager.UpdateScore(currentScore);
    }



}
