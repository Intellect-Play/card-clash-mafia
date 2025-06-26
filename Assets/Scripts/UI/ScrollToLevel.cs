using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollToLevel : MonoBehaviour
{
    [Header("References")]
    public ScrollRect scrollRect;
    public RectTransform content;
    public List<RectTransform> LevelImages;
    public List<LevelRoadMapNumber> levelRoadMapNumbers;

    public int targetLevel = 5; 
    int levelCount;
    void Start()
    {
        GetAndListNumbers();
        ScrollToTargetLevel(levelCount);
    }

    void GetAndListNumbers()
    {
        levelRoadMapNumbers = new List<LevelRoadMapNumber>();
        levelCount = SaveManager.Instance.saveData.playerData.currentLevel;
        for (int i = 0; i < LevelImages.Count; i++)
        {
            var levelImage = LevelImages[i];
            var levelNumber = levelImage.GetComponent<LevelRoadMapNumber>();
            if (levelNumber != null)
            {
                levelRoadMapNumbers.Add(levelNumber);
                levelNumber.GetNumberforText(i+1);
                if(i < levelCount)
                {
                    levelNumber.ActiveLevel(true);
                }
                else
                {
                    levelNumber.ActiveLevel(false);
                }
            }
        }
    }
    public void ScrollToTargetLevel(int level)
    {
        level -=1;

        if (level < 0) level = 0;
        var target = LevelImages[LevelImages.Count - level-1];
        if (target == null)
        {
            Debug.LogWarning("Level not found: " + level);
            return;
        }
    
        Canvas.ForceUpdateCanvases();


        // Dünyadakı mövqeyi al və onu Viewport koordinatına çevir
        Vector3 worldPos = target.position;
        Vector3 viewportLocalPos = scrollRect.viewport.InverseTransformPoint(worldPos);
        float viewportHeight = scrollRect.viewport.rect.height;

        // Nə qədər yuxarıda yerləşir
        float offset = viewportLocalPos.y;
        float contentHeight = content.rect.height;
        float normalizedPos = Mathf.Clamp01(1f - (offset / (contentHeight - viewportHeight)));

        scrollRect.verticalNormalizedPosition = normalizedPos;
    }
}
