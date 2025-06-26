using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BattleButton : MonoBehaviour
{
    Button endTurnButton;
    [Header("Fade Panel")]
    public CanvasGroup fadeCanvasGroup;
    public Button ShopButton;
    public float fadeDuration = 0.5f;
    private void Start()
    {
        endTurnButton = GetComponent<Button>();
        endTurnButton.onClick.AddListener(StartBattle);
        fadeCanvasGroup.alpha = 0;
        if ((SaveManager.Instance.saveData.playerData.currentLevel == 1))
        {
            TutorialManager.Instance.TutorialHandClickButton(this.GetComponent<RectTransform>());

        }
    }
    private void OnEnable()
    {
        if(PlayerPrefs.GetInt("Tutorial2", 0) == 3)
        {
            TutorialManager.Instance.IsTutorialActive = true;

            PlayerPrefs.SetInt("Tutorial2", 4); // Set tutorial to next level
            TutorialManager.Instance.TutorialHandClickButton(this.GetComponent<RectTransform>());
            TutorialManager.Instance.IsTutorialActive = false;
            ShopButton.interactable = false; // Disable shop button during tutorial
        }
    }

    void StartBattle() {
        if (PlayerPrefs.GetInt("Tutorial2", 0) == 4)
        {
            TutorialManager.Instance.IsTutorialActive = true;
            ShopButton.interactable = true; // Disable shop button during tutorial

            PlayerPrefs.SetInt("Tutorial2", 5); // Set tutorial to next level
            TutorialManager.Instance.HideTutorialMoveHand();
            TutorialManager.Instance.IsTutorialActive = false;

        }
        Debug.Log("BattleButton: StartBattle - Starting Battle");
        TutorialManager.Instance.HideTutorialHand();
        StartCoroutine(FadeTime());
        
    }

    IEnumerator FadeTime()
    {
        fadeCanvasGroup.alpha = 0;
        // Fade In
        yield return fadeCanvasGroup.DOFade(1f, fadeDuration).SetEase(Ease.InOutQuad).WaitForCompletion();
        GameManagerMain.Instance.BattleStartInMainMenu(true);
    }
}
