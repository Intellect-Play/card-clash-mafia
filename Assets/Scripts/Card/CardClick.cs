using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;

public class CardClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private float triggerHeight = 150f;
    [SerializeField] private float returnDuration = 0.5f;

    private float startPosY;
    private RectTransform rectTransform;
    private Canvas canvas;
    public CardBase card;

    public Transform spawnParent;
    public CardMoveImage moveImage;
    public CardTypeMove cardType;

    [HideInInspector] public bool Hovering;
    [HideInInspector] public bool CanDrag;
    public bool onDragBool;
    Vector3 GetPos;
    Vector3 SelectedPos;

    private int originalSiblingIndex;
    public bool MoveCard=false;
    public CardState _CardState { get; private set; }

    public enum CardState { Idle, IsDragging, Played }
    public bool Used = false;
    public void Initialize(Transform parent, Canvas canvasRef, GameObject moveImageObj)
    {
        Used = false;
        canvas = canvasRef;
        spawnParent = parent;
        moveImage = moveImageObj.GetComponent<CardMoveImage>();
        moveImage.SetTarget(gameObject, card.mCardSO._CardImage);
        card.mCardMoveImage = moveImage;
        originalSiblingIndex = transform.GetSiblingIndex();

    }

    private void OnEnable()
    {
        onDragBool = false;
        CanDrag = false;
        rectTransform = GetComponent<RectTransform>();
        card = GetComponent<CardBase>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(Used)return;
        if (CardManagerMove.MoveCard) return;
        card.mKing.MoveCardCick(this);
        onDragBool = false;
        CardManagerMove.Instance.ResetCardPositions(this);
        GetPos = transform.position;
        CanDrag = true;
        startPosY = transform.position.y+50;
        SelectedPos = new Vector3(transform.position.x, startPosY, transform.position.z);
        transform.position = SelectedPos;
        card.ClickGiveManagerSelectedCard();
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (Used) return;

        if (TutorialManager.Instance.IsTutorialActive)
        {
            TutorialManager.Instance.TutorialCardSelected();
        }
        if (!CanDrag||!onDragBool) return;
        TutorialManager.Instance.HideTutorialMoveHand();

        if (transform.position.y - startPosY > triggerHeight)
        {
            Used = true;
            card.UseForAllCards();
            return;
        }
        CanDrag = false;

        transform.position = SelectedPos;

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Used) return;

        if (!CanDrag || canvas == null) return;
        onDragBool = true;
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Used) return;

        if (!CanDrag || CardManagerMove.MoveCard) return;

        _CardState = CardState.IsDragging;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Used) return;

        if (!CanDrag) return;

        _CardState = CardState.Idle;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Used) return;

        if (CardManagerMove.MoveCard) return;

        Hovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Used) return;

        if (!CanDrag) return;

        Hovering = false;
    }

    public void ResetCardPosition()
    {
        if (Used) return;


        CanDrag = false;
      
        transform.position = GetPos;
        card.ExitCard();
    }
}