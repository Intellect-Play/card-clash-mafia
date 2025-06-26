using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bishop : BasePiece
{
    List<int> BigGoblin = new List<int> {
    15, 15, 15,
    18, 18, 18, 18, 18, 18, 18,
    21, 21, 21, 21, 21,
    22, 22, 22, 22, 22, 22, 22, 22, 22,
    23, 23, 23, 23, 23, 23
};
    public override void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
    {
        // Base setup
        base.Setup(newTeamColor, newSpriteColor, newPieceManager);
        HealthEnemy = BigGoblin[SaveManager.Instance.saveData.playerData.currentLevel - 1];
        if (PowerText != null) PowerText.text = HealthEnemy.ToString();

        // Pawn Stuff
        mMovement = new Vector3Int(2, 2, 2);
        //GetComponent<Image>().sprite = Resources.Load<Sprite>("Enemy1");
        GetComponent<Image>().sprite = enemySO._EnemyImage;

    }


    public override void CheckPathing()
    {
        // Horizontal
        if (down)
        {
            CreateCellPathForEnemy(0, -1, mMovement.y);

        }
        else
            CreateCellPathForEnemy(0, 1, mMovement.y);
    }
}
