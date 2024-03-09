using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerminationWarningPanel : MonoBehaviour
{
    public Image terminationWarningPanel;
    Color PanelOriginalColor;
    public Text terminationWarningText;
    Color textOriginalColor;

    private void Awake()
    {
        PanelOriginalColor = terminationWarningPanel.color;
        textOriginalColor = terminationWarningText.color;

        StartCoroutine(TerminationWarning());
    }

    public IEnumerator TerminationWarning()
    {
        terminationWarningPanel.color = PanelOriginalColor;
        terminationWarningText.color = textOriginalColor;

        Color imageFadeColor = PanelOriginalColor;
        Color textFadeColor = textOriginalColor;

        float c = PanelOriginalColor.a;

        yield return new WaitForSeconds(0.5f);

        while (terminationWarningPanel.color.a > 0.05f)
        {
            yield return null;
            c = Mathf.Lerp(c, 0, Time.deltaTime);

            imageFadeColor.a = c;
            textFadeColor.a = c;

            terminationWarningPanel.color = imageFadeColor;
            terminationWarningText.color = textFadeColor;
        }

        this.gameObject.SetActive(false);
    }
}
