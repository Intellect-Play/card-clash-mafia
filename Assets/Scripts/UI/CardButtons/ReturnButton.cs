using System.Collections;
using UnityEngine;

public class ReturnButton : MonoBehaviour
{
    public CardManagerMove cardManager;

    public void OnClickSpawn()
    {

        //cardManager.SpawnCards();
        //cardManager.SpawnCards();
    }
    IEnumerator WaitForEndTurn()
    {
        CardManager.Instance.ExitTurnButton();
        cardManager.ReturnAllCards();
        yield return new WaitForSeconds(1);
        cardManager.SpawnCards();
    }
    public void OnClickReturn()
    {
        //cardManager.ReturnAllCards();
    }
}
