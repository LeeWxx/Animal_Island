using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GoldCoin : MonoBehaviour
{
    Vector3 CoinRotate = new Vector3(90f, 360f, 0f);

    Vector3 upPos;
    Vector3 downPos;

    public float duration;

    private Color originalColor;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        originalColor = meshRenderer.material.color;
        originalColor.a = 1f;
    }


    private void OnEnable()
    {
        transform.localPosition= Vector3.zero;
        meshRenderer.material.color = originalColor;

        upPos = transform.position + new Vector3(0, 3, 0);
        downPos = upPos - new Vector3(0, 0.5f, 0);
        StartCoroutine(GoldCoinMove());
        StartCoroutine(GoldCoinFadeOut());
    }

    private void Start()
    {
        duration = TouchCoinPool.Instance.touchCoinDuration;
        transform.DORotate(CoinRotate, duration, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
    }

    IEnumerator GoldCoinMove()
    {
        float upDuration = duration * 0.6f;
        transform.DOMove(upPos, upDuration);
        yield return new WaitForSeconds(upDuration);
        transform.DOMove(downPos, duration * 0.4f);
    }

    IEnumerator GoldCoinFadeOut()
    {
        Color fadeColor = originalColor;
        float c = meshRenderer.material.color.a;

        while (meshRenderer.material.color.a > 0.8f)
        {
            yield return new WaitForSeconds(0.01f);
            c = Mathf.Lerp(c, 0, Time.deltaTime * 0.15f);
            fadeColor.a = c;
            meshRenderer.material.color = fadeColor;
        }
    }
}
