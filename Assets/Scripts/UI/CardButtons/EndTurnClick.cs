using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTurnClick : MonoBehaviour
{

    Button endTurnButton;
    RectTransform rectTransform;
    bool ActiveEndTurn;
    private void OnEnable()
    {
        ActiveEndTurn = true;
        endTurnButton = GetComponent<Button>();
        //endTurnButton.onClick.AddListener(EndTurnButton);
        EndTurnButton(1);
        rectTransform = GetComponent<RectTransform>();
        //rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, CardManagerMove.Instance.rectTransformCardLimit.GetComponent<RectTransform>().rect.width / CardManagerMove.Instance.CardLimit - CardManagerMove.Instance.spawnSpace);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rectTransform.rect.height);
    }
    private void Start()
    {
        if(SaveManager.Instance.saveData.playerData.currentLevel == 1)
        {
            endTurnButton.interactable = false;
        }
    }
    public void EndTurnButton(int t)
    {
        StartCoroutine(WaitForEndTurn(t));
        if (!TutorialManager.Instance.IsTutorialActive)
        {
            endTurnButton.interactable = true;

        }
        else
        {
            Debug.Log("EndTurnButton Tutorial");
            TutorialManager.Instance.HideHandTouchEndTurn();
        }
    }

    IEnumerator WaitForEndTurn(int timeStart)
    {
        yield return new WaitForSeconds(timeStart);
        if (ActiveEndTurn == true)
        {
            ActiveEndTurn = false;
            endTurnButton.interactable = false;
            //CardManager.Instance.ExitTurnButton();
            GameManager.Instance.EndTurn();
            CardManager.Instance.cardManagerMove.ReturnAllCards();
            yield return new WaitForSeconds(1.2f);
            CardManager.Instance.cardManagerMove.SpawnCards();
            yield return new WaitForSeconds(1);
            ActiveEndTurn = true;
            if (SaveManager.Instance.saveData.playerData.currentLevel != 1)
            {
                endTurnButton.interactable = true;
            }

        }
         

    }
}
