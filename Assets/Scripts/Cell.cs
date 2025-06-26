using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public Image mOutlineImage;
    public Image mOutlineEnemyImage;
    public Image mOutlineMoveImage;
    //[HideInInspector]
    public Vector2Int mBoardPosition = Vector2Int.zero;
    //[HideInInspector]
    public Board mBoard = null;
    //[HideInInspector]
    public RectTransform mRectTransform = null;

    //[HideInInspector]
    public BasePiece mCurrentPiece = null;

    [SerializeField] private Color mMoveColor;
    Button cellButton;

    private void OnEnable()
    {
        cellButton = GetComponent<Button>();
        //cellButton.onClick.AddListener(CellButtonClick);
        MoveActive(false);
    }

    public void CellButtonClick()
    {
        if (TutorialManager.Instance.IsTutorialActive) {
            if (mBoardPosition == new Vector2Int(2, 3)|| mBoardPosition == new Vector2Int(2, 4))
            {
                Debug.Log("Cell Button Clicked");

                TutorialManager.Instance.HideTutorialHand();
                PieceManager.Instance.mWhitePiece.MoveKing(this);
            }
        }else PieceManager.Instance.mWhitePiece.MoveKing(this);
    }
    public void Setup(Vector2Int newBoardPosition, Board newBoard)
    {
        mBoardPosition = newBoardPosition;
        mBoard = newBoard;

        mRectTransform = GetComponent<RectTransform>();
    }

    public void MoveActive(bool active)
    {
        //mOutlineImage.color = active ? mMoveColor : Color.black;
        mOutlineMoveImage.enabled = active;
        cellButton.interactable = active;
    }

    public void RemovePiece(int damage)
    {
        if (mCurrentPiece != null)
        {
            mCurrentPiece.TakeDamage(damage);
        }
    }
}
