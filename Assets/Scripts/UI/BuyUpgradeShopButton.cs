using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyUpgradeShopButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private TextMeshProUGUI buttonCostText;

    public Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void SetButtonText(bool active, int cost, CardAction cardAction)
    {
        button.interactable = active;
        buttonText.text = cardAction.ToString();
        buttonCostText.text = cost.ToString();      
    }
}
