using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPowerManager : BasePiece
{
    public BasePiece mKing;

    public List<CardBase> mCards = new List<CardBase>();
    public CardBase _SelectedCard;


    private void Start()
    {
        //Invoke("SetupCards", 1);
        //SetupCards();
    }
    public void GetICard(CardBase card, bool moveActive = false)
    {
        
        if (true)
        {
            if(_SelectedCard!=null) _SelectedCard.ExitCard();
           
            _SelectedCard = card;
            //_SelectedCard.GetGameObject();
            _SelectedCard.SelectedCard(moveActive);
        }
    }

    public void SetupThisCard(CardBase _cardBase)
    {
        mCards.Add(_cardBase);
        _cardBase.CardSetup(mKing, this);
    }
    public void SetupCards()
    {
        //foreach (CardBase card in mCards)
        //{
        //    card.CardSetup(mKing, this);
        //}
    }
}

public enum CardType
{
    Move,
    Bomb3x3,
    Bat,
    DoubleFire,
    LinearFire,
    Pyramide,
    PushBack,    
    StrongStrike,
    BombDouble,
    PoisonAttack,
    Meteor5x,
    Sword,
    Healer
}