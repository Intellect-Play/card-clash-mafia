using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IdleInputDetector : MonoBehaviour
{
    private bool TutorialActive = false;

    [Header("Idle Timer A")]
    public float MoveCardTutorialTime = 5f;
    

    [Header("Idle Timer B")]
    public float CardMoveTutorialTime = 10f;
    

    private float timer = 0f;
    private bool triggeredA = false;
    private bool triggeredB = false;
    void Start()
    {
        TutorialActive = TutorialManager.Instance.IsTutorialActive;
    }




    void Update()
    {
        if (TutorialActive) return;
        // Əgər toxunma və ya klik varsa → hər şeyi sıfırla
        if (Input.anyKeyDown || Input.touchCount > 0 || Input.GetMouseButtonDown(0))
        {
            TutorialManager.Instance.EndGameTutorial();
            timer = 0f;
            triggeredA = false;
            triggeredB = false;
            return;
        }

        // Əks halda zaman artır
        timer += Time.deltaTime;

        // Timer A tetiklenmemişsə və vaxtı keçibsə
        if (!triggeredA && timer >= MoveCardTutorialTime)
        {
            triggeredA = true;
            TutorialManager.Instance.MoveCardTutorialinGame();
        }

        // Timer B tetiklenmemişsə və vaxtı keçibsə
        if (!triggeredB && timer >= CardMoveTutorialTime)
        {
            triggeredB = true;
            TutorialManager.Instance.CardMoveTutorialinGame();
        }
    }
}
