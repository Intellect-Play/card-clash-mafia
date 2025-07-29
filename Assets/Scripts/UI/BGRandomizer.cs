using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BGRandomizer : MonoBehaviour
{
    public Image bgImage;                 
    public List<Sprite> backgroundSprites; 

    void Start()
    {
        SetRandomBackground();
    }

    public void SetRandomBackground()
    {
        if (backgroundSprites.Count == 0) return;

        int randomIndex = Random.Range(0, backgroundSprites.Count);
        bgImage.sprite = backgroundSprites[randomIndex];
    }
}
