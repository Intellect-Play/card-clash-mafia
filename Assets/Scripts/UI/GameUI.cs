using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance;
    [Header("Game UI Panels")]
    [SerializeField] private GamePanel GameUIPanel;
    [SerializeField] private GamePanel NextGameUIPanel;
    [SerializeField] private GamePanel GameOverUIPanel;

    [Header("UIElements")]
    [SerializeField]private TextMeshProUGUI scoreText;
    [SerializeField]private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI levelText;


    [Header("Fade Panel")]
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 0.5f;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        CardManagerMove.Instance.CheckOpenedCards();

        StartCoroutine(FadeThenLoad());

        ActivePanel(UIPanelType.GameUI);

        healthText.text = SaveManager.Instance.saveData.playerData.health.ToString();
        scoreText.text = SaveManager.Instance.saveData.playerData.coins.ToString();
        levelText.text = SaveManager.Instance.saveData.playerData.currentLevel.ToString();
    }


    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }

    public void UpdateHealth(int health)
    {
        healthText.text = health.ToString();
    }

    public void WinGame()
    {
        ActivePanel(UIPanelType.NextGameUI);
    }

    public void LoseGame()
    {
        ActivePanel(UIPanelType.GameOverUI);
    } 
    public void ActivePanel(UIPanelType panelType)
    {
        GameUIPanel.gameObject.SetActive(panelType == UIPanelType.GameUI);
        NextGameUIPanel.gameObject.SetActive(panelType == UIPanelType.NextGameUI);
        GameOverUIPanel.gameObject.SetActive(panelType == UIPanelType.GameOverUI);
        
    }

    private IEnumerator FadeThenLoad()
    {
  
        yield return fadeCanvasGroup.DOFade(0f, 2).SetEase(Ease.InOutQuad).WaitForCompletion();

    }

   
}

public enum UIPanelType
{
    GameUI,
    NextGameUI,
    GameOverUI
}