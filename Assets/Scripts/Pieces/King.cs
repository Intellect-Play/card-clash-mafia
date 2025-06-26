using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class King : BasePiece
{
    CardClick cardClick;
    public bool isDragging = false;
    public override void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
    {
        // Base setup
        base.Setup(newTeamColor, newSpriteColor, newPieceManager);
        moveAnime.SetActive(false);
        // King setup
        mMovement = new Vector3Int(2, 2, 1);
        GetComponent<Image>().sprite = Resources.Load<Sprite>("Player");
        Heal.SetActive(false);
    }

    public override void Kill()
    {
        base.Kill();
        //mPieceManager.mIsKingAlive = false;
    }
    public override void CreateCellPath(int xDirection, int yDirection, int movement)
    {
        int currentX = mCurrentCell.mBoardPosition.x;
        int currentY = mCurrentCell.mBoardPosition.y;

        for (int i = 1; i <= movement; i++)
        {
            currentX += xDirection;
            currentY += yDirection;

            CellState cellState = mCurrentCell.mBoard.ValidateCell(currentX, currentY, this);
            if (cellState == CellState.Enemy)
            {
                break;
            }

            if (cellState != CellState.Free)
            {
                break;
            }

            mHighlightedCells.Add(mCurrentCell.mBoard.mAllCells[currentX, currentY]);
        }
    }

    public override void ShowCells()
    {
        foreach (Cell cell in mHighlightedCells)
            cell.MoveActive(true);
      
    }

    public override void ClearCells()
    {
        foreach (Cell cell in mHighlightedCells)
            cell.MoveActive(false);     
        mHighlightedCells.Clear();
    }

    #region Cross-Platform Input
    void Update()
    {
        if (!moveCard) return;
        
        Vector2 pointerPosition = Vector2.zero;
        bool begin = false, move = false, end = false;

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        if (Input.GetMouseButtonDown(0))
        {
            pointerPosition = Input.mousePosition;
            begin = true;
        }
        else if (Input.GetMouseButton(0))
        {
            pointerPosition = Input.mousePosition;
            move = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            pointerPosition = Input.mousePosition;
            end = true;
        }
#else
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            pointerPosition = touch.position;
            begin = touch.phase == TouchPhase.Began;
            move = touch.phase == TouchPhase.Moved;
            end = touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled;
        }
#endif

        Camera uiCamera = Camera.main;

        if (begin)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(mRectTransform, pointerPosition, uiCamera))
            {
                CheckPathing();
                ShowCells();
                isDragging = true;
            }
        }

        if (move && isDragging)
        {
            Vector3 worldPos;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(mRectTransform, pointerPosition, uiCamera, out worldPos))
            {
                mRectTransform.position = worldPos;
            }

            mTargetCell = null;
            foreach (Cell cell in mHighlightedCells)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(cell.mRectTransform, pointerPosition, uiCamera))
                {
                    mTargetCell = cell;
                    break;
                }
            }
        }

        if (end && isDragging)
        {
            if(TutorialManager.Instance.IsTutorialActive)
            {
                if(mTargetCell!=TutorialManager.Instance.cell)
                {
                    mTargetCell = null;
                }else TutorialManager.Instance.HideTutorialHand();
            }
            // ClearCells();
            isDragging = false;

            if (mTargetCell != null)
            {
                MoveKing(mTargetCell);

            }
            else
            {
                // Return to original position
                mRectTransform.position = mCurrentCell.transform.position;
            }
        }
    }

    public override void AttackAnimation()
    {
        if (mAnimatorController != null)
            mAnimatorController.SetTrigger("Attack");
    }
    public override void MoveKing(Cell targetCell)
    {
        isDragging = false;
        mTargetCell = targetCell;
        ClearCells();
        Move();
        if(cardClick != null)
        {
            cardClick.card.UseForMoveCards();
        }
     
        GameManager.Instance.EndTurnButton();

        CardManagerMove.MoveCard = false;
        CardManagerMove.Instance.currentCardClick = null;
        moveCard = false;
    }

    public override void MoveCardCick(CardClick _cardClick)
    {
        cardClick = _cardClick;
    }
    #endregion
}
