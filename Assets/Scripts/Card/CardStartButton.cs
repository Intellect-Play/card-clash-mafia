using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardStartButton : MonoBehaviour
{
    public Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(CardUse);      
    }
    private void Start()
    {
        gameObject.SetActive(false);
    }
    public void CardUse()
    {
    
        CardManagerMove.Instance.UseCurrentCard();
        TutorialManager.Instance.CardStartClickTutorial();
    }
}
