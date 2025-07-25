using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Board mBoard;
    public PieceManager mPieceManager;
    public EnemySpawner mEnemySpawner;
    public CardPowerManager mCardPowerManager;
    public Health mHealth;
    public Coin mCoin;
    [SerializeField] public EndTurnClick endTurnClick;
    bool cardMove;
    bool isGameOver = false;
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
        isGameOver = false;
        cardMove = true;
        mEnemySpawner = GetComponent<EnemySpawner>();
        mHealth = GetComponent<Health>();
        mCoin = GetComponent<Coin>();
        mBoard.Create();
        mEnemySpawner.GetBP(mPieceManager, mBoard);
        mPieceManager.board = mBoard;
        
    }
    private void Start()
    {
        mPieceManager.Setup(mBoard);
        mCardPowerManager.mKing = mPieceManager.mWhitePiece;
    }
    public void EndTurn(bool move = true)
    {
        mPieceManager.SwitchSides(Color.white);
        if(!move) return;
        //mEnemySpawner.EnemySpawnF();
        mEnemySpawner.EnemyMoveF();
        WaveManager.Instance.SpawnNextWave();

    }

    public void ChangeHealth(int heath)
    {
        mHealth.TakeDamage(heath);
        GameUI.Instance.UpdateHealth(mHealth.HealthPlayer);
    }

    public void ChangeCoin(int coin)
    {
        mCoin.AddCoin(coin);
        GameUI.Instance.UpdateScore(mCoin.CoinPlayer);
    }

    public void WinGame()
    {


        if (isGameOver) return;
        if(SaveManager.Instance.saveData.playerData.currentLevel==1) PlayerPrefs.SetInt("Tutorial2", 1);

        isGameOver = true;
        SaveManager.Instance.saveData.playerData.coins += mCoin.CoinPlayerInPlay;
        SaveManager.Instance.saveData.playerData.currentLevel += 1;


        SaveManager.Instance.Save();
        GameUI.Instance.WinGame();
    }


    public void LoseGame()
    {
        if (isGameOver) return;
        isGameOver = true;
       // SaveManager.Instance.saveData.playerData.coins += mCoin.CoinPlayerInPlay;
       // SaveManager.Instance.Save();

        GameUI.Instance.LoseGame();
    }

    public void EndTurnButton(bool move = true)
    {
        if (!cardMove) return;
        StartCoroutine(WaitForEndTurn( move ));
    }
    IEnumerator WaitForEndTurn(bool move = true)
    {
        cardMove = false;
        CardManagerMove.Instance.FadeOutCards();
      //  yield return new WaitForSeconds(1);
        //CardManager.Instance.ExitTurnButton();
       // EndTurn(move);
        //CardManager.Instance.cardManagerMove.ReturnAllCards();
        yield return new WaitForSeconds(1);
       // CardManager.Instance.cardManagerMove.SpawnCards();
        //yield return new WaitForSeconds(1);
        cardMove = true;
        CardManagerMove.Instance.FadeInCards();
        if(TutorialManager.Instance.IsTutorialActive)
        {
            yield return new WaitForSeconds(.3f);
            TutorialManager.Instance.AddTutorial();

        }

    }
}
