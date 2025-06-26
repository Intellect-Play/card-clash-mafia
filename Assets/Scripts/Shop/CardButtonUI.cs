using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class CardButtonUI : MonoBehaviour
{
    private RectTransform targetRectTransform;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI powerText;
    public Button actionButton;
    public TextMeshProUGUI actionButtonText;
    public Image cardImage;
    public Image cardLockImage;
    public Image cardUpImage;
    public GameObject UpgradeAnimation;
    public Image cardBuyImage;
    public string nameCard;
    [HideInInspector] public int cardId;

    public Color UpdateColor;
    public Color BuyColor;

    private Vector2 originalAnchoredPos;
    private Vector3 originalScale;
    private Vector3 originalRotation;

    public bool upgrade;
    [SerializeField] GameObject CardPowerObject;
    [SerializeField] GameObject CardBuyObject;
    private void Awake()
    {
        UpgradeAnimation.SetActive(false);
        targetRectTransform = GetComponent<RectTransform>();
        // cardImage = GetComponent<Image>();
    }
    public void SetCardDetails(CardData cardData, Sprite image)
    {
        nameCard = cardData.name;
        powerText.text = cardData.power.ToString();
        cardImage.sprite = image;
        ActiveCard(cardData.isUnlocked);
    }
    public void GetColor(bool Buy)
    {
        cardBuyImage.color = Buy ? BuyColor : UpdateColor;
    }
    public void FalseUpdate()
    {
        upgrade = false;
        CardPowerObject.SetActive(false);
        CardBuyObject.SetActive(false);
    }
    public void ActiveCard(bool active)
    {
        cardImage.color = active ? Color.white : new Color(1f, 1f, 1f, 0.5f);
        actionButtonText.text = active ? "Active" : "Inactive";
        cardLockImage.gameObject.SetActive(!active);
        //cardBuyImage.gameObject.SetActive(active);
    }
    public void SelectCard(bool Active)
    {

        targetRectTransform.SetAsLastSibling();

        SetAnchorPivotToCenterPreservePosition();
        if (Active)
        {
            targetRectTransform.DOScale(1.2f, 0.25f)
       .SetEase(Ease.OutBack);
        }
        else
        {
            targetRectTransform.DOScale(1f, 0.25f).SetEase(Ease.OutBack);
        }
       
    }
    public void GetColor(int Level)
    {
        if (Level > ColorManager.Instance.colors.Count)
        {
            Level = ColorManager.Instance.colors.Count;
        }
        CardBuyObject.GetComponent<Image>().color = ColorManager.Instance.colors[Level - 1];
    }
    public void BuyCard()
    {
        BuyUpgradeAnimationCard(false);
    }
    public void UpgradeCard()
    {
        BuyUpgradeAnimationCard(true);

    }
    public void BuyUpgradeAnimationCard(bool Up)
    {
        UpgradeAnimation.SetActive(true);
        targetRectTransform.SetAsLastSibling();
        // 1. Orijinal konum, dönüş ve ölçek kaydedilir
        originalAnchoredPos = targetRectTransform.anchoredPosition;
        originalScale = targetRectTransform.localScale;
        originalRotation = targetRectTransform.localEulerAngles;

        Sequence sequence = DOTween.Sequence();

        // 2. Hafif sağa dönme ve kayma
       // sequence.Append(targetRectTransform.DORotate(new Vector3(0, 15, 0), 0.15f).SetEase(Ease.OutQuad));
        //sequence.Join(targetRectTransform.DOAnchorPos(originalAnchoredPos + new Vector2(100f, 0f), 0.15f));

        // 3. Parent pozisyonuna yaklaşma (diyelim ki merkez)
        RectTransform parentRect = targetRectTransform.parent.GetComponent<RectTransform>();
        Vector2 center = Vector2.zero; // merkeze göre

        sequence.Append(targetRectTransform.DORotate(Vector3.zero, 0.3f).SetEase(Ease.OutQuad));
        sequence.Join(targetRectTransform.DOAnchorPos(new Vector2(0,400), 0.3f).SetEase(Ease.OutQuad));
        sequence.Join(targetRectTransform.DOScale(2f, 0.3f).SetEase(Ease.OutBack));
        ActiveCard(true);
        // 4. Bekleme
        sequence.AppendInterval(1.5f);

        // 5. Eski pozisyona, boyuta ve rotasyona geri dönme
        sequence.Append(targetRectTransform.DOAnchorPos(originalAnchoredPos, 0.25f).SetEase(Ease.InOutQuad));
        sequence.Join(targetRectTransform.DOScale(1, 0.25f).SetEase(Ease.InOutQuad));
        sequence.Join(targetRectTransform.DORotate(originalRotation, 0.25f).SetEase(Ease.InOutQuad)).OnComplete(() => {
            ShopManager.ShopActive = true;
            UpgradeAnimation.SetActive(false);
            if (PlayerPrefs.GetInt("Tutorial2", 0) == 1)
            {
                PlayerPrefs.SetInt("Tutorial2", 2);

                ShopManager.Instance.CloseTutorial();
                //PlayerPrefs.GetInt("Tutorial", 1);
                Debug.Log("ShopManager: SelectCard - Tutorial Hand Click");
            }
        });
    }

    public void SetAnchorPivotToCenterPreservePosition()
    {
        // 1. Dünya pozisyonunu kaydet
        Vector3 worldPos = targetRectTransform.position;

        // 2. Anchor ve pivot'u merkeze ayarla
        targetRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        targetRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        targetRectTransform.pivot = new Vector2(0.5f, 0.5f);

        // 3. Eski dünya pozisyonuna göre anchored position'u yeniden hesapla
        targetRectTransform.position = worldPos;
    }
    public void UpBuyAnime(bool Up)
    {

    }
}
