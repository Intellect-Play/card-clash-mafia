using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Queen : BasePiece
{
    public List<int> skilet = new List<int> {
    8, 8, 8,
    11, 11, 11, 11, 11, 11, 11,
    14, 14, 14, 14, 14,
    15, 15, 15, 15, 15, 15, 15, 15, 15,
    16, 16, 16, 16, 16, 16
};
    public int AddHealth;
    public override void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
    {
        // Base setup
        base.Setup(newTeamColor, newSpriteColor, newPieceManager);
        HealthEnemy = skilet[SaveManager.Instance.saveData.playerData.currentLevel - 1] + AddHealth;
        if (PowerText != null) PowerText.text = HealthEnemy.ToString();

        // Pawn Stuff
        mMovement = new Vector3Int(2, 2, 2);
        //GetComponent<Image>().sprite = Resources.Load<Sprite>("Enemy5");
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
