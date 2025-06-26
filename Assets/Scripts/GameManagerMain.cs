
using UnityEngine;

public class GameManagerMain : MonoBehaviour
{
    public static GameManagerMain Instance { get; private set; }

    [SerializeField]private GameObject mMainStage;
    [SerializeField]private GameObject mGameUI;
    [SerializeField] private GameObject mGameShop;
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
        BattleStartInMainMenu(false);
    }

    public void BattleStartInMainMenu(bool start)
    {
        mMainStage.SetActive(!start);
        mGameShop.SetActive(false);
        mGameUI.SetActive(start);
    }
    public void OpenShop()
    {
        mGameShop.SetActive(true);
        mMainStage.SetActive(false);
    }
    public void CloseShop()
    {
        mGameShop.SetActive(false);
        mMainStage.SetActive(true);
    }
}

