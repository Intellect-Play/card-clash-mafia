using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DoubleFire : CardBase
{
    public override CardType _CardType => CardType.DoubleFire;

    public void Awake()
    {
        mCardPower = SaveManager.Instance.cardDataList.cards.Find(x => x.name == _CardType.ToString());

    }
    public override void CheckPathing()
    {
        CreateCellPathD(0, 1, mMovement.y);
    }

    private void CreateCellPathD(int xDir, int yDir, int movement)
    {
        int curX = mCurrentCell.mBoardPosition.x;
        int curY = mCurrentCell.mBoardPosition.y;

        CheckPaths(xDir, yDir, movement, curX + 1, curY);
        CheckPaths(xDir, yDir, movement, curX - 1, curY);
        CheckPaths(xDir, -yDir, movement, curX + 1, curY+1);
        CheckPaths(xDir, -yDir, movement, curX - 1, curY+1);
    }
    public override void UseCard()
    {
        CardEffects.Instance.BombEffect(EnemylightedCells);
        CardEffects.Instance.BombEffect(HighlightedCells);

        base.UseCard();

    }
    private void CheckPaths(int xDir, int yDir, int movement, int x, int y)
    {
        for (int i = 1; i <= movement; i++)
        {
            x += xDir;
            y += yDir;

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
