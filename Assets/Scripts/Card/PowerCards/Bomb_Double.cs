using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bomb_Double : CardBase
{
    public override CardType _CardType => CardType.BombDouble;

    private List<Cell> BombAreas = new();

    public void Awake()
    {
        mCardPower = SaveManager.Instance.cardDataList.cards.Find(x => x.name == _CardType.ToString());

    }
    public override void CardSetup(BasePiece basePiece, CardPowerManager _cardPowerManager)
    {
        base.CardSetup(basePiece, _cardPowerManager);
        BombAreas = GetRandom2x2Cells(GameManager.Instance.mBoard.mAllCells);
    }
    public override void UseCard()
    {
        CardEffects.Instance.BombEffect(EnemylightedCells);
        CardEffects.Instance.BombEffect(HighlightedCells);

        base.UseCard();

    }
    public override void CheckPathing()
    {
        foreach (var cell in BombAreas)
        {
            if (mCurrentCell.mBoardPosition == cell.mBoardPosition)
                continue;
            var state = mCurrentCell.mBoard.ValidateCellforCards(cell.mBoardPosition.x, cell.mBoardPosition.y, this);

            if (state == CellState.Enemy)
            {
                EnemylightedCells.Add(cell);
                Enemies.Add(cell);
            }
            if ( state == CellState.Free)
                HighlightedCells.Add(cell);
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

    private List<Cell> GetRandom2x2Cells(Cell[,] allCells, int count = 3)
    {
        var result = new List<Cell>();
        var usedPositions = new HashSet<Vector2Int>();

        int maxX = Board.cellX - 1;
        int maxY = Board.cellY - 1;

        var rng = new System.Random();
        int attempts = 0;

        while (result.Count < count * 4 && attempts < 200)
        {
            int x = rng.Next(0, maxX);
            int y = rng.Next(0, maxY);

            var positions = new Vector2Int[]
            {
                new(x, y), new(x + 1, y),
                new(x, y + 1), new(x + 1, y + 1)
            };

            if (positions.Any(p => usedPositions.Contains(p)))
            {
                attempts++;
                continue;
            }

            foreach (var pos in positions)
            {
                usedPositions.Add(pos);
                result.Add(allCells[pos.x, pos.y]);
            }
        }

        return result;
    }
}
