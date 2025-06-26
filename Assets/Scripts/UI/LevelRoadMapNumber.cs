using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelRoadMapNumber : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Image levelImage;
    public int levelNumber;

    
    public void GetNumberforText(int num)
    {
        levelNumber=num;
        levelText.text = num.ToString();
    }

    public void ActiveLevel(bool active)
    {
        Color targetColor;
        if (active)
        {
            ColorUtility.TryParseHtmlString("#41CCF1", out targetColor);
        }
        else
        {
            ColorUtility.TryParseHtmlString("#DEDDD9", out targetColor);
        }
        levelImage.color = targetColor;
    }
    // DEDDD9   45F13E   41CCF1
}
