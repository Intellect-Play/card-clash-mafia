using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TutorialHandAnimator : MonoBehaviour
{
    public RectTransform handImage;
    public float moveDistance = 100f;
    public float duration = 1f;

    public Canvas canvas;
    private Tweener currentTween;
    public bool TweenPlay;

    [Header("Optional Directional Arrows")]
    public RectTransform fourArrow;


    void Awake()
    {
        TweenPlay = false;
        canvas = handImage.GetComponentInParent<Canvas>();
        handImage.gameObject.SetActive(false); // başlanğıcda gizli olsun
    }

    public void ShowMoveHandAnimationUI(RectTransform uiTarget, Vector3 offset)
    {
        if (uiTarget == null)
        {
            HideHand();
            return;
        }

        TweenPlay = true;

        // Dünyadakı mövqe
        Vector3 worldPos = uiTarget.position + offset;

        // Ekran mövqeyi
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main,
            worldPos
        );

        // Lokal mövqe
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            handImage.parent as RectTransform,
            screenPos,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main,
            out localPoint
        );

        // Əsas mövqeyi təyin et
        handImage.anchoredPosition = localPoint;
        handImage.gameObject.SetActive(true);
        gameObject.SetActive(true);

        currentTween?.Kill();
        handImage.localScale = Vector3.one;

        // ✳️ Animasiya: sağ-yuxarıya sürüş → geri
        Vector2 targetPos = localPoint + new Vector2(60f, 700f); // sağ-yuxarı offset

        currentTween = handImage
            .DOAnchorPos(targetPos, duration * 0.8f)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void ShowTapAnimationWorldUI(RectTransform uiTarget, Vector3 offset)
    {
        if (uiTarget == null)
        {
            HideHand();
            return;
        }

        TweenPlay = true;

        bool isWorldCanvas = canvas.renderMode == RenderMode.WorldSpace;
        bool isSameCanvas = handImage.GetComponentInParent<Canvas>() == uiTarget.GetComponentInParent<Canvas>();

        if (isWorldCanvas && isSameCanvas)
        {
            // Hər ikisi World Space canvas-dadırsa, sadəcə pozisiyanı kopyala
            handImage.position = uiTarget.position + offset;
        }
        else
        {
            // Ekran mövqeyi ilə konvertasiya
            Vector3 screenPos = Camera.main.WorldToScreenPoint(uiTarget.position + offset);

            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                handImage.parent as RectTransform,
                screenPos,
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main,
                out localPoint
            );

            handImage.localPosition = localPoint;
        }

        handImage.gameObject.SetActive(true);
        gameObject.SetActive(true);

        currentTween?.Kill();
        handImage.localScale = Vector3.one;

        currentTween = handImage
            .DOScale(1.3f, duration * 0.5f)
            .SetEase(Ease.OutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }
    public void ShowTapAnimationUI(RectTransform uiTarget, Vector3 offset)
    {
        if (uiTarget == null)
        {
            HideHand();
            return;
        }

        TweenPlay = true;

        // 1. Dünyadakı (world space) mövqeni tap
        Vector3 worldPos = uiTarget.position + offset;

        // 2. Ekran koordinatını al
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main,
            worldPos
        );

        // 3. UI parent içində lokal mövqe tap
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            handImage.parent as RectTransform,
            screenPos,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main,
            out localPoint
        );

        // 4. handImage yerləşdir
        handImage.localPosition = localPoint;
        handImage.gameObject.SetActive(true);
        gameObject.SetActive(true);

        // 5. Animasiya təmizlənir və yenidən başlayır
        currentTween?.Kill();
        handImage.localScale = Vector3.one;

        currentTween = handImage
            .DOScale(1.3f, duration * 0.5f)
            .SetEase(Ease.OutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void ShowTapAnimationUIEndTurn(RectTransform uiTarget, Vector2 offset)
    {
        if (uiTarget == null)
        {
            HideHand();
            return;
        }

        TweenPlay = true;

        // 1. Dünyadakı (world space) mövqeni tap
        Vector3 worldPos = uiTarget.anchoredPosition;

        // 2. Ekran koordinatını al
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main,
            worldPos
        );

        // 3. UI parent içində lokal mövqe tap
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            handImage.parent as RectTransform,
            screenPos,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main,
            out localPoint
        );

        // 4. handImage yerləşdir
        handImage.localPosition = localPoint;
        handImage.gameObject.SetActive(true);
        gameObject.SetActive(true);

        // 5. Animasiya təmizlənir və yenidən başlayır
        currentTween?.Kill();
        handImage.localScale = Vector3.one;

        currentTween = handImage
            .DOScale(1.3f, duration * 0.5f)
            .SetEase(Ease.OutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void HideHandTouch()
    {
        TweenPlay = false;
        currentTween?.Kill();
        currentTween = null;
        handImage.gameObject.SetActive(false);
        gameObject.SetActive(false);
        
    }

    public void HideHand()
    {
        if (!TweenPlay)
        {
            currentTween?.Kill();
            currentTween = null;
            handImage.gameObject.SetActive(false);
            gameObject.SetActive(false);
           
        }
    }
  
  

  
}
