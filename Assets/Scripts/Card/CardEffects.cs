using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CardEffects : MonoBehaviour
{
    public static CardEffects Instance { get; private set; }
    [Header("CanvasParent")]
    [SerializeField] private Transform canvasParent;
    [Header("Tornado")]
    [SerializeField] private List<GameObject> TornadoEffects;

    [Header("BombEffect")]
    [SerializeField] private GameObject BombEffectParticle;
    [SerializeField] private GameObject MeteorEffectParticle;
    [SerializeField] private GameObject PyramideEffectParticle;
    [SerializeField] private List<Sprite> PyramideSprites;



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

    public void BombEffect(List<Cell> bombPositions)
    {
        List<Cell> clonedList = new List<Cell>(bombPositions);
        StartCoroutine(BombTime(clonedList));
    }
    IEnumerator BombTime(List<Cell> bombPositions)
    {
        foreach (var cell in bombPositions)
        {
            GameObject bombEffect = Instantiate(BombEffectParticle, Vector3.zero, Quaternion.identity, cell.transform);
            bombEffect.transform.localPosition = Vector3.zero;
            CardManager.Instance.VibrateCustom(20);
            Destroy(bombEffect, .4f); yield return new WaitForSeconds(0.1f);

        }
    }

    #region MeteorEffect
    public void MeteorEffect(List<Cell> bombPositions)
    {
        List<Cell> clonedList = new List<Cell>(bombPositions);
        StartCoroutine(SpawnMeteorEffects(clonedList));
        StartCoroutine(FadeShakeImage());
    }
 
    private IEnumerator SpawnMeteorEffects(List<Cell> bombPositions)
    {
        foreach (Cell cell in bombPositions)
        {
            GameObject bombEffect = Instantiate(MeteorEffectParticle, Vector3.zero, Quaternion.identity, cell.transform);
            bombEffect.transform.localPosition = new Vector3(500, 500, 0);
          
            bombEffect.transform.DOLocalMove(Vector3.zero, 0.4f)
                .SetEase(Ease.OutCubic).OnComplete(() =>
                {
                    CardManager.Instance.VibrateCustom(20);
                    Destroy(bombEffect); // 1 saniyə sonra silinir
                });

            // 0.2 saniyə sonra bombanı yarat
            StartCoroutine(TriggerBombAfterDelay(cell.transform, 0.2f, bombEffect));

            yield return new WaitForSeconds(0.1f); // Növbəti cell üçün 0.5s gözlə
        }
    }

    private IEnumerator TriggerBombAfterDelay(Transform cell, float delay, GameObject meteor)
    {
        yield return new WaitForSeconds(delay);
        BombMeteroid(cell);
        //Destroy(meteor);
    }

    void BombMeteroid(Transform cell)
    {
        GameObject bombEffect = Instantiate(BombEffectParticle, Vector3.zero, Quaternion.identity, cell);
        bombEffect.transform.localPosition = Vector3.zero;
        Destroy(bombEffect, 0.4f);
    }
    #endregion

    #region Tornado
    public void TornadoEffect()
    {
        StartCoroutine(TornadoEffectTime());
    }
    IEnumerator TornadoEffectTime()
    {
        foreach (GameObject tornado in TornadoEffects)
        {
            tornado.SetActive(true);
        }
        CardManager.Instance.VibrateCustom(70);
        yield return new WaitForSeconds(.6f);

        EnemySpawner.Instance.EnemyBackMoveF();
        
        yield return new WaitForSeconds(.8f);

        foreach (GameObject tornado in TornadoEffects)
        {
            tornado.SetActive(false);
        }
    }
   
    #endregion

    #region Pyramide


    public float fadeInDuration = .5f;
    public float fadeOutDuration = .5f;
    public float shakeDuration = .3f;
    public float shakeStrength = 10f;

    public void PyramideEffect(List<Cell> transforms)
    {
        //int count = Mathf.Min(prefabs.Count, transforms.Count);

        foreach (var cell in transforms)
        {
            GameObject bombEffect = Instantiate(PyramideEffectParticle, cell.transform);
            Image img = bombEffect.GetComponent<Image>();
            img.sprite = PyramideSprites[Random.Range(0, PyramideSprites.Count)];
            bombEffect.transform.localPosition = new Vector3(0, 300f, 0);
            bombEffect.transform.parent = canvasParent; // CanvasParent-ə əlavə et
            bombEffect.transform.SetAsLastSibling();


           // bombEffect.transform.SetAsLastSibling(); // UI-də ən üstdə göstər
            Vector3 targetPos = bombEffect.transform.localPosition - new Vector3(0, 300f, 0);

            RectTransform rect = bombEffect.GetComponent<RectTransform>();
           // rect.localPosition = startPos;
            // Shake + fade zənciri
            Sequence seq = DOTween.Sequence();
            rect.localScale = Vector3.one;

            seq.Append(rect.DOLocalMove(targetPos, 0.2f).SetEase(Ease.InFlash))
               .AppendCallback(() => img.color = new Color(img.color.r, img.color.g, img.color.b, .8f)) // görünən et
               .Append(rect.DOShakePosition(shakeDuration, shakeStrength))
               .Append(img.DOFade(0f, fadeOutDuration))
               .OnComplete(() => {  Destroy(bombEffect);  });
        }
        StartCoroutine(FadeShakeImage());
    }

    IEnumerator FadeShakeImage()
    {
        yield return new WaitForSeconds(0.2f); // Gözləmə müddəti, lazım gələrsə dəyişdirin
        CameraShake.Instance.ShakeCardAttack();
        CardManager.Instance.VibrateCustom(80);
    }
    IEnumerator FadeShakeImage(Image img)
    {

        if (img == null)
        {
            Debug.LogWarning("Prefab-da Image komponenti yoxdur: " + img.name);
            yield break;
        }

        Color c = img.color;
        c.a = 0;
        img.color = c;

        //Tween fadeInTween = img.DOFade(1f, fadeInDuration);
        Tween shakeTween = img.rectTransform.DOShakePosition(shakeDuration, shakeStrength);

        //fadeInTween.Play();
        shakeTween.Play();

        //yield return fadeInTween.WaitForCompletion();
        yield return shakeTween.WaitForCompletion();

        Tween fadeOutTween = img.DOFade(0f, fadeOutDuration);
        fadeOutTween.Play();
        yield return fadeOutTween.WaitForCompletion();

       // Destroy(img.gameObject);
    }
    #endregion
}
