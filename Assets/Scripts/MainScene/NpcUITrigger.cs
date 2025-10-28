using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NpcUITrigger : MonoBehaviour
{

    [SerializeField] private GameObject miniGameUI;

    [SerializeField] private Button chickenStartButton;
    [SerializeField] private Button stackStartButton;
    [SerializeField] private Button exitButton;

    private void Start()
    {
        chickenStartButton.onClick.AddListener(OnClickChickenStartButton);
        stackStartButton.onClick.AddListener(OnClickStackStartButton);
        exitButton.onClick.AddListener(OnClickExitButton);
    }

    public void OnClickChickenStartButton()
    {
        SceneManager.LoadScene("ChickenFly");
        Time.timeScale = 1f;
    }
    public void OnClickStackStartButton()
    {
        Screen.SetResolution(1080, 1920, true); // ºôµå ÈÄ¿¡ ¹Ù²ï´Ù. Game ºä ÀüÈ¯X
        SceneManager.LoadScene("TheStack");
        Time.timeScale = 1f;
    }
    public void OnClickExitButton()
    {
        miniGameUI.SetActive(false);
        Time.timeScale = 1f;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("OK!");
        miniGameUI.SetActive(true);
        Time.timeScale = 0f;

    }

}
