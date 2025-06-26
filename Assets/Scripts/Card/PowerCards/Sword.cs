using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Sword : CardBase
{
    public override CardType _CardType => mCardSO._CardType;

 
    public override void CheckPathing()
    {
        CreateCellPathS(0, 1, mMovement.y);
    }
    public void Awake()
    {
        mCardPower = SaveManager.Instance.cardDataList.cards.Find(x => x.name == _CardType.ToString());

    }
    private void CreateCellPathS(int x, int y, int movement)
    {
        int curX = mCurrentCell.mBoardPosition.x;
        int curY = mCurrentCell.mBoardPosition.y;

        CheckPaths(x, y, movement, curX, curY);
        CheckPaths(x, y, 1, curX + 1, curY + 1);
        CheckPaths(x, y, 1, curX - 1, curY + 1);
        CheckPaths(x, -y, movement, curX, curY);
        CheckPaths(x, y, 1, curX + 1, curY - 3);
        CheckPaths(x, y, 1, curX - 1, curY - 3);
    }
    public override void UseCard()
    {
        CardEffects.Instance.BombEffect(EnemylightedCells);
        CardEffects.Instance.BombEffect(HighlightedCells);

        base.UseCard();

    }
    private void CheckPaths(int xDir, int yDir, int move, int x, int y)
    {
        for (int i = 1; i <= move; i++)
        {
            x += xDir;
            y += yDir;
            if (mCurrentCell.mBoardPosition == new Vector2Int(x, y))
                continue;
            var cellState = mCurrentCell.mBoard.ValidateCellforCards(x, y, this);

            if (cellState == CellState.Enemy)
            {
                var cell = mCurrentCell.mBoard.mAllCells[x, y];
                Enemies.Add(cell);
                EnemylightedCells.Add(cell);
                continue;
            }

            if (cellState != CellState.Free) continue;

            HighlightedCells.Add(mCurrentCell.mBoard.mAllCells[x, y]);
        }
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
}
