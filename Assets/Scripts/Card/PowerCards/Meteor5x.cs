using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Meteor5x : CardBase
{
    public override CardType _CardType => CardType.Meteor5x;
    List<Cell> randomCells;
    public void Awake()
    {
        mCardPower = SaveManager.Instance.cardDataList.cards.Find(x => x.name == _CardType.ToString());

    }
    public override void CardSetup(BasePiece basePiece, CardPowerManager _cardPowerManager)
    {
        base.CardSetup(basePiece, _cardPowerManager);
        randomCells = GetRandomCells(GameManager.Instance.mBoard.mAllCells, 5);

    }
    public override void UseCard()
    {
        CardEffects.Instance.MeteorEffect(EnemylightedCells);
        CardEffects.Instance.MeteorEffect(HighlightedCells);

        base.UseCard();

    }

    public override void CheckPathing()
    {

        foreach (var cell in randomCells)
        {
            if (mCurrentCell.mBoardPosition == cell.mBoardPosition)
                continue;
            var state = mCurrentCell.mBoard.ValidateCellforCards(cell.mBoardPosition.x, cell.mBoardPosition.y, this);

            if (state == CellState.Enemy)
                Enemies.Add(cell);

            if (state == CellState.Enemy)
                EnemylightedCells.Add(cell);
            if (state == CellState.Free)
                HighlightedCells.Add(cell);
        }
    }

    private List<Cell> GetRandomCells(Cell[,] allCells, int count)
    {
        var flatList = new List<Cell>();
        int width = allCells.GetLength(0);
        int height = allCells.GetLength(1);

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                flatList.Add(allCells[x, y]);

        System.Random rng = new System.Random();
        return flatList.OrderBy(_ => rng.Next()).Take(count).ToList();
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
