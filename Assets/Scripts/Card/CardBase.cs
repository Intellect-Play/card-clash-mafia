using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(CardClick))]

public abstract class CardBase : BasePiece
{
    public BasePiece mKing;
    [NonSerialized] public CardPowerManager cardPowerManager;
    [NonSerialized] public List<Cell> Enemies = new();
    protected List<Cell> HighlightedCells = new();
    protected List<Cell> EnemylightedCells = new();

    [NonSerialized] public CardMoveImage mCardMoveImage;
    public CardSO mCardSO;

    private bool used;
    public bool PowerBool;
    [SerializeField] public TextMeshProUGUI mCardPowerText;
    public CardData mCardPower;

    private void Start()
    {
        GetComponent<Image>().sprite = mCardSO._CardImage;
        used = false;
        cardPowerManager = GameManager.Instance.mCardPowerManager;
        cardPowerManager.SetupThisCard(this);
        mCardPowerText = GetComponentInChildren<TextMeshProUGUI>();
        mCardMoveImage.powerText.text = mCardPower.power.ToString();
        if(PowerBool)Destroy(mCardMoveImage.PowerImage.gameObject);
        else
        {
            mCardMoveImage.PowerImage.color = ColorManager.Instance.colors[mCardPower.level - 1];
        }
    }

    public virtual void CardSetup(BasePiece king, CardPowerManager manager)
    {
        mKing = king;
        cardPowerManager = manager;
        mMovement = new Vector3Int(0, 15, 0);
        mColor = Color.white;
    }

    public virtual void SelectedCard(bool moveActive = false)
    {
        if (used) return;
        mCurrentCell = mKing.mCurrentCell;
        CheckPathing();
        ShowCells();
    }

    public virtual void UseForAllCards()
    {
        GetComponent<CardClick>().Used = true;
        Debug.Log("UseForAllCards: " + gameObject.name);
        mKing.AttackAnimation();
        CardManagerMove.Instance.RemoveSpawnCard(this.gameObject);

        CameraShake.Instance.ShakeCardAttack();
        UseCard();
        mCardMoveImage.PlayPopFadeAnimation();
        GameManager.Instance.EndTurnButton();

        gameObject.SetActive(false);
    }
    public virtual void UseForMoveCards()
    {
       
    }
    public virtual void UseCard()
    {
        if (used) return;
        foreach (var cell in Enemies)
            cell.RemovePiece(mCardPower.power);
        ExitCard();
    }

    public virtual void ExitCard()
    {
        ClearCells();
        Enemies.Clear();
    }

    public void ClickGiveManagerSelectedCard(bool moveActive = false)
    {
        if (used) return;
        cardPowerManager.GetICard(this,moveActive);
    }

    public void Vibrate()
    {
        Handheld.Vibrate();
    }
   
    public GameObject GetGameObject() => gameObject;

    public abstract override void CheckPathing();
    public abstract CardType _CardType { get; }
}

