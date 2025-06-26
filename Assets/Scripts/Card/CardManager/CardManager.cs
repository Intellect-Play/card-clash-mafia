using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{

    public static CardManager Instance;
    public CardManagerMove cardManagerMove;
    [SerializeField]private CardPowerManager powerManager;
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
    }
    private void Start()
    {
        powerManager = GetComponent<CardPowerManager>();
    }

    public void ExitTurnButton()
    {
        cardManagerMove.ReturnAllCards();
    }

    public void VibrateCustom(long milliseconds = 100)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
    using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
    {
        var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        var vibrator = activity.Call<AndroidJavaObject>("getSystemService", "vibrator");

        if (vibrator != null)
        {
            using (var version = new AndroidJavaClass("android.os.Build$VERSION"))
            {
                int sdkInt = version.GetStatic<int>("SDK_INT");

                if (sdkInt >= 26)
                {
                    var vibrationEffectClass = new AndroidJavaClass("android.os.VibrationEffect");
                    var effect = vibrationEffectClass.CallStatic<AndroidJavaObject>("createOneShot", milliseconds, 50);
                    vibrator.Call("vibrate", effect);
                }
                else
                {
                    vibrator.Call("vibrate", milliseconds);
                }
            }
        }
    }
#endif
    }
}
