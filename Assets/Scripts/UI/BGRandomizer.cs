using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BGRandomizer : MonoBehaviour
{
    public Image bgImage;
    public List<Sprite> backgroundSprites;

    private int lastIndex = -1; // Son seçilen görselin index'i

    void Start()
    {
        SetRandomBackground();
    }

    public void SetRandomBackground()
    {
        if (backgroundSprites.Count == 0) return;

        int randomIndex;

        // Önceki index ile aynı olmamasını sağla
        do
        {
            randomIndex = Random.Range(0, backgroundSprites.Count);
        }
        while (randomIndex == lastIndex && backgroundSprites.Count > 1);

        // Yeni görseli uygula
        bgImage.sprite = backgroundSprites[randomIndex];

        // Son seçimi kaydet
        lastIndex = randomIndex;
    }
}
