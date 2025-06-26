using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PieceManager : MonoBehaviour
{
    public static PieceManager Instance { get; private set; }
    [HideInInspector]
    public bool mIsKingAlive = true;

    public GameObject mPiecePrefab;
    public GameObject mKillParticle;

    public BasePiece mWhitePiece = null;
    public BasePiece mBlackPiece = null;
    public List<GameObject> mPiecePrefabs = null;

    public List<BasePiece> mAllBlackPieces = null;

    public List<BasePiece> mPromotedPieces = new List<BasePiece>();
    public Board board;

    GameObject newPieceObject;

    [SerializeField] private Color flashColor = Color.red;
    [SerializeField] private float flashDuration = 0.1f;
    //private Dictionary<string, Type> mPieceLibrary = new Dictionary<string, Type>()
    //{
    //    {"P",  typeof(Pawn)},
    //    {"R",  typeof(Rook)},
    //    {"KN", typeof(Knight)},
    //    {"B",  typeof(Bishop)},
    //    {"K",  typeof(King)},
    //    {"Q",  typeof(Queen)}
    //};
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Setup(Board _board)
    {
        board = _board;
        mWhitePiece = CreatePieces(Color.white, new Color32(80, 124, 159, 255),1, "_King");

        PlacePiece(1, 2, mWhitePiece);       
    }
    public void SetupNewEnemies(string enemyType, int posEnemy)
    {
        mBlackPiece = CreatePieces(Color.black, Color.white, posEnemy, enemyType);
        mAllBlackPieces.Add(mBlackPiece);

        PlacePieces(Board.cellY - 1,  mBlackPiece, posEnemy);        
    }

    public void EnemyMove(int moveDistance,bool down)
    {
        // SwitchSides(Color.black);
        if (down)
        {
            for (int i = 0; i < mAllBlackPieces.Count; i++)
            {
                //if (!mAllBlackPieces[i].HasMove())
                //    continue;
                if (mAllBlackPieces[i].gameObject.activeInHierarchy)
                    mAllBlackPieces[i].ComputerMove(moveDistance, down);
            }
        }
        else
        {
            for (int i = mAllBlackPieces.Count-1; i >-1; i--)
            {
                //if (!mAllBlackPieces[i].HasMove())
                //    continue;
                if (mAllBlackPieces[i].gameObject.activeInHierarchy)
                    mAllBlackPieces[i].ComputerMove(moveDistance, down);
            }
        }

            SwitchSides(Color.black);
    }

    private BasePiece CreatePieces(Color teamColor, Color32 spriteColor,int pieceCount,string enemyType)
    {
        string key = enemyType;
        // Type pieceType = mPieceLibrary[key];

        // Create
        BasePiece newPiece = CreatePiece(enemyType);


        // Setup
        newPiece.Setup(teamColor, spriteColor, this);

        return newPiece;
    }

    private BasePiece CreatePiece(string pieceType)
    {
        foreach (GameObject piece in mPiecePrefabs)
        {
            if (piece.name == pieceType)
            {
                newPieceObject = Instantiate(piece);

                continue;
            }
        }   
        // Create new object
        newPieceObject.transform.SetParent(transform);
        //newPieceObject.name=n.ToString() + pieceType;
        // Set scale and position
        newPieceObject.transform.localScale = new Vector3(1, 1, 1);
        newPieceObject.transform.localRotation = Quaternion.identity;

        // Store new piece
        BasePiece newPiece = newPieceObject.GetComponent<BasePiece>();

        return newPiece;
    }

    private void PlacePieces(int pawnRow, BasePiece piece, int posEnemy)
    {
        // Mövcud sütunların siyahısı (0, 1, 2, ..., cellX-1)
        List<int> availableColumns = new List<int>();

        for (int x = 0; x < Board.cellX; x++)
        {
            availableColumns.Add(x);
        }

        // Təsadüfi qarışdır
        ShuffleList(availableColumns);
        int randomX = availableColumns[0]; // təkrarsız x (sütun)
        PlacePiece(pawnRow, posEnemy-1, piece);
        // countPieces sayda düşməni yerləşdir
      
    }

    private void PlacePiece(int pawnRow, int x, BasePiece piece)
    {
        piece.Place(board.mAllCells[x, pawnRow]); 
    }

    private void ShuffleList(List<int> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
    private void SetInteractive(List<BasePiece> allPieces, bool value)
    {
        //foreach (BasePiece piece in allPieces)
        //    piece.enabled = value;
    }
    private void SetInteractiveWhite(BasePiece allPieces, bool value)
    {
        allPieces.enabled = value;
    }


    public void SwitchSides(Color color)
    {
        if (!mIsKingAlive)
        {
            ResetPieces();
            mIsKingAlive = true;

            color = Color.black;
        }

        bool isBlackTurn = color == Color.white ? true : false;

        //SetInteractiveWhite(mWhitePiece, !isBlackTurn);

        SetInteractive(mAllBlackPieces, isBlackTurn);
        foreach (BasePiece piece in mPromotedPieces)
        {
            bool isBlackPiece = piece.mColor != Color.white ? true : false;
            bool isPartOfTeam = isBlackPiece == true ? isBlackTurn : !isBlackTurn;

            //if(piece.mColor != Color.white)
            //   piece.enabled = isPartOfTeam;
        }


    }

    public void DamageTower()
    {
        foreach (Image img in board.towerCellsInHierarchyImages)
        {
            Color originalColor = img.color;

            // DOTween ilə rəng animasiyası: qırmızıya və geri, 2 dəfə
            img.DOColor(flashColor, flashDuration)
                .SetLoops(3, LoopType.Yoyo)
                .SetEase(Ease.InOutSine)
                .OnComplete(() => img.color = originalColor);
        }
    }
    public void KillEnemy(BasePiece piece)
    {
        //Instantiate(mKillParticle, new Vector3(piece.gameObject.transform.position.x, piece.gameObject.transform.position.y,-180),Quaternion.identity,transform);
        mAllBlackPieces.Remove(piece);
        if(mAllBlackPieces.Count == 0)
        {
            GameManager.Instance.endTurnClick.EndTurnButton(1);
        }
        Destroy(piece.gameObject);
        if(WaveManager.Instance.levelFinished && mAllBlackPieces.Count == 0)
        {
            GameManager.Instance.WinGame();
        }
    }
    public void ResetPieces()
    {
        foreach (BasePiece piece in mPromotedPieces)
        {
            piece.Kill();
            Destroy(piece.gameObject);
        }

        mPromotedPieces.Clear();

        mWhitePiece.ResetKill();

        foreach (BasePiece piece in mAllBlackPieces)
            piece.ResetKill();
    }

    public void PromotePiece(Pawn pawn, Cell cell, Color teamColor, Color spriteColor)
    {
        // Kill Pawn
        //pawn.Kill();

        //// Create
        //BasePiece promotedPiece = CreatePiece(typeof(Queen));
        //promotedPiece.Setup(teamColor, spriteColor, this);

        //// Place piece
        //promotedPiece.Place(cell);

        //// Add
        //mPromotedPieces.Add(promotedPiece);
    }
}
