using System.Collections.Generic;
using UnityEngine;

public class Bat : CardBase
{
    public override CardType _CardType => CardType.Bat;
    int currentX;
    int currentY;
    public void Awake()
    {
        mCardPower = SaveManager.Instance.cardDataList.cards.Find(x => x.name == _CardType.ToString());

    }
    public override void CardSetup(BasePiece basePiece, CardPowerManager _cardPowerManager)
    {
        base.CardSetup(basePiece, _cardPowerManager);
        mMovement = mCardSO._CardPoweraArea; // z = distance for diagonal   0.0.15
    }

    public override void CheckPathing()
    {
        currentX = mCurrentCell.mBoardPosition.x;
        currentY = mCurrentCell.mBoardPosition.y;
        CreateCellPath(1, 1, mMovement.z,true,currentX,currentY,true);   // Diagonal right
        CreateCellPath(-1, 1, mMovement.z,false, currentX, currentY,true);  // Diagonal left
        CreateCellPath(1, -1, mMovement.z, true, currentX, currentY, true);   // Diagonal right
        CreateCellPath(-1, -1, mMovement.z, false, currentX, currentY, true);  // Diagonal left
    }

    public override void ShowCells()
    {
        foreach (var cell in HighlightedCells)
            cell.mOutlineImage.enabled = true;
        foreach (var cell in EnemylightedCells)
            cell.mOutlineEnemyImage.enabled = true;
    }

    public override void ClearCells()
    {
        foreach (var cell in HighlightedCells)
            cell.mOutlineImage.enabled = false;
        foreach (var cell in EnemylightedCells)
            cell.mOutlineEnemyImage.enabled = false;
        HighlightedCells.Clear();
        EnemylightedCells.Clear();

    }
    public override void UseCard()
    {
        CardEffects.Instance.BombEffect(EnemylightedCells);
        CardEffects.Instance.BombEffect(HighlightedCells);

        base.UseCard();

    }
    private void CreateCellPath(int xDirection, int yDirection, int movement,bool right, int _currentX,int _currentY,bool turn)
    {
        

        for (int i = 1; i <= movement; i++)
        {
            _currentX += xDirection;
            _currentY += yDirection;
            if(mCurrentCell.mBoardPosition == new Vector2Int(_currentX, _currentY))
                continue;
            var cellState = mCurrentCell.mBoard.ValidateCellforCards(_currentX, _currentY, this);

            if (cellState == CellState.Enemy)
            {
                var cell = mCurrentCell.mBoard.mAllCells[_currentX, _currentY];
                Enemies.Add(cell);
                EnemylightedCells.Add(cell);
                continue;
            }

            if (cellState != CellState.Free)
            {
                if (turn)
                {
                    if (right)
                        CreateCellPath(-1, 1, mMovement.z, false, _currentX, _currentY-2, false);
                    else
                        CreateCellPath(1, 1, mMovement.z, true, _currentX, _currentY-2, false);
                }
             

                break;

            }

            HighlightedCells.Add(mCurrentCell.mBoard.mAllCells[_currentX, _currentY]);
        }
    }
}
