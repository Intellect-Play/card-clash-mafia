using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pawn : BasePiece
{
    List<int> goblin2 = new List<int> {
    10, 10, 10,
    13, 13, 13, 13, 13, 13, 13,
    16, 16, 16, 16, 16,
    17, 17, 17, 17, 17, 17, 17, 17, 17,
    18, 18, 18, 18, 18, 18
};
    public override void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
    {
        // Base setup
        base.Setup(newTeamColor, newSpriteColor, newPieceManager);
        HealthEnemy = goblin2[SaveManager.Instance.saveData.playerData.currentLevel -1];
        // Pawn Stuff
        mMovement = new Vector3Int(2, 2, 2);
        //GetComponent<Image>().sprite = Resources.Load<Sprite>("Enemy3");
        if (PowerText != null) PowerText.text = HealthEnemy.ToString();

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
