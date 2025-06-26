using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int CoinPlayer;
    public int CoinPlayerInPlay;

    private void Start()
    {
        CoinPlayerInPlay = 0;
        CoinPlayer = SaveManager.Instance.saveData.playerData.coins;
    }
    public void AddCoin(int damage)
    {
        CoinPlayerInPlay +=damage;
        CoinPlayer += damage;
    }
}
