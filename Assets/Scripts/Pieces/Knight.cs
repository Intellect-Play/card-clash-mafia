using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Knight : BasePiece
{
    List<int> mag = new List<int> {
    12, 12, 12,
    15, 15, 15, 15, 15, 15, 15,
    18, 18, 18, 18, 18,
    19, 19, 19, 19, 19, 19, 19, 19, 19,
    20, 20, 20, 20, 20, 20
};
    public override void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
    {
        // Base setup
        base.Setup(newTeamColor, newSpriteColor, newPieceManager);
        HealthEnemy = mag[SaveManager.Instance.saveData.playerData.currentLevel - 1];
        if (PowerText != null) PowerText.text = HealthEnemy.ToString();

        // Pawn Stuff
        mMovement = new Vector3Int(2, 2, 2);
        //GetComponent<Image>().sprite = Resources.Load<Sprite>("Enemy2");
    }


    public override void CheckPathing()
    {
        // Horizontal
        if (down)
            CreateCellPathForEnemy(0, -1, mMovement.y);
        else
            CreateCellPathForEnemy(0, 1, mMovement.y);
    }
}
