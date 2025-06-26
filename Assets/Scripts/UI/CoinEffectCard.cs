using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CoinEffectCard : MonoBehaviour
{
    public Image coinImage;            // Coin image (UI Image)
    public Transform parentTransform;  // Hədəf parent obyekt
    public float moveUpAmount = 100f;  // Yuxarı qalxma məsafəsi
    public float duration = 1.2f;      // Cəmi animasiya müddəti

    private void Start()
    {
       // coinImage.GetComponent<Image>();
        PlayCoinEffect();
    }

    public void PlayCoinEffect()
    {
        // Parent-ə əlavə et
       // transform.SetParent(parentTransform, false);

        // Başlanğıc alpha 0
        Color startColor = coinImage.color;
        startColor.a = 0f;
        coinImage.color = startColor;

        // Orijinal mövqe
        Vector3 startPos = transform.localPosition;
        Vector3 endPos = startPos + new Vector3(0, moveUpAmount, 0);

        Sequence seq = DOTween.Sequence();

        // Fade-in və yuxarı qalxma
        seq.Append(coinImage.DOFade(1f, duration * 0.3f)); // 0.3s fade-in
        seq.Join(transform.DOLocalMove(endPos, duration).SetEase(Ease.OutQuad)); // Yuxarı qalxış

        // Fade-out
        seq.Append(coinImage.DOFade(0f, duration * 0.2f)); // 0.2s fade-out

        // Yox olduqdan sonra obyektin silinməsi (opsional)
        seq.OnComplete(() => Destroy(gameObject));
    }
}
