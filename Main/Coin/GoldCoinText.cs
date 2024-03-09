using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldCoinText : MonoBehaviour
{
    public Color originalTextColor;
    public TextMesh textMesh;

    private void Awake()
    {
        textMesh = GetComponent<TextMesh>();
        originalTextColor = textMesh.color;
        originalTextColor.a = 1f;
    }

    private void OnEnable()
    {
        textMesh.color = originalTextColor;
        StartCoroutine(CoinTextFadeOut());
    }

    IEnumerator CoinTextFadeOut()
    {
        Color fadeColor = originalTextColor;
        float c = originalTextColor.a;

        while (textMesh.color.a > 0.6f)
        {
            yield return new WaitForSeconds(0.05f);
            c = Mathf.Lerp(c, 0, Time.deltaTime);
            fadeColor.a = c;
            textMesh.color = fadeColor;
        }
    }
    public void CoinTextChange(int plusGold)
    {
        textMesh.text = "+ " + plusGold;
    }
}
