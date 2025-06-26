using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
using System.Collections;
using TMPro;

public abstract class BasePiece : MonoBehaviour
{
    public EnemySO enemySO;
    public Color mColor = Color.clear;
    public bool mIsFirstMove = true;

    [SerializeField] public Cell mOriginalCell = null;
    [SerializeField] public Cell mCurrentCell = null;
    [SerializeField] public GameObject moveAnime;
    [SerializeField] public TextMeshProUGUI PowerText;


    public RectTransform mRectTransform = null;
    public PieceManager mPieceManager;

    public Cell mTargetCell = null;
    public Vector3Int mMovement = Vector3Int.one;
    public List<Cell> mHighlightedCells = new List<Cell>();
    public List<Cell> mEnemylightedCells = new List<Cell>();
    public Animator mAnimatorController = null;
    public bool down=true;

    public bool moveCard=false;
    public GameObject Heal;
    public int HealthEnemy=100;
    int deadCase = 0;
    [SerializeField] private Image targetImage;
    [SerializeField] private float duration = 0.5f;
    bool AttackPiece = false;
    Color softRed = new Color(1f, 0.5f, 0.5f);
    private void OnEnable()
    {
        if (targetImage == null)
            targetImage = GetComponent<Image>();
        AttackPiece = false;
        if (GetComponent<Animator>() != null)
            mAnimatorController = GetComponent<Animator>();
        //deadCase = 0;
    }
    public virtual void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
    {
        down = true;
        mPieceManager = newPieceManager;
        mColor = newTeamColor;
        mRectTransform = GetComponent<RectTransform>();
        if(newTeamColor == Color.black) GetComponent<Image>().sprite = enemySO._EnemyImage;

    }

    public virtual void Place(Cell newCell)
    {
        if(newCell.mCurrentPiece != null)
        {
            PieceManager.Instance.KillEnemy(this);
        }
        mCurrentCell = newCell;
        mOriginalCell = newCell;
        mCurrentCell.mCurrentPiece = this;
        transform.position = mCurrentCell.transform.position;
        gameObject.SetActive(true);
    }

    public void ResetKill()
    {
        Kill();
        mIsFirstMove = true;
        Place(mOriginalCell);
    }

    public virtual void Kill()
    {
        
        if (mAnimatorController != null&&!AttackPiece)
        {
            StartCoroutine(KillTime());
        }
        else
        {
            if (mCurrentCell != null)
                mCurrentCell.mCurrentPiece = null;
            GameManager.Instance.ChangeCoin(10);
            PieceManager.Instance.KillEnemy(this);
        }       
    }
    public virtual void TakeDamage(int damage)
    {
        if(mAnimatorController!=null)
           mAnimatorController.enabled = false;
        HealthEnemy -= damage;
        if(PowerText!=null) PowerText.text = HealthEnemy.ToString();
        if (HealthEnemy <= 0)
        {
            Kill();
        }else StartColorPulse();

    }
    public void StartColorPulse()
    {
      
        // R=1, G=0.5, B=0.5
        targetImage.color = softRed;

        targetImage.DOColor(Color.white, .5f)
           .SetEase(Ease.InOutSine)
           .OnComplete(() =>
           {
               targetImage.DOColor(softRed, .5f)
                   .SetEase(Ease.InOutSine)
                   .OnComplete(() =>
                   {
                       if (mAnimatorController != null)
                           mAnimatorController.enabled = true;
                       targetImage.color = Color.white;
                   });
           });
        
        
    }
    IEnumerator KillTime()
    {
        if (mAnimatorController != null)
            mAnimatorController.SetTrigger("Die");
        yield return new WaitForSeconds(.3f);
        if (mCurrentCell != null)
            mCurrentCell.mCurrentPiece = null;
        GameManager.Instance.ChangeCoin(10);
        PieceManager.Instance.KillEnemy(this);
    }
    public void HasMoveTarget(int moveDistance)
    {
        if (mHighlightedCells.Count <= 0) return;
        if (mHighlightedCells.Count < moveDistance) mTargetCell = mHighlightedCells[mHighlightedCells.Count-1];
        if (mHighlightedCells.Count >= moveDistance) mTargetCell = mHighlightedCells[moveDistance];
    }
    public virtual void AttackAnimation()
    {
        if (mAnimatorController != null)
            mAnimatorController.SetTrigger("Attack");
    }
    public void ComputerMove(int moveDistance, bool _down)
    {
        ClearCells();
        down = _down;
        CheckPathing();

        HasMoveTarget(moveDistance);
        Move();
        CheckEnemyKillCase();
       
    }

    private void CheckEnemyKillCase()
    {
        if (mColor == Color.black && mCurrentCell.mBoardPosition.y == 0)
        {
            if (mAnimatorController != null)
                mAnimatorController.SetTrigger("Attack");
            PieceManager.Instance.DamageTower();
            GameManager.Instance.ChangeHealth(1);
            //if (deadCase >= 1)
            //{
              
            //    //Kill();
            //}
            //else PieceManager.Instance.DamageTower();

            ++deadCase;          
        }else deadCase = 0;
    }

    #region Movement
    public virtual void CreateCellPath(int xDirection, int yDirection, int movement)
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
                mEnemylightedCells.Add(mCurrentCell.mBoard.mAllCells[currentX, currentY]);
                break;
            }
           
            if (cellState != CellState.Free)
            {
                break;
            }

            mHighlightedCells.Add(mCurrentCell.mBoard.mAllCells[currentX, currentY]);
        }
    }
    public virtual void CreateCellPathForEnemy(int xDirection, int yDirection, int movement)
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
                mHighlightedCells.Add(mCurrentCell.mBoard.mAllCells[currentX, currentY]);
                return;
            }

            if (cellState != CellState.Free)
            {
                return;
            }

            mHighlightedCells.Add(mCurrentCell.mBoard.mAllCells[currentX, currentY]);
        }
    }
    public virtual void CheckPathing()
    {
        CreateCellPath(1, 0, mMovement.x);
        CreateCellPath(-1, 0, mMovement.x);
        CreateCellPath(0, 1, mMovement.y);
        CreateCellPath(0, -1, mMovement.y);
        CreateCellPath(1, 1, mMovement.z);
        CreateCellPath(-1, 1, mMovement.z);
        CreateCellPath(-1, -1, mMovement.z);
        CreateCellPath(1, -1, mMovement.z);
    }

    public virtual void ShowCells()
    {
        foreach (Cell cell in mHighlightedCells)
            cell.mOutlineImage.enabled = true;
        foreach (Cell cell in mEnemylightedCells)
            cell.mOutlineEnemyImage.enabled = true;
    }

    public virtual void ClearCells()
    {
        foreach (Cell cell in mHighlightedCells)
            cell.mOutlineImage.enabled = false;
        foreach (Cell cellx in mEnemylightedCells)
            cellx.mOutlineEnemyImage.enabled = false;
        mEnemylightedCells.Clear();
        mHighlightedCells.Clear();
    }
    public IEnumerator  AttackTime()
    {
        if (mAnimatorController != null)
            mAnimatorController.SetTrigger("Attack");
       
        PieceManager.Instance.mWhitePiece.StartColorPulse();
        yield return new WaitForSeconds(.3f);
        //Kill();
    }
    public virtual void Move()
    {
        if (mTargetCell == null) return;
     

        mIsFirstMove = false;
        if(mTargetCell.mCurrentPiece != null&& mTargetCell.mCurrentPiece.mColor==Color.white)
        {
            EnemyPassKingCehck();
            return;
          //  GameManager.Instance.ChangeHealth(1);
            //if (mAnimatorController != null) {
            //    Debug.Log("Attack Piece");
            //    AttackPiece = true;
            //    StartCoroutine(AttackTime());
            //    return;
            //}
            //else { Kill(); Debug.Log("Kill Else"); return; }
            
        }
        else
        {
            //mTargetCell.RemovePiece();
            //mCurrentCell.mCurrentPiece = null;

            //mCurrentCell = mTargetCell;
            //mCurrentCell.mCurrentPiece = this;
        }


        EnemyMovePart();
    }

    public void EnemyPassKingCehck()
    {
        ClearCells();
        int currentX = mCurrentCell.mBoardPosition.x+1;
        int currentY = mCurrentCell.mBoardPosition.y-1;
        CellState cellState = mCurrentCell.mBoard.ValidateCell(currentX, currentY, this);
        if(cellState == CellState.Free)
        {
            Debug.Log("Enemy Piece Killed R");
            mHighlightedCells.Add(mCurrentCell.mBoard.mAllCells[currentX, currentY]);
            mTargetCell = mHighlightedCells[0];

            EnemyMovePart();
            return;
        }
        currentX = mCurrentCell.mBoardPosition.x - 1;
        currentY = mCurrentCell.mBoardPosition.y - 1;
        cellState = mCurrentCell.mBoard.ValidateCell(currentX, currentY, this);
        if (cellState == CellState.Free)
        {
            HasMoveTarget(1);

            mHighlightedCells.Add(mCurrentCell.mBoard.mAllCells[currentX, currentY]);
            mTargetCell = mHighlightedCells[0];

            EnemyMovePart();

           
            return;
        }
        return;
    }
    public void EnemyMovePart()
    {
        //mTargetCell.RemovePiece();

        mCurrentCell.mCurrentPiece = null;

        mCurrentCell = mTargetCell;
        mCurrentCell.mCurrentPiece = this;
        if (mAnimatorController != null && !AttackPiece) mAnimatorController.SetTrigger("Jump");
        // DOTween ile pozisyonu yumuşakça taşı
        transform.DOMove(mCurrentCell.transform.position, 0.7f)
                 .SetEase(Ease.OutCubic);

        mTargetCell = null;
    }
    #endregion
    public virtual void MoveKing(Cell targetCell)
    {
       
    }
    public virtual void MoveCardCick(CardClick cardClick)
    {

    }
}
