using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopButtons : MonoBehaviour
{
    public RectTransform StartButton;
    Button StartButtonB;
    [SerializeField] GameObject BuyImage;
    private void Start()
    {
        if(StartButton != null)  StartButtonB = StartButton.GetComponent<Button>();
        Debug.Log("ShopManager: ShopTutorial - Tutorial Level 0");

        if (BuyImage == null) return;
        if (PlayerPrefs.GetInt("Tutorial2", 0) == 0)
        {
            StartButton.anchorMin = new Vector2(0.5f, 0);
            StartButton.anchorMax = new Vector2(0.5f, 0);
            StartButton.pivot = new Vector2(0.5f, 0);
            StartButton.anchoredPosition = new Vector2(0, 175);
            gameObject.SetActive(false);
        }
        if (StartButton != null && (PlayerPrefs.GetInt("Tutorial2", 0) == 1))
        {
            Debug.Log("ShopManager: ShopTutorial - Tutorial Level 1");

            TutorialManager.Instance.TutorialHandClickButtonShop(this.GetComponent<RectTransform>());

        }

        if (BuyImage != null)BuyImage.SetActive(CheckBuyCondition());
    }

    public void ShopTutorial()
    {
       
    }
    private void OnEnable()
    {
        if(SaveManager.Instance != null&& BuyImage != null) BuyImage.SetActive(CheckBuyCondition());
    }

    public bool CheckBuyCondition()
    {
        List<CardData> _cards = SaveManager.Instance.cardDataList.cards;
        int coin = SaveManager.Instance.saveData.playerData.coins;
        foreach (CardData card in _cards) {
            if (card.isUnlocked&&card.upgradeCost< coin)
            {
                return true;
            }else if(!card.isUnlocked && card.buyCost< coin)
            {
                return true;
            }
        }

        return false;        
    }
    public void OnClickShopButton()
    {
        TutorialManager.Instance.HideTutorialHand();
        if(StartButton != null) StartButtonB.interactable = true;
        GameManagerMain.Instance.OpenShop();
    }
    public void OnClickCloseShopButton()
    {
        if (PlayerPrefs.GetInt("Tutorial2", 0) == 2)
        {
            PlayerPrefs.SetInt("Tutorial2",3);
            TutorialManager.Instance.HideTutorialHand();

            TutorialManager.Instance.IsTutorialActive=false;

            Debug.Log("ShopManager: SelectCard - Tutorial Hand Click");
        }
        GameManagerMain.Instance.CloseShop();
    }
}
