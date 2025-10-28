using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject startUI;
    [SerializeField] private GameObject gameoverUI;

    [SerializeField] private Button startButton;
    [SerializeField] private Button startExitButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button gameoverExitButton;


    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private TextMeshProUGUI ResultScoreText;

    void Start()
    {
        startButton.onClick.AddListener(OnClickStartButton);
        startExitButton.onClick.AddListener(OnClickExitButton);
        restartButton.onClick.AddListener(OnClickRestartButton);
        gameoverExitButton.onClick.AddListener(OnClickExitButton);
    }

    public void OnClickStartButton() // Start ��ư Ŭ���� ���� ����
    {
        startUI.SetActive(false);
        Time.timeScale = 1.0f;
    }
    public void OnClickRestartButton()
    {
        gameoverUI.SetActive(false);
        UpdateScore(0);
        Time.timeScale = 1.0f;
    }
    public void OnClickExitButton()
    {
        SceneManager.LoadScene("MainScene");
        Time.timeScale = 1.0f;
    }
    public void GameStartUI() // ���� ���� UI ȣ��
    {
        startUI.SetActive(true);
    }
    public void GameOverUI() // ���� ���� UI ȣ��
    {
        gameoverUI.SetActive(true);
    }

    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
        ResultScoreText.text = scoreText.text;
    }

}