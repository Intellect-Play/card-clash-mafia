using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Pyramide : CardBase
{
   
    public override CardType _CardType => CardType.Pyramide;

 
    Cell cell;

    public void Awake()
    {
        mCardPower = SaveManager.Instance.cardDataList.cards.Find(x => x.name == _CardType.ToString());

    }
    public override void CardSetup(BasePiece basePiece, CardPowerManager _cardPowerManager)
    {

        base.CardSetup(basePiece, _cardPowerManager);

        mMovement = mCardSO._CardPoweraArea;  //5.0.0
    }

    public override void CreateCellPath(int xDirection, int yDirection, int movement)
    {
        int currentX = mCurrentCell.mBoardPosition.x-3;
        int currentY = mCurrentCell.mBoardPosition.y;
        CheckPaths(xDirection, yDirection, movement,  currentX,  currentY+1);
        CheckPaths(xDirection, yDirection, movement-2, currentX+1, currentY + 2);
        CheckPaths(xDirection, yDirection, movement-4, currentX+2, currentY + 3);
        CheckPaths(xDirection, yDirection, movement, currentX, currentY - 1);
        CheckPaths(xDirection, yDirection, movement - 2, currentX + 1, currentY - 2);
        CheckPaths(xDirection, yDirection, movement - 4, currentX + 2, currentY - 3);
        //CheckPaths(xDirection, yDirection, movement,  currentX,  currentY);
        //CheckPaths(xDirection, yDirection, movement,  currentX-1,  currentY);

    }
    public override void UseForAllCards()
    {
        CardManagerMove.Instance.RemoveSpawnCard(this.gameObject);

       
        UseCard();
        mCardMoveImage.PlayPopFadeAnimation();
        GameManager.Instance.EndTurnButton();

        gameObject.SetActive(false);
        
    }
    public override void UseCard()
    {
        CardEffects.Instance.PyramideEffect(mEnemylightedCells);
        CardEffects.Instance.PyramideEffect(mHighlightedCells);

        base.UseCard();

    }
    private void CheckPaths(int xDirection, int yDirection, int movement,  int currentX,  int currentY)
    {
        for (int i = 1; i <= movement; i++)
        {
            currentX += xDirection;
            currentY += yDirection;

            CellState cellState = CellState.None;
            cellState =mCurrentCell.mBoard.ValidateCellforCards(currentX, currentY,this);

            if (cellState == CellState.Enemy)
            {

                cell = mCurrentCell.mBoard.mAllCells[currentX, currentY];
                Enemies.Add(cell);
                mEnemylightedCells.Add(cell);
                continue;
            }

            if (cellState != CellState.Free)
                continue;

            mHighlightedCells.Add(mCurrentCell.mBoard.mAllCells[currentX, currentY]);
        }
    }
    public override void CheckPathing()
    {
        CreateCellPath(1, 0, mMovement.x);
        //CreateCellPath(-1, 0, mMovement.x);

    }
  

}
