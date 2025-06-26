using UnityEngine;
using DG.Tweening;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;
    [SerializeField] public CameraSO cameraSO;
    [SerializeField] public RectTransform yourUIRectTransform;

    Vector3 originalPosition;
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
        originalPosition = transform.localPosition;
    }

    public void ShakeCardAttack() {
        //    {
        //        
        //        yourUIRectTransform.DOShakeAnchorPos(
        //    duration: 0.5f,
        //    strength: new Vector2(30f, 30f),
        //    vibrato: 10,
        //    randomness: 90
        //); 
        transform.DOShakePosition(cameraSO.duration, cameraSO.strength, cameraSO.vibrato, cameraSO.randomness).OnComplete(()=>transform.localPosition = originalPosition);
    }
}
