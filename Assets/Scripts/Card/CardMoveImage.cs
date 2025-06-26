using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardMoveImage : MonoBehaviour
{
    //Refences we have to set in the inspector
    [Header("Refences")]
    public GameObject target;
    public GameObject Visual;
    public GameObject Shadow;
    public Image PowerImage;
    public TextMeshProUGUI Powertext;
    public Transform spawnParent;
    public TextMeshProUGUI powerText;

    //public setting changes to our cards movement
    [Header("Settings")]
    public float rotationSpeed;
    public float movementSpeed;
    public float rotationAmount;
    public Vector3 offset;

    [Header("FadeInOut")]
    private float fadeDuration = 0.3f;
    private float fadeColor = 0.7f;

    //private setting changes to our cards movement
    private Vector3 rotation;
    private Vector3 movement;
    private float randomRot;
    private bool Hovering;
    private Vector3 originalScale;
    private bool ActiveCard;

    private void Awake()
    {
       // powerText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        ActiveCard=true;
        if (Visual != null)
            originalScale = Visual.transform.localScale;
        randomRot = Random.Range(-rotationAmount, rotationAmount);
    }

    void Update()
    {
        if (!ActiveCard&& Vector3.Distance(transform.position,target.transform.position)<0.3f)
        {
            Destroy(gameObject);
        }
        if (target == null)
            return;

        //Set our hovering boolean to the targets hovering
        Hovering = target.GetComponent<CardClick>().Hovering && target.GetComponent<CardClick>()._CardState != CardClick.CardState.Played;

        //Set Cards visual to the target cards position
        transform.position = Vector3.Lerp(transform.position, target.transform.position, Time.deltaTime * movementSpeed);
        Visual.transform.position = Vector3.Lerp(Visual.transform.position, (Hovering ) ? target.transform.position + offset : target.transform.position, Time.deltaTime * movementSpeed);

        //If Card is not played
        if (target.GetComponent<CardClick>()._CardState != CardClick.CardState.Played)
        {
            // Calculate position offset relative to the camera
            Vector3 localPos = Camera.main.transform.InverseTransformPoint(transform.position) - Camera.main.transform.InverseTransformPoint(target.transform.position);
            movement = Vector3.Lerp(movement, localPos, 10 * Time.deltaTime); // Adjust speed as needed

            // Sway effect
            Vector3 movementRotation = movement;
            rotation = Vector3.Lerp(rotation, movementRotation, rotationSpeed * Time.deltaTime);

            // Apply sway effect using Quaternion for smoother rotation
            float clampedRotation = Mathf.Clamp(movementRotation.x, -rotationAmount, rotationAmount);
            Quaternion targetRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, clampedRotation);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, randomRot);
        }

       // UpdateCardInfo();
    }
    public void FadeOutCard()
    {
        if (Visual == null || Shadow == null) return;
        // Fade out visual and shadow
        Image visualImage = Visual.GetComponent<Image>();
        Image shadowImage = Shadow.GetComponent<Image>();
       if(PowerImage!=null) PowerImage.DOFade(fadeColor, fadeDuration).SetEase(Ease.OutQuad);
        if (Powertext != null) Powertext.DOFade(fadeColor, fadeDuration).SetEase(Ease.OutQuad);
        if (visualImage != null)
            visualImage.DOFade(fadeColor, fadeDuration).SetEase(Ease.OutQuad);
        if (shadowImage != null)
            shadowImage.DOFade(fadeColor, fadeDuration).SetEase(Ease.OutQuad);
    }
    public void FadeInCard()
    {
        if (Visual == null || Shadow == null) return;
        // Fade in visual and shadow
        Image visualImage = Visual.GetComponent<Image>();
        Image shadowImage = Shadow.GetComponent<Image>();
        if (PowerImage != null) PowerImage.DOFade(1f, fadeDuration).SetEase(Ease.OutQuad);
        if (Powertext != null) Powertext.DOFade(1f, fadeDuration).SetEase(Ease.OutQuad);

        if (visualImage != null)
            visualImage.DOFade(1f, fadeDuration).SetEase(Ease.OutQuad);
        if (shadowImage != null)
            shadowImage.DOFade(1f, fadeDuration).SetEase(Ease.OutQuad);
    }
    public void PlayPopFadeAnimation()
    {
        if (Visual == null || Shadow == null) return;

        // İkisini də aktiv et (əgər əvvəl gizlidirsə)
        Visual.SetActive(true);
        Shadow.SetActive(true);

        // Fade və Scale üçün komponentləri al
        Image visualImage = Visual.GetComponent<Image>();
        Image shadowImage = Shadow.GetComponent<Image>();

        // Şəffaflığı sıfırdan başlatma
        if (visualImage != null)
            visualImage.color = new Color(visualImage.color.r, visualImage.color.g, visualImage.color.b, 1f);
        if (shadowImage != null)
            shadowImage.color = new Color(shadowImage.color.r, shadowImage.color.g, shadowImage.color.b, 1f);

        // Ölçünü sıfırla
        Visual.transform.localScale = originalScale;
        Shadow.transform.localScale = originalScale;

        // Ölçünü bir az böyüt
        Visual.transform.DOScale(originalScale * 1.7f, 0.4f).SetEase(Ease.OutBack);
        Shadow.transform.DOScale(originalScale * 1.7f, 0.4f).SetEase(Ease.OutBack);

        // Fade və disable
        if (visualImage != null)
        {
            visualImage.DOFade(0f, 0.6f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() => Visual.SetActive(false));
        }
        if (PowerImage != null) PowerImage.DOFade(0f, 0.6f)
               .SetEase(Ease.OutQuad)
               .OnComplete(() => Visual.SetActive(false));
        if (Powertext != null) Powertext.DOFade(0f, 0.6f)
               .SetEase(Ease.OutQuad)
               .OnComplete(() => Visual.SetActive(false));
        if (shadowImage != null)
        {
            shadowImage.DOFade(0f, 0.6f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() => Shadow.SetActive(false));
        }
    }

    public void CardDestroy()
    {
        target.GetComponent<CardBase>().ExitCard();
        ActiveCard = false;
    }
    public void ReturnParent()
    {
        transform.parent = spawnParent;
    }
    public void SetTarget(GameObject newTarget, Sprite sprite)
    {
        target = newTarget;
        Visual.GetComponent<Image>().sprite = sprite;
        Shadow.GetComponent<Image>().sprite = sprite;

    }
    //public void UpdateCardInfo()
    //{
    //    CardTypeMove Info = target.GetComponent<Card>().cardType;

    //    //Icon.sprite = Info.CardIcon;        
    //}
}
