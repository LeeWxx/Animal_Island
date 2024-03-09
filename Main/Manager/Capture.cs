using System.Collections;
using UnityEngine;
using System.IO;

public class Capture : MonoBehaviour
{ 
    public void ClickShare()
    {
        StartCoroutine(TakeScreenShot());
    }

    private IEnumerator TakeScreenShot()
    {
        SoundManager.Instance.PlaySFXSound("ShutterSound");

        yield return new WaitForEndOfFrame();

        //스크린 캡쳐
        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();

        //스크린 캡쳐 저장 및 PNG로 변환
        string filePath = Path.Combine(Application.temporaryCachePath, "shared img.png");
        File.WriteAllBytes(filePath, ss.EncodeToPNG());

        // To avoid memory leaks
        Destroy(ss);

        //공유 기능
        new NativeShare().AddFile(filePath).SetSubject("Share test").SetText("Hello world!").Share();
    }
}
