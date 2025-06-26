using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : CardBase
{
    public override CardType _CardType => CardType.Move;
    GameObject gameObjectM;
    public void Awake()
    {
        mCardPower = SaveManager.Instance.cardDataList.cards.Find(x => x.name == _CardType.ToString());

    }
    public override void CheckPathing()
    {
        throw new System.NotImplementedException();
    }
    public override void SelectedCard(bool moveActive = false)
    {
        mKing.CheckPathing();
        //mKing.moveAnime.SetActive(true);
        gameObjectM = Instantiate(mKing.moveAnime, mKing.transform.position, Quaternion.identity, mKing.transform);
        gameObjectM.transform.parent = mKing.transform.parent;
        gameObjectM.SetActive(true);
        if (mKing.mHighlightedCells.Count <= 0) return;

        
        mKing.ShowCells();
        mKing.moveCard = true;
    }
    public override void UseForAllCards()
    {
        //mKing.moveAnime.SetActive(false);

        CardManagerMove.MoveCard = true;
        CardManagerMove.Instance.RemoveSpawnCard(this.gameObject);

        mCardMoveImage.PlayPopFadeAnimation();
        gameObject.SetActive(false);
    }
    public override void UseForMoveCards()
    {
        //mKing.moveAnime.SetActive(false);
        Destroy(gameObjectM);
        CardManagerMove.Instance.RemoveSpawnCard(this.gameObject);

        mCardMoveImage.PlayPopFadeAnimation();
        gameObject.SetActive(false);
    }
    public void ExitFormOneTouch()
    {
        mKing.moveAnime.SetActive(false);

        CardManagerMove.Instance.RemoveSpawnCard(this.gameObject);

        mCardMoveImage.PlayPopFadeAnimation();
        gameObject.SetActive(false);
    }
    public override void ExitCard()
    {
        mKing.moveAnime.SetActive(false);
        Destroy(gameObjectM);

        // UseCard();
        CardManagerMove.MoveCard = false;

        base.ExitCard();
        mKing.moveCard = false;
        mKing.ClearCells();
    }
}
