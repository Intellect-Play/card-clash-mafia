using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }


    [Header("Objects")]
    public GameObject tutorialCanvas;
    public GameObject tutorialHand;
    public TutorialHandAnimator tutorialHandAnimator;

    [Header("Tutorial Settings")]
    [SerializeField] private Vector3 ButtonClickOffset = new Vector3(280,70,0);
    [SerializeField] private Button EndTurnButton;
    [SerializeField] private Button StartButton;
    [SerializeField] private Button ShopButton;
    [SerializeField] private RectTransform EndTurnImage;
    private Tweener currentTween;
    public Cell cell;
    public bool IsTutorialActive = false;
    int tutorialLevel;
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
        HideHandTouchEndTurn();
        if (PlayerPrefs.GetInt("Tutorial2", 0) == 0)
        {
            IsTutorialActive = true;
            tutorialLevel = 0;
            StartButton.interactable = true;
            ShopButton.interactable = false;
            ShopButton.GetComponent<ShopButtons>().ShopTutorial();
            //EndTurnButton.interactable = false;
        }
        else if(PlayerPrefs.GetInt("Tutorial2", 0) == 1)
        {
            tutorialHandAnimator.gameObject.SetActive(true);
            IsTutorialActive = true;
            ShopButton.GetComponent<ShopButtons>().ShopTutorial();

            StartButton.interactable = false;
            ShopButton.interactable = true;
          //  tutorialHandAnimator.ShowTapAnimationUI(ShopButton.gameObject.GetComponent<RectTransform>(), new Vector3(-300, 0, 0));

        }
        else
        {
            HideTutorialHand();
            IsTutorialActive = false;

        }
    }

    public void SelectCard(int currentCardCount)
    {

    }
    public void AddTutorial()
    {
        tutorialLevel++;
        switch (tutorialLevel)
        {
            case 0:
                StartCoroutine(ShowTutorialHandWithDelay(0));
                break;
            case 1:
                StartCoroutine(ShowTutorialHandWithDelay(0));
                break;
            case 2:
                StartCoroutine(ShowTutorialHandWithDelay(0));
                break;
            case 3:
                //StartCoroutine(ShowTutorialHandWithDelay());
                break;
            case 4:
                StartCoroutine(ShowTutorialHandWithDelay(0));
                break;
            case 5:
                {
                    StartCoroutine(ShowTutorialHandWithDelay(2));

                }
                break;
            case 7:
                StartCoroutine(ShowTutorialHandWithDelay(3));
                break;
            case 8:
                TutorialCardSelected();
                EndTurnButton.interactable = true;
                break;
            case 9:
                StartCoroutine(ShowTutorialHandWithDelay(2));
                break;
            case 10:
                StartCoroutine(ShowTutorialHandWithDelay(2));
                break;
        }   

    }
    IEnumerator ShowTutorialHandWithDelay(int CardCount)
    {
       // Debug.Log("ShowTutorialHandWithDelay  "+ tutorialLevel);
        yield return new WaitForSeconds(1);
        if (IsTutorialActive) {
            for (int i = 0; i < CardManagerMove.Instance.spawnedCards.Count; i++)
            {
                CardClick card = CardManagerMove.Instance.spawnedCards[i].GetComponent<CardClick>();
                if (i == CardCount)
                {
                    if (tutorialLevel < 3)
                        tutorialHandAnimator.ShowTapAnimationUI(card.gameObject.GetComponent<RectTransform>(), new Vector3(50, 0, 0));
                    else tutorialHandAnimator.ShowMoveHandAnimationUI(card.gameObject.GetComponent<RectTransform>(), new Vector3(50, 0, 0));
                    card.moveImage.FadeInCard();
                    card.enabled = true;
                }
                else
                {
                    if (card != null)
                    {
                        card.moveImage.FadeOutCard();
                        card.enabled = false;
                    }
                }
            }
        }        
    }
    public void TutorialCardSelected()
    {
        if (!IsTutorialActive) return;
        if ( tutorialLevel < 9)
        {

            if (tutorialLevel == 1)
            {
                cell = GameManager.Instance.mBoard.mAllCells[2, 3];
                tutorialHandAnimator.ShowTapAnimationWorldUI(cell.GetComponent<RectTransform>(), new Vector3(3, -3, 0));

            }
            else if (tutorialLevel == 4)
            {
                cell = GameManager.Instance.mBoard.mAllCells[2, 4];
                tutorialHandAnimator.ShowTapAnimationWorldUI(cell.GetComponent<RectTransform>(), new Vector3(3, -3, 0));

            }
            else if (tutorialLevel == 8)
                ShowTapAnimationUIEndTurn();

            else tutorialHandAnimator.ShowTapAnimationWorldUI(GameManager.Instance.mBoard.mAllCells[2, 3].GetComponent<RectTransform>(), Vector3.zero);
        }
    }
    public void CardStartClickTutorial()
    {
        if (tutorialLevel >= 1&&tutorialLevel!=8)
        {
            HideTutorialHand();
        }
    }
    public void ShowTapAnimationUIEndTurn()
    {
       

        // 4. handImage yerləşdir
        EndTurnImage.gameObject.SetActive(true);
        gameObject.SetActive(true);

        // 5. Animasiya təmizlənir və yenidən başlayır
        currentTween?.Kill();

        currentTween = EndTurnImage
            .DOScale(1.3f, 1 * 0.5f)
            .SetEase(Ease.OutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }
    public void HideHandTouchEndTurn()
    {
        currentTween?.Kill();
        currentTween = null;
        EndTurnImage.gameObject.SetActive(false);

    }
    public void TutorialHandClickButton(RectTransform rectTransform)
    {
        if (!IsTutorialActive) return;
        tutorialHandAnimator.ShowTapAnimationUI(rectTransform, ButtonClickOffset);
    }
    public void TutorialHandClickButtonClose(RectTransform rectTransform)
    {
        tutorialHandAnimator.ShowTapAnimationUI(rectTransform, -new Vector3(0,150,0));
    }
    public void TutorialHandClickButtonShop(RectTransform rectTransform)
    {
        if (!IsTutorialActive) return;
        tutorialHandAnimator.ShowTapAnimationUI(rectTransform, new Vector3(200,0,0));
    }
    public void HideTutorialMoveHand()
    {
        if (!IsTutorialActive || tutorialLevel == 1 || tutorialLevel==4) return;
        tutorialHandAnimator.HideHandTouch();
    }
    public void HideTutorialHand()
    {
        if (!IsTutorialActive) return;
        tutorialHandAnimator.HideHandTouch();
    }

    public void CardMoveTutorialinGame()
    {
        if ((CardManagerMove.Instance.currentCardClick != null && !CardManagerMove.Instance.currentCardClick.MoveCard)|| CardManagerMove.Instance.currentCardClick == null)
        {
            if (CardManagerMove.Instance.spawnedCards.Count>0 && CardManagerMove.Instance.spawnedCards[0] != null)
            {
                tutorialHandAnimator.ShowMoveHandAnimationUI(CardManagerMove.Instance.spawnedCards[0].GetComponent<RectTransform>(), new Vector3(50, 0, 0));

            }

        }
    }

    public void MoveCardTutorialinGame()
    {
        if (CardManagerMove.Instance.currentCardClick != null && CardManagerMove.Instance.currentCardClick.MoveCard)
        {
            tutorialHandAnimator.ShowTapAnimationWorldUI(GameManager.Instance.mBoard.mAllCells[2, 3].GetComponent<RectTransform>(), new Vector3(3, -3, 0));

        }
    }

    public void EndGameTutorial()
    {

        tutorialHandAnimator.HideHandTouch();
    }
}
