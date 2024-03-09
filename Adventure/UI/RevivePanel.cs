using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RevivePanel : MonoBehaviour
{
    public TextMeshProUGUI countText;
    private int count;

    private void OnEnable()
    {
        count = 5;
        countText.text = "" + count;

        StartCoroutine(CountDownCoroutine());
    }

    private IEnumerator CountDownCoroutine()
    {
        while(count >= 1)
        {
            yield return new WaitForSeconds(1f);
            count -= 1;
            countText.text = "" + count;
        }

        AdventureManager.Instance.GameOver();
        this.gameObject.SetActive(false);
    }
}
