using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : BaseUI
{
    TextMeshProUGUI scoreText;
    TextMeshProUGUI comboText;
    TextMeshProUGUI bestScoreText;
    TextMeshProUGUI bestComboText;

    Button startButton;
    Button exitButton;

    // Start is called before the first frame update
    protected override UIState GetUIState()
    {
        return UIState.Score;
    }

    public override void Init(TheStackUIManager uiManager)
    {
        base.Init(uiManager);

        scoreText = transform.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        comboText = transform.Find("ComboText").GetComponent <TextMeshProUGUI>();
        bestScoreText = transform.Find("BestScoreText").GetComponent<TextMeshProUGUI>();
        bestComboText = transform.Find("BestComboText").GetComponent<TextMeshProUGUI>();

        startButton = transform.Find("StartButton").GetComponent<Button>();
        exitButton = transform.Find("ExitButton").GetComponent<Button>();

        startButton.onClick.AddListener(OnClickStartButton);
        exitButton.onClick.AddListener(OnClickExitButton);
    }

    public void SetUI(int score, int combo, int bestScore, int bestCombo)
    {
        scoreText.text = score.ToString();
        comboText.text = combo.ToString();
        bestScoreText.text = bestScore.ToString();
        bestComboText.text = bestCombo.ToString();
    }

    void OnClickStartButton()
    {
        uiManager.OnClickStart();
    }

    void OnClickExitButton()
    {
        uiManager.OnClickExit();
    }
}
