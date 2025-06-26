using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ScaleLoop : MonoBehaviour
{
    [SerializeField] private Image targetImage; // UI Image (drag in Inspector)
    [SerializeField] private float scaleFactor = 1.2f; // Nə qədər böyüsün
    [SerializeField] private float duration = 0.5f;     // Bir istiqamətdə vaxt

    private void OnEnable()
    {
        if (targetImage == null)
            targetImage = GetComponent<Image>();

        // Başlanğıc ölçü
        Vector3 originalScale = targetImage.rectTransform.localScale;

        // DOTween ilə loop edən scale animasiyası (pingpong)
        targetImage.rectTransform.DOScale(originalScale * scaleFactor, duration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }
    private void OnDisable()
    {
        transform.DOKill(); // DOTween animasiyasını dayandır
    }
}
