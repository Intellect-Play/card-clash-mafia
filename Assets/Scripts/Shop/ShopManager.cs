using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using System.Collections;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;
    [Header("UI")]
    [SerializeField] BuyUpgradeShopButton BuyorUpgradeButton;
    [SerializeField] TextMeshProUGUI CoinText;
    [SerializeField] public Button CloseButton;

    public static bool ShopActive = true;
    public Transform cardButtonParent;
    public GameObject cardButtonPrefab;

    public CardDataList cardDataList;
    public string jsonPath;
    public Dictionary<int, CardList> cardButtons = new Dictionary<int, CardList>();

    private GetcardImage getcardImage;
    private int selectedCardId = -1;
    private CardAction selectedCardAction = CardAction.None;
    int coins;

    public int CardUpdateCosts = 150;
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
        ShopActive = true;
        getcardImage = GetComponent<GetcardImage>();
        //DeleteSaveFile();
        cardDataList = SaveManager.Instance.cardDataList;
        CoinText.text= SaveManager.Instance.saveData.playerData.coins.ToString();
        StartCoroutine(StartTime());
        Debug.Log("Enable Shop");
      
    }
    private void OnEnable()
    {
      
    }
    public void CloseTutorial() {
        CloseButton.interactable = true;
        TutorialManager.Instance.TutorialHandClickButtonClose(CloseButton.GetComponent<RectTransform>());
        for (int i = 0; i < cardButtons.Count; i++)
        {
            if (i != 5)
            {
                cardButtons[i].cardButtonUI.GetComponent<Button>().interactable = true; // Diğer kartları devre dışı bırak
            }
        }
    }

    IEnumerator StartTime()
    {
       
        CreateCardButtons(); 
        yield return new WaitForSeconds(.1f);

        cardButtonParent.GetComponent<GridLayoutGroup>().enabled = false; // GridLayoutGroup'u devre dışı bırak
        BuyorUpgradeButton.button.onClick.AddListener(BuyOrUpgradeFunc);
        yield return new WaitForSeconds(.1f);

        if (PlayerPrefs.GetInt("Tutorial2", 0) == 1)
        {
           // PlayerPrefs.SetInt("Tutorial", 2);
            TutorialManager.Instance.TutorialHandClickButton(cardButtons[5].cardObject.GetComponent<RectTransform>());
            for (int i = 0; i < cardButtons.Count; i++)
            {
                if (i!=5)
                {
                    cardButtons[i].cardButtonUI.GetComponent<Button>().interactable = false; // Diğer kartları devre dışı bırak
                }
            }
            CloseButton.interactable = false;
        }else if(PlayerPrefs.GetInt("Tutorial2", 0) == 2) TutorialManager.Instance.HideTutorialHand();

        else CloseButton.interactable = true;

    }

    public void DeleteSaveFile()
    {
        jsonPath = Path.Combine(Application.persistentDataPath, "Data/cards.json");

        if (File.Exists(jsonPath))
        {
            File.Delete(jsonPath);
            Debug.Log("cards.json dosyası silindi: " + jsonPath);
        }
        else
        {
            Debug.LogWarning("Silinecek cards.json dosyası bulunamadı: " + jsonPath);
        }
    }


    private void SaveData()
    {
        SaveManager.Instance.SaveData();

        //string directory = Path.GetDirectoryName(jsonPath);
        //if (!Directory.Exists(directory))
        //{
        //    Directory.CreateDirectory(directory);
        //}

        //string json = JsonUtility.ToJson(cardDataList, true);
        //File.WriteAllText(jsonPath, json);
    }




    #region UI

    private void CreateCardButtons()
    {
        foreach (var card in cardDataList.cards)
        {
            GameObject buttonGO = Instantiate(cardButtonPrefab, cardButtonParent);
            cardButtons[card.id] = new CardList(buttonGO);
            buttonGO.GetComponent<CardButtonUI>().SetCardDetails(
                card,
                getcardImage.cardImagesList.Find(x => x.cardType.ToString() == card.name).cardImage
            );
            UpdateCardButtonUI(card);
        }
    }

    private void UpdateCardButtonUI(CardData card)
    {

        GameObject btn = cardButtons[card.id]?.cardObject;
        var ui = btn.GetComponent<CardButtonUI>();
        ui.cardId = card.id;
        ui.nameText.text = card.name;
        ui.powerText.text = card.power.ToString();

        ui.actionButton.onClick.RemoveAllListeners();
        if (card.isUnlocked)
        {
            //cardButtons[card.id].cardButtonUI.UpgradeCard();
            ui.actionButtonText.text = (CardUpdateCosts + card.level * 50).ToString();
            ui.GetColor(false);
            ui.actionButton.onClick.AddListener(() => SelectCard(card, CardAction.Upgrade));
            if(!card.update||card.level>=10)ui.FalseUpdate();
            else
            {
                ui.GetColor(card.level);
                //if (cardDataList.cards[selectedCardId] != null)
                //{
                //    int buyOrUpgrade = cardDataList.cards[selectedCardId].upgradeCost;
                //    bool activeBuy = coins >= cardDataList.cards[selectedCardId].upgradeCost;
                //    Debug.Log("UpdateCardButtonUI: " + cardDataList.cards[selectedCardId].name + " with cost: " + cardDataList.cards[selectedCardId].upgradeCost);
                //    BuyorUpgradeButton.SetButtonText(activeBuy, buyOrUpgrade, CardAction.Upgrade);
                //}

            }
        }
        else
        {
            ui.GetColor(true);

            ui.actionButtonText.text = $"Buy";
            ui.actionButton.onClick.AddListener(() => SelectCard(card,CardAction.Buy));
        }
    }

    #endregion

    #region Actions

    public void BuyCard(int cardId)
    {
        var card = cardDataList.cards.Find(c => c.id == cardId);
        if (card == null || card.isUnlocked) return;

        coins = SaveManager.Instance.saveData.playerData.coins; // debug amaçlı varsayılan yüksek
        if (coins >= card.buyCost)
        {
            cardButtons[cardId].cardButtonUI.BuyCard();
            SaveManager.Instance.saveData.playerData.coins = coins;

            coins -= card.buyCost;
            PlayerPrefs.SetInt("Coins", coins);

            card.isUnlocked = true;
            SaveData();
            UpdateCardButtonUI(card);
            if (cardDataList.cards[cardId].update)
            {
                int buyOrUpgrade = CardUpdateCosts + card.level * 50;
                bool activeBuy = coins >= buyOrUpgrade;
                BuyorUpgradeButton.SetButtonText(activeBuy, buyOrUpgrade, CardAction.Upgrade);
            }
         
        }
        else
        {
            ShopActive = true;

        }
    }
  
  
    public void UpgradeCard(int cardId)
    {
        var card = cardDataList.cards.Find(c => c.id == cardId);
        if (card == null || !card.isUnlocked) return;

        coins = SaveManager.Instance.saveData.playerData.coins;
        int cardCost= CardUpdateCosts+card.level * 50; // Her seviye için 50 ekleniyor
        if (coins >= cardCost)
        {
            cardButtons[cardId].cardButtonUI.UpgradeCard();

            coins -= cardCost;
            SaveManager.Instance.saveData.playerData.coins = coins;
            PlayerPrefs.SetInt("Coins", coins);
            card.level++;
            card.power += 2;
            card.upgradeCost += 50;
            SaveData();
            UpdateCardButtonUI(card);

            int buyOrUpgrade = CardUpdateCosts + card.level * 50;
            bool activeBuy = coins >= buyOrUpgrade;
            BuyorUpgradeButton.SetButtonText(activeBuy, buyOrUpgrade, CardAction.Upgrade);
        }
        else
        {
            ShopActive = true;
        }
    }

    public void SelectCard(CardData card, CardAction cardAction)
    {
        if (!ShopActive) return;
        if (card.isUnlocked && !card.update) return;
        if (selectedCardId != -1) cardButtons[selectedCardId].cardButtonUI.SelectCard(false);
        cardButtons[card.id].cardButtonUI.SelectCard(true);
        selectedCardAction = cardAction;
        selectedCardId = card.id;
        int coins = SaveManager.Instance.saveData.playerData.coins;
        int buyOrUpgrade = cardAction == CardAction.Buy ? card.buyCost : (CardUpdateCosts + card.level * 50);
        bool activeBuy = cardAction == CardAction.Buy ? coins >= card.buyCost : coins >= buyOrUpgrade;
        BuyorUpgradeButton.SetButtonText(activeBuy, buyOrUpgrade, cardAction);
        if(PlayerPrefs.GetInt("Tutorial2", 0) == 1)
        {
            TutorialManager.Instance.TutorialHandClickButton(BuyorUpgradeButton.GetComponent<RectTransform>());
            //PlayerPrefs.GetInt("Tutorial", 1);

        }else if (PlayerPrefs.GetInt("Tutorial2", 0) == 2)
        {
            TutorialManager.Instance.HideTutorialHand();
        }
    }


    #endregion


    public void BuyOrUpgradeFunc()
    {
        if (!ShopActive) return;

        if (selectedCardId == -1 || selectedCardAction == CardAction.None)
        {
            return;
        }
        ShopActive = false; // Disable shop interaction while processing
        if (selectedCardAction == CardAction.Buy)
        {
            BuyCard(selectedCardId);
        }
        else if (selectedCardAction == CardAction.Upgrade)
        {
            UpgradeCard(selectedCardId);
        }
        SaveManager.Instance.saveData.playerData.coins = coins;
        SaveManager.Instance.Save();
        CoinText.text = coins.ToString();
        if (PlayerPrefs.GetInt("Tutorial2", 0) == 1)
        {
            TutorialManager.Instance.HideTutorialMoveHand();

        }
        // UpdateCardButtonUI(selectedCardData);
    }
}
public enum CardAction
{
    Buy,
    Upgrade,
    None
}
[System.Serializable]
public class CardList
{
    public GameObject cardObject;
    public CardButtonUI cardButtonUI;

    public CardList(GameObject cardObject)
    {
        this.cardObject = cardObject;
        this.cardButtonUI = cardObject.GetComponent<CardButtonUI>();
    }
}